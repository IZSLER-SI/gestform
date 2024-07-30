<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="ChangePassword.aspx.vb" Inherits="GestioneFormazione.FrontOffice.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="onecol">
                <div class="title green">
                    MODIFICA PASSWORD
                </div>
                <asp:Panel ID="pnlPassword" runat="server">
                    <div class="dataexpl top20">
                        Inserisci la password attuale e la nuova password.<br />
                        La nuova password deve essere lunga almeno 8 caratteri.<br />
                        Fai attenzione: il sistema distingue le lettere minuscole dalle maiuscole.
                    </div>
                    <div class="datagroup">
                        <div class="row">
                            <div class="label">
                                Password attuale
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" MaxLength="50" CssClass="txt txtnarrow" />
                            </div>
                            <div class="error">
                                <asp:Label ID="errOldPassword" runat="server" EnableViewState="false" />
                            </div>
                        </div>
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

                <asp:Panel ID="pnlSave" runat="server">
                    <div class="top20">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClientClick="return confirm('Confermi la modifica della password?');">Modifica Password</asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDone" runat="server" Visible="false">
                    <div class="top20">
                        La tua password è stata cambiata.
                    </div>
                    <div class="top20" style="font-weight:bold;">
                        <a href="/" class="btnlink btnlink_green">Vai alla home page</a>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
