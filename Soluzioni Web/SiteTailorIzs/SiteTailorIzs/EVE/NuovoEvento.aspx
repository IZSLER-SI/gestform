<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="NuovoEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.NuovoEvento" %>
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
                    Creazione Nuovo Evento
                </div>
                <table class="fieldtable">
                    <tr>
                        <td class="labelcol bordertop">Codice interno</td>
                        <td class="datacol bordertop">
                            <asp:TextBox ID="tx_CODINT" runat="server" CssClass="txt txtwide" MaxLength="32" />
                        </td>
                        <td class="errorcol bordertop">
                            <asp:Label ID="errtx_CODINT" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Tipologia</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_TIPOLOGIAEVENTO" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_TIPOLOGIAEVENTO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Categoria (corsi obbligatori)</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_TIPOCOBDETT" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_TIPOCOBDETT" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Titolo</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_TITOLO" runat="server" CssClass="txt txtwide" MaxLength="600" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_TITOLO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Edizione</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_EDIZIONE" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_EDIZIONE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Sede</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_SEDE" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_SEDE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Dettagli sede (reparto, aula...)</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_DETTAGLISEDE" runat="server" CssClass="txt txtwide" MaxLength="200" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_DETTAGLISEDE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Data Inizio</td>
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
                        <td class="labelcol">Data fine</td>
                        <td class="datacol">
                            <asp:TextBox ID="dt_FINE" runat="server" CssClass="txt txtdate" />
                            <ajaxToolkit:CalendarExtender ID="caldt_FINE" runat="server" TargetControlID="dt_FINE"
                                ClearTime="true" Format="dd/MM/yyyy" />
                            <ajaxToolkit:MaskedEditExtender ID="meddt_FINE" runat="server" TargetControlID="dt_FINE"
                                Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                            <ajaxToolkit:MaskedEditValidator runat="server" ID="mevdt_FINE" ControlToValidate="dt_FINE"
                                ControlExtender="meddt_FINE" Display="dynamic" IsValidEmpty="False" InvalidValueMessage="*" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errdt_FINE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Organizzatore</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_ORGANIZZATORE" runat="server" CssClass="ddn ddnwide" AutoPostBack="true" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_ORGANIZZATORE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Rappresentante Legale Organizzatore</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_RAPPRLEGALE_ORGANIZZATORE" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_RAPPRLEGALE_ORGANIZZATORE" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Segreteria Organizzativa</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_SEGRETERIAORGANIZZATIVA" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_SEGRETERIAORGANIZZATIVA" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Centro di Referenza</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_CENTROREFERENZA" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_CENTROREFERENZA" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Centro di Costo</td>
                        <td class="datacol">
                            <asp:DropDownList ID="ac_CDC" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_CDC" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Piano Formativo</td>
                        <td class="datacol">
                            <asp:DropDownList ID="id_PIANOFORMATIVO" runat="server" CssClass="ddn ddnwide" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errid_PIANOFORMATIVO" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Aggiunto al Piano Formativo</td>
                        <td class="datacol">
                            <asp:CheckBox ID="fl_NUOVOINPF" runat="server" Text="Evento aggiunto in seguito al piano formativo" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errfl_NUOVOINPF" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Obiettivi</td>
                        <td class="datacol">
                            <asp:TextBox ID="tx_OBBIETTIVI" runat="server" CssClass="txt txtwide" TextMode="MultiLine" Height="50px" />
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errtx_OBBIETTIVI" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcol">Accreditamento ECM</td>
                        <td class="datacol">
                            <asp:RadioButtonList ID="ac_NORMATIVAECM" runat="server" RepeatLayout="Flow" AutoPostBack="true">
                                <asp:ListItem Text="Evento accreditato ECM" Value="2011" />
                                <asp:ListItem Text="Evento NON accreditato ECM" Value="NONE" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="errorcol">
                            <asp:Label ID="errac_NORMATIVAECM" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlDatiEcm" runat="server" Visible="false">
                    <table class="fieldtable">
                        <tr>
                            <td class="labelcol">Tipo Formazione</td>
                            <td class="datacol">
                                <asp:DropDownList ID="id_TIPOLOGIAECMEVENTO" runat="server" AutoPostBack="true" CssClass="ddn ddnwide" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errid_TIPOLOGIAECMEVENTO" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Tipologia Formativa</td>
                            <td class="datacol">
                                <asp:DropDownList ID="ecm2_TFORM_EVE" runat="server" AutoPostBack="true" CssClass="ddn ddnwide" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_TFORM_EVE" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr id="row_ecm2_TFORM_EVE_SEC" runat="server" visible=False>
                            <td class="labelcol">Tipologie Formative Secondarie</td>
                            <td class="datacol">
                                <div style="border:1px solid #888888;padding:5px;width:402px;height:200px;overflow-y:scroll;">
                                    <asp:CheckBoxList ID="ecm2_TFORM_EVE_SEC" runat="server" RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_TFORM_EVE_SEC" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Tipo Evento</td>
                            <td class="datacol">
                                <asp:DropDownList ID="ecm2_TIPO_EVE" runat="server" CssClass="ddn ddnwide" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_TIPO_EVE" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Ambito / Obbiettivo Formativo</td>
                            <td class="datacol">
                                <asp:DropDownList ID="ecm2_COD_OBI" runat="server" CssClass="ddn ddnwide" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_COD_OBI" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Ente Accreditante</td>
                            <td class="datacol">
                                <asp:DropDownList ID="ecm2_COD_ACCR" runat="server" CssClass="ddn ddnwide" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_COD_ACCR" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Provider</td>
                            <td class="datacol">
                                <asp:DropDownList ID="id_PROVIDERECM" runat="server" CssClass="ddn ddnwide" AutoPostBack="true" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errid_PROVIDERECM" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Rappresentante Legale Provider</td>
                            <td class="datacol">
                                <asp:DropDownList ID="id_RAPPRLEGALE_PROVIDERECM" runat="server" CssClass="ddn ddnwide" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errid_RAPPRLEGALE_PROVIDERECM" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Professioni e Discipline accreditate</td>
                            <td class="datacol">
                                <div style="border:1px solid #888888;padding:5px;width:402px;height:200px;overflow-y:scroll;">
                                    <asp:CheckBoxList ID="id_DISCIPLINAs" runat="server" RepeatLayout="Flow">
                                        <asp:ListItem Text="Veterinari - Sanità Animale" Value="33" />
                                        <asp:ListItem Text="Medici Chirurghi - Dermatologia" Value="44" />
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errid_DISCIPLINAs" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Codice Evento AGENAS</td>
                            <td class="datacol">
                                <asp:TextBox ID="ecm2_COD_EVE" runat="server" CssClass="txt txtnarrow" MaxLength="20" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_COD_EVE" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Numero crediti (cifre)</td>
                            <td class="datacol">
                                <asp:TextBox ID="ecm2_NUM_CRED" runat="server" CssClass="txt txtnarrow" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_NUM_CRED" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelcol">Numero crediti (lettere)</td>
                            <td class="datacol">
                                <asp:TextBox ID="ecm2_CREDITILETTERE" runat="server" CssClass="txt txtmedium" MaxLength="100" />
                            </td>
                            <td class="errorcol">
                                <asp:Label ID="errecm2_CREDITILETTERE" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div class="endCommands">
                    <asp:LinkButton ID="lnkCreate" runat="server" CssClass="btnlink" Font-Bold="true">Crea Evento</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="cnfCreate" runat="server" TargetControlID="lnkCreate" ConfirmText="Confermi la creazione dell'evento?" />
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
