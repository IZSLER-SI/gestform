Imports System.IO

Public Class MailSubjectHelperGF

    Public Const subjPromemoriaPartecipazione = "Promemoria partecipazione ad evento formativo"
    Public Const subjAccettazione = "Accettazione iscrizione ad evento formativo"
    Public Const subjNonAccettazione = "Mancata accettazione iscrizione ad evento formativo"
    Public Const subjPromemoriaDocenza = "Promemoria partecipazione ad evento formativo"

    Public Shared Function GetSubject(ac_CATEGORIAECM As String, ac_STATOISCRIZIONE As String, fl_ACCETTAZIONEINCHIUSURA As Boolean) As String

        Select Case ac_CATEGORIAECM
            Case "D", "M", "R"
                Return subjPromemoriaDocenza
            Case "P"
                Select Case ac_STATOISCRIZIONE
                    Case "NA"
                        Return subjNonAccettazione
                    Case "I", "P", "AG", "AI"
                        If fl_ACCETTAZIONEINCHIUSURA Then
                            Return subjAccettazione
                        Else
                            Return subjPromemoriaPartecipazione
                        End If
                End Select
        End Select

        Return ""

    End Function

    Public Shared Function GetBody(xDoc As XmlDocument, basePath As String, ac_CATEGORIAECM As String, ac_STATOISCRIZIONE As String, fl_ACCETTAZIONEINCHIUSURA As Boolean, tx_BASEURL As String) As String

        Dim fileName As String

        Select ac_CATEGORIAECM
            Case "D", "M", "R"
                fileName = Path.Combine(basePath, My.Settings.CompanyName & "_ParticipationReminderDRM.xslt")
                Return Transformer.Transform_AbsoluteTemplatePath(xDoc, fileName, "baseurl", tx_BASEURL)
            Case "P"
                Select Case ac_STATOISCRIZIONE
                    Case "NA"
                        fileName = Path.Combine(basePath, My.Settings.CompanyName & "_NotAccepted.xslt")
                        Return Transformer.Transform_AbsoluteTemplatePath(xDoc, fileName, "baseurl", tx_BASEURL)
                    Case "I", "P", "AG", "AI"
                        If fl_ACCETTAZIONEINCHIUSURA Then
                            fileName = Path.Combine(basePath, My.Settings.CompanyName & "_AcceptanceConfirmation.xslt")
                            Return Transformer.Transform_AbsoluteTemplatePath(xDoc, fileName, "baseurl", tx_BASEURL)
                        Else
                            fileName = Path.Combine(basePath, My.Settings.CompanyName & "_ParticipationReminder.xslt")
                            Return Transformer.Transform_AbsoluteTemplatePath(xDoc, fileName, "baseurl", tx_BASEURL)
                        End If
                End Select
        End Select

        Return ""


    End Function

End Class