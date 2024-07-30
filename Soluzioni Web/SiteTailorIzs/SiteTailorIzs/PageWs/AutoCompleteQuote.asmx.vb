Imports Softailor.Global.ValidationUtils
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://schemas.softailor.com/AutoCompleteQuote")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class AutoCompleteQuote
    Inherits System.Web.Services.WebService

    Public Class DatiQuota
        Public imponibile As String = ""
        Public totale As String = ""
        Public id_iva As String = ""
    End Class

    <WebMethod()> _
    Public Function GetDatiQuota(id_quotaiscrizione As String, id_iva As String) As DatiQuota

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim dati As New DatiQuota

        dbConn = DbConnectionHandler.GetOpenDataDbConn
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_DatiQuotaIscrizione"
            .Parameters.Add("@id_quotaiscrizione", SqlDbType.Int).Value = CInt(id_quotaiscrizione)
            With .Parameters.Add("@id_iva", SqlDbType.Int)
                If id_iva = "" Then .Value = DBNull.Value Else .Value = CInt(id_iva)
            End With
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        dati.imponibile = FormatItalianCurrency(dbRdr.GetDecimal(0))
        dati.totale = FormatItalianCurrency(dbRdr.GetDecimal(1))
        If id_iva = "" Then
            dati.id_iva = dbRdr.GetInt32(2).ToString
        End If

        dbRdr.Close()
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

        Return dati

    End Function

End Class