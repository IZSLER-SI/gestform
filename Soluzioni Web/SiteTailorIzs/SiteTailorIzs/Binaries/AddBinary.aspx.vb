Imports Softailor.SiteTailor.Binaries
Imports Softailor.Global

Partial Public Class AddBinary
    Inherits System.Web.UI.Page

    Dim dbConn As SqlConnection
    Dim formatoBaseAccettato As FormatoBaseAccettato

    Private Sub AddBinary_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'NOTA: questa pagina non gestisce le autorizzazioni perchè ci si basa sulle autorizzazioni dei dati binari.

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn
       
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim rCODCATEG As String = ""
        Dim rDESEL_TX As String = ""

        'questa pagina viene aperta "stand-alone" oppure passando CODCATEG ed eventualmente DESEL_TX
        If Not Request.QueryString("CODCATEG") Is Nothing Then
            rCODCATEG = Request("CODCATEG").ToUpper
        End If


        If Not Request.QueryString("DESEL_TX") Is Nothing Then
            rDESEL_TX = Request("DESEL_TX").Trim
        End If

        If Not Page.IsPostBack Then

            'nascondo tutto
            pnlCODFORBA.Visible = False
            pnlDescription.Visible = False
            pnlData.Visible = False
            pnlFile.Visible = False
            pnlDimensioniImg.Visible = False
            pnlExternalUrl.Visible = False
            pnlDimensioniElemento.Visible = False
            pnlThumbnail.Visible = False
            pnlUpload.Visible = False

            'determino le categorie abilitate
            Dim categorieAbilitate As Dictionary(Of String, String)
            categorieAbilitate = BinariesHelpers.GetCategorieAbilitate(dbConn, ContextHandler.ID_AZIEN, ContextHandler.ID_UTENT, False, True, False, False)

            If rCODCATEG = "" Then
                'inserisco tutte le categorie per le quali l'utente è abilitato
                MiscUtils.FillDropDown(ddnCODCATEG, categorieAbilitate, True, True)
                ddnCODCATEG.Focus()
            Else
                'verifico se l'utente è abilitato alla categoria scelta
                If categorieAbilitate.ContainsKey(rCODCATEG) Then
                    'OK. Inserisco la categoria scelta e la seleziono.
                    ddnCODCATEG.Items.Add(New ListItem(categorieAbilitate(rCODCATEG), rCODCATEG))
                    SelectCODCATEG()
                Else
                    'utente non abilitato
                    dbConn.Close()
                    Response.StatusCode = 403   'non autorizzato
                    Response.End()
                    Exit Sub
                End If
            End If

            'inserisco DESEL_TX
            If rDESEL_TX <> "" Then Me.txtDESEL_TX.Text = rDESEL_TX

        Else
            'dopo il primo postback, se ho pnlData visibile, carico formato accettato
            If pnlData.Visible Then
                LoadFormatoBaseAccettato()
            End If

        End If
    End Sub

    Private Sub AddBinary_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
            dbConn = Nothing
        End If
    End Sub

    Private Sub SelectCODCATEG()
        'disabilito
        ddnCODCATEG.Enabled = False
        ddnCODCATEG.BackColor = Drawing.Color.Empty
        btnCODCATEG.Enabled = False

        'riempio CODFORMA
        Dim formatiPossibili As New Dictionary(Of String, String)
        formatiPossibili = BinariesHelpers.GetFormatiCategoria(dbConn, ContextHandler.ID_AZIEN, ddnCODCATEG.SelectedValue)
        MiscUtils.FillDropDown(ddnCODFORBA, formatiPossibili, True, True)

        'rendo visibile
        pnlCODFORBA.Visible = True

        If formatiPossibili.Count = 1 Then
            ddnCODFORBA.SelectedValue = formatiPossibili.Keys(0)
            SelectCODFORBA()
        Else
            ddnCODFORBA.Focus()
        End If

    End Sub

    Private Sub SelectCODFORBA()

        ddnCODFORBA.Enabled = False
        ddnCODFORBA.BackColor = Drawing.Color.Empty
        btnCODFORBA.Enabled = False

        'rendo visibile
        pnlDescription.Visible = True
        txtDESEL_TX.Focus()
    End Sub

    
    Private Sub btnDESEL_TX_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDESEL_TX.Click

        Dim result As BinariesHelpers.VerificaDESEL_TXResult

        'dopo inserimento descrizione immagine

        txtDESEL_TX.Text = txtDESEL_TX.Text.Trim

        'verifica
        result = BinariesHelpers.VerificaDESEL_TX(dbConn, ContextHandler.ID_AZIEN, ddnCODCATEG.SelectedValue, txtDESEL_TX.Text)

        Select Case result.Result
            Case BinariesHelpers.VerificaDESEL_TXResult.Results.AlreadyExisting
                txtDESEL_TX.BackColor = Drawing.Color.Yellow
                errDESEL_TX.Text = "In archivio esiste già un elemento con questa descrizione."
                txtDESEL_TX.Focus()
            Case BinariesHelpers.VerificaDESEL_TXResult.Results.Empty
                txtDESEL_TX.BackColor = Drawing.Color.Yellow
                errDESEL_TX.Text = "E' necessario specificare una descrizione."
                txtDESEL_TX.Focus()
            Case BinariesHelpers.VerificaDESEL_TXResult.Results.OK
                txtDESEL_TX.BackColor = Drawing.Color.Empty
                errDESEL_TX.Text = ""
                'ok vado con il resto
                txtDESEL_TX.Enabled = False
                btnDESEL_TX.Enabled = False
                pnlData.Visible = True
                pnlUpload.Visible = True
                LoadFormatoBaseAccettato()
                SetupPnlData()
        End Select

    End Sub

    Private Sub LoadFormatoBaseAccettato()
        formatoBaseAccettato = New FormatoBaseAccettato(dbConn, ContextHandler.ID_AZIEN, ddnCODCATEG.SelectedValue, ddnCODFORBA.SelectedValue)
    End Sub


    Private Sub SetupPnlData()

        Dim s As String
        Dim firstControl As Control = Nothing

        'setup delle varie parti pannelli

        With formatoBaseAccettato

            'pnlFile
            If .IS_LOCAL Then
                pnlFile.Visible = True
                If IsNothing(firstControl) Then firstControl = fupFile
                'formati accettati
                s = ""
                For Each fa As FormatoAccettato In .FormatiAccettati.Values
                    If s <> "" Then s &= "; "
                    s &= fa.EXTE_GET
                Next
                lblFormatiFile.Text = s

                'JSCHECK
                s = ""
                For Each fa As FormatoAccettato In .FormatiAccettati.Values
                    For Each ext As String In fa.EXTE_PUTs
                        s &= ext & ";"
                    Next
                Next
                fupFile_check.Value = "1"
                fupFile_ext.Value = s

                'dimensioni immagine
                If .GESTHUMB = ModalitaGestioneAnteprima.Auto Then
                    If .EsisteLimiteDimensioniImmagine Then
                        pnlDimensioniImg.Visible = True
                        lblDimensioniImg.Text = .DescrizioneLimiteDimensioniImmagine("<br/>")
                    End If
                End If
                'dimensioni file
                lblDimensionefile.Text = BinariesHelpers.KBtoMB(.MAX_SIZE)
            End If

            'pnlExternalUrl
            If Not .IS_LOCAL Then
                pnlExternalUrl.Visible = True
                If IsNothing(firstControl) Then firstControl = txtURL_EXTE
                Select Case .CODFORBA
                    Case "E_FLV"
                        lblURL_EXTE.Text = "E' necessario inserire un URL (http:// o https://) che punti ad un file Flash Video (.flv)."
                    Case "E_SWF"
                        lblURL_EXTE.Text = "E' necessario inserire un URL (http:// o https://) che punti ad un'animazione Flash (.swf)."
                    Case "E_URL"
                        lblURL_EXTE.Text = "E' necessario inserire un URL (http:// o https://)."
                End Select

                'JSCHECK
                URL_EXTE_check.Value = "1"

            End If

            'pnlDimensioniElemento
            If .NEEDSIZE Then
                pnlDimensioniElemento.Visible = True
                If IsNothing(firstControl) Then firstControl = txtELE_WIDT

                'descrizione step


                'testi e limiti larghezza e altezza
                If .LarghezzaFissaElemento > 0 Then
                    txtELE_WIDT.Text = .LarghezzaFissaElemento.ToString
                    txtELE_WIDT.Enabled = False
                Else
                    If .EsisteLimiteLarghezzaElemento Then lblELE_WIDT.Text = .DescrizioneLimiteLarghezzaElemento("")
                End If
                If .AltezzaFissaElemento > 0 Then
                    txtELE_HEIG.Text = .AltezzaFissaElemento.ToString
                    txtELE_HEIG.Enabled = False
                Else
                    If .EsisteLimiteAltezzaElemento Then lblELE_HEIG.Text = .DescrizioneLimiteAltezzaElemento("")
                End If

                'JSCHECK
                ELE_WIDT_check.Value = "1"
                ELE_WIDT_min.Value = .MIN_WIDT.ToString
                ELE_WIDT_max.Value = .MAX_WIDT.ToString
                ELE_HEIG_check.Value = "1"
                ELE_HEIG_min.Value = .MIN_HEIG.ToString
                ELE_HEIG_max.Value = .MAX_HEIG.ToString


            End If

            'pnlThumbnail
            If .GESTHUMB = ModalitaGestioneAnteprima.Optional Then
                pnlThumbnail.Visible = True

                If .NEEDSIZE Then
                    lblThumbnail.Text = "6."
                Else
                    lblThumbnail.Text = "5."
                End If
                lblThumbnail.Text &= " Immagine per generazione anteprime"
               
                If IsNothing(firstControl) Then firstControl = fupThumbnail
                If ThumbnailDefaults.DimensioneMassimaSorgenteThumbnail Mod 1024 = 0 Then
                    lblMaxDimensioneImgThumbnail.Text = (ThumbnailDefaults.DimensioneMassimaSorgenteThumbnail \ 1024).ToString & " MB"
                Else
                    lblMaxDimensioneImgThumbnail.Text = BinariesHelpers.KBtoMB(ThumbnailDefaults.DimensioneMassimaSorgenteThumbnail)
                End If

                'JSCHECK
                fupThumbnail_check.Value = "1"
                fupThumbnail_ext.Value = ThumbnailDefaults.EstensioniAccettateSorgenteThumbnail
            End If
        End With

        If Not IsNothing(firstControl) Then firstControl.Focus()
    End Sub

    Private Sub btnCODCATEG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCODCATEG.Click
        If ddnCODCATEG.SelectedValue <> "" Then
            SelectCODCATEG()
        Else
            ddnCODCATEG.BackColor = Drawing.Color.Yellow
            errCODCATEG.Text = "Seleziona una categoria."
            ddnCODCATEG.Focus()
        End If
    End Sub

    Private Sub btnCODFORBA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCODFORBA.Click
        If ddnCODFORBA.SelectedValue <> "" Then
            SelectCODFORBA()
        Else
            ddnCODFORBA.BackColor = Drawing.Color.Yellow
            errCODFORBA.Text = "Seleziona un formato."
            ddnCODFORBA.Focus()
        End If
    End Sub

    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If CheckData() Then
            'OK. Se siamo arrivati fino a qui, significa che tutti i dati del form 
            'sono corretti e che possiamo caricare il dato.

            'creo l'oggetto Loader
            Dim binarySaver As New BinarySaver(ContextHandler.ID_AZIEN, formatoBaseAccettato, ContextHandler.BinariesBasePath)

            'fornisco i dati al binarySaver
            With binarySaver
                .DESEL_TX = txtDESEL_TX.Text
                If formatoBaseAccettato.NEEDSIZE Then
                    .ELE_WIDT = Integer.Parse(txtELE_WIDT.Text)
                    .ELE_HEIG = Integer.Parse(txtELE_HEIG.Text)
                End If
                If Not formatoBaseAccettato.IS_LOCAL Then
                    .URL_EXTE = txtURL_EXTE.Text
                End If
                If formatoBaseAccettato.IS_LOCAL Then
                    .FileName = fupFile.FileName
                    .FilePostedFile = fupFile.PostedFile
                End If
                If formatoBaseAccettato.GESTHUMB = ModalitaGestioneAnteprima.Optional And fupThumbnail.HasFile Then
                    .ThumbName = fupThumbnail.FileName
                    .ThumbPostedFile = fupThumbnail.PostedFile
                End If
            End With

            'salvo
            Dim saveResult As BinarySaver.SaveResult = binarySaver.SaveToDbAndFS(dbConn, ContextHandler.ID_UTENT, Date.Now)

            'e ora...
            Select Case saveResult.result
                Case SiteTailor.Binaries.BinarySaver.SaveResults.OK
                    'OK ti chiudo
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "done", "parent.stl_sel_done('" & saveResult.ID_ELEME.ToString & "');", True)
                Case SiteTailor.Binaries.BinarySaver.SaveResults.DESEL_Duplicate
                    errUpload.Text = "La descrizione immessa è già presente in archivio. Un altro utente ha inserito un immagine/allegato con uguale descrizione negli ultimi secondi. Ti preghiamo di chiudere la finestra e riprovare."
                Case SiteTailor.Binaries.BinarySaver.SaveResults.UnexpectedError
                    errUpload.Text = "<b>Si è verificato un errore inatteso. Testo dell'errore:</b><br/>" & saveResult.exceptionText
            End Select
        End If
    End Sub

    Private Function CheckData() As Boolean

        Dim result As Boolean = True

        'riproduco praticamente tutto quello che fa il Javascript...

        With formatoBaseAccettato
            'fupFile (islocal)
            If .IS_LOCAL Then
                result = result And ValidateFupFile()
            End If

            'URL_EXTE (not islocal)
            If Not .IS_LOCAL Then
                result = result And ValidateURL_EXTE()
            End If

            'ELE_WIDT (needsize)
            If .NEEDSIZE Then
                result = result And ValidateELE_WIDT()
                result = result And ValidateELE_HEIG()
            End If

            'fupThumbnail (.gestthumb)
            If .GESTHUMB = ModalitaGestioneAnteprima.Optional Then
                result = result And ValidateFupThumbnail()
            End If
        End With

        If Not result Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errValidation", "window.alert('Si sono verificati uno o più errori. Controlla i messaggi di errore in rosso.');", True)
        End If
        Return result

    End Function

    Private Function ValidateFupFile() As Boolean

        Dim result As Boolean = False
        Dim EXTE_PUT As String = formatoBaseAccettato.EXTE_PUT

        If Not fupFile.HasFile Then
            'non c'è un file
            errFupFile.Text = "E' necessario selezionare un file"
        Else
            'ok c'è un file. Controllo dell'estensione.
            Dim ext As String = IO.Path.GetExtension(fupFile.FileName)
            If Not formatoBaseAccettato.ContainsEXTE_PUT(ext) Then
                'formato non valido
                errFupFile.Text = "E' necessario selezionare un file con una delle seguenti estensioni: " & Mid(EXTE_PUT, 1, Len(EXTE_PUT) - 1)
            Else
                'OK formato valido. Controlliamo la dimensione del file!
                Dim sizeKB As Integer
                sizeKB = fupFile.PostedFile.ContentLength \ 1024

                If sizeKB > formatoBaseAccettato.MAX_SIZE Then
                    errFupFile.Text = "La dimensione del file è eccessiva. E' possibile caricare files di dimensione massima " & BinariesHelpers.KBtoMB(formatoBaseAccettato.MAX_SIZE) & "."
                Else
                    'OK dimensione KB giusta
                    'vediamo se stiamo lavorando con un'immagine
                    'e in questo caso proviamo ad aprirla
                    If formatoBaseAccettato.GESTHUMB = ModalitaGestioneAnteprima.Auto Then
                        'trattasi di immagine.
                        'proviamo ad aprirla per vedere se è leggibile
                        Try
                            Dim img As New System.Drawing.Bitmap(fupFile.PostedFile.InputStream)
                            Dim imgW As Integer = img.Width
                            Dim imgH As Integer = img.Height
                            img.Dispose()

                            'OK, se siamo arrivati fino a qui il file immagine è leggibile
                            If formatoBaseAccettato.EsisteLimiteDimensioniImmagine Then
                                'verifica dimensioni
                                With formatoBaseAccettato
                                    If (.MIN_WIDT > 0 And imgW < .MIN_WIDT) Or _
                                       (.MAX_WIDT > 0 And imgW > .MAX_WIDT) Or _
                                       (.MIN_HEIG > 0 And imgH < .MIN_HEIG) Or _
                                       (.MAX_HEIG > 0 And imgH > .MAX_HEIG) Then
                                        'dimensioni non valide)

                                        errFupFile.Text = "Le dimensioni del file caricato (" & imgW.ToString & " x " & imgH.ToString & " pixel) non soddisfano i vincoli richiesti."

                                    Else
                                        'OK, dimensioni valide
                                        errFupFile.Text = ""
                                        result = True
                                    End If
                                End With

                            Else
                                'se non ci sono limiti, l'immagine è valida
                                errFupFile.Text = ""
                                result = True
                            End If

                        Catch ex As Exception
                            'errore apertura immagine
                            errFupFile.Text = "Impossibile aprire il file immagine inviato. Il file potrebbe essere corrotto o in un formato non supportato."
                        End Try
                    Else
                        'non è un'immagine quindi prendiamo per buono il file.
                        errFupFile.Text = ""
                        result = True
                    End If
                End If
            End If
        End If

        Return result

    End Function

    Private Function ValidateURL_EXTE() As Boolean
        Dim result As Boolean = False
        Dim sURL_EXTE As String = txtURL_EXTE.Text.Trim

        If sURL_EXTE = "" Then
            errURL_EXTE.Text = "E' necessario immettere un indirizzo"
            txtURL_EXTE.BackColor = Drawing.Color.Yellow
        Else
            If sURL_EXTE Like "http://*" Or sURL_EXTE Like "https://*" Then
                If sURL_EXTE.Length < 13 Then
                    errURL_EXTE.Text = "L'indirizzo immesso non è valido"
                    txtURL_EXTE.BackColor = Drawing.Color.Yellow
                Else
                    'ok ci siamo
                    errURL_EXTE.Text = ""
                    txtURL_EXTE.BackColor = Drawing.Color.Empty
                    result = True
                End If
            Else
                errURL_EXTE.Text = "L'indirizzo deve iniziare con http:// o https://"
                txtURL_EXTE.BackColor = Drawing.Color.Yellow
            End If
        End If
        Return result
    End Function

    Private Function ValidateELE_WIDT() As Boolean
        Return ValidateWH(txtELE_WIDT, errELE_WIDT, formatoBaseAccettato.MIN_WIDT, formatoBaseAccettato.MAX_WIDT)
    End Function

    Private Function ValidateELE_HEIG() As Boolean
        Return ValidateWH(txtELE_HEIG, errELE_HEIG, formatoBaseAccettato.MIN_HEIG, formatoBaseAccettato.MAX_HEIG)
    End Function

    Private Function ValidateWH(ByVal txt As TextBox, ByVal lbl As Label, ByVal min As Integer, ByVal max As Integer) As Boolean

        Dim result As Boolean = False

        Dim val As String
        Dim num As Integer

        val = txt.Text.Trim
        If val = "" Then
            'vuoto
            lbl.Text = "E' necessario immettere un valore"
            txt.BackColor = Drawing.Color.Yellow
        Else
            'ok c'è un valore
            Try
                num = Integer.Parse(val)
                'ok è un numero
                If num <= 0 Then
                    'zero o negativo
                    lbl.Text = "E' necessario immettere un numero intero maggiore di zero"
                    txt.BackColor = Drawing.Color.Yellow
                Else
                    'positivo. Controllo i limiti.
                    If (min > 0 And num < min) Or (max > 0 And num > max) Then
                        lbl.Text = "Il numero immesso non rispetta i limiti imposti"
                        txt.BackColor = Drawing.Color.Yellow
                    Else
                        'OK ci siamo
                        lbl.Text = ""
                        txt.BackColor = Drawing.Color.Empty
                        result = True
                    End If

                End If

            Catch ex As Exception
                'non è un numero
                lbl.Text = "E' necessario immettere un numero intero"
                txt.BackColor = Drawing.Color.Yellow
            End Try
        End If

        Return result
    End Function

    Private Function ValidateFupThumbnail() As Boolean

        Dim result As Boolean = False
        
        If Not fupThumbnail.HasFile Then
            'non c'è un file > OK
            errFupThumbnail.Text = ""
            result = True
        Else
            'ok c'è un file. Controllo dell'estensione.
            Dim ext As String = IO.Path.GetExtension(fupThumbnail.FileName)
            If InStr(ThumbnailDefaults.EstensioniAccettateSorgenteThumbnail, ext & ";") = 0 Then
                'formato non valido
                errFupThumbnail.Text = "E' necessario selezionare un file con una delle seguenti estensioni: " & Mid(ThumbnailDefaults.EstensioniAccettateSorgenteThumbnail, 1, Len(ThumbnailDefaults.EstensioniAccettateSorgenteThumbnail) - 1)
            Else
                'OK formato valido. Controlliamo la dimensione del file!
                Dim sizeKB As Integer
                sizeKB = fupThumbnail.PostedFile.ContentLength \ 1024

                If sizeKB > ThumbnailDefaults.DimensioneMassimaSorgenteThumbnail Then
                    errFupThumbnail.Text = "La dimensione del file è eccessiva. E' possibile caricare files di dimensione massima " & BinariesHelpers.KBtoMB(ThumbnailDefaults.DimensioneMassimaSorgenteThumbnail) & "."
                Else
                    'OK dimensione KB giusta
                    'proviamo ad aprire l'immagine per vedere se è leggibile
                    Try
                        Dim img As New System.Drawing.Bitmap(fupThumbnail.PostedFile.InputStream)
                        img.Dispose()

                        'OK, se siamo arrivati fino a qui il file immagine è leggibile                        
                        errFupThumbnail.Text = ""
                        result = True
                    Catch ex As Exception
                        'errore apertura immagine
                        errFupThumbnail.Text = "Impossibile aprire il file inviato. Il file potrebbe essere corrotto o in un formato non supportato."
                    End Try

                End If
            End If
        End If

        Return result

    End Function
End Class