Imports Softailor.ReportEngine

Public Class ReportEventi
    Inherits System.Web.UI.Page
    Implements IReportGenerationPage

    Public Sub GetReportParameters(ByRef ac_FONTE As String, ByRef valoreFiltroBase As String, ByRef storageSubFolder As String) Implements IReportGenerationPage.GetReportParameters

        ac_FONTE = "EVENTI"
        valoreFiltroBase = ""
        storageSubFolder = SharepointHelper.f_Generici

    End Sub
End Class