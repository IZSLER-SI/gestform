Public Class CompleteProfile
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Private Sub CompleteProfile_Init(sender As Object, e As EventArgs) Handles Me.Init

        'visibilità pannelli (sempre)
        pnlEmail.Visible = ContextHandler.fl_DIPENDENTE
        pnlPassword.Visible = ContextHandler.fl_DIPENDENTE
        pnlEsterni.Visible = Not ContextHandler.fl_DIPENDENTE

        'drop down rapporto lavorativo e profilo
        If Not Page.IsPostBack And Not ContextHandler.fl_DIPENDENTE Then
            FillRapportoLavorativoProfilo()
        End If

        'scrittura dati iniziali
        If Not Page.IsPostBack Then
            ScriviDatiIniziali()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return ""
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Completamento Profilo"
    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        If ContextHandler.Region = ContextHandler.Regions.LoggedOut Then
            Response.Redirect("/", True)
            Exit Sub
        End If

    End Sub

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'posso accedere solo se siamo in fase "completamento profilo"
        Return ContextHandler.Region = ContextHandler.Regions.CompleteProfile

    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return True
    End Function

    Private Sub ScriviDatiIniziali()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        Dim _tx_email As String = ""
        Dim _ac_categorialavorativa As String = ""
        Dim _ac_ruolo As String = ""
        Dim _ac_profilo As String = ""
        Dim _id_disciplina As String = ""

        dbCmd = fpd.dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_completeprofile_DatiIniziali"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        With dbRdr
            If Not .IsDBNull(1) Then _tx_email = .GetString(1).ToLower
            If Not .IsDBNull(2) Then _ac_categorialavorativa = .GetString(2)
            If Not .IsDBNull(3) Then _ac_ruolo = .GetString(3)
            If Not .IsDBNull(4) Then _ac_profilo = .GetString(4)
            If Not .IsDBNull(5) Then _id_disciplina = .GetInt32(5).ToString
        End With

        dbRdr.Close()
        dbCmd.Dispose()

        If ContextHandler.fl_DIPENDENTE Then
            'dipendente
            txtEmail.Text = _tx_email
            txtEmail2.Text = _tx_email
        Else
            'esterno

            'cat lavorativa, se c'è
            If _ac_categorialavorativa <> "" Then ddnCategoriaLavorativa.SelectedValue = _ac_categorialavorativa

            'ruolo se c'è
            If _ac_ruolo <> String.Empty Then
                ddnRuolo.SelectedValue = _ac_ruolo

                'aggiorno il ruolo (a cascata pulisce tutto)
                AfterUpdateRuolo()

                'se ho un profilo (compatibile), proseguo
                If _ac_profilo <> String.Empty Then
                    Try
                        ddnProfilo.SelectedValue = _ac_profilo
                    Catch ex As Exception
                        ddnProfilo.SelectedValue = ""
                    End Try

                    'se sono riuscito a selezionare...
                    If ddnProfilo.SelectedValue <> "" Then
                        AfterUpdateProfilo()
                        If ddnDisciplina.Items.Count > 1 And _id_disciplina <> String.Empty Then
                            Try
                                ddnDisciplina.SelectedValue = _id_disciplina
                            Catch ex As Exception
                                ddnDisciplina.SelectedValue = ""
                            End Try
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub FillRapportoLavorativoProfilo()

        Dim dbCmd As SqlCommand

        'cat lavorativa
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_completeprofile_CategorieLavorative"
        End With
        Softailor.Global.MiscUtils.FillDropDown(ddnCategoriaLavorativa, dbCmd, True, True, True, True)

        'ruolo
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_completeprofile_Ruoli"
        End With
        Softailor.Global.MiscUtils.FillDropDown(ddnRuolo, dbCmd, True, True, True, True)

    End Sub

    Private Sub AfterUpdateRuolo()

        Dim dbCmd As SqlCommand

        'svuoto a cascata
        ddnProfilo.Items.Clear()
        ddnDisciplina.Items.Clear()
        pnlDisciplina.Visible = False

        'riempio i profili
        If ddnRuolo.SelectedValue <> "" Then
            dbCmd = fpd.dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_completeprofile_ProfiliRuolo"
                .Parameters.Add("@ac_ruolo", SqlDbType.NVarChar, 4).Value = ddnRuolo.SelectedValue
            End With
            Softailor.Global.MiscUtils.FillDropDown(ddnProfilo, dbCmd, True, True, True, True)
        End If

    End Sub

    Private Sub AfterUpdateProfilo()

        Dim dbCmd As SqlCommand

        'svuoto a cascata
        ddnDisciplina.Items.Clear()
        pnlDisciplina.Visible = False

        'riempio le discipline
        If ddnProfilo.SelectedValue <> "" Then
            dbCmd = fpd.dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_completeprofile_DisciplineProfilo"
                .Parameters.Add("@ac_profilo", SqlDbType.NVarChar, 20).Value = ddnProfilo.SelectedValue
            End With
            Softailor.Global.MiscUtils.FillDropDown(ddnDisciplina, dbCmd, False, True, True, True)
        End If

        'se ho almeno una disciplina, rendo visibile
        If ddnDisciplina.Items.Count > 1 Then
            pnlDisciplina.Visible = True
        End If

    End Sub

    Private Sub ddnRuolo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddnRuolo.SelectedIndexChanged
        AfterUpdateRuolo()
    End Sub

    Private Sub ddnProfilo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddnProfilo.SelectedIndexChanged
        AfterUpdateProfilo()
    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click
        If ValidateMe() Then
            'salvataggio
            SaveMe()

            'mail nella session
            If ContextHandler.fl_DIPENDENTE Then
                ContextHandler.tx_EMAIL = txtEmail.Text
            End If

            'cambio regione
            ContextHandler.Region = ContextHandler.Regions.LoggedIn

            'risistemo menu sopra
            CType(Me.Master, MenuUserMP).RefreshTopMenu()

            'tutto invisibile
            pnlEmail.Visible = False
            pnlPassword.Visible = False
            pnlEsterni.Visible = False
            pnlSave.Visible = False
            pnlDone.Visible = True

        End If
    End Sub

    Private Sub SaveMe()

        Dim dbCmd As SqlCommand

        dbCmd = fpd.dbConn.CreateCommand
        If ContextHandler.fl_DIPENDENTE Then
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_completeprofile_SalvaInterno"
                .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
                .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = txtPassword.Text
                .Parameters.Add("@tx_email", SqlDbType.NVarChar, 150).Value = txtEmail.Text
            End With
        Else
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_completeprofile_SalvaEsterno"
                .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
                .Parameters.Add("@ac_categorialavorativa", SqlDbType.NVarChar, 8).Value = ddnCategoriaLavorativa.SelectedValue
                .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = ddnProfilo.SelectedValue
                With .Parameters.Add("@id_disciplina", SqlDbType.Int)
                    If ddnDisciplina.SelectedValue = "" Then .Value = DBNull.Value Else .Value = CInt(ddnDisciplina.SelectedValue)
                End With
            End With
        End If
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
    End Sub

    Private Function ValidateMe() As Boolean

        If ContextHandler.fl_DIPENDENTE Then
            Return ValidateMe_Dipendente()
        Else
            Return ValidateMe_Esterno()
        End If

    End Function

    Private Function ValidateMe_dipendente() As Boolean

        Dim valid = True

        'pulizia
        errPassword.Text = ""
        errPassword2.Text = ""
        errEmail.Text = ""
        errEmail2.Text = ""
        txtEmail.Text = txtEmail.Text.Trim.ToLower
        txtEmail2.Text = txtEmail2.Text.Trim.ToLower

        If txtPassword.Text.Trim = String.Empty Then
            valid = False
            errPassword.Text = "Campo obbligatorio"
        Else
            If Len(txtPassword.Text) < 8 Then
                valid = False
                errPassword.Text = "Lunghezza minima 8 caratteri"
            Else
                If String.Compare(txtPassword.Text, txtPassword2.Text, False) <> 0 Then
                    valid = False
                    errPassword2.Text = "Le due password non coincidono"
                End If
            End If
        End If

        If txtEmail.Text = String.Empty Then
            valid = False
            errEmail.Text = "Campo obbligatorio"
        Else
            If Not Softailor.Global.ValidationUtils.ValidateEmail(txtEmail.Text) Then
                valid = False
                errEmail.Text = "Indirizzo e-mail non valido"
            Else
                If String.Compare(txtEmail.Text, txtEmail2.Text, True) <> 0 Then
                    valid = False
                    errEmail2.Text = "I due indirizzi e-mail non coincidono"
                End If
            End If
        End If

        Return valid

    End Function

    Private Function ValidateMe_esterno() As Boolean

        Dim valid = True

        'pulizia
        errCategoriaLavorativa.Text = ""
        errRuolo.Text = ""
        errProfilo.Text = ""
        errDisciplina.Text = ""

        If ddnCategoriaLavorativa.SelectedValue = "" Then
            valid = False
            errCategoriaLavorativa.Text = "Campo obbligatorio"
        End If

        If ddnRuolo.SelectedValue = "" Then
            valid = False
            errRuolo.Text = "Campo obbligatorio"
        End If

        If ddnProfilo.SelectedValue = "" Then
            valid = False
            errProfilo.Text = "Campo obbligatorio"
        End If

        If ddnDisciplina.SelectedValue = "" And ddnDisciplina.Items.Count > 1 Then
            valid = False
            errDisciplina.Text = "Campo obbligatorio"
        End If

        Return valid

    End Function
End Class