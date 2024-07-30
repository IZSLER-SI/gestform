Public Class News
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Private Sub News_Init(sender As Object, e As EventArgs) Handles Me.Init

        'determino l'ID della news
        Dim id_news As String = ""

        If RouteData.Values.Count > 0 Then
            id_news = CInt(RouteData.Values("id")).ToString
        End If

        'trasformazione
        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_NewsXml"
            .Parameters.Add("@dt_dataoggi", SqlDbType.DateTime).Value = Date.Today
            .Parameters.Add("@max_num", SqlDbType.Int).Value = 8
        End With

        Transformer.Transform(dbCmd, fpd.baseTemplates & "News.xslt",
                              phdContent,
                              "region", ContextHandler.RegionString,
                              "selectedid", id_news.ToString)

    End Sub

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "news"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Area Notizie"
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