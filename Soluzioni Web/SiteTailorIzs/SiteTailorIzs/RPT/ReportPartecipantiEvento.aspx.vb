Imports Softailor.ReportEngine

Public Class ReportPartecipantiEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId, IReportGenerationPage

    Public Sub GetReportParameters(ByRef ac_FONTE As String, ByRef valoreFiltroBase As String, ByRef storageSubFolder As String) Implements IReportGenerationPage.GetReportParameters

        ac_FONTE = "PARTEVENTI"
        valoreFiltroBase = GecFinalContextHandler.id_EVENTO.ToString
        storageSubFolder = SharepointHelper.f_Eventi & "/" & SharepointHelper.f_EventoPrefix & GecFinalContextHandler.id_EVENTO.ToString

    End Sub
End Class