Public Class HtmlEditor
    Inherits WebControl
    Implements ICustomControl, System.Web.UI.INamingContainer

    'proprietà da designer
    Private _fieldName As String = ""
    Private _maxLength As Integer = 0
    Private _toolbarSet As String = ""
    'Private _toolbarStartExpanded As Boolean = True
    Private _basePath As String = ""
    Private _value As String = ""

    Private _fckeditor As CKEditor.NET.CKEditorControl
    Private _errordiv As System.Web.UI.HtmlControls.HtmlGenericControl
    Private _div As System.Web.UI.HtmlControls.HtmlGenericControl
    Private _outdiv As System.Web.UI.HtmlControls.HtmlGenericControl
    Private _errorText As String = ""

    Private Property IntIsEnabled() As Boolean
        Get
            Return CBool(ViewState("IntIsEnabled"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("IntIsEnabled") = value
        End Set
    End Property

    Private Property IntHasError() As Boolean
        Get
            Return CBool(ViewState("IntHasError"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("IntHasError") = value
        End Set
    End Property

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        _errorText = ""
        If Not _errordiv Is Nothing Then _errordiv.InnerText = _errorText
    End Sub

    Public Property FieldName() As String Implements ICustomControl.FieldName
        Get
            Return _fieldName
        End Get
        Set(ByVal value As String)
            _fieldName = value
        End Set
    End Property

    Public Property BasePath() As String
        Get
            Return _basePath
        End Get
        Set(ByVal value As String)
            _basePath = value
        End Set
    End Property

    Public Property ToolbarSet() As String
        Get
            Return _toolbarSet
        End Get
        Set(ByVal value As String)
            _toolbarSet = value
        End Set
    End Property

    'Public Property ToolbarStartExpanded() As Boolean
    '    Get
    '        Return _toolbarStartExpanded
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _toolbarStartExpanded = value
    '    End Set
    'End Property

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return IntIsEnabled
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
        IntIsEnabled = False
        GenerazioneControlli()
    End Sub

    Public Sub SetEnabled() Implements ICustomControl.SetEnabled
        IntIsEnabled = True
        GenerazioneControlli()
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        _errorText = errorText
        If Not _errordiv Is Nothing Then _errordiv.InnerText = _errorText
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        'faccio niente
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        IntHasError = True
        If Not _errordiv Is Nothing Then
            _errordiv.Style.Clear()
            _errordiv.Style.Add("background-color", "yellow")
            _errordiv.Style.Add("padding-left", "3px")
            _errordiv.Style.Add("font-size", "10px")
        End If
        If Not _fckeditor Is Nothing Then
            _fckeditor.Height = Unit.Pixel(CInt(Me.Height.Value) - 12)
        End If
    End Sub

    Public Sub SetIsOK() Implements ICustomControl.SetIsOK
        IntHasError = False
        IntHasError = True
        If Not _errordiv Is Nothing Then
            _errordiv.Style.Clear()
        End If
        If Not _fckeditor Is Nothing Then
            _fckeditor.Height = Me.Height
        End If
    End Sub

    Public Property Value() As String Implements ICustomControl.Value
        Get
            If IntIsEnabled Then
                Return _fckeditor.Text
            Else
                Return _div.InnerHtml
            End If
        End Get
        Set(ByVal value As String)
            _value = value
            If IntIsEnabled Then
                If (Not _fckeditor Is Nothing) Then _fckeditor.Text = _value
            Else
                If (Not _div Is Nothing) Then _div.InnerHtml = _value
            End If
        End Set
    End Property

    Private Sub HtmlEditor_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        IntIsEnabled = False
        IntHasError = False
    End Sub

    Private Sub GenerazioneControlli()

        Me.Controls.Clear()

        If IntIsEnabled Then
            _div = Nothing
            _outdiv = Nothing
            _fckeditor = New CKEditor.NET.CKEditorControl
            _fckeditor.Width = Me.Width
            If Me.ToolbarSet = "Full" Then
                _fckeditor.Height = Unit.Pixel(CInt(Me.Height.Value) - 86)
            Else
                _fckeditor.Height = Unit.Pixel(CInt(Me.Height.Value) - 27)
            End If

            _fckeditor.BasePath = If(String.IsNullOrEmpty(_basePath), "~/ckeditor", _basePath)
            _fckeditor.Toolbar = _toolbarSet
            '_fckeditor.ToolbarCanCollapse = False
            _fckeditor.ResizeEnabled = False
            '_fckeditor.ToolbarStartupExpanded = True    '_toolbarStartExpanded
            _fckeditor.Text = _value
            Me.Controls.Add(_fckeditor)
            _errordiv = New System.Web.UI.HtmlControls.HtmlGenericControl("div")
            Me.Controls.Add(_errordiv)
        Else
            _fckeditor = Nothing
            _outdiv = New System.Web.UI.HtmlControls.HtmlGenericControl("div")
            _outdiv.Style.Add("width", Me.Width.ToString)
            _outdiv.Style.Add("height", Me.Height.ToString)
            _outdiv.Attributes.Add("class", "hteddis_out")
            _div = New System.Web.UI.HtmlControls.HtmlGenericControl("div")
            _div.Attributes.Add("class", "hteddis_in")
            _div.InnerHtml = _value
            _outdiv.Controls.Add(_div)
            Me.Controls.Add(_outdiv)
        End If

    End Sub

    Private Sub HtmlEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GenerazioneControlli()
    End Sub

    Public Function IsValid() As Boolean Implements ICustomControl.IsValid
        Return True
    End Function
End Class

