Imports System.Web.UI.WebControls.GridView
Imports System.ComponentModel
Imports System.Web.UI
Imports Softailor.Global.JSUtils

Public Class StlGridView
    Inherits System.Web.UI.WebControls.GridView

    'oggetti collegati

    Private _ro_boundStlFormViewID As String
    Private _ro_boundStlFormView As StlFormView
    Private _ro_boundStlFormViewSearched As Boolean = False

    Private _ro_parentStlGridViewID As String
    Private _ro_parentStlGridView As StlGridView
    Private _ro_parentStlGridViewSearched As Boolean = False

    Private _ro_boundStlSqlDataSource As StlSqlDataSource
    Private _ro_boundStlSqlDataSourceSearched As Boolean = False

    Private _ro_containingUpdatePanel As UpdatePanel
    Private _ro_containingUpdatePanelSearched As Boolean = False

    Private _ro_childrenStlGridViews As New List(Of StlGridView)
    Private _ro_childrenStlGridViewsSearched As Boolean = False

    Private _ro_sqlStringProviderID As String
    Private _ro_sqlStringProvider As ISqlStringProvider
    Private _ro_sqlStringProviderSearched As Boolean = False

    Public Event BeforeNewRow(ByVal sender As Object, ByVal e As CancelEventArgs)
    Public Event AfterNewRow(ByVal sender As Object, ByVal e As System.EventArgs)

    Private _gotoActive As Boolean = False
    Private _gotoField As String = ""
    Private _gotoValue As String = ""
    Private _gotoIndex As Integer = -1
    Private _gotoResyncBoundForm As Boolean = False
    Private _gotoResyncChildrenGrids As Boolean = False

    Protected _headerRow2 As GridViewRow
    Protected _footerRow2 As GridViewRow

    'variabili per proprietà
    Private _title As String = ""
    Private _itemDescriptionSingular As String = ""
    Private _itemDescriptionPlural As String = ""
    Private _AddCommandText As String = ""
    Private _DeleteConfirmationQuestion As String = ""
    Private _AllowReselectSelectedRow As Boolean = False
    Private _commandsInLastColumn As Boolean = False

    'per gestione numero di righe
    Private _rowCountLabel As Label

    'funzionalità
    Private _allowDelete As Boolean = True
    Private _allowInsert As Boolean = True
    Private _allowEdit As Boolean = True

    'controlli creati dinamicamente
    Private insertBtn As LinkButton

    Public Event RowSelected(ByVal dataKeyName As String, ByVal dataKeyValue As String)

#Region "Proprietà"

    <Category("Stl"), DefaultValue(False)> _
    Public Property CommandsInLastColumn() As Boolean
        Get
            Return _commandsInLastColumn
        End Get
        Set(ByVal value As Boolean)
            _commandsInLastColumn = value
        End Set
    End Property

    <Category("Stl"), DefaultValue(True)> _
    Public Property AllowDelete() As Boolean
        Get
            Return _allowDelete
        End Get
        Set(ByVal value As Boolean)
            _allowDelete = value
        End Set
    End Property

    <Category("Stl"), DefaultValue(True)> _
    Public Property AllowReselectSelectedRow() As Boolean
        Get
            Return _AllowReselectSelectedRow
        End Get
        Set(ByVal value As Boolean)
            _AllowReselectSelectedRow = value
        End Set
    End Property

    <Category("Stl"), DefaultValue(False)> _
    Public Property AllowInsert() As Boolean
        Get
            Return _allowInsert
        End Get
        Set(ByVal value As Boolean)
            _allowInsert = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property SqlStringProviderID() As String
        Get
            Return _ro_sqlStringProviderID
        End Get
        Set(ByVal value As String)
            _ro_sqlStringProviderID = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property ItemDescriptionSingular() As String
        Get
            Return _itemDescriptionSingular
        End Get
        Set(ByVal value As String)
            _itemDescriptionSingular = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property ItemDescriptionPlural() As String
        Get
            Return _itemDescriptionPlural
        End Get
        Set(ByVal value As String)
            _itemDescriptionPlural = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property AddCommandText() As String
        Get
            Return _AddCommandText
        End Get
        Set(ByVal value As String)
            _AddCommandText = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property DeleteConfirmationQuestion() As String
        Get
            Return _DeleteConfirmationQuestion
        End Get
        Set(ByVal value As String)
            _DeleteConfirmationQuestion = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property BoundStlFormViewID() As String
        Get
            Return _ro_boundStlFormViewID
        End Get
        Set(ByVal value As String)
            _ro_boundStlFormViewID = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property ParentStlGridViewID() As String
        Get
            Return _ro_parentStlGridViewID
        End Get
        Set(ByVal value As String)
            _ro_parentStlGridViewID = value
        End Set
    End Property

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
    Public Overrides ReadOnly Property FooterRow() As GridViewRow
        Get
            Dim f As GridViewRow = MyBase.FooterRow
            If f IsNot Nothing Then
                Return f
            Else
                Return _footerRow2
            End If
        End Get
    End Property

    <Browsable(False)> _
    Public Overrides ReadOnly Property HeaderRow() As GridViewRow
        Get
            Dim h As GridViewRow = MyBase.HeaderRow
            If h IsNot Nothing Then
                Return h
            Else
                Return _headerRow2
            End If
        End Get
    End Property

