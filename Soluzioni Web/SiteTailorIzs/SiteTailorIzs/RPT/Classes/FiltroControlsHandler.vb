Imports Softailor.ReportEngine
Imports System.Web.HttpUtility

Public Class FiltroControlsHandler

#Region "Classi di servizio"

    Private Class ValueControlSet
        Public txt1 As TextBox
        Public txt2 As TextBox
        Public ddn As DropDownList
        Public lnkDel As ImageButton
    End Class

#End Region

#Region "Costanti"

    Private bgOk As Drawing.Color = Drawing.Color.Empty
    Private bgKo As Drawing.Color = Drawing.Color.Yellow

#End Region

#Region "Variabili base"
    Private fonte As Fonte
    Public filtro As Filtro
    Private phd As PlaceHolder
    Private rptDbConn As SqlConnection
#End Region

#Region "Liste controlli creati"
    Private updPanels As New Dictionary(Of String, UpdatePanel)
    Private ddnCompars As New Dictionary(Of String, DropDownList)
    Private hidFldCounts As New Dictionary(Of String, HiddenField)
    Private hidCtlTypes As New Dictionary(Of String, HiddenField)
    Private phdValues As New Dictionary(Of String, PlaceHolder)
    Private cblValues As New Dictionary(Of String, CheckBoxList)
    Private valueCtls As New Dictionary(Of String, Dictionary(Of Integer, ValueControlSet))
    Private lblErrs As New Dictionary(Of String, Label)
#End Region

#Region "Costruttori"
    Public Sub New(fonte As Fonte, filtro As Filtro, phd As PlaceHolder, rptDbConn As SqlConnection)
        Me.fonte = fonte
        Me.filtro = filtro
        Me.phd = phd
        Me.rptDbConn = rptDbConn
    End Sub
#End Region

#Region "Creazione Controlli Principali"

    Public Sub CreateMainControls()

        phd.Controls.Clear()

        For Each campo As Campo In fonte.CampiCorpo
            If campo.Filtro Then
                CreateMainControlsCampo(campo)
            End If
        Next

    End Sub

    Private Sub CreateMainControlsCampo(campo As Campo)

        'update panel
        Dim upd As New UpdatePanel
        With upd
            .UpdateMode = UpdatePanelUpdateMode.Conditional
            .ID = "upd_" & campo.NomeDb
            .EnableViewState = True
        End With
        updPanels.Add(campo.NomeDb, upd)
        phd.Controls.Add(upd)

        With upd.ContentTemplateContainer
            'contenitore
            .Controls.Add(New LiteralControl("<div class=""campo"">"))

            'label
            .Controls.Add(New LiteralControl("<div class=""lbl"">" & HtmlEncode(campo.Descrizione) & "</div>"))

            'apertura dato
            .Controls.Add(New LiteralControl("<div class=""dat"">"))

            'checkboxlist oppure campi?
            If campo.TipoControllo = TipoControllo.CheckBoxList Then
                'apertura contenitore checkboxlist
                .Controls.Add(New LiteralControl("<div class=""cblcont"">"))

                'tutti
                .Controls.Add(New LiteralControl("<span class=""allnone"" " &
                                                 "onclick=""cblFiltroAll('cblValue_" & campo.NomeDb & "');"">Tutti</span>"))

                .Controls.Add(New LiteralControl("<span class=""allnone"" " &
                                                 "onclick=""cblFiltroNone('cblValue_" & campo.NomeDb & "');"">Nessuno</span>"))

                'checkboxlist (già popolata)
                Dim cblValue = GeneraCblValue(campo)
                cblValues.Add(campo.NomeDb, cblValue)
                .Controls.Add(cblValue)

                'chiusura contenitore checkboxlist
                .Controls.Add(New LiteralControl("</div>"))
            Else
                'apertura comparazione
                .Controls.Add(New LiteralControl("<div class=""compar"">"))

                'drop down comparazione
                Dim ddnCompar = GeneraDdnCompar(campo)
                ddnCompars.Add(campo.NomeDb, ddnCompar)
                .Controls.Add(ddnCompar)
                'evento change
                AddHandler ddnCompar.SelectedIndexChanged, AddressOf ddnCompar_SelectedIndexChanged

                'hidden field numero di valori
                Dim hidFldCount = GeneraFldCount(campo)
                hidFldCounts.Add(campo.NomeDb, hidFldCount)
                .Controls.Add(hidFldCount)

                'hidden field tipo di campo
                Dim hidCtlType = GeneraCtlType(campo)
                hidCtlTypes.Add(campo.NomeDb, hidCtlType)
                .Controls.Add(hidCtlType)

                'chiusura comparazione
                .Controls.Add(New LiteralControl("</div>"))

                'apertura valori
                .Controls.Add(New LiteralControl("<div class=""val"">"))

                'placeholder valori
                Dim phdValue As New PlaceHolder
                With phdValue
                    .ID = "phd_" & campo.NomeDb
                    .EnableViewState = False
                End With
                phdValues.Add(campo.NomeDb, phdValue)
                .Controls.Add(phdValue)

                'chiusura valori
                .Controls.Add(New LiteralControl("</div>"))

                'clear
                .Controls.Add(New LiteralControl("<div class=""clear""></div>"))
            End If

            'chiusura dato
            .Controls.Add(New LiteralControl("</div>"))

            'apertura errore
            .Controls.Add(New LiteralControl("<div class=""err"">"))

            'label errore
            Dim lblErr = GeneraLblErr(campo)
            lblErrs.Add(campo.NomeDb, lblErr)
            .Controls.Add(lblErr)

            'chiusura errore
            .Controls.Add(New LiteralControl("</div>"))
            'clear
            .Controls.Add(New LiteralControl("<div class=""clear""></div>"))

            'chiusura contenitore
            .Controls.Add(New LiteralControl("</div>"))
        End With

    End Sub

    Private Function GeneraFldCount(campo As Campo) As HiddenField

        Dim hid As New HiddenField
        With hid
            .ID = "hidFldCount_" & campo.NomeDb.ToString
            .EnableViewState = False
            .Value = "0"
        End With
        Return hid

    End Function

    Private Function GeneraCtlType(campo As Campo) As HiddenField

        Dim hid As New HiddenField
        With hid
            .ID = "hidCtlType_" & campo.NomeDb.ToString
            .EnableViewState = False
            .Value = ""
        End With
        Return hid
    End Function

    Private Function GeneraDdnCompar(campo As Campo) As DropDownList

        Dim ddn As New DropDownList
        With ddn
            .ID = "ddnCompar_" & campo.NomeDb.ToString
            .CssClass = "ddn ddncompar"
            .EnableViewState = True
            .AutoPostBack = True
        End With

        ddn.Items.Add(New ListItem("", ""))

        'criteri

        If campo.TipoControllo = TipoControllo.DropDown Then
            'campi di tipo drop down: solo uguale, diverso, vuoto, non vuoto
            ddn.Items.Add(New ListItem("Uguale a", "Uguale"))
            ddn.Items.Add(New ListItem("Diverso da", "Diverso"))
            ddn.Items.Add(New ListItem("Vuoto", "Vuoto"))
            ddn.Items.Add(New ListItem("Non vuoto", "NonVuoto"))
        Else
            'campi di tipo standard 

            'uguale / diverso: per tutti
            ddn.Items.Add(New ListItem("Uguale a", "Uguale"))

            'solo per stringhe
            If campo.Tipo = TipoDato.Stringa Then
                ddn.Items.Add(New ListItem("Inizia per", "IniziaPer"))
                ddn.Items.Add(New ListItem("Finisce per", "FiniscePer"))
                ddn.Items.Add(New ListItem("Contiene", "Contiene"))
            End If

            'diverso: per tutti
            ddn.Items.Add(New ListItem("Diverso da", "Diverso"))

            'ordinati: solo per alcuni
            If campo.Tipo = TipoDato.Data Or campo.Tipo = TipoDato.DataOra Or campo.Tipo = TipoDato.Decimale Or campo.Tipo = TipoDato.Intero Or campo.Tipo = TipoDato.Ora Then
                ddn.Items.Add(New ListItem("Maggiore di", "Maggiore"))
                ddn.Items.Add(New ListItem("Minore di", "Minore"))
                ddn.Items.Add(New ListItem("Maggiore o uguale a", "MaggioreUguale"))
                ddn.Items.Add(New ListItem("Minore o uguale a", "MinoreUguale"))
                ddn.Items.Add(New ListItem("Compreso tra...e", "Compreso"))
            End If

            'vuoto/non vuoto: per tutti
            ddn.Items.Add(New ListItem("Vuoto", "Vuoto"))
            ddn.Items.Add(New ListItem("Non vuoto", "NonVuoto"))
        End If

        Return ddn
    End Function

    Private Function GeneraLblErr(campo As Campo) As Label

        Dim lbl As New Label
        With lbl
            .ID = "lblErr_" & campo.NomeDb.ToString
            .EnableViewState = False
            .CssClass = "lblerr"
            .Text = ""
        End With
        Return lbl

    End Function

