Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Web.UI

Public Class StlSqlDataSource
    Inherits SqlDataSource

    Public Enum WorkingModes As Integer
        NotBound = 0
        BoundToForm = 1
        BoundToGrid = 2
    End Enum

    'variabili per la memorizzazione dei metadati
    Private _metaDataRead As Boolean = False
    Private _stringFieldsMaxLens As Dictionary(Of String, Integer)
    Private _requiredFieldNames As List(Of String)
    Private _fieldTypes As Dictionary(Of String, String)
    Private _hasIdentity As Boolean
    Private _identityFieldName As String

    'oggetti collegati
    Private _ro_boundStlGridView As StlGridView
    Private _ro_boundStlGridViewSearched As Boolean = False
    Private _ro_boundStlFormView As StlFormView
    Private _ro_boundStlFormViewSearched As Boolean = False

    Private Sub ReadMetaData()
        If _metaDataRead = False Then
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim myPrm As System.Web.UI.WebControls.Parameter
            Dim dbPrm As SqlParameter
            Dim dbRdr As SqlDataReader
            Dim schema As DataTable
            Dim row As DataRow
            Dim rowType As System.Type
            Dim rowLen As Integer

            dbConn = New SqlConnection(Me.ConnectionString)
            dbConn.Open()
            dbCmd = dbConn.CreateCommand
            With dbCmd
                Select Case Me.SelectCommandType
                    Case SqlDataSourceCommandType.StoredProcedure
                        .CommandType = CommandType.StoredProcedure
                    Case SqlDataSourceCommandType.Text
                        .CommandType = CommandType.Text
                End Select
                .CommandText = Me.SelectCommand
                'aggiunta dei parametri
                For Each myPrm In Me.SelectParameters
                    dbPrm = New SqlParameter()
                    dbPrm.ParameterName = myPrm.Name
                    Select Case myPrm.Type
                        Case TypeCode.Boolean
                            dbPrm.SqlDbType = SqlDbType.Bit
                        Case TypeCode.Byte
                            dbPrm.SqlDbType = SqlDbType.TinyInt
                        Case TypeCode.DateTime
                            dbPrm.SqlDbType = SqlDbType.DateTime
                        Case TypeCode.Decimal
                            dbPrm.SqlDbType = SqlDbType.Money
                        Case TypeCode.Int16
                            dbPrm.SqlDbType = SqlDbType.SmallInt
                        Case TypeCode.Int32
                            dbPrm.SqlDbType = SqlDbType.Int
                        Case TypeCode.Int64
                            dbPrm.SqlDbType = SqlDbType.BigInt
                        Case TypeCode.String
                            dbPrm.SqlDbType = SqlDbType.NVarChar
                            dbPrm.Size = myPrm.Size
                    End Select
                    dbPrm.Value = DBNull.Value
                    dbCmd.Parameters.Add(dbPrm)
                Next
            End With

            'OK ho il command con tutti i valori dei parametri eventuali pari a null

            'eseguo!

            'lettura dello schema
            dbRdr = dbCmd.ExecuteReader(CommandBehavior.SchemaOnly)
            schema = dbRdr.GetSchemaTable
            dbRdr.Close()

            'chiusura della connessione
            dbConn.Close()

            'generazione di _stringMaxLens, _hasIdentity, _identityFieldName, requiredFieldNames
            _stringFieldsMaxLens = New Dictionary(Of String, Integer)
            _requiredFieldNames = New List(Of String)
            _fieldTypes = New Dictionary(Of String, String)
            _hasIdentity = False
            _identityFieldName = ""

            For Each row In schema.Rows

                '_hasIdentity e _identityFieldName
                If CType(row("IsIdentity"), Boolean) Then
                    _hasIdentity = True
                    _identityFieldName = CType(row(0), String).ToUpper
                End If

                '_requiredfieldnames
                If CType(row("IsIdentity"), Boolean) = False Then
                    If CType(row("AllowDbNull"), Boolean) = False Then
                        _requiredFieldNames.Add(CType(row(0), String).ToUpper)
                    End If
                End If

                '_stringMaxLens e _fieldTypes
                rowType = CType(row("DataType"), System.Type)
                _fieldTypes.Add(CType(row(0), String).ToUpper, CType(row("DataType"), System.Type).Name)
                If rowType.Name = GetType(System.String).Name Then
                    rowLen = CType(row(2), Integer)
                    If rowLen > 0 And rowLen < 100000 Then
                        _stringFieldsMaxLens.Add(CType(row(0), String).ToUpper, rowLen)
                    End If
                End If
            Next
            schema.Dispose()




            _metaDataRead = True
        End If
    End Sub

    Public ReadOnly Property StringFieldsMaxLens() As Dictionary(Of String, Integer)
        Get
            'leggo i metadati se non li ho già letti
            ReadMetaData()
            Return _stringFieldsMaxLens
        End Get
    End Property

    Public ReadOnly Property FieldTypes() As Dictionary(Of String, String)
        Get
            'leggo i metadati se non li ho già letti
            ReadMetaData()
            Return _fieldTypes
        End Get
    End Property

    Public ReadOnly Property RequiredFieldNames() As List(Of String)
        Get
            'leggo i metadati se non li ho già letti
            ReadMetaData()
            Return _requiredFieldNames
        End Get
    End Property

    Public ReadOnly Property WorkingMode() As WorkingModes
        Get
            Dim outWorkingMode As WorkingModes = WorkingModes.NotBound
            'determinazione del WorkingMode
            With Me
                If Not Me.BoundStlGridView Is Nothing Then
                    outWorkingMode = WorkingModes.BoundToGrid
                Else
                    If Not Me.BoundStlFormView Is Nothing Then
                        outWorkingMode = WorkingModes.BoundToForm
                    End If
                End If
            End With
            Return outWorkingMode
        End Get
    End Property

    Public ReadOnly Property BoundStlGridView() As StlGridView
        Get
            If Not _ro_boundStlGridViewSearched Then
                _ro_boundStlGridView = FindBoundStlGridView(Me.Page, Me.ID)
                _ro_boundStlGridViewSearched = True
            End If
            Return _ro_boundStlGridView
        End Get
    End Property

    Public ReadOnly Property BoundStlFormView() As StlFormView
        Get
            If Not _ro_boundStlFormViewSearched Then
                _ro_boundStlFormView = FindBoundStlFormView(Me.Page, Me.ID)
                _ro_boundStlFormViewSearched = True
            End If
            Return _ro_boundStlFormView
        End Get
    End Property

    Private Sub StlSqlDataSource_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles Me.Inserted

        'se ho un eccezione non faccio nulla!
        If Not e.Exception Is Nothing Then Exit Sub

        Dim newID As Object
        Dim dataKeyName As String

        'dopo l'inserimento devo riposizionare la griglia sul nuovo record
        'vale solo se sono boundToForm

        If Me.WorkingMode = WorkingModes.BoundToForm Then

            'lettura struttura (se non è stata letta)
            ReadMetaData()

            'determino il nome del campo chiave: se ho autoincrement, _identityFieldName; altrimenti il dataKeyNames(0) del StlFormView
            If _hasIdentity Then
                dataKeyName = _identityFieldName
            Else
                dataKeyName = Me.BoundStlFormView.DataKeyNames(0)
            End If

            'leggo il valore della nuova chiave
            newID = e.Command.Parameters("@" & dataKeyName).Value

            Dim formGrid As StlGridView = Me.BoundStlFormView.BoundStlGridView()
            'posiziono la griglia sulla riga
            If Not formGrid Is Nothing Then
                formGrid.PreGotoRow(dataKeyName, newID.ToString, True, True)
                formGrid.DataBind()
                formGrid.PostGotoRow()
                Dim gridUpd As UpdatePanel = formGrid.ContainingUpdatePanel
                'aggiorno l'updatepanel della griglia
                If Not gridUpd Is Nothing Then
                    gridUpd.Update()
                    formGrid.EnsureSelectedRowVisible()
                End If
            End If

            'riposiziono me stesso sulla riga
            SelectParameters(dataKeyName).DefaultValue = newID.ToString

        End If

    End Sub


