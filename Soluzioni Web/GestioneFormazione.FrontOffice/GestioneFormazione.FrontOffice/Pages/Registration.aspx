<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="Registration.aspx.vb" Inherits="GestioneFormazione.FrontOffice.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
						<input style="display:none" type="text" name="fakeusernameremembered"/>
						<input style="display:none" type="password" name="fakepasswordremembered"/>
            <div class="onecol">
                <div class="title green">
                    REGISTRAZIONE AL PORTALE
                </div>
                <asp:Panel ID="pnlStep1" runat="server">
                    <div class="top20">
                        Per iniziare il processo di registrazione, immetti il tuo codice fiscale nel campo sottostante.
                    </div>
                    <div class="datagroup top20">
                        <div class="row">
                            <div class="label">
                                Codice Fiscale *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="ac_CODICEFISCALE" runat="server" MaxLength="16" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_CODICEFISCALE" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                    <div class="top20">
                        <asp:LinkButton ID="lnkNext1" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">Avanti &gt;</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="errStep1" runat="server" EnableViewState="false" ForeColor="#ff0000" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlStep2" runat="server">
                    <div class="datatitle top20">Dati Anagrafici</div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label" style="width:145px;">
                                Titolo *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_TITOLO" runat="server" DataSourceID="sdsac_TITOLO"
                                DataTextField="tx_TITOLO" DataValueField="ac_TITOLO" AppendDataBoundItems="true"
                                CssClass="ddn ddnnarrow">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_TITOLO" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Cognome *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_COGNOME" runat="server" MaxLength="100" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_COGNOME" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Nome *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_NOME" runat="server" MaxLength="100" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_NOME" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Data di nascita *
                                <div class="expl">gg/mm/aaaa</div>
                            </div>
                            <div class="data">
                                <asp:TextBox ID="dt_NASCITA" runat="server" MaxLength="10" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_dt_NASCITA" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Provincia di nascita *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_PROVINCIA_nascita" runat="server" DataSourceID="sdsac_PROVINCIA_nascita"
                                DataTextField="tx_PROVINCIA" DataValueField="ac_PROVINCIA" AppendDataBoundItems="true"
                                CssClass="ddn ddnmedium" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_PROVINCIA_nascita" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Luogo di nascita *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_LUOGO_nascita" runat="server" DataSourceID="sdsac_LUOGO_nascita"
                                DataTextField="tx_COMUNE" DataValueField="ac_COMUNE" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_LUOGO_nascita" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Genere *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_GENERE" runat="server"
                                CssClass="ddn ddnnarrow">
                                    <asp:ListItem Text="" Value="" />
                                    <asp:ListItem Text="Maschio" Value="M" />
                                    <asp:ListItem Text="Femmina" Value="F" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_GENERE" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">Residenza</div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label" style="width:145px;">
                                Indirizzo *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_INDIRIZZO_res" runat="server" MaxLength="300" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_INDIRIZZO_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Provincia *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_PROVINCIA_res" runat="server" DataSourceID="sdsac_PROVINCIA_indirizzo"
                                DataTextField="tx_PROVINCIA" DataValueField="ac_PROVINCIA" AppendDataBoundItems="true"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_PROVINCIA_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Comune *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_COMUNE_res" runat="server" DataSourceID="sdsac_COMUNE_indirizzo_res"
                                DataTextField="tx_COMUNE" DataValueField="ac_COMUNE" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_COMUNE_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                CAP *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_CAP_res" runat="server" DataSourceID="sdsac_CAP_indirizzo_res"
                                DataTextField="ac_CAP" DataValueField="ac_CAP" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_CAP_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">Recapiti</div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label" style="width:145px;">
                                Telefono
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_TELEFONO_res" runat="server" MaxLength="20" CssClass="txt txtmedium" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_TELEFONO_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label" style="width:145px;">
                                Fax
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_FAX_res" runat="server" MaxLength="20" CssClass="txt txtmedium" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_FAX_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Cellulare *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_CELLULARE_res" runat="server" MaxLength="20" CssClass="txt txtmedium" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_CELLULARE_res" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                E-mail *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_EMAIL" runat="server" MaxLength="150" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_EMAIL" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                E-mail *
                                <div class="expl">reinserisci per conferma</div>
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_EMAIL_conf" runat="server" MaxLength="150" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_EMAIL_conf" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                         <div class="row">
                            <div class="label">
                                PEC
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_EMAILPEC" runat="server" MaxLength="150" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_EMAILPEC" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>

                    <div class="top20">
                        <asp:LinkButton ID="lnkPrevious2" runat="server" CssClass="btnlink btnlink_green">&lt; Indietro</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkNext2" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">Avanti &gt;</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="errStep2" runat="server" EnableViewState="false" ForeColor="#ff0000" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlStep3" runat="server">
                    <div class="datatitle top20">Informazioni Professionali</div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label" style="width:200px;">
                                Tipologia rapporto lavorativo *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_CATEGORIALAVORATIVA" runat="server" DataSourceID="sdsac_CATEGORIALAVORATIVA"
                                DataTextField="tx_CATEGORIALAVORATIVA" DataValueField="ac_CATEGORIALAVORATIVA" AppendDataBoundItems="true"
                                CssClass="ddn ddnwide">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_CATEGORIALAVORATIVA" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Ruolo *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_RUOLO" runat="server" DataSourceID="sdsac_RUOLO"
                                DataTextField="tx_RUOLO" DataValueField="ac_RUOLO" AppendDataBoundItems="true"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_RUOLO" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Profilo *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_PROFILO" runat="server" DataSourceID="sdsac_PROFILO"
                                DataTextField="tx_PROFILO" DataValueField="ac_PROFILO" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_PROFILO" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <asp:Panel ID="pnlac_CODICEESTERNO" runat="server" Visible="false" CssClass="row">
                            <div class="label">
                                <asp:Label ID="lbl_ac_CODICEESTERNO" runat="server" />
                            </div>
                            <div class="data">
                                <asp:TextBox ID="ac_CODICEESTERNO" runat="server" MaxLength="16" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_CODICEESTERNO" runat="server" EnableViewState="false" />
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="label">
                                Professione/Disciplina ECM
                                <div class="expl">Solo per profili accreditati ECM</div>
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="id_DISCIPLINA" runat="server" DataSourceID="sdsid_DISCIPLINA"
                                DataTextField="tx_DISCIPLINA" DataValueField="id_DISCIPLINA" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_id_DISCIPLINA" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">Ente presso il quale lavori</div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label" style="width:200px;">
                                Denominazione ente
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_ENTE_lav" runat="server" MaxLength="300" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_ENTE_lav" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label" style="width:145px;">
                                Indirizzo
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_INDIRIZZO_lav" runat="server" MaxLength="300" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_INDIRIZZO_lav" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Provincia
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_PROVINCIA_lav" runat="server" DataSourceID="sdsac_PROVINCIA_indirizzo"
                                DataTextField="tx_PROVINCIA" DataValueField="ac_PROVINCIA" AppendDataBoundItems="true"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_PROVINCIA_lav" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Comune
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_COMUNE_lav" runat="server" DataSourceID="sdsac_COMUNE_indirizzo_lav"
                                DataTextField="tx_COMUNE" DataValueField="ac_COMUNE" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide" AutoPostBack="true">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_COMUNE_lav" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                CAP
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_CAP_lav" runat="server" DataSourceID="sdsac_CAP_indirizzo_lav"
                                DataTextField="ac_CAP" DataValueField="ac_CAP" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_CAP_lav" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                    <div class="top20">
                        <asp:LinkButton ID="lnkPrevious3" runat="server" CssClass="btnlink btnlink_green">&lt; Indietro</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkNext3" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">Avanti &gt;</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="errStep3" runat="server" EnableViewState="false" ForeColor="#ff0000" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlStep4" runat="server">
                    <div class="datatitle top20">Scelta della password</div>
                    <div class="top20">
                        Scegli una password da utilizzare per l'accesso al portale.<br />
                        La password deve essere lunga almeno 8 caratteri.<br />
                        Fai attenzione: il sistema distingue le lettere minuscole dalle maiuscole.
                    </div>
                    <div class="datagroup top20">
                        <div class="row">
                            <div class="label">
                                Password *
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_PASSWORD" runat="server" TextMode="Password" MaxLength="50" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_PASSWORD" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Password *
                        <div class="expl">reinserisci per conferma</div>
                            </div>
                            <div class="data">
                                <asp:TextBox ID="tx_PASSWORD_conf" runat="server" TextMode="Password" MaxLength="50" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="err_tx_PASSWORD_conf" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <asp:HiddenField ID="hidPassword" runat="server" />
                    </div>
                    <div class="top20">
                        <asp:LinkButton ID="lnkPrevious4" runat="server" CssClass="btnlink btnlink_green">&lt; Indietro</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkNext4" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true">Avanti &gt;</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="errStep4" runat="server" EnableViewState="false" ForeColor="#ff0000" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlStep5" runat="server">
                    <div class="datatitle top20">Riepilogo dei dati immessi</div>
                    <div>
                        Controlla attentamente i dati che hai inserito.<br />
                        Se riscontri delle inesattezze puoi tornare ai passi precedenti mediante il pulsante "Indietro".<br />
                    </div>
                    <div class="datatitle top20">
                        Dati Personali
                    </div>
                    <div class="riepgroup">
                        <div class="row">
                            <div class="label">
                                Codice Fiscale
                            </div>
                            <div class="value">
                                <asp:Label ID="r_ac_CODICEFISCALE" runat="server" />
                            </div>
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
                                Cognome
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_COGNOME" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Nome
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_NOME" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Data di nascita
                            </div>
                            <div class="value">
                                <asp:Label ID="r_dt_NASCITA" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Provincia di nascita
                            </div>
                            <div class="value">
                                <asp:Label ID="r_ac_PROVINCIA_nascita" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Luogo di nascita
                            </div>
                            <div class="value">
                                <asp:Label ID="r_ac_LUOGO_nascita" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Genere
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_GENERE" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">
                        Residenza
                    </div>
                    <div class="riepgroup">
                        <div class="row">
                            <div class="label">
                                Indirizzo
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_INDIRIZZO_res" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Cap
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_CAP_res" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Comune
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_COMUNE_res" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Provincia
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_PROVINCIA_res" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">
                        Recapiti
                    </div>
                    <div class="riepgroup">
                        <div class="row">
                            <div class="label">
                                Telefono
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_TELEFONO_res" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Fax
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_FAX_res" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Cellulare
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_CELLULARE_res" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                E-mail
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_EMAIL" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                PEC
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_EMAILPEC" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">
                        Informazioni Professionali
                    </div>
                    <div class="riepgroup">
                        <div class="row">
                            <div class="label">
                                Tipologia rapporto lavorativo
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_CATEGORIALAVORATIVA" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Ruolo
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_RUOLO" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Profilo
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_PROFILO" runat="server" />
                            </div>
                        </div>
                        <asp:Panel ID="pnl_r_ac_CODICEESTERNO" runat="server" CssClass="row">
                            <div class="label">
                                <asp:Label ID="lbl_r_ac_CODICEESTERNO" runat="server" />
                            </div>
                            <div class="value">
                                <asp:Label ID="r_ac_CODICEESTERNO" runat="server" />
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="label">
                                Professione/Disciplina ECM
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_DISCIPLINA" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">
                        Ente presso il quale lavori
                    </div>
                    <div class="riepgroup">
                        <div class="row">
                            <div class="label">
                                Denominazione ente
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_ENTE_lav" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Indirizzo
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_INDIRIZZO_lav" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                CAP
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_CAP_lav" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Comune
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_COMUNE_lav" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Provincia
                            </div>
                            <div class="value">
                                <asp:Label ID="r_tx_PROVINCIA_lav" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="datatitle top20">Privacy</div>
                    <div>
                        <asp:CheckBox ID="chkConsensoPrivacy" Text="Autorizzo il trattamento dei dati personali ai sensi dell'art. 23 del DLGS 30/06/2003 n.196" runat="server" />
                    </div>
                    <div class="datatitle top20">Avvertenze sul copyright e sulle condizioni d'uso</div>
                    <div>
                        Tutti i diritti sono riservati. Il materiale didattico è di proprietà degli editori ed è protetto dalle leggi di 
                        copyright nazionali e dai trattati internazionali. È vietata la riproduzione, la distribuzione e la vendita del 
                        materiale didattico o di qualsiasi sua parte ad eccezione dell’uso personale e di quanto consentito nelle avvertenze 
                        relative al copyright e alle condizioni.
                        <br />
                        <br />
                        <asp:CheckBox ID="chkConsensoCopyright" Text="Dichiaro di aver letto e di accettare le condizioni sopra esposte" runat="server" />
                    </div>
                    <div class="top20">
                        <asp:LinkButton ID="lnkPrevious5" runat="server" CssClass="btnlink btnlink_green">&lt; Indietro</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkRegister" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClientClick="return confirm('Confermi la correttezza di tutti i dati inseriti?');">Completa Registrazione</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="errStep5" runat="server" EnableViewState="false" ForeColor="#ff0000" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlStep6" runat="server">
                    <div class="top20">
                        <b>Il processo di registrazione si è concluso correttamente.</b><br />
                        Per accedere al portale fai clic sul link "ACCEDI AL PORTALE" in alto a destra.<br />
                        Ti raccomandiamo di annotare la password scelta in un luogo sicuro.<br />
                        <br />
                        <a href="/" class="btnlink btnlink_green" style="font-weight:bold;">Torna alla home page</a>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sdsac_TITOLO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_Titoli">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_PROVINCIA_nascita" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_ProvinceNascita">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_LUOGO_nascita" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_ComuniProvinciaNascita">
        <SelectParameters>
            <asp:ControlParameter Name="ac_PROVINCIA" Type="String" ControlID="ac_PROVINCIA_nascita" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_PROVINCIA_indirizzo" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_ProvinceIndirizzo">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_COMUNE_indirizzo_res" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_ComuniProvinciaIndirizzo">
        <SelectParameters>
            <asp:ControlParameter Name="ac_PROVINCIA" Type="String" ControlID="ac_PROVINCIA_res" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_CAP_indirizzo_res" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_CapComuniIndirizzo">
        <SelectParameters>
            <asp:ControlParameter Name="ac_COMUNE" Type="String" ControlID="ac_COMUNE_res" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_CATEGORIALAVORATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_CategorieLavorative">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_RUOLO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_Ruoli">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_PROFILO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_ProfiliRuolo">
        <SelectParameters>
            <asp:ControlParameter Name="ac_RUOLO" Type="String" ControlID="ac_RUOLO" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsid_DISCIPLINA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_DisciplineProfilo">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_COMUNE_indirizzo_lav" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_ComuniProvinciaIndirizzo">
        <SelectParameters>
            <asp:ControlParameter Name="ac_PROVINCIA" Type="String" ControlID="ac_PROVINCIA_lav" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

     <asp:SqlDataSource ID="sdsac_CAP_indirizzo_lav" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_CapComuniIndirizzo">
        <SelectParameters>
            <asp:ControlParameter Name="ac_COMUNE" Type="String" ControlID="ac_COMUNE_lav" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
