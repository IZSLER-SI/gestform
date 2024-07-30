Imports Softailor.Global.XmlParser

Public Class Events
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Consente di impostare i filtri di ricerca
        If Not Page.IsPostBack Then
            GeneraCriteri()

            'se ho qualcosa nella session, la riscrivo
            'per gestire il BACK dalla lista

            If Session("sev_hidMeseAnno") IsNot Nothing Then
                Try
                    ddnMeseAnno.SelectedValue = CStr(Session("sev_hidMeseAnno"))
                    hidMeseAnno.Value = CStr(Session("sev_hidMeseAnno"))
                Catch ex As Exception
                End Try
            End If
            If Session("sev_hidProfilo") IsNot Nothing Then
                Try
                    ddnProfilo.SelectedValue = CStr(Session("sev_hidProfilo"))
                    hidProfilo.Value = CStr(Session("sev_hidProfilo"))
                Catch ex As Exception
                End Try
            End If
            If Session("sev_hidSede") IsNot Nothing Then
                Try
                    ddnSede.SelectedValue = CStr(Session("sev_hidSede"))
                    hidSede.Value = CStr(Session("sev_hidSede"))
                Catch ex As Exception
                End Try

            End If
            If Session("sev_hidEcm") IsNot Nothing Then
                Try
                    ddnEcm.SelectedValue = CStr(Session("sev_hidEcm"))
                    hidEcm.Value = CStr(Session("sev_hidEcm"))
                Catch ex As Exception
                End Try
            End If
            If Session("sev_hidTitolo") IsNot Nothing Then
                txtTitolo.Text = CStr(Session("sev_hidTitolo"))
                hidTitolo.Value = CStr(Session("sev_hidTitolo"))
            End If
            If Session("sev_hidSearchActive") IsNot Nothing Then
                hidSearchActive.Value = CStr(Session("sev_hidSearchActive"))
            End If

        End If

        GeneraListaEventi()

    End Sub

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "eventi"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Offerta Formativa"
    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData
        Me.fpd = fpd
    End Sub

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'criteri: provo a conservare i dati immessi
        Dim oldMeseAnno = ddnMeseAnno.SelectedValue
        Dim oldProfilo = ddnProfilo.SelectedValue
        Dim oldSede = ddnSede.SelectedValue
        Dim oldEcm = ddnEcm.SelectedValue

        'rigenero i dati nelle liste criteri
        GeneraCriteri()

        'provo a re-immettere i valori
        Try
            ddnMeseAnno.SelectedValue = oldMeseAnno
        Catch ex As Exception
        End Try

        Try
            ddnProfilo.SelectedValue = oldProfilo
        Catch ex As Exception
        End Try

        Try
            ddnSede.SelectedValue = oldSede
        Catch ex As Exception
        End Try

        Try
            ddnEcm.SelectedValue = oldEcm
        Catch ex As Exception
        End Try


        'devo rigenerare la lista degli eventi
        GeneraListaEventi()


    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'posso accedere sempre
        Return True

    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return False
    End Function

    Private Sub GeneraListaEventi()

        Dim filtertype As String
        If RouteData.Values.Count > 0 Then
            filtertype = RouteData.Values("filtertype").ToString
        End If
        filtertype = If(filtertype Is Nothing, "nxt", filtertype)

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
            .Parameters.Add("@ni_MAX", SqlDbType.Int).Value = 5000
            .Parameters.Add("@ac_STATOISCRIZIONI", SqlDbType.NVarChar, 10).Value = DBNull.Value
        End With
        Dim xReader = dbCmd.ExecuteXmlReader
        Dim xDoc As New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        'eventuale applicazione filtri
        If hidSearchActive.Value = "1" Then
            FiltraElencoEventi(xDoc)
        End If

        FiltraFilterType(xDoc, filtertype)

        phdEventi.Controls.Clear()
        Transformer.Transform(xDoc, fpd.baseTemplates & "Common_EventList.xslt",
                              phdEventi,
                              "node", "eventi",
                              "searchactive", If(hidSearchActive.Value = "", "0", "1"),
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short,
                              "filtertype", filtertype)
        updEventi.Update()

    End Sub

    Private Sub GeneraCriteri()

        Dim xNode As XmlNode
        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_EventiIscrizionePossibile_Criteri"
            .Parameters.Add("@dt_DATAOGGI", SqlDbType.DateTime).Value = Date.Today
            With .Parameters.Add("@id_PERSONA", SqlDbType.Int)
                If ContextHandler.Region = ContextHandler.Regions.LoggedIn Then
                    .Value = ContextHandler.id_PERSONA
                Else
                    .Value = DBNull.Value
                End If
            End With
        End With
        Dim xReader = dbCmd.ExecuteXmlReader
        Dim xDoc As New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        'mese e anno
        ddnMeseAnno.Items.Clear()
        ddnMeseAnno.Items.Add(New ListItem("Seleziona...", ""))
        For Each xNode In xDoc.SelectNodes("/dati/meseanno")

            Dim mese = ParseXmlInteger(xNode, "mese")
            Dim anno = ParseXmlInteger(xNode, "anno")
            Dim data = New Date(anno, mese, 1)
            ddnMeseAnno.Items.Add(New ListItem(data.ToString("MMMM yyyy"), data.ToString("yyyyMM")))
        Next

        'sede
        ddnSede.Items.Clear()
        ddnSede.Items.Add(New ListItem("Seleziona...", ""))
        For Each xNode In xDoc.SelectNodes("/dati/sede")
            ddnSede.Items.Add(New ListItem(ParseXmlString(xNode, "tx_sede"), ParseXmlString(xNode, "id_sede")))
        Next

        'accreditamento ECM
        ddnEcm.Items.Clear()
        ddnEcm.Items.Add(New ListItem("Seleziona...", ""))
        For Each xNode In xDoc.SelectNodes("/dati/ecm")
            ddnEcm.Items.Add(New ListItem(ParseXmlString(xNode, "tx_accreditato"), ParseXmlString(xNode, "fl_accreditato")))
        Next

        'profili
        ddnProfilo.Items.Clear()
        ddnProfilo.Items.Add(New ListItem("Seleziona...", ""))
        For Each xNode In xDoc.SelectNodes("/dati/profilo")
            ddnProfilo.Items.Add(New ListItem(ParseXmlString(xNode, "tx_profilo_plur"), ParseXmlString(xNode, "ac_profilo")))
        Next


        updSearchEventi.Update()


    End Sub

    Private Sub lnkCerca_Click(sender As Object, e As EventArgs) Handles lnkCerca.Click

        'pulizia
        txtTitolo.Text = txtTitolo.Text.Trim

        'copia nei controlli nascosti
        hidMeseAnno.Value = ddnMeseAnno.SelectedValue
        hidProfilo.Value = ddnProfilo.SelectedValue
        hidSede.Value = ddnSede.SelectedValue
        hidEcm.Value = ddnEcm.SelectedValue
        hidTitolo.Value = txtTitolo.Text
        hidSearchActive.Value = "1"

        'copio dati nella sessione
        CopyHiddenToSession()

        'esecuzione ricerca
        GeneraListaEventi()

    End Sub

    Private Sub lnkPulisci_Click(sender As Object, e As EventArgs) Handles lnkPulisci.Click

        'pulizia completa
        ddnMeseAnno.SelectedValue = ""
        ddnProfilo.SelectedValue = ""
        ddnSede.SelectedValue = ""
        ddnEcm.SelectedValue = ""
        txtTitolo.Text = ""

        hidMeseAnno.Value = ""
        hidProfilo.Value = ""
        hidSede.Value = ""
        hidEcm.Value = ""
        hidTitolo.Value = ""
        hidSearchActive.Value = ""

        'copio dati nella sessione
        CopyHiddenToSession()

        GeneraListaEventi()

    End Sub

    Private Sub CopyHiddenToSession()

        Session("sev_hidMeseAnno") = hidMeseAnno.Value
        Session("sev_hidProfilo") = hidProfilo.Value
        Session("sev_hidSede") = hidSede.Value
        Session("sev_hidEcm") = hidEcm.Value
        Session("sev_hidTitolo") = hidTitolo.Value
        Session("sev_hidSearchActive") = hidSearchActive.Value
    End Sub