#End Region

    Public Sub PreGotoRow(ByVal gotoField As String, ByVal gotoValue As String, ByVal resyncBoundForm As Boolean, ByVal resyncChildrenGrids As Boolean)
        _gotoActive = True
        _gotoField = gotoField
        _gotoValue = gotoValue
        _gotoIndex = -1
        _gotoResyncBoundForm = resyncBoundForm
        _gotoResyncChildrenGrids = resyncChildrenGrids
    End Sub

    Public Sub PostGotoRow()
        If _gotoActive Then
            If _gotoIndex <> -1 Then
                'posizionamento su riga esistente
                Me.SelectedIndex = _gotoIndex
                _gotoIndex = -1
                IntBoundToExistingRow(False, FormViewMode.ReadOnly, False, "", _gotoResyncBoundForm, _gotoResyncChildrenGrids)
                'evento
                RaiseRowSelectedEvent()
            Else
                'posizionamento su indice vuoto: significa che è stato richiesto
                'il posizionamento su una riga ma questa non esiste più.
                'Pertanto devo segnalare quanto accaduto e inoltre riagganciare TUTTO.
                _gotoIndex = -1
                IntBoundToNewRow(False)
                'evento
                RaiseRowSelectedEvent()
                'messaggio
                LaunchError("A causa delle modifiche effettuate, l'elemento non rientra più nei criteri di ricerca impostati e pertanto non è più visualizzato.")
            End If

        End If
        _gotoActive = False
    End Sub

    Public Function GetAddNewJs() As String

        If insertBtn Is Nothing Then
            Return ""
        Else
            Return Me.Page.ClientScript.GetPostBackEventReference(insertBtn, "")
        End If

    End Function
    Private Function RowNumberDescription(ByVal rowNumber As Integer) As String
        Dim sOut As String

        If rowNumber = 1 Then
            If _itemDescriptionSingular = "" Then
                sOut = "1 riga"
            Else
                sOut = "1 " & _itemDescriptionSingular
            End If
        Else
            If _itemDescriptionPlural = "" Then
                sOut = rowNumber.ToString & " righe"
            Else
                sOut = rowNumber.ToString & " " & _itemDescriptionPlural
            End If
        End If
        If _title <> "" Then
            sOut = "(" & sOut & ")"
        End If
        Return sOut
    End Function

    Protected Overrides Function CreateChildControls(ByVal dataSource As System.Collections.IEnumerable, ByVal dataBinding As Boolean) As Integer

        'recupero il numero di righe
        Dim NumRows As Integer = MyBase.CreateChildControls(dataSource, dataBinding)

        If NumRows = 0 Then
            'create table
            Dim _table As New System.Web.UI.WebControls.Table
            _table.ID = Me.ID
            Dim fields() As System.Web.UI.WebControls.DataControlField = New System.Web.UI.WebControls.DataControlField(Me.Columns.Count - 1) {}
            Me.Columns.CopyTo(fields, 0)

            'convert the exisiting columns into an array and initialize
            If Me.ShowHeader Then
                _headerRow2 = _
                MyBase.CreateRow(-1, -1, _
                DataControlRowType.Header, _
                DataControlRowState.Normal)
                Me.InitializeRow(_headerRow2, fields)

                'classe
                Dim tot = 0
                If _allowEdit Then tot += 1
                If _allowDelete Then tot += 1
                If tot > 0 Then _headerRow2.Cells(0).CssClass = "cmd" & tot.ToString

                _table.Rows.Add(_headerRow2)
                'creazione TitleRow
                HandleHeaderRow(_headerRow2)
            End If

            'Create Empty Row
            Dim EmptyRow As New System.Web.UI.WebControls.GridViewRow(-1, -1, _
            System.Web.UI.WebControls.DataControlRowType.EmptyDataRow, _
            System.Web.UI.WebControls.DataControlRowState.Normal)

            Dim Cell As New System.Web.UI.WebControls.TableCell
            Cell.ColumnSpan = Me.Columns.Count
            If Not System.String.IsNullOrEmpty(EmptyDataText) Then
                Dim Lit As New System.Web.UI.WebControls.Literal
                Lit.Text = Me.EmptyDataText
                Cell.Controls.Add(Lit)
            End If

            If Not Me.EmptyDataTemplate Is Nothing Then
                Me.EmptyDataTemplate.InstantiateIn(Cell)
            End If

            EmptyRow.Cells.Add(Cell)
            _table.Rows.Add(EmptyRow)

            'Create footer row
            If Me.ShowFooter Then
                _footerRow2 = MyBase.CreateRow(-1, -1, _
            System.Web.UI.WebControls.DataControlRowType.Footer, _
            System.Web.UI.WebControls.DataControlRowState.Normal)
                Me.InitializeRow(_footerRow2, fields)
                _table.Rows.AddAt(0, _footerRow2)
            End If

            Me.Controls.Clear()
            Me.Controls.Add(_table)

        End If

        'creazione del DIV titolo e etichetta n. righe
        CreateTitleDiv()

        'scrivo il numero di righe
        If Not _rowCountLabel Is Nothing Then
            _rowCountLabel.Text = RowNumberDescription(NumRows)
        End If

        Return NumRows

    End Function

    Private Sub CreateTitleDiv()
        'creazione del DIV con i comandi

        Dim sTitle As String = ""
        Dim cmdDiv As HtmlControls.HtmlGenericControl
        Dim cmdTable As HtmlControls.HtmlTable
        Dim cmdRow As HtmlControls.HtmlTableRow
        Dim cmdCellLeft As HtmlControls.HtmlTableCell
        Dim cmdCellRight As HtmlControls.HtmlTableCell
        Dim createInsertBtn As Boolean
        Dim enableInsertBtn As Boolean
        Dim insertSpan As HtmlControls.HtmlGenericControl

        cmdDiv = New HtmlControls.HtmlGenericControl("div")
        cmdDiv.Attributes.Add("class", "stl_grd_hdr")
        cmdDiv.Attributes.Add("id", Me.ClientID & "_title")

        'creazione dei controlli all'interno
        cmdTable = New HtmlControls.HtmlTable
        cmdTable.CellPadding = 0
        cmdTable.CellSpacing = 0
        cmdTable.Border = 0

        cmdRow = New HtmlControls.HtmlTableRow

        cmdCellLeft = New HtmlControls.HtmlTableCell
        cmdCellLeft.Attributes.Add("class", "l")
        If _title <> "" Then
            'titolo
            cmdCellLeft.Controls.Add(New LiteralControl("<b>" & System.Web.HttpUtility.HtmlEncode(_title) & "</b> "))
        End If
        'numero di righe
        _rowCountLabel = New Label
        _rowCountLabel.EnableViewState = False
        _rowCountLabel.Text = RowNumberDescription(0)
        cmdCellLeft.Controls.Add(_rowCountLabel)

        cmdCellRight = New HtmlControls.HtmlTableCell
        cmdCellRight.Attributes.Add("class", "r")

        'bottone nuova riga: se ho un form E se il form ha allowedit E se ho allowinsert E se non ho un sqlprovider
        createInsertBtn = False
        enableInsertBtn = False
        If _allowInsert And _ro_boundStlFormViewID <> "" And _ro_sqlStringProviderID = "" Then
            If BoundStlFormView.AllowEdit Then
                createInsertBtn = True
                'il bottone insert c'è. Lo abilitiamo se:
                '- non ho un parent
                '- ho un parent che si trova su una riga esistente
                If _ro_parentStlGridViewID <> "" Then
                    If ParentStlGridView.SelectedIndex >= 0 Then enableInsertBtn = True
                Else
                    enableInsertBtn = True
                End If
            End If
        End If

        If createInsertBtn Then

            'creo insertBtn - enabled o disabled
            insertBtn = New LinkButton
            insertBtn.ID = "addNew"
            insertBtn.Enabled = enableInsertBtn
            insertBtn.Text = "."
            cmdCellRight.Controls.Add(insertBtn)
            AddHandler insertBtn.Click, AddressOf NewClick

            'creazione span
            insertSpan = New HtmlControls.HtmlGenericControl("span")
            If _AddCommandText = "" Then
                insertSpan.InnerText = "Aggiungi"
            Else
                insertSpan.InnerText = _AddCommandText
            End If
            If enableInsertBtn Then
                insertSpan.Attributes.Add("onclick", "stl_grd_add(" & EncodeJsStringWithQuotes(Me.ID) & ");")
                insertSpan.Attributes.Add("class", "addenabled")
            Else
                insertSpan.Attributes.Add("class", "adddisabled")
            End If
            cmdCellRight.Controls.Add(insertSpan)

        End If

        'composizione
        cmdRow.Cells.Add(cmdCellLeft)
        cmdRow.Cells.Add(cmdCellRight)
        cmdTable.Rows.Add(cmdRow)
        cmdDiv.Controls.Add(cmdTable)
        Me.Controls.Add(cmdDiv)
    End Sub

    Protected Sub NewClick(ByVal sender As Object, ByVal e As EventArgs)

        'se ho allowinsert=false, esco (misura di sicurezza)
        If Not _allowInsert Then Exit Sub

        'se ho un provider di stringa SQL, esco (misura di sicurezza)
        If _ro_sqlStringProviderID <> "" Then Exit Sub

        'prima di aggiungere il nuovo gestisco eventuali situazioni dirty
        If Not SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, True, True) Then Exit Sub

        Dim ce As New CancelEventArgs
        RaiseEvent BeforeNewRow(Me, ce)

        If Not ce.Cancel Then
            IntBoundToNewRow(True, FormViewMode.Insert)
            RaiseEvent AfterNewRow(Me, New EventArgs)
        End If
    End Sub

    Public Sub GotoEmptyRow()
        IntBoundToNewRow(False)

        'accodo uno scroll to top
        EnqueueScrollTop()

        'lancio l'evento
        RaiseRowSelectedEvent()

    End Sub

    Friend Sub GotoFirstOrEmptyRow()

        If Me.DataKeys.Count >= 1 Then
            'seleziono la prima riga
            Me.SelectedIndex = 0
            'riga esistente
            IntBoundToExistingRow(False, FormViewMode.ReadOnly, False, "", True, True)
        Else
            'riga vuota
            IntBoundToNewRow(False)
        End If

        'accodo uno scroll to top
        EnqueueScrollTop()

        'lancio evento
        RaiseRowSelectedEvent()
    End Sub

    Private Sub StlGridView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBound

        'se non ho un genitore E non siamo in postback, seleziono la prima riga o vado a riga vuota
        If Not Me.Page.IsPostBack And _ro_parentStlGridViewID = "" Then
            GotoFirstOrEmptyRow()
            'evento
            RaiseRowSelectedEvent()
        End If

    End Sub

    <Browsable(False)> _
    Public ReadOnly Property BoundStlFormView() As StlFormView
        Get
            If Not _ro_boundStlFormViewSearched Then
                _ro_boundStlFormView = FindBoundStlFormView()
                _ro_boundStlFormViewSearched = True
            End If
            Return _ro_boundStlFormView
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property SqlStringProvider() As ISqlStringProvider
        Get
            If Not _ro_sqlStringProviderSearched Then
                _ro_sqlStringProvider = FindSqlStringProvider()
                _ro_sqlStringProviderSearched = True
            End If
            Return _ro_sqlStringProvider
        End Get
    End Property


    <Browsable(False)> _
    Public ReadOnly Property BoundStlSqlDataSource() As StlSqlDataSource
        Get
            If Not _ro_boundStlSqlDataSourceSearched Then
                _ro_boundStlSqlDataSource = FindBoundStlSqlDataSource()
                _ro_boundStlSqlDataSourceSearched = True
            End If
            Return _ro_boundStlSqlDataSource
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
    <Browsable(False)> _
    Public ReadOnly Property ParentStlGridView() As StlGridView
        Get
            If Not _ro_parentStlGridViewSearched Then
                _ro_parentStlGridView = FindParentStlGridView()
                _ro_parentStlGridViewSearched = True
            End If
            Return _ro_parentStlGridView
        End Get
    End Property

    Public Sub UpdateParentPanel()
        'se esiste un updatepanel, lo aggiorna
        If Not Me.ContainingUpdatePanel Is Nothing Then
            If Me.ContainingUpdatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional Then
                Me.ContainingUpdatePanel.Update()
            End If
        End If

    End Sub

    Private Sub StlGridView_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'rimozione viewstate
        Me.EnableViewState = False

        'click su riga selezionata
        If _AllowReselectSelectedRow Then
            Me.SelectedRowStyle.CssClass = "src"
        End If

    End Sub

    Private Sub StlGridView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.GridLines = WebControls.GridLines.None
        Me.CellSpacing = -1
        Me.CellPadding = -1

        If Me.Page Is Nothing Then Exit Sub

        'determino _allowEdit: si ha true se ho un form collegato per il quale allowedit=true
        _allowEdit = False
        If _ro_boundStlFormViewID <> "" Then
            _allowEdit = BoundStlFormView.AllowEdit
        End If

        'sostituzione colonna comandi
        Dim srcIdx As Integer
        Dim colIdx As Integer = -1
        For srcIdx = 0 To Columns.Count - 1
            If TypeOf Columns(srcIdx) Is CommandField Then
                colIdx = srcIdx
                Exit For
            End If
        Next
        If colIdx <> -1 Then
            'rimuovo vecchia
            Me.Columns.RemoveAt(colIdx)

            'aggiungo la nuova colonna SOLO se ho allowDelete OR allowEdit
            If _allowEdit Or _allowDelete Then
                'aggiungo nuova
                Dim stlCommandField As New StlCommandField
                With stlCommandField
                    .ButtonType = ButtonType.Link
                    .DeleteText = "<img border=""0"" src=""" & Me.Page.ResolveClientUrl("~/img/icoDelete.gif") & """ />"
                    .EditText = "<img border=""0"" src=""" & Me.Page.ResolveClientUrl("~/img/icoEdit.gif") & """ />"
                    'il bottone delete viene visualizzato se ho allowdelete=True
                    .ShowDeleteButton = _allowDelete
                    'il bottone edit viene visualizzato se ho un form collegato, se il form ha allowedit=true
                    .ShowEditButton = _allowEdit
                End With
                Me.Columns.Insert(colIdx, stlCommandField)
            End If
        End If

    End Sub

    Private Sub StlGridView_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If Me.Page Is Nothing Then Exit Sub
        If Me.Page.IsPostBack Then Exit Sub

        If Not Me.Page.ClientScript.IsClientScriptBlockRegistered(Me.Page.GetType, "stl_grid_routing") Then
            'aggiunta del routing
            Dim clientGridScripts = Softailor.Web.UI.DbForms.StlGridViewHelpers.GetClientGridScripts(Me.Page, Me.Page.ClientScript)
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "stl_grid_routing", clientGridScripts, True)

            Softailor.Web.UI.DbForms.StlGridViewHelpers.AdjustTabContainers(Me.Page)

        End If

    End Sub

    Private Sub StlGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowCreated

        If _gotoField <> "" Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                With CType(e.Row.DataItem, DataRowView)
                    Try
                        If .Item(_gotoField).ToString = _gotoValue Then
                            _gotoField = ""
                            _gotoValue = ""
                            _gotoIndex = e.Row.RowIndex
                        End If
                    Catch ex As Exception
                    End Try
                End With
            End If
        End If

        'classe per cella comandi (sia in header sia in corpo)
        Dim tot = 0
        If _allowEdit Then tot += 1
        If _allowDelete Then tot += 1
        If tot > 0 Then e.Row.Cells(0).CssClass = "cmd" & tot.ToString

    End Sub

    Private Sub StlGridView_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowDataBound

        'creazione dell'header
        If e.Row.RowType = DataControlRowType.Header Then
            HandleHeaderRow(e.Row)
        End If

        'selezione mediante click su riga
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("id", Me.ID & "$" & e.Row.RowIndex.ToString)
            If _commandsInLastColumn Then
                e.Row.Cells(e.Row.Cells.Count - 1).CssClass = "lastcmd"
            End If

        End If
    End Sub

    Private Sub HandleHeaderRow(ByVal hdrRow As GridViewRow)

        'creazione della prima riga fittizia (viene poi sovrascritta dal DIV con l'header)
        Dim row As New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)
        row.CssClass = "ph"

        For i As Integer = 0 To Me.Columns.Count - 1
            Dim cell As New TableHeaderCell
            cell.Text = "&nbsp;"
            If i = 0 Then cell.CssClass = "cmd"
            row.Cells.Add(cell)
        Next
        hdrRow.Parent.Controls.AddAt(0, row)

    End Sub

    Private Sub StlGridView_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles Me.RowDeleted

        'modifica in data 12/11: dopo cancellazione NON SI VA PIU' all'eventuale record successivo
        'ma sempre al record vuoto in quanto questo creava enormi problemi con i controlparameters

        If Not e.Exception Is Nothing Then
            LaunchDeleteError(e.Exception.Message)
            e.ExceptionHandled = True
        Else
            'prima andiamo al record vuoto
            Me.SelectedIndex = -1
            'Me.DataBind()
            IntBoundToNewRow(False)
            'aggiorno
            Me.UpdateParentPanel()
        End If

    End Sub

    Private Sub StlGridView_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Me.RowEditing

        'se viene richiesto l'editing...

        'cancello comunque l'evento
        e.Cancel = True

        'esco se non ho un form o il form ha _allowEdit=false (misura di sicurezza)
        If _ro_boundStlFormViewID = "" Then
            Exit Sub
        Else
            If Not BoundStlFormView.AllowEdit Then Exit Sub
        End If


        'salvo se dirty...
        If SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, True, True) Then
            Me.SelectedIndex = e.NewEditIndex
            IntBoundToExistingRow(True, FormViewMode.Edit, False, "", True, True)
        End If
    End Sub

    Private Sub StlGridView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SelectedIndexChanged

        'cambio riga standard, clic su riga
        IntBoundToExistingRow(False, FormViewMode.ReadOnly, False, "", True, True)

        'lancio l'evento
        RaiseRowSelectedEvent()

    End Sub

    Private Sub RaiseRowSelectedEvent()
        If Me.SelectedIndex = -1 Then
            RaiseEvent RowSelected(Me.DataKeyNames(0), "")
        Else
            RaiseEvent RowSelected(Me.DataKeyNames(0), Me.SelectedDataKey.Value.ToString)
        End If
    End Sub
    Private Sub IntBoundToExistingRow(ByVal setMode As Boolean, ByVal newMode As FormViewMode, ByVal forceKey As Boolean, ByVal forcedkeyValue As String, ByVal reSyncBoundForm As Boolean, ByVal reSyncChildrenGrids As Boolean)

        Dim childGrid As StlGridView

        'aggancio del form
        If reSyncBoundForm Then
            If Not Me.BoundStlFormView Is Nothing Then
                If forceKey Then
                    Me.BoundStlFormView.BoundStlSqlDataSource.SelectParameters(Me.BoundStlFormView.DataKeyNames(0)).DefaultValue = _
                    forcedkeyValue
                Else
                    Me.BoundStlFormView.BoundStlSqlDataSource.SelectParameters(Me.BoundStlFormView.DataKeyNames(0)).DefaultValue = _
                    Me.SelectedDataKey.Value.ToString()
                End If
                BoundStlFormView.DataBind() 'aggiunto
                If setMode Then
                    Me.BoundStlFormView.ChangeMode(newMode)
                End If

                Me.BoundStlFormView.UpdateParentPanel()
            End If
        End If

        'aggancio dei figli al mio ID o a nothing
        If reSyncChildrenGrids Then
            For Each childGrid In Me.ChildrenStlGridViews
                If forceKey Then
                    childGrid.SyncToParentKey(Me.DataKeyNames(0), forcedkeyValue)
                Else
                    childGrid.SyncToParentKey(Me.DataKeyNames(0), Me.SelectedDataKey.Value.ToString)
                End If
            Next
        End If

    End Sub

    Private Sub IntBoundToNewRow(ByVal setMode As Boolean, Optional ByVal newMode As FormViewMode = FormViewMode.ReadOnly)

        'io ho riga vuota
        Me.SelectedIndex = -1

        'aggancio del form
        If Not Me.BoundStlFormView Is Nothing Then

            If setMode Then
                Me.BoundStlFormView.ChangeMode(newMode)
            End If
            Me.BoundStlFormView.BoundStlSqlDataSource.SelectParameters(Me.BoundStlFormView.DataKeyNames(0)).DefaultValue = ""
            BoundStlFormView.DataBind()
            Me.BoundStlFormView.UpdateParentPanel()
        End If

        'porto a vuoti tutti i figli
        For Each childGrid In Me.ChildrenStlGridViews
            childGrid.SyncToParentKey(Me.DataKeyNames(0), "")
        Next
    End Sub

    Friend Sub SyncToParentKey(ByVal keyName As String, ByVal keyValue As String)
        'chiamata dal parent su selezione riga
        'se il parent è su riga nuova ricevo keyValue=""
        'se il parent è su una riga ricevo keyValue<>""

        'rifiltro il contenuto
        Me.BoundStlSqlDataSource.SelectParameters(keyName).DefaultValue = keyValue
        'nessuna riga selezionata
        Me.SelectedIndex = -1
        'aggancio ed aggiorno
        Me.DataBind()
        Me.GotoFirstOrEmptyRow()
        Me.UpdateParentPanel()

    End Sub

    Private Sub EnqueueScrollTop()

        'il comando non ha senso alla prima apertura
        If Me.Page Is Nothing Then Exit Sub
        If Not Me.Page.IsPostBack Then Exit Sub

        Dim sScript As String
        sScript = "stl_grd_clientscrolltop('" & Me.ID & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, Me.ID & "_totop", sScript, True)
    End Sub

#Region "Ricerca controlli collegati"

    Private Function FindBoundStlSqlDataSource() As StlSqlDataSource
        Return CType(Softailor.Global.WebUIUtils.FindControl(Me.Page, Me.DataSourceID), StlSqlDataSource)
    End Function

    Private Function FindSqlStringProvider() As ISqlStringProvider
        Return CType(Softailor.Global.WebUIUtils.FindControl(Me.Page, Me.SqlStringProviderID), ISqlStringProvider)
    End Function

    Private Function FindBoundStlFormView() As StlFormView
        If _ro_boundStlFormViewID = "" Then
            Return Nothing
        Else
            Return CType(Softailor.Global.WebUIUtils.FindControl(Me.Page, _ro_boundStlFormViewID), StlFormView)
        End If
    End Function

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

    Private Function FindParentStlGridView() As StlGridView
        If _ro_parentStlGridViewID = "" Then
            Return Nothing
        Else
            Return CType(Softailor.Global.WebUIUtils.FindControl(Me.Page, _ro_parentStlGridViewID), StlGridView)
        End If
    End Function


    Private Sub FindChildrenStlGridViews()
        _ro_childrenStlGridViews.Clear()
        FindChildrenStlGridViewsInt(Me.Page)
    End Sub

    Private Sub FindChildrenStlGridViewsInt(ByVal rootControl As Control)

        Dim cSub As Control

        If TypeOf (rootControl) Is StlGridView Then
            If CType(rootControl, StlGridView).ParentStlGridViewID = Me.ID Then
                _ro_childrenStlGridViews.Add(CType(rootControl, StlGridView))
            End If
        Else
            For Each cSub In rootControl.Controls
                FindChildrenStlGridViewsInt(cSub)
            Next
        End If

    End Sub
#End Region

    Private Sub StlGridView_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles Me.SelectedIndexChanging

        'prima di cambiare riga salvo tutto TRANNE IL PARENT
        If Not SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, False, True) Then e.Cancel = True

    End Sub

    Friend Function SaveMyForm_ParentForms_ChildrenFormsIfDirty(ByVal saveMyForm As Boolean, ByVal saveParentForms As Boolean, ByVal saveChildrenForms As Boolean) As Boolean

        Dim childGrid As StlGridView


        'salvataggio - se dirty - dei form parent, del mio form e dei form children

        Dim result As Boolean = True    'parto con true e metto in AND tutti i result

        'parte 1: parent
        If saveParentForms Then
            If _ro_parentStlGridViewID <> "" Then
                result = result And Me.ParentStlGridView.SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, True, False)
            End If
        End If

        'parte 2: me stesso
        If saveMyForm Then
            If Me.BoundStlFormViewID <> "" Then
                With Me.BoundStlFormView
                    Select Case .CurrentMode
                        Case FormViewMode.Edit
                            .UpdateItem(False)
                            'se siamo ancora in edit il salvataggio non è riuscito
                            If .CurrentMode = FormViewMode.Edit Then result = False 'non faccio OR perchè tanto x or false=False
                            'aggiorno il pannello per riflettere l'errore o il salvataggio ok
                            .UpdateParentPanel()
                        Case FormViewMode.Insert
                            .InsertItem(False)
                            'se siamo ancora in edit il salvataggio non è riuscito
                            If .CurrentMode = FormViewMode.Insert Then result = False 'non faccio OR perchè tanto x or false=False
                            'aggiorno il pannello per riflettere l'errore o il salvataggio ok
                            .UpdateParentPanel()
                    End Select
                End With
            End If
        End If

        'parte 3: children
        If saveChildrenForms Then
            'ora faccio ricorsivamente la stessa cosa sui children
            For Each childGrid In ChildrenStlGridViews
                result = result And childGrid.SaveMyForm_ParentForms_ChildrenFormsIfDirty(True, False, True)
            Next

        End If

        Return result

    End Function

    Private Sub LaunchDeleteError(ByVal exceptionMessage As String)

        'lancio l'errore
        Dim formClientID As String
        Dim sScript As String
        Dim sMessage As String

        formClientID = Me.ClientID
        sMessage = "Si è verificato un errore durante l'eliminazione dell'elemento." & vbCrLf & _
        "Possibili cause:" & vbCrLf & _
        " - L'elemento è dotato di elementi ""figli"", che vanno preventivamente eliminati" & vbCrLf & _
        " - L'elemento è in uso in altre parti del software"

        sScript = "stl_gridview_error(" & _
        EncodeJsStringWithQuotes(formClientID) & ", " & _
        EncodeJsStringWithQuotes(sMessage) & ", " & _
        EncodeJsStringWithQuotes(exceptionMessage) & ");"

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ID & "_error", sScript, True)
    End Sub

    Private Sub LaunchError(ByVal errorMessage As String)

        'lancio l'errore
        Dim formClientID As String
        Dim sScript As String

        formClientID = Me.ClientID

        sScript = "stl_searchform_error(" & _
        EncodeJsStringWithQuotes(formClientID) & ", " & _
        EncodeJsStringWithQuotes(errorMessage) & ");"

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ID & "_error", sScript, True)
    End Sub

    Public Sub EnsureSelectedRowVisible()

        Dim sScript As String

        sScript = "stl_grd_clientensurevisible(" & _
        EncodeJsStringWithQuotes(Me.ID) & ");"

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ID & "_ev", sScript, True)
    End Sub

    Public ReadOnly Property SelectedValueOrZero() As Object
        Get
            If Me.SelectedIndex = -1 Then
                Return 0
            Else
                Return Me.SelectedValue
            End If
        End Get
    End Property
End Class


