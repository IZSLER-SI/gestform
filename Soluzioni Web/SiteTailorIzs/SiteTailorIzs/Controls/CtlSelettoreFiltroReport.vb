Imports System.Web.UI
Imports Softailor.Web.UI.DbForms
Imports Softailor.ReportEngine

Public Class CtlSelettoreFiltroReport
    Inherits WebControl
    Implements ICustomControl, INamingContainer

    'proprietà da designer
    Private _fieldName As String
    Private _isEnabled As Boolean
    Private _maxLength As Integer
    Private _soloDipendenti As Boolean

    'controlli
    Dim txtXmlFiltro As TextBox         'controllo nascosto che contiene l'XML del filtro.
    Dim spanEdit As HtmlGenericControl  'span editor

    Public Property Value() As String Implements ICustomControl.Value
        Get
            'ritorno un vuoto?
            If txtXmlFiltro.Text = "" Then
                Return (New Filtro).GetXml()
            Else
                Return txtXmlFiltro.Text
            End If
        End Get
        Set(ByVal value As String)
            txtXmlFiltro.Text = value
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

        spanEdit.Attributes("onclick") = "editFiltro();"
        spanEdit.Style("display") = "inline-block"

    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        'nulla
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
        'nulla
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        'nulla
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        'nulla
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        'nulla
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled

        _isEnabled = False

        spanEdit.Attributes("onclick") = "EditFiltro();"
        spanEdit.Style("display") = "none"

    End Sub

    Private Sub CtlSelettoreFiltroReport_Init(sender As Object, e As EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        txtXmlFiltro = New TextBox
        txtXmlFiltro.ID = "xmlFiltroPers"
        txtXmlFiltro.TextMode = TextBoxMode.MultiLine
        txtXmlFiltro.ClientIDMode = UI.ClientIDMode.Static
        txtXmlFiltro.Style.Add("display", "none")

        spanEdit = New HtmlGenericControl("span")
        spanEdit.ID = "spanEditFiltro"
        spanEdit.Attributes.Add("onclick", "")
        spanEdit.Attributes.Add("class", "btnlink")
        spanEdit.Style.Add("display", "none")
        spanEdit.InnerText = "Visualizza/Modifica Criteri"

        'aggiunta dei controlli
        Me.Controls.Add(spanEdit)
        Me.Controls.Add(txtXmlFiltro)
        
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
        Return True
    End Function


End Class
