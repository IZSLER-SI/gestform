<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="SpedizioneMailStep1.aspx.vb" Inherits="Softailor.SiteTailorIzs.SpedizioneMailStep1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        #campifonte
        {
            font-family:Calibri, Arial;
            width:448px;
            height:648px;
            border:1px solid #888888;
            overflow:scroll;
            white-space:nowrap;
        }
        #campifonte .title
        {
            display:none;
        }
        #campifonte .section
        {
            font-size:18px;
            font-weight:bold;
            padding:2px 10px 2px 10px;
            margin-bottom:2px;
        }
        #campifonte .campo
        {
            font-size:12px;
            line-height:14px;
            padding:5px 10px 5px 10px;
        }
            #campifonte .campo .d
            {
                font-weight:bold;
                padding:2px 10px 2px 10px;
                border:1px solid #336699;
                border-radius:6px;
                background-color:#e0e0e0;
                cursor:pointer;
            }
            #campifonte .campo .d:hover
            {
                background-color:#fade42;
            }
            #campifonte .campo .p
            {
                font-size:12px;
                padding-left:5px;
            }
    </style>
    <script type="text/javascript">
        function CopyValue(v) {
            clipboardData.setData("Text", v);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <script type="text/javascript">
        function NextStep() 
        {
            //copio tutti i dati
            $("#sd2_ac_FONTE").val($("#sd_ac_FONTE").val());
            $("#sd2_id_MAILREPORT").val($("#sd_id_MAILREPORT").val());
            $("#sd2_tx_VALOREFILTROBASE").val($("#sd_tx_VALOREFILTROBASE").val());
            $("#sd2_xm_FILTRO").val($("#sd_xm_FILTRO").val());
            $("#sd2_ac_ORDINAMENTO").val($("#sd_ac_ORDINAMENTO").val());
            $("#sd2_tx_ORDINAMENTO1").val($("#sd_tx_ORDINAMENTO1").val());
            $("#sd2_tx_ORDINAMENTO2").val($("#sd_tx_ORDINAMENTO2").val());
            $("#sd2_tx_ORDINAMENTO3").val($("#sd_tx_ORDINAMENTO3").val());
            $("#sd2_tx_ORDINAMENTO4").val($("#sd_tx_ORDINAMENTO4").val());
            $("#sd2_tx_ORDINAMENTO5").val($("#sd_tx_ORDINAMENTO5").val());

            $("#sd2_ragionesociale").val($("#sd_ragionesociale").val());
            $("#sd2_indirizzocompleto").val($("#sd_indirizzocompleto").val());
            $("#sd2_tel").val($("#sd_tel").val());
            $("#sd2_fax").val($("#sd_fax").val());
            $("#sd2_email").val($("#sd_email").val());

            $("#sd2_tx_OGGETTO").val($("#sd_tx_OGGETTO").val());
            $("#sd2_ht_CORPO").val($("#sd_ht_CORPO").val());

            $("#dataform").submit();
        }

    </script>
    <div class="tworows_1">
        Spedizione e-mail personalizzate
    </div>
    <div class="tworows_2">
        <b>Passo 1: eventuale personalizzazione del modello</b> - Nota: eventuali modifiche non verranno salvate nel modello origine
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
    <div>
        <div style="display:block;float:left;width:619px;height:650px;">
            <asp:UpdatePanel ID="updModello" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="height:72px;font-size:18px;line-height:23px;font-family:Calibri, Arial">
                        <b>Oggetto</b><br />
                        <asp:TextBox ID="tx_OGGETTO" runat="server" CssClass="txt" Width="596px" MaxLength="300" Font-Bold="true" /><br />
                        <b>Corpo del messaggio</b>
                    </div>
                    <CKEditor:CKEditorControl ID="ht_CORPO" runat="server" EnableViewState="true" Width="598px" Height="549px" Toolbar="Email" ResizeEnabled="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="display:block;float:left;width:450px;height:650px;margin-top:1px;">
            <asp:UpdatePanel ID="updElencoCampi" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="campifonte">
                        <asp:PlaceHolder ID="phdElencoCampi" runat="server" EnableViewState="false" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="clear"></div>
    </div>
    <div style="padding-top:13px;font-size:16px;font-family:Arial;font-weight:bold;">
        <asp:UpdatePanel ID="updComandi" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="lnkProsegui" runat="server" CssClass="btnlink">Prosegui &gt;</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phdOutOfForm" runat="server">
    <form method="post" action="SpedizioneMailStep2.aspx" id="dataform" style="display:none;">
        <input type="text" id="sd2_ac_FONTE" name="sd2_ac_FONTE" />
        <input type="text" id="sd2_id_MAILREPORT" name="sd2_id_MAILREPORT" />
        <input type="text" id="sd2_tx_VALOREFILTROBASE" name="sd2_tx_VALOREFILTROBASE" />
        <textarea id="sd2_xm_FILTRO" name="sd2_xm_FILTRO"></textarea>
        <input type="text" id="sd2_ac_ORDINAMENTO" name="sd2_ac_ORDINAMENTO" />
        <input type="text" id="sd2_tx_ORDINAMENTO1" name="sd2_tx_ORDINAMENTO1" />
        <input type="text" id="sd2_tx_ORDINAMENTO2" name="sd2_tx_ORDINAMENTO2" />
        <input type="text" id="sd2_tx_ORDINAMENTO3" name="sd2_tx_ORDINAMENTO3" />
        <input type="text" id="sd2_tx_ORDINAMENTO4" name="sd2_tx_ORDINAMENTO4" />
        <input type="text" id="sd2_tx_ORDINAMENTO5" name="sd2_tx_ORDINAMENTO5" />

        <input type="text" id="sd2_ragionesociale" name="sd2_ragionesociale" />
        <input type="text" id="sd2_indirizzocompleto" name="sd2_indirizzocompleto" />
        <input type="text" id="sd2_tel" name="sd2_tel" />
        <input type="text" id="sd2_fax" name="sd2_fax" />
        <input type="text" id="sd2_email" name="sd2_email" />

        <input type="text" id="sd2_tx_OGGETTO" name="sd2_tx_OGGETTO" />
        <textarea id="sd2_ht_CORPO" name="sd2_ht_CORPO"></textarea>
    </form>
</asp:Content>