#End Region

#Region "Creazione Controlli Valori (checkboxlist)"

    Private Function GeneraCblValue(campo As Campo) As CheckBoxList

        Dim cbl As CheckBoxList

        cbl = New CheckBoxList
        With cbl
            .ID = "cblValue_" & campo.NomeDb
            .EnableViewState = False
            .RepeatLayout = RepeatLayout.Flow
            .CssClass = "cbl"
            .ClientIDMode = ClientIDMode.Static
        End With

        'valori
        With GetListValues(campo)
            For Each key In .Keys
                cbl.Items.Add(New ListItem(.Item(key), key))
            Next
        End With

        Return cbl

    End Function

#End Region

#Region "Creazione Controlli Valori (standard)"

    Public Sub CreateValueControls()

        Dim nomeDb As String
        Dim oldFldCount As Integer
        Dim newFldCount As Integer
        Dim oldComparazione As String
        Dim newComparazione As String
        Dim oldFldNo As NumeroCampiVisualizzati
        Dim newFldNo As NumeroCampiVisualizzati

        For Each campo As Campo In fonte.CampiCorpo
            If campo.Filtro And campo.TipoControllo <> TipoControllo.CheckBoxList Then
                'preparo i valori
                nomeDb = campo.NomeDb
                oldFldCount = CInt(hidFldCounts(nomeDb).Value)
                oldComparazione = hidCtlTypes(nomeDb).Value
                oldFldNo = NumeroCampiVisualizzato(oldComparazione)
                newFldCount = oldFldCount
                newComparazione = oldComparazione
                newFldNo = oldFldNo
                'gestisco la creazione dei campi valore (senza copia!)
                CreateValueControlsCampo(nomeDb,
                                         oldComparazione, oldFldCount, oldFldNo,
                                         newComparazione, newFldCount, newFldNo,
                                         False)

            End If
        Next

    End Sub

    Private Sub CreateValueControlsCampo(
                                        nomeDb As String,
                                        oldComparazione As String,
                                        oldFldCount As Integer,
                                        oldFldNo As NumeroCampiVisualizzati,
                                        newComparazione As String,
                                        newFldCount As Integer,
                                        newFldNo As NumeroCampiVisualizzati,
                                        copyValues As Boolean)

        Dim campo As Campo = Nothing
        Dim phdValue As PlaceHolder
        Dim valueCtlsDict As Dictionary(Of Integer, ValueControlSet)

        Dim oldValues1 As New Dictionary(Of Integer, String)
        Dim oldValues2 As New Dictionary(Of Integer, String)

        'aggancio il campo
        For Each campoFind In fonte.CampiCorpo
            If campoFind.NomeDb = nomeDb Then
                campo = campoFind
                Exit For
            End If
        Next

        'aggancio il placeHolder
        phdValue = phdValues(campo.NomeDb)

        'pulizia
        phdValue.Controls.Clear()

        'se serve, leggo i vecchi valori
        If copyValues Then
            If valueCtls.ContainsKey(nomeDb) Then
                For idx = 0 To valueCtls(nomeDb).Count - 1
                    With valueCtls(nomeDb)(idx)
                        If .txt1 IsNot Nothing Then
                            If .txt1.Text.TrimEnd <> String.Empty Then
                                oldValues1.Add(idx, .txt1.Text.TrimEnd)
                            End If
                        End If
                        If .txt2 IsNot Nothing Then
                            If .txt2.Text.TrimEnd <> String.Empty Then
                                oldValues2.Add(idx, .txt2.Text.TrimEnd)
                            End If
                        End If
                        If .ddn IsNot Nothing Then
                            If .ddn.SelectedValue <> String.Empty Then
                                oldValues1.Add(idx, .ddn.SelectedValue)
                            End If
                        End If
                    End With
                Next
            End If
        End If

        'genero i controlli e ottengo la lista dei controlset
        valueCtlsDict = CreateValueControlsCampoInt(campo, phdValue, newComparazione, newFldCount, newFldNo)

        'accodo o sostituisco
        If valueCtls.ContainsKey(nomeDb) Then
            valueCtls(nomeDb) = valueCtlsDict
        Else
            valueCtls.Add(nomeDb, valueCtlsDict)
        End If

        'se serve, riscrivo i valori
        If copyValues Then
            For idx = 0 To valueCtlsDict.Count - 1
                With valueCtlsDict(idx)
                    If .txt1 IsNot Nothing Then
                        If oldValues1.ContainsKey(idx) Then
                            .txt1.Text = oldValues1(idx)
                        End If
                    End If
                    If .txt2 IsNot Nothing Then
                        If oldValues2.ContainsKey(idx) Then
                            .txt2.Text = oldValues2(idx)
                        End If
                    End If
                    If .ddn IsNot Nothing Then
                        If oldValues1.ContainsKey(idx) Then
                            .ddn.SelectedValue = oldValues1(idx)
                        End If
                    End If
                End With
            Next
        End If

        'bottone creazione
        If newFldNo = NumeroCampiVisualizzati.Multiplo Then
            Dim lnk As New LinkButton
            With lnk
                .ID = "lnkAdd_" & campo.NomeDb
                .CssClass = "btnlink"
                Select Case newComparazione
                    Case "Diverso"
                        .Text = "e..."
                    Case Else
                        .Text = "o..."
                End Select
                .EnableViewState = False
            End With
            With phdValues(campo.NomeDb)
                .Controls.Add(New LiteralControl("<div>"))
                .Controls.Add(New LiteralControl("<span class=""congiunz"">&nbsp;</span>"))
                .Controls.Add(lnk)
                .Controls.Add(New LiteralControl("</div>"))
            End With
            AddHandler lnk.Click, AddressOf lnkAdd_Click
        End If

        'scrivo i nuovi valori
        hidFldCounts(nomeDb).Value = newFldCount.ToString
        hidCtlTypes(nomeDb).Value = newComparazione

    End Sub

    Private Function CreateValueControlsCampoInt(campo As Campo, phdValue As PlaceHolder, newComparazione As String,
                                        newFldCount As Integer,
                                        newFldNo As NumeroCampiVisualizzati) As Dictionary(Of Integer, ValueControlSet)

        Dim result As New Dictionary(Of Integer, ValueControlSet)
        Dim listValues As Dictionary(Of String, String) = Nothing

        'leggo la lista dei valori da DB se devo creare una o più dropdown e se devo creare almeno un controllo
        If campo.TipoControllo = TipoControllo.DropDown And newFldCount > 0 Then
            listValues = GetListValues(campo)
        End If

        'istanzio l'output

        For i = 0 To newFldCount - 1

            Dim vcs As New ValueControlSet

            'apertura div
            phdValue.Controls.Add(New LiteralControl("<div>"))

            'e/o
            If i > 0 Then
                If newComparazione = "Diverso" Then
                    phdValue.Controls.Add(New LiteralControl("<span class=""congiunz"">e</span>"))
                Else
                    phdValue.Controls.Add(New LiteralControl("<span class=""congiunz"">o</span>"))
                End If
            Else
                phdValue.Controls.Add(New LiteralControl("<span class=""congiunz"">&nbsp;</span>"))
            End If

            Select Case campo.TipoControllo
                Case TipoControllo.Standard
                    'primo textbox: sempre
                    Dim txt1 As New TextBox
                    txt1.ID = "txtVal_" & campo.NomeDb & "_" & i.ToString & "_1"
                    txt1.EnableViewState = False
                    'eventuali mask e larghezze
                    Select Case campo.Tipo
                        Case TipoDato.Data
                            txt1.CssClass = "txt valdata stl_dt_data_ddmmyyyy"
                        Case TipoDato.DataOra
                            txt1.CssClass = "txt valdataora stl_dt_dataora_ddmmyyyyhhmmss"
                        Case TipoDato.Ora
                            txt1.CssClass = "txt valora stl_dt_ora_hhmmss"
                        Case TipoDato.Decimale
                            txt1.CssClass = "txt valnumero"
                        Case TipoDato.Intero
                            txt1.CssClass = "txt valnumero"
                        Case TipoDato.Stringa
                            txt1.CssClass = "txt valstringa"
                    End Select
                    phdValue.Controls.Add(txt1)
                    vcs.txt1 = txt1
                    'secondo textbox: solo se compreso
                    If newComparazione = "Compreso" Then
                        Dim txt2 As New TextBox
                        txt2.ID = "txtVal_" & campo.NomeDb & "_" & i.ToString & "_2"
                        txt2.EnableViewState = False
                        'eventuali mask e larghezze
                        Select Case campo.Tipo
                            Case TipoDato.Data
                                txt2.CssClass = "txt valdata stl_dt_data_ddmmyyyy"
                            Case TipoDato.DataOra
                                txt2.CssClass = "txt valdataora stl_dt_dataora_ddmmyyyyhhmmss"
                            Case TipoDato.Ora
                                txt2.CssClass = "txt valora stl_dt_ora_hhmmss"
                            Case TipoDato.Decimale
                                txt2.CssClass = "txt valnumero"
                            Case TipoDato.Intero
                                txt2.CssClass = "txt valnumero"
                            Case TipoDato.Stringa
                                txt2.CssClass = "txt valstringa"
                        End Select
                        phdValue.Controls.Add(New LiteralControl(" - "))
                        phdValue.Controls.Add(txt2)
                        vcs.txt2 = txt2
                    End If
                Case TipoControllo.DropDown
                    Dim ddn = GeneraDdnValue(campo.NomeDb, i, listValues)
                    phdValue.Controls.Add(ddn)
                    'aggancio
                    vcs.ddn = ddn
            End Select

            'link eliminazione
            If i > 0 Then
                Dim lnk As New ImageButton
                lnk.ID = "lnkDco_" & campo.NomeDb & "_" & i.ToString
                lnk.EnableViewState = False
                lnk.ImageUrl = "~/Img/IcoDelete.gif"
                phdValue.Controls.Add(lnk)

                'aggancio
                vcs.lnkDel = lnk
                AddHandler lnk.Click, AddressOf lnkDelete_Click

            End If

            'chiusura div
            phdValue.Controls.Add(New LiteralControl("</div>"))

            'aggiungo ai risultati
            result.Add(i, vcs)

        Next

        Return result

    End Function

    Private Function GeneraDdnValue(nomeDb As String, index As Integer, values As Dictionary(Of String, String)) As DropDownList

        Dim ddn As DropDownList

        ddn = New DropDownList
        With ddn
            .ID = "ddnValue_" & nomeDb & "_" & index.ToString
            .EnableViewState = False
            .CssClass = "ddn valddn"
        End With

        'valore vuoto
        ddn.Items.Add(New ListItem("", ""))

        'valori
        With values
            For Each key In .Keys
                ddn.Items.Add(New ListItem(.Item(key), key))
            Next
        End With

        Return ddn

    End Function

