<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="DuplicaEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.DuplicaEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        function undoCreation() {
            if (window.confirm("Confermi l'abbandono dei dati inseriti?")) {
                location.href = "HomeEvento.aspx";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="stl_dfo" style="width: 850px;">
                <div class="title">
                    Duplicazione Evento Evento
                </div>
                <table class="fieldtable">
                    <tr>
                        <td class="labelcol">Origine: titolo</td>
                        <td class="datacol" style="font-weight:bold;">
                            <asp:Label ID="lbltx_TITOLO" runat="server" EnableViewState="false" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Titolo</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_TITOLO" runat="server" CssClass="txt txtwide" MaxLength="600" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_TITOLO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Edizione</td>
                        <td class="datacol">
                            <asp:Label ID="lbltx_EDIZIONE" runat="server" EnableViewState="false" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Edizione</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_EDIZIONE" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_EDIZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Sede</td>
                        <td class="datacol">
                            <asp:Label ID="lbltx_SEDE" runat="server" EnableViewState="false" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Sede</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_SEDE" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_SEDE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Dettagli sede (reparto, aula...)</td>
                        <td class="datacol">
                            <asp:Label ID="lbltx_DETTAGLISEDE" runat="server" EnableViewState="false" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Dettagli sede (reparto, aula...)</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_DETTAGLISEDE" runat="server" CssClass="txt txtwide" MaxLength="200" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_DETTAGLISEDE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Data Inizio</td>
                        <td class="datacol">
                            <asp:Label ID="lbldt_INIZIO" runat="server" EnableViewState="false" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Data Fine</td>
                        <td class="datacol">
                            <asp:Label ID="lbldt_FINE" runat="server" EnableViewState="false" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Data Inizio</td>
                        <td class="datacol">
                            <asp:TextBox ID="dt_INIZIO" runat="server" CssClass="txt txtdate" />
                            <ajaxToolkit:CalendarExtender ID="caldt_INIZIO" runat="server" TargetControlID="dt_INIZIO"
                                ClearTime="true" Format="dd/MM/yyyy" />
                            <ajaxToolkit:MaskedEditExtender ID="meddt_INIZIO" runat="server" TargetControlID="dt_INIZIO"
                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                            <ajaxToolkit:MaskedEditValidator runat="server" ID="mevdt_INIZIO" ControlToValidate="dt_INIZIO"
                                ControlExtender="meddt_INIZIO" Display="dynamic" IsValidEmpty="False" InvalidValueMessage="*" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errdt_INIZIO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Data fine</td>
                        <td class="datacol">
                            <asp:TextBox ID="dt_FINE" runat="server" CssClass="txt txtdate" />
                            <ajaxToolkit:CalendarExtender ID="caldt_FINE" runat="server" TargetControlID="dt_FINE"
                                ClearTime="true" Format="dd/MM/yyyy" />
                            <ajaxToolkit:MaskedEditExtender ID="meddt_FINE" runat="server" TargetControlID="dt_FINE"
                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear"/>
                            <ajaxToolkit:MaskedEditValidator runat="server" ID="mevdt_FINE" ControlToValidate="dt_FINE"
                                ControlExtender="meddt_FINE" Display="dynamic" IsValidEmpty="False" InvalidValueMessage="*" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errdt_FINE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Piano Formativo</td>
                        <td class="datacol">
                            <asp:Label ID="lbltx_PIANOFORMATIVO" runat="server" EnableViewState="False" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Piano Formativo</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_PIANOFORMATIVO" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_PIANOFORMATIVO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Origine: Aggiunto al Piano Formativo</td>
                        <td class="datacol">
                            <asp:CheckBox ID="orig_fl_NUOVOINPF" runat="server" Text="" Enabled="false" />Evento 
                            aggiunto in seguito al piano formativo
                        </td>
                        <td class="errorcol">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="labelcol">Destinazione: Aggiunto al Piano Formativo</td>
                        <td class="datacol">
                            <asp:CheckBox ID="fl_NUOVOINPF" runat="server" Text="Evento aggiunto in seguito al piano formativo" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errfl_NUOVOINPF" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Opzioni di copia</td>
                        <td class="datacol">
                            <asp:CheckBox ID="fl_COPIASPESERICAVIEVENTO" runat="server" Text="Copia spese e ricavi a livello di evento" />
                            <br />
                            <asp:CheckBox ID="fl_COPIASPESERICAVIPERSONE" runat="server" Text="Copia spese e ricavi a livello di docente/tutor/relatore" />
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Informazioni sui dati che<br /> vengono o non vengono duplicati</td>
                        <td class="datacol" style="line-height:normal">
                            <ul style="margin:0px;padding-left:20px;">
                                <li>
                                    <b>Intestazione evento e dati ECM</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Vengono copiati tutti i dati tranne:</li>
                                    <ul style="margin:0px;padding-left:20px;">
                                        <li>Data di acquisizione dei crediti (che compare sugli attestati ECM)</li>
                                        <li>Luogo e data che compare negli attestati ECM prima della firma</li>
                                        <li>Struttura questionario di apprendimento (numero domande, risposte esatte)</li>
                                        <li>Testi personalizzati nelle mail di accettazione / non accettazione / promemoria partecipazione</li>
                                    </ul>
                                </ul>
                                <li>
                                    <b>Quote di iscrizione (anagrafica)</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Se presente, l'anagrafica viene copiata</li>
                                </ul>
                                <li>
                                    <b>Spese e ricavi a livello di evento (se richiesto)</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Vengono copiati sia gli importi a preventivo sia gli importi a consuntivo.</li>
                                    <li>I seguenti dati vengono lasciati in bianco anche se presenti nell'evento di origine:</li>
                                    <ul style="margin:0px;padding-left:20px;">
                                        <li>Data di pagamento</li>
                                        <li>Metodo di pagamento</li>
                                        <li>Numero di protocollo</li>
                                        <li>Data protocollo</li>
                                    </ul>
                                </ul>
                                <li>
                                    <b>Calendario svolgimento</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Il calendario viene copiato.</li>
                                    <li>Le date vengono traslate in base alle date del nuovo evento.</li>
                                    <li>Se la sede del nuovo evento è differente dalla sede dell'evento di origine, non vengono replicate le aule utilizzate.</li>
                                </ul>
                                <li>
                                    <b>Programma / Materiale didattico</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Il programma scientifico e/o il materiale didattico NON vengono copiati.</li>
                                </ul>
                                <li>
                                    <b>Criteri per l'accesso all'evento</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Il numero massimo di iscritti viene copiato.</li>
                                    <li>I criteri per l'accesso vengono copiati.</li>
                                    <li>NON vengono copiate:</li>
                                    <ul style="margin:0px;padding-left:20px;">
                                        <li>L'eventuale data di inizio visibilità evento sul portale pubblico</li>
                                        <li>Le eventuali date di apertura e chiusura iscrizioni on line</li>
                                    </ul>
                                </ul>
                                <li>
                                    <b>Docenti, Tutor, Moderatori, Responsabili Scientifici</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Eventuali docenti/tutor/RS/etc contrassegnati come annullati non vengono copiati</li>
                                    <li>Le persone contrassegnate come presenti, assenti giustificati, assenti ingiustificati vengono registrate come iscritte</li>
                                    <li>I non candidati al conseguimento dei crediti nell'evento di origine vengono mantenuti tali nell'evento copiato</li>
                                    <li>I candidati al conseguimento dei crediti e coloro che hanno e che non hanno conseguito i crediti nell'evento di origine vengono contrassegnati come candidati al conseguimento dei crediti nell'evento copiato</li>
                                    <li>Il numero di protocollo e la data del protocollo, anche se presenti, non vengono copiati</li>
                                </ul>
                                <li>
                                    <b>Spese e ricavi a livello di docente/tutor/relatore (se richiesto)</b>
                                </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>Vengono copiati sia gli importi a preventivo sia gli importi a consuntivo.</li>
                                    <li>I seguenti dati vengono lasciati in bianco anche se presenti nell'evento di origine:</li>
                                    <ul style="margin:0px;padding-left:20px;">
                                        <li>Data di pagamento</li>
                                        <li>Metodo di pagamento</li>
                                    </ul>
                                </ul>
                                <li><b>Persone autorizzate all&#39;inserimento delle presenze dal sito pubblico</b> </li>
                                <ul style="margin:0px;padding-left:20px;">
                                    <li>L&#39;elenco delle persone autorizzate all&#39;inserimento delle presenze dal sito pubblico NON viene copiato.</li>
                                </ul>
                                </li>
                            </ul>
                        </td>
                        <td class="errorcol">
                        </td>
                    </tr>
                </table>
                <div class="endCommands">
                    <asp:LinkButton ID="lnkDuplica" runat="server" CssClass="btnlink" Font-Bold="true">Duplica Evento</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="cnfDuplica" runat="server" TargetControlID="lnkDuplica" ConfirmText="Confermi la duplicazione dell'evento?" />
                    &nbsp;
                    <span class="btnlink" onclick="undoCreation();">Annulla</span>
                    &nbsp;
                    <asp:Label ID="lblGlobalError" runat="server" EnableViewState="false" Font-Bold="true" ForeColor="Red" />
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
