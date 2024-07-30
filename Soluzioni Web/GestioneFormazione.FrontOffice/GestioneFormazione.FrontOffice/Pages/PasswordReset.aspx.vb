Public Class PasswordReset
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblCompanyName.Text = My.Settings.CompanyName_Short

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'accesso: possibile solo se siamo fuori
        Return ContextHandler.Region = ContextHandler.Regions.LoggedOut

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "password-smarrita"

    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Reimpostazione Password"

    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'me ne vado in ogni caso
        Response.Redirect("/", True)

    End Sub

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage

        Return False

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

    Private Sub lnkStart_Click(sender As Object, e As EventArgs) Handles lnkStart.Click

        Dim valid = True

        'pulizie
        txtCodiceFiscale.Text = txtCodiceFiscale.Text.Trim
        txtEmail.Text = txtEmail.Text.Trim.ToLower

        'validazione
        If txtCodiceFiscale.Text = String.Empty Then
            valid = False
            errCodiceFiscale.Text = "Campo obbligatorio"
        End If
        If txtEmail.Text = String.Empty Then
            valid = False
            errEmail.Text = "Campo obbligatorio"
        Else
            If Not Softailor.Global.ValidationUtils.ValidateEmail(txtEmail.Text) Then
                valid = False
                errEmail.Text = "Indirizzo e-mail non valido"
            End If
        End If

        'controllo presenza email e cf nel database
        Dim errorMsg As String
        errorMsg = CheckIfCfAndEmailExists(txtCodiceFiscale.Text, txtEmail.Text)

        errStart.Text = errorMsg

        If valid And String.IsNullOrEmpty(errorMsg) Then

            If StartPasswordReset() Then

                'ok, la mail è stata inviata
                pnlData.Visible = False
                pnlStart.Visible = False
                pnlDone.Visible = True

            End If

        End If

    End Sub

    Private Function StartPasswordReset() As Boolean

        Dim found = False
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        Dim id_PASSWORDRESET As Integer = 0
        Dim ac_HASH As String = ""

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_PasswordReset_Start"
            .Parameters.Add("@ac_codicefiscale", SqlDbType.NVarChar, 16).Value = txtCodiceFiscale.Text
            .Parameters.Add("@tx_email", SqlDbType.NVarChar, 150).Value = txtEmail.Text
            .Parameters.Add("@ac_secretkey", SqlDbType.NVarChar, 150).Value = My.Settings.PasswordResetKey
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        If Not dbRdr.IsDBNull(0) Then
            found = True
            id_PASSWORDRESET = dbRdr.GetInt32(0)
            ac_HASH = dbRdr.GetString(1)
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        If Not found Then Return False

        'spedizione mail

        Dim mailBody = Transformer.Transform(New XmlDocument,
                                             "~/Templates/" & My.Settings.CompanyKey & "/Mail/PasswordReset.xslt",
                                             "baseurl", My.Settings.FrontOfficeUrl,
                                             "reseturl", My.Settings.FrontOfficeUrl & "reset-password?i=" & id_PASSWORDRESET.ToString & "&h=" & ac_HASH)

        GFMailHandler.SendMail(txtEmail.Text, "Portale Formazione - Reimpostazione Password", mailBody)

        Return True


    End Function

    Private Function CheckIfCfAndEmailExists(cf As String, email As String) As String
        Dim result As Int32
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim msg As String

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_CheckIfUserExists"
            .Parameters.Add("@ac_codicefiscale", SqlDbType.NVarChar, 16).Value = txtCodiceFiscale.Text
            .Parameters.Add("@tx_email", SqlDbType.NVarChar, 150).Value = txtEmail.Text
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        result = dbRdr.GetInt32(0)
        dbRdr.Close()
        dbCmd.Dispose()
        If result = 1 Then
            msg = "Il Codice Fiscale non corrisponde a nessun utente del portale, si invita a registrarsi."
        End If
        If result = 2 Then
            msg = "A questo codice fiscale è associato un indirizzo e-mail diverso da quello inserito. Nel caso non ricordassi quale indirizzo hai usato per la registrazione, o se non puoi più accedere a quella casella di posta, scrivi all’assistenza ('assistenza_formazione_izsler@invisiblefarm.it') per la modifica dei tuoi dati indicando il tuo codice fiscale e il nuovo indirizzo email."
        End If
        Return msg
    End Function
End Class