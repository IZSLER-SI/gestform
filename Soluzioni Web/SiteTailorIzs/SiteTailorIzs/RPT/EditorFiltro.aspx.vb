Imports Softailor.ReportEngine

Public Class EditorFiltro
    Inherits System.Web.UI.Page

    Dim ac_FONTE As String
    Dim fonte As Fonte
    Dim fcHandler As FiltroControlsHandler
    Dim rptDbConn As SqlConnection

    Private Sub EditorFiltro_Init(sender As Object, e As EventArgs) Handles Me.Init

        Try
            ac_FONTE = Request("fonte")
        Catch ex As Exception
            ac_FONTE = ""
        End Try

        If String.IsNullOrEmpty(ac_FONTE) Then
            Response.End()
            Exit Sub
        End If

        'apertura connessione
        rptDbConn = DbConnectionHandler.GetOpenRptDbConn

        'creo la fonte
        fonte = ReportEngine.Fonte.FromXml(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'gestore controlli
        fcHandler = New FiltroControlsHandler(fonte, Nothing, phdContent, rptDbConn)

        'generazione controlli
        fcHandler.CreateMainControls()

        'gestione caricamento del filtro
        If Not Page.IsPostBack Then
            If Request("load") IsNot Nothing Then
                Select Case Request("load")
                    Case "parent"
                        'devo caricare il filtro dal parent.

                        'script per il clic sul pulsante
                        Dim sScript = "function ClickLoadParent() {" & vbCrLf
                        sScript &= ClientScript.GetPostBackClientHyperlink(lnkLoadParent, "").Replace("javascript:", "")
                        sScript &= "}" & vbCrLf
                        Me.ltrLoadParent.Text = sScript

                        'effettuo la chiamata
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "parentxmlloader", "LoadFiltroFromParent();", True)

                    Case "db"

                End Select
            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        fcHandler.CreateValueControls()
    End Sub

    Private Sub EditorFiltro_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not rptDbConn Is Nothing Then
            If rptDbConn.State = ConnectionState.Open Then rptDbConn.Close()
            rptDbConn.Dispose()
        End If
    End Sub

    Private Sub lnkSaveClose_Click(sender As Object, e As EventArgs) Handles lnkSaveClose.Click

        If fcHandler.ValidateMe Then
            'lettura
            fcHandler.ReadMe()
            Dim xml = fcHandler.filtro.GetXml()
            'chiusura!
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "okdone", "parent.stl_sel_done(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(xml) & ");", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Dati mancanti o non validi. Controlla i dati inseriti.');", True)
        End If

    End Sub

   
    Private Sub lnkLoadParent_Click(sender As Object, e As EventArgs) Handles lnkLoadParent.Click

        Dim xml = txtParentXml.Text
        txtParentXml.Text = ""

        If xml <> String.Empty Then

            'istanzio il filtro
            Dim filtro = ReportEngine.Filtro.FromXml(xml, fonte.CampiCorpo)

            fcHandler.WriteData(filtro)

        End If

    End Sub
End Class