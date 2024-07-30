Public Class Download
    Inherits System.Web.UI.Page
    Implements IFOPage


    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_FilesXml"
            .Parameters.Add("@dt_dataoggi", SqlDbType.DateTime).Value = Date.Today
        End With

        Transformer.Transform(dbCmd, fpd.baseTemplates & "Download.xslt",
                              phdContent,
                              "region", ContextHandler.RegionString)

    End Sub

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "download"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Area Download"
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