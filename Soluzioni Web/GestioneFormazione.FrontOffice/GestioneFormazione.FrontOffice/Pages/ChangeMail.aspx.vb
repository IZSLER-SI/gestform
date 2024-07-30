Public Class ChangeMail
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'scrittura dati iniziali
        If Not Page.IsPostBack Then
            ScriviDatiIniziali()
        End If

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'accesso: possibile solo se siamo dentro
        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "cambio-mail"

    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Modifica indirizzo e-mail"

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

    Private Sub ScriviDatiIniziali()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        Dim _tx_email As String = ""

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
        End With

        dbRdr.Close()
        dbCmd.Dispose()

        txtOldEmail.Text = _tx_email

    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click

        If ValidateMe() Then
            'salvataggio
            SaveMe()

            'mail nella session
            ContextHandler.tx_EMAIL = txtEmail.Text

            'risistemo menu sopra
            CType(Me.Master, MenuUserMP).RefreshTopMenu()

            'tutto invisibile
            pnlEmail.Visible = False
            pnlSave.Visible = False
            pnlDone.Visible = True

        End If
    End Sub

    Private Function ValidateMe() As Boolean

        Dim valid = True

        'pulizia
        errEmail.Text = ""
        errEmail2.Text = ""
        txtEmail.Text = txtEmail.Text.Trim.ToLower
        txtEmail2.Text = txtEmail2.Text.Trim.ToLower

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

    Private Sub SaveMe()

        Dim dbCmd As SqlCommand

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ChangeMail"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@tx_email", SqlDbType.NVarChar, 150).Value = txtEmail.Text
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
    End Sub
End Class