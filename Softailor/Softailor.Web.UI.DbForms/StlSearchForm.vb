Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.Unit
Imports System.Configuration
Imports System.ComponentModel
Imports Softailor.Global.JSUtils
Imports Softailor.Global.SqlUtils
Imports System.Web.UI.HtmlControls

Public Class StlSearchForm
    Inherits Panel
    Implements ISqlStringProvider

    'Eventi
    Public Event BuildCustomSqlString(ByVal searchSqlValues As Dictionary(Of String, Object), ByVal SearchActive As Boolean, ByVal SHOWSTRT As Boolean, ByRef customSql As String)
    Public Event GetCustomConfirmationMessage(ByVal dataDbConn As SqlConnection, ByVal sqlValues As Dictionary(Of String, Object), ByRef Message As String)

    Public Event DoingSearch()
    Public Event DoingReset()

    'oggetti collegati
    Private _ro_childrenStlGridViews As New List(Of StlGridView)
    Private _ro_childrenStlGridViewsSearched As Boolean = False
    Private _ro_containingUpdatePanel As UpdatePanel
    Private _ro_containingUpdatePanelSearched As Boolean = False

    'connessioni DB
    Private _metaSqlConnection As SqlConnection
    Private _dataSqlConnection As SqlConnection

    'variabili per proprietà
    Private _searchName As String                                           'nome ricerca nella tabella mb_RICERC
    Private _title As String = "Parametri Ricerca/Creazione"                 'titolo box
    Private _searchParameters As New Dictionary(Of Integer, String)         'parametriAddizionali
    Private _allowAddNew As Boolean = True                                  'consenti aggiunta (se la ricerca lo prevede)
    Private _newButtonText As String = "Nuovo"
    Private _clearButtonText As String = "Pulisci"
    Private _searchButtonText As String = "Cerca"
    Private _unableToSearchText As String = "Impossibile procedere a causa dei seguenti errori:"
    Private _unableToCreateNewText As String = "Impossibile creare l'elemento a causa dei seguenti errori:"
    Private _creationConfirmationText As String = "Confermi la creazione dell'elemento?"
    Private _layoutType As LayoutTypes
    Private _dontSelectFirstRecord As Boolean = False
    Private _NewLineAfter As New List(Of String)

    'variabili per gestione metadati
    Private _searchItems As StlSearchFormItemCollection

    Private _ID_RICER As Integer
    Private _SQL_BASE As String
    Private _CAMPO_ID As String
    Private _USEWHERE As Boolean
    Private _SHOWSTRT As Boolean
    Private _ORDER_BY As String

    'variabili per gestione flusso dati
    Private searchActive As TextBox

    'comandi
    Dim CS As Button            'cerca
    Dim CR As Button            'pulisci
    Dim CN As Button            'nuovo (visibile)
    Dim CNH As LinkButton       'conferma nuovo (invisibile)

    'enum per tipo impaginazione
    Public Enum LayoutTypes As Integer
        Vertical = 0
        Horizontal = 1
    End Enum

    Public Sub New()
        _layoutType = LayoutTypes.Vertical
        _searchItems = New StlSearchFormItemCollection(Me)
    End Sub

    

