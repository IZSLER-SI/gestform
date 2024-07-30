Public Class FormazioneEsterna
    Inherits System.Web.UI.Page

    Dim dbConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'gestione degli script
        If Not Page.IsPostBack Then
            Dim sScript = ""

            sScript &= "function editPartecipazione_callback(codice) { if(codice!='') {" & vbCrLf
            sScript &= "$get('" & txtReposition.ClientID & "').value=codice;" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkReposition, "").Replace("javascript:", "")
            sScript &= "}}" & vbCrLf

            Me.ltrRepositioning.Text = sScript
        End If

        srcPARTECIPAZIONI.AddNewLineAfter("ac_MATRICOLA")

        If Not Page.IsPostBack Then
            Dim dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ext_TipiPartecipazioneBO"
            End With
            Transformer.Transform(dbCmd, "Templates/FormazioneEsterna_NuovaPartecipazione.xslt", phdPulsantiNuovo)
        End If


    End Sub

    Private Sub FormazioneEsterna_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then
            srcPARTECIPAZIONI.SetVisibleValue("ac_STATOPARTECIPAZIONE", "BO_WAIT")
        End If
    End Sub

    Private Sub lnkReposition_Click(sender As Object, e As EventArgs) Handles lnkReposition.Click

        'deseleziono
        grdPARTECIPAZIONI.SelectedIndex = -1
        'forzo ricerca
        srcPARTECIPAZIONI.ForceSearch(True)

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdPARTECIPAZIONI.DataKeys.Count - 1
            If grdPARTECIPAZIONI.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdPARTECIPAZIONI.SelectedIndex = sIdx
            grdPARTECIPAZIONI.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)
        End If
    End Sub

    Private Sub grdPARTECIPAZIONI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdPARTECIPAZIONI.SelectedIndexChanged

        If grdPARTECIPAZIONI.SelectedIndex <> -1 Then

            Dim id_PARTECIPAZIONE As String = grdPARTECIPAZIONI.SelectedDataKey.Value.ToString

            'scrivo il valore
            txtReposition.Text = id_PARTECIPAZIONE
            updHiddenCtls.Update()

            'script apertura
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupPartecipazione",
                    "schedaPartecipazione('" & id_PARTECIPAZIONE.ToString & "');", True)

        End If
    End Sub

End Class