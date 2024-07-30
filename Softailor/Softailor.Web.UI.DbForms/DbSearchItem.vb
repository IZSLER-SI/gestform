Imports System.Web.UI
Imports System.Data.SqlTypes
Imports Softailor.Global.SqlUtils
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient

'classe per memorizzazione elementi ricerca
Public Class DbSearchItem
    Implements IStlSearchFormItem

    'proprietà da DB
    Public ID_RITEM As Integer                  'identificatore RITEM
    Public LABELRIC As String                   'etichetta, dopo sostituzione parametri addizionali
    Public LARGHEZZ As Single                   'larghezza in..."access-centimetri :-)"
    Public ALTEZZAA As Single                   'altezza in..."access-centimetri :-)"
    Private _NOMECAMP As String                   'nome campo
    Public COMPARAZ As String                   'operatore comparazione
    Public TIPOCTRL As String                   'tipo di controllo da generare
    Public MAXLUNGH As Integer                  'lunghezza massima campo per creazione nuovo elemento (0 = non def)
    Public TIPODATO As String                   'tipo di dato
    Public TIPO_QRY As String                   'tipo di sorgente riga
    Public SQLCMBEV As String                   'origine riga SQL o valori separati da ;
    Public LARGHCOL As String                   'larghezza colonne
    Public NUM_COLO As Integer                  'numero di colonne
    Public COLONASS As Integer                  'colonna associata
    Public LABELWID As Single                   'larghezza etichetta in "access-centimetri"
    Public NUOVOFLD As Boolean
    Public NUOVOREQ As Boolean

    'per proprietà
    Private _orderIndex As Integer
    Private _searchForm As StlSearchForm

    'per memorizzazione controlli associati
    Public control As Control
    Public hiddenControl As TextBox

    'per validazione
    'variabili per gestione formati
    Private FormData_Accettati As String() = {"d/M/yy", "d/M/yyyy"}
    Private FormData_Output As String = "dd/MM/yyyy"
    Private FormOra_Accettati As String() = {"H:m:s", "H:m"}
    Private FormOra_Output As String = "HH:mm:ss"

    Public Property NOMECAMP() As String Implements IStlSearchFormItem.NOMECAMP
        Get
            Return _NOMECAMP
        End Get
        Set(ByVal value As String)
            _NOMECAMP = value
        End Set
    End Property

    Private Function EnqueueSelected(ByVal list As ListItemCollection) As String
        Dim li As ListItem
        Dim sOut As String = ""
        For Each li In list
            If li.Selected Then
                If li.Value = "" Then
                    sOut &= "000"
                Else
                    sOut &= Len(li.Value).ToString("000") & li.Value
                End If
            End If
        Next

        Return sOut
    End Function

    Private Function DequeueSelected(ByVal enqueuedList As String) As List(Of String)
        Dim outList As New List(Of String)

        Dim list As String = enqueuedList
        Dim length As Integer, value As String

        Do While Len(list) > 0
            length = CInt(Mid(list, 1, 3))
            value = Mid(list, 4, length)
            outList.Add(value)
            list = Mid(list, length + 4)
        Loop

        Return outList

    End Function

    Private Function AccessToPixels(ByVal accessCm As Single) As Unit
        Return Unit.Pixel(CInt(accessCm * CSng(38)))
    End Function

    Public Sub ClearVisible() Implements IStlSearchFormItem.ClearVisible
        If Not control Is Nothing Then
            If TypeOf control Is TextBox Then
                CType(control, TextBox).Text = ""
            ElseIf TypeOf control Is CheckBox Then
                CType(control, CheckBox).Checked = False
            ElseIf TypeOf control Is DropDownList Then
                CType(control, DropDownList).SelectedValue = ""
            ElseIf TypeOf control Is CheckBoxList Then
                Dim li As ListItem
                For Each li In CType(control, CheckBoxList).Items
                    li.Selected = False
                Next
            End If
        End If
    End Sub

    Public Sub CopyValuesToHiddenControls() Implements IStlSearchFormItem.CopyValuesToHiddenControls
        'copia nel controllo hidden
        Select Case TIPOCTRL
            Case "Testo"
                hiddenControl.Text = CType(control, TextBox).Text
            Case "CheckBox"
                If CType(control, CheckBox).Checked Then hiddenControl.Text = "1" Else hiddenControl.Text = "0"
            Case "NullBox"
                If CType(control, CheckBox).Checked Then hiddenControl.Text = "1" Else hiddenControl.Text = "0"
            Case "Riepilogo"
                hiddenControl.Text = EnqueueSelected(CType(control, CheckBoxList).Items)
            Case "ComboBox"
                hiddenControl.Text = CType(control, DropDownList).SelectedValue
        End Select
    End Sub

    Public Function GetHiddenControls() As List(Of Control) Implements IStlSearchFormItem.GetHiddenControls
        'ritora sempre e comunque una textbox
        Dim cList As New List(Of Control)
        Dim hc As New TextBox
        hc.Visible = False
        hc.ID = ControlID(True)
        hiddenControl = hc
        cList.Add(hiddenControl)
        Return cList
    End Function

    Private Function ControlID(ByVal hidden As Boolean) As String
        If hidden Then
            Return _searchForm.ID & "_" & ID_RITEM.ToString & "_HIDDEN"
        Else
            Return _searchForm.ID & "_" & ID_RITEM.ToString
        End If
    End Function

    Public Function GetNewItemSqlValues(ByVal dataSqlConnection As System.Data.SqlClient.SqlConnection) As System.Collections.Generic.Dictionary(Of String, Object) Implements IStlSearchFormItem.GetNewItemSqlValues

        Dim myDictionary As New Dictionary(Of String, Object)

        'se non si ha NUOVOFLD, il dictionary rimane vuoto
        If Me.NUOVOFLD Then
            'creo l'item
            Dim value As String
            Dim outValue As Object

            'popolo l'item a seconda dei dati nei controlli
            value = hiddenControl.Text
            Select Case TIPOCTRL
                Case "Testo"
                    Select Case TIPODATO
                        Case "Stringa"
                            If value = "" Then outValue = SqlString.Null Else outValue = New SqlString(value)
                        Case "N° Intero"
                            If value = "" Then outValue = SqlInt32.Null Else outValue = New SqlInt32(Integer.Parse(value))
                        Case "N° Decimale"
                            If value = "" Then outValue = SqlDouble.Null Else outValue = New SqlDouble(Double.Parse(value))
                        Case "Data"
                            If value = "" Then outValue = SqlDateTime.Null Else outValue = New SqlDateTime(Date.ParseExact(value, FormData_Output, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None))
                        Case "Ora"
                            If value = "" Then outValue = SqlDateTime.Null Else outValue = New SqlDateTime(Date.ParseExact(value, FormOra_Output, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None))
                        Case Else
                            Throw New Exception("Invalid data type for TextBox")
                    End Select
                Case "Checkbox"
                    If value = "1" Then outValue = New SqlBoolean(True) Else outValue = New SqlBoolean(False)
                Case "Combobox"
                    Select Case TIPODATO
                        Case "Stringa"
                            If value = "" Then outValue = SqlString.Null Else outValue = New SqlString(value)
                        Case "N° Intero"
                            If value = "" Then outValue = SqlInt32.Null Else outValue = New SqlInt32(Integer.Parse(value))
                        Case Else
                            Throw New Exception("Invalid data type for TextBox")
                    End Select
                Case Else
                    Throw New Exception("Invalid data type for TextBox")
            End Select

            'aggiungo l'item alla collezione
            myDictionary.Add(_NOMECAMP, outValue)

        End If

        'ritorno il dizionario VUOTO
        Return myDictionary

    End Function

    Public Function GetVisibleControlsCells(ByVal dataSqlConnection As System.Data.SqlClient.SqlConnection) As List(Of HtmlTableCell) Implements IStlSearchFormItem.GetVisibleControlsCells

        Dim myCtlTableLblCell As HtmlTableCell
        Dim myCtlTableCtlCell As HtmlTableCell

        'preparazione celle
        myCtlTableLblCell = New HtmlTableCell
        myCtlTableLblCell.Attributes.Add("class", "lbl")

        myCtlTableCtlCell = New HtmlTableCell

        'assegno una larghezza all'etichetta se siamo in layout hor
        If _searchForm.LayoutType = StlSearchForm.LayoutTypes.Horizontal Then
            If LABELWID = 0 Then
                myCtlTableLblCell.Style.Add("width", AccessToPixels(3).ToString)
            Else
                myCtlTableLblCell.Style.Add("width", AccessToPixels(LABELWID).ToString)
            End If
        End If

        'etichetta
        myCtlTableLblCell.Controls.Add(New LiteralControl(_searchForm.ReplaceHolders(LABELRIC)))

        'controlli visibili e hidden

        Select Case TIPOCTRL

            Case "Testo"
                Dim AC As New TextBox
                AC.CssClass = "txt"

                Select Case TIPODATO
                    Case "Data" : AC.CssClass &= " stl_dt_data_ddmmyyyy"
                End Select

                'controllo visibile
                AC.ID = ControlID(False)
                AC.EnableViewState = False
                'maxlen
                If MAXLUNGH <> 0 Then
                    AC.MaxLength = MAXLUNGH
                End If
                'larghezza
                If LARGHEZZ <> 0 Then
                    AC.Width = Unit.Pixel(CInt(AccessToPixels(LARGHEZZ).Value) - 4)
                Else
                    AC.Width = Unit.Pixel(CInt(AccessToPixels(3).Value) - 4)
                End If
                'altezza
                If ALTEZZAA <> 0 Then
                    AC.Height = AccessToPixels(ALTEZZAA)
                End If

                myCtlTableCtlCell.Controls.Add(AC)
                myCtlTableCtlCell.Attributes.Add("class", "cnt_r")

                'aggiungo a collezione
                control = AC

            Case "Checkbox"
                Dim AC As New CheckBox
                AC.CssClass = "chk"

                AC.ID = ControlID(False)
                AC.EnableViewState = False

                myCtlTableCtlCell.Controls.Add(AC)
                myCtlTableCtlCell.Attributes.Add("class", "cnt_l")

                'aggiungo a collezione
                control = AC

            Case "NullBox"

                Dim AC As New CheckBox
                AC.CssClass = "chk"


                AC.ID = ControlID(False)
                AC.EnableViewState = False

                myCtlTableCtlCell.Controls.Add(AC)
                myCtlTableCtlCell.Attributes.Add("class", "cnt_l")

                'aggiungo a collezione
                control = AC

            Case "Riepilogo"
                Dim AC As New CheckBoxList

                'controllo visibile
                AC.ID = ControlID(False)
                AC.RepeatLayout = RepeatLayout.Flow
                AC.BorderStyle = WebControls.BorderStyle.Solid
                AC.BorderColor = Drawing.Color.FromArgb(210, 210, 210)
                AC.BorderWidth = Unit.Pixel(1)
                AC.BackColor = Drawing.Color.White
                AC.EnableViewState = False

                'larghezza
                If LARGHEZZ <> 0 Then
                    AC.Width = AccessToPixels(LARGHEZZ)
                Else
                    AC.Width = AccessToPixels(3)
                End If
                'altezza
                If ALTEZZAA <> 0 Then
                    AC.Height = AccessToPixels(ALTEZZAA)
                End If

                'scrittura valori
                FillList(dataSqlConnection, AC.Items, False)

                myCtlTableCtlCell.Controls.Add(AC)
                myCtlTableCtlCell.Attributes.Add("class", "cnt_l")

                'aggiungo a collezione
                control = AC

            Case "combobox"

                Dim AC As New DropDownList
                AC.CssClass = "ddn"

                'controllo visibile
                AC.ID = ControlID(False)
                AC.EnableViewState = False

                'larghezza
                If LARGHEZZ <> 0 Then
                    AC.Width = AccessToPixels(LARGHEZZ)
                Else
                    AC.Width = AccessToPixels(3)
                End If

                'scrittura valori
                FillList(dataSqlConnection, AC.Items, True)

                'aggiunta
                myCtlTableCtlCell.Controls.Add(AC)
                myCtlTableCtlCell.Attributes.Add("class", "cnt_r")

                'aggiungo a collezione
                control = AC

        End Select

        'generazione collezione
        Dim cellsList As New List(Of HtmlTableCell)
        cellsList.Add(myCtlTableLblCell)
        cellsList.Add(myCtlTableCtlCell)

        'ritorno le celle
        Return cellsList

    End Function

    Public Function GetWhereClause() As String Implements IStlSearchFormItem.GetWhereClause
        Dim value As String
        Dim myComparaz As String
        Dim whereClause As String

        'innanzitutto azzero la condizione
        whereClause = ""

        'genero il criterio di comparazione
        If COMPARAZ = "" Then
            If TIPODATO = "Stringa" And TIPOCTRL = "Testo" Then
                myComparaz = "like"
            Else
                myComparaz = "="
            End If
        Else
            myComparaz = COMPARAZ
        End If
        myComparaz = " " & myComparaz & " "

        'leggo i dati dal controllo hidden
        value = hiddenControl.Text

        'genero la condizione Where
        Select Case TIPOCTRL
            Case "Testo"
                'creo una condizione where solo se il testo non è nullo
                If value <> "" Then
                    Select Case TIPODATO
                        Case "Stringa"
                            If myComparaz = " like% " Then
                                whereClause = _NOMECAMP & " like " & SQL_String(value & "%")
                            ElseIf myComparaz = " %like% " Then
                                whereClause = _NOMECAMP & " like " & SQL_String("%" & value & "%")
                            Else
                                whereClause = _NOMECAMP & myComparaz & SQL_String(value)
                            End If
                        Case "N° Intero"
                            whereClause = _NOMECAMP & myComparaz & SQL_Int32(Integer.Parse(value))
                        Case "N° Decimale"
                            whereClause = _NOMECAMP & myComparaz & SQL_Double(Double.Parse(value))
                        Case "Data"
                            whereClause = _NOMECAMP & myComparaz & SQL_Date(Date.ParseExact(value, FormData_Output, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None))
                        Case "Ora"
                            whereClause = _NOMECAMP & myComparaz & SQL_Date(Date.ParseExact(value, FormOra_Output, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None))
                    End Select
                End If
            Case "Checkbox"
                If value = "1" Then
                    whereClause = _NOMECAMP & myComparaz & "1"
                Else
                    whereClause = _NOMECAMP & myComparaz & "0"
                End If
            Case "NullBox"
                If value = "1" Then
                    whereClause = _NOMECAMP & myComparaz & " is not null"
                End If
            Case "Combobox"
                If value <> "" Then
                    whereClause = _NOMECAMP & myComparaz
                    If TIPODATO = "Stringa" Then
                        whereClause &= SQL_String(value)
                    ElseIf TIPODATO = "N° Intero" Then
                        whereClause &= SQL_Int32(Integer.Parse(value))
                    End If
                End If
            Case "Riepilogo"
                If value <> "" Then
                    whereClause = _NOMECAMP & " IN ("
                    Dim values As List(Of String) = DequeueSelected(value)
                    Dim selectedValue As String
                    If TIPODATO = "Stringa" Then
                        For Each selectedValue In values
                            whereClause &= SQL_String(selectedValue) & ", "
                        Next
                    ElseIf TIPODATO = "N° Intero" Then
                        For Each selectedValue In values
                            whereClause &= SQL_Int32(CInt(selectedValue)) & ", "
                        Next
                    End If
                    whereClause = Left(whereClause, Len(whereClause) - 2) & ")"
                End If
        End Select

        Return whereClause

    End Function

    Public Sub HiLite() Implements IStlSearchFormItem.HiLite
        If Not control Is Nothing Then
            If TypeOf control Is TextBox Then
                CType(control, TextBox).BackColor = Drawing.Color.Yellow
            ElseIf TypeOf control Is CheckBox Then
                CType(control, CheckBox).BackColor = Drawing.Color.Yellow
            ElseIf TypeOf control Is DropDownList Then
                CType(control, DropDownList).BackColor = Drawing.Color.Yellow
            ElseIf TypeOf control Is CheckBoxList Then
                CType(control, CheckBoxList).BackColor = Drawing.Color.Yellow
            End If
        End If
    End Sub

    Public Property OrderIndex() As Integer Implements IStlSearchFormItem.OrderIndex
        Get
            Return _orderIndex
        End Get
        Set(ByVal value As Integer)
            _orderIndex = value
        End Set
    End Property

    Public Sub PrepareVisibleValueForNewItem() Implements IStlSearchFormItem.PrepareVisibleValueForNewItem
        If Not NUOVOFLD Then
            Me.ClearVisible()
        Else

        End If
    End Sub

    Public Property SearchForm() As StlSearchForm Implements IStlSearchFormItem.SearchForm
        Get
            Return _searchForm
        End Get
        Set(ByVal value As StlSearchForm)
            _searchForm = value
        End Set
    End Property

    Public Function ValidateForNewItem(ByVal dataSqlConnection As System.Data.SqlClient.SqlConnection) As String Implements IStlSearchFormItem.ValidateForNewItem
        Dim result As ValidateFormatResult = ValidateFormat()
        If Not result.Valid Then
            Return SearchForm.ReplaceHolders(LABELRIC) & ": " & result.InvalidFormatErrorMessage
        Else
            'valido
            If NUOVOFLD And NUOVOREQ And result.Empty Then
                Return SearchForm.ReplaceHolders(LABELRIC) & ": dato obbligatorio - inserisci un valore"
            End If
        End If
        Return ""
    End Function

    Public Function ValidateForSearch(ByVal dataSqlConnection As System.Data.SqlClient.SqlConnection) As String Implements IStlSearchFormItem.ValidateForSearch
        Dim result As ValidateFormatResult = ValidateFormat()
        If result.Valid Then
            Return ""
        Else
            Return SearchForm.ReplaceHolders(LABELRIC) & ": " & result.InvalidFormatErrorMessage
        End If
    End Function

    Private Class ValidateFormatResult
        Public Valid As Boolean = True
        Public Empty As Boolean = False
        Public InvalidFormatErrorMessage As String = ""
    End Class

    Private Function ValidateFormat() As ValidateFormatResult

        'restituisce "" se il formato è valido oppure se il campo è vuoto
        'altrimenti restituisce la descrizione del messaggio di errore
        'riscrive anche i valori facendo trimEnd, etc

        Dim result As New ValidateFormatResult With {.Valid = True, .Empty = False, .InvalidFormatErrorMessage = ""}


        Dim valFrm As String
        Dim valOut As String

        Select Case TIPOCTRL
            Case "Testo"

                valFrm = CType(control, TextBox).Text.TrimEnd
                valOut = valFrm


                'SE il campo contiene qualcosa...
                If valFrm <> "" Then

                    result.Empty = False

                    Select Case TIPODATO
                        Case "Stringa"
                            'sempre valido
                        Case "N° Intero"
                            'vediamo se possiamo convertire in Integer...
                            Try
                                Dim i As Integer = Integer.Parse(valFrm)
                                valOut = i.ToString
                            Catch ex As Exception
                                result.Valid = False
                            End Try
                        Case "N° Decimale"
                            Try
                                Dim dbl As Double = Double.Parse(valFrm)
                                valOut = dbl.ToString(System.Globalization.CultureInfo.CurrentCulture)
                            Catch ex As Exception
                                result.Valid = False
                            End Try
                        Case "Data"
                            Try
                                Dim d As Date = DateTime.ParseExact(valFrm, FormData_Accettati, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None)
                                valOut = d.ToString(FormData_Output, Softailor.Global.Cultures.CulturaItalian)
                            Catch ex As Exception
                                result.Valid = False
                            End Try
                        Case "Ora"
                            Try
                                Dim h As Date = DateTime.ParseExact(valFrm, FormOra_Accettati, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None)
                                valOut = h.ToString(FormOra_Output, Softailor.Global.Cultures.CulturaItalian)
                            Catch ex As Exception
                                result.Valid = False
                            End Try
                    End Select
                Else
                    'il campo è vuoto
                    result.Empty = True
                End If
                'riscrivo il valore per eventuali TRIM
                CType(control, TextBox).Text = valOut
            Case "Checkbox"
                'sempre valido
                result.Valid = True
                result.Empty = False
            Case "NullBox"
                'sempre valido
                result.Valid = True
                result.Empty = False
            Case "Combobox"
                'sempre valido
                'imposto Empty
                result.Valid = True
                result.Empty = CType(control, DropDownList).SelectedValue = ""
            Case "Riepilogo"
                'sempre valido per ricerca
                'me ne frego per creazione
                result.Valid = True
                result.Empty = True
        End Select

        'creazione del messaggio di errore
        If Not result.Valid Then
            If TIPOCTRL = "Testo" And TIPODATO = "Data" Then
                result.InvalidFormatErrorMessage = "formato non valido - utilizza gg/mm/aaaa"
            ElseIf TIPOCTRL = "Testo" And TIPODATO = "Ora" Then
                result.InvalidFormatErrorMessage = "formato non valido - utilizza hh.mm oppure hh.mm.ss"
            Else
                result.InvalidFormatErrorMessage = "formato non valido"
            End If
        End If

        Return result

    End Function

    Private Sub FillList(ByVal dataSqlConnection As SqlConnection, ByVal items As System.Web.UI.WebControls.ListItemCollection, ByVal addEmptyItem As Boolean)

        'scrittura dei listItem

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        Dim SQLQuery As String
        Dim LC As String
        Dim ColAss As Integer, NumCol As Integer

        Dim cntCol As Integer, StartDisp As Integer
        Dim nRighe As Integer
        Dim cntRig As Integer

        Dim Riga As String
        Dim Valori(0, 0) As String
        Dim Valore As String

        Dim Chiave As String
        Dim ListaVal As String

        ListaVal = ""

        ColAss = COLONASS
        NumCol = NUM_COLO

        'Pulizia della stringa relativa alla larghezza delle colonne->determino se la prima colonna deve essere nascosta o meno
        If LARGHCOL = "" Then
            LC = "1;"
        Else
            LC = LARGHCOL
            LC = Replace(LC, " ", "")
            LC = Replace(LC, "cm", "")
            If Right(LC, 1) <> ";" Then LC = LC & ";"
        End If
        If LC Like "0;*" Then StartDisp = 1 Else StartDisp = 0

        SQLQuery = SQLCMBEV

        'aggiunta eventuale linea bianca...
        If addEmptyItem Then
            items.Add(New ListItem("", ""))
        End If

        Select Case TIPO_QRY
            Case "Query"
                'dobbiamo aprire la query...

                SQLQuery = _searchForm.ReplaceHolders(SQLQuery)

                dbCmd = dataSqlConnection.CreateCommand()
                dbCmd.CommandText = SQLQuery
                dbRdr = dbCmd.ExecuteReader()
                Do While dbRdr.Read()
                    'scrittura OPTION
                    If dbRdr.IsDBNull(ColAss - 1) Then
                        Chiave = ""
                    Else
                        Chiave = dbRdr.GetSqlValue(ColAss - 1).ToString
                    End If

                    Riga = ""
                    For cntCol = StartDisp To NumCol - 1
                        If Not dbRdr.IsDBNull(cntCol) Then
                            Riga = Riga & dbRdr.GetValue(cntCol).ToString & " - "
                        End If
                    Next cntCol
                    If Len(Riga) > 0 Then Riga = Left(Riga, Len(Riga) - 3)
                    items.Add(New ListItem(Riga, Chiave))
                Loop
                dbRdr.Close()

            Case "Elenco Valori"
                'obbiamo fare il parsing di SQLQuery che contiene elenco valori separati da ; e metterli nell'array Valori
                ReDim Valori(NumCol - 1, 0)
                nRighe = 0
                cntCol = NumCol
                If Right(SQLQuery, 1) <> ";" Then SQLQuery = SQLQuery & ";"
                Do While SQLQuery <> ""
                    'estraiamo valore
                    Valore = Mid(SQLQuery, 1, InStr(SQLQuery, ";") - 1)
                    SQLQuery = Mid(SQLQuery, InStr(SQLQuery, ";") + 1)
                    If cntCol = NumCol Then
                        nRighe += 1
                        ReDim Preserve Valori(NumCol - 1, nRighe)
                        cntCol = 0
                    End If
                    Valori(cntCol, nRighe) = Valore
                    cntCol = cntCol + 1
                Loop

                'scrittura effettiva dei dati
                For cntRig = 1 To nRighe
                    'scrittura OPTION

                    Chiave = Valori(ColAss - 1, cntRig)
                    Riga = ""

                    For cntCol = StartDisp To NumCol - 1
                        If Valori(cntCol, cntRig) <> "" Then
                            Riga = Riga & Valori(cntCol, cntRig) & " - "
                        End If
                    Next cntCol
                    If Len(Riga) > 0 Then Riga = Left(Riga, Len(Riga) - 3)
                    items.Add(New ListItem(Riga, Chiave))
                Next cntRig
        End Select
    End Sub

    Public Function GetValidKeyValue() As System.Collections.Generic.KeyValuePair(Of String, Object) Implements IStlSearchFormItem.GetValidKeyValue

        Dim value As String
        Dim outValue As Object

        'popolo l'item a seconda dei dati nei controlli
        value = hiddenControl.Text
        Select Case TIPOCTRL
            Case "Testo"
                Select Case TIPODATO
                    Case "Stringa"
                        If value = "" Then outValue = SqlString.Null Else outValue = New SqlString(value)
                    Case "N° Intero"
                        If value = "" Then outValue = SqlInt32.Null Else outValue = New SqlInt32(Integer.Parse(value))
                    Case "N° Decimale"
                        If value = "" Then outValue = SqlDouble.Null Else outValue = New SqlDouble(Double.Parse(value))
                    Case "Data"
                        If value = "" Then outValue = SqlDateTime.Null Else outValue = New SqlDateTime(Date.ParseExact(value, FormData_Output, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None))
                    Case "Ora"
                        If value = "" Then outValue = SqlDateTime.Null Else outValue = New SqlDateTime(Date.ParseExact(value, FormOra_Output, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None))
                    Case Else
                        Throw New Exception("Invalid data type for TextBox")
                End Select
            Case "Checkbox"
                If value = "1" Then outValue = New SqlBoolean(True) Else outValue = New SqlBoolean(False)
            Case "Combobox"
                Select Case TIPODATO
                    Case "Stringa"
                        If value = "" Then outValue = SqlString.Null Else outValue = New SqlString(value)
                    Case "N° Intero"
                        If value = "" Then outValue = SqlInt32.Null Else outValue = New SqlInt32(Integer.Parse(value))
                    Case Else
                        Throw New Exception("Invalid data type for TextBox")
                End Select
            Case Else
                Throw New Exception("Invalid data type for TextBox")
        End Select

        Return New KeyValuePair(Of String, Object)(_NOMECAMP, outValue)

    End Function

    Public Sub SetVisibleValue(ByVal value As String)
        Select Case TIPOCTRL
            Case "Testo"
                CType(control, TextBox).Text = value
            Case "Checkbox"
                CType(control, CheckBox).Checked = (value = "1") Or (value.ToLower = "true")
            Case "Combobox"
                CType(control, DropDownList).SelectedValue = value
            Case Else
                Throw New Exception("Invalid control type")
        End Select
    End Sub
End Class