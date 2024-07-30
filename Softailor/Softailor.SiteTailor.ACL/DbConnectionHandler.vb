Public Class DbConnectionHandler
    Public Shared Function GetOpenAclDbConn() As SqlConnection
        Dim dbConn As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("SiteTailorAclConnectionString").ConnectionString)
        dbConn.Open()
        Return dbConn
    End Function

    Public Shared Function GetOpenDataDbConn() As SqlConnection
        Dim dbConn As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("SiteTailorDbConnectionString").ConnectionString)
        dbConn.Open()
        Return dbConn
    End Function

    Public Shared Function GetOpenRptDbConn() As SqlConnection
        Dim dbConn As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("SiteTailorRptConnectionString").ConnectionString)
        dbConn.Open()
        Return dbConn
    End Function

    Public Shared Function GetOpenWsDbConn() As SqlConnection
        Dim dbConn As New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("SiteTailorWsConnectionString").ConnectionString)
        dbConn.Open()
        Return dbConn
    End Function
End Class
