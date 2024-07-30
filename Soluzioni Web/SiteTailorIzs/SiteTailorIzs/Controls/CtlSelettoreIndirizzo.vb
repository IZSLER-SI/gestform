Imports System.Web.UI
Imports System.Data.SqlClient
Imports Softailor.SiteTailor.ACL
Imports Softailor.Web.UI.DbForms

Public Class CtlSelettoreIndirizzo
    Inherits WebControl
    Implements ICustomControl, INamingContainer


    'proprietà da designer
    Private _fieldName As String    'inutile
    Private _isEnabled As Boolean
    Private _maxLength As Integer

    Private _firstLabelWidthPx As Integer

    'controlli
    Protected WithEvents ddnac_tipoindirizzo As DropDownList
    Protected WithEvents txttx_indirizzoITA As TextBox
    Protected WithEvents txttx_indirizzoINTL As TextBox
    Protected WithEvents txtcapcomuneprovincia As TextBox
    'Protected WithEvents acecapcomuneprovincia As AjaxControlToolkit.AutoCompleteExtender
    Protected WithEvents txttx_localita As TextBox

    Protected WithEvents txttx_postalcode As TextBox
    Protected WithEvents txttx_city As TextBox
    Protected WithEvents txttx_provincia As TextBox
    Protected WithEvents txttx_stato As TextBox
    Protected WithEvents ddnac_nazione As DropDownList

    Protected WithEvents pnlIndirizzoITA As Panel
    Protected WithEvents pnlIndirizzoINTL As Panel

#Region "proprietà associate ai dati"

    Public Property ac_tipoindirizzo As String
        Get
            Return ddnac_tipoindirizzo.SelectedValue
        End Get
        Set(value As String)
            If value = "" Then
                ddnac_tipoindirizzo.SelectedValue = "ITA"
            Else
                ddnac_tipoindirizzo.SelectedValue = value
            End If
            pnlIndirizzoITA.Visible = ddnac_tipoindirizzo.SelectedValue = "ITA"
            pnlIndirizzoINTL.Visible = ddnac_tipoindirizzo.SelectedValue = "INTL"
        End Set
    End Property

    Public Property tx_indirizzo As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "ITA" Then
                Return txttx_indirizzoITA.Text.Trim
            Else
                Return txttx_indirizzoINTL.Text.Trim
            End If
        End Get
        Set(value As String)
            'lo scrivo in entrambi
            txttx_indirizzoITA.Text = value
            txttx_indirizzoINTL.Text = value
        End Set
    End Property

    Public Property ac_cap_ac_comune As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "ITA" Then
                'quando leggo, cerco il testo esatto - con il CAP
                Return FindCapBelfiore(txtcapcomuneprovincia.Text.Trim)
            Else
                Return "         "
            End If
        End Get
        Set(value As String)
            'quando scrivo, leggo dall'intera query
            txtcapcomuneprovincia.Text = FindCapComuneProvincia(value)
        End Set
    End Property

    Public Property tx_localita As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "ITA" Then
                Return txttx_localita.Text.Trim
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            txttx_localita.Text = value
        End Set
    End Property

    Public Property tx_postalcode As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "INTL" Then
                Return txttx_postalcode.Text.Trim
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            txttx_postalcode.Text = value
        End Set
    End Property

    Public Property tx_city As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "INTL" Then
                Return txttx_city.Text.Trim
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            txttx_city.Text = value
        End Set
    End Property

    Public Property tx_provincia As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "INTL" Then
                Return txttx_provincia.Text.Trim
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            txttx_provincia.Text = value
        End Set
    End Property

    Public Property tx_stato As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "INTL" Then
                Return txttx_stato.Text.Trim
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            txttx_stato.Text = value
        End Set
    End Property

    Public Property ac_nazione As String
        Get
            If ddnac_tipoindirizzo.SelectedValue = "INTL" Then
                Return ddnac_nazione.SelectedValue
            Else
                Return ""
            End If
        End Get
        Set(value As String)
            ddnac_nazione.SelectedValue = value
        End Set
    End Property

