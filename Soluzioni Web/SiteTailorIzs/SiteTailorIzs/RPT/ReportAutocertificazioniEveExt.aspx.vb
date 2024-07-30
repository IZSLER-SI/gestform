Imports Softailor.ReportEngine

Public Class ReportAutocertificazioniEveExt
    Inherits System.Web.UI.Page
    Implements IReportGenerationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub GetReportParameters(ByRef ac_FONTE As String, ByRef valoreFiltroBase As String, ByRef storageSubFolder As String) Implements IReportGenerationPage.GetReportParameters

        ac_FONTE = "AUTOC"
        valoreFiltroBase = ""
        storageSubFolder = SharepointHelper.f_Generici

    End Sub

End Class