Imports Softailor.Global.XmlParser

Public Class Home
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Private Sub Home_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GeneraListaEventi()

        GeneraNews()

        GeneraReminders()

    End Sub

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        'home: nessun titolo
        Return ""

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "homepage"
    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData
        Me.fpd = fpd
    End Sub

    Private Sub GeneraListaEventi()

        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_EventiIscrizionePossibile"
            .Parameters.Add("@dt_DATAOGGI", SqlDbType.DateTime).Value = Date.Today
            With .Parameters.Add("@id_PERSONA", SqlDbType.Int)
                If ContextHandler.Region = ContextHandler.Regions.LoggedIn Then
                    .Value = ContextHandler.id_PERSONA
                Else
                    .Value = DBNull.Value
                End If
            End With
            .Parameters.Add("@ni_MAX", SqlDbType.Int).Value = 5
            .Parameters.Add("@ac_STATOISCRIZIONI", SqlDbType.NVarChar, 10).Value = "APERTE"
        End With
        phdEventi.Controls.Clear()
        Transformer.Transform(dbCmd, fpd.baseTemplates & "Common_EventList.xslt",
                              phdEventi,
                              "node", "homepage",
                              "searchactive", "0",
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short)
        updEventi.Update()

    End Sub

    Private Sub GeneraReminders()

        'pulizia
        phdReminders.Controls.Clear()

        'generazione (se sono loggato)
        If ContextHandler.Region = ContextHandler.Regions.LoggedIn Then

            Dim dbCmd As SqlCommand
            Dim xReader As XmlReader
            Dim xDoc As XmlDocument
            Dim baseUrl As String
            Dim key As String

            dbCmd = fpd.dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
								.CommandText = "sp_fo_RemindersHomePage_v2"
								.Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            End With
            xReader = dbCmd.ExecuteXmlReader
            xDoc = New XmlDocument
            xDoc.Load(xReader)
            xReader.Dispose()
            dbCmd.Dispose()

            baseUrl = My.Settings.ValutazioneWeb_Url
            key = My.Settings.ValutazioneWeb_Key

            For Each eNode As XmlNode In xDoc.SelectNodes("/root/valutazioneweb")
                CreaLink(xDoc, eNode, baseUrl, key)
            Next

            Transformer.Transform(xDoc, fpd.baseTemplates & "Home_Reminders.xslt",
                                  phdReminders,
                                  "node", "homepage",
                                  "region", ContextHandler.RegionString,
                                  "companyname", My.Settings.CompanyName_Short)
        End If

        'update
        updReminders.Update()

    End Sub

    Private Sub GeneraNews()

        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_NewsXml"
            .Parameters.Add("@dt_dataoggi", SqlDbType.DateTime).Value = Date.Today
            .Parameters.Add("@max_num", SqlDbType.Int).Value = 8
        End With

        Transformer.Transform(dbCmd, fpd.baseTemplates & "Home_News.xslt",
                              phdNews,
                              "region", ContextHandler.RegionString)

    End Sub

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'devo rigenerare la lista degli eventi
        GeneraListaEventi()

        '... e anche i reminders
        GeneraReminders()

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'posso accedere sempre
        Return True

    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return False
    End Function

    Private Sub CreaLink(xDoc As XmlDocument, eNode As XmlNode, baseUrl As String, key As String)

        Dim nome = ParseXmlString(eNode, "tx_nome")
        Dim cognome = ParseXmlString(eNode, "tx_cognome")
        Dim email = ParseXmlString(eNode, "tx_email")
        Dim id_evento = ParseXmlString(eNode, "id_evento")
        Dim id_iscritto = ParseXmlString(eNode, "id_iscritto")

        Dim hash = MD5Hash(id_iscritto & id_evento & key)

        Dim link = baseUrl &
            "?q=compilazione" &
            "&from=gestionale" &
            "&chiave=" & hash &
            "&codice_utente=" & id_iscritto &
            "&codice_evento=" & id_evento &
            "&nome=" & Server.UrlEncode(nome) &
            "&cognome=" & Server.UrlEncode(cognome) &
            "&email=" & Server.UrlEncode(email)

        Dim urlAttr = xDoc.CreateAttribute("url")
        urlAttr.Value = link
        eNode.Attributes.Append(urlAttr)

    End Sub

    Private Function MD5Hash(s As String) As String

        Using MD5 = System.Security.Cryptography.MD5.Create()

            Dim retVal As Byte() = MD5.ComputeHash(Encoding.UTF8.GetBytes(s))
            Dim sb As New StringBuilder()
            For i = 0 To retVal.Length - 1
                sb.Append(retVal(i).ToString("x2"))
            Next

            MD5Hash = sb.ToString

        End Using

    End Function
End Class