#End Region

    Public Property FirstLabelWidthPx() As Integer
        Get
            Return _firstLabelWidthPx
        End Get
        Set(ByVal value As Integer)
            _firstLabelWidthPx = value
        End Set
    End Property

    Public Property Value() As String Implements ICustomControl.Value
        Get
            Return ""
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    Public Property FieldName() As String Implements ICustomControl.FieldName
        Get
            Return _fieldName
        End Get
        Set(ByVal value As String)
            _fieldName = value
        End Set
    End Property

    Public Sub SetEnabled() Implements ICustomControl.SetEnabled
        ddnac_tipoindirizzo.Enabled = True
        txttx_indirizzoITA.Enabled = True
        txttx_indirizzoINTL.Enabled = True
        txtcapcomuneprovincia.Enabled = True
        txttx_localita.Enabled = True
        txttx_postalcode.Enabled = True
        txttx_city.Enabled = True
        txttx_provincia.Enabled = True
        txttx_stato.Enabled = True
        ddnac_nazione.Enabled = True
    End Sub

    Public Sub SetErrorText(ByVal errorType As StlFormView.ErrorTypes, ByVal errorText As String) Implements ICustomControl.SetErrorText
        If errorType = StlFormView.ErrorTypes.InvalidCustomControl Then
            txtcapcomuneprovincia.ToolTip = "Combinazione CAP/comune/provincia inesistente."
        Else
            txtcapcomuneprovincia.ToolTip = errorText
        End If
    End Sub

    Public Property MaxLength() As Integer Implements ICustomControl.MaxLength
        Get
            Return _maxLength
        End Get
        Set(ByVal value As Integer)
            _maxLength = value
        End Set
    End Property

    Public Sub SetIsOK() Implements ICustomControl.SetIsOK
        txtcapcomuneprovincia.BackColor = Drawing.Color.Empty
    End Sub

    Public Shadows Function IsEnabled() As Boolean Implements ICustomControl.IsEnabled
        Return _isEnabled
    End Function

    Public Sub ClearErrorText() Implements ICustomControl.ClearErrorText
        txtcapcomuneprovincia.ToolTip = ""
    End Sub

    Public Sub SetFocus() Implements ICustomControl.SetFocus
        ddnac_tipoindirizzo.Focus()
    End Sub

    Public Sub SetHasError(ByVal errorType As StlFormView.ErrorTypes) Implements ICustomControl.SetHasError
        txtcapcomuneprovincia.BackColor = Drawing.Color.Yellow
    End Sub

    Public Sub SetDisabled() Implements ICustomControl.SetDisabled
        ddnac_tipoindirizzo.Enabled = False
        txttx_indirizzoITA.Enabled = False
        txttx_indirizzoINTL.Enabled = False
        txtcapcomuneprovincia.Enabled = False
        txttx_localita.Enabled = False
        txttx_postalcode.Enabled = False
        txttx_city.Enabled = False
        txttx_provincia.Enabled = False
        txttx_stato.Enabled = False
        ddnac_nazione.Enabled = False
    End Sub

    Private Sub CtlSelettoreIndirizzo_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerazioneControlli()
    End Sub

    Public Sub GenerazioneControlli()

        'controlli
        Me.Controls.Clear()

        ddnac_tipoindirizzo = New DropDownList
        With ddnac_tipoindirizzo
            .ID = "ddnac_tipoindirizzo"
            .CssClass = "ddn"
            .Width = Unit.Pixel(150)
            .Items.Clear()
            .Items.Add(New ListItem("Italiano", "ITA"))
            .Items.Add(New ListItem("Internazionale", "INTL"))
            .AutoPostBack = True
            .SelectedValue = "ITA"
        End With

        txttx_indirizzoITA = New TextBox
        With txttx_indirizzoITA
            .ID = "txttx_indirizzoITA"
            .CssClass = "txt"
            .Width = Unit.Pixel(480)
            .MaxLength = 300
        End With
        txttx_indirizzoINTL = New TextBox
        With txttx_indirizzoINTL
            .ID = "txttx_indirizzoINTL"
            .CssClass = "txt"
            .Width = Unit.Pixel(480)
            .MaxLength = 300
        End With

        txtcapcomuneprovincia = New TextBox
        With txtcapcomuneprovincia
            .ID = "txtcapcomuneprovincia"
            .CssClass = "txt stl_ac_capcomuneprovincia"
            .AutoCompleteType = AutoCompleteType.Disabled
            .Width = Unit.Pixel(460)
        End With

        'acecapcomuneprovincia = New AjaxControlToolkit.AutoCompleteExtender
        'With acecapcomuneprovincia
        '    .ID = "acecapcomuneprovincia"
        '    .ServicePath = "~/PageWS/AutoCompleteCapComuneProvincia.asmx"
        '    .ServiceMethod = "GetSuggestions"
        '    .TargetControlID = "txtcapcomuneprovincia"
        '    .MinimumPrefixLength = 2
        '    .CompletionSetCount = 17
        '    .CompletionInterval = 1
        '    .EnableCaching = True


        '    '.OnClientShown = "moveItUp"
        'End With

        txttx_localita = New TextBox
        With txttx_localita
            .ID = "txttx_localita"
            .CssClass = "txt"
            .Width = Unit.Pixel(480)
            .MaxLength = 100
        End With

        txttx_postalcode = New TextBox
        With txttx_postalcode
            .ID = "txttx_postalcode"
            .CssClass = "txt"
            .Width = Unit.Pixel(79)
            .MaxLength = 20
        End With

        txttx_city = New TextBox
        With txttx_city
            .ID = "txttx_city"
            .CssClass = "txt"
            .Width = Unit.Pixel(159)
            .MaxLength = 150
        End With

        txttx_provincia = New TextBox
        With txttx_provincia
            .ID = "txttx_provincia"
            .CssClass = "txt"
            .Width = Unit.Pixel(29)
            .MaxLength = 50
        End With

        txttx_stato = New TextBox
        With txttx_stato
            .ID = "txttx_stato"
            .CssClass = "txt"
            .Width = Unit.Pixel(95)
            .MaxLength = 50
        End With

        ddnac_nazione = New DropDownList
        With ddnac_nazione
            .ID = "ddnac_nazione"
            .CssClass = "ddn"
            .Width = Unit.Pixel(484)
        End With
        FillNazioni()

        'creazione dei pannelli
        pnlIndirizzoITA = New Panel
        pnlIndirizzoITA.Visible = True

        With pnlIndirizzoITA
            .Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Indirizzo</span>"))
            .Controls.Add(txttx_indirizzoITA)
            .Controls.Add(New LiteralControl("<br/>"))
            .Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Località</span>"))
            .Controls.Add(txttx_localita)
            .Controls.Add(New LiteralControl("<br/>"))
            .Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">CAP/Comune/Pr</span>"))

            Dim img As New Image
            With img
                .ImageUrl = "~/img/icoInfo.gif"
                .ToolTip = "Digita il CAP o una parte del nome del comune. Il sistema ti suggerirà le possibili alternative."
            End With
            .Controls.Add(txtcapcomuneprovincia)
            .Controls.Add(New LiteralControl("<span class=""flbl"">"))
            .Controls.Add(img)
            .Controls.Add(New LiteralControl("</span>"))
            '.Controls.Add(acecapcomuneprovincia)
        End With

        pnlIndirizzoINTL = New Panel
        pnlIndirizzoINTL.Visible = False
        With pnlIndirizzoINTL
            .Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Indirizzo</span>"))
            .Controls.Add(txttx_indirizzoINTL)
            .Controls.Add(New LiteralControl("<br/>"))
            .Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Codice Postale</span>"))
            .Controls.Add(txttx_postalcode)
            .Controls.Add(New LiteralControl("<span class=""slbl"" style=""width:35px"">Città</span>"))
            .Controls.Add(txttx_city)
            .Controls.Add(New LiteralControl("<span class=""slbl"" style=""width:34px"">Prov</span>"))
            .Controls.Add(txttx_provincia)
            .Controls.Add(New LiteralControl("<span class=""slbl"" style=""width:37px"">Stato</span>"))
            .Controls.Add(txttx_stato)
            .Controls.Add(New LiteralControl("<br/>"))
            .Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:90px"">Nazione</span>"))
            .Controls.Add(ddnac_nazione)
        End With

        'aggiunta dei controlli
        If _firstLabelWidthPx <= 0 Then
            Me.Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:73px"">Tipo Indirizzo</span>"))
        Else
            Me.Controls.Add(New LiteralControl("<span class=""flbl"" style=""width:" & _firstLabelWidthPx.ToString & "px"">Tipo Indirizzo</span>"))
        End If

        Me.Controls.Add(ddnac_tipoindirizzo)
        Me.Controls.Add(pnlIndirizzoITA)
        Me.Controls.Add(pnlIndirizzoINTL)

    End Sub

    Private Function FindParentStlFormView() As StlFormView

        Dim stlFormView As StlFormView = Nothing

        Dim curC As Control

        curC = Me

        Do While Not curC.Parent Is Nothing
            If TypeOf curC Is StlFormView Then
                stlFormView = CType(curC, StlFormView)
                Exit Do
            Else
                curC = curC.Parent
            End If
        Loop

        Return stlFormView

    End Function

    Public Sub New()
        _fieldName = ""
        _isEnabled = True
        _maxLength = 0
    End Sub


    Public Function IsValid() As Boolean Implements Web.UI.DbForms.ICustomControl.IsValid
        'nel caso di indirizzo internazionale è sempre valido; nel caso di indirizzo italiano è valido solo se è vuoto o se matcha
        If ddnac_tipoindirizzo.SelectedValue = "ITA" Then
            If txtcapcomuneprovincia.Text.Trim = "" Then
                Return True
            Else
                Return FindCapBelfiore(txtcapcomuneprovincia.Text.Trim).Trim <> String.Empty
            End If
        Else
            Return True
        End If
        Return True
    End Function

    Private Sub FillNazioni()

        'riempimento delle liste - diverso a seconda del fatto che si sia _enabled o meno

        'per iniziare, pulisco tutto come Chicco
        ddnac_nazione.Items.Clear()
        ddnac_nazione.Items.Add(New ListItem("", ""))
        ddnac_nazione.SelectedValue = ""

        'OK, dobbiamo gestire dei dati.
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
        dbConn.Open()

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_age_SelettoreIndirizzo_ListaNazioni"
        End With
        dbRdr = dbCmd.ExecuteReader
        With dbRdr
            Do While .Read
                ddnac_nazione.Items.Add(New ListItem(.GetString(1), .GetString(0)))
            Loop
        End With
        dbRdr.Close()
        dbCmd.Dispose()

        dbConn.Close()
        dbConn.Dispose()

    End Sub

    Private Sub ddnac_tipoindirizzo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddnac_tipoindirizzo.SelectedIndexChanged

        If ddnac_tipoindirizzo.SelectedValue = "ITA" Then
            pnlIndirizzoITA.Visible = True
            pnlIndirizzoINTL.Visible = False

            If txttx_indirizzoITA.Text.Trim = "" And txttx_indirizzoINTL.Text.Trim <> "" Then
                txttx_indirizzoITA.Text = txttx_indirizzoINTL.Text.Trim
            End If

        Else
            pnlIndirizzoITA.Visible = False
            pnlIndirizzoINTL.Visible = True

            If txttx_indirizzoINTL.Text.Trim = "" And txttx_indirizzoITA.Text.Trim <> "" Then
                txttx_indirizzoINTL.Text = txttx_indirizzoITA.Text.Trim
            End If

        End If

    End Sub

    Private Function FindCapComuneProvincia(ac_CAP_ac_COMUNE As String) As String

        Dim sOut = ""

        If ac_CAP_ac_COMUNE.Trim <> String.Empty Then

            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT tx_CAPCOMUNEPROVINCIA FROM vw_geo_CAP_COMUNI WHERE ac_CAP_ac_COMUNE=@ac_cap_ac_comune"
                .Parameters.Add("@ac_cap_ac_comune", SqlDbType.NVarChar, 9).Value = ac_CAP_ac_COMUNE
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                If .Read Then
                    sOut = .GetString(0)
                End If
            End With
            dbRdr.Close()
            dbCmd.Dispose()

            'se non ho trovato...
            If sOut = "" Then

                If Mid(ac_CAP_ac_COMUNE, 6).Trim = String.Empty Then
                    'solo CAP
                    sOut = Mid(ac_CAP_ac_COMUNE, 1, 5)
                Else
                    'solo comune o CAP + Comune, che non matcha
                    dbCmd = dbConn.CreateCommand
                    With dbCmd
                        .CommandType = CommandType.Text
                        .CommandText = "SELECT tx_COMUNE, ac_PROVINCIA FROM geo_COMUNIITALIA WHERE ac_COMUNE=@ac_comune"
                        .Parameters.Add("@ac_comune", SqlDbType.NVarChar, 4).Value = Mid(ac_CAP_ac_COMUNE, 6)
                    End With
                    dbRdr = dbCmd.ExecuteReader
                    With dbRdr
                        If .Read Then
                            sOut = ""
                            If Mid(ac_CAP_ac_COMUNE, 1, 5).Trim <> String.Empty Then
                                sOut &= Mid(ac_CAP_ac_COMUNE, 1, 5) & " "
                                sOut &= .GetString(0)
                                sOut &= " (" & .GetString(1) & ")"
                            End If
                            sOut = .GetString(0)
                        End If
                    End With
                    dbRdr.Close()
                    dbCmd.Dispose()
                End If

                
            End If
            dbConn.Close()
            dbConn.Dispose()
        End If

        Return sOut
    End Function

    Private Function FindCapBelfiore(tx_CAPCOMUNEPROVINCIA As String) As String

        Dim sOut = "         "

        If tx_CAPCOMUNEPROVINCIA.Trim <> String.Empty Then
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = New SqlConnection(FindParentStlFormView.BoundStlSqlDataSource.ConnectionString)
            dbConn.Open()

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT ac_CAP_ac_COMUNE FROM vw_geo_CAP_COMUNI WHERE " & _
                               "tx_CAPCOMUNEPROVINCIA=@tx_capcomuneprovincia AND " & _
                               "ac_COMUNE not like 'Z%' and fl_ATTUALE=1 and ac_CAP is not null"
                .Parameters.Add("@tx_capcomuneprovincia", SqlDbType.NVarChar, 500).Value = tx_CAPCOMUNEPROVINCIA
            End With
            dbRdr = dbCmd.ExecuteReader
            With dbRdr
                If .Read Then
                    sOut = .GetString(0)
                End If
            End With
            dbRdr.Close()
            dbCmd.Dispose()
            dbConn.Close()
            dbConn.Dispose()
        End If

        Return sOut

    End Function


End Class
