Public Class AclHelper

    Public Shared Function AclInitForPagesWithoutMasterPage(ByVal server As System.Web.HttpServerUtility, ByVal request As System.Web.HttpRequest, ByVal response As System.Web.HttpResponse, ByVal expiresZero As Boolean) As Boolean

        'chiamata per gestione ACL

        'scadenza
        If expiresZero Then response.Expires = 0

        'controllo autorizzazione

        'ottengo il percorso su disco della pagina corrente
        Dim myPath As String = server.MapPath(request.FilePath).ToLower

        'verifico se l'utente è autorizzato
        Dim functionAuthorization As ACL.SiteTailorFunctionAuthorization

        Try
            functionAuthorization = ContextHandler.USERFUNC(myPath)
            If functionAuthorization.Disabled Then
                functionAuthorization = Nothing
            End If
        Catch ex As Exception
            functionAuthorization = Nothing
        End Try
        If functionAuthorization Is Nothing Then
            response.StatusCode = 403
            response.End()
            Return False
        Else
            Return True
        End If

    End Function

    Public Class AclInitForPagesWithoutMasterPage_FA_Result

        Public canAccess As Boolean = False
        Public functionAuthorization As ACL.SiteTailorFunctionAuthorization

    End Class

    Public Shared Function AclInitForPagesWithoutMasterPage_FA(ByVal server As System.Web.HttpServerUtility, ByVal request As System.Web.HttpRequest, ByVal response As System.Web.HttpResponse, ByVal expiresZero As Boolean) As AclInitForPagesWithoutMasterPage_FA_Result

        'chiamata per gestione ACL

        'scadenza
        If expiresZero Then response.Expires = 0

        'controllo autorizzazione

        'ottengo il percorso su disco della pagina corrente
        Dim myPath As String = server.MapPath(request.FilePath).ToLower

        'verifico se l'utente è autorizzato
        Dim functionAuthorization As ACL.SiteTailorFunctionAuthorization

        Try
            functionAuthorization = ContextHandler.USERFUNC(myPath)
            If functionAuthorization.Disabled Then
                functionAuthorization = Nothing
            End If
        Catch ex As Exception
            functionAuthorization = Nothing
        End Try

        If functionAuthorization Is Nothing Then
            response.StatusCode = 403
            response.End()
            Dim FA = New AclInitForPagesWithoutMasterPage_FA_Result
            FA.canAccess = False
            FA.functionAuthorization = Nothing
            Return FA
        Else
            Dim FA = New AclInitForPagesWithoutMasterPage_FA_Result
            FA.canAccess = True
            FA.functionAuthorization = functionAuthorization
            Return FA
        End If

    End Function
End Class
