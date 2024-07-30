Public Class ChangePassword
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'accesso: possibile solo se siamo dentro
        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "cambio-password"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Modifica password"
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


    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click

        If ValidateMe() Then
            'salvataggio
            SaveMe()

            'tutto invisibile
            pnlPassword.Visible = False
            pnlSave.Visible = False
            pnlDone.Visible = True

        End If

    End Sub

    Private Function ValidateMe() As Boolean

        Dim valid = True

        'pulizia
        errOldPassword.Text = ""
        errPassword.Text = ""
        errPassword2.Text = ""

        'validazione vecchia password
        If txtOldPassword.Text.Trim = "" Then
            valid = False
            errOldPassword.Text = "Campo obbligatorio"
        Else
            If Not PasswordCorretta() Then
                valid = False
                errOldPassword.Text = "Password Errata"
            End If
        End If

        'validazione nuova password
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


        Return valid

    End Function

    Public Sub SaveMe()

        Dim dbCmd As SqlCommand

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ChangePassword"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = txtPassword.Text
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
    End Sub

    Private Function PasswordCorretta() As Boolean

        Dim corretta As Boolean
        Dim dbCmd As SqlCommand
        Dim prmOut As SqlParameter

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ChangePassword_ValidateOld"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = txtOldPassword.Text
            prmOut = .Parameters.Add("@fl_result", SqlDbType.Bit)
            prmOut.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        corretta = CBool(prmOut.Value)
        dbCmd.Dispose()

        Return corretta

    End Function
End Class