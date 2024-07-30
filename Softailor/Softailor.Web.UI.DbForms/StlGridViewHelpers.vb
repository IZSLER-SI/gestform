Public Class StlGridViewHelpers

    Public Shared Sub AdjustTabContainers(rootControl As Control)
        Dim tabContainersList As New List(Of AjaxControlToolkit.TabContainer)
        'recupero tutti i panel nel documento
        EnumerateTabContainers(rootControl, tabContainersList)

        For Each c In tabContainersList
            c.OnClientActiveTabChanged = "stl_tab_activetabchanged"
        Next
    End Sub

    Public Shared Function GetClientGridScripts(rootControl As Control, csm As ClientScriptManager) As String

        Dim gridList As New List(Of StlGridView)
        Dim script As String = ""
        Dim postBack As String
        Dim question As String

        'recupero tutte le griglie nel documento
        EnumerateGrids(rootControl, gridList)

        script &= "function stl_grd_eventRouter(gridId, rowIdx, commandName) {" & vbCrLf
        script &= vbTab & "switch (gridId) {" & vbCrLf
        For Each grid In gridList
            script &= vbTab & vbTab & "case """ & grid.ID & """: " & vbCrLf

            postBack = csm.GetPostBackEventReference(grid, "[[[args]]]")
            postBack = postBack.Replace("'[[[args]]]'", "commandName + '$' + rowIdx")
            postBack = postBack.Replace("""[[[args]]]""", "commandName + ""$"" + rowIdx")

            script &= vbTab & vbTab & vbTab & postBack & ";" & vbCrLf

            script &= vbTab & vbTab & vbTab & "break;" & vbCrLf

        Next
        script &= vbTab & "};" & vbCrLf
        script &= "}" & vbCrLf

        script &= "function stl_grd_addNewRouter(gridId) {" & vbCrLf
        script &= vbTab & "switch (gridId) {" & vbCrLf
        For Each grid In gridList

            postBack = grid.GetAddNewJs()
            If postBack <> String.Empty Then
                script &= vbTab & vbTab & "case """ & grid.ID & """: " & vbCrLf
                script &= vbTab & vbTab & vbTab & postBack & ";" & vbCrLf
                script &= vbTab & vbTab & vbTab & "break;" & vbCrLf
            End If
        Next

        script &= vbTab & "};" & vbCrLf
        script &= "}" & vbCrLf

        script &= "function stl_grd_selectedRowClass(gridId) {" & vbCrLf
        script &= vbTab & "switch (gridId) {" & vbCrLf
        For Each grid In gridList
            script &= vbTab & vbTab & "case """ & grid.ID & """: return("""

            If grid.AllowReselectSelectedRow Then
                script &= "src"");"
            Else
                script &= "sr"");"
            End If
            script &= " break;" & vbCrLf

        Next
        script &= vbTab & "};" & vbCrLf
        script &= "}" & vbCrLf

        script &= "function stl_grd_clientId(gridId) {" & vbCrLf
        script &= vbTab & "switch (gridId) {" & vbCrLf
        For Each grid In gridList
            script &= vbTab & vbTab & "case """ & grid.ID & """: return(""" & grid.ClientID & """);"
            script &= " break;" & vbCrLf

        Next
        script &= vbTab & "};" & vbCrLf
        script &= "}" & vbCrLf

        script &= "function stl_grd_deleteConfirmationQuestion(gridId) {" & vbCrLf
        script &= vbTab & "switch (gridId) {" & vbCrLf
        For Each grid In gridList

            script &= vbTab & vbTab & "case """ & grid.ID & """: return("

            'messaggio conferma eliminazione
            If grid.DeleteConfirmationQuestion = "" Then
                question = "Confermi l'eliminazione della riga selezionata?"
            Else
                question = grid.DeleteConfirmationQuestion
            End If
            script &= Softailor.Global.JSUtils.EncodeJsStringWithQuotes(question) & "); break;" & vbCrLf

        Next
        script &= vbTab & "};" & vbCrLf
        script &= "}" & vbCrLf

        Return script

    End Function

    Private Shared Sub EnumerateGrids(c As Control, ByRef list As List(Of StlGridView))

        If TypeOf c Is StlGridView Then
            list.Add(CType(c, StlGridView))
        End If

        For Each cSub As Control In c.Controls
            EnumerateGrids(cSub, list)
        Next

    End Sub

    Private Shared Sub EnumerateTabContainers(c As Control, ByRef list As List(Of AjaxControlToolkit.TabContainer))

        If TypeOf c Is AjaxControlToolkit.TabContainer Then
            list.Add(CType(c, AjaxControlToolkit.TabContainer))
        End If

        For Each cSub As Control In c.Controls
            EnumerateTabContainers(cSub, list)
        Next

    End Sub


End Class
