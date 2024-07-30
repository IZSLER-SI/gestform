Imports System.Web.UI
Imports System.Data.SqlClient

Public Class StlItalianCFPITextBox
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer
    Private _acceptStraniero As Boolean = False

    'controlli
    Dim txtCodiceFiscale As TextBox

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return txtCodiceFiscale.Text.Trim.ToUpper
        End Get
        Set(ByVal value As String)
            txtCodiceFiscale.Text = value.Trim.ToUpper
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
        txtCodiceFiscale.Enabled = True
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            txtCodiceFiscale.ToolTip = "Codice Fiscale o Partita IVA formalmente errati."
        Else
            txtCodiceFiscale.ToolTip = errorText
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
        txtCodiceFiscale.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        txtCodiceFiscale.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        txtCodiceFiscale.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        txtCodiceFiscale.BackColor = Drawing.Color.Yellow
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        _isEnabled = False
        txtCodiceFiscale.Enabled = False
    End Sub

    Private Sub SelettoreArticolo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        txtCodiceFiscale = New TextBox
        txtCodiceFiscale.ID = "txtCodiceFiscale"
        txtCodiceFiscale.MaxLength = 16  'lunghezza massima ac_ARTICOLO
        txtCodiceFiscale.CssClass = "txt"
        txtCodiceFiscale.Width = Unit.Parse("150px")

        Me.Controls.Add(txtCodiceFiscale)

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

        Dim sCf = txtCodiceFiscale.Text.Trim.ToUpper
        If sCf = "" Then
            Return True
        Else
            If _acceptStraniero Then
                Return Softailor.Global.ValidationUtils.ValidateCodiceFiscaleItaliano(sCf) Or
                       Softailor.Global.ValidationUtils.ValidatePartitaIVAItaliana(sCf) Or
                       sCf = "STRANIERO"
            Else
                Return Softailor.Global.ValidationUtils.ValidateCodiceFiscaleItaliano(sCf) Or
                       Softailor.Global.ValidationUtils.ValidatePartitaIVAItaliana(sCf)
            End If

        End If
    End Function

    Public Property AcceptStraniero() As Boolean
        Get
            Return _acceptStraniero
        End Get
        Set(value As Boolean)
            _acceptStraniero = value
        End Set
    End Property
End Class
