Public Class GestioneQuestionarioQualita
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Dim id_QUESTIONARIOQUALITA As Integer = 0
    Dim ni_QUESTIONARIOQUALITA As Integer = 0
    Dim fl_SPONSOR As Boolean = False

    'controlli
    Protected WithEvents ni_RILEVANZA As TextBox
    Protected WithEvents ni_QUALITA As TextBox
    Protected WithEvents ni_UTILITA As TextBox
    Protected WithEvents ni_INFLUENZASPONSOR As TextBox
    Protected WithEvents tx_INFLUENZASPONSOR As TextBox
    Protected WithEvents ni_CAPACITAESPOSIZIONE As List(Of TextBox)
    Protected WithEvents ni_SODDISFAZIONE As TextBox
    Protected WithEvents ni_MATERIALE As TextBox
    Protected WithEvents ni_INFRASTRUTTURE As TextBox
    Protected WithEvents ni_CONSIGLIACOLLEGHI As TextBox
    Protected WithEvents ni_PROBLEMIORARIO As TextBox
    Protected WithEvents ni_FREQUENTAALTRICORSI As TextBox
    Protected WithEvents tx_FREQUENTAALTRICORSI As TextBox
    Protected WithEvents tx_COMMENTI As TextBox
    Protected WithEvents id_ISCRITTO As DropDownList

    Private Sub GestioneQuestionarioQualita_Init(sender As Object, e As EventArgs) Handles Me.Init

        'chiamata per gestione ACL: verifica le autorizzazioni di accesso alla pagina e imposta expires=0
        Dim canAccess = AclHelper.AclInitForPagesWithoutMasterPage_FA(Server, Request, Response, True)

        If Not canAccess.canAccess Then Exit Sub

        Try
            id_QUESTIONARIOQUALITA = CInt(Request("id"))
        Catch ex As Exception
            id_QUESTIONARIOQUALITA = 0
        End Try

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'creazione controlli
        CreaControlliRisposte()

        'titolo
        If id_QUESTIONARIOQUALITA = 0 Then
            Me.ltrTitolo.Text = "Immissione Questionario Qualità Percepita"
        Else
            Me.ltrTitolo.Text = "Modifica Questionario Qualità Percepita n° " & ni_QUESTIONARIOQUALITA.ToString
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ni_RILEVANZA.Attributes.Add("onkeyup", "gonext(this,'" & ni_QUALITA.ClientID & "');")
        ni_QUALITA.Attributes.Add("onkeyup", "gonext(this,'" & ni_UTILITA.ClientID & "');")
        ni_UTILITA.Attributes.Add("onkeyup", "gonext(this,'" & ni_INFLUENZASPONSOR.ClientID & "');")
        ni_INFLUENZASPONSOR.Attributes.Add("onkeyup", "gonext(this,'" & tx_INFLUENZASPONSOR.ClientID & "');")
        'ciclo sui relatori
        If ni_CAPACITAESPOSIZIONE.Count > 0 Then
            'al primo
            ni_PROBLEMIORARIO.Attributes.Add("onkeyup", "gonext(this,'" & ni_CAPACITAESPOSIZIONE(0).ClientID & "');")
            'successivi
            For i = 0 To ni_CAPACITAESPOSIZIONE.Count - 2
                ni_CAPACITAESPOSIZIONE(i).Attributes.Add("onkeyup", "gonext(this,'" & ni_CAPACITAESPOSIZIONE(i + 1).ClientID & "');")
            Next
            'dall'ultimo
            ni_CAPACITAESPOSIZIONE(ni_CAPACITAESPOSIZIONE.Count - 1).Attributes.Add("onkeyup", "gonext(this,'" & ni_SODDISFAZIONE.ClientID & "');")
        End If

        ni_SODDISFAZIONE.Attributes.Add("onkeyup", "gonext(this,'" & ni_MATERIALE.ClientID & "');")
        ni_MATERIALE.Attributes.Add("onkeyup", "gonext(this,'" & ni_INFRASTRUTTURE.ClientID & "');")
        ni_INFRASTRUTTURE.Attributes.Add("onkeyup", "gonext(this,'" & ni_CONSIGLIACOLLEGHI.ClientID & "');")
        ni_CONSIGLIACOLLEGHI.Attributes.Add("onkeyup", "gonext(this,'" & ni_PROBLEMIORARIO.ClientID & "');")
        ni_PROBLEMIORARIO.Attributes.Add("onkeyup", "gonext(this,'" & ni_FREQUENTAALTRICORSI.ClientID & "');")
        ni_FREQUENTAALTRICORSI.Attributes.Add("onkeyup", "gonext(this,'" & tx_FREQUENTAALTRICORSI.ClientID & "');")

        If Not Page.IsPostBack Then
            ni_RILEVANZA.Focus()
        End If

    End Sub

    Private Sub CreaControlliRisposte()

        Dim dbCmd As SqlCommand
        Dim qXDoc As XmlDocument
        Dim xReader As XmlReader

        dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_GetQuestionarioQualita"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_questionarioqualita", SqlDbType.Int).Value = id_QUESTIONARIOQUALITA
        End With
        xReader = dbCmd.ExecuteXmlReader
        qXDoc = New XmlDocument
        qXDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        'determino flag sponsor
        fl_SPONSOR = qXDoc.SelectSingleNode("/evento").Attributes("tx_sponsor") IsNot Nothing
        If qXDoc.SelectSingleNode("/evento").Attributes("ni_questionarioqualita") IsNot Nothing Then
            ni_QUESTIONARIOQUALITA = CInt(qXDoc.SelectSingleNode("/evento").Attributes("ni_questionarioqualita").Value)
        Else
            ni_QUESTIONARIOQUALITA = 0
        End If
        Dim sAspx = Transformer.Transform(qXDoc, "Templates/ImmissioneQuestionarioQualita.xslt", "dummy", "dummy")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")

        phdRisposte.Controls.Clear()
        Dim ccreato = Page.ParseControl(sAspx)
        phdRisposte.Controls.Add(ccreato)

        'aggancio dei controlli
        ni_RILEVANZA = CType(ccreato.FindControl("ni_RILEVANZA"), TextBox)
        ni_QUALITA = CType(ccreato.FindControl("ni_QUALITA"), TextBox)
        ni_UTILITA = CType(ccreato.FindControl("ni_UTILITA"), TextBox)
        ni_INFLUENZASPONSOR = CType(ccreato.FindControl("ni_INFLUENZASPONSOR"), TextBox)
        tx_INFLUENZASPONSOR = CType(ccreato.FindControl("tx_INFLUENZASPONSOR"), TextBox)

        'relatori
        ni_CAPACITAESPOSIZIONE = New List(Of TextBox)

        For Each c As Control In ccreato.Controls
            If TypeOf c Is TextBox Then
                Dim tBox = CType(c, TextBox)
                If tBox.ID Like "ni_CAPACITAESPOSIZIONE_*" Then
                    ni_CAPACITAESPOSIZIONE.Add(tBox)
                End If
            End If
        Next

        ni_SODDISFAZIONE = CType(ccreato.FindControl("ni_SODDISFAZIONE"), TextBox)
        ni_MATERIALE = CType(ccreato.FindControl("ni_MATERIALE"), TextBox)
        ni_INFRASTRUTTURE = CType(ccreato.FindControl("ni_INFRASTRUTTURE"), TextBox)
        ni_CONSIGLIACOLLEGHI = CType(ccreato.FindControl("ni_CONSIGLIACOLLEGHI"), TextBox)
        ni_PROBLEMIORARIO = CType(ccreato.FindControl("ni_PROBLEMIORARIO"), TextBox)
        ni_FREQUENTAALTRICORSI = CType(ccreato.FindControl("ni_FREQUENTAALTRICORSI"), TextBox)
        tx_FREQUENTAALTRICORSI = CType(ccreato.FindControl("tx_FREQUENTAALTRICORSI"), TextBox)
        tx_COMMENTI = CType(ccreato.FindControl("tx_COMMENTI"), TextBox)
        id_ISCRITTO = CType(ccreato.FindControl("id_ISCRITTO"), DropDownList)

    End Sub

    Private Sub GestioneQuestionarioQualita_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub lnkChiudi_Click(sender As Object, e As System.EventArgs) Handles lnkChiudi.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('');", True)
    End Sub

    Private Sub lnkSalvaChiudi_Click(sender As Object, e As System.EventArgs) Handles lnkSalvaChiudi.Click

        Dim saveResult = SaveMe()

        If saveResult.ni_QUESTIONARIOQUALITA = -1 Then
            'nominativo già presente
            id_ISCRITTO.BackColor = Drawing.Color.Yellow
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "nonfunge", "window.alert('Esiste già un questionario relativo al nominativo selezionato.');", True)
        Else
            'ok, salvato
            If id_QUESTIONARIOQUALITA = 0 Then
                'era nuovo
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "window.alert('Il questionario è stato salvato.\nAl questionario è stato attribuito il numero\n\n" & saveResult.ni_QUESTIONARIOQUALITA & "\n\nTI RACCOMANDIAMO DI ANNOTARE IL NUMERO SULLA COPIA CARTACEA.');parent.stl_sel_done('" & saveResult.id_QUESTIONARIOQUALITA.ToString & "');", True)
            Else
                'update
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('" & saveResult.id_QUESTIONARIOQUALITA.ToString & "');", True)
            End If
        End If

    End Sub

    Private Class SaveResult
        Public id_QUESTIONARIOQUALITA As Integer = 0
        Public ni_QUESTIONARIOQUALITA As Integer = -1
    End Class

    Private Function SaveMe() As SaveResult

        Dim result As New SaveResult
        Dim dbCmd As SqlCommand
        Dim prmNum As SqlParameter
        Dim prmId As SqlParameter


        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_AddUpdateQuestionarioQualita"

            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            With .Parameters.Add("@id_QUESTIONARIOQUALITA", SqlDbType.Int)
                If id_QUESTIONARIOQUALITA = 0 Then .Value = DBNull.Value Else .Value = id_QUESTIONARIOQUALITA
            End With

            .Parameters.Add("@dt_DATAORA", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@tx_UTENTE", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME

            With .Parameters.Add("@id_ISCRITTO", SqlDbType.Int)
                If id_ISCRITTO.SelectedValue = "" Then .Value = DBNull.Value Else .Value = CInt(id_ISCRITTO.SelectedValue)
            End With

            .Parameters.Add("@ni_RILEVANZA", SqlDbType.Int).Value = Read15(ni_RILEVANZA)
            .Parameters.Add("@ni_QUALITA", SqlDbType.Int).Value = Read15(ni_QUALITA)
            .Parameters.Add("@ni_UTILITA", SqlDbType.Int).Value = Read15(ni_UTILITA)

            With .Parameters.Add("@ni_INFLUENZASPONSOR", SqlDbType.Int)
                If fl_SPONSOR Then
                    .Value = Read15(ni_INFLUENZASPONSOR)
                Else
                    .Value = DBNull.Value
                End If
            End With
            With .Parameters.Add("@tx_INFLUENZASPONSOR", SqlDbType.NVarChar, -1)
                If fl_SPONSOR Then
                    .Value = ReadMultiLine(tx_INFLUENZASPONSOR)
                Else
                    .Value = DBNull.Value
                End If
            End With

            .Parameters.Add("@ni_PROBLEMIORARIO", SqlDbType.Int).Value = ReadSN(ni_PROBLEMIORARIO)
            .Parameters.Add("@ni_INFRASTRUTTURE", SqlDbType.Int).Value = Read15(ni_INFRASTRUTTURE)
            .Parameters.Add("@ni_CONSIGLIACOLLEGHI", SqlDbType.Int).Value = ReadSN(ni_CONSIGLIACOLLEGHI)
            .Parameters.Add("@ni_FREQUENTAALTRICORSI", SqlDbType.Int).Value = ReadSN(ni_FREQUENTAALTRICORSI)
            .Parameters.Add("@tx_FREQUENTAALTRICORSI", SqlDbType.NVarChar, -1).Value = ReadMultiLine(tx_FREQUENTAALTRICORSI)
            .Parameters.Add("@tx_COMMENTI", SqlDbType.NVarChar, -1).Value = ReadMultiLine(tx_COMMENTI)
            .Parameters.Add("@ni_TEMPO", SqlDbType.Int).Value = DBNull.Value
            .Parameters.Add("@ni_SODDISFAZIONE", SqlDbType.Int).Value = Read15(ni_SODDISFAZIONE)
            .Parameters.Add("@ni_MATERIALE", SqlDbType.Int).Value = Read15(ni_MATERIALE)

            Dim dataTable = New Softailor.Global.StructuredUtils.GenericIntStringStringIntList

            For Each tBox In ni_CAPACITAESPOSIZIONE
                If tBox.Text.Trim <> "" Then
                    Dim issi As New Softailor.Global.StructuredUtils.IntStringStringInt
                    issi.int1 = CInt(tBox.ID.Split("_"c)(2))
                    issi.int2 = CInt(tBox.Text)
                    dataTable.Add(issi)
                End If
            Next

            .Parameters.Add("@valutazionirelatori", SqlDbType.Structured).Value = dataTable.GetTable

            prmNum = .Parameters.Add("@out_ni_QUESTIONARIOQUALITA", SqlDbType.Int)
            prmNum.Direction = ParameterDirection.Output

            prmId = .Parameters.Add("@out_id_QUESTIONARIOQUALITA", SqlDbType.Int)
            prmId.Direction = ParameterDirection.Output

        End With

        dbCmd.ExecuteNonQuery()
        result.id_QUESTIONARIOQUALITA = CInt(prmId.Value)
        result.ni_QUESTIONARIOQUALITA = CInt(prmNum.Value)
        dbCmd.Dispose()


        Return result

    End Function

    Private Function Read15(txt As TextBox) As Object

        If txt.Text.Trim = "" Then
            Return DBNull.Value
        Else
            Return CInt(txt.Text)
        End If

    End Function

    Private Function ReadMultiLine(txt As TextBox) As Object
        Dim sContent As String = txt.Text.Trim

        Do While Left(sContent, 1) = vbCr Or Left(sContent, 1) = vbLf
            sContent = Mid(sContent, 2)
        Loop

        Do While Right(sContent, 1) = vbCr Or Right(sContent, 1) = vbLf
            sContent = Mid(sContent, 1, Len(sContent) - 1)
        Loop

        If sContent = "" Then
            Return DBNull.Value
        Else
            Return sContent
        End If

    End Function

    Private Function ReadSN(txt As TextBox) As Object

        Dim t = txt.Text.Trim.ToUpper

        Select Case t
            Case "S" : Return 1
            Case "N" : Return 0
            Case Else : Return DBNull.Value
        End Select

    End Function
End Class