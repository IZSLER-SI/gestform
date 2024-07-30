Imports Softailor.Global.StructuredUtils
Imports Softailor.Global.AspxCleaner

Public Class CriterioAccesso
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Dim id_CRITERIO As Integer

    'controlli
    Dim rblac_DIPEXT As RadioButtonList
    Dim cblac_UNITAOPERATIVA As CheckBoxList
    Dim rblfl_PROFILI As RadioButtonList
    Dim cblac_PROFILO As CheckBoxList
    Dim rblfl_PRODISCECMACCR As RadioButtonList
    Dim rblac_DESTINAZIONE As RadioButtonList

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'determino id criterio
        If Request("id") IsNot Nothing Then
            id_CRITERIO = CInt(Request("id"))
        Else
            id_CRITERIO = 0
        End If

        'titoli e bottone chiusura
        If id_CRITERIO = 0 Then
            ltrTitle.Text = "Aggiunta criterio di accesso all'evento"
            ltrClose.Text = "Annulla"
            lblSave.Text = "Aggiungi criterio e chiudi"
        Else
            ltrTitle.Text = "Modifica criterio di accesso all'evento"
            ltrClose.Text = "Chiudi"
            lblSave.Text = "Salva modifiche e chiudi"
        End If

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'gestione degli script
        Dim sScript = "function invokeSave() { " & vbCrLf
        sScript &= ClientScript.GetPostBackClientHyperlink(lnkSave, "").Replace("javascript:", "")
        sScript &= "}" & vbCrLf
        Me.ltrRepositioning.Text = sScript

        'generazione controlli
        GeneraControlliCriterio()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub CriterioAccesso_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click

        If ValidateMe() Then
            SaveMe()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('REFRESH');", True)
        End If

    End Sub

    Private Function ValidateMe() As Boolean
        Return True
    End Function

    Private Sub SaveMe()

        Dim dbCmd As SqlCommand
        Dim prmid_CRITERIO_out As SqlParameter

        Dim unitaoperative As New GenericStringList
        Dim profili As New GenericStringList

        'popolamento UO
        If rblac_DIPEXT.SelectedValue = "DIP_UO" Then
            For Each li As ListItem In cblac_UNITAOPERATIVA.Items
                If li.Selected Then unitaoperative.Add(li.Value)
            Next
        End If

        'popolamento profili
        If rblfl_PROFILI.SelectedValue = "1" Then
            For Each li As ListItem In cblac_PROFILO.Items
                If li.Selected Then profili.Add(li.Value)
            Next
        End If

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_AddUpdateCriterio"

            'evento
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO

            'criterio in ingresso
            With .Parameters.Add("@id_CRITERIO_in", SqlDbType.Int)
                If id_CRITERIO = 0 Then .Value = DBNull.Value Else .Value = id_CRITERIO
            End With

            'utente
            .Parameters.Add("@tx_UTENTE", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME

            'dipendente/esterno
            .Parameters.Add("@ac_DIPEXT", SqlDbType.NVarChar, 10).Value = rblac_DIPEXT.SelectedValue

            'profili
            .Parameters.Add("@fl_PROFILI", SqlDbType.Bit).Value = rblfl_PROFILI.SelectedValue = "1"

            'professioni e discipline ECM
            With .Parameters.Add("@fl_PRODISCECMACCR", SqlDbType.Bit)
                If rblfl_PRODISCECMACCR IsNot Nothing Then
                    Select Case rblfl_PRODISCECMACCR.SelectedValue
                        Case "" : .Value = DBNull.Value
                        Case "0" : .Value = False
                        Case "1" : .Value = True
                    End Select
                Else
                    .Value = DBNull.Value
                End If
            End With

            'destinazione
            With .Parameters.Add("@ac_DESTINAZIONE", SqlDbType.NVarChar, 10)
                If rblac_DESTINAZIONE IsNot Nothing Then
                    .Value = rblac_DESTINAZIONE.SelectedValue
                Else
                    .Value = "ACC_Q1"
                End If
            End With

            'unità operative
            .Parameters.Add("@unitaoperative", SqlDbType.Structured).Value = unitaoperative.GetTable

            'profili
            .Parameters.Add("@profili", SqlDbType.Structured).Value = profili.GetTable

            'uscita
            prmid_CRITERIO_out = .Parameters.Add("@id_CRITERIO_out", SqlDbType.Int)
            prmid_CRITERIO_out.Direction = ParameterDirection.Output

        End With

        'eseguo
        dbCmd.ExecuteNonQuery()

    End Sub

    Private Sub GeneraControlliCriterio()

        Dim dbCmd As SqlCommand
        Dim cCreato As Control

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_CriterioAccessoXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_criterio", SqlDbType.Int).Value = id_CRITERIO 'può essere ZERO
        End With

        Dim sAspx = Transformer.Transform(dbCmd, "Templates/CriterioAccesso_Criterio.xslt", "companyname", My.Settings.CompanyName)
        CleanAspx(sAspx)

        cCreato = Me.ParseControl(sAspx)

        phdCriterio.Controls.Clear()
        phdCriterio.Controls.Add(cCreato)

        'aggancio controlli
        'controlli
        rblac_DIPEXT = CType(cCreato.FindControl("rblac_DIPEXT"), RadioButtonList)
        cblac_UNITAOPERATIVA = CType(cCreato.FindControl("cblac_UNITAOPERATIVA"), CheckBoxList)
        rblfl_PROFILI = CType(cCreato.FindControl("rblfl_PROFILI"), RadioButtonList)
        cblac_PROFILO = CType(cCreato.FindControl("cblac_PROFILO"), CheckBoxList)
        rblfl_PRODISCECMACCR = CType(cCreato.FindControl("rblfl_PRODISCECMACCR"), RadioButtonList)
        rblac_DESTINAZIONE = CType(cCreato.FindControl("rblac_DESTINAZIONE"), RadioButtonList)

    End Sub
End Class