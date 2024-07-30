Public Interface IFOPage

    Function GetSubTitle() As String

    Function GetMainMenuNodeKey() As String

    Sub SetFOPageData(fpd As FOPageData)

    Sub HandleRegionChange()

    Function CheckAccess() As Boolean

    Function IsCompleteProfilePage() As Boolean

End Interface

Public Class FOPageData

    Public dbConn As SqlConnection
    Public basePathCompany As String
    Public baseTemplates As String
    Public baseTemplatesContent As String

End Class
