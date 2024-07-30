Imports System.Configuration.ConfigurationManager
Public Class GFMailHandler

    Public Overloads Shared Function SendMail(destinationAddress As String, subject As String, bodyHtml As String) As Boolean

        'spedizione mail non legata ad evento - con dati globali

        'lettura dati organizzatore
        Dim ragionesociale As String = My.Settings.GenericMail_RagioneSociale
        Dim indirizzocompleto As String = My.Settings.GenericMail_IndirizzoCompleto
        Dim tel As String = My.Settings.GenericMail_Tel
        Dim fax As String = My.Settings.GenericMail_Fax
        Dim email As String = My.Settings.GenericMail_MailFrom
        Dim baseUrl = My.Settings.FrontOfficeUrl

        Try
            Dim smtpClient As New System.Net.Mail.SmtpClient()
            Dim mailMsg As New System.Net.Mail.MailMessage
            With mailMsg
                .From = New System.Net.Mail.MailAddress(email, ragionesociale)
                .Subject = subject
                .IsBodyHtml = True
                .Body = Transformer.Transform(New XmlDocument, "~/Templates/" & My.Settings.CompanyKey & "/Mail/MailContainer.xslt", _
                                                "ht_testo", bodyHtml, _
                                                "baseurl", baseUrl, _
                                                "subject", subject, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)
                .To.Add(New System.Net.Mail.MailAddress(destinationAddress))
            End With
            smtpClient.Send(mailMsg)
            smtpClient = Nothing
        Catch ex As Exception
            Return False
        End Try

        Return True


    End Function

End Class
