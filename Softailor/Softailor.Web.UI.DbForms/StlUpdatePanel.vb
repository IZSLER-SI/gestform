Imports System.Web.UI.WebControls.GridView
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class StlUpdatePanel
    Inherits UpdatePanel

    Private Enum MyType As Integer
        ContainingNothing = 0
        ContainingGrid = 1
        ContainingForm = 2
        ContainingSearchForm = 3
    End Enum

    Private _myType As MyType = MyType.ContainingNothing
    Private _width As Unit = Unit.Empty
    Private _height As Unit = Unit.Empty
    Private _left As Unit = Unit.Empty
    Private _top As Unit = Unit.Empty

    Public Property Width() As Unit
        Get
            Return _width
        End Get
        Set(ByVal value As Unit)
            _width = value
        End Set
    End Property

    Public Property Height() As Unit
        Get
            Return _height
        End Get
        Set(ByVal value As Unit)
            _height = value
        End Set
    End Property

    Public Property Left() As Unit
        Get
            Return _left
        End Get
        Set(ByVal value As Unit)
            _left = value
        End Set
    End Property

    Public Property Top() As Unit
        Get
            Return _top
        End Get
        Set(ByVal value As Unit)
            _top = value
        End Set
    End Property

    'NEWGRID
    Protected Overrides Sub RenderChildren(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Not IsInPartialRendering Then
            If _myType = MyType.ContainingGrid Then
                'aggiunta classe css
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "stl_upg")
            ElseIf _myType = MyType.ContainingForm Then
                'aggiunta classe css
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "stl_upf")
            ElseIf _myType = MyType.ContainingSearchForm Then
                'aggiunta classe css
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "stl_srf")
            End If
            If _myType = MyType.ContainingForm Or _myType = MyType.ContainingGrid Or _myType = MyType.ContainingSearchForm Then
                'aggiunta larghezza, altezza, top, left, position
                If Not _width.IsEmpty Then writer.AddStyleAttribute(HtmlTextWriterStyle.Width, _width.ToString)
                If Not _height.IsEmpty Then writer.AddStyleAttribute(HtmlTextWriterStyle.Height, _height.ToString)
                If Not _top.IsEmpty Then writer.AddStyleAttribute(HtmlTextWriterStyle.Top, _top.ToString)
                If Not _left.IsEmpty Then writer.AddStyleAttribute(HtmlTextWriterStyle.Left, _left.ToString)
                If Not _top.IsEmpty Then writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute")
            End If

        End If
        MyBase.RenderChildren(writer)
    End Sub

    Private Sub StlUpdatePanel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'forzo il mio updatemode
        Me.UpdateMode = UpdatePanelUpdateMode.Conditional

        'determino _myType
        DiscoverMyType()

        'se griglia > no children
        If _myType = MyType.ContainingGrid Then
            Me.ChildrenAsTriggers = False
        End If

    End Sub

    Private Sub DiscoverMyType()
        _myType = MyType.ContainingNothing
        DiscoverMyTypeInt(Me)
    End Sub

    Private Sub DiscoverMyTypeInt(ByVal rootControl As Control)

        Dim cSub As Control

        If TypeOf rootControl Is StlGridView Then
            _myType = MyType.ContainingGrid
        ElseIf TypeOf rootControl Is StlFormView Then
            _myType = MyType.ContainingForm
        ElseIf TypeOf rootControl Is StlSearchForm Then
            _myType = MyType.ContainingSearchForm
        ElseIf TypeOf rootControl Is ISqlStringProvider Then
            _myType = MyType.ContainingSearchForm
        Else
            For Each cSub In rootControl.Controls
                DiscoverMyTypeInt(cSub)
            Next
        End If

    End Sub
End Class
