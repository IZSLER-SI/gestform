Public Class CorrezioneQuestionarioApprendimento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Dim id_ISCRITTO As Integer = 0
    Dim ac_SEQUENZAUTENTE As String = ""
    Dim ac_SEQUENZAOK As String = ""
    Dim ni_DOMANDE As Integer = 0
    Dim ni_MINIMOESATTE As Integer = 0

    Private panels As New Dictionary(Of Integer, Panel)
    Private textboxes As New Dictionary(Of Integer, TextBox)
    Private Const MaxRisposte = 200

    Private Sub CorrezioneQuestionarioApprendimento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'chiamata per gestione ACL: verifica le autorizzazioni di accesso alla pagina e imposta expires=0
        Dim canAccess = AclHelper.AclInitForPagesWithoutMasterPage_FA(Server, Request, Response, True)

        If Not canAccess.canAccess Then Exit Sub

        Try
            id_ISCRITTO = CInt(Request("id"))
        Catch ex As Exception
        End Try

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'lettura dati globali
        Inizializzazione()

        'creazione controlli
        CreaControlliRisposte()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'se siamo in apertura scrivo i dati
        If Not Page.IsPostBack Then
            ScriviDatiIniziali()
            textboxes(1).Focus()
        End If

    End Sub

    Private Sub ScriviDatiIniziali()

        If ac_SEQUENZAUTENTE <> "" Then
            Dim mySeq As String
            'normalizzo
            mySeq = Left(ac_SEQUENZAUTENTE & Space(MaxRisposte), ni_DOMANDE)
            For i = 1 To ni_DOMANDE
                If Mid(ac_SEQUENZAUTENTE, i, 1) <> " " Then
                    textboxes(i).Text = Mid(ac_SEQUENZAUTENTE, i, 1)
                End If
            Next
            Correggi(True)
        Else
            Correggi(False)
        End If

    End Sub

    Private Sub Correggi(hiliteCells As Boolean)

        lbl_MinimoEsatte.Text = ni_MINIMOESATTE.ToString

        'costruzione della mia sequenza e contestuale correzione
        Dim nEsatte As Integer = 0
        Dim nNonDate As Integer = 0
        Dim nErrate As Integer = 0

        For i = 1 To ni_DOMANDE
            'pulizia
            textboxes(i).Text = textboxes(i).Text.Trim.ToLower
            'reset style
            textboxes(i).CssClass = "tri"
            If textboxes(i).Text = "" Then
                'non data
                nNonDate += 1
                If hiliteCells Then textboxes(i).CssClass &= " nondata"
            Else
                If textboxes(i).Text = Mid(ac_SEQUENZAOK, i, 1) Then
                    nEsatte += 1
                    If hiliteCells Then textboxes(i).CssClass &= " esatta"
                Else
                    nErrate += 1
                    If hiliteCells Then textboxes(i).CssClass &= " errata"
                End If
            End If
        Next
        lbl_Esatte.Text = nEsatte.ToString
        lbl_Errate.Text = nErrate.ToString
        lbl_NonDate.Text = nNonDate.ToString
        If hiliteCells Then
            If nEsatte >= ni_MINIMOESATTE Then
                lbl_Esito.Text = "Questionario superato"
                lbl_Esito.CssClass = "esito esitoOK"
            Else
                lbl_Esito.Text = "Questionario non superato"
                lbl_Esito.CssClass = "esito esitoKO"
            End If
        Else
            lbl_Esito.Text = "Questionario da correggere"
            lbl_Esito.CssClass = "esito esitoNA"
        End If



    End Sub

    Private Sub CreaControlliRisposte()

        Dim pnl As Panel
        Dim txt As TextBox

        phdRisposte.Controls.Clear()
        panels.Clear()
        textboxes.Clear()
        Dim i As Integer
        For i = 1 To ni_DOMANDE

            pnl = New Panel
            panels.Add(i, pnl)
            pnl.ID = "rp" & i
            pnl.CssClass = "pdo"
            pnl.EnableViewState = True

            pnl.Controls.Add(New LiteralControl("<div class=""dom"">domanda</div>"))
            pnl.Controls.Add(New LiteralControl("<div class=""ndo""><b>" & i.ToString & "</b> (" & Mid(ac_SEQUENZAOK, i, 1) & ")</div>"))

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

    Private Sub CorrezioneQuestionarioApprendimento_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub Inizializzazione()
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'lettura dati
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT * FROM vw_eve_ISCRITTI WHERE id_ISCRITTO=@id_ISCRITTO AND id_EVENTO=@id_EVENTO"
            .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        ltrTitolo.Text = "Correzione Questionario Apprendimento di " & dbRdr.GetString(dbRdr.GetOrdinal("tx_COGNOME")) & " " & dbRdr.GetString(dbRdr.GetOrdinal("tx_NOME"))
        hidMinRisposte.Value = dbRdr.GetInt32(dbRdr.GetOrdinal("ni_MINIMORISPOSTE")).ToString
        hidNumDomande.Value = dbRdr.GetInt32(dbRdr.GetOrdinal("ni_RISPOSTE")).ToString
        hidSeqRisposte.Value = dbRdr.GetString(dbRdr.GetOrdinal("ac_SEQUENZARISPOSTEESATTE"))
        If Not dbRdr.IsDBNull(dbRdr.GetOrdinal("ac_SEQUENZARISPOSTE")) Then
            ac_SEQUENZAUTENTE = dbRdr.GetString(dbRdr.GetOrdinal("ac_SEQUENZARISPOSTE"))
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'scrivo nelle variabili interne
        ac_SEQUENZAOK = hidSeqRisposte.Value
        ni_DOMANDE = CInt(hidNumDomande.Value)
        ni_MINIMOESATTE = CInt(hidMinRisposte.Value)

    End Sub

    Private Sub lnkRicalcola_Click(sender As Object, e As System.EventArgs) Handles lnkRicalcola.Click
        Correggi(True)
    End Sub

    Private Sub lnkSalva_Click(sender As Object, e As System.EventArgs) Handles lnkSalva.Click
        Correggi(True)
        SaveMe()
    End Sub

    Private Sub lnkSalvaChiudi_Click(sender As Object, e As System.EventArgs) Handles lnkSalvaChiudi.Click
        Correggi(True)
        SaveMe()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('refresh');", True)
    End Sub

    Private Sub lnkChiudi_Click(sender As Object, e As System.EventArgs) Handles lnkChiudi.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('');", True)
    End Sub

    Private Sub SaveMe()

        'lettura sequenza
        Dim mySeq As String = ""

        For i = 1 To ni_DOMANDE
            If textboxes(i).Text.Trim = "" Then
                'non data
                mySeq &= " "
            Else
                mySeq &= textboxes(i).Text.Trim.ToLower
            End If
        Next

        'salvataggio vero e proprio
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_AddUpdateQuestionarioIscritto"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@ac_SEQUENZARISPOSTE", SqlDbType.NVarChar, 200).Value = mySeq
            .Parameters.Add("@dt_CORREZIONEQUESTIONARIO", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@tx_UTENTECORREZIONEQUESTIONARIO", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()


    End Sub
End Class