#Region "Filtro"
    Private Sub FiltraElencoEventi(xDoc As XmlDocument)

        Dim eNode As XmlNode
        Dim isMatch As Boolean

        For Each eNode In xDoc.SelectNodes("/eventi/evento")

            isMatch = True

            'filtro mese-anno
            If isMatch Then
                If hidMeseAnno.Value <> String.Empty Then

                    Dim eDate = ParseXmlDateOnly(eNode, "dt_inizio")
                    Dim fDate = New Date(CInt(Mid(hidMeseAnno.Value, 1, 4)), CInt(Mid(hidMeseAnno.Value, 5, 2)), 1)

                    If Not (eDate.Month = fDate.Month And eDate.Year = fDate.Year) Then isMatch = False

                End If
            End If

            'filtro sede
            If isMatch Then
                If hidSede.Value <> String.Empty Then
                    If ParseXmlString(eNode, "id_sede") <> hidSede.Value Then isMatch = False
                End If
            End If

            'filtro ecm
            If isMatch Then
                If hidEcm.Value <> String.Empty Then
                    If ParseXmlString(eNode, "fl_ecm") <> hidEcm.Value Then isMatch = False
                End If
            End If

            'filtro profili
            If isMatch Then
                If hidProfilo.Value <> String.Empty Then
                    'solo se l'evento prevede un filtro sui profili
                    If ParseXmlBoolean01(eNode, "fl_profili") = True Then
                        Dim found = False
                        For Each pNode As XmlNode In eNode.SelectNodes("profilo")
                            If hidProfilo.Value = ParseXmlString(pNode, "ac_profilo") Then
                                found = True
                                Exit For
                            End If
                        Next
                        If Not found Then isMatch = False
                    End If
                End If
            End If

            'filtro titolo
            If isMatch Then
                If hidTitolo.Value <> String.Empty Then
                    If Not ParseXmlString(eNode, "tx_titolo").ToLower Like "*" & hidTitolo.Value.ToLower & "*" Then isMatch = False
                End If
            End If

            'imposto match
            If Not isMatch Then
                eNode.Attributes("fl_match").Value = "0"
            End If

        Next

    End Sub

    Private Sub FiltraFilterType(xDoc As XmlDocument, filtertype As String)

        Dim eNode As XmlNode
        Dim isMatch As Boolean

        For Each eNode In xDoc.SelectNodes("/eventi/evento")

            isMatch = True

            ' filtro sul tipo di sottomenu selezionato
            Select Case filtertype
                Case "nxt"
                    If isMatch Then
                        Dim eDate = ParseXmlDateOnly(eNode, "dt_inizio")
                        Dim fDate = Date.Today

                        If Not (Not ParseXmlBoolean01(eNode, "fl_fad") And DateTime.Compare(eDate, fDate) >= 0) Then isMatch = False
                    End If
                Case "fad"
                    If isMatch Then
                        Dim eDate = ParseXmlDateOnly(eNode, "dt_fine")
                        Dim fDate = Date.Today

                        If Not (ParseXmlBoolean01(eNode, "fl_fad") And DateTime.Compare(eDate, fDate) >= 0) Then isMatch = False
                    End If
                Case Else

            End Select

            'imposto match
            If Not isMatch Then
                eNode.Attributes("fl_match").Value = "0"
            End If

        Next

    End Sub
#End Region
End Class