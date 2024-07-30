Public Class GestioneAttestatiOnLine
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        SetupPage()

    End Sub


    Private Sub SetupPage()

        Dim fl_ATTECMONLINE As Boolean
        Dim dt_ATTECMONLINE As SqlDateTime
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim i As Integer

        'pulizie
        lblNumPartecipanti.Text = ""
        lblNumRelatori.Text = ""
        lblStatoAttivazione.Text = ""
        lnkAttiva.Visible = False
        lnkDisattiva.Visible = False
        pnlWarningAttiva.Visible = False
        pnlWarningDisattiva.Visible = False
        phdRiepiloghi.Controls.Clear()

        'lettura numero relatori
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOECM='COK' AND ac_CATEGORIAECM<>'P'"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumRelatori.Text = "Nessuno"
            lblNumRelatori.ForeColor = Drawing.Color.DarkRed
        Else
            lblNumRelatori.Text = i.ToString
            lblNumRelatori.ForeColor = Drawing.Color.DarkGreen
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura numero partecipanti
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOECM='COK' AND ac_CATEGORIAECM='P'"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumPartecipanti.Text = "Nessuno"
            lblNumPartecipanti.ForeColor = Drawing.Color.DarkRed
        Else
            lblNumPartecipanti.Text = i.ToString
            lblNumPartecipanti.ForeColor = Drawing.Color.DarkGreen
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura stato attivazione
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT fl_ATTECMONLINE, dt_ATTECMONLINE FROM eve_EVENTI WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        fl_ATTECMONLINE = dbRdr.GetBoolean(0)
        dt_ATTECMONLINE = dbRdr.GetSqlDateTime(1)
        dbRdr.Close()
        dbCmd.Dispose()

        If fl_ATTECMONLINE Then
            lblStatoAttivazione.Text = _
                "Stato: <b>Scaricamento attivo</b> (attivato in data " & dt_ATTECMONLINE.Value.ToString("dd/MM/yyyy") & ")"
            lnkDisattiva.Visible = True
            pnlWarningDisattiva.Visible = True
            GeneraListe()
        Else
            lblStatoAttivazione.Text = _
                "Stato: <b>Scaricamento non attivo</b>"
            lnkAttiva.Visible = True
            pnlWarningAttiva.Visible = True
        End If


    End Sub

    Private Sub GeneraListe()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ecm_StatoScaricamentoECM"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Transformer.Transform(dbCmd, "Templates/StatoScaricamentoECM.xslt", phdRiepiloghi)

    End Sub

    Private Sub lnkAttiva_Click(sender As Object, e As System.EventArgs) Handles lnkAttiva.Click

        Dim dbCmd As SqlCommand

        'attivazione
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_EVENTI SET fl_ATTECMONLINE=1, dt_ATTECMONLINE=@dt_attecmonline WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@dt_attecmonline", SqlDbType.DateTime).Value = Date.Now
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        'refresh
        SetupPage()

    End Sub

    Private Sub lnkDisattiva_Click(sender As Object, e As System.EventArgs) Handles lnkDisattiva.Click

        Dim dbCmd As SqlCommand

        'disattivazione
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_EVENTI SET fl_ATTECMONLINE=0, dt_ATTECMONLINE=null WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        'refresh
        SetupPage()

    End Sub

    Private Sub GestioneAttestatiOnLineGF_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub
End Class