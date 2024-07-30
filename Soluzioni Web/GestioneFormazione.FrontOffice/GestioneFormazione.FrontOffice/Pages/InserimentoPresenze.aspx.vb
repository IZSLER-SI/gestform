Public Class InserimentoPresenze
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData

    Dim id_EVENTO As Integer

    Dim eventXmlDoc As XmlDocument

    'controlli creati dinamicamente
    Protected WithEvents lnkSave As LinkButton
    Dim ddnData As DropDownList
    Const nOrari As Integer = 5
    Dim txtDa As New Dictionary(Of Integer, TextBox)
    Dim txtA As New Dictionary(Of Integer, TextBox)
    Dim cblDRMT As CheckBoxList
    Dim cblP As CheckBoxList
    Dim errData As Label
    Dim errOrari As Label
    Dim errDRMT As Label
    Dim errP As Label
    Dim errGlobal As Label

    Private Sub InserimentoPresenze_Init(sender As Object, e As EventArgs) Handles Me.Init

        'esco se non ho l'area corretta
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        'verifica ID evento
        If Not RouteData.Values.ContainsKey("id") Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        Try
            id_EVENTO = CInt(RouteData.Values("id"))
        Catch ex As Exception
            id_EVENTO = 0
        End Try

        If id_EVENTO <= 0 Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        'caricamento documento XML
        LoadEventXmlDoc()

        'se non ho un evento > esco
        If eventXmlDoc.SelectNodes("/evento").Count = 0 Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        'generazione controlli
        GeneraControlli()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "inserimento-presenze"

    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Registrazione date svolgimento / orari / presenze"

    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'vado alla home se sono uscito
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Response.Redirect("/", True)
        End If

    End Sub

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage

        Return False

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

#Region "gestione propria di questa pagina"
    Private Sub LoadEventXmlDoc()

        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ip_DatiEventoXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_EVENTO
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With
        xReader = dbCmd.ExecuteXmlReader
        eventXmlDoc = New XmlDocument
        eventXmlDoc.Load(xReader)
        xReader.Close()
        xReader.Dispose()
        dbCmd.Dispose()

    End Sub

    Private Sub GeneraControlli()

        Dim sAspx As String
        Dim cCreato As Control

        sAspx = Transformer.Transform(eventXmlDoc, fpd.baseTemplates & "InserimentoPresenze.xslt", "dummy", "dummy")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)

        cCreato = ParseControl(sAspx)

        phdContent.Controls.Clear()
        phdContent.Controls.Add(cCreato)

        'aggancio controlli
        lnkSave = CType(cCreato.FindControl("lnkSave"), LinkButton)
        ddnData = CType(cCreato.FindControl("ddnData"), DropDownList)
        cblDRMT = CType(cCreato.FindControl("cblDRMT"), CheckBoxList)
        cblP = CType(cCreato.FindControl("cblP"), CheckBoxList)
        errData = CType(cCreato.FindControl("errData"), Label)
        errOrari = CType(cCreato.FindControl("errOrari"), Label)
        errDRMT = CType(cCreato.FindControl("errDRMT"), Label)
        errP = CType(cCreato.FindControl("errP"), Label)
        errGlobal = CType(cCreato.FindControl("errGlobal"), Label)

        txtDa.Clear()
        txtA.Clear()
        For i = 1 To nOrari
            txtDa.Add(i, CType(cCreato.FindControl("txtDalle_" & i.ToString), TextBox))
            txtA.Add(i, CType(cCreato.FindControl("txtAlle_" & i.ToString), TextBox))
        Next

    End Sub
