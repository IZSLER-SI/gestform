Imports System.Web.UI
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Softailor.Global.JSUtils
Imports System.Web

Public Class StlFormView
    Inherits System.Web.UI.WebControls.FormView

    Private _ro_boundStlSqlDataSource As StlSqlDataSource
    Private _ro_boundStlSqlDataSourceSearched As Boolean = False

    Private _ro_parentUpdatePanel As UpdatePanel
    Private _ro_parentUpdatePanelSearched As Boolean = False

    Private _ro_boundStlGridViewID As String
    Private _ro_boundStlGridView As StlGridView
    Private _ro_boundStlGridViewSearched As Boolean = False

    Private _newItemText As String = ""
    Private _allowEdit As Boolean = True

    Private Shared ReadOnly REGEX_BETWEEN_TAGS As New Regex(">\s+<", RegexOptions.Compiled)
    Private Shared ReadOnly REGEX_LINE_BREAKS_BETWEEN_TAGS As New Regex(">\n\s+<", RegexOptions.Compiled)

    Public Shared Function SomeDirtyOnPage(p As Page) As Boolean

        Return SomeDirtyOnPageRecursive(p)

    End Function

    Private Shared Function SomeDirtyOnPageRecursive(c As Control) As Boolean

        Dim retValue = False

        If TypeOf c Is StlFormView Then
            If CType(c, FormView).CurrentMode = FormViewMode.Edit Or CType(c, FormView).CurrentMode = FormViewMode.Insert Then
                retValue = True
            End If
        Else
            For Each cSub As Control In c.Controls
                retValue = retValue Or SomeDirtyOnPageRecursive(cSub)
                If retValue Then Exit For
            Next
        End If

        Return retValue

    End Function

    <Category("Stl"), DefaultValue(True)> _
    Public Property AllowEdit() As Boolean
        Get
            Return _allowEdit
        End Get
        Set(ByVal value As Boolean)
            _allowEdit = value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property BoundStlGridView() As StlGridView
        Get
            If Not _ro_boundStlGridViewSearched Then
                _ro_boundStlGridView = FindBoundStlGridView()
                _ro_boundStlGridViewSearched = True
            End If
            Return _ro_boundStlGridView
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
    Public ReadOnly Property ParentUpdatePanel() As UpdatePanel
        Get
            If Not _ro_parentUpdatePanelSearched Then
                _ro_parentUpdatePanel = FindParentUpdatePanel()
                _ro_parentUpdatePanelSearched = True
            End If
            Return _ro_parentUpdatePanel
        End Get
    End Property

    <Category("Stl")> _
    Public Property BoundStlGridViewID() As String
        Get
            Return _ro_boundStlGridViewID
        End Get
        Set(ByVal value As String)
            _ro_boundStlGridViewID = value
        End Set
    End Property

    <Category("Stl")> _
    Public Property NewItemText() As String
        Get
            Return _newItemText
        End Get
        Set(ByVal value As String)
            _newItemText = value
        End Set
    End Property
    Public Sub UpdateParentPanel()
        'se esiste un updatepanel, lo aggiorna
        If Not Me.ParentUpdatePanel Is Nothing Then
            If Me.ParentUpdatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional Then
                Me.ParentUpdatePanel.Update()
            End If
        End If
    End Sub

    Private Sub SetHdrButton(ByVal btn As Button, ByVal visible As Boolean, ByVal enabled As Boolean)
        btn.Visible = visible
        btn.Enabled = enabled
        If enabled Then btn.CssClass = "btne" Else btn.CssClass = "btnd"
    End Sub

    Private Sub StlFormView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBound

        Dim disable As Boolean
        Dim clear As Boolean
        Dim _allowInsert As Boolean
        Dim disableKey As Boolean
        Dim keyFieldName As String

        'gestione lunghezze massime e focus sul primo controllo
        If Me.CurrentMode = FormViewMode.Edit Or Me.CurrentMode = FormViewMode.Insert Then
            'gestione maxLens
            Dim maxLens As Dictionary(Of String, Integer) = Me.BoundStlSqlDataSource.StringFieldsMaxLens
            If Not maxLens Is Nothing Then
                AssignMaxLens(Me, maxLens)
            End If
            Me.FocusFirstControl()
        End If

        'determino se devo disattivare il campo ID: lo devo fare se sono in edit e se non ho autoincrement
        disableKey = False
        keyFieldName = ""

        If CurrentMode = FormViewMode.Edit Then
            If Not Me.BoundStlSqlDataSource.HasIdentity Then
                disableKey = True
                keyFieldName = Me.DataKeyNames(0)
            End If
        End If

        'gestione stili e disattivazione controlli
        disable = (Me.CurrentMode = FormViewMode.ReadOnly)
        clear = Me.DataKey.Value Is Nothing
        GestisciControlli(Me, disable, clear, disableKey, keyFieldName)

        'gestione bottoni header
        Try
            Dim newButton As Button = CType(Me.FindControl("NewButton"), Button)
            Dim editButton As Button = CType(Me.FindControl("EditButton"), Button)
            Dim insertButton As Button = CType(Me.FindControl("InsertButton"), Button)
            Dim insertCancelButton As Button = CType(Me.FindControl("InsertCancelButton"), Button)
            Dim updateButton As Button = CType(Me.FindControl("UpdateButton"), Button)
            Dim updateCancelButton As Button = CType(Me.FindControl("UpdateCancelButton"), Button)
            Dim statusLabel As Label = CType(Me.FindControl("StatusLabel"), Label)
            Select Case Me.CurrentMode
                Case FormViewMode.Edit

                    'sono qui SOLO se _allowedit

                    'new - insert - insertcancel: nascosti
                    SetHdrButton(newButton, False, False)
                    SetHdrButton(insertButton, False, False)
                    SetHdrButton(insertCancelButton, False, False)

                    'edit - update - updatecancel: visibili, attivi 2 e 3
                    SetHdrButton(editButton, True, False)
                    SetHdrButton(updateButton, True, True)
                    SetHdrButton(updateCancelButton, True, True)

                    'etichetta
                    statusLabel.Text = ""

                Case FormViewMode.Insert

                    'sono qui SOLO se boundGrid.allowInsert

                    'new - insert - insertcancel: visibili, attivi 2 e 3
                    SetHdrButton(newButton, True, False)
                    SetHdrButton(insertButton, True, True)
                    SetHdrButton(insertCancelButton, True, True)

                    'edit - update - updatecancel: invisibili
                    SetHdrButton(editButton, False, False)
                    SetHdrButton(updateButton, False, False)
                    SetHdrButton(updateCancelButton, False, False)

                    'etichetta
                    If _newItemText = "" Then
                        statusLabel.Text = "nuova riga"
                    Else
                        statusLabel.Text = _newItemText
                    End If

                Case FormViewMode.ReadOnly

                    'dobbiamo capire se siamo agganciati al "nulla" o a qualcosa
                    If IsNewRecordReadOnly() Then

                        'siamo al nuovo record, read only

                        'determino _allowInsert: ho una grid, la grid ha allowinsert, ho allowedit, la griglia non ha un provider SQL
                        _allowInsert = False
                        If _allowEdit And _ro_boundStlGridViewID <> "" Then
                            If BoundStlGridView.AllowInsert And BoundStlGridView.SqlStringProviderID = "" Then _allowInsert = True
                        End If

                        'edit - update - updatecancel: invisibili in ogni caso
                        SetHdrButton(editButton, False, False)
                        SetHdrButton(updateButton, False, False)
                        SetHdrButton(updateCancelButton, False, False)

                        If _allowInsert Then

                            'è possibile fare un inserimento

                            'A MENO CHE io abbia una boundgrid, che ha un parent, che ha selectedindex=-1

                            Dim parentNewRow As Boolean = False

                            If _ro_boundStlGridViewID <> "" Then
                                If BoundStlGridView.ParentStlGridViewID <> "" Then
                                    If BoundStlGridView.ParentStlGridView.SelectedIndex = -1 Then
                                        parentNewRow = True
                                    End If
                                End If
                            End If

                            'a seconda di parentNewRow...
                            If parentNewRow Then
                                'new - insert - insertcancel: visibili, disattivati
                                SetHdrButton(newButton, True, False)
                                SetHdrButton(insertButton, True, False)
                                SetHdrButton(insertCancelButton, True, False)

                                'etichetta
                                statusLabel.Text = ""

                            Else
                                'new - insert - insertcancel: visibili, attivo 1
                                SetHdrButton(newButton, True, True)
                                SetHdrButton(insertButton, True, False)
                                SetHdrButton(insertCancelButton, True, False)

                                'etichetta
                                If _newItemText = "" Then
                                    statusLabel.Text = "nuova riga"
                                Else
                                    statusLabel.Text = _newItemText
                                End If

                            End If

                        Else

                            'non è possibile fare un inserimento -> è come se fossi su "record inesistente"

                            'se sono editabile, mostro i pulsanti disattivati; se non sono editabile, non mostro nulla
                            If _allowEdit Then
                                SetHdrButton(newButton, True, False)
                                SetHdrButton(insertButton, True, False)
                                SetHdrButton(insertCancelButton, True, False)
                            Else
                                SetHdrButton(newButton, False, False)
                                SetHdrButton(insertButton, False, False)
                                SetHdrButton(insertCancelButton, False, False)
                            End If

                            'etichetta
                            statusLabel.Text = ""
                        End If
                    Else
                        'siamo su record esistente

                        'new - insert - insertcancel: invisibili
                        SetHdrButton(newButton, False, False)
                        SetHdrButton(insertButton, False, False)
                        SetHdrButton(insertCancelButton, False, False)


                        If _allowEdit Then
                            'edit - update - updatecancel: visibili, attivo 1
                            SetHdrButton(editButton, True, True)
                            SetHdrButton(updateButton, True, False)
                            SetHdrButton(updateCancelButton, True, False)
                        Else
                            'edit - update - updatecancel: invisibili
                            SetHdrButton(editButton, False, False)
                            SetHdrButton(updateButton, False, False)
                            SetHdrButton(updateCancelButton, False, False)
                        End If
                        'etichetta
                        statusLabel.Text = ""
                    End If


            End Select
        Catch ex As Exception

        End Try

    End Sub

    Private Function IsNewRecordReadOnly() As Boolean
        'determina se ci troviamo sul nuovo record in modalità read only
        If Me.CurrentMode = FormViewMode.ReadOnly Then
            With Me.BoundStlSqlDataSource
                Return .SelectParameters(Me.DataKeyNames(0)).DefaultValue = ""
            End With
        Else
            Return False
        End If
    End Function

    Private Sub GestisciElementiNullDropDown(ByVal c As Control)

        Dim cSub As System.Web.UI.Control

        If TypeOf c Is DropDownList Then
            With CType(c, DropDownList)
                If .ID.ToUpper Like "*DROPDOWNLIST" Then
                    'creo l'item vuoto
                    .AppendDataBoundItems = True
                    .Items.Insert(0, "")
                End If
            End With
        End If

        For Each cSub In c.Controls
            GestisciElementiNullDropDown(cSub)
        Next

    End Sub

    Private Sub GestisciControlli(ByVal c As Control, ByVal disable As Boolean, ByVal clear As Boolean, ByVal disableKey As Boolean, ByVal keyFieldName As String)
        Dim cSub As System.Web.UI.Control

        If TypeOf c Is TextBox Then
            With CType(c, TextBox)
                If disable Then .Enabled = False
                If .ID.ToUpper Like "*TEXTBOX" Then
                    If .CssClass = "" Then
                        .CssClass = "txt"
                    Else
                        .CssClass = "txt " & .CssClass
                    End If

                    If clear Then .Text = ""
                End If
                'disabilito se è la key
                If disableKey Then
                    If .ID.ToUpper = keyFieldName.ToUpper & "TEXTBOX" Then .Enabled = False
                End If
            End With
        ElseIf TypeOf c Is DropDownList Then
            With CType(c, DropDownList)
                'se questo controllo è associato a una lista
                'e manca l'entry vuota, lancio un eccezione
                'così mi accorgo subito di aver fatto una cagata

                If .DataSourceID <> "" Then
                    'If .Items(0).Value <> "" Then Throw New Exception("Missing Empty Item in control " & c.ID & " of " & Me.ID)
                    If Not .AppendDataBoundItems Then Throw New Exception("Missing AppendDataBoundItems in control " & c.ID & " of " & Me.ID)
                End If

                If disable Then .Enabled = False
                If .ID.ToUpper Like "*DROPDOWNLIST" Then
                    .CssClass = "ddn"
                    If clear Then .SelectedIndex = -1
                End If
            End With
        ElseIf TypeOf c Is CheckBox Then
            With CType(c, CheckBox)
                If disable Then .Enabled = False
                If .ID.ToUpper Like "*CHECKBOX" Then
                    .CssClass = "chk"
                    If clear Then .Checked = False
                End If
            End With
        ElseIf TypeOf c Is StlFormViewLinkButton Then
            With CType(c, StlFormViewLinkButton)
                If disable Then .Enabled = False
            End With
        ElseIf TypeOf c Is StlFormViewHyperLink Then
            With CType(c, StlFormViewHyperLink)
                If disable Then .Enabled = False
            End With
        ElseIf TypeOf c Is ICustomControl Then
            'gestione custom controls
            With CType(c, ICustomControl)
                If disable Then .SetDisabled() Else .SetEnabled()
                If clear Then .Value = ""
                'disabilito se è la key
                If disableKey Then
                    If .FieldName.ToUpper = keyFieldName.ToUpper Then .SetDisabled()
                End If
            End With
        End If

        'scendo all'interno ovunque a patto che non siamo in un iCustomControl
        If Not TypeOf c Is ICustomControl Then
            For Each cSub In c.Controls
                GestisciControlli(cSub, disable, clear, disableKey, keyFieldName)
            Next
        End If

    End Sub

    Private Sub StlFormView_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Me.DesignMode Then Exit Sub

        If Not Me.DesignMode Then
            Me.InsertItemTemplate = Me.EditItemTemplate
            Me.ItemTemplate = Me.EditItemTemplate
            Me.HeaderTemplate = New StlFormViewHeaderTemplate
            Me.EmptyDataTemplate = New StlFormViewEmptyTemplate(Me.HeaderTemplate, Me.EditItemTemplate)
        End If
    End Sub

    Private Sub StlFormView_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ItemCreated
        'dobbiamo mettere una classe (b) alla TD centrale.
        'struttura:
        '   formview.controls(0) = ChildTable
        '   quando la childtable contiene 3 o più oggetti, quello centrale è una FormViewRow
        'mettiamo tutto in un try... al limite non va.
        Try
            If Me.Controls(0).Controls.Count >= 3 Then
                Dim row As FormViewRow = CType(Me.Controls(0).Controls(1), FormViewRow)
                row.Cells(0).CssClass = "b"
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub StlFormView_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertedEventArgs) Handles Me.ItemInserted
        If Not e.Exception Is Nothing Then
            LaunchSaveError(e.Exception.Message)

            e.ExceptionHandled = True
            e.KeepInInsertMode = True
            'il rebind viene fatto dal sqldatasource (solo se non ci sono eccezioni da DB)
        End If
    End Sub

    Private Sub StlFormView_ItemInserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertEventArgs) Handles Me.ItemInserting

        Dim _allowInsert As Boolean = False

        'determino _allowInsert: sempre true a meno che la grid non abbia allowinsert
        _allowInsert = True
        If _ro_boundStlGridViewID <> "" Then
            If Not BoundStlGridView.AllowInsert Then _allowInsert = False
        End If

        'se non ho allowinsert, blocco
        If Not _allowInsert Then
            e.Cancel = True
            Exit Sub
        End If

        'faccio il rtrim di tutte le stringhe ricevute
        Dim i As Integer
        For i = 0 To e.Values.Count - 1
            e.Values(i) = e.Values(i).ToString.TrimEnd
        Next

        'validazione ed evidenziazione controlli "difettosi"
        If Not autoValidate(e.Values) Then
            e.Cancel = True
            Exit Sub
        End If

        'se la mia griglia è figlia di un'altra griglia
        'mi auto-imposto i valori del campo che mi collega al parent
        If _ro_boundStlGridViewID <> "" Then
            If Me.BoundStlGridView.ParentStlGridViewID <> "" Then
                Me.BoundStlSqlDataSource.InsertParameters(Me.BoundStlGridView.ParentStlGridView.DataKeyNames(0)).DefaultValue = Me.BoundStlGridView.ParentStlGridView.SelectedDataKey.Value.ToString
            End If
        End If
    End Sub

    Private Sub LaunchSaveError(ByVal exceptionMessage As String)
        'lancio l'errore
        Dim formClientID As String
        Dim sScript As String
        Dim sMessage As String

        formClientID = Me.ClientID
        sMessage = "Si è verificato un errore nel salvataggio dei dati del riquadro evidenziato." & vbCrLf & _
        "Possibili cause:" & vbCrLf & _
        " - uno o più campi non contengono dati obbligatori" & vbCrLf & _
        " - uno o più campi contengono dati scritti scorrettamente" & vbCrLf & _
        " - uno o più campi contengono valori duplicati"

        sScript = "stl_formview_error(" & _
        EncodeJsStringWithQuotes(formClientID) & ", " & _
        EncodeJsStringWithQuotes(sMessage) & ", " & _
        EncodeJsStringWithQuotes(exceptionMessage) & ");"

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ID & "_error", sScript, True)
    End Sub

    Private Sub LaunchPreSaveError()
        'lancio l'errore
        Dim formClientID As String
        Dim sScript As String
        Dim sMessage As String

        formClientID = Me.ClientID
        sMessage = "I campi evidenziati in giallo contengono errori. Impossibile salvare." & vbCrLf & _
        "Passa con il mouse sui campi evidenziati per visualizzare il tipo di errore."

        sScript = "stl_formview_presaveerror(" & _
        EncodeJsStringWithQuotes(formClientID) & ", " & _
        EncodeJsStringWithQuotes(sMessage) & ");"

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, Me.ID & "_error", sScript, True)
    End Sub

    Private Sub StlFormView_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdatedEventArgs) Handles Me.ItemUpdated

        If Not e.Exception Is Nothing Then
            LaunchSaveError(e.Exception.Message)

            e.ExceptionHandled = True
            e.KeepInEditMode = True
        Else
            'rivado alla riga giusta nel caso in cui l'ordinamento sia cambiato
            If Not Me.BoundStlGridView Is Nothing Then
                'non risincronizzo il form e/o i figli perchè tanto sono sempre loro
                Me.BoundStlGridView.PreGotoRow(Me.BoundStlGridView.DataKeyNames(0), Me.DataKey.Value.ToString, False, False)
                Me.BoundStlGridView.DataBind()
                Me.BoundStlGridView.PostGotoRow()
                Me.BoundStlGridView.UpdateParentPanel()
                If Me.BoundStlGridView.ContainingUpdatePanel IsNot Nothing Then
                    Me.BoundStlGridView.EnsureSelectedRowVisible()
                End If
            End If
        End If

    End Sub

    Private Sub StlFormView_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdateEventArgs) Handles Me.ItemUpdating

        'se non ho allowedit, cancello (misura di sicurezza)
        If Not _allowEdit Then
            e.Cancel = True
            Exit Sub
        End If

        'faccio il rtrim di tutte le stringhe ricevute
        Dim i As Integer
        For i = 0 To e.NewValues.Count - 1
            e.NewValues(i) = e.NewValues(i).ToString.TrimEnd
        Next

        'validazione ed evidenziazione controlli "difettosi"
        If Not autoValidate(e.NewValues) Then
            e.Cancel = True
            Exit Sub
        End If

    End Sub

    Private Sub AssignMaxLens(ByVal c As Control, ByVal maxLens As Dictionary(Of String, Integer))
        Dim fieldName As String
        Dim cSub As System.Web.UI.Control
        If TypeOf c Is TextBox Then
            If c.ID Like "*TextBox" Then
                fieldName = Mid(c.ID, 1, Len(c.ID) - 7).ToUpper
                If maxLens.ContainsKey(fieldName) Then
                    CType(c, TextBox).MaxLength = maxLens(fieldName)
                End If
            End If
        ElseIf TypeOf c Is ICustomControl Then
            With CType(c, ICustomControl)
                If maxLens.ContainsKey(.FieldName.ToUpper) Then
                    .MaxLength = maxLens(.FieldName.ToUpper)
                End If
            End With
        End If

        For Each cSub In c.Controls
            AssignMaxLens(cSub, maxLens)
        Next

    End Sub

