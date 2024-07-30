<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="ChangeMail.aspx.vb" Inherits="GestioneFormazione.FrontOffice.ChangeMail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
        <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="onecol">
                <div class="title green">
                    Modifica indirizzo e-mail
                </div>
                <asp:Panel ID="pnlEmail" runat="server">
                    <div class="datatitle top20">Indirizzo e-mail</div>
                    <div class="dataexpl">
                        Inserisci un nuovo indirizzo e-mail (valido e funzionante).<br />
                        Utilizzeremo questo indirizzo e-mail per inviarti tutte le comunicazioni relative alla tua attività formativa.
                    </div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label">
                                Indirizzo e-mail attuale
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtOldEmail" runat="server" MaxLength="150" CssClass="txt txtwide" Enabled="false" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errOldEmail" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Nuovo indirizzo e-mail
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
                                Nuovo indirizzo e-mail
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
                <asp:Panel ID="pnlSave" runat="server">
                    <div class="top20">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClientClick="return confirm('Confermi la modifica dell\'indirizzo e-mail?');">Modifica e-mail</asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDone" runat="server" Visible="false">
                    <div class="top20">
                        Il tuo indirizzo e-mail è stato correttamente aggiornato.
                    </div>
                    <div class="top20" style="font-weight:bold;">
                        <a href="/" class="btnlink btnlink_green">Vai alla home page</a>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