#Region "Proprietà"

    <Category("Stl")> _
    Public Property SearchName() As String
        Get
            Return _searchName
        End Get
        Set(ByVal Value As String)
            _searchName = Value
        End Set

    End Property

    <Category("Stl"), DefaultValue(False)> _
    Public Property DontSelectFirstRecord() As Boolean
        Get
            Return _dontSelectFirstRecord
        End Get
        Set(ByVal value As Boolean)
            _dontSelectFirstRecord = value
        End Set
    End Property

    <Category("Stl"), DefaultValue(LayoutTypes.Vertical)> _
    Public Property LayoutType() As LayoutTypes
        Get
            Return _layoutType
        End Get
        Set(ByVal value As LayoutTypes)
            _layoutType = value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Parametri Ricerca/Creazione")> _
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal Value As String)
            _title = Value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Nuovo")> _
    Public Property NewButtonText() As String
        Get
            Return _newButtonText
        End Get
        Set(ByVal value As String)
            _newButtonText = value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Pulisci")> _
    Public Property ClearButtonText() As String
        Get
            Return _clearButtonText
        End Get
        Set(ByVal value As String)
            _clearButtonText = value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Cerca")> _
    Public Property SearchButtonText() As String
        Get
            Return _searchButtonText
        End Get
        Set(ByVal value As String)
            _searchButtonText = value
        End Set
    End Property

    <Category("Stl"), DefaultValue(True)> _
    Public Property AllowAddNew() As Boolean
        Get
            Return _allowAddNew
        End Get
        Set(ByVal value As Boolean)
            _allowAddNew = value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Impossibile procedere a causa dei seguenti errori:")> _
    Public Property UnableToSearchText() As String
        Get
            Return _unableToSearchText
        End Get
        Set(ByVal value As String)
            _unableToSearchText = value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Impossibile creare l'elemento a causa dei seguenti errori:")> _
    Public Property UnableToCreateNewText() As String
        Get
            Return _unableToCreateNewText
        End Get
        Set(ByVal value As String)
            _unableToCreateNewText = value
        End Set
    End Property

    <Category("Stl"), DefaultValue("Confermi la creazione dell'elemento?")> _
   Public Property CreationConfirmationText() As String
        Get
            Return _creationConfirmationText
        End Get
        Set(ByVal value As String)
            _creationConfirmationText = value
        End Set
    End Property

    <Browsable(False)> _
    Public Property SearchParameters() As Dictionary(Of Integer, String)
        Get
            Return _searchParameters
        End Get
        Set(ByVal Value As Dictionary(Of Integer, String))
            _searchParameters = Value
        End Set
    End Property
