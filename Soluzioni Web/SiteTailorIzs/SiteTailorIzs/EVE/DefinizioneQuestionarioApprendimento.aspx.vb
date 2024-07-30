Imports Softailor.Global.SqlUtils

Public Class DefinizioneQuestionarioApprendimento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Const MaxRisposte As Integer = 200
    Private panels As New Dictionary(Of Integer, Panel)
    Private textboxes As New Dictionary(Of Integer, TextBox)
    Private dbConn As SqlConnection

    Private Sub DefinizioneQuestionarioApprendimento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'riempimento drop down risposte
        FillDropDownNumeri()

        'creazione controlli
        CreaControlliRisposte()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'se siamo in apertura scrivo i dati
        If Not Page.IsPostBack Then
            ScriviDatiIniziali()
        End If

        'rendo visibili o invisibili i controlli risposte
        VisibilitaControlliRisposte()

    End Sub

    Private Sub FillDropDownNumeri()

        Dim i As Integer
        ddnNumeroDomande.Items.Clear()
        ddnMinimoRisposte.Items.Clear()

        ddnNumeroDomande.Items.Add(New ListItem("", ""))
        ddnMinimoRisposte.Items.Add(New ListItem("", ""))

        For i = 1 To MaxRisposte
            ddnNumeroDomande.Items.Add(New ListItem(i.ToString, i.ToString))
            ddnMinimoRisposte.Items.Add(New ListItem(i.ToString, i.ToString))
        Next

    End Sub

    Private Sub CreaControlliRisposte()

        Dim pnl As Panel
        Dim txt As TextBox

        phdRisposte.Controls.Clear()
        panels.Clear()
        textboxes.Clear()
        Dim i As Integer
        For i = 1 To MaxRisposte

            pnl = New Panel
            panels.Add(i, pnl)
            pnl.ID = "rp" & i
            pnl.CssClass = "pdo"
            pnl.Visible = False
            pnl.EnableViewState = True

            pnl.Controls.Add(New LiteralControl("<div class=""dom"">domanda</div>"))
            pnl.Controls.Add(New LiteralControl("<div class=""ndo"">" & i.ToString & "</div>"))

            txt = New TextBox
            textboxes.Add(i, txt)
            txt.ID = "rt" & i.ToString("000")
            txt.EnableViewState = True
            txt.CssClass = "tri"
            txt.MaxLength = 1
            txt.Attributes.Add("onkeyup", "gonext(this,'" & (i + 1).ToString("000") & "');")
            pnl.Controls.Add(txt)

            phdRisposte.Controls.Add(pnl)

        Next

    End Sub

    Private Sub ScriviDatiIniziali()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_GetQuestionario"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        If Not dbRdr.IsDBNull(0) Then

            ddnNumeroDomande.SelectedValue = dbRdr.GetInt32(0).ToString
            ddnMinimoRisposte.SelectedValue = dbRdr.GetInt32(1).ToString



            Dim sSequenza = dbRdr.GetString(2)
            For i = 1 To CInt(ddnNumeroDomande.SelectedValue)
                textboxes(i).Text = Mid(sSequenza, i, 1)
            Next


        End If


        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Private Sub VisibilitaControlliRisposte()

        Dim i As Integer

        Dim nRisp As Integer
        If ddnNumeroDomande.SelectedValue = "" Then
            nRisp = 0
        Else
            nRisp = CInt(ddnNumeroDomande.SelectedValue)
        End If

        For i = 1 To MaxRisposte
            panels(i).Visible = (i <= nRisp)
        Next
    End Sub

    Private Function SalvaDati() As Boolean
        Dim result = False

        If ValidaDati() Then

            Dim nDomande As Integer = CInt(ddnNumeroDomande.SelectedValue)
            Dim minRisposte As Integer = CInt(ddnMinimoRisposte.SelectedValue)

            Dim sSequenza As String = ""
            For i = 1 To nDomande
                sSequenza &= textboxes(i).Text
            Next

            Dim dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_eve_AddUpdateQuestionario"
                .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
                .Parameters.Add("@ni_risposte", SqlDbType.Int).Value = nDomande
                .Parameters.Add("@ni_minimorisposte", SqlDbType.Int).Value = minRisposte
                .Parameters.Add("@ac_sequenzarisposte", SqlDbType.NVarChar, 200).Value = sSequenza
                .Parameters.Add("@tx_creazionemodificaquestionario", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
            End With
            dbCmd.ExecuteNonQuery()
            dbCmd.Dispose()
            result = True
        End If

        Return result

    End Function

    Private Sub lnkSalvaChiudi_Click(sender As Object, e As System.EventArgs) Handles lnkSalvaChiudi.Click
        If SalvaDati() Then
            Response.Redirect("HomeEvento.aspx")
        End If
    End Sub

    Private Sub lnkSalva_Click(sender As Object, e As System.EventArgs) Handles lnkSalva.Click
        Dim dummy = SalvaDati()
    End Sub

    Private Sub lnkChiudi_Click(sender As Object, e As System.EventArgs) Handles lnkChiudi.Click
        Response.Redirect("HomeEvento.aspx")
    End Sub

    Private Function ValidaDati() As Boolean

        Dim errList As New List(Of String)
        Dim i As Integer
        Dim nDomande As Integer
        Dim minRisposte As Integer

        'pulizie
        ddnMinimoRisposte.BackColor = Drawing.Color.Empty
        ddnNumeroDomande.BackColor = Drawing.Color.Empty
        For i = 1 To MaxRisposte
            textboxes(i).Text = textboxes(i).Text.Trim.ToLower
            textboxes(i).BackColor = Drawing.Color.Empty
        Next

        If ddnNumeroDomande.SelectedValue = "" Then
            nDomande = 0
        Else
            nDomande = CInt(ddnNumeroDomande.SelectedValue)
        End If

        If ddnMinimoRisposte.SelectedValue = "" Then
            minRisposte = 0
        Else
            minRisposte = CInt(ddnMinimoRisposte.SelectedValue)
        End If

        'ci deve essere qualcosa!
        If nDomande = 0 And minRisposte = 0 Then
            ddnMinimoRisposte.BackColor = Drawing.Color.Yellow
            ddnNumeroDomande.BackColor = Drawing.Color.Yellow
            errList.Add("Il questionario è vuoto.")
        End If

        'verifico che ci siano entrambe o nessuna
        If nDomande = 0 And minRisposte > 0 Then
            ddnMinimoRisposte.BackColor = Drawing.Color.Yellow
            errList.Add("E\' stato indicato un numero minimo di risposte per il superamento del questionario, ma non è stata inserita nessuna risposta.")
        End If
        If nDomande > 0 And minRisposte = 0 Then
            ddnMinimoRisposte.BackColor = Drawing.Color.Yellow
            errList.Add("Non e\' stato indicato il numero minimo di risposte esatte per il superamento del questionario.")
        End If
        'verifico i minimi
        If (minRisposte > nDomande) And (minRisposte > 0) And (nDomande > 0) Then
            ddnMinimoRisposte.BackColor = Drawing.Color.Yellow
            ddnNumeroDomande.BackColor = Drawing.Color.Yellow
            errList.Add("Il numero di risposte esatte è maggiore del numero di domande.")
        End If

        'verifico i testi
        Dim allDomandeOk = True
        For i = 1 To nDomande
            Dim tDomanda As Char
            If textboxes(i).Text = "" Then
                tDomanda = " "c
            Else
                tDomanda = textboxes(i).Text.Chars(0)
            End If
            If Not (tDomanda >= "a"c And tDomanda <= "z"c) Then
                textboxes(i).BackColor = Drawing.Color.Yellow
                allDomandeOk = False
            End If
        Next
        If Not allDomandeOk Then
            errList.Add("Per una o più domande non si è specificata la risposta esatta o non si è inserita una lettera (a-z).")
        End If

        If errList.Count > 0 Then
            Dim errMsg = "Impossibile salvare a causa dei seguenti errori (controlla i campi in giallo):\n\n"
            For Each erritem In errList
                errMsg &= " - " & erritem & "\n"
            Next
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "whattadoing", "window.alert('" & errMsg & "');", True)
        End If

        Return errList.Count = 0


    End Function

    Private Sub DefinizioneQuestionarioECMGF_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub ddnNumeroDomande_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddnNumeroDomande.SelectedIndexChanged
        If ddnNumeroDomande.SelectedValue = "" Then
            ddnMinimoRisposte.SelectedValue = ""
        Else
            Dim numTot = CDec(CInt(ddnNumeroDomande.SelectedValue))
            Dim numMin = numTot * 0.75D
            numMin = Decimal.Ceiling(numMin)
            ddnMinimoRisposte.SelectedValue = CInt(numMin).ToString
        End If
    End Sub

End Class