Imports Softailor.Global.StructuredUtils

Public Class AttribuzioneOrari
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection
    Dim chkOrari As New Dictionary(Of Integer, CheckBox)
    Dim chkIscritti As New Dictionary(Of Integer, CheckBox)

    Private Sub AttribuzioneOrari_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione controlli
        GeneraControlli()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub AttribuzioneOrari_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraControlli()

        Dim dbCmd As SqlCommand
        Dim cCreato As Control
        Dim sAspx As String
        Dim chk As CheckBox

        'pulizie
        phdOrari.Controls.Clear()
        phdIscritti.Controls.Clear()
        chkOrari.Clear()
        chkIscritti.Clear()

        'orari
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_AttrOrari_OrariXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        sAspx = Transformer.Transform(dbCmd, "Templates/AttribuzioneOrari_Orari.xslt")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        cCreato = Me.ParseControl(sAspx)
        phdOrari.Controls.Add(cCreato)
        For Each c In cCreato.Controls
            If TypeOf c Is CheckBox Then
                chk = CType(c, CheckBox)
                If chk.ID Like "chkOrario_*" Then
                    chkOrari.Add(CInt(chk.ID.Split("_"c)(1)), chk)
                End If
            End If
        Next

        'iscritti
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_AttrOrari_IscrittiXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        sAspx = Transformer.Transform(dbCmd, "Templates/AttribuzioneOrari_Iscritti.xslt")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        cCreato = Me.ParseControl(sAspx)
        phdIscritti.Controls.Add(cCreato)
        For Each c In cCreato.Controls
            If TypeOf c Is CheckBox Then
                chk = CType(c, CheckBox)
                If chk.ID Like "chkIscritto_*" Then
                    chkIscritti.Add(CInt(chk.ID.Split("_"c)(1)), chk)
                End If
            End If
        Next

    End Sub

    Private Sub lnkGo_Click(sender As Object, e As EventArgs) Handles lnkGo.Click

        SetResponse(False, "")

        Dim lstIscritti As New GenericIntList
        Dim lstCalendario As New GenericIntList
        Dim id_ISCRITTO As Integer
        Dim id_CALENDARIO As Integer
        Dim dbCmd As SqlCommand
        Dim prmEsito As SqlParameter
        Dim esito As String

        'costruisco le liste

        For Each id_CALENDARIO In chkOrari.Keys
            If chkOrari(id_CALENDARIO).Checked Then
                lstCalendario.Add(id_CALENDARIO)
            End If
        Next

        For Each id_ISCRITTO In chkIscritti.Keys
            If chkIscritti(id_ISCRITTO).Checked Then
                lstIscritti.Add(id_ISCRITTO)
            End If
        Next

        If lstCalendario.Count = 0 Then
            SetResponse(True, "Seleziona almeno un intervallo ingresso/uscita dalla lista.")
            Exit Sub
        End If

        If lstIscritti.Count = 0 Then
            SetResponse(True, "Seleziona almeno una persona dall'elenco.")
            Exit Sub
        End If

        'OK ci siamo. Eseguo
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_AttrOrari"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_calendarios", SqlDbType.Structured).Value = lstCalendario.GetTable
            .Parameters.Add("@id_iscrittos", SqlDbType.Structured).Value = lstIscritti.GetTable
            prmEsito = .Parameters.Add("@esito", SqlDbType.NVarChar, -1)
            prmEsito.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        esito = CStr(prmEsito.Value)
        dbCmd.Dispose()

        If esito <> String.Empty Then
            SetResponse(True, esito)
        Else
            SetResponse(False, "Operazione completata correttamente.")
        End If

    End Sub

    Private Sub SetResponse(isError As Boolean, text As String)

        lblResult.Text = text
        If isError Then
            lblResult.ForeColor = Drawing.Color.Red
        Else
            lblResult.ForeColor = Drawing.Color.DarkGreen
        End If

    End Sub

End Class