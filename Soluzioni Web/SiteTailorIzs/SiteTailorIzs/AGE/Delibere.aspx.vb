Public Class Delibere
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsDELIBERE_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

    Private Sub srcDELIBERE_CreateNew(dataDbConn As SqlConnection, sqlValues As Dictionary(Of String, Object), ByRef errorEncountered As Boolean, ByRef errorMessage As String, ByRef gotoNewKey As Boolean, ByRef newKeyFieldName As String, ByRef newKeyValue As String) Handles srcDELIBERE.CreateNew

        Dim id_DELIBERA As New SqlInt32
        Dim errorMsg As String = ""

        With sqlValues
            CreaNuovo_DELIBERE(dataDbConn,
                               ContextHandler.ID_UTENT,
                               CType(.Item("dt_DATA"), SqlDateTime),
                               CType(.Item("ac_DELIBERA"), SqlString),
                               CType(.Item("tx_DELIBERA"), SqlString),
                               CType(.Item("ac_TIPOLOGIADELIBERA"), SqlString),
                               id_DELIBERA,
                               errorMsg)

        End With

        If id_DELIBERA.IsNull Then
            errorEncountered = True
            errorMessage = errorMsg
        Else
            errorEncountered = False
            gotoNewKey = True
            newKeyFieldName = "id_DELIBERA"
            newKeyValue = id_DELIBERA.Value.ToString
        End If

    End Sub

    Friend Sub CreaNuovo_DELIBERE(ByVal dbConn As SqlConnection,
                                  ByVal id_UTENT As Integer,
                                  ByVal dt_DATA As SqlDateTime,
                                  ByVal ac_DELIBERA As SqlString,
                                  ByVal tx_DELIBERA As SqlString,
                                  ByVal ac_TIPOLOGIADELIBERA As SqlString,
                                  ByRef id_DELIBERA As SqlInt32,
                                  ByRef errorMsg As String)

        id_DELIBERA = SqlInt32.Null
        errorMsg = ""

        'creazione di un nuovo FORNIT e restituzione di ID_FORNI
        Dim dbCmd As SqlCommand = dbConn.CreateCommand()
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_age_DELIBERE_insert"

            .Parameters.Add("@id_UTENT", SqlDbType.Int).Value = id_UTENT
            .Parameters.Add("@dt_DATA", SqlDbType.DateTime).Value = dt_DATA
            .Parameters.Add("@ac_DELIBERA", SqlDbType.NVarChar, 16).Value = ac_DELIBERA
            .Parameters.Add("@tx_DELIBERA", SqlDbType.NVarChar, 200).Value = tx_DELIBERA
            .Parameters.Add("@ac_TIPOLOGIADELIBERA", SqlDbType.NVarChar, 16).Value = ac_TIPOLOGIADELIBERA
            With .Parameters.Add("@id_DELIBERA", SqlDbType.Int)
                .Direction = ParameterDirection.Output
            End With

            Try
                .ExecuteNonQuery()
                id_DELIBERA = CType(.Parameters("@id_DELIBERA").SqlValue, SqlInt32)
            Catch ex As Exception
                errorMsg = "Impossibile creare la delibera. Errore restituito:" & ex.ToString
            End Try
        End With
        dbCmd.Dispose()

    End Sub
End Class