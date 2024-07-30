Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.ACL
Imports Softailor.Web.UI.DbForms

Public Class CtlSelettoreRuoloProfilo
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer

    'controlli
    Dim ddnRuolo As DropDownList
    Dim ddnProfilo As DropDownList

    Public Property ac_RUOLO() As String
        Get
            Return ddnRuolo.SelectedValue
        End Get
        Set(value As String)
            HandleLists(value, ddnProfilo.SelectedValue)
        End Set
    End Property

    Public Property ac_PROFILO() As String
        Get
            Return ddnProfilo.SelectedValue
        End Get
        Set(value As String)
            HandleLists(ddnRuolo.SelectedValue, value)
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

    Private Sub HandleLists(ac_RUOLO As String, ac_PROFILO As String)

        'riempimento delle liste - diverso a seconda del fatto che si sia _enabled o meno

        'per iniziare, pulisco tutto come Chicco
        ddnRuolo.Items.Clear()
        ddnRuolo.Items.Add(New ListItem("", ""))
        ddnRuolo.SelectedValue = ""
        ddnProfilo.Items.Clear()
        ddnProfilo.Items.Add(New ListItem("", ""))
        ddnProfilo.SelectedValue = ""

        'se il campo è vuoto e il controllo non è abilitato, esco
        If Not _isEnabled And ac_RUOLO = "" Then Exit Sub

        'OK, dobbiamo gestire dei dati.
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
        dbConn.Open()

        If _isEnabled Then
            'COMPORTAMENTO DA CONTROLLO ATTIVO
            'riempimento e pre-selezione della lista dei ruoli
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_SelettoreRuoliProfili_ListaRuoli"
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    ddnRuolo.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()
            'se ho un ruolo, lo seleziono e riempio i profili
            If ac_RUOLO <> String.Empty Then
                ddnRuolo.SelectedValue = ac_RUOLO
                dbCmd = dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "sp_age_SelettoreRuoliProfili_ProfiliRuolo"
                    .Parameters.Add("@ac_ruolo", SqlDbType.NVarChar, 4).Value = ac_RUOLO
                End With
                dbRdr = dbCmd.ExecuteReader
                With dbRdr
                    Do While .Read
                        ddnProfilo.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                    Loop
                End With
                dbRdr.Close()
                dbCmd.Dispose()
                'e per finire, seleziono la disciplina (che potrebbe essere stringa vuota)
                ddnProfilo.SelectedValue = ac_PROFILO
            End If
        Else
            'COMPORTAMENTO DA CONTROLLO NON ATTIVO (abbiamo qualcosa di selezionato)
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_SelettoreRuoliProfili_SingoloRuoloProfilo"
                .Parameters.Add("@ac_ruolo", SqlDbType.NVarChar, 4).Value = ac_RUOLO
                With .Parameters.Add("@ac_profilo", SqlDbType.NVarChar, 20)
                    If ac_PROFILO = "" Then
                        .Value = DBNull.Value
                    Else
                        .Value = ac_PROFILO
                    End If
                End With
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                If .Read Then
                    ddnRuolo.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                    ddnRuolo.SelectedIndex = 1
                    If ac_PROFILO <> "" Then
                        ddnProfilo.Items.Add(New ListItem(.GetString(3), .GetString(2)))
                        ddnProfilo.SelectedIndex = 1
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
        Dim ac_RUOLO = ddnRuolo.SelectedValue
        Dim ac_PROFILO = ddnProfilo.SelectedValue
        _isEnabled = True
        HandleLists(ac_RUOLO, ac_PROFILO)
        ddnRuolo.Enabled = True
        ddnProfilo.Enabled = True
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            ddnRuolo.ToolTip = "Seleziona un ruolo."
            ddnProfilo.ToolTip = ""
        Else
            ddnRuolo.ToolTip = errorText
            ddnProfilo.ToolTip = errorText
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
        ddnRuolo.BackColor = Drawing.Color.Empty
        ddnProfilo.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        ddnRuolo.ToolTip = ""
        ddnProfilo.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        ddnRuolo.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            ddnRuolo.BackColor = Drawing.Color.Yellow
            ddnProfilo.BackColor = Drawing.Color.Empty
        Else
            ddnRuolo.BackColor = Drawing.Color.Yellow
            ddnProfilo.BackColor = Drawing.Color.Yellow
        End If
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        'leggo il valore precedente
        Dim ac_RUOLO = ddnRuolo.SelectedValue
        Dim ac_PROFILO = ddnProfilo.SelectedValue
        _isEnabled = False
        HandleLists(ac_RUOLO, ac_PROFILO)
        ddnRuolo.Enabled = False
        ddnProfilo.Enabled = False
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        ddnRuolo = New DropDownList
        ddnRuolo.ID = "ddnRuolo"
        ddnRuolo.AutoPostBack = True
        ddnRuolo.CssClass = "ddn"
        ddnRuolo.Width = Unit.Parse("152px")

        ddnProfilo = New DropDownList
        ddnProfilo.ID = "ddnProfilo"
        ddnProfilo.AutoPostBack = False
        ddnProfilo.CssClass = "ddn"
        ddnProfilo.Width = Unit.Parse("329px")

        'aggiunta dei controlli
        Me.Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Ruolo</span>"))
        Me.Controls.Add(ddnRuolo)
        Me.Controls.Add(New LiteralControl("<span class=""slbl"" style=""width:44px"">Profilo</span>"))
        Me.Controls.Add(ddnProfilo)

        'aggancio eventi
        AddHandler ddnRuolo.SelectedIndexChanged, AddressOf ddnRuolo_SelectedIndexChanged

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

    Protected Sub ddnRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        'cambio di ruolo!

        'svuoto i profili
        ddnProfilo.Items.Clear()
        ddnProfilo.Items.Add(New ListItem("", ""))
        ddnProfilo.SelectedValue = ""

        'riempio la lista
        If ddnRuolo.SelectedValue <> "" Then

            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_SelettoreRuoliProfili_ProfiliRuolo"
                .Parameters.Add("@ac_ruolo", SqlDbType.NVarChar, 4).Value = ddnRuolo.SelectedValue
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    ddnProfilo.Items.Add(New ListItem(.GetString(1), .GetString(0)))
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()

            dbConn.Close()
            dbConn.Dispose()

        End If

    End Sub

    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid
        Return Not (ddnProfilo.SelectedValue <> String.Empty And ddnRuolo.SelectedValue = String.Empty)
    End Function

End Class