#End Region

#Region "Logica campi valore"

    Private Enum NumeroCampiVisualizzati
        Nessuno
        Singolo
        Multiplo
    End Enum

    Private Function NumeroCampiVisualizzato(comparazione As String) As NumeroCampiVisualizzati

        Select Case comparazione
            Case "", "Vuoto", "NonVuoto"
                Return NumeroCampiVisualizzati.Nessuno
            Case "Uguale", "Diverso", "Compreso", "IniziaPer", "FiniscePer", "Contiene"
                Return NumeroCampiVisualizzati.Multiplo
            Case "Maggiore", "Minore", "MaggioreUguale", "MinoreUguale"
                Return NumeroCampiVisualizzati.Singolo
            Case Else
                Throw New NotImplementedException
        End Select

    End Function

#End Region

#Region "utilità"

    Private Function GetListValues(campo As Campo) As Dictionary(Of String, String)

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim dict As New Dictionary(Of String, String)

        'valori
        dbCmd = rptDbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = campo.QueryLista
        End With
        dbRdr = dbCmd.ExecuteReader
        'riempimento
        Select Case campo.Tipo
            Case TipoDato.Stringa
                Do While dbRdr.Read
                    dict.Add(dbRdr.GetString(0), dbRdr.GetString(1))
                Loop
            Case TipoDato.Intero
                Do While dbRdr.Read
                    dict.Add(dbRdr.GetInt32(0).ToString, dbRdr.GetString(1))
                Loop
            Case Else
                Throw New NotImplementedException
        End Select
        dbRdr.Close()
        dbCmd.Dispose()

        Return dict

    End Function

