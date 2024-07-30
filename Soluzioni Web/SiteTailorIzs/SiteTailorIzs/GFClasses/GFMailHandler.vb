Imports System.Configuration.ConfigurationManager
Public Class GFMailHandler

    Public Shared Function SendMail(dbConn As SqlConnection, id_EVENTO As Integer, baseUrl As String, destinationAddress As String, subject As String, bodyHtml As String) As Boolean

        'lettura dati organizzatore
        Dim ragionesociale As String = ""
        Dim indirizzocompleto As String = ""
        Dim tel As String = ""
        Dim fax As String = ""
        Dim email As String = ""
        'mail usata per spedire: sempre la stessa per questione SMTP
        Dim email_from As String = GecFinalContextHandler.tx_MAILFROM

        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_GetDatiSegreteriaOrganizzativaEvento"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        If dbrdr.Read Then
            ragionesociale = dbrdr.GetString(0)
            indirizzocompleto = dbrdr.GetString(1)
            tel = dbrdr.GetString(2)
            fax = dbrdr.GetString(3)
            
            email = dbrdr.GetString(4)
        End If
        dbrdr.Close()
        dbCmd.Dispose()

        Try
            Dim smtpClient As New System.Net.Mail.SmtpClient()
            Dim mailMsg As New System.Net.Mail.MailMessage
            With mailMsg
                .From = New System.Net.Mail.MailAddress(email_from, ragionesociale)
                .Subject = subject
                .IsBodyHtml = True
                .Body = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainer.xslt", _
                                                "ht_testo", bodyHtml, _
                                                "baseurl", baseUrl, _
                                                "subject", subject, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)
                .To.Add(New System.Net.Mail.MailAddress(destinationAddress))
                If My.Settings.BccMail <> "" Then .Bcc.Add(New System.Net.Mail.MailAddress(My.Settings.BccMail))
            End With
            smtpClient.Send(mailMsg)
            smtpClient.Dispose()
        Catch ex As Exception
            Return False
        End Try

        Return True


    End Function

End Class
