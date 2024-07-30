Public Class RootMP
    Inherits System.Web.UI.MasterPage

    'dati pagina
    Dim fpd As FOPageData

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'expiry
        Response.Cache.SetNoStore()
        Response.Expires = 0

        'controllo accesso base
        If Not CType(Me.Page, IFOPage).CheckAccess Then
            Response.Redirect("/")
            Exit Sub
        End If

        'se sono in zona completamento profilo ma sono andato in un'altra pagina > logout
        If ContextHandler.Region = ContextHandler.Regions.CompleteProfile Then
            If Not CType(Me.Page, IFOPage).IsCompleteProfilePage Then
                ContextHandler.NewEmptySession()
            End If
        End If

        'pulisco tutti i dati della ricerca, A MENO CHE io sia nella scheda evento o nella lista eventi
        If CType(Me.Page, IFOPage).GetMainMenuNodeKey <> "eventi" Then
            Session.Remove("sev_hidMeseAnno")
            Session.Remove("sev_hidProfilo")
            Session.Remove("sev_hidSede")
            Session.Remove("sev_hidEcm")
            Session.Remove("sev_hidTitolo")
            Session.Remove("sev_hidSearchActive")
        End If

        'dati pagina
        fpd = New FOPageData

        'apertura connessione
        fpd.dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        fpd.dbConn.Open()

        'path di base company (viene TRASLATO in automatico)
        fpd.basePathCompany = "/Content/" & My.Settings.CompanyKey & "/"
        'path di base templates (relativo)
        fpd.baseTemplates = "~/Templates/" & My.Settings.CompanyKey & "/"
        'path templates/content (relativo)
        fpd.baseTemplatesContent = "~/Templates/" & My.Settings.CompanyKey & "/GenericContent/"

        'passo i dati alla pagina
        CType(Me.Page, IFOPage).SetFOPageData(fpd)

        'favicon
        favicon.Href = fpd.basePathCompany & "FavIcon/favicon.ico"

        'css global
        globalcss.Href = fpd.basePathCompany & "Styles/global.css"

        'elementi pagina
        RenderHeader()
        RenderFooter()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'titolo pagina
        Dim subTitle = CType(Me.Page, IFOPage).GetSubTitle
        If subTitle = String.Empty Then
            Me.Page.Title = My.Settings.PageTitle
        Else
            Me.Page.Title = My.Settings.PageTitle & " - " & subTitle
        End If

    End Sub

    Private Sub RenderHeader()

        Transformer.Transform(New XmlDocument,
                              fpd.baseTemplates & "Header.xslt",
                              phdHeader,
                              "anno", Date.Today.Year.ToString)

    End Sub

    Private Sub RenderFooter()

        Transformer.Transform(New XmlDocument,
                              fpd.baseTemplates & "Footer.xslt",
                              phdFooter,
                              "anno", Date.Today.Year.ToString)

    End Sub

    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If fpd IsNot Nothing Then
            If fpd.dbConn IsNot Nothing Then
                If fpd.dbConn.State = ConnectionState.Open Then
                    fpd.dbConn.Close()
                End If
                fpd.dbConn.Dispose()
            End If
        End If
    End Sub

    Public Function GetFpd() As FOPageData
        Return fpd
    End Function
End Class