#End Region

#Region "Reazione a eventi"

    Private Sub ddnCompar_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim ddn = CType(sender, DropDownList)
        Dim nomeDb = Mid(ddn.ID, 11)
        Dim oldFldCount As Integer
        Dim newFldCount As Integer
        Dim oldComparazione As String
        Dim newComparazione As String
        Dim oldFldNo As NumeroCampiVisualizzati
        Dim newFldNo As NumeroCampiVisualizzati

        'vecchi valori: li leggo
        oldFldCount = CInt(hidFldCounts(nomeDb).Value)
        oldComparazione = hidCtlTypes(nomeDb).Value
        oldFldNo = NumeroCampiVisualizzato(oldComparazione)

        'nuovi valori: logica
        newComparazione = ddn.SelectedValue
        newFldNo = NumeroCampiVisualizzato(newComparazione)

        Select Case oldFldNo
            Case NumeroCampiVisualizzati.Nessuno
                Select Case newFldNo
                    Case NumeroCampiVisualizzati.Nessuno
                        newFldCount = 0
                    Case Else
                        newFldCount = 1
                End Select
            Case NumeroCampiVisualizzati.Singolo
                Select Case newFldNo
                    Case NumeroCampiVisualizzati.Nessuno
                        newFldCount = 0
                    Case Else
                        newFldCount = 1
                End Select
            Case NumeroCampiVisualizzati.Multiplo
                Select Case newFldNo
                    Case NumeroCampiVisualizzati.Nessuno
                        newFldCount = 0
                    Case NumeroCampiVisualizzati.Singolo
                        newFldCount = 1
                    Case NumeroCampiVisualizzati.Multiplo
                        newFldCount = oldFldCount
                End Select
        End Select

        'gestisco la ri-creazione o la creazione dei campi valore
        CreateValueControlsCampo(nomeDb,
                                 oldComparazione, oldFldCount, oldFldNo,
                                 newComparazione, newFldCount, newFldNo,
                                 True)

    End Sub

    Private Sub lnkAdd_Click(sender As Object, e As EventArgs)

        Dim lnk = CType(sender, LinkButton)
        Dim nomeDb = Mid(lnk.ID, 8)

        'preparo i valori
        Dim oldFldCount As Integer
        Dim newFldCount As Integer
        Dim oldComparazione As String
        Dim newComparazione As String
        Dim oldFldNo As NumeroCampiVisualizzati
        Dim newFldNo As NumeroCampiVisualizzati

        oldFldCount = CInt(hidFldCounts(nomeDb).Value)
        oldComparazione = hidCtlTypes(nomeDb).Value
        oldFldNo = NumeroCampiVisualizzato(oldComparazione)

        newFldCount = oldFldCount + 1
        newComparazione = oldComparazione
        newFldNo = oldFldNo

        'gestisco la ri-creazione o la creazione dei campi valore
        CreateValueControlsCampo(nomeDb,
                                 oldComparazione, oldFldCount, oldFldNo,
                                 newComparazione, newFldCount, newFldNo, True)

    End Sub

    Private Sub lnkDelete_Click(sender As Object, e As EventArgs)

        Dim lnk = CType(sender, ImageButton)
        Dim nomeDb As String
        Dim index As Integer

        Debug.Print(lnk.ID)
        Dim arr = lnk.ID.Split("_"c)
        index = CInt(arr(arr.Length - 1))
        nomeDb = Mid(lnk.ID, 8, Len(lnk.ID) - 8 - index.ToString.Length)

        With valueCtls(nomeDb)
            For i = index To .Count - 2
                If .Item(i).txt1 IsNot Nothing Then
                    .Item(i).txt1.Text = .Item(i + 1).txt1.Text
                End If
                If .Item(i).txt2 IsNot Nothing Then
                    .Item(i).txt2.Text = .Item(i + 1).txt2.Text
                End If
                If .Item(i).ddn IsNot Nothing Then
                    .Item(i).ddn.SelectedValue = .Item(i + 1).ddn.SelectedValue
                End If
            Next
        End With

        'preparo i valori
        Dim oldFldCount As Integer
        Dim newFldCount As Integer
        Dim oldComparazione As String
        Dim newComparazione As String
        Dim oldFldNo As NumeroCampiVisualizzati
        Dim newFldNo As NumeroCampiVisualizzati

        oldFldCount = CInt(hidFldCounts(nomeDb).Value)
        oldComparazione = hidCtlTypes(nomeDb).Value
        oldFldNo = NumeroCampiVisualizzato(oldComparazione)

        newFldCount = oldFldCount - 1
        newComparazione = oldComparazione
        newFldNo = oldFldNo

        'gestisco la ri-creazione o la creazione dei campi valore
        CreateValueControlsCampo(nomeDb,
                                 oldComparazione, oldFldCount, oldFldNo,
                                 newComparazione, newFldCount, newFldNo, True)

    End Sub

