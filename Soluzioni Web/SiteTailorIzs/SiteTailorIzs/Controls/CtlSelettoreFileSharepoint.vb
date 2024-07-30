Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.ACL
Imports Softailor.Web.UI.DbForms

Public Class CtlSelettoreFileSharepoint
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer
    Private _folderRelativeUrlTextBoxID As String
    Private _extensionList As String

    'controlli
    Dim hidac_TIPOFILE As TextBox       'controllo nascosto che contiene il tipo di file (Word o Excel)
    Dim txttx_FILE As TextBox           'campo file
    Dim imgCerca As Image               'link per cercare
    Dim lnkPickFile As LinkButton         'iink nascosto per la gestione della restituzione del valore dall'iframe

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return txttx_FILE.Text
        End Get
        Set(ByVal value As String)
            txttx_FILE.Text = value
            'impostazione della descrizione
            SetTipoFile(value)
        End Set
    End Property

    Public Property FolderRelativeUrlTextBoxID As String
        Get
            Return _folderRelativeUrlTextBoxID
        End Get
        Set(value As String)
            _folderRelativeUrlTextBoxID = value
        End Set
    End Property

    Public Property ExtensionList As String
        Get
            Return _extensionList
        End Get
        Set(value As String)
            _extensionList = value
        End Set
    End Property

    Public Property ac_TIPOFILE() As String
        Get
            Return hidac_TIPOFILE.Text
        End Get
        Set(ByVal value As String)
            hidac_TIPOFILE.Text = value
        End Set
    End Property

    Private Sub SetTipoFile(ByVal tx_FILE As String)
        If tx_FILE = "" Then
            hidac_TIPOFILE.Text = ""
        Else
            If Len(tx_FILE) > 5 Then
                Select Case Right(tx_FILE, 5)
                    Case ".xlsx"
                        hidac_TIPOFILE.Text = "Excel"
                    Case ".docx"
                        hidac_TIPOFILE.Text = "Word"
                    Case Else
                        hidac_TIPOFILE.Text = "N/D"
                End Select
            Else
                hidac_TIPOFILE.Text = ""
            End If
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
        txttx_FILE.Enabled = True

        imgCerca.ImageUrl = "~/img/icoLens.gif"
        imgCerca.ToolTip = "Seleziona"

        If imgCerca.Attributes("onclick") IsNot Nothing Then
            imgCerca.Attributes.Remove("onclick")
        End If
        imgCerca.Attributes.Add("onclick", Me.ClientID & "_Search();")

        If imgCerca.Style("cursor") IsNot Nothing Then
            imgCerca.Style.Remove("cursor")
        End If
        imgCerca.Style.Add("cursor", "pointer")

    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        txttx_FILE.ToolTip = errorText
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
        txttx_FILE.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        txttx_FILE.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        txttx_FILE.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        txttx_FILE.BackColor = Drawing.Color.Yellow
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        _isEnabled = False
        txttx_FILE.Enabled = False

        imgCerca.ImageUrl = "~/img/icoLensGrey.gif"
        imgCerca.ToolTip = ""

    End Sub

    Private Sub SelettoreArticolo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        Dim script As String

        hidac_TIPOFILE = New TextBox
        hidac_TIPOFILE.ID = "ac_TIPOFILE"
        hidac_TIPOFILE.Style.Add("display", "none")

        imgCerca = New Image
        imgCerca.BorderWidth = Unit.Parse("0px")
        imgCerca.Style.Add("vertical-align", "top")
        imgCerca.Style.Add("margin-left", "2px")
        imgCerca.Style.Add("margin-top", "2px")
        imgCerca.Style.Add("width", "16px")
        imgCerca.Style.Add("height", "16px")

        txttx_FILE = New TextBox
        txttx_FILE.ID = "txttx_FILE"
        txttx_FILE.CssClass = ""
        txttx_FILE.Attributes.Add("readonly", "readonly")
        txttx_FILE.TabIndex = 0
        txttx_FILE.CssClass = "txt"
        If Me.Width <> 0 Then
            txttx_FILE.Width = Unit.Pixel(CInt(Me.Width.Value - 26))
        End If

        lnkPickFile = New LinkButton
        lnkPickFile.ID = "lnkPickFile"
        lnkPickFile.Text = "x"
        lnkPickFile.Style.Add("display", "none")

        'aggiunta dei controlli
        Me.Controls.Add(hidac_TIPOFILE)
        Me.Controls.Add(txttx_FILE)
        Me.Controls.Add(imgCerca)
        Me.Controls.Add(lnkPickFile)

        Dim myExtList As String
        If _extensionList Is Nothing Then
            myExtList = ""
        Else
            myExtList = _extensionList
        End If

        'aggiunta funzione di ricerca, callback e di pulizia
        script = "function " & Me.ClientID & "_Callback(tx_FILE) {" & vbCrLf &
                 "if(tx_FILE!='')" & vbCrLf & _
                 "{" & vbCrLf &
                 "   $get(""" & txttx_FILE.ClientID & """).value=tx_FILE;" & vbCrLf &
                 "   " & Me.Page.ClientScript.GetPostBackEventReference(lnkPickFile, "") & "}" & vbCrLf &
                 "}" & vbCrLf &
                 "function " & Me.ClientID & "_Search() {" & vbCrLf &
                 "   stl_sel_display(""" & Page.ResolveClientUrl("~/Selettori/SelettoreFileSharepoint.aspx") &
                 "?ext=" & myExtList &
                 "&folder=" &
                 """ + encodeURI($get(""" & _folderRelativeUrlTextBoxID & """).value)" &
                 ", " & Me.ClientID & "_Callback);" &
                 "}" & vbCrLf

        Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, Me.ClientID & "_PopUpActions", script, True)

        'aggancio eventi
        AddHandler lnkPickFile.Click, AddressOf lnkPickFile_Click

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

    Protected Sub lnkPickFile_Click(ByVal sender As Object, ByVal e As EventArgs)
        'mi trovo l'ID nel txtCodice (valorizzato)
        SetTipoFile(txttx_FILE.Text)
    End Sub

    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid
        Return True
    End Function

End Class
