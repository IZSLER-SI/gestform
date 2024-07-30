<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="DoPasswordReset.aspx.vb" Inherits="GestioneFormazione.FrontOffice.DoPasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="onecol">
                <div class="title green">
                    REIMPOSTAZIONE PASSWORD
                </div>
                <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
                    <div class="top20" style="color: red;">
                        Siamo spiacenti, il link per il cambio password non può essere più utilizzato.<br />
                        Puoi richiedere un nuovo link per il cambio password
                        <a class="classica" href="/password-smarrita">cliccando qui</a>.
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlExpired" runat="server" Visible="false">
                    <div class="top20" style="color: red;">
                        Siamo spiacenti, il link per il cambio password non può essere più utilizzato in quanto sono trascorse più di 24 ore dalla generazione.<br />
                        Puoi richiedere un nuovo link per il cambio password
                        <a class="classica" href="/password-smarrita">cliccando qui</a>.
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFeasible" runat="server" Visible="false">
                    <div class="dataexpl top20">
                        Scegli una nuova password per l'accesso al portale.<br />
                        La nuova password deve essere lunga almeno 8 caratteri.<br />
                        Fai attenzione: il sistema distingue le lettere minuscole dalle maiuscole.
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
                    <div class="top20">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClientClick="return confirm('Confermi la modifica della password?');">Modifica Password</asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDone" runat="server" Visible="false">
                    <div class="top20">
                        La tua password è stata correttamente reimpostata.<br />
                        Puoi accedere al portale utilizzando la nuova password facendo clic sul link "ACCEDI AL PORTALE" in alto a destra.
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
