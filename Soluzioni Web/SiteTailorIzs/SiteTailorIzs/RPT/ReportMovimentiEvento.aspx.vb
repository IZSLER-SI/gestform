Imports Softailor.ReportEngine

Public Class ReportMovimentiEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId, IReportGenerationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub GetReportParameters(ByRef ac_FONTE As String, ByRef valoreFiltroBase As String, ByRef storageSubFolder As String) Implements IReportGenerationPage.GetReportParameters

        ac_FONTE = "MOVIMENTIEVENTO"
        valoreFiltroBase = GecFinalContextHandler.id_EVENTO.ToString
        storageSubFolder = SharepointHelper.f_Eventi & "/" & SharepointHelper.f_EventoPrefix & GecFinalContextHandler.id_EVENTO.ToString

    End Sub
End Class