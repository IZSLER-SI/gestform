Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.ACL
Imports Softailor.Web.UI.DbForms

Public Class CtlSelettorePersonaGForm
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer
    Private _soloDipendenti As Boolean

    'controlli
    Dim hidid_PERSONA As TextBox    'controllo nascosto che contiene l'ID articolo. Usato per il databinding
    Dim imgCerca As Image               'link per cercare
    Dim imgCancella As Image            'link per cancellare
    Dim lnkPickID As LinkButton         'iink nascosto per la gestione della restituzione del valore dall'iframe
    Dim txtDescrizione As TextBox        'campo descrizione

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return hidid_PERSONA.Text
        End Get
        Set(ByVal value As String)
            hidid_PERSONA.Text = value
            'impostazione della descrizione
            SetPersona(value)
        End Set
    End Property

    Public Property SoloDipendenti() As Boolean
        Get
            Return _soloDipendenti
        End Get
        Set(value As Boolean)
            _soloDipendenti = value
        End Set
    End Property

    Private Sub SetPersona(ByVal sID_PERSONA As String)
        If sID_PERSONA = "" Then
            txtDescrizione.Text = ""
        Else
            'ricerca e compilazione
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_PERSONE_desc"
                .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = CInt(sID_PERSONA)
            End With
            dbRdr = dbCmd.ExecuteReader()
            dbRdr.Read()

            txtDescrizione.Text = dbRdr.GetString(0)

            dbRdr.Close()
            dbCmd.Dispose()
            dbConn.Close()
        End If
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
        _isEnabled = True
        txtDescrizione.Enabled = True

        imgCerca.ImageUrl = "~/img/icoLens.gif"
        imgCancella.ImageUrl = "~/img/icoDelete.gif"
        imgCerca.ToolTip = "Cerca"
        imgCancella.ToolTip = "Cancella"

        If imgCerca.Attributes("onclick") IsNot Nothing Then
            imgCerca.Attributes.Remove("onclick")
        End If
        imgCerca.Attributes.Add("onclick", Me.ClientID & "_Search();")

        If imgCerca.Style("cursor") IsNot Nothing Then
            imgCerca.Style.Remove("cursor")
        End If
        imgCerca.Style.Add("cursor", "pointer")



        If imgCancella.Attributes("onclick") IsNot Nothing Then
            imgCancella.Attributes.Remove("onclick")
        End If
        imgCancella.Attributes.Add("onclick", Me.ClientID & "_Clear();")

        If imgCancella.Style("cursor") IsNot Nothing Then
            imgCancella.Style.Remove("cursor")
        End If
        imgCancella.Style.Add("cursor", "pointer")

    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        txtDescrizione.ToolTip = errorText
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
        txtDescrizione.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        txtDescrizione.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        txtDescrizione.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        txtDescrizione.BackColor = Drawing.Color.Yellow
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        _isEnabled = False
        txtDescrizione.Enabled = False

        imgCerca.ImageUrl = "~/img/icoLensGrey.gif"
        imgCancella.ImageUrl = "~/img/icoDeleteGrey.gif"
        imgCerca.ToolTip = ""
        imgCancella.ToolTip = ""

        If imgCancella.Attributes("onclick") IsNot Nothing Then
            imgCancella.Attributes.Remove("onclick")
        End If
        If imgCancella.Style("cursor") IsNot Nothing Then
            imgCancella.Style.Remove("cursor")
        End If
        If imgCancella.Attributes("onclick") IsNot Nothing Then
            imgCancella.Attributes.Remove("onclick")
        End If
        If imgCancella.Style("cursor") IsNot Nothing Then
            imgCancella.Style.Remove("cursor")
        End If

    End Sub

    Private Sub SelettoreArticolo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        Dim script As String

        hidid_PERSONA = New TextBox
        hidid_PERSONA.ID = "id_PERSONA"
        hidid_PERSONA.Style.Add("display", "none")

        imgCerca = New Image
        imgCerca.BorderWidth = Unit.Parse("0px")
        imgCerca.Style.Add("vertical-align", "top")
        imgCerca.Style.Add("margin-left", "2px")
        imgCerca.Style.Add("margin-top", "2px")
        imgCerca.Style.Add("width", "16px")
        imgCerca.Style.Add("height", "16px")

        imgCancella = New Image
        imgCancella.BorderWidth = Unit.Parse("0px")
        imgCancella.Style.Add("vertical-align", "top")
        imgCancella.Style.Add("margin-left", "2px")
        imgCancella.Style.Add("margin-top", "2px")
        imgCancella.Style.Add("width", "16px")
        imgCancella.Style.Add("height", "16px")

        txtDescrizione = New TextBox
        txtDescrizione.ID = "txtDescrizione"
        txtDescrizione.CssClass = ""
        txtDescrizione.Attributes.Add("readonly", "readonly")
        'txtDescrizione.ReadOnly = True
        txtDescrizione.TabIndex = 0
        txtDescrizione.CssClass = "txt"
        If Me.Width <> 0 Then
            txtDescrizione.Width = Unit.Pixel(CInt(Me.Width.Value - 42))
        End If

        lnkPickID = New LinkButton
        lnkPickID.ID = "lnkPick"
        lnkPickID.Text = "x"
        lnkPickID.Style.Add("display", "none")

        'aggiunta dei controlli
        Me.Controls.Add(hidid_PERSONA)
        Me.Controls.Add(txtDescrizione)
        Me.Controls.Add(imgCerca)
        Me.Controls.Add(imgCancella)
        Me.Controls.Add(lnkPickID)

        'aggiunta funzione di ricerca, callback e di pulizia
        script = "function " & Me.ClientID & "_Callback(id_PERSONA) {" & vbCrLf &
                 "if(id_PERSONA!='')" & vbCrLf & _
                 "{" & vbCrLf &
                 "   $get(""" & hidid_PERSONA.ClientID & """).value=id_PERSONA;" & vbCrLf &
                 "   " & Me.Page.ClientScript.GetPostBackEventReference(lnkPickID, "") & "}" & vbCrLf &
                 "}" & vbCrLf &
                 "function " & Me.ClientID & "_Clear() {" & vbCrLf &
                 "   $get(""" & hidid_PERSONA.ClientID & """).value="""";" & vbCrLf &
                 "   $get(""" & txtDescrizione.ClientID & """).value="""";" & vbCrLf &
                 "}" & vbCrLf &
                 "function " & Me.ClientID & "_Search() {" & vbCrLf &
                 "   stl_sel_display(""" & Page.ResolveClientUrl("~/Selettori/SelettorePersonaGForm.aspx?diponly=") &
                 If(_soloDipendenti, "1", "0") & """, " & Me.ClientID & "_Callback);" &
                 "}" & vbCrLf

        Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, Me.ClientID & "_PopUpActions", script, True)

        'aggancio eventi
        AddHandler lnkPickID.Click, AddressOf lnkPickId_Click

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

    Protected Sub lnkPickId_Click(ByVal sender As Object, ByVal e As EventArgs)
        'mi trovo l'ID nel txtCodice (valorizzato)
        SetPersona(hidid_PERSONA.Text)
    End Sub

    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid
        Return True
    End Function

End Class
