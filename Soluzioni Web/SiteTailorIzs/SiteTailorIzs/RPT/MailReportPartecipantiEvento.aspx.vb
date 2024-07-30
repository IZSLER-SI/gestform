Public Class MailReportPartecipantiEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId, IReportGenerationPage, IMailReportGenerationPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub GetReportParameters(ByRef ac_FONTE As String, ByRef valoreFiltroBase As String, ByRef storageSubFolder As String) Implements IReportGenerationPage.GetReportParameters

        ac_FONTE = "PARTEVENTI"
        valoreFiltroBase = GecFinalContextHandler.id_EVENTO.ToString

    End Sub

    Public Function GetMailParameters() As MailSenderData Implements IMailReportGenerationPage.GetMailParameters

        Dim msd As New MailSenderData

        Dim dbConn = DbConnectionHandler.GetOpenDataDbConn

        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_GetDatiSegreteriaOrganizzativaEvento"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        dbrdr.Read()
        msd.ragionesociale = dbrdr.GetString(0)
        msd.indirizzocompleto = dbrdr.GetString(1)
        msd.tel = dbrdr.GetString(2)
        msd.fax = dbrdr.GetString(3)
        msd.email = dbrdr.GetString(4)
        dbrdr.Close()
        dbCmd.Dispose()

        dbConn.Close()
        dbConn.Dispose()

        Return msd

    End Function
End Class