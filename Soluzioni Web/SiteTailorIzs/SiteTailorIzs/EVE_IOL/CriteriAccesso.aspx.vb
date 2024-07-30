Imports Softailor.Global.ValidationUtils
Imports Softailor.Global.AspxCleaner

Public Class CriteriAccesso
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Const errRequired = "Campo richiesto"
    Private Const errInvalidDate = "Data non valida"
    Private bgErr As Drawing.Color = Drawing.Color.Yellow
    Private bgOk As Drawing.Color = Drawing.Color.Empty

    Dim dbConn As SqlConnection

    Private Sub CriteriAccesso_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'gestione degli script
        Dim sScript = "function editCriterio_callback(codice) { if(codice!='') {" & vbCrLf
        sScript &= ClientScript.GetPostBackClientHyperlink(lnkRefreshCriteri, "").Replace("javascript:", "")
        sScript &= "}}" & vbCrLf
        Me.ltrRepositioning.Text = sScript

        'generazione controlli criteri
        GeneraControlliCriteri()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'dati controlli
        If Not Page.IsPostBack Then
            WriteHeader()
        End If
    End Sub

    Private Sub CriteriAccesso_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraControlliCriteri()

        Dim dbCmd As SqlCommand
        Dim cCreato As Control

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_CriteriAccessoXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Dim sAspx = Transformer.Transform(dbCmd, "Templates/CriteriAccesso_Criteri.xslt", "companyname", My.Settings.CompanyName)
        CleanAspx(sAspx)

        cCreato = Me.ParseControl(sAspx)

        phdCriteri.Controls.Clear()
        phdCriteri.Controls.Add(cCreato)

        For Each c As Control In cCreato.Controls
            If TypeOf c Is LinkButton Then
                With CType(c, LinkButton)
                    If .ID Like "lnkEliminaCriterio_*" Then
                        AddHandler .Click, AddressOf lnkEliminaCriterio_Click
                    ElseIf .ID Like "lnkMoveUpCriterio_*" Then
                        AddHandler .Click, AddressOf lnkMoveUpCriterio_Click
                    ElseIf .ID Like "lnkMoveDnCriterio_*" Then
                        AddHandler .Click, AddressOf lnkMoveDnCriterio_Click
                    End If
                End With
            End If
        Next

    End Sub

    Private Sub lnkEliminaCriterio_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_DeleteCriterio"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_CRITERIO", SqlDbType.Int).Value = CInt(CType(sender, LinkButton).CommandArgument)
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        GeneraControlliCriteri()

    End Sub

    Private Sub lnkMoveUpCriterio_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_MoveCriterio"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_CRITERIO", SqlDbType.Int).Value = CInt(CType(sender, LinkButton).CommandArgument)
            .Parameters.Add("@ac_DIREZIONE", SqlDbType.NVarChar, 4).Value = "UP"
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        GeneraControlliCriteri()

    End Sub

    Private Sub lnkMoveDnCriterio_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_MoveCriterio"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_CRITERIO", SqlDbType.Int).Value = CInt(CType(sender, LinkButton).CommandArgument)
            .Parameters.Add("@ac_DIREZIONE", SqlDbType.NVarChar, 4).Value = "DOWN"
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        GeneraControlliCriteri()

    End Sub


    Private Sub lnkRefreshCriteri_Click(sender As Object, e As EventArgs) Handles lnkRefreshCriteri.Click
        'generazione controlli criteri
        GeneraControlliCriteri()
    End Sub

    Private Sub lnkSaveHeader_Click(sender As Object, e As EventArgs) Handles lnkSaveHeader.Click
        If ValidateHeader() Then
            SaveHeader()

            'generazione controlli criteri
            GeneraControlliCriteri()
        End If
    End Sub

    Private Function ValidateHeader() As Boolean

        Dim valid = True

        Dim iniPres = False
        Dim iniOk = False
        Dim finPres = False
        Dim finOk = False

        'pulizie iniziali
        iol_dt_INIZIOVISIBILITA.BackColor = bgOk
        iol_dt_APERTURAISCRIZIONI.BackColor = bgOk
        iol_dt_CHIUSURAISCRIZIONI.BackColor = bgOk
        iol_ni_MAXPARTECIPANTI.BackColor = bgOk

        'validazioni formali
        If iol_dt_INIZIOVISIBILITA.Text <> "" Then
            If Not ValidateItalianDate(iol_dt_INIZIOVISIBILITA.Text) Then
                valid = False
                iol_dt_INIZIOVISIBILITA.BackColor = bgErr
                erriol_dt_INIZIOVISIBILITA.Text = errInvalidDate
            End If
        End If

        If iol_dt_APERTURAISCRIZIONI.Text <> "" Then
            iniPres = True
            If Not ValidateItalianDate(iol_dt_APERTURAISCRIZIONI.Text) Then
                valid = False
                iol_dt_APERTURAISCRIZIONI.BackColor = bgErr
                erriol_dt_APERTURAISCRIZIONI.Text = errInvalidDate
            Else
                iniOk = True
            End If
        End If

        If iol_dt_CHIUSURAISCRIZIONI.Text <> "" Then
            finPres = True
            If Not ValidateItalianDate(iol_dt_CHIUSURAISCRIZIONI.Text) Then
                valid = False
                iol_dt_CHIUSURAISCRIZIONI.BackColor = bgErr
                erriol_dt_CHIUSURAISCRIZIONI.Text = errInvalidDate
            Else
                finOk = True
            End If
        End If

        'entrambe le date
        If (iniPres And iniOk And (Not finPres)) Then
            valid = False
            iol_dt_CHIUSURAISCRIZIONI.BackColor = bgErr
            erriol_dt_CHIUSURAISCRIZIONI.Text = "Indica sia la data di apertura sia la data di chiusura iscrizioni, oppure nessuna delle due."
        End If

        If ((Not iniPres) And finPres And finOk) Then
            valid = False
            iol_dt_APERTURAISCRIZIONI.BackColor = bgErr
            erriol_dt_APERTURAISCRIZIONI.Text = "Indica sia la data di apertura sia la data di chiusura iscrizioni, oppure nessuna delle due."
        End If

        'numero iscritti
        iol_ni_MAXPARTECIPANTI.Text = iol_ni_MAXPARTECIPANTI.Text.Trim
        If iol_ni_MAXPARTECIPANTI.Text = String.Empty Then
            'numero partecipanti non presente
            'errore se ho almeno un criterio con ac_DESTINAZIONE = 'Q2'

            Dim dbCmd As SqlCommand = dbConn.CreateCommand
            Dim dbRdr As SqlDataReader
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT TOP 1 id_CRITERIO FROM iol_CRITERI WHERE id_EVENTO = @id_EVENTO AND ac_DESTINAZIONE = 'Q2'"
                .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            End With
            dbRdr = dbCmd.ExecuteReader
            If dbRdr.Read Then
                valid = False
                iol_ni_MAXPARTECIPANTI.BackColor = bgErr
                erriol_ni_MAXPARTECIPANTI.Text = "Non puoi rendere l'evento a numero non-chiuso in quanto esistono criteri di accettazione in lista d'attesa secondaria."
            End If
            dbRdr.Close()
            dbCmd.Dispose()

        Else
            If Not ValidateInteger(iol_ni_MAXPARTECIPANTI.Text) Then
                valid = False
                iol_ni_MAXPARTECIPANTI.BackColor = bgErr
                erriol_ni_MAXPARTECIPANTI.Text = "Numero non valido"
            Else
                'ok valido
                If CInt(iol_ni_MAXPARTECIPANTI.Text) <= 0 Then
                    valid = False
                    iol_ni_MAXPARTECIPANTI.BackColor = bgErr
                    erriol_ni_MAXPARTECIPANTI.Text = "Immetti un numero maggiore o uguale a 1."
                Else
                    'ok ci siamo
                    'non verifico nulla: può esserci Q2

                End If
            End If
        End If

        Return valid

    End Function

    Private Sub SaveHeader()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE	eve_EVENTI " &
                            "SET	dt_MODIFICA = GETDATE(), " &
                            "		tx_MODIFICA = @tx_MODIFICA,	" &
                            "		iol_dt_INIZIOVISIBILITA = @iol_dt_INIZIOVISIBILITA, " &
                            "		iol_dt_APERTURAISCRIZIONI = @iol_dt_APERTURAISCRIZIONI, " &
                            "		iol_dt_CHIUSURAISCRIZIONI = @iol_dt_CHIUSURAISCRIZIONI, " &
                            "		iol_ni_MAXPARTECIPANTI = @iol_ni_MAXPARTECIPANTI " &
                            "WHERE	id_EVENTO = @id_EVENTO"
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME

            With .Parameters.Add("@iol_dt_INIZIOVISIBILITA", SqlDbType.DateTime)
                If iol_dt_INIZIOVISIBILITA.Text = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = ParseItalianDate(iol_dt_INIZIOVISIBILITA.Text)
                End If
            End With

            With .Parameters.Add("@iol_dt_APERTURAISCRIZIONI", SqlDbType.DateTime)
                If iol_dt_APERTURAISCRIZIONI.Text = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = ParseItalianDate(iol_dt_APERTURAISCRIZIONI.Text)
                End If
            End With

            With .Parameters.Add("@iol_dt_CHIUSURAISCRIZIONI", SqlDbType.DateTime)
                If iol_dt_CHIUSURAISCRIZIONI.Text = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = ParseItalianDate(iol_dt_CHIUSURAISCRIZIONI.Text)
                End If
            End With

            With .Parameters.Add("@iol_ni_MAXPARTECIPANTI", SqlDbType.Int)
                If iol_ni_MAXPARTECIPANTI.Text = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = CInt(iol_ni_MAXPARTECIPANTI.Text)
                End If
            End With

            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

    End Sub

    Private Sub WriteHeader()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT " &
                            "CAST(iol_dt_INIZIOVISIBILITA as datetime) as iol_dt_INIZIOVISIBILITA, " &
                            "CAST(iol_dt_APERTURAISCRIZIONI as datetime) as iol_dt_APERTURAISCRIZIONI, " &
                            "CAST(iol_dt_CHIUSURAISCRIZIONI as datetime) as iol_dt_CHIUSURAISCRIZIONI, " &
                            "iol_ni_MAXPARTECIPANTI FROM eve_EVENTI " &
                            "WHERE id_EVENTO = @id_EVENTO"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        If Not dbRdr.IsDBNull(0) Then iol_dt_INIZIOVISIBILITA.Text = FormatItalianDateY4(dbRdr.GetDateTime(0))
        If Not dbRdr.IsDBNull(1) Then iol_dt_APERTURAISCRIZIONI.Text = FormatItalianDateY4(dbRdr.GetDateTime(1))
        If Not dbRdr.IsDBNull(2) Then iol_dt_CHIUSURAISCRIZIONI.Text = FormatItalianDateY4(dbRdr.GetDateTime(2))
        If Not dbRdr.IsDBNull(3) Then iol_ni_MAXPARTECIPANTI.Text = dbRdr.GetInt32(3).ToString

        dbRdr.Close()
        dbCmd.Dispose()

    End Sub
End Class