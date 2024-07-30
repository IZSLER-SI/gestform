Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.ACL
Imports Softailor.Web.UI.DbForms

Public Class CtlSelettoreComuneItalia
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'Eventi
    'Public Event CodiceArticoloChanged(ByVal sender As Object, ByVal e As EventArgs)

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer


    'controlli
    'Dim hidac_COMUNE As TextBox     'controllo nascosto che contiene il codice. Usato per il databinding
    Dim ddnProvincia As DropDownList
    Dim ddnComune As DropDownList

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return ddnComune.SelectedValue
        End Get
        Set(ByVal value As String)
            HandleLists(value)
        End Set
    End Property

    Private Sub HandleLists(ac_COMUNE As String)

        'riempimento delle liste - diverso a seconda del fatto che si sia _enabled o meno

        'per iniziare, pulisco tutto come Chicco
        ddnProvincia.Items.Clear()
        ddnProvincia.Items.Add(New ListItem("", ""))
        ddnProvincia.SelectedValue = ""
        ddnComune.Items.Clear()
        ddnComune.Items.Add(New ListItem("", ""))
        ddnComune.SelectedValue = ""

        'se il campo è vuoto e il controllo non è abilitato, esco
        If Not _isEnabled And ac_COMUNE = "" Then Exit Sub

        'OK, dobbiamo gestire dei dati.
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim ac_PROVINCIA As String = ""

        dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
        dbConn.Open()

        If _isEnabled Then
            'COMPORTAMENTO DA CONTROLLO ATTIVO
            'riempimento e pre-selezione della lista delle province
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_SelettoreComuneItalia_ListaProvince"
                .Parameters.Add("@ac_comune", SqlDbType.NVarChar, 4).Value = ac_COMUNE
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    ddnProvincia.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                    If .GetBoolean(2) Then ac_PROVINCIA = .GetString(0)
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()
            'se ho una provincia, la seleziono e riempio i comuni!
            If ac_PROVINCIA <> String.Empty Then
                ddnProvincia.SelectedValue = ac_PROVINCIA
                dbCmd = dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "sp_age_SelettoreComuneItalia_ComuniProvincia"
                    .Parameters.Add("@ac_provincia", SqlDbType.NVarChar, 4).Value = ac_PROVINCIA
                End With
                dbRdr = dbCmd.ExecuteReader
                With dbRdr
                    Do While .Read
                        ddnComune.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                    Loop
                End With
                dbRdr.Close()
                dbCmd.Dispose()
                'e per finire, seleziono il comune!
                ddnComune.SelectedValue = ac_COMUNE
            End If


        Else
            'COMPORTAMENTO DA CONTROLLO NON ATTIVO (abbiamo qualcosa di selezionato
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_SelettoreComuneItalia_SingoloComune"
                .Parameters.Add("@ac_comune", SqlDbType.NVarChar, 4).Value = ac_COMUNE
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                If .Read Then
                    ddnProvincia.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                    ddnComune.Items.Add(New ListItem(.GetString(3), .GetString(2)))
                    ddnProvincia.SelectedValue = .GetString(0)
                    ddnComune.SelectedValue = .GetString(2)
                End If
            End With
            dbRdr.Close()
            dbCmd.Dispose()
        End If

        dbConn.Close()
        dbConn.Dispose()

    End Sub

    Public Property FieldName() As String Implements ICustomControl.FieldName
        Get
            Return _fieldName
        End Get
        Set(ByVal value As String)
            _fieldName = value
        End Set
    End Property

    Public Sub SetEnabled() Implements ICustomControl.SetEnabled

        'leggo il valore precedente
        Dim ac_COMUNE = ddnComune.SelectedValue
        _isEnabled = True
        HandleLists(ac_COMUNE)
        ddnComune.Enabled = True
        ddnProvincia.Enabled = True
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            ddnComune.ToolTip = "Seleziona un comune."
            ddnProvincia.ToolTip = ""
        Else
            ddnComune.ToolTip = errorText
            ddnProvincia.ToolTip = errorText
        End If
    End Sub

    Public Property MaxLength() As Integer Implements ICustomControl.MaxLength
        Get
            Return _maxLength
        End Get
        Set(ByVal value As Integer)
            _maxLength = value
        End Set
    End Property

    Public Sub SetIsOK() Implements ICustomControl.SetIsOK
        ddnComune.BackColor = Drawing.Color.Empty
        ddnProvincia.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        ddnComune.ToolTip = ""
        ddnProvincia.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        ddnComune.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            ddnComune.BackColor = Drawing.Color.Yellow
            ddnProvincia.BackColor = Drawing.Color.Empty
        Else
            ddnComune.BackColor = Drawing.Color.Yellow
            ddnProvincia.BackColor = Drawing.Color.Yellow
        End If
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        'leggo il valore precedente
        Dim ac_COMUNE = ddnComune.SelectedValue
        _isEnabled = False
        HandleLists(ac_COMUNE)
        ddnComune.Enabled = False
        ddnProvincia.Enabled = False
    End Sub

    Private Sub SelettoreComuneItalia_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        ddnProvincia = New DropDownList
        ddnProvincia.ID = "ddnProvincia"
        ddnProvincia.AutoPostBack = True
        ddnProvincia.CssClass = "ddn"
        ddnProvincia.Width = Unit.Parse("160px")

        ddnComune = New DropDownList
        ddnComune.ID = "ddnComune"
        ddnComune.AutoPostBack = False
        ddnComune.CssClass = "ddn"
        ddnComune.Width = Unit.Parse("323px")

        'aggiunta dei controlli
        Me.Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:51px"">Provincia</span>"))
        Me.Controls.Add(ddnProvincia)
        Me.Controls.Add(New LiteralControl("<span class=""slbl"" style=""width:81px"">Comune/Naz.</span>"))
        Me.Controls.Add(ddnComune)

        'aggancio eventi
        AddHandler ddnProvincia.SelectedIndexChanged, AddressOf ddnProvincia_SelectedIndexChanged

    End Sub

    Private Function FindParentStlFormView() As StlFormView

        Dim stlFormView As StlFormView = Nothing

        Dim curC As Control

        curC = Me

        Do While Not curC.Parent Is Nothing
            If TypeOf curC Is StlFormView Then
                stlFormView = CType(curC, StlFormView)
                Exit Do
            Else
                curC = curC.Parent
            End If
        Loop

        Return stlFormView

    End Function

    Public Sub New()
        _fieldName = ""
        _isEnabled = True
        _maxLength = 0
    End Sub


    Protected Sub ddnProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        'cambio di provincia!

        'svuoto il comune
        ddnComune.Items.Clear()
        ddnComune.Items.Add(New ListItem("", ""))
        ddnComune.SelectedValue = ""

        'riempio la lista
        If ddnProvincia.SelectedValue <> "" Then

            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_SelettoreComuneItalia_ComuniProvincia"
                .Parameters.Add("@ac_provincia", SqlDbType.NVarChar, 4).Value = ddnProvincia.SelectedValue
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    ddnComune.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()

            dbConn.Close()
            dbConn.Dispose()

        End If

    End Sub

    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid
        Return Not (ddnProvincia.SelectedValue <> String.Empty And ddnComune.SelectedValue = String.Empty)
    End Function
End Class
