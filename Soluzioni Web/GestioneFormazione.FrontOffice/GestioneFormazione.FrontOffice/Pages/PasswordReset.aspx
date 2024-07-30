<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="PasswordReset.aspx.vb" Inherits="GestioneFormazione.FrontOffice.PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="onecol">
                <div class="title green">
                    REIMPOSTAZIONE PASSWORD
                </div>
                <asp:Panel ID="pnlData" runat="server">
                    <div class="top20">
                        Se hai smarrito la password di accesso al portale, immetti nei campi sottostanti
                        il tuo codice fiscale ed il tuo indirizzo e-mail e fai clic su "reimposta password".<br/>
                        Riceverai un'e-mail contenente un link mediante il quale potrai creare una nuova password.<br/>
                        <b>Nota:</b> se non hai mai effettuato l'accesso al Portale Formazione e sei dipendente
                        <asp:Label ID="lblCompanyName" runat="server" />,
                        non utilizzare la funzione di reimpostazione password ma fai clic su "ACCEDI AL PORTALE" nella parte alta
                        della pagina e segui le istruzioni visualizzate.
                    </div>
                    <div class="datagroup top20">
                        <div class="row">
                            <div class="label">
                                Codice Fiscale
                            </div>
                            <div class="data">
                                <asp:TextBox ID="txtCodiceFiscale" runat="server" MaxLength="16" CssClass="txt txtnarrow"/>
                            </div>
                            <div class="error">
                                <asp:Label ID="errCodiceFiscale" runat="server" EnableViewState="false" />
                            </div>
                        </div>
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
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlStart" runat="server">
                    <div class="top20">
                        <asp:LinkButton ID="lnkStart" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClientClick="return confirm('Confermi la reimpostazione della password?');">Reimposta Password</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="errStart" runat="server" EnableViewState="false" ForeColor="#ff0000" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDone" runat="server" Visible="false">
                    <div class="dataexpl top20">
                        Ti è stata inviata un'e-mail contenente un link mediante il quale potrai creare una nuova password.<br />
                        Controlla la tua casella e-mail entro 24 ore.
                    </div>
                    <div class="top20">
                        <a href="/" class="btnlink btnlink_green">Vai alla home page</a>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
