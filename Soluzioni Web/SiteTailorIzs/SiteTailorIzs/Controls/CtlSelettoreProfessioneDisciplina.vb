Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.ACL
Imports Softailor.Web.UI.DbForms

Public Class CtlSelettoreProfessioneDisciplina
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer

    'controlli
    Dim ddnProfessione As DropDownList
    Dim ddnDisciplina As DropDownList

    Public Property ac_PROFESSIONE() As String
        Get
            Return ddnProfessione.SelectedValue
        End Get
        Set(value As String)
            HandleLists(value, ddnDisciplina.SelectedValue)
        End Set
    End Property

    Public Property id_DISCIPLINA() As String
        Get
            Return ddnDisciplina.SelectedValue
        End Get
        Set(value As String)
            HandleLists(ddnProfessione.SelectedValue, value)
        End Set
    End Property


    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return ""
        End Get
        Set(ByVal value As String)
            'do nothing
        End Set
    End Property

    Private Sub HandleLists(ac_PROFESSIONE As String, id_DISCIPLINA As String)

        'riempimento delle liste - diverso a seconda del fatto che si sia _enabled o meno

        'per iniziare, pulisco tutto come Chicco
        ddnProfessione.Items.Clear()
        ddnProfessione.Items.Add(New ListItem("", ""))
        ddnProfessione.SelectedValue = ""
        ddnDisciplina.Items.Clear()
        ddnDisciplina.Items.Add(New ListItem("", ""))
        ddnDisciplina.SelectedValue = ""

        'se il campo è vuoto e il controllo non è abilitato, esco
        If Not _isEnabled And ac_PROFESSIONE = "" Then Exit Sub

        'OK, dobbiamo gestire dei dati.
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
        dbConn.Open()

        If _isEnabled Then
            'COMPORTAMENTO DA CONTROLLO ATTIVO
            'riempimento e pre-selezione della lista delle professioni
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ecm_SelettoreProfessioniDiscipline_ListaProfessioni"
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    ddnProfessione.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()
            'se ho una professione, la seleziono e riempio i comuni!
            If ac_PROFESSIONE <> String.Empty Then
                ddnProfessione.SelectedValue = ac_PROFESSIONE
                dbCmd = dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "sp_ecm_SelettoreProfessioneDisciplina_DisciplineProfessione"
                    .Parameters.Add("@ac_professione", SqlDbType.NVarChar, 10).Value = ac_PROFESSIONE
                End With
                dbRdr = dbCmd.ExecuteReader
                With dbRdr
                    Do While .Read
                        ddnDisciplina.Items.Add(New ListItem(.GetString(1), .GetInt32(0).ToString))
                    Loop
                End With
                dbRdr.Close()
                dbCmd.Dispose()
                'e per finire, seleziono la disciplina (che potrebbe essere stringa vuota)
                ddnDisciplina.SelectedValue = id_DISCIPLINA
            End If
        Else
            'COMPORTAMENTO DA CONTROLLO NON ATTIVO (abbiamo qualcosa di selezionato)
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ecm_SelettoreProfessioneDisciplina_SingolaProfessioneDisciplina"
                .Parameters.Add("@ac_professione", SqlDbType.NVarChar, 10).Value = ac_PROFESSIONE
                With .Parameters.Add("@id_disciplina", SqlDbType.Int)
                    If id_DISCIPLINA = "" Then
                        .Value = DBNull.Value
                    Else
                        .Value = CInt(id_DISCIPLINA)
                    End If
                End With
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                If .Read Then
                    ddnProfessione.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                    ddnProfessione.SelectedIndex = 1
                    If id_DISCIPLINA <> "" Then
                        ddnDisciplina.Items.Add(New ListItem(.GetString(3), .GetInt32(2).ToString))
                        ddnDisciplina.SelectedIndex = 1
                    End If
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
        Dim ac_PROFESSIONE = ddnProfessione.SelectedValue
        Dim id_DISCIPLINA = ddnDisciplina.SelectedValue
        _isEnabled = True
        HandleLists(ac_PROFESSIONE, id_DISCIPLINA)
        ddnProfessione.Enabled = True
        ddnDisciplina.Enabled = True
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            ddnProfessione.ToolTip = "Seleziona una professione."
            ddnDisciplina.ToolTip = ""
        Else
            ddnProfessione.ToolTip = errorText
            ddnDisciplina.ToolTip = errorText
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
        ddnProfessione.BackColor = Drawing.Color.Empty
        ddnDisciplina.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        ddnProfessione.ToolTip = ""
        ddnDisciplina.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        ddnProfessione.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            ddnProfessione.BackColor = Drawing.Color.Yellow
            ddnDisciplina.BackColor = Drawing.Color.Empty
        Else
            ddnProfessione.BackColor = Drawing.Color.Yellow
            ddnDisciplina.BackColor = Drawing.Color.Yellow
        End If
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        'leggo il valore precedente
        Dim ac_PROFESSIONE = ddnProfessione.SelectedValue
        Dim id_DISCIPLINA = ddnDisciplina.SelectedValue
        _isEnabled = False
        HandleLists(ac_PROFESSIONE, id_DISCIPLINA)
        ddnProfessione.Enabled = False
        ddnDisciplina.Enabled = False
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        ddnProfessione = New DropDownList
        ddnProfessione.ID = "ddnProfessione"
        ddnProfessione.AutoPostBack = True
        ddnProfessione.CssClass = "ddn"
        ddnProfessione.Width = Unit.Parse("200px")

        ddnDisciplina = New DropDownList
        ddnDisciplina.ID = "ddnDisciplina"
        ddnDisciplina.AutoPostBack = False
        ddnDisciplina.CssClass = "ddn"
        ddnDisciplina.Width = Unit.Parse("479px")

        'aggiunta dei controlli
        Me.Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Professione ECM</span>"))
        Me.Controls.Add(ddnProfessione)
        Me.Controls.Add(New LiteralControl("<span class=""slbl"" style=""width:65px"">Disciplina</span>"))
        Me.Controls.Add(ddnDisciplina)

        'aggancio eventi
        AddHandler ddnProfessione.SelectedIndexChanged, AddressOf ddnProfessione_SelectedIndexChanged

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

    Protected Sub ddnProfessione_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        'cambio di professione!

        'svuoto le discipline
        ddnDisciplina.Items.Clear()
        ddnDisciplina.Items.Add(New ListItem("", ""))
        ddnDisciplina.SelectedValue = ""

        'riempio la lista
        If ddnProfessione.SelectedValue <> "" Then

            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ecm_SelettoreProfessioneDisciplina_DisciplineProfessione"
                .Parameters.Add("@ac_professione", SqlDbType.NVarChar, 10).Value = ddnProfessione.SelectedValue
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    ddnDisciplina.Items.Add(New ListItem(.GetString(1), .GetInt32(0).ToString))
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()

            dbConn.Close()
            dbConn.Dispose()

        End If

    End Sub

    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid
        Return Not (ddnDisciplina.SelectedValue <> String.Empty And ddnProfessione.SelectedValue = String.Empty)
    End Function

End Class
