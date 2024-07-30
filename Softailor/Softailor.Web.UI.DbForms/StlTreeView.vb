Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls

Public Class StlTreeView
    Inherits System.Web.UI.WebControls.TreeView

    Private Sub StlTreeView_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim hiddenName As String
        Dim js As String
        Dim realNodeName As String
        Dim nodeCssClass As String = ""
        Dim selectedNodeCssClass As String = ""

        'ID campo "hidden" per mantenere l'informazione circa il nodo selezionato precedentemente
        hiddenName = Me.ClientID & "_Old"

        'ID assegnato da asp al nodo
        realNodeName = "'" & Me.ClientID & "t' + id_nodo"

        If Not String.IsNullOrEmpty(Me.NodeStyle.CssClass) Then
            nodeCssClass = NodeStyle.CssClass
        End If
        If Not String.IsNullOrEmpty(Me.NodeStyle.CssClass) Then
            selectedNodeCssClass = SelectedNodeStyle.CssClass
        End If

        js = "function " & Me.ClientID & "_Select(id_nodo) {" & vbCrLf & _
                "TreeView_SelectNode(" & Me.ClientID & "_Data, this,'" & Me.ClientID & "t' + id_nodo);" & vbCrLf & _
                "if (document.getElementById('" & hiddenName & "').value != " & realNodeName & ") {" & vbCrLf & _
                    "var vecchioElemento = document.getElementById(document.getElementById('" & hiddenName & "').value);" & vbCrLf & _
                    "var nuovoElemento = document.getElementById(" & realNodeName & ");" & vbCrLf & _
                    "if (document.getElementById('" & hiddenName & "').value != -1) {" & vbCrLf & _
                        "vecchioElemento.parentNode.className = """ & nodeCssClass & " " & Me.ClientID & "_2"";" & vbCrLf & _
                        "vecchioElemento.className = """ & Me.ClientID & "_0 " & nodeCssClass & " " & Me.ClientID & "_1""" & vbCrLf & _
                    "}" & vbCrLf & _
                    "nuovoElemento.parentNode.className = """ & nodeCssClass & " " & Me.ClientID & "_2 " & selectedNodeCssClass & " " & Me.ClientID & "_4"";" & vbCrLf & _
                    "nuovoElemento.className = """ & Me.ClientID & "_0 " & nodeCssClass & " " & Me.ClientID & "_1 " & selectedNodeCssClass & " " & Me.ClientID & "_3"";" & vbCrLf & _
                    "document.getElementById('" & hiddenName & "').value = " & realNodeName & ";" & vbCrLf & _
                "}" & vbCrLf & _
            "}" & vbCrLf

        Dim oldSelected As String
        If Not Page.IsPostBack Then
            oldSelected = "-1"
        Else
            oldSelected = Page.Request(Me.ClientID & "_Old")
        End If
        Page.ClientScript.RegisterHiddenField(Me.ClientID & "_Old", oldSelected)

        Page.ClientScript.RegisterClientScriptBlock(Page.GetType, Me.ClientID & "Script", js, True)

        Dim nodeCount As Integer = 0
        RecursivelyFormatNodes(Me.Nodes, nodeCount)

    End Sub

    Private Sub RecursivelyFormatNodes(ByVal ndc As TreeNodeCollection, ByRef nodeCount As Integer)
        For Each nd As TreeNode In ndc
            If nd.SelectAction = TreeNodeSelectAction.Select Then
                nd.NavigateUrl = "javascript:" & Me.ClientID & "_Select(" & nodeCount & ");"
                nodeCount += 1
                If nd.ChildNodes.Count > 0 Then
                    RecursivelyFormatNodes(nd.ChildNodes, nodeCount)
                End If

            End If
        Next
    End Sub

End Class
