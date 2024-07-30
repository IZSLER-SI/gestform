Imports Softailor.Global.MiscUtils

Public Class NuovoPartecipante
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection
    Dim ac_CATEGORIAECM As String

    Private Sub NuovoPartecipante_Init(sender As Object, e As EventArgs) Handles Me.Init

        If Request("t") Is Nothing Then
            CloseMe("")
            Exit Sub
        Else
            ac_CATEGORIAECM = Request("t")
        End If

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'titolo
        If Not Page.IsPostBack Then
            SetupTitolo()
        End If

        'riempimento drop down categorie lavorative e titoli (nuovo)
        RiempiDropDownNuoviElementi()

        'script per invocazione bottoni
        If Not Page.IsPostBack Then

            Dim sScript As String = ""

            sScript &= "function invokeIscriviEsistente() {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkIscriviEsistente, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            sScript &= "function invokeCreaIscriviNuovo() {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkCreaIscriviNuovo, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            Me.ltrScripts.Text = sScript
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        srcDOCENTI.SearchParameters.Add(0, " AND dt_scadenza_docente >= CONCAT(CONVERT(date, GETDATE()),' 00:00:00')")
        If Not Page.IsPostBack Then srcPERSONE.Focus()
    End Sub

    Private Sub CloseMe(value As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMe",
           "parent.stl_sel_done(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(value) & ");", True)
    End Sub

    Private Sub NuovoPartecipante_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        'chiusura connessione
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn = Nothing
        End If
    End Sub

    Private Sub SetupTitolo()
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT tx_CATEGORIAECM FROM eve_CATEGORIEECM WHERE ac_CATEGORIAECM=@ac_CATEGORIAECM"
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        lblTitle.Text = "Aggiunta " & dbRdr.GetString(0).ToLower
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Sub grdPERSONE_RowSelected(dataKeyName As String, dataKeyValue As String) Handles grdPERSONE.RowSelected

        If dataKeyValue <> "" Then

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "preAddEsistente",
                                "tryIscriviEsistente(" & GecFinalContextHandler.id_EVENTO.ToString & ", " &
                                dataKeyValue & ", " &
                                Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_CATEGORIAECM) &
                                ");", True)

        End If

    End Sub

		Private Sub grdDOCENTI_RowSelected(dataKeyName As String, dataKeyValue As String) Handles grdDOCENTI.RowSelected

				If dataKeyValue <> "" Then

						ScriptManager.RegisterStartupScript(Me, Me.GetType, "preAddEsistente",
								"tryIscriviEsistente(" & GecFinalContextHandler.id_EVENTO.ToString & ", " &
								dataKeyValue & ", " &
								Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_CATEGORIAECM) &
								");", True)

				End If

		End Sub

		Private Sub AddIt(id_PERSONA As Integer)

        Dim dbCmd = dbConn.CreateCommand
        Dim prmid_ISCRITTO As SqlParameter
        Dim newid_ISCRITTO As Integer = 0

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_IscriviPersona"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = id_PERSONA
            .Parameters.Add("@ac_categoriaecm", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM
            .Parameters.Add("@ac_origineiscrizione", SqlDbType.NVarChar, 8).Value = "BO"
            .Parameters.Add("@dt_creazione", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@tx_creazione", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
            prmid_ISCRITTO = .Parameters.Add("@id_iscritto", SqlDbType.Int)
            prmid_ISCRITTO.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        With CType(prmid_ISCRITTO.SqlValue, SqlInt32)
            If Not .IsNull Then
                newid_ISCRITTO = .Value
            End If
        End With
        dbCmd.Dispose()

        If newid_ISCRITTO = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert('Nominativo già presente nell\'elenco dei partecipanti con il ruolo specificato.');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "done", "parent.stl_sel_done('" & newid_ISCRITTO.ToString & "_SELECT');", True)
        End If

    End Sub

		Private Sub srcPERSONE_DoingSearch() Handles srcPERSONE.DoingSearch

				'copia dati
				Dim sqlCognome As SqlString = CType(srcPERSONE.GetValidSqlValue("tx_COGNOME"), SqlString)
				Dim sqlNome As SqlString = CType(srcPERSONE.GetValidSqlValue("tx_NOME"), SqlString)

				If Not sqlCognome.IsNull Then
						txtCognome.Text = sqlCognome.Value.Trim.ToUpper
				End If

				If Not sqlNome.IsNull Then
						txtNome.Text = sqlNome.Value.Trim.ToUpper
				End If

				ddnCategoriaLavorativa.SelectedValue = ""
				ddnProfilo.SelectedValue = ""
				updNuovo.Update()

		End Sub

		Private Sub srcDOCENTI_DoingSearch() Handles srcDOCENTI.DoingSearch

				'copia dati
				Dim sqlCognome As SqlString = CType(srcDOCENTI.GetValidSqlValue("tx_COGNOME"), SqlString)
				Dim sqlNome As SqlString = CType(srcDOCENTI.GetValidSqlValue("tx_NOME"), SqlString)

				If Not sqlCognome.IsNull Then
						txtCognome.Text = sqlCognome.Value.Trim.ToUpper
				End If

				If Not sqlNome.IsNull Then
						txtNome.Text = sqlNome.Value.Trim.ToUpper
				End If

				ddnCategoriaLavorativa.SelectedValue = ""
				ddnProfilo.SelectedValue = ""
				updNuovo.Update()

		End Sub

		Private Sub RiempiDropDownNuoviElementi()

        'titolo
        FillDropDown(ddnTitolo, dbConn, "SELECT ac_TITOLO, tx_TITOLO FROM age_TITOLI ORDER BY tx_TITOLO", True, True, True, True)

        'profilo (solo quelli validi per gli esterni)
        FillDropDown(ddnProfilo, dbConn, "SELECT ac_PROFILO, tx_PROFILO FROM age_PROFILI WHERE fl_ESTERNO=1 ORDER BY tx_PROFILO", True, True, True, True)

        'cat lavorative (tutte)
        FillDropDown(ddnCategoriaLavorativa, dbConn, "SELECT ac_CATEGORIALAVORATIVA as ac, tx_CATEGORIALAVORATIVA as tx FROM age_CATEGORIELAVORATIVE ORDER BY ni_ORDINE", True, True, True, True)

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim errorList As String = ""
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'pulizie
        ddnTitolo.BackColor = Drawing.Color.Empty
        txtCognome.BackColor = Drawing.Color.Empty
        txtNome.BackColor = Drawing.Color.Empty
        ddnProfilo.BackColor = Drawing.Color.Empty
        ddnCategoriaLavorativa.BackColor = Drawing.Color.Empty
        txtCodiceFiscale.BackColor = Drawing.Color.Empty

        txtCognome.Text = txtCognome.Text.Trim.ToUpper
        txtNome.Text = txtNome.Text.Trim.ToUpper
        txtCodiceFiscale.Text = txtCodiceFiscale.Text.Trim.ToUpper

        Dim isValid = True

        If txtCognome.Text = String.Empty Then
            txtCognome.BackColor = Drawing.Color.Yellow
            isValid = False
            errorList &= vbCrLf & "- cognome: dato obbligatorio"
        End If

        If txtNome.Text = String.Empty Then
            txtNome.BackColor = Drawing.Color.Yellow
            isValid = False
            errorList &= vbCrLf & "- nome: dato obbligatorio"
        End If

        'validazione codice fiscale
        If txtCodiceFiscale.Text <> "" Then
            If Not Softailor.Global.ValidationUtils.ValidateCodiceFiscaleItaliano(txtCodiceFiscale.Text) Then
                txtCodiceFiscale.BackColor = Drawing.Color.Yellow
                isValid = False
                errorList &= vbCrLf & "- codice fiscale: codice formalmente scorretto"
            Else
                'verifico presenza CF in archivio
                dbCmd = dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT TOP 1 id_PERSONA, tx_COGNOME, tx_NOME FROM age_PERSONE WHERE ac_CODICEFISCALE=@ac_CODICEFISCALE"
                    .Parameters.Add("@ac_CODICEFISCALE", SqlDbType.NVarChar, 16).Value = txtCodiceFiscale.Text
                End With
                dbRdr = dbCmd.ExecuteReader
                If dbRdr.Read Then
                    txtCodiceFiscale.BackColor = Drawing.Color.Yellow
                    isValid = False
                    errorList &= vbCrLf & "- codice fiscale già presente in archivio (" & dbRdr.GetString(1) & " " & dbRdr.GetString(2) & ")"
                End If
                dbRdr.Close()
                dbCmd.Dispose()
            End If
        End If

        'validazione profilo
        If ddnProfilo.SelectedValue = String.Empty Then
            ddnProfilo.BackColor = Drawing.Color.Yellow
            isValid = False
            errorList &= vbCrLf & "- profilo: dato obbligatorio"
        Else
            'verifico che il profilo sia valido per gli esterni
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT fl_ESTERNO FROM age_PROFILI WHERE ac_PROFILO=@ac_PROFILO"
                .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = ddnProfilo.SelectedValue
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()
            If Not dbRdr.GetBoolean(0) Then
                ddnProfilo.BackColor = Drawing.Color.Yellow
                isValid = False
                errorList &= vbCrLf & "- il profilo selezionato non è utilizzabile per persone non dipendenti " & My.Settings.CompanyName
            End If
            dbRdr.Close()
            dbCmd.Dispose()
        End If

        

        If Not isValid Then
            errorList = "Impossibile creare la persona per i seguenti errori:" & vbCrLf & errorList
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(errorList) & ");", True)
        Else
            'OK, ci siamo
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "preAddEsistente",
                                "tryCreaIscriviNuovo(" & GecFinalContextHandler.id_EVENTO.ToString & ", " &
                                Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ddnProfilo.SelectedValue) & ", " &
                                Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_CATEGORIAECM) &
                                ");", True)
        End If

    End Sub

    Private Sub CreateAndAddIt()

        Dim dbCmd = dbConn.CreateCommand
        Dim prmid_ISCRITTO As SqlParameter
        Dim newid_ISCRITTO As Integer = 0

        Dim datiCF As New Softailor.Global.ValidationUtils.DatiCodiceFiscale

        'dati da codice fiscale
        If txtCodiceFiscale.Text <> String.Empty Then
            datiCF = Softailor.Global.ValidationUtils.DatiDaCodiceFiscaleValido(txtCodiceFiscale.Text)
        End If

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_CreaIscriviPersona"
            .Parameters.Add("@ac_TITOLO", SqlDbType.NVarChar, 10).Value = ddnTitolo.SelectedValue
            .Parameters.Add("@tx_COGNOME", SqlDbType.NVarChar, 100).Value = txtCognome.Text
            .Parameters.Add("@tx_NOME", SqlDbType.NVarChar, 100).Value = txtNome.Text
            .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = ddnProfilo.SelectedValue
            .Parameters.Add("@ac_CODICEFISCALE", SqlDbType.NVarChar, 16).Value = IIf(txtCodiceFiscale.Text = String.Empty, DBNull.Value, txtCodiceFiscale.Text)
            .Parameters.Add("@dt_NASCITA", SqlDbType.DateTime).Value = datiCF.dataNascita
            .Parameters.Add("@ac_COMUNENASCITA", SqlDbType.NVarChar, 4).Value = datiCF.belfioreNascita
            .Parameters.Add("@ac_GENERE", SqlDbType.NVarChar, 1).Value = datiCF.genereMF
            .Parameters.Add("@ac_CATEGORIALAVORATIVA", SqlDbType.NVarChar, 8).Value = IIf(ddnCategoriaLavorativa.SelectedValue = String.Empty, DBNull.Value, ddnCategoriaLavorativa.SelectedValue)
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM
            .Parameters.Add("@ac_ORIGINEISCRIZIONE", SqlDbType.NVarChar, 8).Value = "BO"
            .Parameters.Add("@dt_CREAZIONE", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@tx_CREAZIONE", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
            prmid_ISCRITTO = .Parameters.Add("@id_ISCRITTO", SqlDbType.Int)
            prmid_ISCRITTO.Direction = ParameterDirection.Output
        End With
        Try
            dbCmd.ExecuteNonQuery()
            With CType(prmid_ISCRITTO.SqlValue, SqlInt32)
                If Not .IsNull Then
                    newid_ISCRITTO = .Value
                End If
            End With
        Catch ex As Exception
        End Try
        dbCmd.Dispose()

        If newid_ISCRITTO = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert('Si è verificato un errore durante la creazione del nominativo.');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "done", "parent.stl_sel_done('" & newid_ISCRITTO.ToString & "_EDIT');", True)
        End If

    End Sub

    Private Sub lnkIscriviEsistente_Click(sender As Object, e As EventArgs) Handles lnkIscriviEsistente.Click

				If checkDocente() = "1" Then
						AddIt(CInt(grdDOCENTI.SelectedDataKey.Value))
				Else
						AddIt(CInt(grdPERSONE.SelectedDataKey.Value))
				End If

		End Sub

		Private Sub lnkCreaIscriviNuovo_Click(sender As Object, e As EventArgs) Handles lnkCreaIscriviNuovo.Click
				CreateAndAddIt()
		End Sub

		Public Function checkDocente() As String
				Dim dbCmd As SqlCommand
				Dim dbRdr As SqlDataReader

				dbCmd = dbConn.CreateCommand
				With dbCmd
						.CommandType = CommandType.Text
						.CommandText = "SELECT CAST(CASE WHEN fl_DEFAULT=1 THEN '1' ELSE '0' END as VARCHAR(1)) fl_DEFAULT FROM cf_ALBO_DOCENTI_CATEGORIE_ECM WHERE ac_CATEGORIAECM=@ac_CATEGORIAECM"
						.Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM
				End With
				dbRdr = dbCmd.ExecuteReader
				dbRdr.Read()

				If dbRdr.HasRows AndAlso dbRdr.GetString(0) IsNot Nothing AndAlso dbRdr.GetString(0).ToLower Like "1" Then
						dbRdr.Close()
						dbCmd.Dispose()
						Return "1"
				Else
						dbRdr.Close()
						dbCmd.Dispose()
						Return "0"
				End If

		End Function


End Class