#Region "Ricerca oggetti collegati"

    Private Function FindBoundStlGridView(ByVal rootControl As Control, ByVal dataSourceID As String) As StlGridView

        Dim stlGW As StlGridView
        Dim c As Control
        Dim foundGW As StlGridView

        If TypeOf rootControl Is StlGridView Then
            stlGW = CType(rootControl, StlGridView)
            If stlGW.DataSourceID = dataSourceID Then
                Return stlGW
            End If
        End If

        For Each c In rootControl.Controls
            foundGW = FindBoundStlGridView(c, dataSourceID)
            If Not foundGW Is Nothing Then Return foundGW
        Next

        Return Nothing

    End Function

    Private Function FindBoundStlFormView(ByVal rootControl As Control, ByVal dataSourceID As String) As StlFormView

        Dim stlFW As StlFormView
        Dim c As Control
        Dim foundFW As StlFormView

        If TypeOf rootControl Is StlFormView Then
            stlFW = CType(rootControl, StlFormView)
            If stlFW.DataSourceID = dataSourceID Then
                Return stlFW
            End If
        End If

        For Each c In rootControl.Controls
            foundFW = FindBoundStlFormView(c, dataSourceID)
            If Not foundFW Is Nothing Then Return foundFW
        Next

        Return Nothing

    End Function
#End Region

    Private Sub StlSqlDataSource_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles Me.Selecting
        'prendo il command dal provider...
        If Me.WorkingMode = WorkingModes.BoundToGrid Then
            If BoundStlGridView.SqlStringProviderID <> "" Then
                e.Command.CommandText = BoundStlGridView.SqlStringProvider.GetSql
            End If
        End If
    End Sub

    Public ReadOnly Property HasIdentity() As Boolean
        Get
            'lettura struttura (se non è stata letta)
            ReadMetaData()
            Return _hasIdentity
        End Get
    End Property

End Class
