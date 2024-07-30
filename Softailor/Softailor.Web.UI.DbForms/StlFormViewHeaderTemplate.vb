Public Class StlFormViewHeaderTemplate
    Implements System.Web.UI.ITemplate

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn

        Dim button As Button
        Dim label As Label

        button = New Button
        With button
            .ID = "NewButton"
            .CausesValidation = False
            .CommandName = "New"
            .Text = "Modifica"
            .CssClass = "frmBtn"
        End With
        container.Controls.Add(button)

        button = New Button
        With button
            .ID = "EditButton"
            .CausesValidation = False
            .CommandName = "Edit"
            .Text = "Modifica"
            .CssClass = "frmBtn"
        End With
        container.Controls.Add(button)

        button = New Button
        With button
            .ID = "InsertButton"
            .CausesValidation = False
            .CommandName = "Insert"
            .Text = "Salva"
            .CssClass = "frmBtn"
        End With
        container.Controls.Add(button)

        button = New Button
        With button
            .ID = "InsertCancelButton"
            .CausesValidation = False
            .CommandName = "Cancel"
            .Text = "Annulla"
            .CssClass = "frmBtn"
        End With
        container.Controls.Add(button)

        button = New Button
        With button
            .ID = "UpdateButton"
            .CausesValidation = False
            .CommandName = "Update"
            .Text = "Salva"
            .CssClass = "frmBtn"
        End With
        container.Controls.Add(button)

        button = New Button
        With button
            .ID = "UpdateCancelButton"
            .CausesValidation = False
            .CommandName = "Cancel"
            .Text = "Annulla"
            .CssClass = "frmBtn"
        End With
        container.Controls.Add(button)

        label = New Label
        With label
            .ID = "StatusLabel"
            .Text = ""
            .CssClass = "status"
        End With
        container.Controls.Add(label)

    End Sub
End Class
