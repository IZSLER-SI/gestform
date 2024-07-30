Public Class GenericContent
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData
    Dim gContentKey As String

    Private Sub GenericContent_Init(sender As Object, e As EventArgs) Handles Me.Init


        'chiave
        gContentKey = CType(Me.RouteData.Route, Route).Url

        'contenuto
        Transformer.Transform(New XmlDocument,
                                     fpd.baseTemplatesContent & "GenericContent_Content.xslt",
                                     phdContent,
                                     "gcontentkey", gContentKey.ToLower)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return Transformer.Transform(New XmlDocument,
                                     fpd.baseTemplatesContent & "GenericContent_Title.xslt",
                                     "gcontentkey", gContentKey.ToLower)

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        'non è mai nel main menu
        Return ""

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData
        Me.fpd = fpd
    End Sub

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange
        'nessuna necessità di ri-modificare il contenuto in base al cambio di regione
    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'posso accedere sempre
        Return True

    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return False
    End Function
End Class