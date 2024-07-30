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
Imports Softailor.Web.UI.DbForms

Public Class SearchIscrittiGF
    Inherits Panel
    Implements ISqlStringProvider

    'connessioni DB
    Private _dataSqlConnection As SqlConnection

    'oggetti collegati
    Private _ro_childrenStlGridViews As New List(Of StlGridView)
    Private _ro_childrenStlGridViewsSearched As Boolean = False

    'sub-controlli
    Protected WithEvents txtCognome As TextBox
    Protected WithEvents txtNome As TextBox

    Protected WithEvents lnkDEAll As LinkButton
    Protected WithEvents lnkDENone As LinkButton
    Protected WithEvents cblDipendenteEsterno As CheckBoxList

    Protected WithEvents lnkOIAll As LinkButton
    Protected WithEvents lnkOINone As LinkButton
    Protected WithEvents cblOrigine As CheckBoxList

    Protected WithEvents lnkCEAll As LinkButton
    Protected WithEvents lnkCENone As LinkButton
    Protected WithEvents cblCategoriaEcm As CheckBoxList

    Protected WithEvents lnkSIAll As LinkButton
    Protected WithEvents lnkSINone As LinkButton
    Protected WithEvents cblStatoIscrizione As CheckBoxList

    Protected WithEvents lnkSEAll As LinkButton
    Protected WithEvents lnkSENone As LinkButton
    Protected WithEvents cblStatoEcm As CheckBoxList

    Protected WithEvents lnkSQAll As LinkButton
    Protected WithEvents lnkSQNone As LinkButton
    Protected WithEvents cblStatoQuestionario As CheckBoxList

    Protected WithEvents lnkSPAll As LinkButton
    Protected WithEvents lnkSPNone As LinkButton
    Protected WithEvents cblStatoPresenza As CheckBoxList

    Protected WithEvents ddnOrder As DropDownList

    Protected WithEvents btnClear As Button
    Protected WithEvents btnFilter As Button

    Protected WithEvents hidCognome As HiddenField
    Protected WithEvents hidNome As HiddenField
    Protected WithEvents hidDipendenteEsterno As HiddenField
    Protected WithEvents hidOrigine As HiddenField
    Protected WithEvents hidCategoriaEcm As HiddenField
    Protected WithEvents hidStatoIscrizione As HiddenField
    Protected WithEvents hidStatoEcm As HiddenField
    Protected WithEvents hidStatoQuestionario As HiddenField
    Protected WithEvents hidStatoPresenza As HiddenField
    Protected WithEvents hidOrder As HiddenField

    Public Property id_EVENTO As Integer = 0

    Public Function GetSql() As String Implements Web.UI.DbForms.ISqlStringProvider.GetSql

        Dim sOut = "SELECT id_ISCRITTO, tx_COGNOMTIT_PRO, ac_MATRICOLA, dt_CREAZIONE, " &
            "ac_ORIGINEISCRIZIONE, tx_ORIGINEISCRIZIONE, ni_ORDINEORIGINEISCRIZIONE, ac_RGBORIGINEISCRIZIONE, " &
            "ac_CATEGORIAECM, tx_CATEGORIAECM, ni_ORDINECATEGORIAECM, ac_RGBCATEGORIAECM, " &
            "ac_STATOISCRIZIONE, tx_STATOISCRIZIONE, ac_RGBSTATOISCRIZIONE, ni_ORDINESTATOISCRIZIONE, " &
            "ac_STATOECM, tx_STATOECM, ac_RGBSTATOECM, ni_ORDINESTATOECM, " &
            "ac_STATOQUESTIONARIO, tx_STATOQUESTIONARIO, ac_RGBSTATOQUESTIONARIO, ni_ORDINESTATOQUESTIONARIO, " &
            "ac_STATOPRESENZA, tx_STATOPRESENZA, ac_RGBSTATOPRESENZA, ni_ORDINESTATOPRESENZA, ni_TOTALEMINUTIEVENTO, ni_MINIMOMINUTIEVENTO, ni_TOTALEMINUTIISCRITTO, " &
            "ni_RISPOSTEOK, ni_RISPOSTEKO, ni_RISPOSTEND, ni_RISPOSTE, ni_MINIMORISPOSTE, dt_OTTENIMENTOCREDITIECM, tx_EMAIL, dt_INVIOMAILATTECM, dt_INVIOMAILATTPART, fl_ATTECMONLINE, fl_ATTPARTONLINE_DRMT_DIP, fl_ATTPARTONLINE_DRMT_EST, fl_ATTPARTONLINE_P_DIP, fl_ATTPARTONLINE_P_EST " &
            "FROM vw_eve_ISCRITTI WHERE id_EVENTO=" & SQL_Int32(id_EVENTO)

        If hidCognome.Value <> String.Empty Then
            sOut &= " AND tx_COGNOME like " & SQL_String("%" & hidCognome.Value & "%")
        End If

        If hidNome.Value <> String.Empty Then
            sOut &= " AND tx_NOME like " & SQL_String("%" & hidNome.Value & "%")
        End If

        sOut &= filterAtom("ac_DIPENDENTEESTERNO", hidDipendenteEsterno.Value)
        sOut &= filterAtom("ac_ORIGINEISCRIZIONE", hidOrigine.Value)
        sOut &= filterAtom("ac_CATEGORIAECM", hidCategoriaEcm.Value)
        sOut &= filterAtom("ac_STATOISCRIZIONE", hidStatoIscrizione.Value)
        sOut &= filterAtom("ac_STATOECM", hidStatoEcm.Value)
        sOut &= filterAtom("ac_STATOQUESTIONARIO", hidStatoQuestionario.Value)
        sOut &= filterAtom("ac_STATOPRESENZA", hidStatoPresenza.Value)

        sOut &= " ORDER BY "
        Select Case hidOrder.Value
            Case "CN"
                sOut &= "tx_COGNOME, tx_NOME"
            Case "DI"
                sOut &= "dt_CREAZIONE, tx_COGNOME, tx_NOME"
            Case "OI"
                sOut &= "ni_ORDINEORIGINEISCRIZIONE, tx_COGNOME, tx_NOME"
            Case "CE"
                sOut &= "ni_ORDINECATEGORIAECM, tx_COGNOME, tx_NOME"
            Case "SI"
                sOut &= "ni_ORDINESTATOISCRIZIONE, tx_COGNOME, tx_NOME"
            Case "SE"
                sOut &= "ni_ORDINESTATOECM, tx_COGNOME, tx_NOME"
            Case Else
                sOut &= "tx_COGNOME, tx_NOME"
        End Select

        Return sOut

    End Function

    Public Function SomeSearchFilter() As Boolean

        Return _
            hidCognome.Value <> String.Empty Or _
            hidNome.Value <> String.Empty Or _
            hidDipendenteEsterno.Value <> String.Empty Or _
            hidCategoriaEcm.Value <> String.Empty Or _
            hidStatoEcm.Value <> String.Empty Or _
            hidStatoIscrizione.Value <> String.Empty Or _
            hidOrigine.Value <> String.Empty Or _
            hidStatoQuestionario.Value <> String.Empty Or _
            hidStatoPresenza.Value <> String.Empty

    End Function

    Private Function filterAtom(fieldName As String, hText As String) As String

        If hText = "" Then
            'vuoto: non ritorno nulla (sono tutti selezionati)
            Return ""
        Else
            If hText = "\" Then
                'nessuno selezionato
                Return " AND (1=0)"
            Else
                Dim sOut = " AND " & fieldName & " IN ("
                For Each s As String In hText.Split("\"c)
                    If s <> String.Empty Then
                        sOut &= SQL_String(s) & ", "
                    End If
                Next
                sOut = Mid(sOut, 1, Len(sOut) - 2) & ")"
                Return sOut
            End If
        End If
    End Function

    Private Sub SearchIscrittiGF_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Not Me.DesignMode Then
            'apertura connessioni DB
            _dataSqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("SiteTailorDbConnectionString").ConnectionString)
            _dataSqlConnection.Open()

            'generazione controlli
            CreateControls()

            Me.DefaultButton = "btnFilter"
        End If
    End Sub

    Private Sub CreateControls()

        'lettura dati
        Dim dbCmd = _dataSqlConnection.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_SearchIscrittiData"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_EVENTO
        End With

        Dim sAspx = Transformer.Transform(dbCmd, "~/EVE/Templates/SearchIscritti.xslt")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")
        Dim cCreato = Me.Page.ParseControl(sAspx)

        Me.Controls.Clear()
        Me.Controls.Add(cCreato)

        'aggancio controlli
        txtCognome = CType(cCreato.FindControl("txtCognome"), TextBox)
        txtNome = CType(cCreato.FindControl("txtNome"), TextBox)

        lnkDEAll = CType(cCreato.FindControl("lnkDEAll"), LinkButton)
        lnkDENone = CType(cCreato.FindControl("lnkDENone"), LinkButton)
        cblDipendenteEsterno = CType(cCreato.FindControl("cblDipendenteEsterno"), CheckBoxList)

        lnkOIAll = CType(cCreato.FindControl("lnkOIAll"), LinkButton)
        lnkOINone = CType(cCreato.FindControl("lnkOINone"), LinkButton)
        cblOrigine = CType(cCreato.FindControl("cblOrigine"), CheckBoxList)

        lnkCEAll = CType(cCreato.FindControl("lnkCEAll"), LinkButton)
        lnkCENone = CType(cCreato.FindControl("lnkCENone"), LinkButton)
        cblCategoriaEcm = CType(cCreato.FindControl("cblCategoriaEcm"), CheckBoxList)

        lnkSIAll = CType(cCreato.FindControl("lnkSIAll"), LinkButton)
        lnkSINone = CType(cCreato.FindControl("lnkSINone"), LinkButton)
        cblStatoIscrizione = CType(cCreato.FindControl("cblStatoIscrizione"), CheckBoxList)

        lnkSEAll = CType(cCreato.FindControl("lnkSEAll"), LinkButton)
        lnkSENone = CType(cCreato.FindControl("lnkSENone"), LinkButton)
        cblStatoEcm = CType(cCreato.FindControl("cblStatoEcm"), CheckBoxList)

        lnkSQAll = CType(cCreato.FindControl("lnkSQAll"), LinkButton)
        lnkSQNone = CType(cCreato.FindControl("lnkSQNone"), LinkButton)
        cblStatoQuestionario = CType(cCreato.FindControl("cblStatoQuestionario"), CheckBoxList)

        lnkSPAll = CType(cCreato.FindControl("lnkSPAll"), LinkButton)
        lnkSPNone = CType(cCreato.FindControl("lnkSPNone"), LinkButton)
        cblStatoPresenza = CType(cCreato.FindControl("cblStatoPresenza"), CheckBoxList)

        ddnOrder = CType(cCreato.FindControl("ddnOrder"), DropDownList)
        btnClear = CType(cCreato.FindControl("btnClear"), Button)
        btnFilter = CType(cCreato.FindControl("btnFilter"), Button)
        hidCognome = CType(cCreato.FindControl("hidCognome"), HiddenField)
        hidNome = CType(cCreato.FindControl("hidNome"), HiddenField)
        hidDipendenteEsterno = CType(cCreato.FindControl("hidDipendenteEsterno"), HiddenField)
        hidOrigine = CType(cCreato.FindControl("hidOrigine"), HiddenField)
        hidCategoriaEcm = CType(cCreato.FindControl("hidCategoriaEcm"), HiddenField)
        hidStatoIscrizione = CType(cCreato.FindControl("hidStatoIscrizione"), HiddenField)
        hidStatoEcm = CType(cCreato.FindControl("hidStatoEcm"), HiddenField)
        hidStatoQuestionario = CType(cCreato.FindControl("hidStatoQuestionario"), HiddenField)
        hidStatoPresenza = CType(cCreato.FindControl("hidStatoPresenza"), HiddenField)
        hidOrder = CType(cCreato.FindControl("hidOrder"), HiddenField)

    End Sub

    Private Sub SearchIscrittiGF_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not Me.DesignMode Then
            If Not _dataSqlConnection Is Nothing Then
                _dataSqlConnection.Close()
                _dataSqlConnection.Dispose()
            End If
        End If
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        DoSearch()
    End Sub

    Private Overloads Sub SelCb(lb As LinkButton, sel As Boolean)

        Dim cbl As CheckBoxList = Nothing

        Select Case Mid(lb.ID, 4, 2)
            Case "DE" : cbl = cblDipendenteEsterno
            Case "CE" : cbl = cblCategoriaEcm
            Case "OI" : cbl = cblOrigine
            Case "SI" : cbl = cblStatoIscrizione
            Case "SE" : cbl = cblStatoEcm
            Case "SQ" : cbl = cblStatoQuestionario
            Case "SP" : cbl = cblStatoPresenza
        End Select

        For Each li As ListItem In cbl.Items
            li.Selected = sel
        Next
    End Sub

    Private Overloads Sub SelCb(cbl As CheckBoxList, sel As Boolean)

        For Each li As ListItem In cbl.Items
            li.Selected = sel
        Next
    End Sub

    Private Sub lnkSelAll_Click(sender As Object, e As System.EventArgs) Handles lnkDEAll.Click, lnkCEAll.Click, lnkOIAll.Click, lnkSIAll.Click, lnkSEAll.Click, lnkSQAll.Click, lnkSPAll.Click
        SelCb(CType(sender, LinkButton), True)
    End Sub

    Private Sub lnkSelNone_Click(sender As Object, e As System.EventArgs) Handles lnkDENone.Click, lnkCENone.Click, lnkOINone.Click, lnkSINone.Click, lnkSENone.Click, lnkSQNone.Click, lnkSPNone.Click
        SelCb(CType(sender, LinkButton), False)
    End Sub

    Public Sub SetRoamingData(rd As GFRoamingData)

        txtCognome.Text = ""
        If rd.cognomeSet Then txtCognome.Text = rd.cognome

        txtNome.Text = ""
        If rd.cognomeSet Then txtNome.Text = rd.nome

        If rd.categoriePersoneSet Then
            SelCbList(cblDipendenteEsterno, rd.categoriePersone)
        Else
            SelCb(cblDipendenteEsterno, True)
        End If

        If rd.categorieEcmSet Then
            SelCbList(cblCategoriaEcm, rd.categorieEcm)
        Else
            SelCb(cblCategoriaEcm, True)
        End If

        If rd.statiEcmSet Then
            SelCbList(cblStatoEcm, rd.statiEcm)
        Else
            SelCb(cblStatoEcm, True)
        End If

        If rd.statiQuestionarioSet Then
            SelCbList(cblStatoQuestionario, rd.statiQuestionario)
        Else
            SelCb(cblStatoQuestionario, True)
        End If

        If rd.statiPresenzaSet Then
            SelCbList(cblStatoPresenza, rd.statiPresenza)
        Else
            SelCb(cblStatoPresenza, True)
        End If

        If rd.statiIscrizioneSet Then
            SelCbList(cblStatoIscrizione, rd.statiIscrizione)
        Else
            SelCb(cblStatoIscrizione, True)
        End If

        If rd.originiIscrizioneSet Then
            SelCbList(cblOrigine, rd.originiIscrizione)
        Else
            SelCb(cblOrigine, True)
        End If

        ddnOrder.SelectedValue = rd.ordinamento

    End Sub

    Private Sub SelCbList(cbl As CheckBoxList, keys As List(Of String))

        For Each li As ListItem In cbl.Items
            li.Selected = keys.Contains(li.Value)
        Next

    End Sub

    Private Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        SetRoamingData(New GFRoamingData)
        DoSearch()
    End Sub

    Public Sub DoSearch()

        CopyValuesToHiddenControls()
        'aggiornamento delle griglie e aggancio alla prima riga
        For Each stlGridView In ChildrenStlGridViews
            stlGridView.DataBind()
            stlGridView.GotoEmptyRow()
            stlGridView.UpdateParentPanel()
            'stlGridView.ScrollTop()
        Next

    End Sub

    Private Sub CopyValuesToHiddenControls()
        txtCognome.Text = Trim(txtCognome.Text)
        txtNome.Text = Trim(txtNome.Text)

        hidCognome.Value = txtCognome.Text
        hidNome.Value = txtNome.Text

        hidDipendenteEsterno.Value = EnqueueValues(cblDipendenteEsterno)
        hidCategoriaEcm.Value = EnqueueValues(cblCategoriaEcm)
        hidStatoEcm.Value = EnqueueValues(cblStatoEcm)
        hidStatoIscrizione.Value = EnqueueValues(cblStatoIscrizione)
        hidOrigine.Value = EnqueueValues(cblOrigine)
        hidStatoQuestionario.Value = EnqueueValues(cblStatoQuestionario)
        hidStatoPresenza.Value = EnqueueValues(cblStatoPresenza)

        hidOrder.Value = ddnOrder.SelectedValue

    End Sub

    Private Function EnqueueValues(cbl As CheckBoxList) As String

        Dim nTot As Integer = cbl.Items.Count
        Dim nSel As Integer = 0
        Dim sSel As String = ""

        For Each li As ListItem In cbl.Items
            If li.Selected Then
                nSel += 1
                sSel += "\" & li.Value
            End If
        Next

        Select Case nSel
            Case nTot
                sSel = ""
            Case 0
                sSel = "\"
        End Select

        Return sSel

    End Function

#Region "Ricerca griglie collegate"

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

#End Region

End Class
