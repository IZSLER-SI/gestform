Imports Softailor.Global.SqlUtils
Public Class CodiceEsternoProfilo

    Public tx_CODICEESTERNO_DESC As String
    Public ac_CODICEESTERNO_REGEX As String
    Public tx_CODICEESTERNO_VALERR As String

    Public Sub New()
        tx_CODICEESTERNO_DESC = ""
        ac_CODICEESTERNO_REGEX = ""
        tx_CODICEESTERNO_VALERR = ""
    End Sub

    Public Sub New(dbConn As SqlConnection, ac_PROFILO As String)
        Me.New()

        If ac_PROFILO <> "" Then
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_fo_DatiCodiceEsternoProfilo"
                .Parameters.Add("@ac_profilo", SqlDbType.NVarChar, 20).Value = ac_PROFILO
            End With
            dbRdr = dbCmd.ExecuteReader
            'Aggiunto IF perché andava in errore
            If (dbRdr.Read()) Then
                tx_CODICEESTERNO_DESC = Nz(dbRdr.GetSqlString(0))
                ac_CODICEESTERNO_REGEX = Nz(dbRdr.GetSqlString(1))
                tx_CODICEESTERNO_VALERR = Nz(dbRdr.GetSqlString(2))
            End If

            dbRdr.Close()
            dbCmd.Dispose()

        End If

    End Sub

    Public Sub SetupControlli(pnl As Panel, lblDesc As Label)
        If tx_CODICEESTERNO_DESC = "" Then
            pnl.Visible = False
        Else
            pnl.Visible = True
            lblDesc.Text = tx_CODICEESTERNO_DESC & " *"
        End If
    End Sub

    Public Function Validate(txt As TextBox, lblError As Label, errRequired As String) As Boolean

        lblError.Text = ""
        txt.Text = txt.Text.Trim

        If tx_CODICEESTERNO_DESC = "" Then
            'non è richiesto un codice esterno: svuoto il campo ed esco
            txt.Text = ""
            Return True
        Else
            'è richiesto un codice esterno
            If txt.Text = String.Empty Then
                'obbligatorio
                lblError.Text = errRequired
                Return False
            Else
                'OK, c'è un valore
                If Regex.IsMatch(txt.Text, ac_CODICEESTERNO_REGEX) Then
                    Return True
                Else
                    lblError.Text = tx_CODICEESTERNO_VALERR
                    Return False
                End If
            End If
        End If

    End Function
End Class