#End Region

    Public Sub AddNewLineAfter(ByVal NOMECAMP As String)
        _NewLineAfter.Add(NOMECAMP)
    End Sub

    Public Sub HiLiteField(ByVal NOMECAMP As String)
        Dim sItem As IStlSearchFormItem = _searchItems.FindByNOMECAMP(NOMECAMP)
        If Not sItem Is Nothing Then sItem.HiLite()
    End Sub

    Public Sub AddSearchFormItem(ByVal searchFormItem As IStlSearchFormItem)
        _searchItems.Add(searchFormItem)
    End Sub

    Public Function IsSearchActive() As Boolean
        Return searchActive.Text = "1"
    End Function

    Public Sub ForceSearch(ByVal copyToHiddenControls As Boolean)
        If copyToHiddenControls Then
            onSearchClick(Nothing, Nothing)
        Else
            'aggiornamento delle griglie e aggancio alla prima riga
            For Each stlGridView In ChildrenStlGridViews
                stlGridView.DataBind()
                If _dontSelectFirstRecord Then
                    stlGridView.GotoEmptyRow()
                Else
                    stlGridView.GotoFirstOrEmptyRow()
                End If
                stlGridView.UpdateParentPanel()
            Next
        End If
    End Sub

    Public Function GetValidSqlValue(key As String) As Object
        Dim outValue As Object = Nothing
        For Each sitem As IStlSearchFormItem In _searchItems
            Dim kvp = sitem.GetValidKeyValue()
            If Not IsNothing(kvp.Key) Then
                If kvp.Key = key Then
                    outValue = kvp.Value
                End If
            End If
        Next
        Return outValue
    End Function

    Public Function GetSql() As String Implements ISqlStringProvider.GetSql

        'costruzione della query

        'prima di costruire, lancio l'evento...
        Dim customSql As String = ""
        Dim searchSqlValues As New Dictionary(Of String, Object)
        'devo passare tutti i valori
        For Each sitem As IStlSearchFormItem In _searchItems
            Dim kvp = sitem.GetValidKeyValue()
            If Not IsNothing(kvp.Key) Then searchSqlValues.Add(kvp.Key, kvp.Value)
        Next
        'lancio l'evento
        RaiseEvent BuildCustomSqlString(searchSqlValues, searchActive.Text = "1", _SHOWSTRT, customSql)

        'se ho ottenuto qualcosa...
        If customSql <> "" Then
            Return customSql
            Exit Function
        End If

        '...altrimenti faccio in modalità classica
        Dim whereAtom As String
        Dim generateEmpty As Boolean
        Dim query As String

        If searchActive.Text = "1" Then
            generateEmpty = False
        Else
            generateEmpty = Not _SHOWSTRT
        End If

        'generazione di tutte le whereClauses

        If generateEmpty Then
            'query che restituisce zero record (WHERE 1=0)
            query = _SQL_BASE
            If _USEWHERE Then
                query &= " WHERE (1=0)"
            Else
                query &= " AND (1=0)"
            End If
        Else
            'query effettiva
            query = _SQL_BASE
            Dim isFirst As Boolean = True
            For Each sItem As IStlSearchFormItem In _searchItems
                whereAtom = sItem.GetWhereClause
                If whereAtom <> "" Then
                    'aggiungo WHERE o AND
                    If isFirst Then
                        If _USEWHERE Then
                            query &= " WHERE "
                        Else
                            query &= " AND "
                        End If
                        isFirst = False
                    Else
                        query &= " AND "
                    End If
                    'aggiungo il filtro
                    query &= "(" & whereAtom & ")"
                End If
            Next
        End If

        'aggiunta dell'ordinamento
        If _ORDER_BY <> "" Then query &= " " & _ORDER_BY
        Return query
    End Function

    Private Sub StlSearchForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.DesignMode Then
            'apertura connessioni DB
            _metaSqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("SiteTailorMetaConnectionString").ConnectionString)
            _dataSqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("SiteTailorDbConnectionString").ConnectionString)
            _metaSqlConnection.Open()
            _dataSqlConnection.Open()

            'lettura metadati
            ReadDbSearchItems()

            'eseguo un SORT sulle items in modo da "aggiustare" quelle
            'eventualmente aggiunte durante un Init della pagina
            _searchItems.Sort()

            'generazione controlli
            CreateControls()
        End If

    End Sub

    Private Sub StlSearchForm_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'invece del defaultbutton, una routine STL che prima mette il focus sul controllo "cerca"
        'Me.Attributes.Add("onkeypress", "javascript:return WebForm_FireDefaultButton(event, '" & CS.ClientID & "')")
        Me.Attributes.Add("onkeypress", "javascript:return stl_search_FireDefaultButton(event, '" & CS.ClientID & "')")

    End Sub

    Private Sub StlSearchForm_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Not Me.DesignMode Then
            If Not _metaSqlConnection Is Nothing Then _metaSqlConnection.Close()
            If Not _dataSqlConnection Is Nothing Then _dataSqlConnection.Close()
        End If
    End Sub

    Private Sub ReadDbSearchItems()
        'lettura dei metadati

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim sItem As DbSearchItem

        'lettura dati base ricerca
        dbCmd = New SqlCommand("SELECT ID_RICER, SQL_BASE, CAMPO_ID, USEWHERE, SHOWSTRT FROM mb_RICERC WHERE NOME_RIC=" & SQL_String(_searchName), _metaSqlConnection)
        dbRdr = dbCmd.ExecuteReader()
        With dbRdr
            .Read()
            _ID_RICER = .GetInt32(0)
            _SQL_BASE = ReplaceHolders(.GetString(1))
            _CAMPO_ID = .GetString(2)
            _USEWHERE = .GetBoolean(3)
            _SHOWSTRT = .GetBoolean(4)
        End With
        dbRdr.Close()

        'lettura lista campi order by
        dbCmd = New SqlCommand("SELECT NOMECAMP FROM mb_RITEMS WHERE (ORDINAME is not null) AND (RICERCAA=" & _ID_RICER.ToString & ") ORDER BY ORDINAME", _metaSqlConnection)
        dbRdr = dbCmd.ExecuteReader()
        _ORDER_BY = ""
        Do While dbRdr.Read()
            _ORDER_BY &= dbRdr.GetString(0) & ", "
        Loop
        dbRdr.Close()
        If _ORDER_BY <> "" Then
            _ORDER_BY = " ORDER BY " & Left(_ORDER_BY, Len(_ORDER_BY) - 2)
        End If

        'lettura degli items di ricerca
        dbCmd = New SqlCommand("SELECT ID_RITEM, LABELRIC, LARGHEZZ, NOMECAMP, COMPARAZ, " & _
                                  "TIPOCTRL, MAXLUNGH, TIPODATO, TIPO_QRY, SQLCMBEV, LARGHCOL, " & _
                                  "NUM_COLO, COLONASS, ALTEZZAA, NUOVOFLD, NUOVOREQ, LABELWID, ORD_RICE FROM mb_RITEMS WHERE (ORD_RICE is not null) AND (RICERCAA=" & _ID_RICER.ToString & ")", _metaSqlConnection)
        dbRdr = dbCmd.ExecuteReader()
        With dbRdr
            Do While .Read
                sItem = New DbSearchItem()
                sItem.OrderIndex = .GetInt16(17)
                sItem.ID_RITEM = .GetInt32(0)
                sItem.LABELRIC = .GetString(1)
                sItem.LARGHEZZ = Nz(.GetSqlSingle(2))
                sItem.NOMECAMP = Nz(.GetSqlString(3))
                sItem.COMPARAZ = Nz(.GetSqlString(4))
                sItem.TIPOCTRL = .GetString(5)
                sItem.MAXLUNGH = Nz(.GetSqlInt32(6))
                sItem.TIPODATO = .GetString(7)
                sItem.TIPO_QRY = Nz(.GetSqlString(8))
                sItem.SQLCMBEV = Nz(.GetSqlString(9))
                sItem.LARGHCOL = Nz(.GetSqlString(10))
                sItem.NUM_COLO = Nz(.GetSqlInt16(11), 1)
                sItem.COLONASS = Nz(.GetSqlInt16(12), 1)
                sItem.ALTEZZAA = Nz(.GetSqlSingle(13))
                sItem.NUOVOFLD = Nz(.GetSqlBoolean(14))
                sItem.NUOVOREQ = Nz(.GetSqlBoolean(15))
                sItem.LABELWID = Nz(.GetSqlSingle(16), 0)
                _searchItems.Add(sItem)
            Loop
        End With
        dbRdr.Close()
    End Sub

    Private Sub CreateControls()

        'parti tabella esterna
        Dim myTable As New HtmlTable
        Dim myTitleRow As New HtmlTableRow
        Dim myTitleCell As New HtmlTableCell
        Dim myControlsRow As New HtmlTableRow
        Dim myControlsCell As New HtmlTableCell
        Dim myCommandsRow As New HtmlTableRow
        Dim myCommandsCell As New HtmlTableCell

        'parti tabella controlli
        Dim myCtlTable As HtmlTable
        Dim myCtlTableRow As HtmlTableRow

        'classi celle e tabella esterna
        myTable.Attributes.Add("class", "stl_sft")
        myTable.CellSpacing = 0
        myTable.CellPadding = 0
        myTable.Border = 0
        If Me.ContainingUpdatePanel IsNot Nothing Then
            If TypeOf (Me.ContainingUpdatePanel) Is StlUpdatePanel Then
                myTable.Style.Add("height", (CInt(CType(Me.ContainingUpdatePanel, StlUpdatePanel).Height.Value)).ToString & "px")
            End If
        End If

        myTitleCell.Attributes.Add("class", "title")
        myControlsCell.Attributes.Add("class", "controls")
        myCommandsCell.Attributes.Add("class", "commands")

        'struttura tabella esterna
        myTitleRow.Cells.Add(myTitleCell)
        myControlsRow.Cells.Add(myControlsCell)
        myCommandsRow.Cells.Add(myCommandsCell)
        myTable.Rows.Add(myTitleRow)
        myTable.Rows.Add(myControlsRow)
        myTable.Rows.Add(myCommandsRow)



        'titolo
        If _title <> "" Then
            myTitleCell.Controls.Add(New LiteralControl(_title))
        Else
            myTitleCell.Controls.Add(New LiteralControl("&nbsp;"))
        End If


        'Creazione controlli visibili
        Select Case LayoutType
            Case LayoutTypes.Horizontal

                'creazione della tabella interna
                myCtlTable = New HtmlTable

                'classi tabella interna
                myCtlTable.Attributes.Add("class", "stl_sft_ctt")
                If _layoutType = LayoutTypes.Vertical Then myCtlTable.Style.Add("width", "100%")
                'myCtlTable.CellSpacing = 0
                'myCtlTable.CellPadding = 0
                'myCtlTable.Border = 0

                'creazione riga
                myCtlTableRow = New HtmlTableRow

                'aggiunta celle alla singola riga per ciascun controllo
                For Each sItem In _searchItems
                    Dim cells As List(Of HtmlTableCell) = sItem.GetVisibleControlsCells(_dataSqlConnection)
                    For Each cell As HtmlTableCell In cells
                        myCtlTableRow.Cells.Add(cell)
                    Next
                    'se ho un acapo dopo, accodo la tabella e ne creo una nuova
                    If _NewLineAfter.Contains(sItem.NOMECAMP) Then
                        myCtlTable.Rows.Add(myCtlTableRow)
                        'aggiunta tabella controlli al controllo
                        myControlsCell.Controls.Add(myCtlTable)

                        'ricreo la tabella
                        myCtlTable = New HtmlTable

                        'classi tabella interna
                        myCtlTable.Attributes.Add("class", "stl_sft_ctt")
                        If _layoutType = LayoutTypes.Vertical Then myCtlTable.Style.Add("width", "100%")
                        myCtlTable.CellSpacing = 0
                        myCtlTable.CellPadding = 0
                        myCtlTable.Border = 0

                        myCtlTableRow = New HtmlTableRow
                    End If
                Next

                myCtlTable.Rows.Add(myCtlTableRow)
                'aggiunta tabella controlli al controllo
                myControlsCell.Controls.Add(myCtlTable)

            Case LayoutTypes.Vertical

                'creazione della tabella interna
                myCtlTable = New HtmlTable

                'classi tabella interna
                myCtlTable.Attributes.Add("class", "stl_sft_ctt")
                If _layoutType = LayoutTypes.Vertical Then myCtlTable.Style.Add("width", "100%")
                myCtlTable.CellSpacing = 0
                myCtlTable.CellPadding = 0
                myCtlTable.Border = 0

                'aggiunta celle alla singola riga per ciascun controllo
                For Each sItem In _searchItems

                    'creazione riga
                    myCtlTableRow = New HtmlTableRow

                    'aggiunta celle alla riga
                    Dim cells As List(Of HtmlTableCell) = sItem.GetVisibleControlsCells(_dataSqlConnection)
                    For Each cell As HtmlTableCell In cells
                        myCtlTableRow.Cells.Add(cell)
                    Next

                    'aggiunta riga alla tabella
                    myCtlTable.Rows.Add(myCtlTableRow)

                Next

                'aggiunta tabella controlli al controllo
                myControlsCell.Controls.Add(myCtlTable)

        End Select



        'alla fine generiamo i bottoni di "nuovo", "pulisci" e "cerca"

        If _allowAddNew Then
            'comando nuovo - visibile
            CN = New Button
            CN.EnableViewState = False
            CN.Text = _newButtonText
            CN.ID = Me.ID & "_New"
            CN.CssClass = "btnNewClear"
            AddHandler CN.Click, AddressOf onNewClick
            myCommandsCell.Controls.Add(CN)
            CNH = New LinkButton
            CNH.EnableViewState = True
            CNH.Text = ""
            CNH.ID = Me.ID & "-NewConfirmed"
            AddHandler CNH.Click, AddressOf onNewConfirmed

            myCommandsCell.Controls.Add(CNH)
        End If

        CR = New Button
        CR.ID = Me.ID & "_Clear"
        CR.EnableViewState = False
        CR.Text = _clearButtonText
        CR.CssClass = "btnNewClear"
        AddHandler CR.Click, AddressOf onResetClick

        myCommandsCell.Controls.Add(CR)

        CS = New Button
        CS.ID = Me.ID & "_Search"
        CS.Text = _searchButtonText
        CS.CssClass = "btnSearch"
        '
        'Me.DefaultButton = Me.ID & "_Search"

        AddHandler CS.Click, AddressOf onSearchClick

        myCommandsCell.Controls.Add(CS)

        'composizione finale - attenzione: metti QUI tutti i controlli a livello ROOT

        'aggiunta tabella
        Me.Controls.Add(myTable)

        'creazione controlli invisibili
        For Each sItem In _searchItems
            For Each control As Control In sItem.GetHiddenControls()
                Me.Controls.Add(control)
            Next
        Next

        'generazione searchActive
        searchActive = New TextBox
        searchActive.ID = Me.ID & "_SearchActive"
        searchActive.Visible = False
        searchActive.EnableViewState = True
        searchActive.Text = ""
        Me.Controls.Add(searchActive)

    End Sub

