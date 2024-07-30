Imports GestioneFormazione.FrontOffice

Public Class ControlloPreLogin
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    'Variabili necessarie per il recupero e l'aggiornamento delle informazioni
    Dim id_persona As Integer

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData
        Me.fpd = fpd
    End Sub

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange
        'Nothing
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        pnl_User_Message.Visible = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'If necessario altrimenti ogni qualvolta si cambi voce
            'vengono preselezionati i valori originali
            Dim id_PERSONA = ContextHandler.id_Persona_ControlloPreLogin
            Dim dbCmd = Me.fpd.dbConn.CreateCommand
            Dim dbRdr As SqlDataReader
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_CaricamentoDatiPreLogin"
                .Parameters.Add("@id_persona", SqlDbType.Int).Value = id_PERSONA
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()

            ac_RUOLO.DataBind()
            WriteDdnString(dbRdr, "ac_RUOLO", ac_RUOLO)

            ac_PROFILO.DataBind()
            WriteDdnString(dbRdr, "ac_PROFILO", ac_PROFILO)

            If (IsDipendente()) Then
                ac_RUOLO.Enabled = False
                ac_PROFILO.Enabled = False
            End If

            id_DISCIPLINA.DataBind()
            WriteDdnString(dbRdr, "id_DISCIPLINA", id_DISCIPLINA)
        End If
    End Sub

    Private Sub WriteDdnString(dbRdr As SqlDataReader, field As String, ddn As DropDownList)
        If IsDBNull(dbRdr(field)) Then
            Try
                ddn.SelectedValue = ""
            Catch ex As Exception
            End Try
        Else
            Try
                ddn.SelectedValue = CStr(dbRdr(field))
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub WriteDdnInt(dbRdr As SqlDataReader, field As String, ddn As DropDownList)
        If IsDBNull(dbRdr(field)) Then
            Try
                ddn.SelectedValue = ""
            Catch ex As Exception
            End Try
        Else
            Try
                ddn.SelectedValue = CInt(dbRdr(field)).ToString
            Catch ex As Exception
            End Try
        End If
    End Sub

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Controllo Pre Login"
    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "modifica-profilo-pre-login"
    End Function

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess
        Return True
    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return False
    End Function

    Protected Sub Cambio_Downlist()
    End Sub

    Private Function Is_Professione_Disciplina_Obbligatori(ac_PROFILO As String) As Boolean
        Return True
    End Function

    Protected Sub Memorizza_Informazioni_Utente()
        Dim isError As Boolean
        isError = False
        Dim id_PERSONA = ContextHandler.id_Persona_ControlloPreLogin
        Dim ac_RUOLO_select = ac_RUOLO.SelectedItem.Value.ToString
        Dim ac_PROFILO_select = ac_PROFILO.SelectedItem.Value.ToString
        Dim id_DISCIPLINA_select = id_DISCIPLINA.SelectedItem.Value.ToString

        If (Not IsDipendente()) Then
            If (String.IsNullOrWhiteSpace(ac_RUOLO_select) Or ac_RUOLO_select.Equals("-1") Or
                String.IsNullOrWhiteSpace(ac_PROFILO_select) Or ac_PROFILO_select.Equals("-1")) Then
                dato_mancante_lbl_error.ForeColor = Drawing.Color.Red
                dato_mancante_lbl_error.Text = "Almeno un campo tra quelli obbligatori è mancante"
                isError = True
            End If
        End If

        'Professione/Disciplina obbligatori?
        Dim isProfessioneDisciplinaObbligatori As Boolean = True

        If (isProfessioneDisciplinaObbligatori) Then
            If (String.IsNullOrWhiteSpace(id_DISCIPLINA_select)) Then
                dato_mancante_lbl_error.ForeColor = Drawing.Color.Red
                dato_mancante_lbl_error.Text = "Almeno un campo tra quelli obbligatori è mancante"
                isError = True
            End If
        End If

        If (Not isError) Then
#Region "Costruzione parametri"
            Dim obj_ac_RUOLO As Object
            If (String.IsNullOrWhiteSpace(ac_RUOLO_select)) Then
                obj_ac_RUOLO = DBNull.Value
            Else
                obj_ac_RUOLO = ac_RUOLO_select
            End If

            Dim obj_ac_PROFILO As Object
            If (String.IsNullOrWhiteSpace(ac_PROFILO_select)) Then
                obj_ac_PROFILO = DBNull.Value
            Else
                obj_ac_PROFILO = ac_PROFILO_select
            End If

            Dim obj_id_DISCIPLINA As Object
            If (isProfessioneDisciplinaObbligatori) Then
                obj_id_DISCIPLINA = id_DISCIPLINA_select
            Else
                obj_id_DISCIPLINA = DBNull.Value
            End If

            Dim obj_is_DIPENDENTE As Object
            If (IsDipendente()) Then
                obj_is_DIPENDENTE = 1
            Else
                obj_is_DIPENDENTE = 0
            End If
#End Region

            Dim dbCmd As SqlCommand
            dbCmd = Me.fpd.dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_AggiornaRuoloProfiloECM"
                .Parameters.AddWithValue("@id_persona", id_PERSONA)
                .Parameters.AddWithValue("@tx_MODIFICA", "FrontOffice")
                .Parameters.AddWithValue("@ac_RUOLO", obj_ac_RUOLO)
                .Parameters.AddWithValue("@ac_PROFILO", obj_ac_PROFILO)
                .Parameters.AddWithValue("@id_DISCIPLINA", obj_id_DISCIPLINA)
                .Parameters.AddWithValue("@is_DIPENDENTE", obj_is_DIPENDENTE)
            End With
            dbCmd.ExecuteNonQuery()
            dbCmd.Dispose()

            'Se tutto è andato bene, mostriamo un messaggio di avviso all'utente e lo inviatiamo a ripetere l'accesso con le credenziali
            pnlStep1.Visible = False
            pnl_User_Message.Visible = True
        End If

    End Sub

    Protected Sub Ritorna_HomePage()
        Response.Redirect("/")
    End Sub

    Protected Function IsDipendente() As Boolean
        Return ContextHandler.fl_DIPENDENTE
    End Function
End Class