#End Region

#Region "Validazione"
    Public Function ValidateMe() As Boolean

        Dim allValid = True

        'ciclo su tutti i campi filtrabili
        For Each campo In fonte.CampiCorpo

            If campo.Filtro Then
                Select Case campo.TipoControllo
                    Case TipoControllo.CheckBoxList
                        'non faccio nulla: non può essere non valido
                    Case TipoControllo.Standard
                        allValid = allValid And ValidateStandardField(campo)
                        'update del contenitore
                        updPanels(campo.NomeDb).Update()
                    Case TipoControllo.DropDown
                        allValid = allValid And ValidateDropDownField(campo)
                        'update del contenitore
                        updPanels(campo.NomeDb).Update()
                End Select

            End If

        Next

        Return allValid

    End Function

    Private Function ValidateStandardField(campo As Campo) As Boolean

        Dim valid = True

        Dim culture = Softailor.Global.Cultures.CulturaItalian
        Dim nValori = CInt(hidFldCounts(campo.NomeDb).Value)

        If nValori > 0 Then
            For Each cst In valueCtls(campo.NomeDb).Values
                Select Case campo.Tipo
                    Case TipoDato.Data
                        'testo 1
                        If cst.txt1.Text = "" Then
                            cst.txt1.BackColor = bgKo
                            valid = False
                        Else
                            Dim date1 As Date
                            If Not Date.TryParseExact(cst.txt1.Text, "dd/MM/yyyy", culture, Globalization.DateTimeStyles.None, date1) Then
                                cst.txt1.BackColor = bgKo
                                valid = False
                            End If
                        End If
                        'testo 2 se c'è
                        If cst.txt2 IsNot Nothing Then
                            If cst.txt2.Text = "" Then
                                cst.txt2.BackColor = bgKo
                                valid = False
                            Else
                                Dim date2 As Date
                                If Not Date.TryParseExact(cst.txt2.Text, "dd/MM/yyyy", culture, Globalization.DateTimeStyles.None, date2) Then
                                    cst.txt2.BackColor = bgKo
                                    valid = False
                                End If
                            End If
                        End If
                    Case TipoDato.DataOra
                        'testo 1
                        If cst.txt1.Text = "" Then
                            cst.txt1.BackColor = bgKo
                            valid = False
                        Else
                            Dim date1 As Date
                            If Not Date.TryParseExact(cst.txt1.Text, "dd/MM/yyyy HH:mm:ss", culture, Globalization.DateTimeStyles.None, date1) Then
                                cst.txt1.BackColor = bgKo
                                valid = False
                            End If
                        End If
                        'testo 2 se c'è
                        If cst.txt2 IsNot Nothing Then
                            If cst.txt2.Text = "" Then
                                cst.txt2.BackColor = bgKo
                                valid = False
                            Else
                                Dim date2 As Date
                                If Not Date.TryParseExact(cst.txt2.Text, "dd/MM/yyyy HH:mm:ss", culture, Globalization.DateTimeStyles.None, date2) Then
                                    cst.txt2.BackColor = bgKo
                                    valid = False
                                End If
                            End If
                        End If
                    Case TipoDato.Decimale
                        'testo 1
                        cst.txt1.Text = cst.txt1.Text.Trim
                        If cst.txt1.Text = "" Then
                            cst.txt1.BackColor = bgKo
                            valid = False
                        Else
                            Dim dec1 As Decimal
                            If Not Decimal.TryParse(cst.txt1.Text, Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowLeadingSign, culture, dec1) Then
                                cst.txt1.BackColor = bgKo
                                valid = False
                            End If
                        End If
                        'testo 2 se c'è
                        If cst.txt2 IsNot Nothing Then
                            cst.txt2.Text = cst.txt2.Text.Trim
                            If cst.txt2.Text = "" Then
                                cst.txt2.BackColor = bgKo
                                valid = False
                            Else
                                Dim dec2 As Decimal
                                If Not Decimal.TryParse(cst.txt2.Text, Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowLeadingSign, culture, dec2) Then
                                    cst.txt2.BackColor = bgKo
                                    valid = False
                                End If
                            End If
                        End If
                    Case TipoDato.Intero
                        'testo 1
                        cst.txt1.Text = cst.txt1.Text.Trim
                        If cst.txt1.Text = "" Then
                            cst.txt1.BackColor = bgKo
                            valid = False
                        Else
                            Dim int1 As Integer
                            If Not Integer.TryParse(cst.txt1.Text, int1) Then
                                cst.txt1.BackColor = bgKo
                                valid = False
                            End If
                        End If
                        'testo 2 se c'è
                        If cst.txt2 IsNot Nothing Then
                            cst.txt2.Text = cst.txt2.Text.Trim
                            If cst.txt2.Text = "" Then
                                cst.txt2.BackColor = bgKo
                                valid = False
                            Else
                                Dim int2 As Integer
                                If Not Integer.TryParse(cst.txt2.Text, int2) Then
                                    cst.txt2.BackColor = bgKo
                                    valid = False
                                End If
                            End If
                        End If
                    Case TipoDato.Ora
                        'testo 1
                        If cst.txt1.Text = "" Then
                            cst.txt1.BackColor = bgKo
                            valid = False
                        Else
                            Dim date1 As Date
                            If Not Date.TryParseExact(cst.txt1.Text, "HH:mm:ss", culture, Globalization.DateTimeStyles.None, date1) Then
                                cst.txt1.BackColor = bgKo
                                valid = False
                            End If
                        End If
                        'testo 2 se c'è
                        If cst.txt2 IsNot Nothing Then
                            If cst.txt2.Text = "" Then
                                cst.txt2.BackColor = bgKo
                                valid = False
                            Else
                                Dim date2 As Date
                                If Not Date.TryParseExact(cst.txt2.Text, "HH:mm:ss", culture, Globalization.DateTimeStyles.None, date2) Then
                                    cst.txt2.BackColor = bgKo
                                    valid = False
                                End If
                            End If
                        End If
                    Case TipoDato.Stringa
                        'testo 1
                        cst.txt1.Text = cst.txt1.Text.TrimEnd
                        If cst.txt1.Text = "" Then
                            cst.txt1.BackColor = bgKo
                            valid = False
                        End If
                        'testo 2 se c'è
                        If cst.txt2 IsNot Nothing Then
                            cst.txt2.Text = cst.txt2.Text.TrimEnd
                            If cst.txt2.Text = "" Then
                                cst.txt2.BackColor = bgKo
                                valid = False
                            End If
                        End If
                End Select
            Next
            
        End If

        If Not valid Then
            lblErrs(campo.NomeDb).Text = "Dati mancanti o scorretti nei campi evidenziati in giallo."
        End If

        Return valid

    End Function

    Private Function ValidateDropDownField(campo As Campo) As Boolean

        Dim valid = True
        Dim nValori = CInt(hidFldCounts(campo.NomeDb).Value)

        If nValori > 0 Then
            For Each cst In valueCtls(campo.NomeDb).Values
                If cst.ddn.SelectedValue = "" Then
                    valid = False
                    cst.ddn.BackColor = bgKo
                End If
            Next
        End If
        If Not valid Then
            lblErrs(campo.NomeDb).Text = "Seleziona un'opzione in ciascun campo o elimina i campi vuoti."
        End If
        Return valid

    End Function

