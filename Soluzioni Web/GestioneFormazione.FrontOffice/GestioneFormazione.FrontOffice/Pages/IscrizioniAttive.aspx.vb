Imports System.Net

Public Class IscrizioniAttive
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_RemindersHomePage_v2"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With


        pnl1.Visible = Not ContextHandler.fl_SPID
        pnl2.Visible = ContextHandler.fl_SPID

        Transformer.Transform(dbCmd, fpd.baseTemplates & "IscrizioniAttive.xslt",
                      phdContent,
                      "node", "iscrizioni-attive",
                      "region", ContextHandler.RegionString,
                      "companyname", My.Settings.CompanyName_Short)

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "iscrizioni-attive"

    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Gli eventi formativi a cui sono iscritto"

    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'vado alla home se sono uscito
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Response.Redirect("/", True)
        End If

    End Sub

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage

        Return False

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

End Class