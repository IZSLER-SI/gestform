Imports System.Web.UI
Imports System.Web.UI.WebControls.CommandField


Public Class StlCommandField
    Inherits CommandField

    Public Sub New()
        Me.ShowInsertButton = False
        Me.ShowSelectButton = False
        Me.ShowCancelButton = False
    End Sub

    Public Overrides Sub InitializeCell(ByVal cell As System.Web.UI.WebControls.DataControlFieldCell, ByVal cellType As System.Web.UI.WebControls.DataControlCellType, ByVal rowState As System.Web.UI.WebControls.DataControlRowState, ByVal rowIndex As Integer)

        MyBase.InitializeCell(cell, cellType, rowState, rowIndex)

        If cellType = DataControlCellType.DataCell Then
            cell.Controls.Clear()
            If ShowDeleteButton Then
                cell.Controls.Add(New LiteralControl("<span class=""del"" onclick=""stl_grd_del(this);""></span>"))
            End If
            If ShowEditButton Then
                cell.Controls.Add(New LiteralControl("<span class=""edit"" onclick=""stl_grd_edit(this);""></span>"))
            End If
        End If


        'Exit Sub


        'Dim c As Control
        'Dim cFound As Control
        'Dim cIsFound As Boolean
        'Dim cEdit As Control = Nothing
        'Dim cDelete As Control = Nothing

        ''rimozione spazi
        'Do
        '    cFound = Nothing
        '    cIsFound = False
        '    For Each c In cell.Controls
        '        If TypeOf c Is LiteralControl Then
        '            cFound = c
        '            cIsFound = True
        '            Exit For
        '        ElseIf TypeOf c Is LinkButton Then
        '            If CType(c, LinkButton).CommandName = "Edit" Then cEdit = CType(c, LinkButton)
        '            If CType(c, LinkButton).CommandName = "Delete" Then cDelete = CType(c, LinkButton)
        '        End If
        '    Next
        '    If cIsFound Then cell.Controls.Remove(cFound)
        'Loop While cIsFound

        ''swap controlli
        'If Not (cEdit Is Nothing) And Not (cDelete Is Nothing) Then
        '    cell.Controls.Remove(cEdit)
        '    cell.Controls.Remove(cDelete)
        '    cell.Controls.Add(cDelete)
        '    cell.Controls.Add(cEdit)
        'End If


    End Sub

End Class