#End Region

#Region "Lettura Dati Utente"
    Public Sub ReadMe()

        'leggo tutto e posiziono i dati nell'oggetto filtro
        filtro = New Filtro()

        'ciclo su tutti i campi filtrabili
        For Each campo In fonte.CampiCorpo

            If campo.Filtro Then
                Select Case campo.TipoControllo
                    Case TipoControllo.CheckBoxList
                        ReadFiltroCheckBoxList(campo)
                    Case TipoControllo.Standard
                        ReadFiltroStandard(campo)
                    Case TipoControllo.DropDown
                        ReadFiltroDropDown(campo)
                End Select

            End If

        Next

    End Sub

    Private Sub ReadFiltroCheckBoxList(campo As Campo)

        Dim cbl = cblValues(campo.NomeDb)
        Dim selVals As New List(Of String)

        For Each lItem As ListItem In cbl.Items
            If lItem.Selected Then
                selVals.Add(lItem.Value)
            End If
        Next

        'solo se c'è qualcosa, altrimenti è come se non ci fosse un filtro
        If selVals.Count > 0 Then
            Dim condizione As New Condizione
            With condizione
                .NomeCampoDb = campo.NomeDb
                .Comparazione = Comparazioni.Uguale
                For Each selVal In selVals
                    .Valori.Add(New Valore With {.v1 = selVal})
                Next
            End With
            filtro.Condizioni.Add(condizione)
        End If

    End Sub

    Private Sub ReadFiltroStandard(campo As Campo)

        Dim ddn = ddnCompars(campo.NomeDb)
        Dim cIta = Softailor.Global.Cultures.CulturaItalian
        Dim cSQL = Softailor.Global.SqlUtils.CulturaSQL

        If ddn.SelectedValue <> "" Then
            Dim condizione As New Condizione
            With condizione
                .NomeCampoDb = campo.NomeDb
                Select Case ddn.SelectedValue
                    Case "Uguale"
                        .Comparazione = Comparazioni.Uguale
                    Case "IniziaPer"
                        .Comparazione = Comparazioni.IniziaPer
                    Case "FiniscePer"
                        .Comparazione = Comparazioni.FiniscePer
                    Case "Contiene"
                        .Comparazione = Comparazioni.Contiene
                    Case "Diverso"
                        .Comparazione = Comparazioni.Diverso
                    Case "Maggiore"
                        .Comparazione = Comparazioni.Maggiore
                    Case "Minore"
                        .Comparazione = Comparazioni.Minore
                    Case "MaggioreUguale"
                        .Comparazione = Comparazioni.MaggioreUguale
                    Case "MinoreUguale"
                        .Comparazione = Comparazioni.MinoreUguale
                    Case "Compreso"
                        .Comparazione = Comparazioni.Compreso
                    Case "Vuoto"
                        .Comparazione = Comparazioni.Vuoto
                    Case "NonVuoto"
                        .Comparazione = Comparazioni.NonVuoto
                    Case Else
                        Throw New NotImplementedException
                End Select

                'valore o valori
                Select Case .Comparazione
                    Case Comparazioni.Vuoto, Comparazioni.NonVuoto
                        'non faccio nulla
                    Case Comparazioni.Compreso
                        'doppio valore
                        For Each cst In valueCtls(campo.NomeDb).Values
                            .Valori.Add(New Valore With {
                                        .v1 = Helpers.XmlValue(cst.txt1.Text, campo, cIta, cSQL),
                                        .v2 = Helpers.XmlValue(cst.txt2.Text, campo, cIta, cSQL)})
                        Next
                    Case Else
                        'singolo valore
                        For Each cst In valueCtls(campo.NomeDb).Values
                            .Valori.Add(New Valore With {
                                        .v1 = Helpers.XmlValue(cst.txt1.Text, campo, cIta, cSQL)})
                        Next
                End Select
            End With
            filtro.Condizioni.Add(condizione)
        End If
    End Sub

    Private Sub ReadFiltroDropDown(campo As Campo)

        Dim ddn = ddnCompars(campo.NomeDb)

        If ddn.SelectedValue <> "" Then
            Dim condizione As New Condizione
            With condizione
                .NomeCampoDb = campo.NomeDb
                Select Case ddn.SelectedValue
                    Case "Uguale"
                        .Comparazione = Comparazioni.Uguale
                    Case "Diverso"
                        .Comparazione = Comparazioni.Diverso
                    Case "Vuoto"
                        .Comparazione = Comparazioni.Vuoto
                    Case "NonVuoto"
                        .Comparazione = Comparazioni.NonVuoto
                    Case Else
                        Throw New NotImplementedException
                End Select
                'valori, solo se uguale o diverso
                If .Comparazione = Comparazioni.Uguale Or .Comparazione = Comparazioni.Diverso Then
                    For Each cst In valueCtls(campo.NomeDb).Values
                        .Valori.Add(New Valore With {.v1 = cst.ddn.SelectedValue})
                    Next
                End If
            End With
            filtro.Condizioni.Add(condizione)
        End If

    End Sub

