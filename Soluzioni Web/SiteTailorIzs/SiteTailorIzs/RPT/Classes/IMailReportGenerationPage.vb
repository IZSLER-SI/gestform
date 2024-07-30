Public Class MailSenderData
    Public ragionesociale As String
    Public indirizzocompleto As String
    Public tel As String
    Public fax As String
    Public email As String
End Class

Public Interface IMailReportGenerationPage
    Function GetMailParameters() As MailSenderData
End Interface