#Region "Risposta a pressione bottoni"

    Public Event CreateNew(ByVal dataDbConn As SqlConnection, ByVal sqlValues As Dictionary(Of String, Object), ByRef errorEncountered As Boolean, ByRef errorMessage As String, ByRef gotoNewKey As Boolean, ByRef newKeyFieldName As String, ByRef newKeyValue As String)

    Private Sub onNewConfirmed(ByVal sender As Object, ByVal e As System.EventArgs)
        'quando arriviamo qui, abbiamo - nei controlli nascosti - i dati veri per la creazione dell'elemento
        'i dati sono già correttamente TRIMmati e formattati
        'inoltre tutti gli altri dati sono vuoti.

        'iniziamo a creare una lista di sqlValues
        Dim sqlValues As New Dictionary(Of String, Object)
        For Each sItem As IStlSearchFormItem In _searchItems
            For Each newValue In sItem.GetNewItemSqlValues(_dataSqlConnection)
                sqlValues.Add(newValue.Key, newValue.Value)
            Next
        Next

        'lancio dell'evento
        Dim errorEncountered As Boolean = False
        Dim errorMessage As String = ""
        Dim gotoNewKey As Boolean = False
        Dim newKeyValue As String = ""
        Dim newKeyFieldName As String = ""
        RaiseEvent CreateNew(_dataSqlConnection, sqlValues, errorEncountered, errorMessage, gotoNewKey, newKeyFieldName, newKeyValue)

        If errorEncountered Then
            'c'è stato un errore: mostro l'errore e NON rieseguo la ricerca
            Me.LaunchSearchError(errorMessage)
        Else
            'ok l'elemento è stato creato
            'aggiorno le griglie
            For Each stlGridView In ChildrenStlGridViews
                If gotoNewKey Then
                    stlGridView.PreGotoRow(newKeyFieldName, newKeyValue, True, True)
                    stlGridView.DataBind()
                    stlGridView.PostGotoRow()
                    stlGridView.UpdateParentPanel()

                Else
                    stlGridView.DataBind()
                    stlGridView.GotoFirstOrEmptyRow()
                    stlGridView.UpdateParentPanel()

                End If
            Next
        End If

    End Sub

    Private Sub onNewClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim saveOk As Boolean
        Dim stlGridView As StlGridView

        'prima di cercare, provo a salvare se dirty
        saveOk = True
        For Each stlGridView In ChildrenStlGridViews
            saveOk = saveOk And stlGridView.SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, False, True)
        Next

        If saveOk Then
            'ok, siamo riusciti a salvare.
            'verifichiamo che tutti i dati siano corretti E che i dati richiesti ci siano
            If ValidateControlsForNewItem() Then

                'ok, tutti i dati sono corretti e i dati richiesti ci sono

                'innanzitutto pulisco i controlli con dati non utilizzati nel "nuovo"
                'per i controlli di tipo testo - stringa, tolgo le percentuali
                For Each sItem As IStlSearchFormItem In _searchItems
                    sItem.PrepareVisibleValueForNewItem()
                Next

                'a questo punto eseguo una ricerca, in modo che l'utente veda se c'è già qualcosa
                CopyValuesToHiddenControls()

                'imposto search active
                searchActive.Text = "1"

                'aggiornamento delle griglie e aggancio alla prima riga
                For Each stlGridView In ChildrenStlGridViews
                    stlGridView.DataBind()
                    stlGridView.GotoFirstOrEmptyRow()
                    stlGridView.UpdateParentPanel()
                Next

                'costruzione dello script di conferma
                Dim sScript As String

                'lancio evento per eventuale conferma personalizzata
                Dim sMessage As String = _creationConfirmationText

                'iniziamo a creare una lista di sqlValues
                Dim sqlValues As New Dictionary(Of String, Object)
                For Each sItem As IStlSearchFormItem In _searchItems
                    For Each newValue In sItem.GetNewItemSqlValues(_dataSqlConnection)
                        sqlValues.Add(newValue.Key, newValue.Value)
                    Next
                Next

                RaiseEvent GetCustomConfirmationMessage(_dataSqlConnection, sqlValues, sMessage)

                sMessage = Softailor.Global.JSUtils.EncodeJsStringWithQuotes(sMessage)

                'ID controllo (sostituzione _ con $)
                Dim CNHClientIDPrefix = Mid(CNH.UniqueID, 1, Len(CNH.UniqueID) - Len(CNH.ID))
                Dim CNHClientID As String = "'" & CNHClientIDPrefix.Replace(Me.ClientIDSeparator, "$") & CNH.ID & "'"
                Dim args As String = "''"

                sScript = "setCAP(" & sMessage & ", " & CNHClientID & ", " & args & ");"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ID & "_addnewconfirmed", sScript, True)

            End If
        End If

    End Sub

    Private Sub onSearchClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim saveOk As Boolean
        Dim stlGridView As StlGridView

        'prima di cercare, provo a salvare se dirty
        saveOk = True
        For Each stlGridView In ChildrenStlGridViews
            saveOk = saveOk And stlGridView.SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, False, True)
        Next

        If saveOk Then
            If ValidateControlsForSearch() Then
                CopyValuesToHiddenControls()
                'imposto search active
                searchActive.Text = "1"
                'aggiornamento delle griglie e aggancio alla prima riga
                For Each stlGridView In ChildrenStlGridViews
                    stlGridView.DataBind()
                    If _dontSelectFirstRecord Then
                        stlGridView.GotoEmptyRow()
                    Else
                        stlGridView.GotoFirstOrEmptyRow()
                    End If
                    'stlGridView.ScrollTop()
                    stlGridView.UpdateParentPanel()

                    RaiseEvent DoingSearch()
                Next
            End If
        End If
    End Sub

    Private Sub onResetClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim saveOk As Boolean
        Dim stlGridView As StlGridView

        'prima di resettare, provo a salvare se dirty
        saveOk = True
        For Each stlGridView In ChildrenStlGridViews
            saveOk = saveOk And stlGridView.SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, False, True)
        Next

        If saveOk Then
            'non valido i controlli, tanto li pulisco

            'pulizia
            For Each sItem In _searchItems
                sItem.ClearVisible()
            Next

            'copia
            CopyValuesToHiddenControls()

            'imposto search active
            searchActive.Text = ""

            'aggiornamento delle griglie e aggancio alla prima riga
            For Each stlGridView In ChildrenStlGridViews
                stlGridView.DataBind()
                stlGridView.GotoFirstOrEmptyRow()
                stlGridView.UpdateParentPanel()
            Next

            'evento
            RaiseEvent DoingReset()
        End If

    End Sub
