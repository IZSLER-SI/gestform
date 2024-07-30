Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://schemas.softailor.com/AutoCompleteCapComuneProvincia")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class AutoCompleteCapComuneProvincia
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetSuggestions(prefixText As String, count As Integer) As String()

        Dim list As New List(Of String)

        If prefixText.Trim <> "" Then

            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = DbConnectionHandler.GetOpenDataDbConn

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT TOP " & count.ToString & " tx_CAPCOMUNEPROVINCIA FROM vw_geo_CAP_COMUNI WHERE " & _
                               "tx_CAPCOMUNEPROVINCIA like @tx_capcomuneprovincia AND " & _
                               "ac_COMUNE not like 'Z%' and fl_ATTUALE=1 and ac_CAP is not null ORDER BY tx_COMUNE, ac_CAP"
                .Parameters.Add("@tx_capcomuneprovincia", SqlDbType.NVarChar, 500).Value = "%" & prefixText & "%"
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                Do While .Read
                    list.Add(dbRdr.GetString(0))
                Loop
            End With
            dbRdr.Close()
            dbCmd.Dispose()

            dbConn.Close()
            dbConn.Dispose()

        End If

        Return list.ToArray

    End Function

End Class