<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="FormazioneIndividuale.aspx.vb" Inherits="GestioneFormazione.FrontOffice.FormazioneIndividuale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">

        function showAutocertificazionePopup(show) {
            if (show) {
                $("#acf_popup_covering").fadeIn(0, function () {
                    $("#acf_popup").fadeIn(0);
                });
            }
            else {
                $("#acf_popup").fadeOut(250, function () {
                    $("#acf_popup_covering").fadeOut(100);
                });
            }
        }
        function AskCloseAutocertificazionePopup() {
            if (window.confirm("Confermi l'abbandono di eventuali dati inseriti?"))
                showAutocertificazionePopup(false);
        }
    </script>
    <div class="onecol">
        <div class="title green bottom20">
            Formazione Individuale
        </div>
        <div>
            Mediante questa pagina puoi auto-certificare la tua partecipazione ad eventi formativi
             effettuata per iniziativa personale.<br />
            Il processo di auto-certificazione prevede i seguenti passi:
             <ol style="margin-top: 0px; margin-bottom: 0px;">
                 <li>La compilazione di un modulo con i dati dell'evento formativo ai quali hai partecipato</li>
                 <li>La stampa di un'auto-certificazione contenente i dati inseriti</li>
                 <li>L'invio dell'auto-certificazione, firmata e datata, all'Ufficio Formazione</li>
                 <li>La validazione dell'auto-certificazione da parte dell'Ufficio Formazione</li>
             </ol>
            Per caricare una nuova auto-certificazione fai clic su "Nuova auto-certificazione".<br />
            Nella sezione "Auto-certificazioni in attesa di validazione" sono elencate tutte le
            auto-certificazioni che l'Ufficio Formazione deve ancora validare.<br />
            Tutte le auto-certificazioni (in attesa di validazione, validate e non validate) compaiono inoltre nella pagina "Portfolio Formativo".
        </div>
        <div class="top20">
            <asp:UpdatePanel ID="updOpen" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="lnkOpenEmptyForm" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">Carica nuova auto-certificazione</asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel ID="updPending" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdPending" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="acf_popup_covering" style="display: none;"></div>
        <div id="acf_popup" style="display: none;">
            <asp:UpdatePanel ID="updForm" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- controlli nascosti -->
                    <asp:HiddenField ID="id_PARTECIPAZIONE_in" runat="server" />
                    <asp:HiddenField ID="id_PARTECIPAZIONE" runat="server" />
                    <asp:HiddenField ID="ni_ANNO" runat="server" />
                    <asp:HiddenField ID="ni_NUMERO" runat="server" />

                    <div class="acf_popup_header">
                        <div class="left green">
                            <asp:Label ID="lblPopupTitle" runat="server" />
                        </div>
                        <div class="right">
                            &nbsp;
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div class="acf_popup_form">

                        <!-- FORM DATI -->
                        <asp:Panel runat="server" ID="pnlDataEntry">

                            <div class="steptitle">
                                <asp:Label ID="lblStep1Title" runat="server" />
                            </div>
                            <div class="datagroup" style="width: 100%;">

                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Ruolo
                                    </div>
                                    <div class="data" style="width: 477px; padding-bottom: 5px;">
                                        <asp:RadioButtonList ID="ac_CATEGORIAECM" runat="server"
                                            DataSourceID="sdsac_CATEGORIAECM"
                                            DataTextField="tx_CATEGORIAECM"
                                            DataValueField="ac_CATEGORIAECM"
                                            AppendDataBoundItems="false">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_CATEGORIAECM" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Titolo dell'evento
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_TITOLO" runat="server" MaxLength="600" CssClass="txt txtwide" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_tx_TITOLO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Tipologia
                                    </div>
                                    <div class="data" style="padding-bottom: 5px;">
                                        <asp:RadioButtonList ID="ac_TIPOLOGIAEVENTO" runat="server"
                                            DataSourceID="sdsac_TIPOLOGIAEVENTO"
                                            DataTextField="tx_TIPOLOGIAEVENTO"
                                            DataValueField="ac_TIPOLOGIAEVENTO"
                                            AppendDataBoundItems="false"
                                            AutoPostBack="true">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_TIPOLOGIAEVENTO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="pnlDatiFAD" runat="server">
                                <div class="datagroup" style="width: 100%;">
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Data inizio fruizione
                                            <div class="expl">
                                                gg/mm/aaaa
                                            </div>
                                        </div>
                                        <div class="data" style="width: 477px;">
                                            <asp:TextBox ID="dt_INIZIOFRUIZIONE" runat="server" MaxLength="10" CssClass="txt txtnarrow" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_INIZIOFRUIZIONE" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Data fine fruizione
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_FINEFRUIZIONE" runat="server" MaxLength="10" CssClass="txt txtnarrow" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_FINEFRUIZIONE" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Durata totale
                                        </div>
                                        <div class="data">
                                            <asp:DropDownList ID="ni_ORE_FAD" runat="server" CssClass="ddn">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="1" Value="1" />
                                                <asp:ListItem Text="2" Value="2" />
                                                <asp:ListItem Text="3" Value="3" />
                                                <asp:ListItem Text="4" Value="4" />
                                                <asp:ListItem Text="5" Value="5" />
                                                <asp:ListItem Text="6" Value="6" />
                                                <asp:ListItem Text="7" Value="7" />
                                                <asp:ListItem Text="8" Value="8" />
                                                <asp:ListItem Text="9" Value="9" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="11" Value="11" />
                                                <asp:ListItem Text="12" Value="12" />
                                                <asp:ListItem Text="13" Value="13" />
                                                <asp:ListItem Text="14" Value="14" />
                                                <asp:ListItem Text="15" Value="15" />
                                                <asp:ListItem Text="16" Value="16" />
                                                <asp:ListItem Text="17" Value="17" />
                                                <asp:ListItem Text="18" Value="18" />
                                                <asp:ListItem Text="19" Value="19" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="21" Value="21" />
                                                <asp:ListItem Text="22" Value="22" />
                                                <asp:ListItem Text="23" Value="23" />
                                                <asp:ListItem Text="24" Value="24" />
                                                <asp:ListItem Text="25" Value="25" />
                                                <asp:ListItem Text="26" Value="26" />
                                                <asp:ListItem Text="27" Value="27" />
                                                <asp:ListItem Text="28" Value="28" />
                                                <asp:ListItem Text="29" Value="29" />
                                                <asp:ListItem Text="30" Value="30" />
                                                <asp:ListItem Text="31" Value="31" />
                                                <asp:ListItem Text="32" Value="32" />
                                                <asp:ListItem Text="33" Value="33" />
                                                <asp:ListItem Text="34" Value="34" />
                                                <asp:ListItem Text="35" Value="35" />
                                                <asp:ListItem Text="36" Value="36" />
                                                <asp:ListItem Text="37" Value="37" />
                                                <asp:ListItem Text="38" Value="38" />
                                                <asp:ListItem Text="39" Value="39" />
                                                <asp:ListItem Text="40" Value="40" />
                                                <asp:ListItem Text="41" Value="41" />
                                                <asp:ListItem Text="42" Value="42" />
                                                <asp:ListItem Text="43" Value="43" />
                                                <asp:ListItem Text="44" Value="44" />
                                                <asp:ListItem Text="45" Value="45" />
                                                <asp:ListItem Text="46" Value="46" />
                                                <asp:ListItem Text="47" Value="47" />
                                                <asp:ListItem Text="48" Value="48" />
                                                <asp:ListItem Text="49" Value="49" />
                                                <asp:ListItem Text="50" Value="50" />
                                                <asp:ListItem Text="51" Value="51" />
                                                <asp:ListItem Text="52" Value="52" />
                                                <asp:ListItem Text="53" Value="53" />
                                                <asp:ListItem Text="54" Value="54" />
                                                <asp:ListItem Text="55" Value="55" />
                                                <asp:ListItem Text="56" Value="56" />
                                                <asp:ListItem Text="57" Value="57" />
                                                <asp:ListItem Text="58" Value="58" />
                                                <asp:ListItem Text="59" Value="59" />
                                                <asp:ListItem Text="60" Value="60" />
                                                <asp:ListItem Text="61" Value="61" />
                                                <asp:ListItem Text="62" Value="62" />
                                                <asp:ListItem Text="63" Value="63" />
                                                <asp:ListItem Text="64" Value="64" />
                                                <asp:ListItem Text="65" Value="65" />
                                                <asp:ListItem Text="66" Value="66" />
                                                <asp:ListItem Text="67" Value="67" />
                                                <asp:ListItem Text="68" Value="68" />
                                                <asp:ListItem Text="69" Value="69" />
                                                <asp:ListItem Text="70" Value="70" />
                                                <asp:ListItem Text="71" Value="71" />
                                                <asp:ListItem Text="72" Value="72" />
                                                <asp:ListItem Text="73" Value="73" />
                                                <asp:ListItem Text="74" Value="74" />
                                                <asp:ListItem Text="75" Value="75" />
                                                <asp:ListItem Text="76" Value="76" />
                                                <asp:ListItem Text="77" Value="77" />
                                                <asp:ListItem Text="78" Value="78" />
                                                <asp:ListItem Text="79" Value="79" />
                                                <asp:ListItem Text="80" Value="80" />
                                                <asp:ListItem Text="81" Value="81" />
                                                <asp:ListItem Text="82" Value="82" />
                                                <asp:ListItem Text="83" Value="83" />
                                                <asp:ListItem Text="84" Value="84" />
                                                <asp:ListItem Text="85" Value="85" />
                                                <asp:ListItem Text="86" Value="86" />
                                                <asp:ListItem Text="87" Value="87" />
                                                <asp:ListItem Text="88" Value="88" />
                                                <asp:ListItem Text="89" Value="89" />
                                                <asp:ListItem Text="90" Value="90" />
                                                <asp:ListItem Text="91" Value="91" />
                                                <asp:ListItem Text="92" Value="92" />
                                                <asp:ListItem Text="93" Value="93" />
                                                <asp:ListItem Text="94" Value="94" />
                                                <asp:ListItem Text="95" Value="95" />
                                                <asp:ListItem Text="96" Value="96" />
                                                <asp:ListItem Text="97" Value="97" />
                                                <asp:ListItem Text="98" Value="98" />
                                                <asp:ListItem Text="99" Value="99" />
                                                <asp:ListItem Text="100" Value="100" />
                                            </asp:DropDownList>
                                            ore
                                            <asp:DropDownList ID="ni_MINUTI_FAD" runat="server" CssClass="ddn">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="00" Value="0" />
                                                <asp:ListItem Text="15" Value="15" />
                                                <asp:ListItem Text="30" Value="30" />
                                                <asp:ListItem Text="45" Value="45" />
                                            </asp:DropDownList>
                                            minuti
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_ni_ORE_FAD" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlDatiNonFAD" runat="server">
                                <div class="datagroup" style="width: 100%;">
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Sede
                                            <div class="expl">
                                                Città e nazione
                                            </div>
                                        </div>
                                        <div class="data" style="width: 477px;">
                                            <asp:TextBox ID="tx_SEDE" runat="server" MaxLength="300" CssClass="txt txtwide" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_tx_SEDE" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Data inizio
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_INIZIO" runat="server" MaxLength="10" CssClass="txt txtnarrow" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_INIZIO" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Data fine
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_FINE" runat="server" MaxLength="10" CssClass="txt txtnarrow" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_FINE" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Durata totale
                                        </div>
                                        <div class="data">
                                            <asp:DropDownList ID="ni_ORE_RES" runat="server" CssClass="ddn">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="0" Value="0" />
                                                <asp:ListItem Text="1" Value="1" />
                                                <asp:ListItem Text="2" Value="2" />
                                                <asp:ListItem Text="3" Value="3" />
                                                <asp:ListItem Text="4" Value="4" />
                                                <asp:ListItem Text="5" Value="5" />
                                                <asp:ListItem Text="6" Value="6" />
                                                <asp:ListItem Text="7" Value="7" />
                                                <asp:ListItem Text="8" Value="8" />
                                                <asp:ListItem Text="9" Value="9" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="11" Value="11" />
                                                <asp:ListItem Text="12" Value="12" />
                                                <asp:ListItem Text="13" Value="13" />
                                                <asp:ListItem Text="14" Value="14" />
                                                <asp:ListItem Text="15" Value="15" />
                                                <asp:ListItem Text="16" Value="16" />
                                                <asp:ListItem Text="17" Value="17" />
                                                <asp:ListItem Text="18" Value="18" />
                                                <asp:ListItem Text="19" Value="19" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="21" Value="21" />
                                                <asp:ListItem Text="22" Value="22" />
                                                <asp:ListItem Text="23" Value="23" />
                                                <asp:ListItem Text="24" Value="24" />
                                                <asp:ListItem Text="25" Value="25" />
                                                <asp:ListItem Text="26" Value="26" />
                                                <asp:ListItem Text="27" Value="27" />
                                                <asp:ListItem Text="28" Value="28" />
                                                <asp:ListItem Text="29" Value="29" />
                                                <asp:ListItem Text="30" Value="30" />
                                                <asp:ListItem Text="31" Value="31" />
                                                <asp:ListItem Text="32" Value="32" />
                                                <asp:ListItem Text="33" Value="33" />
                                                <asp:ListItem Text="34" Value="34" />
                                                <asp:ListItem Text="35" Value="35" />
                                                <asp:ListItem Text="36" Value="36" />
                                                <asp:ListItem Text="37" Value="37" />
                                                <asp:ListItem Text="38" Value="38" />
                                                <asp:ListItem Text="39" Value="39" />
                                                <asp:ListItem Text="40" Value="40" />
                                                <asp:ListItem Text="41" Value="41" />
                                                <asp:ListItem Text="42" Value="42" />
                                                <asp:ListItem Text="43" Value="43" />
                                                <asp:ListItem Text="44" Value="44" />
                                                <asp:ListItem Text="45" Value="45" />
                                                <asp:ListItem Text="46" Value="46" />
                                                <asp:ListItem Text="47" Value="47" />
                                                <asp:ListItem Text="48" Value="48" />
                                                <asp:ListItem Text="49" Value="49" />
                                                <asp:ListItem Text="50" Value="50" />
                                                <asp:ListItem Text="51" Value="51" />
                                                <asp:ListItem Text="52" Value="52" />
                                                <asp:ListItem Text="53" Value="53" />
                                                <asp:ListItem Text="54" Value="54" />
                                                <asp:ListItem Text="55" Value="55" />
                                                <asp:ListItem Text="56" Value="56" />
                                                <asp:ListItem Text="57" Value="57" />
                                                <asp:ListItem Text="58" Value="58" />
                                                <asp:ListItem Text="59" Value="59" />
                                                <asp:ListItem Text="60" Value="60" />
                                                <asp:ListItem Text="61" Value="61" />
                                                <asp:ListItem Text="62" Value="62" />
                                                <asp:ListItem Text="63" Value="63" />
                                                <asp:ListItem Text="64" Value="64" />
                                                <asp:ListItem Text="65" Value="65" />
                                                <asp:ListItem Text="66" Value="66" />
                                                <asp:ListItem Text="67" Value="67" />
                                                <asp:ListItem Text="68" Value="68" />
                                                <asp:ListItem Text="69" Value="69" />
                                                <asp:ListItem Text="70" Value="70" />
                                                <asp:ListItem Text="71" Value="71" />
                                                <asp:ListItem Text="72" Value="72" />
                                                <asp:ListItem Text="73" Value="73" />
                                                <asp:ListItem Text="74" Value="74" />
                                                <asp:ListItem Text="75" Value="75" />
                                                <asp:ListItem Text="76" Value="76" />
                                                <asp:ListItem Text="77" Value="77" />
                                                <asp:ListItem Text="78" Value="78" />
                                                <asp:ListItem Text="79" Value="79" />
                                                <asp:ListItem Text="80" Value="80" />
                                                <asp:ListItem Text="81" Value="81" />
                                                <asp:ListItem Text="82" Value="82" />
                                                <asp:ListItem Text="83" Value="83" />
                                                <asp:ListItem Text="84" Value="84" />
                                                <asp:ListItem Text="85" Value="85" />
                                                <asp:ListItem Text="86" Value="86" />
                                                <asp:ListItem Text="87" Value="87" />
                                                <asp:ListItem Text="88" Value="88" />
                                                <asp:ListItem Text="89" Value="89" />
                                                <asp:ListItem Text="90" Value="90" />
                                                <asp:ListItem Text="91" Value="91" />
                                                <asp:ListItem Text="92" Value="92" />
                                                <asp:ListItem Text="93" Value="93" />
                                                <asp:ListItem Text="94" Value="94" />
                                                <asp:ListItem Text="95" Value="95" />
                                                <asp:ListItem Text="96" Value="96" />
                                                <asp:ListItem Text="97" Value="97" />
                                                <asp:ListItem Text="98" Value="98" />
                                                <asp:ListItem Text="99" Value="99" />
                                                <asp:ListItem Text="100" Value="100" />
                                            </asp:DropDownList>
                                            ore
                                            <asp:DropDownList ID="ni_MINUTI_RES" runat="server" CssClass="ddn">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="00" Value="0" />
                                                <asp:ListItem Text="05" Value="5" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="15" Value="15" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="25" Value="25" />
                                                <asp:ListItem Text="30" Value="30" />
                                                <asp:ListItem Text="35" Value="35" />
                                                <asp:ListItem Text="40" Value="40" />
                                                <asp:ListItem Text="45" Value="45" />
                                                <asp:ListItem Text="50" Value="50" />
                                                <asp:ListItem Text="55" Value="55" />
                                            </asp:DropDownList>
                                            minuti
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_ni_ORE_RES" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <div class="datagroup" style="width: 100%;">
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Esame di verifica
                                    </div>
                                    <div class="data" style="width: 477px; padding-bottom: 5px;">
                                        <asp:RadioButtonList ID="ac_STATOVERIFICAAPPRENDIMENTO" runat="server"
                                            DataSourceID="sdsac_STATOVERIFICAAPPRENDIMENTO"
                                            DataTextField="tx_STATOVERIFICAAPPRENDIMENTO"
                                            DataValueField="ac_STATOVERIFICAAPPRENDIMENTO"
                                            AppendDataBoundItems="false">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_STATOVERIFICAAPPRENDIMENTO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlDatiEcm" runat="server">
                                <div class="datagroup" style="width: 100%;">
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Accreditamento ECM
                                        </div>
                                        <div class="data" style="width: 477px; padding-bottom: 5px;">
                                            <asp:RadioButtonList ID="ac_NORMATIVAECM" runat="server" AutoPostBack="true">
                                                <asp:ListItem Text="Evento accreditato ECM" Value="2011" />
                                                <asp:ListItem Text="Evento non accreditato ECM" Value="NONE" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_ac_NORMATIVAECM" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <asp:Panel ID="pnlCreditiConseguiti" runat="server" CssClass="row">
                                        <div class="label" style="width: 180px;">
                                            Crediti ECM conseguiti
                                        </div>
                                        <div class="data" style="padding-bottom: 5px;">
                                            <asp:RadioButtonList ID="ac_STATOECM" runat="server" AutoPostBack="true">
                                                <asp:ListItem Text="Sì" Value="COK" />
                                                <asp:ListItem Text="No" Value="CKO" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_ac_STATOECM" runat="server" EnableViewState="false" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlNumeroCrediti" runat="server" CssClass="row">
                                        <div class="label" style="width: 180px;">
                                            Numero crediti conseguiti
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="nd_CREDITIECM" runat="server" MaxLength="8" CssClass="txt txtnarrow" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_nd_CREDITIECM" runat="server" EnableViewState="false" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDataCrediti" runat="server" CssClass="row">
                                        <div class="label" style="width: 180px;">
                                            Data conseguimento crediti
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_OTTENIMENTOCREDITIECM" runat="server" MaxLength="10" CssClass="txt txtnarrow" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_OTTENIMENTOCREDITIECM" runat="server" EnableViewState="false" />
                                        </div>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                            <div class="top10">
                                <asp:LinkButton runat="server" ID="lnkNext1" CssClass="btnlink btnlink_blue" Font-Bold="true">Avanti</asp:LinkButton>
                                &nbsp;
                                <span class="btnlink btnlink_blue" onclick="AskCloseAutocertificazionePopup();">Annulla</span>
                            </div>


                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnlVerifyData">
                            <div class="steptitle">
                                Passo 2 di 3: Verifica Dati Immessi
                            </div>
                            <div class="riepgroup">
                                <div class="row">
                                    <div class="label">
                                        Ruolo
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_CATEGORIAECM" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Titolo dell'evento
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_TITOLO" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Tipologia
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_TIPOLOGIAEVENTO" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="r_pnlDatiFAD" runat="server">
                                <div class="riepgroup">
                                    <div class="row">
                                        <div class="label" style="border-top-width: 0px;">
                                            Data inizio fruizione
                                        </div>
                                        <div class="value" style="border-top-width: 0px;">
                                            <asp:Label ID="r_dt_INIZIOFRUIZIONE" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label">
                                            Data fine fruizione
                                        </div>
                                        <div class="value">
                                            <asp:Label ID="r_dt_FINEFRUIZIONE" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label">
                                            Durata totale (HH:MM)
                                        </div>
                                        <div class="value">
                                            <asp:Label ID="r_ni_MINUTI_FAD" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="r_pnlDatiNonFAD" runat="server">
                                <div class="riepgroup">
                                    <div class="row">
                                        <div class="label" style="border-top-width: 0px;">
                                            Sede
                                        </div>
                                        <div class="value" style="border-top-width: 0px;">
                                            <asp:Label ID="r_tx_SEDE" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label">
                                            Data inizio
                                        </div>
                                        <div class="value">
                                            <asp:Label ID="r_dt_INIZIO" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label">
                                            Data fine
                                        </div>
                                        <div class="value">
                                            <asp:Label ID="r_dt_FINE" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label">
                                            Durata totale (HH:MM)
                                        </div>
                                        <div class="value">
                                            <asp:Label ID="r_ni_MINUTI_RES" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="riepgroup">
                                <div class="row">
                                    <div class="label" style="border-top-width: 0px;">
                                        Esame di verifica
                                    </div>
                                    <div class="value" style="border-top-width: 0px;">
                                        <asp:Label ID="r_tx_STATOVERIFICAAPPRENDIMENTO" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="r_pnlDatiEcm" runat="server">
                                <div class="riepgroup">
                                    <div class="row">
                                        <div class="label" style="border-top-width: 0px;">
                                            Accreditamento ECM
                                        </div>
                                        <div class="value" style="border-top-width: 0px;">
                                            <asp:Label ID="r_tx_NORMATIVAECM" runat="server" />
                                        </div>
                                    </div>
                                    <asp:Panel ID="r_pnlCreditiConseguiti" CssClass="row" runat="server">
                                        <div class="label" style="border-top-width: 0px;">
                                            Crediti ECM conseguiti
                                        </div>
                                        <div class="value" style="border-top-width: 0px;">
                                            <asp:Label ID="r_tx_STATOECM" runat="server" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="r_pnlNumeroCrediti" CssClass="row" runat="server">
                                        <div class="label" style="border-top-width: 0px;">
                                            Numero crediti conseguiti
                                        </div>
                                        <div class="value" style="border-top-width: 0px;">
                                            <asp:Label ID="r_nd_CREDITIECM" runat="server" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="r_pnlDataCrediti" CssClass="row" runat="server">
                                        <div class="label" style="border-top-width: 0px;">
                                            Data conseguimento crediti
                                        </div>
                                        <div class="value" style="border-top-width: 0px;">
                                            <asp:Label ID="r_dt_OTTENIMENTOCREDITIECM" runat="server" />
                                        </div>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>

                            <div class="top20">
                                <asp:LinkButton runat="server" ID="lnkNext2" CssClass="btnlink btnlink_blue" Font-Bold="true" OnClientClick="return(confirm('Sei sicuro/a?'));">Conferma dati immessi</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton runat="server" ID="lnkPrevious2" CssClass="btnlink btnlink_blue">Correggi dati</asp:LinkButton>
                                &nbsp;
                                <span class="btnlink btnlink_blue" onclick="AskCloseAutocertificazionePopup();">Annulla</span>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlPrintForm">
                            <div class="steptitle">
                                Passo 3 di 3: Stampa e firma auto-certificazione
                            </div>
                            <div class="top20">
                                <div>
                                    <asp:Label ID="lblPrintIstruzioni" runat="server" />
                                </div>
                                <div>
                                    <br />
                                </div>
                                <div>Per proseguire:</div>
                                <ol>
                                    <li><b>Stampa una copia</b> dell'autocertificazione cliccando sul pulsante "Stampa Auto-certificazione" qui sotto</li>
                                    <li><b>Firma</b> la pagina stampata</li>
                                    <li><b>Invia</b> l'auto-certificazione firmata all'Ufficio Formazione</li>
                                </ol>
                            </div>
                            <div class="top20">
                                <asp:HyperLink ID="lnkStampaAutocertificazione" runat="server" CssClass="btnlink btnlink_orange" Target="_blank" Font-Bold="true">Stampa Auto-certificazione</asp:HyperLink>
                            </div>
                            <div class="top20">
                                <span class="btnlink btnlink_blue" onclick="showAutocertificazionePopup(false);">Chiudi</span>
                            </div>



                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <asp:SqlDataSource ID="sdsac_CATEGORIAECM" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_ext_CategoriaEcm"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_TIPOLOGIAEVENTO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_ext_TipologiaEvento"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_STATOVERIFICAAPPRENDIMENTO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_ext_StatiVerificaApprendimento"></asp:SqlDataSource>
</asp:Content>
