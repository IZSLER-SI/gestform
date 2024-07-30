Imports Softailor.Global.SqlUtils

Partial Public Class ChooseBinary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim rCODCATEG As String = ""

        'PAGINA PROVVISORIA
        'cerca solo all'interno di una categoria
        'tuttavia usa la stored procedure
        '   sp_bd_searchELEMEN_UTENTI
        'che è già "pronta" per cercare con vari parametri

        'questa pagina viene aperta "stand-alone" oppure passando CODCATEG
        If Not Request.QueryString("CODCATEG") Is Nothing Then
            rCODCATEG = Request("CODCATEG").ToUpper
        End If

        Dim dbConn As SqlConnection = DbConnectionHandler.GetOpenDataDbConn()
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bd_searchELEMEN_UTENTI"
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ContextHandler.ID_AZIEN
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = EmptyToDbNull(rCODCATEG)
            .Parameters.Add("@codforma", SqlDbType.NVarChar, 16).Value = DBNull.Value
            .Parameters.Add("@codforba", SqlDbType.NVarChar, 16).Value = DBNull.Value
            .Parameters.Add("@desel_tx", SqlDbType.NVarChar, 200).Value = DBNull.Value
        End With

        'trasformazione
        Transformer.Transform(dbCmd, "Templates/ChooseBinary.xslt", phdContent)

        dbConn.Close()
    End Sub

End Class