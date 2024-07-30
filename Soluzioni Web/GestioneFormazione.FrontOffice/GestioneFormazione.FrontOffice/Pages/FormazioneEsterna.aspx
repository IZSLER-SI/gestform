<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="FormazioneEsterna.aspx.vb" Inherits="GestioneFormazione.FrontOffice.FormazioneEsterna" %>



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

        function SubmitBtn_Click(id_evento) {
            $("#cphMiddle_cphContent_id_EVENTO_in").val(id_evento);
            document.getElementById("cphMiddle_cphContent_lnkOpenPreFilledForm").click();
        }


        function StampaBtn_Click(id_partecipazione) {
            $("#cphMiddle_cphContent_id_PARTECIPAZIONE_stampa").val(id_partecipazione);
            document.getElementById("cphMiddle_cphContent_lnkGeneraDoc").click();
        }

        function StampaModuloSponsor_Click(e) {
            console.log(e);
            e.preventDefault();
            document.getElementById("cphMiddle_cphContent_lnkModuloSponsor").click();
        }


    </script>
    <input style="display: none" type="text" name="fakeusernameremembered" />
    <input style="display: none" type="password" name="fakepasswordremembered" />
    <div style="clear: both"></div>
    <div class="onecol">
        <div style="display: none">
            <asp:LinkButton ID="lnkGeneraDoc" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">LINK MODULO</asp:LinkButton>
            <asp:LinkButton ID="lnkOpenPreFilledForm" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">LINK FITTIZIO</asp:LinkButton>
            <asp:LinkButton runat="server" ID="lnkModuloSponsor" CssClass="" Font-Bold="true">stampare il modulo cliccando qui</asp:LinkButton>
        </div>
        <div class="title green bottom20">
            Formazione Esterna
        </div>
        <div>
            Mediante questa pagina &egrave; possibile pre-compilare il modulo di richiesta di partecipazione ad un evento esterno.
            <br />
            <ol style="margin-top: 0px; margin-bottom: 0px;">
                <li>Si ricorda che, ai sensi dell'art. 76 del DPR 445/2000, le dichiarazioni mendaci, la falsit&agrave; negli atti e l'uso di atti falsi sono puniti ai sensi del codice penale e delle leggi speciali. </li>
                <li>Si ricorda al soggetto titolare dell'accesso al portale che inserendo i dati quale attivit&agrave; di formazione esterna, autorizza l'Istituto Zooprofilattico della Lombardia e dell'Emilia Romagna, al trattamento dei dati personali secondo quanto previsto dal Regolamento UE n. 679/2016 - GDPR General Data Protection Regulation, D.Lgs 196/2003 modificato da D.Lgs 101/2018.</li>
            </ol>
            Se si intende continuare occorre inserire nel modulo online i dati dell'evento formativo a cui si intende partecipare.<br />
            Per inserire una richiesta di partecipazione per un evento gi&agrave; registrato occorre utilizzare la maschera "Ricerca Eventi" e selezionare l'evento di interesse, se presente.
            <br />
            Qualora l'evento non sia presente in piattaforma &egrave necessario fare clic su "Nuovo Evento" e compilare il modulo.<br />
            Una volta salvate tutte le informazioni, il modulo pre-compilato sar&agrave; inviato tramite email.
            <br />
            Il modulo di partecipazione potr&agrave; inoltre essere riscaricato cliccando sulla voce "Download" dell'evento nella tabella "Richiesta in attesa di validazione".
        </div>

        <br />

        <div id="acf_popup_covering" style="display: none;"></div>
        <div id="acf_popup" style="display: none;">
            <asp:HiddenField ID="id_EVENTO_in" Value="0" runat="server" />
            <asp:UpdatePanel ID="updForm" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- controlli nascosti -->
                    <asp:HiddenField ID="id_EVENTO" runat="server" />
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
                            <br />
                            <div class="datagroup" style="width: 100%;">
                                <asp:HiddenField ID="tx_CATEGORIAECM" Value="" runat="server" />
                                <asp:HiddenField ID="tx_VOCATIVO" Value="" runat="server" />
                                <div class="row">
                                    <div class="label">
                                        <b>Dati personali</b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Nome *
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_NOME" runat="server" MaxLength="100" CssClass="txt txtwide" Enabled="false" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_tx_NOME" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Cognome *
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_COGNOME" runat="server" MaxLength="100" CssClass="txt txtwide" Enabled="false" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_tx_COGNOME" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Codice Fiscale *
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_CODICE_FISCALE" runat="server" MaxLength="100" CssClass="txt txtwide" Enabled="false" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_tx_CODICE_FISCALE" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Matricola *
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="ac_MATRICOLA" runat="server" MaxLength="100" CssClass="txt txtnarrow" Enabled="false" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_MATRICOLA" runat="server" EnableViewState="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="label">
                                        In servizio presso *
                                    </div>
                                    <div class="data">
                                        <asp:DropDownList ID="ac_UNITAOPERATIVA" runat="server" DataSourceID="sdsac_UNITAOPERATIVA"
                                            DataTextField="tx_UNITAOPERATIVA" DataValueField="ac_UNITAOPERATIVA" AppendDataBoundItems="true"
                                            CssClass="ddn ddnwide" Enabled="false" AutoPostBack="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_UNITAOPERATIVA" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Telefono interno
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_TELEFONO_INTERNO" runat="server" MaxLength="100" CssClass="txt txtnarrow" Enabled="true" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_tx_TELEFONO_INTERNO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        In qualit&agrave; di *
                                    </div>
                                    <div class="data">
                                        <asp:DropDownList ID="ac_PROFILO" runat="server" DataSourceID="sdsac_PROFILO"
                                            DataTextField="tx_PROFILO" DataValueField="ac_PROFILO" AppendDataBoundItems="false"
                                            CssClass="ddn ddnwide" Enabled="false" AutoPostBack="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_PROFILO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                    </div>
                                    <div class="data">
                                        <asp:DropDownList ID="ac_RUOLO" runat="server" DataSourceID="sdsac_RUOLO"
                                            DataTextField="tx_RUOLO" DataValueField="ac_RUOLO" AppendDataBoundItems="true"
                                            CssClass="ddn ddnwide" Enabled="false" AutoPostBack="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                    </div>

                                    <div class="error">
                                        <asp:Label ID="err_ac_RUOLO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>

                          
                                <br />
                                <div class="row">
                                    <div class="label">
                                        Indirizzo e-mail *
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_EMAIL" runat="server" MaxLength="150" CssClass="txt txtwide" Enabled="false" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_tx_EMAIL" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="label">
                                        <b>Dati evento</b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Titolo *
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
                                        Tipologia *
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
                                            Data inizio fruizione *
                                            <div class="expl">
                                                gg/mm/aaaa
                                            </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_INIZIOFRUIZIONE" runat="server" MaxLength="10" CssClass="txt txtdate" AutoPostBack="true" />
                                            <ajaxToolkit:CalendarExtender ID="caldt_INIZIOFRUIZIONE" runat="server" TargetControlID="dt_INIZIOFRUIZIONE"
                                                ClearTime="true" Format="dd/MM/yyyy" />
                                            <ajaxToolkit:MaskedEditExtender ID="medt_INIZIOFRUIZIONE" runat="server" TargetControlID="dt_INIZIOFRUIZIONE"
                                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_INIZIOFRUIZIONE" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Data fine fruizione *
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_FINEFRUIZIONE" runat="server" MaxLength="10" CssClass="txt txtdate" />
                                            <ajaxToolkit:CalendarExtender ID="caldt_FINEFRUIZIONE" runat="server" TargetControlID="dt_FINEFRUIZIONE"
                                                ClearTime="true" Format="dd/MM/yyyy" />
                                            <ajaxToolkit:MaskedEditExtender ID="medt_FINEFRUIZIONE" runat="server" TargetControlID="dt_FINEFRUIZIONE"
                                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_FINEFRUIZIONE" runat="server" EnableViewState="false" />
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
                                                Citt&agrave; e nazione *
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
                                            Data inizio *
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_INIZIO" runat="server" MaxLength="10" CssClass="txt txtdate" AutoPostBack="true" />
                                            <ajaxToolkit:CalendarExtender ID="caldt_INIZIO" runat="server" TargetControlID="dt_INIZIO"
                                                ClearTime="true" Format="dd/MM/yyyy" />
                                            <ajaxToolkit:MaskedEditExtender ID="meddt_INIZIO" runat="server" TargetControlID="dt_INIZIO"
                                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_INIZIO" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="label" style="width: 180px;">
                                            Data fine *
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                        </div>
                                        <div class="data">
                                            <asp:TextBox ID="dt_FINE" runat="server" MaxLength="10" CssClass="txt txtdate" />
                                            <ajaxToolkit:CalendarExtender ID="caldt_FINE" runat="server" TargetControlID="dt_FINE"
                                                ClearTime="true" Format="dd/MM/yyyy" />
                                            <ajaxToolkit:MaskedEditExtender ID="meddt_FINE" runat="server" TargetControlID="dt_FINE"
                                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                                        </div>
                                        <div class="error">
                                            <asp:Label ID="err_dt_FINE" runat="server" EnableViewState="false" />
                                        </div>
                                    </div>
                                   
                                </div>
                            </asp:Panel>
                            <br />
                            <div class="datagroup" style="width: 100%;">
                                
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Nome ente organizzatore *
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_ORGANIZZATORE" runat="server" MaxLength="600" CssClass="txt txtwide" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="error_tx_ORGANIZZATORE" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Accreditamento ECM *
                                    </div>
                                    <div class="data" style="width: 477px; padding-bottom: 5px;">
                                        <asp:RadioButtonList ID="ac_NORMATIVAECM" runat="server">
                                            <asp:ListItem Text="Evento accreditato ECM" Value="2011" />
                                            <asp:ListItem Text="Evento non accreditato ECM" Value="NONE" />
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_NORMATIVAECM" runat="server" EnableViewState="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="label">
                                        <b>Dati partecipazione</b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Ruolo *
                                    </div>
                                    <div class="data" style="width: 477px; padding-bottom: 5px;">
                                        <asp:RadioButtonList ID="ac_CATEGORIAECM" runat="server"
                                            DataSourceID="sdsac_CATEGORIAECM"
                                            DataTextField="tx_CATEGORIAECM"
                                            DataValueField="ac_CATEGORIAECM"
                                            AppendDataBoundItems="false">
                                        </asp:RadioButtonList>
                                        <small><i>Ruolo Docente/Relatore non selezionabile - "Istituto giuridico ex art.53 (rivolgersi a UOC Politiche e gestione delle Risorse Umane)"</i></small>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_CATEGORIAECM" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Tipo autorizzazione *
                                    </div>
                                    <div class="data">
                                        <asp:RadioButtonList runat="server" ID="ac_TIPOPARTECIPAZIONE" RepeatDirection="Vertical"
                                            RepeatLayout="Flow" CssClass="labels" AutoPostBack="true">
                                            <asp:ListItem Value="PG56_B"><b>Partecipazione a corso obbligatorio esterno</b></asp:ListItem>
                                            <asp:ListItem Value="PG56_C"><b>Partecipazione a corso facoltativo esterno</b></asp:ListItem>
                                            <asp:ListItem Value="PG56_D"><b>Partecipazione a corso con oneri a carico di progetti di ricerca o altra attivit&agrave; finanziata da terzi</b></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_TIPOPARTECIPAZIONE" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <br />

                                <%If (ac_TIPOPARTECIPAZIONE.SelectedIndex <> -1) Then%>
                                <div class="row">
                                    <div class="label">
                                        Dirigente che autorizza *
                                    </div>
                                    <div class="data">
                                        <asp:DropDownList ID="DropDownListDirigenti" runat="server" DataSourceID="sdsac_Dirigente"
                                            DataTextField="tx_NOMINATIVO" DataValueField="id_PERSONA"
                                            CssClass="ddn ddnwide" Enabled="true" AppendDataBoundItems="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_DropDownListDirigenti" runat="server" EnableViewState="false" />
                                    </div>
                                </div>

                                 <%If (ac_TIPOPARTECIPAZIONE.SelectedValue = "PG56_D") Then%>
                                <div class="row">
                                    <div class="label">Gestore dello specifico piano di spesa *</div>
                                    <div class="data">
                                        <asp:RadioButtonList runat="server" ID="ac_RESPONSABILE" RepeatDirection="Vertical"
                                            RepeatLayout="Flow" CssClass="labels">
                                            <asp:ListItem Value="SCENT"><b>Respons. Scientifico</b></asp:ListItem>
                                            <asp:ListItem Value="UOPER"><b>Respons. Unit&agrave; Operativa</b></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                     <div class="error">
                                        <asp:Label ID="err_ac_RESPONSABILE" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="label">Del progetto-ricerca/altra-attivit&agrave;-finanziata *</div>
                                    <div class="data">
                                        <asp:RadioButtonList runat="server" ID="ac_PROGETTOATTIVITA" RepeatDirection="Vertical"
                                            RepeatLayout="Flow" CssClass="labels"  AutoPostBack="true">
                                            <asp:ListItem Value="PROGRIC"><b>Di progetto di ricerca</b></asp:ListItem>
                                            <asp:ListItem Value="ATTFIN"><b>Di altra attivit&agrave; finanziata</b></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_PROGETTOATTIVITA" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <br />
                                <div class="row" id="container_ac_CODICEPR" runat="server" visible ="False">
                                    <div class="label">Codice progetto-ricerca *</div>
                                    <div class="data">
                                        <asp:TextBox ID="ac_CODICEPR" runat="server" MaxLength="50" CssClass="txt txtnarrow" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errac_CODICEPR" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row" id="container_ac_CODICEAF" runat="server" visible ="False">
                                    <div class="label">Codice altra-attivit&agrave;-finanziata</div>
                                    <div class="data">
                                        <asp:TextBox ID="ac_CODICEAF" runat="server" MaxLength="50" CssClass="txt txtnarrow" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errac_CODICEAF" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row" id ="container_ac_CODICECUP" runat="server" visible ="False">
                                    <div class="label">Codice CUP * (tutti i dati del progetto si trovano in intranet - "applicazioni sisinfo" - tabella progetti attivi)</div>
                                    <div class="data">
                                        <asp:TextBox ID="ac_CODICECUP" runat="server" MaxLength="50" CssClass="txt txtnarrow" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errac_CODICECUP" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                
                                <%End If %>

                                <br />
                                <div class="row">
                                    <div class="label">Contratto *</div>
                                    <div class="data">
                                        <asp:RadioButtonList ID="ac_CONTRATTO" runat="server" DataSourceID="sdsac_Contratto"
                                            DataTextField="tx_CONTRATTO" DataValueField="ac_CONTRATTO"
                                            RepeatDirection="Vertical"
                                                RepeatLayout="Flow" CssClass="labels">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_ac_CONTRATTO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <br />
                               
                                <div class="row">
                                    <div class="label">Centro di costo *</div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_CENTROCOSTO" runat="server" CssClass="txt txtnarrow" MaxLength="50" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errtx_CENTROCOSTO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="label">Centro di costo commessa</div>
                                    <div class="data">
                                        <asp:TextBox ID="tx_CENTROCOSTO_COMMESSA" runat="server" CssClass="txt txtnarrow" MaxLength="50" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errtx_CENTROCOSTO_COMMESSA" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="label">
                                        Costo quota di iscrizione (se presente)
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="nd_QUOTAISCRIZIONE_PREV" runat="server" CssClass="txt txtnarrow" MaxLength="10" Autopostback="true" />
                                         &nbsp;Valuta&nbsp;&nbsp;
                                        <asp:DropDownList ID="nd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" DataSourceID="sdsnd_QUOTAISCRIZIONE_PREV_VALUTA"
                                            DataTextField="tx_CURRENCY" DataValueField="ac_CURRENCY"
                                            CssClass="ddn ddnnarrow" Enabled="true" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errnd_QUOTAISCRIZIONE_PREV" runat="server" EnableViewState="false" />
                                    </div>
                                     <div class="error">
                                        <asp:Label ID="errnd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Quota da pagarsi entro
                                        <div class="expl">
                                            gg/mm/aaaa
                                        </div>
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="dt_PAGARSIENTRO" runat="server" MaxLength="10" CssClass="txt txtdate" Enabled = "False" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dt_PAGARSIENTRO"
                                            ClearTime="true" Format="dd/MM/yyyy" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="dt_PAGARSIENTRO"
                                            Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                                        <asp:hiddenfield id="fl_dt_PAGARSIENTRO_filled" value="false" runat="server"/>
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errdt_PAGARSIENTRO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>

                                <%If (pnlDatiNonFAD.Visible) Then%>
                                <div class="row">
                                    <div class="label" style="width: 180px;">
                                        Numero giorni viaggio
                                    </div>
                                    <div class="data">
                                        <asp:TextBox ID="nd_GIORNIVIAGGIO" runat="server" type="number" /><br />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="err_nd_GIORNIVIAGGIO" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">Costo presunto viaggio</div>
                                    <div class="data">
                                        <asp:TextBox ID="nd_COSTOVIAGGIO_PREV" runat="server" CssClass="txt txtnarrow" MaxLength="10" /> &euro;
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errnd_COSTOVIAGGIO_PREV" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <%End If %>

                                <%If (ac_TIPOPARTECIPAZIONE.SelectedValue = "PG56_C") Then%>
                                <div class="row" id="tr_GIORNIFORMAZIONE">
                                    <div class="label">Giorni usufruiti (max 8 GG CCNL)</div>
                                    <div class="data">
                                        <asp:TextBox ID="nd_GIORNIFORMAZIONE" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errnd_GIORNIFORMAZIONE" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <div class="row" id="tr_OREFORMAZIONE">
                                    <div class="label">Ore di aggiornamento usufruite (dirigenza)</div>
                                    <div class="data">
                                        <asp:TextBox ID="nd_OREFORMAZIONE" runat="server" CssClass="txt txtnarrow" MaxLength="10" />
                                    </div>
                                    <div class="error">
                                        <asp:Label ID="errnd_OREFORMAZIONE" runat="server" EnableViewState="false" />
                                    </div>
                                </div>
                                <%End If %>

                               


                                <%End If %>

                                
                            </div>
            

                            <div class="top10">
                                <asp:LinkButton runat="server" ID="lnkNext1" CssClass="btnlink btnlink_blue" Font-Bold="true">Avanti</asp:LinkButton>
                                &nbsp;
                                <span class="btnlink btnlink_blue" onclick="AskCloseAutocertificazionePopup();">Annulla</span>
                            </div>
                            <br />


                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlVerifyData">
                            <div class="steptitle">
                                Passo 2 di 3: Verifica Dati Immessi
                            </div>
                            <div class="riepgroup">
                                <div class="row">
                                    <div class="label">
                                        <b>Dati Evento</b>
                                    </div>
                                    <div class="value"></div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Titolo
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
                                
                                <div class="row">
                                    <div class="label">
                                        Accreditamento ECM
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_NORMATIVAECM" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Nome ente organizzatore
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_ORGANIZZATORE" runat="server" />
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
                                    <div class="row" style="display: none">
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
                                    
                                    <div class="row" style="display: none">
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
                                    <div class="label">
                                        <b>Dati Partecipazione</b>
                                    </div>
                                    <div class="value"></div>
                                </div>
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
                                        Tipo autorizzazione
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_TIPOPARTECIPAZIONE" runat="server" />
                                    </div>
                                </div>
                                <div class="row" enabled="false" id="row_r_tx_NOMINATIVO" runat="server">
                                    <div class="label">
                                        Dirigente che autorizza
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_NOMINATIVO" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_ac_RESPONSABILE" runat="server">
                                    <div class="label">
                                        Gestore dello specifico piano di spesa
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_RESPONSABILE" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_ac_PROGETTOATTIVITA" runat="server">
                                    <div class="label">
                                        Del progetto-ricerca/altra-attivit&agrave;-finanziata
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_PROGETTOATTIVITA" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_ac_CODICEPRAF" runat="server">
                                    <div class="label">
                                        Codice progetto-ricerca/altra-attivit&agrave;-finanziata
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_CODICEPRAF" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_ac_CODICECUP" runat="server">
                                    <div class="label">
                                        Codice CUP
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_CODICECUP" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_ac_CONTRATTO" runat="server">
                                    <div class="label">
                                        Contratto
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_ac_CONTRATTO" runat="server" />
                                    </div>
                                </div>


                                <div class="row" enabled="false" id="row_r_tx_CENTROCOSTO" runat="server">
                                    <div class="label">
                                        Centro di costo
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_CENTROCOSTO" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_tx_CENTROCOSTO_COMMESSA" runat="server">
                                    <div class="label">
                                        Commessa centro di costo
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_CENTROCOSTO_COMMESSA" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_nd_QUOTAISCRIZIONE_PREV" runat="server">
                                    <div class="label">
                                        Costo quota di iscrizione
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_nd_QUOTAISCRIZIONE_PREV" runat="server" />&nbsp;<asp:Label ID="r_nd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_dt_PAGARSIENTRO" runat="server">
                                    <div class="label">
                                        Quota da pagarsi entro
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_dt_PAGARSIENTRO" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_nd_GIORNIVIAGGIO" runat="server">
                                    <div class="label">
                                        Numero giorni viaggio
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_nd_GIORNIVIAGGIO" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_nd_COSTOVIAGGIO_PREV" runat="server">
                                    <div class="label">
                                        Costo presunto viaggio
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_nd_COSTOVIAGGIO_PREV" runat="server" />&nbsp;&euro;
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_nd_GIORNIFORMAZIONE" runat="server">
                                    <div class="label">
                                        Giorni usufruiti (max 8 GG CCNL)
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_nd_GIORNIFORMAZIONE" runat="server" />
                                    </div>
                                </div>

                                <div class="row" enabled="false" id="row_r_nd_OREFORMAZIONE" runat="server">
                                    <div class="label">
                                        Ore di aggiornamento usufruite (dirigenza)
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_nd_OREFORMAZIONE" runat="server" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="label">
                                        <b>Dati personali integrativi</b>
                                    </div>
                                    <div class="value"></div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Telefono interno
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_TELEFONO_INTERNO" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="label">
                                        Indirizzo e-mail
                                    </div>
                                    <div class="value">
                                        <asp:Label ID="r_tx_EMAIL" runat="server" />
                                    </div>
                                </div>


                            </div>

                            <div class="riepgroup" style="display: none">
                                <div class="row">
                                    <div class="label" style="border-top-width: 0px;">
                                        Esame di verifica
                                    </div>
                                    <div class="value" style="border-top-width: 0px;">
                                        <asp:Label ID="r_tx_STATOVERIFICAAPPRENDIMENTO" runat="server" />
                                    </div>
                                </div>
                            </div>
                           

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
                                Passo 3 di 3: Modulo di richiesta inserito
                            </div>
                            <div class="top20">
                                <div>
                                    <asp:Label ID="lblPrintIstruzioni" runat="server" />
                                    <br />                               
                                    Dopo l'approvazione del dirigente con firma digitale visibile (PADES) occorre inviare per mail all'Ufficio Formazione all'indirizzo <a href="mailto:formazione.esterna@izsler.it">formazione.esterna@izsler.it</a>: l'autorizzazione firmata, il programma dell'evento e ogni altro documento utile.
                                 </div>
                            </div>
                            <div class="top20">
                                <asp:HyperLink ID="lnkStampaAutocertificazione" runat="server" CssClass="btnlink btnlink_orange" Target="_blank" Font-Bold="true">Download Modulo di partecipazione</asp:HyperLink>
                            </div>
                            <div class="top20">
                                <span class="btnlink btnlink_blue" onclick="showAutocertificazionePopup(false);">Chiudi</span>
                            </div>

                        </asp:Panel>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkOpenPreFilledForm" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div style="clear: both"></div>


    <div class="twocol_left">
        <asp:UpdatePanel style="display: block" ID="updEventi" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdEventi" runat="server" EnableViewState="true" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkCerca" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkPulisci" EventName="Click" />

            </Triggers>
        </asp:UpdatePanel>

    </div>
    <div class="onecol">
        <asp:UpdatePanel style="display: block" ID="UpdatePanelFilter" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="twocol_right" id="updSearchEventi">
                    <div class="title blue">
                        RICERCA EVENTI
                    </div>
                    <div class="datagroup top20" style="width: 100%">
                        <div class="row" style="height: 28px;">
                            <div class="label">
                                Tipo evento
                            </div>
                            <div class="data right" style="font-size: 12px">
                                <asp:RadioButtonList ID="acTIPOLOGIAEVENTO" runat="server"
                                    DataSourceID="sdsac_TIPOLOGIAEVENTO"
                                    DataTextField="tx_TIPOLOGIAEVENTO"
                                    DataValueField="ac_TIPOLOGIAEVENTO"
                                    AppendDataBoundItems="false">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <br />
                        <div class="row" style="height: 28px;">
                            <div class="label">
                                Sede
                            </div>
                            <div class="data right">
                                <asp:TextBox ID="txtSede" runat="server" CssClass="txt" Width="206px" />
                            </div>
                        </div>
                        <div class="row" style="height: 28px;">
                            <div class="label">
                                Organizzatore
                            </div>
                            <div class="data right">
                                <asp:TextBox ID="txtOrganizzatore" runat="server" CssClass="txt" Width="206px" />
                            </div>
                        </div>
                        <div class="row" style="height: 28px;">
                            <div class="label">
                                Titolo (contiene)
                            </div>
                            <div class="data right">
                                <asp:TextBox ID="txtTitolo" runat="server" CssClass="txt" Width="206px" />
                            </div>
                        </div>
                        <asp:HiddenField ID="fl_filter" Value="0" runat="server" />
                    </div>
                    <div style="font-size: 14px; padding-top: 10px;">
                        <asp:LinkButton ID="lnkCerca" runat="server" CssClass="btnlink btnlink_blue" Font-Bold="true">Cerca</asp:LinkButton>
                        &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkPulisci" runat="server" CssClass="btnlink btnlink_blue" Font-Bold="true">Pulisci</asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkCerca" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkPulisci" EventName="Click" />

            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div style="clear: both"></div>
    <div class="onecol">
        <asp:HiddenField ID="id_PARTECIPAZIONE_stampa" runat="server" />
        <asp:UpdatePanel style="display: block" ID="updPending" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdPending" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="clear: both"></div>



    <asp:SqlDataSource ID="sdsac_CATEGORIAECM" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_ext_CategoriaEcm"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_TIPOLOGIAEVENTO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_ext_TipologiaEvento"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_STATOVERIFICAAPPRENDIMENTO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_ext_StatiVerificaApprendimento"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_RUOLO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_Ruoli"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_PROFILO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="Text" SelectCommand="SELECT * FROM age_PROFILI WHERE ac_RUOLO = @ac_RUOLO">
        <SelectParameters>
            <asp:ControlParameter Name="ac_RUOLO" Type="String" ControlID="ac_RUOLO" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_UNITAOPERATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_UnitaOperative"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_UnitaOperative"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_Dirigente" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="Text" SelectCommand="SELECT * FROM vw_age_PERSONE_grid WHERE dt_DECESSO IS NULL AND ac_MATRICOLA is not null"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_CONTRATTO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="Text" SelectCommand="SELECT * FROM ext_TIPIPARTECIPAZIONE_CONTRATTO WHERE ac_TIPOPARTECIPAZIONE = @ac_TIPOPARTECIPAZIONE">
        <SelectParameters>
            <asp:ControlParameter Name="ac_TIPOPARTECIPAZIONE" Type="String" ControlID="ac_TIPOPARTECIPAZIONE" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsnd_QUOTAISCRIZIONE_PREV_VALUTA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="Text" SelectCommand="SELECT code as ac_CURRENCY,CONCAT(code, ' (',symbol,') - ', name) as tx_CURRENCY FROM age_CURRENCY">
    </asp:SqlDataSource>

</asp:Content>
