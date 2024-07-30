<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="CompleteProfile.aspx.vb" Inherits="GestioneFormazione.FrontOffice.CompleteProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="onecol">
                <div class="title green">
                    Completa il tuo profilo
                </div>
                <asp:Panel ID="pnlPassword" runat="server">
                    <div class="datatitle top20">Nuova Password</div>
                    <div class="dataexpl">
                        Scegli una password da utilizzare per accedere al portale.
                        La password deve essere lunga almeno 8 caratteri.<br />
                        Fai attenzione: il sistema distingue le lettere minuscole dalle maiuscole.<br />
                        Dopo aver scelto la password, potrai accedere al portale utilizzando il tuo codice fiscale come nome utente e la password scelta come password.
                    </div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label">
                                Nuova password
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="50" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errPassword" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Nuova password
                        <div class="expl">reinserisci per conferma</div>
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" MaxLength="50" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errPassword2" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlEmail" runat="server">
                    <div class="datatitle top20">Indirizzo e-mail</div>
                    <div class="dataexpl">
                        Inserisci un indirizzo e-mail valido e funzionante.<br />
                        Utilizzeremo questo indirizzo e-mail per inviarti tutte le comunicazioni relative alla tua attività formativa.
                    </div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label">
                                Indirizzo e-mail
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errEmail" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Indirizzo e-mail
                        <div class="expl">reinserisci per conferma</div>
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtEmail2" runat="server" MaxLength="150" CssClass="txt txtwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errEmail2" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlEsterni" runat="server">
                    <div class="dataexpl top20">
                        Completa i seguenti campi. Ci serviranno per identificarti e per capire a quali eventi puoi partecipare.
                    </div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label">
                                Tipologia rapporto lavorativo
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ddnCategoriaLavorativa" runat="server" CssClass="ddn ddnwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errCategoriaLavorativa" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Ruolo
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ddnRuolo" runat="server" CssClass="ddn ddnwide" AutoPostBack="true" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errRuolo" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Profilo
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ddnProfilo" runat="server" CssClass="ddn ddnwide" AutoPostBack="true" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errProfilo" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="pnlDisciplina" Visible="false" CssClass="row">
                            <div class="label">
                                Professione/Disciplina ECM
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ddnDisciplina" runat="server" CssClass="ddn ddnwide" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errDisciplina" runat="server" EnableViewState="false" />
                            </div>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlSave" runat="server">
                    <div class="top20">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClientClick="return confirm('Confermi il salvataggio dei dati inseriti?');">Salva Dati</asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDone" runat="server" Visible="false">
                    <div class="top20">
                        I dati del tuo profilo sono stati correttamente aggiornati.
                    </div>
                    <div class="top20" style="font-weight:bold;">
                        <a href="/" class="btnlink btnlink_green">Vai alla home page</a>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
