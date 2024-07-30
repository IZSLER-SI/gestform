Imports System.Web.Optimization
Imports System.Web.HttpContext

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ContextHandler.NewEmptySession()
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)

        'mando la mail in caso di eccezione non gestita, solo se nel web.config c'è un erroreportmailto
        If My.Settings.ErrorReportMailTo <> "" Then
            Try
                Dim smtpClient As New System.Net.Mail.SmtpClient()
                Dim mailMsg As New System.Net.Mail.MailMessage
                With mailMsg
                    .From = New System.Net.Mail.MailAddress(My.Settings.ErrorReportMailFrom)
                    .Subject = My.Settings.ErrorReportMailSubject
                    .IsBodyHtml = True
                    .Body = ErrorReport()
                    For Each address As String In My.Settings.ErrorReportMailTo.Split(";"c)
                        If address.Trim <> String.Empty Then
                            .To.Add(New System.Net.Mail.MailAddress(address.Trim))
                        End If
                    Next
                End With
                smtpClient.Send(mailMsg)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Shared Function ErrorReport() As String
        Dim M As String = "", i As Integer

        M &= "<html><head><title>Rapporto Errore</title>" & vbCrLf
        M &= "<style type=""text/css"">" & vbCrLf
        M &= "body {font-family: Verdana, Arial; font-size:11px;}"
        M &= "</style>" & vbCrLf
        M &= "</head><body>" & vbCrLf

        M &= "<div><b>ECCEZIONE NON GESTITA</b></div>" & vbCrLf

        'dati della request
        M &= "<div><br/></div>" & vbCrLf
        M &= "<div><b>Request</b></div>" & vbCrLf
        M &= "<hr/>" & vbCrLf
        M &= "<div>" & vbCrLf
        If Current.Request IsNot Nothing Then
            M &= Current.Request.PhysicalApplicationPath & "<br/>" & vbCrLf
            M &= Current.Request.RawUrl & "<br/>" & vbCrLf
        Else
            M &= "<div>[l'oggetto HttpContext.Current.Request è nothing]</div>" & vbCrLf
        End If
        M &= "</div>" & vbCrLf

        'dati della session
        M &= "<div><br/></div>" & vbCrLf
        M &= "<div><b>Session</b></div>" & vbCrLf
        M &= "<hr/>" & vbCrLf
        M &= "<div>" & vbCrLf
        If Current.Session IsNot Nothing Then
            M &= "<b>IsNewSession</b> = " & Current.Session.IsNewSession.ToString & "<br/>" & vbCrLf
            M &= "<b>SessionID</b> = " & Current.Session.SessionID.ToString & "<br/>" & vbCrLf
            For i = 0 To Current.Session.Count - 1
                Try
                    M &= "<b>" & Current.Session.Keys(i) & "</b> = " & Current.Session(i).ToString & "<br/>" & vbCrLf
                Catch ex As Exception
                End Try
            Next
        Else
            M &= "<div>[l'oggetto HttpContext.Current.Session è nothing]</div>" & vbCrLf
        End If
        M &= "</div>" & vbCrLf

        'dati errore
        Dim ErrorInfo As Exception = Current.Server.GetLastError
        M &= "<div><br/></div>" & vbCrLf
        M &= "<div><b>Testo Eccezione</b></div>" & vbCrLf
        M &= "<hr/>" & vbCrLf
        M &= "<div>" & vbCrLf
        If ErrorInfo IsNot Nothing Then
            M &= ErrorInfo.ToString.Replace(vbCrLf, "<br/>") & "<br/>"
            M &= "<b>Error Source:</b> " & ErrorInfo.Source.ToString & "<br/>"
        Else
            M &= "<div>[l'oggetto HttpContext.Current.Server.GetLastError è nothing]</div>" & vbCrLf
        End If
        M &= "</div>" & vbCrLf

        'dati post
        M &= "<div><br/></div>" & vbCrLf
        M &= "<div><b>Dati POST</b></div>" & vbCrLf
        M &= "<hr/>" & vbCrLf
        M &= "<div>" & vbCrLf
        If Current.Request.Form IsNot Nothing Then
            For i = 0 To Current.Request.Form.Count - 1
                M &= "<b>" & Current.Request.Form.Keys(i) & "</b> = " & Current.Request.Form(i) & "<br/>" & vbCrLf
            Next
        Else
            M &= "<div>[l'oggetto HttpContext.Current.Request.Form è nothing]</div>" & vbCrLf
        End If
        M &= "</div>" & vbCrLf

        'server variables
        M &= "<div><br/></div>" & vbCrLf
        M &= "<div><b>Server Variables</b></div>" & vbCrLf
        M &= "<hr/>" & vbCrLf
        M &= "<div>" & vbCrLf
        If Current.Request.ServerVariables IsNot Nothing Then
            For i = 0 To Current.Request.ServerVariables.Count - 1
                M &= "<b>" & Current.Request.ServerVariables.Keys(i) & "</b> = " & Current.Request.ServerVariables(i) & "<br/>" & vbCrLf
            Next
        Else
            M &= "<div>[l'oggetto HttpContext.Current.ServerVariables è nothing]</div>" & vbCrLf
        End If
        M &= "</div>" & vbCrLf
        M &= "</body></html>"

        Return M
    End Function
End Class