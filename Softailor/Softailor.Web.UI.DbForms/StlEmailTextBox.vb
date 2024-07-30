Imports System.Web.UI
Imports System.Data.SqlClient
'Imports Softailor.Net35.SiteTailor.ACL

Public Class StlEmailTextBox
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer

    'controlli
    Dim txtEmail As TextBox

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return txtEmail.Text.Trim.ToLower
        End Get
        Set(ByVal value As String)
            txtEmail.Text = value.Trim.ToLower
        End Set
    End Property


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
        txtEmail.Enabled = True
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            txtEmail.ToolTip = "Indirizzo e-mail formalmente errato."
        Else
            txtEmail.ToolTip = errorText
        End If
    End Sub

    Public Property MaxLength() As Integer Implements ICustomControl.MaxLength
        Get
            Return _maxLength
        End Get
        Set(ByVal value As Integer)
            _maxLength = value
            If Not txtEmail Is Nothing Then
                txtEmail.MaxLength = _maxLength
            End If
        End Set
    End Property

    Public Sub SetIsOK() Implements ICustomControl.SetIsOK
        txtEmail.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        txtEmail.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        txtEmail.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        txtEmail.BackColor = Drawing.Color.Yellow
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        _isEnabled = False
        txtEmail.Enabled = False
    End Sub

    Private Sub SelettoreArticolo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        txtEmail = New TextBox
        txtEmail.ID = "txtCodiceFiscale"
        txtEmail.MaxLength = _maxLength
        txtEmail.CssClass = "txt"
        txtEmail.Width = Me.Width

        Me.Controls.Add(txtEmail)

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

    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid

        Dim sEmail = txtEmail.Text.Trim.ToLower
        If sEmail = "" Then
            Return True
        Else
            Return Softailor.Global.ValidationUtils.ValidateEmail(sEmail)
        End If
    End Function
End Class