#End Region

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click

        Dim dataPartecipazione As Date
        Dim rangeOrari As New TimeRangeList
        Dim dbCmd As SqlCommand
        Dim presenti As Softailor.Global.StructuredUtils.GenericIntList
        Dim li As ListItem

        If Not ValidateMeAndReadDateAndTimes(dataPartecipazione, rangeOrari) Then Exit Sub

        'OK, procediamo al salvataggio

        'lettura delle persone
        presenti = New Softailor.Global.StructuredUtils.GenericIntList
        For Each li In cblDRMT.Items
            If li.Selected Then
                presenti.Add(CInt(li.Value))
            End If
        Next
        For Each li In cblP.Items
            If li.Selected Then
                presenti.Add(CInt(li.Value))
            End If
        Next

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ip_InsertDati"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_EVENTO
            .Parameters.Add("@dt_data", SqlDbType.Date).Value = dataPartecipazione
            .Parameters.Add("@tm_orari", SqlDbType.Structured).Value = rangeOrari.GetTable()
            .Parameters.Add("@id_iscritti", SqlDbType.Structured).Value = presenti.GetTable()
        End With

        dbCmd.ExecuteNonQuery()



        pnlDati.Visible = False
        pnlDone.Visible = True

        Transformer.Transform(eventXmlDoc, fpd.baseTemplates & "InserimentoPresenze_Done.xslt", phdDone)

    End Sub

    Private Class TimeRangeList
        Inherits List(Of TimeRange)

        Public Function SomeOverlap() As Boolean

            Dim overlap = False

            For i = 0 To Me.Count - 2
                For j = i + 1 To Me.Count - 1
                    If Me(i).Overlaps(Me(j)) Then
                        overlap = True
                        Exit For
                    End If
                Next
            Next

            Return overlap


        End Function

        Public Function GetTable() As DataTable
            Dim DT As New DataTable
            DT.Columns.Add("Int1", Type.GetType("System.Int32"))
            DT.Columns.Add("Int2", Type.GetType("System.Int32"))
            DT.Columns.Add("Int3", Type.GetType("System.Int32"))
            DT.Columns.Add("Int4", Type.GetType("System.Int32"))
            Dim row As DataRow
            For Each ro In Me
                row = DT.NewRow
                row("Int1") = ro.startTime.Hour
                row("Int2") = ro.startTime.Minute
                row("Int3") = ro.endTime.Hour
                row("Int4") = ro.endTime.Minute
                DT.Rows.Add(row)
            Next
            Return DT
        End Function

    End Class

    Private Class TimeRange
        Public startTime As Date
        Public endTime As Date

        Public Function Overlaps(tr As TimeRange) As Boolean
            Return Not (Me.endTime < tr.startTime Or Me.startTime > tr.endTime)
        End Function
    End Class

    Private Function ValidateMeAndReadDateAndTimes(ByRef dataPartecipazione As Date, ByRef rangeOrari As TimeRangeList) As Boolean

        Dim allValid = True
        Dim someSel = False
        Dim validStart As Boolean
        Dim validEnd As Boolean
        Dim timeStart As Date
        Dim timeEnd As Date
        Dim li As ListItem

        'data
        If ddnData.SelectedValue = "" Then
            errData.Text = "Seleziona una data"
            allValid = False
        Else
            'leggo la data
            dataPartecipazione = Date.ParseExact(ddnData.SelectedValue, "yyyyMMdd", Softailor.Global.Cultures.CulturaEnglish)

            'ci sono già dati?
            Dim dateGiaInserite As New List(Of Date)
            For Each dataNode As XmlNode In eventXmlDoc.SelectNodes("/evento/data")
                dateGiaInserite.Add(Softailor.Global.XmlParser.ParseXmlDateOnly(dataNode, "dt_data"))
            Next

            If dateGiaInserite.Contains(dataPartecipazione) Then
                errData.Text = "Per la data selezionata risultano già inseriti orari e presenze."
                allValid = False

            End If
        End If

        'almeno un relatore
        someSel = False
        For Each li In cblDRMT.Items
            If li.Selected Then
                someSel = True
                Exit For
            End If
        Next
        If Not someSel Then
            errDRMT.Text = "Seleziona almeno un nominativo"
            allValid = False
        End If

        'almeno un partecipante
        someSel = False
        For Each li In cblP.Items
            If li.Selected Then
                someSel = True
                Exit For
            End If
        Next
        If Not someSel Then
            errP.Text = "Seleziona almeno un nominativo"
            allValid = False
        End If

        'validazione dei range orari
        Dim someRangePresent = False
        Dim allRangeValid = True
        For i = 1 To nOrari
            If txtDa(i).Text <> String.Empty Or txtA(i).Text <> String.Empty Then
                someRangePresent = True
                If txtDa(i).Text = String.Empty Or txtA(i).Text = String.Empty Then
                    'manca uno dei due
                    allRangeValid = False
                Else
                    'ok ci sono entrambi
                    validStart = True
                    Try
                        timeStart = Date.ParseExact(txtDa(i).Text, "HH:mm", Softailor.Global.Cultures.CulturaItalian)
                    Catch ex As Exception
                        validStart = False
                    End Try
                    validEnd = True
                    Try
                        timeEnd = Date.ParseExact(txtA(i).Text, "HH:mm", Softailor.Global.Cultures.CulturaItalian)
                    Catch ex As Exception
                        validEnd = False
                    End Try

                    If (Not validStart) Or (Not validEnd) Then
                        allRangeValid = False
                    Else
                        'OK sono validi formalmente
                        If timeStart >= timeEnd Then
                            allRangeValid = False
                        Else
                            'OK uno è minore dell'altro > aggiungo alla lista!
                            rangeOrari.Add(New TimeRange With {
                                           .startTime = timeStart,
                                           .endTime = timeEnd
                            })
                        End If
                    End If
                End If
            End If
        Next

        If Not someRangePresent Then
            errOrari.Text = "Inserisci almeno un orario di inizio ed un orario di fine."
            allValid = False
        Else
            If Not allRangeValid Then
                errOrari.Text = "Orari non validi. Ogni riga deve contenere l'orario di inizio e l'orario di fine oppure essere vuota; gli orari devono essere scritti nel formato corretto (ore:minuti); non devono esserci sovrapposizioni tra gli intervalli orari."
                allValid = False
            Else
                'i range sono tutti validi
                If rangeOrari.SomeOverlap Then
                    errOrari.Text = "Almeno due intervalli inizio-fine sono sovrapposti o contigui."
                    allValid = False
                End If
            End If
        End If


        'TODO sovrapposizione per data

        If Not allValid Then
            errGlobal.Text = "Dati mancanti o non validi. Controlla i messaggi di errore in rosso."
        End If

        Return allValid

    End Function
End Class