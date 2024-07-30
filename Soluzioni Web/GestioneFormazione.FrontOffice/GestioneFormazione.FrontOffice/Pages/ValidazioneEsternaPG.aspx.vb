Public Class ValidazioneEsternaPG
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData


    Public Shared id_persona As Integer
    Public Shared rix As String

    Private Sub Validazione_Load(sender As Object, e As EventArgs) Handles Me.Load
        'determino l'ID della news
        id_persona = ContextHandler.id_PERSONA
        'Esecuzione della store-procedure "sp_fo_ValidazioneEsternaPG"
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim dtDataTable As New DataTable

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ValidazioneEsternaPG"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With
        dbRdr = dbCmd.ExecuteReader
        If (dbRdr.HasRows = True) Then
            dtDataTable.Load(dbRdr)
            tabellaEventiEsterniPG.DataSource = dtDataTable
            tabellaEventiEsterniPG.DataBind()
        Else
            Response.Redirect("/")
        End If
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    'Funzione che convalida la partecipazione all'evento
    Public Function confermaPartecipazione_click(sender As Object, e As EventArgs) As Boolean
        'Recupero id_partecipazione
        Dim lbn As LinkButton = CType(sender, LinkButton)
        Dim stringa_id_partecipazione As String = lbn.CommandArgument
        Dim id_partecipazione As Integer = Convert.ToInt32(stringa_id_partecipazione)
        Dim dbCmd As SqlCommand
        Dim dtDataTable As New DataTable
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ConfermaValidazioneEsternaPG"
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_partecipazione
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
        Response.Redirect("validazione-pg")
    End Function

    'Funzione che convalida l'annullamento all'evento
    Public Function annullaPartecipazione_click(sender As Object, e As EventArgs) As Boolean
        'Recupero id_partecipazione
        Dim lbn As LinkButton = CType(sender, LinkButton)
        Dim stringa_id_partecipazione As String = lbn.CommandArgument
        Dim id_partecipazione As Integer = Convert.ToInt32(stringa_id_partecipazione)
        Dim dbCmd As SqlCommand
        Dim dtDataTable As New DataTable
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_AnnullaValidazioneEsternaPG"
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_partecipazione
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
        Response.Redirect("validazione-pg")
    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "validazione-pg"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Validazione PG"
    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData
        Me.fpd = fpd
    End Sub

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange
        'nessuna necessità di ri-modificare il contenuto in base al cambio di regione
    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'posso accedere sempre
        Return True

    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return False
    End Function
End Class