#End Region

    Private Function ValidateControlsForSearch() As Boolean

        Dim allValid As Boolean = True
        Dim errList As String = ""
        Dim err As String

        For Each sItem As IStlSearchFormItem In _searchItems
            err = sItem.ValidateForSearch(_dataSqlConnection)
            If err <> "" Then
                allValid = False
                sItem.HiLite()
                errList &= vbCrLf & " - " & err
            End If
        Next

        If Not allValid Then
            errList = _unableToSearchText & vbCrLf & errList
            LaunchSearchError(errList)
            Return False
        Else
            Return True
        End If

    End Function

    Private Function ValidateControlsForNewItem() As Boolean

        Dim allValid As Boolean = True
        Dim errList As String = ""
        Dim err As String

        For Each sItem As IStlSearchFormItem In _searchItems
            err = sItem.ValidateForNewItem(_dataSqlConnection)
            If err <> "" Then
                allValid = False
                sItem.HiLite()
                errList &= vbCrLf & " - " & err
            End If
        Next

        If Not allValid Then
            errList = _unableToCreateNewText & vbCrLf & errList
            LaunchSearchError(errList)
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub CopyValuesToHiddenControls()
        'copio i dati da visible a hidden
        For Each sItem In _searchItems
            sItem.CopyValuesToHiddenControls()
        Next
    End Sub

    Private Sub LaunchSearchError(ByVal message As String)
        'lancio l'errore
        Dim formClientID As String
        Dim sScript As String

        formClientID = Me.ClientID

        sScript = "stl_searchform_error(" & _
        EncodeJsStringWithQuotes(formClientID) & ", " & _
        EncodeJsStringWithQuotes(message) & ");"
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, Me.ID & "_error", sScript, True)
    End Sub

