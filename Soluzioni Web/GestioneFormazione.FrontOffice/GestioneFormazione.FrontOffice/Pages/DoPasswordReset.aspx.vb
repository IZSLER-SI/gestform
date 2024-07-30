Public Class DoPasswordReset
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData
    Dim id_PASSWORDRESET As Integer = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ac_HASH As String = ""
        Dim dbCmd As SqlCommand
        Dim prmResult As SqlParameter
        Dim result As String = ""

        'lettura parametri
        If Request("i") IsNot Nothing Then
            Try
                id_PASSWORDRESET = CInt(Request("i"))
            Catch ex As Exception
                id_PASSWORDRESET = 0
            End Try
        End If
        If Request("h") IsNot Nothing Then
            ac_HASH = Request("h").Trim
        End If

        If id_PASSWORDRESET = 0 Or ac_HASH = String.Empty Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        'validazione dell'hash
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_PasswordReset_Validate"
            .Parameters.Add("@id_passwordreset", SqlDbType.Int).Value = id_PASSWORDRESET
            .Parameters.Add("@ac_hash", SqlDbType.NVarChar, 100).Value = ac_HASH
            .Parameters.Add("@ac_secretkey", SqlDbType.NVarChar, 100).Value = My.Settings.PasswordResetKey
            prmResult = .Parameters.Add("@ni_result", SqlDbType.Int)
            prmResult.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        Select Case CInt(prmResult.Value)
            Case 0 : result = "NOTFOUND"
            Case 1 : result = "OK"
            Case -1 : result = "EXPIRED"
        End Select
        dbCmd.Dispose()

        Select Case result
            Case "NOTFOUND"
                pnlNotFound.Visible = True
            Case "EXPIRED"
                pnlExpired.Visible = True
            Case "OK"
                pnlFeasible.Visible = True
        End Select

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

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click
        If ValidateMe() Then
            'salvataggio
            SaveMe()

            'tutto invisibile
            pnlFeasible.Visible = False
            pnlDone.Visible = True

        End If
    End Sub

    Private Function ValidateMe() As Boolean

        Dim valid = True

        'pulizia
        errPassword.Text = ""
        errPassword2.Text = ""

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
            .CommandText = "sp_fo_PasswordReset_DoIt"
            .Parameters.Add("@id_passwordreset", SqlDbType.Int).Value = id_PASSWORDRESET
            .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = txtPassword.Text
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
    End Sub
End Class