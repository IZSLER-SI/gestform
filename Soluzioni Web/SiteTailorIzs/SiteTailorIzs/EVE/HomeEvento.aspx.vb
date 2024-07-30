Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class HomeEvento
		Inherits System.Web.UI.Page
		Implements IGFNeedsEventId

		Dim dbConn As SqlConnection
		Dim eventXDoc As XmlDocument
		Protected WithEvents lnkEliminaEvento As LinkButton
		Dim sAccesLevel As String

    Private Sub HomeEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

				Dim sAspx As String
				Dim cCreato As Control

				'apertura connessione
				dbConn = DbConnectionHandler.GetOpenDataDbConn

				'livello accesso
				sAccesLevel = CType(Me.Master, SiteTailorMP).FunctionAuthorization.AccessLevel.ToString

				'lettura documento xml evento
				ReadEventXDoc()

				'generazione pannello dati evento
				sAspx = Transformer.Transform(eventXDoc, "Templates/HomeEvento_Evento.xslt", "accesslevel", sAccesLevel)
				Softailor.Global.AspxCleaner.CleanAspx(sAspx)
				cCreato = ParseControl(sAspx)
				phdDatiEvento.Controls.Clear()
				phdDatiEvento.Controls.Add(cCreato)
				'aggancio controlli
				lnkEliminaEvento = CType(cCreato.FindControl("lnkEliminaEvento"), LinkButton)

				'generazione pannello riepilogo
				sAspx = Transformer.Transform(eventXDoc, "Templates/HomeEvento_Iscritti.xslt", "accesslevel", sAccesLevel)
				Softailor.Global.AspxCleaner.CleanAspx(sAspx)
				cCreato = ParseControl(sAspx)
				phdIscritti.Controls.Clear()
				phdIscritti.Controls.Add(cCreato)
        'aggancio controlli
        For Each c As Control In cCreato.Controls
            If TypeOf c Is LinkButton Then
                With CType(c, LinkButton)
                    If .ID.StartsWith("lnkgoto_") Then
                        AddHandler .Click, AddressOf lnkGoto_click
                    End If
                End With
            End If
        Next

        'bottone partecipanti

    End Sub

		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		End Sub

		Private Sub HomeEvento_Unload(sender As Object, e As EventArgs) Handles Me.Unload
				If Not dbConn Is Nothing Then
						dbConn.Close()
						dbConn.Dispose()
				End If
		End Sub

		Private Sub ReadEventXDoc()

				Dim dbCmd As SqlCommand
				Dim xReader As XmlReader

				dbCmd = dbConn.CreateCommand
				With dbCmd
						.CommandType = CommandType.StoredProcedure
						.CommandText = "sp_eve_RiepilogoEventoXml"
						.Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
						.Parameters.Add("@dt_dataoggi", SqlDbType.DateTime).Value = Date.Today
						.Parameters.Add("@dt_dataora", SqlDbType.DateTime).Value = Date.Now
						.Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
				End With
				xReader = dbCmd.ExecuteXmlReader
				eventXDoc = New XmlDocument
				eventXDoc.Load(xReader)
				xReader.Close()
				dbCmd.Dispose()

		End Sub

    Private Sub lnkGoto_click(sender As Object, e As EventArgs)

        Dim RD As New GFRoamingData
        With CType(sender, LinkButton)
            If .ID <> "lnkgoto_" Then
                Dim key = .ID.Split("_"c)(1)
                Dim value = .ID.Split("_"c)(2)
                Select Case key
                    Case "OI"
                        RD.originiIscrizione.Add(value)
                    Case "CE"
                        RD.categorieEcm.Add(value)
                    Case "SI"
                        RD.statiIscrizione.Add(value)
                    Case "SE"
                        RD.statiEcm.Add(value)
                    Case "SQ"
                        RD.statiQuestionario.Add(value)
                    Case "SP"
                        RD.statiPresenza.Add(value)
                    Case "DE"
                        RD.categoriePersone.Add(value)
                End Select
            End If
        End With

        dbConn.Close()
        dbConn.Dispose()

        Response.Redirect("PartecipantiEvento.aspx?" & RD.GetRequestData, True)

    End Sub

    Private Sub lnkEliminaEvento_Click(sender As Object, e As EventArgs) Handles lnkEliminaEvento.Click

				Dim dbCmd = dbConn.CreateCommand
				With dbCmd
						.CommandType = CommandType.StoredProcedure
						.CommandText = "sp_eve_EliminaEvento"
						.Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
						.Parameters.Add("@id_UTENT", SqlDbType.Int).Value = ContextHandler.ID_UTENT
				End With
				dbCmd.ExecuteNonQuery()
				dbCmd.Dispose()

				dbConn.Close()
				dbConn.Dispose()

				'redirect
				Response.Redirect(ResolveUrl("~/0/EVE/SelezioneEvento.aspx"))

		End Sub


End Class