#Region "Ricerca controlli collegati"
    Private Function FindParentUpdatePanel() As UpdatePanel
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

    Private Function FindBoundStlSqlDataSource() As StlSqlDataSource
        Return CType(Softailor.Global.WebUIUtils.FindControl(Me.Page, Me.DataSourceID), StlSqlDataSource)
    End Function

    Public Function FindBoundStlGridView() As StlGridView
        If _ro_boundStlGridViewID = "" Then
            Return Nothing
        Else
            Return CType(Softailor.Global.WebUIUtils.FindControl(Me.Page, _ro_boundStlGridViewID), StlGridView)
        End If
    End Function

#End Region

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Me.HeaderRow Is Nothing Then
            Dim hdr As FormViewRow = Me.CreateRow(0, DataControlRowType.Header, DataControlRowState.Normal)
        End If
        Dim htmlWriter As New HtmlTextWriter(New System.IO.StringWriter)
        MyBase.Render(htmlWriter)
        Dim html As String = htmlWriter.InnerWriter.ToString
        html = REGEX_BETWEEN_TAGS.Replace(html, "><")
        html = REGEX_LINE_BREAKS_BETWEEN_TAGS.Replace(html, String.Empty)
        writer.Write(html.Trim)
    End Sub

    Private Sub FocusFirstControl()
        Dim dummy As Boolean = IntFocusFirstActiveControl(Me)
    End Sub

    Private Function IntFocusFirstActiveControl(ByVal c As Control) As Boolean

        Dim focused As Boolean = False

        If TypeOf c Is TextBox Then
            With CType(c, TextBox)
                If .Enabled And Not .ReadOnly Then
                    .Focus()
                    focused = True
                End If
            End With
        ElseIf TypeOf c Is DropDownList Then
            With CType(c, DropDownList)
                If .Enabled Then
                    .Focus()
                    focused = True
                End If
            End With
        ElseIf TypeOf c Is CheckBox Then
            With CType(c, CheckBox)
                If .Enabled Then
                    .Focus()
                    focused = True
                End If
            End With
        ElseIf TypeOf c Is ICustomControl Then
            With CType(c, ICustomControl)
                If .IsEnabled Then
                    .SetFocus()
                    focused = True
                End If
            End With
        End If

        If Not focused Then
            For Each subC As Control In c.Controls
                If IntFocusFirstActiveControl(subC) Then Exit For
            Next
        End If

        Return focused

    End Function

    Private Sub StlFormView_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewModeEventArgs) Handles Me.ModeChanging

        Dim _allowInsert As Boolean = False
        Dim canDo As Boolean

        'determino _allowInsert: ho una grid, la grid ha allowinsert, ho allowedit
        _allowInsert = False
        If _allowEdit And _ro_boundStlGridViewID <> "" Then
            If BoundStlGridView.AllowInsert Then _allowInsert = True
        End If

        'misura di sicurezza: blocco i cambi di stato non autorizzati
        canDo = True
        If e.NewMode = FormViewMode.Insert And Not _allowInsert Then canDo = False
        If e.NewMode = FormViewMode.Edit And Not _allowEdit Then canDo = False

        If Not canDo Then
            e.Cancel = True
            Exit Sub
        End If


        'se sto andando in edit o insert...
        If e.NewMode = FormViewMode.Edit Or e.NewMode = FormViewMode.Insert Then
            If _ro_boundStlGridViewID <> "" Then
                If Not BoundStlGridView.SaveMyForm_ParentForms_ChildrenFormsIfDirty(False, True, True) Then e.Cancel = True
            End If
        End If

    End Sub

    Private Enum ControlTypes
        None
        TextBox
        DropDownList
        CustomControl
    End Enum

    Public Enum ErrorTypes
        None
        Required
        InvalidInteger
        InvalidDecimal
        InvalidDateTime
        InvalidCustomControl
    End Enum

    Private Function ErrorDescription(ByVal errorType As ErrorTypes) As String
        Select Case errorType
            Case ErrorTypes.Required : Return "Campo obbligatorio."
            Case ErrorTypes.InvalidInteger : Return "Numero non valido. Immetti un numero intero."
            Case ErrorTypes.InvalidDecimal : Return "Numero non valido. Utilizza la virgola come separatore decimale."
            Case ErrorTypes.InvalidDateTime : Return "Data o ora non valida. Scrivi le date in formato gg/mm/aaaa e gli orari in formato hh:mm oppure hh:mm:ss."
            Case Else : Return ""
        End Select
    End Function

    Private Function autoValidate(ByVal values As System.Collections.Specialized.IOrderedDictionary) As Boolean

        Dim allOk As Boolean
        Dim fName As String
        Dim fieldName As String
        Dim fieldValue As String
        Dim controlType As ControlTypes   'TextBox o DropDownList oppure nulla
        Dim errorType As ErrorTypes     'required invalidInt
        Dim sds As StlSqlDataSource = Me.BoundStlSqlDataSource
        Dim c As Control
        Dim cTxt As TextBox = Nothing
        Dim cDdn As DropDownList = Nothing
        Dim cCus As ICustomControl = Nothing
        Dim cReadOnly As Boolean


        'ciclo sui dati letti dal form

        allOk = True

        For Each fname In values.Keys

            fieldName = fname.ToUpper
            fieldValue = CStr(values(fName))

            'reset variabili
            controlType = ControlTypes.None
            errorType = ErrorTypes.None
            cReadOnly = False

            'provo a localizzare il controllo. Se non lo trovo, allora è inutile che faccia controlli.

            'opzione 1: textbox
            c = Me.FindControl(fieldName & "TextBox")
            If Not c Is Nothing Then
                controlType = ControlTypes.TextBox
                cTxt = CType(c, TextBox)
                cReadOnly = cTxt.ReadOnly
            End If
            'opzione 2: dropdownlist
            c = Me.FindControl(fieldName & "DropDownList")
            If Not c Is Nothing Then
                controlType = ControlTypes.DropDownList
                cDdn = CType(c, DropDownList)
            End If
            'opzione 3: custom control
            For Each c In FindICustomControls(Me)
                If TypeOf c Is ICustomControl Then
                    If CType(c, ICustomControl).FieldName = fieldName Then
                        controlType = ControlTypes.CustomControl
                        cCus = CType(c, ICustomControl)
                        Exit For
                    End If
                End If
            Next

            'se ho trovato il controllo e NON è read only, allora faccio i controlli
            If controlType <> ControlTypes.None And Not cReadOnly Then

                'verifica nullo
                If sds.RequiredFieldNames.Contains(fieldName) And fieldValue = "" Then
                    errorType = ErrorTypes.Required
                End If

                'verifica parsing
                If fieldValue <> "" Then
                    Select Case sds.FieldTypes(fieldName)
                        Case "Int32", "Integer"
                            Try
                                Integer.Parse(fieldValue, Softailor.Global.Cultures.CulturaItalian)
                            Catch ex As Exception
                                errorType = ErrorTypes.InvalidInteger
                            End Try
                        Case "Decimal"
                            Try
                                Decimal.Parse(fieldValue, Softailor.Global.Cultures.CulturaItalianConSeparatoreMigliaia)
                            Catch ex As Exception
                                errorType = ErrorTypes.InvalidDecimal
                            End Try
                        Case "Single"
                            Try
                                Single.Parse(fieldValue, Softailor.Global.Cultures.CulturaItalianConSeparatoreMigliaia)
                            Catch ex As Exception
                                errorType = ErrorTypes.InvalidDecimal
                            End Try
                        Case "Double"
                            Try
                                Single.Parse(fieldValue, Softailor.Global.Cultures.CulturaItalianConSeparatoreMigliaia)
                            Catch ex As Exception
                                errorType = ErrorTypes.InvalidDecimal
                            End Try
                        Case "DateTime", "TimeSpan"
                            Try
                                Date.Parse(fieldValue, Softailor.Global.Cultures.CulturaItalian)
                            Catch ex As Exception
                                errorType = ErrorTypes.InvalidDateTime
                            End Try
                    End Select
                End If

                'verifica validità controlli custom
                If controlType = ControlTypes.CustomControl Then
                    If Not cCus.IsValid Then
                        errorType = ErrorTypes.InvalidCustomControl
                    End If
                End If

                'Se ho errore...
                If errorType <> ErrorTypes.None Then
                    'evidenzio e scrivo
                    Select Case controlType
                        Case ControlTypes.TextBox
                            cTxt.BackColor = Drawing.Color.Yellow
                            cTxt.ToolTip = ErrorDescription(errorType)
                        Case ControlTypes.DropDownList
                            cDdn.BackColor = Drawing.Color.Yellow
                            cDdn.ToolTip = ErrorDescription(errorType)
                        Case ControlTypes.CustomControl
                            cCus.SetHasError(errorType)
                            cCus.SetErrorText(errorType, ErrorDescription(errorType))
                    End Select

                    'imposto a false allOk
                    allOk = False
                Else
                    'disevidenzio
                    Select Case controlType
                        Case ControlTypes.TextBox
                            cTxt.BackColor = Drawing.Color.Empty
                            cTxt.ToolTip = ""
                        Case ControlTypes.DropDownList
                            cDdn.BackColor = Drawing.Color.Empty
                            cDdn.ToolTip = ""
                        Case ControlTypes.CustomControl
                            cCus.SetIsOK()
                            cCus.ClearErrorText()
                    End Select
                End If
            End If
        Next



        If Not allOk Then
            'scrivo l'errore
            LaunchPreSaveError()
        End If
        Return allOk
    End Function

    Private Function FindICustomControls(ByVal root As Control) As List(Of Control)

        Dim list As New List(Of Control)

        FindICustomControlsRecursive(root, list)

        Return list

    End Function

    Private Sub FindICustomControlsRecursive(ByVal c As Control, ByVal list As List(Of Control))

        Dim cSub As Control

        For Each cSub In c.Controls
            If TypeOf cSub Is ICustomControl Then
                list.Add(cSub)
            Else
                FindICustomControlsRecursive(cSub, list)
            End If
        Next

    End Sub


End Class
