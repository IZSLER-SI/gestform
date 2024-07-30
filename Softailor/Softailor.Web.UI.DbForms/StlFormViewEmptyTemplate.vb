Imports System.Web.UI

Public Class StlFormViewEmptyTemplate
    Implements System.Web.UI.ITemplate


    Private _headerTemplate As ITemplate
    Private _itemTemplate As ITemplate

    Public Sub New(ByVal headerTemplate As ITemplate, ByVal itemTemplate As ITemplate)
        _headerTemplate = headerTemplate
        _itemTemplate = itemTemplate
    End Sub

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn

        _headerTemplate.InstantiateIn(container)
        'trucco schifoso :-)
        container.Controls.Add(New LiteralControl("</td></tr><tr class=""b""><td class=""b"">"))
        _itemTemplate.InstantiateIn(container)
    End Sub

End Class
