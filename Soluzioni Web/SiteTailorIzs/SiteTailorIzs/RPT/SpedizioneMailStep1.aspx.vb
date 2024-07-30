Public Class SpedizioneMailStep1
    Inherits System.Web.UI.Page

    Private Sub SpedizioneMailStep1_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim fromProxy As Boolean

        'recupero campi dal parent
        If Not Page.IsPostBack Then
            With Request.Unvalidated.Form

                If .Item("sd_ac_FONTE") IsNot Nothing Then
                    fromProxy = True
                    sd_ac_FONTE.Text = .Item("sd_ac_FONTE")
                    sd_id_MAILREPORT.Text = .Item("sd_id_MAILREPORT")
                    sd_tx_VALOREFILTROBASE.Text = .Item("sd_tx_VALOREFILTROBASE")
                    sd_xm_FILTRO.Text = .Item("sd_xm_FILTRO")
                    sd_ac_ORDINAMENTO.Text = .Item("sd_ac_ORDINAMENTO")
                    sd_tx_ORDINAMENTO1.Text = .Item("sd_tx_ORDINAMENTO1")
                    sd_tx_ORDINAMENTO2.Text = .Item("sd_tx_ORDINAMENTO2")
                    sd_tx_ORDINAMENTO3.Text = .Item("sd_tx_ORDINAMENTO3")
                    sd_tx_ORDINAMENTO4.Text = .Item("sd_tx_ORDINAMENTO4")
                    sd_tx_ORDINAMENTO5.Text = .Item("sd_tx_ORDINAMENTO5")
                    sd_ragionesociale.Text = .Item("sd_ragionesociale")
                    sd_indirizzocompleto.Text = .Item("sd_indirizzocompleto")
                    sd_tel.Text = .Item("sd_tel")
                    sd_fax.Text = .Item("sd_fax")
                    sd_email.Text = .Item("sd_email")
                Else
                    fromProxy = False
                    sd_ac_FONTE.Text = .Item("sd3_ac_FONTE")
                    sd_id_MAILREPORT.Text = .Item("sd3_id_MAILREPORT")
                    sd_tx_VALOREFILTROBASE.Text = .Item("sd3_tx_VALOREFILTROBASE")
                    sd_xm_FILTRO.Text = .Item("sd3_xm_FILTRO")
                    sd_ac_ORDINAMENTO.Text = .Item("sd3_ac_ORDINAMENTO")
                    sd_tx_ORDINAMENTO1.Text = .Item("sd3_tx_ORDINAMENTO1")
                    sd_tx_ORDINAMENTO2.Text = .Item("sd3_tx_ORDINAMENTO2")
                    sd_tx_ORDINAMENTO3.Text = .Item("sd3_tx_ORDINAMENTO3")
                    sd_tx_ORDINAMENTO4.Text = .Item("sd3_tx_ORDINAMENTO4")
                    sd_tx_ORDINAMENTO5.Text = .Item("sd3_tx_ORDINAMENTO5")
                    sd_ragionesociale.Text = .Item("sd3_ragionesociale")
                    sd_indirizzocompleto.Text = .Item("sd3_indirizzocompleto")
                    sd_tel.Text = .Item("sd3_tel")
                    sd_fax.Text = .Item("sd3_fax")
                    sd_email.Text = .Item("sd3_email")
                End If

            End With
        End If

        'campi fonte
        If Not Page.IsPostBack Then
            Dim fonteXDoc As New XmlDocument
            fonteXDoc.Load(Server.MapPath("~/RPT/Fonti/" & sd_ac_FONTE.Text & ".xml"))
            Transformer.Transform(fonteXDoc, "Templates/CampiFonte.xslt", phdElencoCampi)
        End If

        'oggetto e corpo
        If Not Page.IsPostBack Then
            If fromProxy Then
                Dim rptDbConn = DbConnectionHandler.GetOpenDataDbConn

                Dim dbCmd = rptDbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT tx_OGGETTO, ht_CORPO FROM rpt_MAILREPORTS WHERE ac_FONTE=@ac_FONTE AND id_MAILREPORT=@id_MAILREPORT"
                    .Parameters.Add("@ac_FONTE", SqlDbType.NVarChar, 20).Value = sd_ac_FONTE.Text
                    .Parameters.Add("@id_MAILREPORT", SqlDbType.Int).Value = CInt(sd_id_MAILREPORT.Text)
                End With
                Dim dbRdr = dbCmd.ExecuteReader
                If dbRdr.Read Then
                    If Not dbRdr.IsDBNull(0) Then tx_OGGETTO.Text = dbRdr.GetString(0)
                    If Not dbRdr.IsDBNull(1) Then ht_CORPO.Text = dbRdr.GetString(1)
                End If
                dbRdr.Close()
                dbCmd.Dispose()


                rptDbConn.Close()
                rptDbConn.Dispose()
            Else
                With Request.Unvalidated.Form
                    tx_OGGETTO.Text = .Item("sd3_tx_OGGETTO")
                    ht_CORPO.Text = .Item("sd3_ht_CORPO")
                End With
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub lnkProsegui_Click(sender As Object, e As EventArgs) Handles lnkProsegui.Click

        'copio i dati di oggetto e testo
        sd_tx_OGGETTO.Text = tx_OGGETTO.Text
        sd_ht_CORPO.Text = ht_CORPO.Text

        'forzo update
        updDatiGenerazione.Update()

        'lancio javascript per il POST
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "gotoNext", "NextStep();", True)

    End Sub
End Class