#Region "Ricerca oggetti collegati"

    Private Sub FindChildrenStlGridViews()
        _ro_childrenStlGridViews.Clear()
        FindChildrenStlGridViewsInt(Me.Page)
    End Sub

    Private Sub FindChildrenStlGridViewsInt(ByVal rootControl As Control)
        Dim cSub As Control
        If TypeOf (rootControl) Is StlGridView Then
            If CType(rootControl, StlGridView).SqlStringProviderID = Me.ID Then
                _ro_childrenStlGridViews.Add(CType(rootControl, StlGridView))
            End If
        Else
            For Each cSub In rootControl.Controls
                FindChildrenStlGridViewsInt(cSub)
            Next
        End If
    End Sub

    <Browsable(False)> _
    Public ReadOnly Property ChildrenStlGridViews() As List(Of StlGridView)
        Get
            If Not _ro_childrenStlGridViewsSearched Then
                Me.FindChildrenStlGridViews()
                _ro_childrenStlGridViewsSearched = True
            End If
            Return _ro_childrenStlGridViews
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ContainingUpdatePanel() As UpdatePanel
        Get
            If Not _ro_containingUpdatePanelSearched Then
                _ro_containingUpdatePanel = FindContainingUpdatePanel()
                _ro_containingUpdatePanelSearched = True
            End If
            Return _ro_containingUpdatePanel
        End Get
    End Property

    Private Function FindContainingUpdatePanel() As UpdatePanel
        Dim c As Control
        Dim panel As Control = Nothing

        c = Me.Parent
        Do
            If TypeOf c Is UpdatePanel Then
                panel = c
                Exit Do
            End If
            c = c.Parent
        Loop Until c.Parent Is Nothing
        If TypeOf c Is UpdatePanel Then
            Return CType(c, UpdatePanel)
        Else
            Return Nothing
        End If

    End Function

#End Region

    Friend Function ReplaceHolders(ByVal InputVar As String) As String

        Dim OutputVar As String
        OutputVar = InputVar

        Dim parameter As KeyValuePair(Of Integer, String)
        For Each parameter In _searchParameters
            OutputVar = Replace(OutputVar, "{[(" & parameter.Key.ToString & ")]}", parameter.Value.Trim)
        Next

        Return OutputVar

    End Function

    Public Sub SetVisibleValue(ByVal NOMECAMP As String, ByVal value As String)

        Dim sItem As IStlSearchFormItem = _searchItems.FindByNOMECAMP(NOMECAMP)
        If sItem Is Nothing Then
            Throw New Exception("Invalid field name in SetVisibleValue")
        Else
            If Not TypeOf sItem Is DbSearchItem Then
                Throw New Exception("SetVisibleValue can only be called on DbSearchItem items")
            Else
                CType(sItem, DbSearchItem).SetVisibleValue(value)
            End If
        End If
    End Sub
End Class
