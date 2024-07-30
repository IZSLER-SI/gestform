Imports Softailor.Global

Public Class Portfolio
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bo_fo_Portfolio"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@fl_BACKOFFICE", SqlDbType.Bit).Value = False
        End With
        Dim sAspx = Transformer.Transform(dbCmd, fpd.baseTemplates & "Portfolio.xslt",
                              "node", "portfolio",
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short)

        AspxCleaner.CleanAspx(sAspx)

        Dim cCreato = Me.ParseControl(sAspx)

        phdContent.Controls.Clear()
        phdContent.Controls.Add(cCreato)

        For Each c As Control In cCreato.Controls
            If TypeOf c Is LinkButton Then
                With CType(c, LinkButton)
                    If .ID Like "lnkMateriale_*" Then
                        AddHandler .Click, AddressOf lnkMateriale_Click
                    End If
                End With
            End If
        Next

    End Sub

    Private Sub lnkMateriale_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'generazione del materiale
        phdMateriale.Controls.Clear()

        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_FilesEvento"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = CInt(CType(sender, LinkButton).CommandArgument)
        End With

        Transformer.Transform(dbCmd, fpd.baseTemplates & "FilesEvento.xslt",
                              phdMateriale,
                              "node", "portfolio",
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short)

        updMateriale.Update()
        'apro il popup
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "openPopup", "showMaterialPopup(true);", True)


    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess
        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn
    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "portfolio"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Portfolio Formativo"

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