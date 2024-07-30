Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.Binaries
Imports Softailor.SiteTailor.ACL

Public Class BinaryElementBox
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer
    Private _defaultCODCATEG As String
    Private _defaultDescriptionTextBoxID As String
    Private _defaultDescriptionPreamble As String
    Private _defaultDescriptionPostamble As String

    'controlli
    Dim ID_ELEME As HiddenField 'dato
    Dim lnkElement As HyperLink
    Dim lblDESEL_TX As Label
    Dim lblDESCATEG As Label
    Dim lblDESFORMA As Label
    Dim lnkNuovo As HyperLink
    Dim lnkSeleziona As HyperLink
    Dim lnkRimuovi As HyperLink
    Dim hidDefaultCODCATEG As HiddenField
    Dim hidDefaultDescriptionPreamble As HiddenField
    Dim hidDefaultDescriptionSourceTextBoxClientID As HiddenField
    Dim hidDefaultDescriptionPostamble As HiddenField
    Dim imgError As Image
    Dim lnkRefresh As LinkButton

    Public Property DefaultCODCATEG() As String
        Get
            Return _defaultCODCATEG
        End Get
        Set(ByVal value As String)
            _defaultCODCATEG = value
        End Set
    End Property

    Public Property DefaultDescriptionPreamble() As String
        Get
            Return _defaultDescriptionPreamble
        End Get
        Set(ByVal value As String)
            _defaultDescriptionPreamble = value
        End Set
    End Property

    Public Property DefaultDescriptionSourceTextBoxID() As String
        Get
            Return _defaultDescriptionTextBoxID
        End Get
        Set(ByVal value As String)
            _defaultDescriptionTextBoxID = value
        End Set
    End Property

    Public Property DefaultDescriptionPostamble() As String
        Get
            Return _defaultDescriptionPostamble
        End Get
        Set(ByVal value As String)
            _defaultDescriptionPostamble = value
        End Set
    End Property

    Private Sub BinaryElementBox_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        GenerazioneControlli()

    End Sub



    Public Property FieldName() As String Implements ICustomControl.FieldName
        Get
            Return _fieldName
        End Get
        Set(ByVal value As String)
            _fieldName = value
        End Set
    End Property

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Property MaxLength() As Integer Implements ICustomControl.MaxLength
        Get
            Return _maxLength
        End Get
        Set(ByVal value As Integer)
            _maxLength = value
        End Set
    End Property

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        _isEnabled = False
        lnkNuovo.Enabled = False
        lnkSeleziona.Enabled = False
        lnkRimuovi.Enabled = False
        lnkRefresh.Enabled = False
    End Sub

    Public Sub SetEnabled() Implements ICustomControl.SetEnabled
        _isEnabled = True
        lnkNuovo.Enabled = True
        lnkSeleziona.Enabled = True
        lnkRimuovi.Enabled = True
        lnkRefresh.Enabled = True
    End Sub

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        imgError.AlternateText = ""
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        imgError.AlternateText = errorText
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        lnkNuovo.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        imgError.Visible = True
    End Sub

    Public Sub SetIsOK() Implements ICustomControl.SetIsOK
        imgError.Visible = False
    End Sub

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return ID_ELEME.Value
        End Get
        Set(ByVal value As String)
            ID_ELEME.Value = value
        End Set
    End Property

    Public Sub New()
        _fieldName = ""
        _isEnabled = True
        _maxLength = 0
        _defaultDescriptionTextBoxID = ""
    End Sub

    Public Sub GenerazioneControlli()

        Dim baseClientId As String
        Dim clientIdSeparator As String
        Dim refreshJsClientId As String
        Dim binariesBaseUrl As String
        Dim formBaseClientId As String

        'istanzio e attribuisco gli ID
        ID_ELEME = New HiddenField
        ID_ELEME.ID = "ID_ELEME"

        lnkElement = New HyperLink
        lnkElement.ID = "lnkElement"
        'lnkElement.Target = "_blank"
        lnkElement.CssClass = "beb_img"

        lblDESEL_TX = New Label
        lblDESEL_TX.ID = "lblDESEL_TX"
        lblDESEL_TX.CssClass = "beb_dsc"

        lblDESFORMA = New Label
        lblDESFORMA.ID = "lblDESFORMA"
        lblDESFORMA.CssClass = "beb_fmt"

        lblDESCATEG = New Label
        lblDESCATEG.ID = "lblDESCATEG"
        lblDESCATEG.CssClass = "beb_cat"

        lnkNuovo = New HyperLink
        lnkNuovo.ID = "lnkNuovo"
        lnkNuovo.Text = "Nuovo"
        lnkNuovo.CssClass = "beb_a"

        lnkSeleziona = New HyperLink
        lnkSeleziona.ID = "lnkSeleziona"
        lnkSeleziona.Text = "Scegli"
        lnkSeleziona.CssClass = "beb_a"

        lnkRimuovi = New HyperLink
        lnkRimuovi.ID = "lnkRimuovi"
        lnkRimuovi.NavigateUrl = "#"
        lnkRimuovi.Text = "Rimuovi"
        lnkRimuovi.CssClass = "beb_al"

        hidDefaultCODCATEG = New HiddenField
        hidDefaultCODCATEG.ID = "hidDefaultCODCATEG"

        hidDefaultDescriptionPreamble = New HiddenField
        hidDefaultDescriptionPreamble.ID = "hidDefaultDescriptionPreamble"

        hidDefaultDescriptionSourceTextBoxClientID = New HiddenField
        hidDefaultDescriptionSourceTextBoxClientID.ID = "hidDefaultDescriptionSourceTextBoxClientID"

        hidDefaultDescriptionPostamble = New HiddenField
        hidDefaultDescriptionPostamble.ID = "hidDefaultDescriptionPostamble"

        imgError = New Image
        imgError.ID = "imgError"
        imgError.ImageUrl = Page.ResolveUrl("~/img/icoExcl.gif")
        imgError.AlternateText = ""
        imgError.CssClass = "beb_err"
        imgError.Visible = False

        lnkRefresh = New LinkButton
        lnkRefresh.ID = "lnkRefresh"
        lnkRefresh.Text = "Refresh"
        lnkRefresh.Style.Add("display", "none")

        'aggiunta dei controlli (e classi CSS)
        Me.Controls.Add(New LiteralControl("<table class=""beb_tbl""><tr>"))

        Me.Controls.Add(New LiteralControl("<td class=""beb_tdi"">"))
        Me.Controls.Add(lnkElement)
        Me.Controls.Add(New LiteralControl("</td>"))

        Me.Controls.Add(New LiteralControl("<td class=""beb_tdd"">"))

        Me.Controls.Add(New LiteralControl("<div class=""beb_dtf"">"))
        Me.Controls.Add(imgError)
        Me.Controls.Add(lblDESEL_TX)
        Me.Controls.Add(lblDESFORMA)
        Me.Controls.Add(lblDESCATEG)
        Me.Controls.Add(New LiteralControl("</div>"))

        Me.Controls.Add(New LiteralControl("<div class=""beb_cmd"">"))
        Me.Controls.Add(lnkNuovo)
        Me.Controls.Add(lnkSeleziona)
        Me.Controls.Add(lnkRimuovi)
        Me.Controls.Add(New LiteralControl("</div>"))

        Me.Controls.Add(New LiteralControl("</td>"))

        Me.Controls.Add(New LiteralControl("</tr></table>"))

        'aggiunta controlli nascosti
        Me.Controls.Add(ID_ELEME)
        Me.Controls.Add(hidDefaultCODCATEG)
        Me.Controls.Add(hidDefaultDescriptionPreamble)
        Me.Controls.Add(hidDefaultDescriptionSourceTextBoxClientID)
        Me.Controls.Add(hidDefaultDescriptionPostamble)
        Me.Controls.Add(lnkRefresh)



        'gestione javascript
        formBaseClientId = FindParentStlFormView.ClientID
        baseClientId = Me.ClientID
        clientIdSeparator = Me.ClientIDSeparator
        refreshJsClientId = lnkRefresh.ClientID.Replace(clientIdSeparator, "$")
        binariesBaseUrl = Me.Page.ResolveUrl("~/Binaries/")

        lnkNuovo.NavigateUrl = "javascript:stl_binaryelementbox_new('" & formBaseClientId & "','" & baseClientId & "','" & refreshJsClientId & "','" & clientIdSeparator & "','" & binariesBaseUrl & "');"
        lnkSeleziona.NavigateUrl = "javascript:stl_binaryelementbox_choose('" & formBaseClientId & "','" & baseClientId & "','" & refreshJsClientId & "','" & clientIdSeparator & "','" & binariesBaseUrl & "');"
        lnkRimuovi.NavigateUrl = "javascript:stl_binaryelementbox_clear('" & formBaseClientId & "','" & baseClientId & "','" & refreshJsClientId & "','" & clientIdSeparator & "','" & binariesBaseUrl & "');"


    End Sub

    Private Sub BinaryElementBox_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        'scrittura dati nei controlli nascosti
        hidDefaultCODCATEG.Value = _defaultCODCATEG
        hidDefaultDescriptionPreamble.Value = _defaultDescriptionPreamble
        hidDefaultDescriptionPostamble.Value = _defaultDescriptionPostamble
        If _defaultDescriptionTextBoxID = "" Then
            hidDefaultDescriptionSourceTextBoxClientID.Value = ""
        Else
            hidDefaultDescriptionSourceTextBoxClientID.Value = CType(FindParentStlFormView.FindControl(_defaultDescriptionTextBoxID), TextBox).ClientID
        End If

        'gestione di tutto quello che ha a che fare con l'immagine
        If ID_ELEME.Value = "" Then
            'vuoto
            lnkElement.ImageUrl = Page.ResolveUrl("~/img/EmptyBinary.gif")
            lnkElement.NavigateUrl = ""
            lblDESEL_TX.Text = "Non presente"
            lblDESFORMA.Text = ""
            lblDESCATEG.Text = ""
        Else
            'ok, abbiamo l'immagine
            Dim dbConn As SqlConnection
            Dim userBinaryElement As UserBinaryElement
            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()
            userBinaryElement = New UserBinaryElement(dbConn, ContextHandler.ID_AZIEN, ContextHandler.ID_UTENT, CInt(ID_ELEME.Value), ContextHandler.BinariesBasePath)
            dbConn.Close()

            With userBinaryElement
                If Not .Exists Or Not .CAN_VIEW Then
                    Throw New Exception("Impossibile accedere all'immagine/allegato. Elemento inesistente o utente non autorizzato.")
                    Exit Sub
                End If
            End With

            'OK abbiamo l'elemento e siamo autorizzati.
            lnkElement.ImageUrl = Page.ResolveUrl("~/Binaries/BOThumbnail.aspx") & "?id=" & ID_ELEME.Value

            lnkElement.NavigateUrl = "javascript:wopen('" & Page.ResolveUrl("~/Binaries/ElementPreview.aspx") & "?id=" & ID_ELEME.Value & "','binarypreview',980,700,1,0,0,1,1);"
            lblDESEL_TX.Text = userBinaryElement.DESEL_TX
            lblDESFORMA.Text = userBinaryElement.DESFORMA
            lblDESCATEG.Text = userBinaryElement.DESCATEG

        End If
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

    Public Function IsValid() As Boolean Implements ICustomControl.IsValid
        Return True
    End Function
End Class
