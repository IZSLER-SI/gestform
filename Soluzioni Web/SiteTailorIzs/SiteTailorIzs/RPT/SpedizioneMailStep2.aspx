<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="SpedizioneMailStep2.aspx.vb" Inherits="Softailor.SiteTailorIzs.SpedizioneMailStep2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <link href="<%=Page.ResolveUrl("~/App_Themes/SiteTailorCustom/SiteTailor_CustomGrid.css")%>" rel="stylesheet" />
    <style type="text/css">
        .selezcb input
        {
            margin: 0px;
            padding: 0px;
        }
        .previewimg
        {
            cursor:pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <script src="<%=Page.ResolveUrl("~/Scripts/StlWebUI_CustomGrids.js")%>"></script>
    <script src="SpedizioneMailStep2.js"></script>
    <div class="tworows_1">
        Spedizione e-mail personalizzate
    </div>
    <div class="tworows_2">
        <b>Passo 2: selezione destinatari, anteprima, spedizione effettiva</b>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a ID="lnkClose" class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Annulla
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <div style="display:none;">
        <asp:UpdatePanel ID="updDatiGenerazione" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_ac_FONTE" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_id_MAILREPORT" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_VALOREFILTROBASE" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_xm_FILTRO" TextMode="MultiLine" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_ac_ORDINAMENTO" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_ORDINAMENTO1" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_ORDINAMENTO2" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_ORDINAMENTO3" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_ORDINAMENTO4" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_ORDINAMENTO5" ClientIDMode="Static" />

                <asp:TextBox runat="server" EnableViewState="false" ID="sd_ragionesociale" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_indirizzocompleto" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tel" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_fax" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_email" ClientIDMode="Static" />

                <asp:TextBox runat="server" EnableViewState="false" ID="sd_tx_OGGETTO" ClientIDMode="Static" />
                <asp:TextBox runat="server" EnableViewState="false" ID="sd_ht_CORPO" TextMode="MultiLine" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <div class="stl_cus_upg" style="width: 1068px; height: 605px; position: absolute; top: 15px; left: 15px;">
        <asp:UpdatePanel ID="updLista" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdLista" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="position:absolute;top:630px;left:15px;width:300px;font-size:11px;">
        <span class="btnlink" onclick="SelAll();">Seleziona tutti</span>
        <span class="btnlink" onclick="SelNone();">Deseleziona tutti</span>

        
    </div>
    <div style="position:absolute;top:680px;left:15px;font-family:Arial;font-weight:bold;width:300px;font-size:16px;">
        <asp:UpdatePanel ID="updComandi" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <a class="btnlink" style="font-weight:normal;" onclick="PrevStep();">&lt; Indietro</a>
                <asp:LinkButton ID="lnkSpedizione" runat="server" CssClass="btnlink" OnClientClick="return(ConfirmSpedizione());">Spedisci</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phdOutOfForm" runat="server">
    <form method="post" action="SpedizioneMailStep1.aspx" id="dataform" style="display:none;">
        <input type="text" id="sd3_ac_FONTE" name="sd3_ac_FONTE" />
        <input type="text" id="sd3_id_MAILREPORT" name="sd3_id_MAILREPORT" />
        <input type="text" id="sd3_tx_VALOREFILTROBASE" name="sd3_tx_VALOREFILTROBASE" />
        <textarea id="sd3_xm_FILTRO" name="sd3_xm_FILTRO"></textarea>
        <input type="text" id="sd3_ac_ORDINAMENTO" name="sd3_ac_ORDINAMENTO" />
        <input type="text" id="sd3_tx_ORDINAMENTO1" name="sd3_tx_ORDINAMENTO1" />
        <input type="text" id="sd3_tx_ORDINAMENTO2" name="sd3_tx_ORDINAMENTO2" />
        <input type="text" id="sd3_tx_ORDINAMENTO3" name="sd3_tx_ORDINAMENTO3" />
        <input type="text" id="sd3_tx_ORDINAMENTO4" name="sd3_tx_ORDINAMENTO4" />
        <input type="text" id="sd3_tx_ORDINAMENTO5" name="sd3_tx_ORDINAMENTO5" />

        <input type="text" id="sd3_ragionesociale" name="sd3_ragionesociale" />
        <input type="text" id="sd3_indirizzocompleto" name="sd3_indirizzocompleto" />
        <input type="text" id="sd3_tel" name="sd3_tel" />
        <input type="text" id="sd3_fax" name="sd3_fax" />
        <input type="text" id="sd3_email" name="sd3_email" />

        <input type="text" id="sd3_tx_OGGETTO" name="sd3_tx_OGGETTO" />
        <textarea id="sd3_ht_CORPO" name="sd3_ht_CORPO"></textarea>
    </form>

    <form method="post" action="AnteprimaMail.aspx" target="stl_popup_iframe" id="previewform" style="display:none;">
        <input type="text" id="sd4_ac_FONTE" name="sd4_ac_FONTE" />
        <input type="text" id="sd4_tx_VALOREFILTROBASE" name="sd4_tx_VALOREFILTROBASE" />
        <input type="text" id="sd4_tx_OGGETTO" name="sd4_tx_OGGETTO" />
        <textarea id="sd4_ht_CORPO" name="sd4_ht_CORPO"></textarea>
        <input type="text" id="sd4_ac_KEY" name="sd4_ac_KEY" />
        <input type="text" id="sd4_ragionesociale" name="sd4_ragionesociale" />
        <input type="text" id="sd4_indirizzocompleto" name="sd4_indirizzocompleto" />
        <input type="text" id="sd4_tel" name="sd4_tel" />
        <input type="text" id="sd4_fax" name="sd4_fax" />
        <input type="text" id="sd4_email" name="sd4_email" />
    </form>

</asp:Content>
