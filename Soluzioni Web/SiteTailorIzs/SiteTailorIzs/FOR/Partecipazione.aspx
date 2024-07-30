<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="Partecipazione.aspx.vb" Inherits="Softailor.SiteTailorIzs.Partecipazione" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script>
        function confirmClose() {
            if (window.confirm("Confermi l\'abbandono di eventuali dati inseriti o modificati?")) {
                parent.stl_sel_done('');
            }
        }
        function clearResponsabile()
        {
            $("#id_PERSONA_RESPONSABILE").val("");
            $("#tx_PERSONA_RESPONSABILE").val("");
        }
        function pickResponsabile()
        {
            $("#id_PERSONA_RESPONSABILE").val("");
            var url = "../Selettori/SelettorePersonaGForm.aspx?diponly=1";
            stl_sel_display(url, pickResponsabile_CallBack);
        }
        function pickResponsabile_CallBack(id_persona)
        {
            $("#id_PERSONA_RESPONSABILE").val(id_persona);
            clickPickResponsabile();
        }

        function clearPartecipante() {
            $("#id_PERSONA").val("");
            clickPickPersona();
        }
        function pickPartecipante() {
            $("#id_PERSONA").val("");
            var url = "../Selettori/SelettorePersonaGForm.aspx?diponly=1";
            stl_sel_display(url, pickPartecipante_CallBack);
        }
        function pickPartecipante_CallBack(id_persona) {
            $("#id_PERSONA").val(id_persona);
            clickPickPersona();
        }
        function pickEvento() {
            $("#id_EVENTO").val("");
            stl_sel_display("PickEvento.aspx", pickEvento_CallBack);
        }
        function pickEvento_CallBack(id_evento) {
            $("#id_EVENTO").val(id_evento);
            clickPickEvento();
        }
    </script>
    <style type="text/css">
        .btnicon
        {
            vertical-align: top;
            margin-top: 3px;
            cursor: pointer;
            margin-left: 0px;
        }
        /* form > binary element block */
        .beb_tbl /* tabella esterna */
        {
            display: inline-block;
            vertical-align: top;
            width: 250px;
            margin-bottom: 1px;
            margin-top: 1px;
            font-size: 11px;
            border-spacing: 0;
        }

        .beb_tdi /* TD immagine */
        {
            width: 70px;
            height: 70px;
            border: solid 1px #cacaca;
            padding: 0px 0px 0px 0px;
            background-color: #ffffff;
        }

            .beb_tdi img
            {
                border-width: 0px;
                vertical-align: bottom;
            }

        .beb_tdd /* TD dati */
        {
            height: 70px;
            border-top: solid 1px #cacaca;
            border-right: solid 1px #cacaca;
            border-bottom: solid 1px #cacaca;
            padding: 0px 0px 0px 0px;
            background-color: #ffffff;
            vertical-align: top;
        }

        .beb_dtf /* descrizione, formato, categoria */
        {
            display: block;
            width: 171px;
            height: 47px;
            padding: 2px 3px 2px 3px;
            border-bottom: solid 1px #cacaca;
            overflow: hidden;
            vertical-align: top;
        }

        .beb_err /* immagine errore */
        {
            vertical-align: middle;
            margin-right: 3px;
        }

        .beb_dsc /* label descrizione */
        {
            color: #000000;
            line-height: 11px;
            padding-bottom: 2px;
            font-family: Verdana, Arial, Sans-Serif;
        }

        .beb_fmt /* label formato */
        {
            display: block;
            color: #999999;
            font-size: 10px;
            line-height: 11px;
        }

        .beb_cat /* label categoria */
        {
            display: block;
            color: #999999;
            font-size: 10px;
            line-height: 11px;
        }

        .beb_cmd /* div comandi */
        {
            display: block;
            width: 177px;
            height: 18px;
            overflow: hidden;
            background-color: #000000;
            padding: 0px 0px 0px 0px;
        }

            .beb_cmd a
            {
                cursor: pointer;
                color: #15428b;
            }

                .beb_cmd a:hover
                {
                    background-color: #e3efff;
                }

                .beb_cmd a.aspNetDisabled
                {
                    cursor: default;
                    color: #999999;
                }

                    .beb_cmd a.aspNetDisabled:hover
                    {
                        background-color: #eaeaea;
                    }

        .beb_a /* comandi nuovo e seleziona */
        {
            display: inline-block;
            width: 88px;
            line-height: 18px;
            height: 18px;
            border-right: solid 1px #cacaca;
            text-align: center;
            background-color: #eaeaea;
            text-decoration: none;
        }

        .beb_al /* comando rimuovi */
        {
            display: inline-block;
            width: 88px;
            line-height: 18px;
            height: 18px;
            text-align: center;
            background-color: #eaeaea;
            color: #15428b;
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <script>
        <asp:Literal ID="ltrScripts" runat="server" />
    </script>
    <div class="singlerow">
        <asp:Label ID="lblTitle" runat="server" EnableViewState="false" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" onclick="javascript:confirmClose();">
            <span class="icon close"></span>
            Annulla
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updDati" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hid_id_PARTECIPAZIONE" runat="server" />
            <asp:HiddenField ID="hid_ac_TIPOPARTECIPAZIONE" runat="server" />
            <asp:HiddenField ID="hid_fl_GIORNIORE" runat="server" />
            <div class="stl_dfo" style="width: 1040px;">
                <table class="fieldtable">
                    <tr id="trCreazioneModifica" runat="server">
                        <td class="labelcol">Creazione/Modifica</td>
                        <td class="datacol" style="width: 620px;">
                            <asp:Label ID="lblCreazioneModifica" runat="server" EnableViewState="true" />
                        </td>
                        <td class="errorcol">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Tipo Partecipazione</td>
                        <td class="datacol" style="width: 620px;">
                            <asp:TextBox ID="tx_TIPOPARTECIPAZIONE" runat="server" CssClass="txt" Width="560px" Enabled="false" />
                        </td>
                        <td class="errorcol">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">
                            <asp:Label ID="lblNumeroData" runat="server" />
                        </td>
                        <td class="datacol">
                            <asp:TextBox ID="ni_PARTECIPAZIONE" runat="server" CssClass="txt txtnarrow" Enabled="false" Font-Bold="true" />
                            <asp:Label ID="lblSpazioNumeroData" runat="server" />
                            <asp:TextBox ID="dt_PARTECIPAZIONE" runat="server" CssClass="txt txtdate stl_dt_data_ddmmyyyy" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errdt_PARTECIPAZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Partecipante</td>
                        <td class="datacol">
                            <span style="display: none;">
                                <asp:TextBox ID="id_PERSONA" runat="server" ClientIDMode="Static" />
                                <asp:TextBox ID="fl_PROFILOECM" runat="server" ClientIDMode="Static" />
                                <asp:LinkButton ID="lnkPickPersona" runat="server">Pick Persona</asp:LinkButton>
                            </span>
                            <asp:TextBox ID="tx_PERSONA" runat="server" CssClass="txt" Width="560px" ReadOnly="true" Font-Bold="true" ClientIDMode="Static" />
                            <img src="<% =Page.ResolveUrl("~/Img/icoLens.gif")%>" class="btnicon" title="Seleziona Persona" onclick="pickPartecipante();" />
                            <img src="<% =Page.ResolveUrl("~/Img/icoDelete.gif")%>" class="btnicon" title="Rimuovi Persona" onclick="clearPartecipante();" />
                            <asp:Panel ID="pnlTipoContrattoKo" runat="server" Visible="false">
                                <img src="<% =Page.ResolveUrl("~/Img/icoExclOrange.png")%>" style="vertical-align:top;margin-right:3px;margin-top:3px;" />
                                <span style="color:#ff6600;">Attenzione: il tipo di contratto del dipendente non è compatibile con la tipologia di partecipazione</span>
                            </asp:Panel>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_PERSONA" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <%If (tx_TIPOPARTECIPAZIONE.Text <> "PG57") Then%>
                    <tr>
                        <td class="labelcol">In servizio presso</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_UNITAOPERATIVA" runat="server" DataSourceID="sdsac_UNITAOPERATIVA"
                                            DataTextField="tx_UNITAOPERATIVA" DataValueField="ac_UNITAOPERATIVA" AppendDataBoundItems="true"
                                            Width="564px" CssClass="ddn" AutoPostBack="true" Enabled="false">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="err_ac_UNITAOPERATIVA" runat="server" EnableViewState="false"/>
                        </td>
                    </tr>
                    <%End If %>
                    <tr>
                        <td class="labelcol">Data inizio / fine</td>
                        <td class="datacol">
                            <asp:TextBox ID="dt_INIZIO" runat="server" CssClass="txt txtdate stl_dt_data_ddmmyyyy" AutoPostBack="True" />
                            -
                            <asp:TextBox ID="dt_FINE" runat="server" CssClass="txt txtdate stl_dt_data_ddmmyyyy" AutoPostBack="True" />
                            <asp:Panel ID="pnlPartParallele" runat="server" Visible="false">
                                <img src="<% =Page.ResolveUrl("~/Img/icoExclOrange.png")%>" style="vertical-align:top;margin-right:3px;margin-top:3px;" />
                                <span style="color:#ff6600;">Attenzione: la persona risulta iscritta ad uno o più eventi nelle date selezionate</span>
                            </asp:Panel>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errdt_INIZIOFINE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Dirigente che autorizza</td>
                        <td class="datacol">
                            <span style="display: none;">
                                <asp:TextBox ID="id_PERSONA_RESPONSABILE" runat="server" ClientIDMode="Static" /><br />
                                <asp:LinkButton ID="lnkPickResponsabile" runat="server">Pick Responsabile</asp:LinkButton>
                            </span>
                            <asp:TextBox ID="tx_PERSONA_RESPONSABILE" runat="server" CssClass="txt" Width="560px" ReadOnly="true" ClientIDMode="Static" />
                            <img src="<% =Page.ResolveUrl("~/Img/icoLens.gif")%>" class="btnicon" title="Seleziona Persona" onclick="pickResponsabile();" />
                            <img src="<% =Page.ResolveUrl("~/Img/icoDelete.gif")%>" class="btnicon" title="Rimuovi Persona" onclick="clearResponsabile();" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_PERSONA_RESPONSABILE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <%If (hid_ac_TIPOPARTECIPAZIONE.Value.ToUpper = "PG56D" Or hid_ac_TIPOPARTECIPAZIONE.Value.ToUpper = "PG56_D") Then%>
                    <tr>
                        <td class="labelcol">Responsabile</td>
                        <td class="datacol">
                            <asp:RadioButtonList runat="server" ID="ac_RESPONSABILE" RepeatDirection="Vertical"
                                                RepeatLayout="Flow" CssClass="labels">
                                                <asp:ListItem Value="SCENT"><b>Respons. Scientifico</b></asp:ListItem>
                                                <asp:ListItem Value="UOPER"><b>Respons. Unità Operativa</b></asp:ListItem>
                                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Progetto/attività</td>
                        <td class="datacol">
                            <asp:RadioButtonList runat="server" ID="ac_PROGETTOATTIVITA" RepeatDirection="Vertical"
                                                RepeatLayout="Flow" CssClass="labels">
                                                <asp:ListItem Value="PROGRIC"><b>Di progetto di ricerca</b></asp:ListItem>
                                                <asp:ListItem Value="ATTFIN"><b>Di altra attività finanziata</b></asp:ListItem>
                                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <%End If %>
                    <tr>
                        <td class="labelcol">Contratto</td>
                        <td class="datacol">
                            <asp:RadioButtonList ID="ac_CONTRATTO" runat="server" DataSourceID="sdsac_Contratto"
                                DataTextField="tx_CONTRATTO" DataValueField="ac_CONTRATTO"
                                RepeatDirection="Vertical"
                                    RepeatLayout="Flow" CssClass="labels">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Codice Progetto Ricerca / altra attività</td>
                        <td class="datacol">
                            <asp:TextBox ID="ac_CODICEPRAF" runat="server" ClientIDMode="Static" MaxLength="50" CssClass="txt" Width="560px" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_CODICEPRAF" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Codice CUP Progetto Ricerca</td>
                        <td class="datacol">
                            <asp:TextBox ID="ac_CODICECUP" runat="server" ClientIDMode="Static" MaxLength="50" CssClass="txt" Width="560px" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_CODICECUP" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Titolo del corso</td>
                        <td class="datacol">
                            <span style="display: none;">
                                <asp:TextBox ID="id_EVENTO" runat="server" ClientIDMode="Static" /><br />
                                <asp:LinkButton ID="lnkPickEvento" runat="server">Pick Evento</asp:LinkButton>
                            </span>
                            <asp:TextBox ID="tx_TITOLO" runat="server" ClientIDMode="Static" MaxLength="600" CssClass="txt" Width="560px" />
                            <img src="<% =Page.ResolveUrl("~/Img/icoLens.gif")%>" class="btnicon" title="Seleziona da inserimenti precedenti" onclick="pickEvento();" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_TITOLO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Tipologia</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_TIPOLOGIAEVENTO" runat="server" DataSourceID="sdsac_TIPOLOGIAEVENTO"
                                DataTextField="tx_TIPOLOGIAEVENTO" DataValueField="ac_TIPOLOGIAEVENTO" AppendDataBoundItems="true"
                                Width="564px" CssClass="ddn">
                                <asp:ListItem Text="" Value="" />
                            </asp:DropDownList>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_TIPOLOGIAEVENTO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Categoria (corsi obbligatori)</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_TIPOCOBDETT" runat="server" DataSourceID="sdsac_TIPOCOBDETT"
                                DataTextField="tx_TIPOCOBDETT" DataValueField="ac_TIPOCOBDETT" AppendDataBoundItems="true"
                                Width="564px" CssClass="ddn">
                                <asp:ListItem Text="" Value="" />
                            </asp:DropDownList>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_TIPOCOBDETT" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Città di svolgimento</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_SEDE" runat="server" ClientIDMode="Static" MaxLength="300" CssClass="txt" Width="560px" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_SEDE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Nazione di svolgimento (se all'estero)</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_NAZIONE" runat="server" DataSourceID="sdsac_NAZIONE"
                                DataTextField="tx_NAZIONE" DataValueField="ac_NAZIONE" AppendDataBoundItems="true"
                                Width="564px" CssClass="ddn">
                                <asp:ListItem Text="" Value="" />
                            </asp:DropDownList>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_NAZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Nome ente organizzatore</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_ORGANIZZATORE" runat="server" ClientIDMode="Static" MaxLength="300" CssClass="txt" Width="560px" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_ORGANIZZATORE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Note</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_NOTEDATE" runat="server" CssClass="txt" TextMode="MultiLine" Width="560px" Height="28px" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_NOTEDATE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr id="tr_GIORNIFORMAZIONE" runat="server">
                        <td class="labelcol">Giorni usufruiti (max 8 GG CCNL)</td>
                        <td class="datacol">
                            <asp:TextBox ID="nd_GIORNIFORMAZIONE" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errnd_GIORNIFORMAZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr id="tr_OREFORMAZIONE" runat="server">
                        <td class="labelcol">Ore di aggiornamento usufruite (dirigenza)</td>
                        <td class="datacol">
                            <asp:TextBox ID="nd_OREFORMAZIONE" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errnd_OREFORMAZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Costo quota di iscrizione (se presente)</td>
                        <td class="datacol">
                            <asp:TextBox ID="nd_QUOTAISCRIZIONE_PREV" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                            &nbsp;Valuta&nbsp;&nbsp;
                            <asp:DropDownList ID="nd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" DataSourceID="sdsnd_QUOTAISCRIZIONE_PREV_VALUTA"
                                DataTextField="tx_CURRENCY" DataValueField="ac_CURRENCY"
                                CssClass="ddn ddnnarrow" Enabled="true" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errnd_QUOTAISCRIZIONE_PREV" runat="server" EnableViewState="false" />
                            <asp:Label ID="errnd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Da pagarsi entro</td>
                        <td class="datacol">
                            <asp:TextBox ID="dt_PAGARSIENTRO" runat="server" CssClass="txt txtdate stl_dt_data_ddmmyyyy" />
                        </td>
                        <td class="errorcol">
                                <asp:Label ID="err_dt_PAGARSIENTRO" runat="server" EnableViewState="false" />
                            </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Si richiede anticipo della quota di iscrizione</td>
                        <td class="datacol">
                            <asp:CheckBox ID="fl_ANTICIPOQUOTAISCRIZIONE" runat="server" AutoPostBack="true" />
                        </td>
                        <td class="errorcol">&nbsp;
                        </td>
                    </tr>
                    <tr id="tr_CIGQUOTAISCRIZIONE" runat="server">
                        <td class="labelcol">CIG</td>
                        <td class="datacol">
                            <asp:TextBox ID="ac_CIGQUOTAISCRIZIONE" runat="server" CssClass="txt txtmedium" MaxLength="20" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_CIGQUOTAISCRIZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Giorni di viaggio (se richiesti)</td>
                        <td class="datacol">
                            <asp:TextBox ID="nd_GIORNIVIAGGIO" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errnd_GIORNIVIAGGIO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Costo presunto viaggio</td>
                        <td class="datacol">
                            <asp:TextBox ID="nd_COSTOVIAGGIO_PREV" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errnd_COSTOVIAGGIO_PREV" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    
                    <%If (tx_TIPOPARTECIPAZIONE.Text <> "PG57") Then%>
                    <tr>
                        <td class="labelcol">Centro di costo</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_CENTROCOSTO" runat="server" CssClass="txt txtnarrow" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Commessa</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_CENTROCOSTO_COMMESSA" runat="server" CssClass="txt txtnarrow" MaxLength="50" />
                        </td>
                    </tr>
                    
                    <%End If %>
                    <tr id="tr_EVENTOECM" runat="server">
                        <td class="labelcol">Corso accreditato ECM</td>
                        <td class="datacol">
                            <asp:CheckBox ID="fl_EVENTOECM" runat="server" AutoPostBack="true" />
                        </td>
                        <td class="errorcol">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">File allegato</td>
                        <td class="datacol">
                            <bof:UnboundBinaryElementBox ID="id_ELEME" runat="server" FieldName="id_ELEME"
                                DefaultCODCATEG="PDF_PAR"
                                DefaultDescriptionPreamble="Logo "
                                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" />
                        </td>
                        <td class="errorcol">&nbsp;
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlValdazione" runat="server">
                    <div style="padding-top: 5px;">
                        <div class="title">
                            Validazione
                        </div>
                    </div>
                    <table class="fieldtable">
                        <tr>
                            <td class="labelcol bordertop">Stato</td>
                            <td class="datacol bordertop" style="width: 620px;">
                                <asp:DropDownList ID="ac_STATOPARTECIPAZIONE" runat="server" DataSourceID="sdsac_STATOPARTECIPAZIONE"
                                    DataTextField="tx_STATOPARTECIPAZIONE" DataValueField="ac_STATOPARTECIPAZIONE"
                                    Width="564px" CssClass="ddn" Font-Bold="true" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td class="errorcol bordertop">
                                <asp:Label ID="Label1" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                    </table>
                    <table class="fieldtable" id="tblDatiConsuntivo" runat="server">
                        <tr>
                            <td class="labelcol">Consuntivo quota di iscrizione (se presente)</td>
                            <td class="datacol" style="width: 620px;">
                                <asp:TextBox ID="nd_QUOTAISCRIZIONE_CONS" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errnd_QUOTAISCRIZIONE_CONS" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Consuntivo costo viaggio (se presente)</td>
                            <td class="datacol">
                                <asp:TextBox ID="nd_COSTOVIAGGIO_CONS" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errnd_COSTOVIAGGIO_CONS" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr id="tr_CREDITIECM" runat="server">
                            <td class="labelcol">N° crediti ECM (se ottenuti)</td>
                            <td class="datacol">
                                <asp:TextBox ID="nd_CREDITIECM" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errnd_CREDITIECM" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr id="tr_OTTENIMENTOCREDITIECM" runat="server">
                            <td class="labelcol">Data conseg. crediti ECM (se ottenuti)</td>
                            <td class="datacol">
                                <asp:TextBox ID="dt_OTTENIMENTOCREDITIECM" runat="server" CssClass="txt txtdate stl_dt_data_ddmmyyyy" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errdt_OTTENIMENTOCREDITIECM" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <div style="padding-top: 10px; padding-bottom:15px;">
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="btnlink" Font-Size="14px" Font-Names="Arial" Font-Bold="true">Salva</asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sdsac_UNITAOPERATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_UnitaOperative"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_TIPOLOGIAEVENTO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPOLOGIAEVENTO, tx_TIPOLOGIAEVENTO FROM ext_TIPOLOGIEEVENTI WHERE fl_BACKOFFICE = 1 ORDER BY ac_TIPOLOGIAEVENTO desc"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_TIPOCOBDETT" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPOCOBDETT, tx_TIPOCOBDETT FROM vw_cob_TIPICORSODETT ORDER BY sort1, sort2 desc"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_NAZIONE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_NAZIONE, tx_NAZIONE FROM geo_NAZIONI WHERE fl_ATTUALE = 1 AND ac_NAZIONE <> 'Z000' ORDER BY tx_NAZIONE"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_STATOPARTECIPAZIONE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" SelectCommand="SELECT ac_STATOPARTECIPAZIONE + '\' + CASE fl_OK WHEN 0 THEN '0' ELSE '1' END as ac_STATOPARTECIPAZIONE, tx_STATOPARTECIPAZIONE + (CASE WHEN fl_BACKOFFICE=1 THEN '' ELSE (' (azione effettuata dal corsista)') END) AS tx_STATOPARTECIPAZIONE FROM ext_STATIPARTECIPAZIONI WHERE ac_STATOPARTECIPAZIONE <> 'FO_WAIT' AND ac_STATOPARTECIPAZIONE <> 'FO_DEL' ORDER BY ac_STATOPARTECIPAZIONE ASC"></asp:SqlDataSource>
     <asp:SqlDataSource ID="sdsac_CONTRATTO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="Text" SelectCommand="SELECT * FROM ext_TIPIPARTECIPAZIONE_CONTRATTO WHERE ac_TIPOPARTECIPAZIONE = @ac_TIPOPARTECIPAZIONE">
        <SelectParameters>
            <asp:ControlParameter Name="ac_TIPOPARTECIPAZIONE" Type="String" ControlID="hid_ac_TIPOPARTECIPAZIONE" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsnd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="Text" SelectCommand="SELECT code as ac_CURRENCY,CONCAT(code, ' (',symbol,') - ', name) as tx_CURRENCY FROM age_CURRENCY">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