#End Region

#Region "Scrittura dati da XML"
    Public Sub WriteData(filtroIn As Filtro)

        Dim condizione As Condizione
        Dim condizioneFind As Condizione
        Dim cIta = Softailor.Global.Cultures.CulturaItalian
        Dim cSql = Softailor.Global.SqlUtils.CulturaSQL

        filtro = filtroIn

        'ciclo su tutti i campi
        For Each campo In fonte.CampiCorpo
            If campo.Filtro Then

                'provo a vedere se esiste una condizione per il campo
                condizione = Nothing
                For Each condizioneFind In filtro.Condizioni
                    If condizioneFind.NomeCampoDb = campo.NomeDb Then
                        condizione = condizioneFind
                        Exit For
                    End If
                Next

                'ho trovato una condizione?
                If condizione IsNot Nothing Then
                    'drop down e campi nascosti
                    Select Case campo.TipoControllo
                        Case TipoControllo.Standard, TipoControllo.DropDown
                            'standard o drop down
                            'impostazione della drop down
                            With ddnCompars(campo.NomeDb)
                                Select Case condizione.Comparazione
                                    Case Comparazioni.Uguale
                                        .SelectedValue = "Uguale"
                                    Case Comparazioni.IniziaPer
                                        .SelectedValue = "IniziaPer"
                                    Case Comparazioni.FiniscePer
                                        .SelectedValue = "FiniscePer"
                                    Case Comparazioni.Contiene
                                        .SelectedValue = "Contiene"
                                    Case Comparazioni.Diverso
                                        .SelectedValue = "Diverso"
                                    Case Comparazioni.Maggiore
                                        .SelectedValue = "Maggiore"
                                    Case Comparazioni.Minore
                                        .SelectedValue = "Minore"
                                    Case Comparazioni.MaggioreUguale
                                        .SelectedValue = "MaggioreUguale"
                                    Case Comparazioni.MinoreUguale
                                        .SelectedValue = "MinoreUguale"
                                    Case Comparazioni.Compreso
                                        .SelectedValue = "Compreso"
                                    Case Comparazioni.Vuoto
                                        .SelectedValue = "Vuoto"
                                    Case Comparazioni.NonVuoto
                                        .SelectedValue = "NonVuoto"
                                    Case Else
                                        Throw New NotImplementedException
                                End Select
                            End With

                            'rigenero i controlli in base ai dati
                            Dim oldFldCount As Integer
                            Dim newFldCount As Integer
                            Dim oldComparazione As String
                            Dim newComparazione As String
                            Dim oldFldNo As NumeroCampiVisualizzati
                            Dim newFldNo As NumeroCampiVisualizzati

                            oldFldCount = 0
                            newFldCount = condizione.Valori.Count
                            oldComparazione = ""
                            newComparazione = ddnCompars(campo.NomeDb).SelectedValue
                            oldFldNo = NumeroCampiVisualizzato("")
                            newFldNo = NumeroCampiVisualizzato(newComparazione)

                            'gestisco la ri-creazione o la creazione dei campi valore
                            'senza copiare i dati
                            'la riscrittura nei controlli hidden viene fatta da CreateValueControlsCampo
                            CreateValueControlsCampo(campo.NomeDb,
                                                     oldComparazione, oldFldCount, oldFldNo,
                                                     newComparazione, newFldCount, newFldNo,
                                                     False)

                            'scrittura dei dati in caso di ddn o txt
                            Select Case campo.TipoControllo
                                Case TipoControllo.DropDown
                                    'selezione delle dropdown
                                    For i = 0 To condizione.Valori.Count - 1
                                        Dim val = condizione.Valori(i).v1
                                        Dim ddn = valueCtls(campo.NomeDb)(i).ddn
                                        For Each listItem As ListItem In ddn.Items
                                            If listItem.Value = val Then
                                                listItem.Selected = True
                                                Exit For
                                            End If
                                        Next
                                    Next
                                Case TipoControllo.Standard
                                    For i = 0 To condizione.Valori.Count - 1
                                        Dim val1 = condizione.Valori(i).v1
                                        Dim val2 = condizione.Valori(i).v2
                                        Dim cst = valueCtls(campo.NomeDb)(i)
                                        cst.txt1.Text = Helpers.XmlToText(val1, campo, cIta, cSql)
                                        If cst.txt2 IsNot Nothing Then cst.txt2.Text = Helpers.XmlToText(val2, campo, cIta, cSql)
                                    Next
                            End Select
                        Case TipoControllo.CheckBoxList
                            'selezione delle checkbox
                            For Each listItem As ListItem In cblValues(campo.NomeDb).Items
                                For Each v In condizione.Valori
                                    If v.v1 = listItem.Value Then
                                        listItem.Selected = True
                                    End If
                                Next
                            Next
                    End Select

                    'update del panel
                    updPanels(campo.NomeDb).Update()
                End If


                

            End If
        Next


    End Sub
#End Region
End Class
