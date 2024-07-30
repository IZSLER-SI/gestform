<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="ControlloPreLogin.aspx.vb" Inherits="GestioneFormazione.FrontOffice.ControlloPreLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <div class="onecol">
                <div class="title green">MODIFICA PROFILO</div>
                <asp:Panel ID="pnlStep1" runat="server">
                    <div class="top20">
                        Almeno un dato non è stato inserito oppure è errato. <br />
                        Ti chiediamo gentilmente di controllare i tuoi dati. <br />
                        I campi contrassegnati da * sono obbligatori. <br />
                        Al termine fai clic su "Avanti"
                    </div>
                    <br />
                    <div class="err">
                        <asp:Label ID="dato_mancante_lbl_error" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                    <div class="datatitle top20">Informazioni professionali</div>
                    <div class="datagroup">
                        <div class="row" <%= If(IsDipendente(), "style='display: none'", "") %>>
                            <div class="label">Ruolo *</div>
                            <div class="data">
                                <asp:DropDownList ID="ac_RUOLO" runat="server" DataSourceID="sdsac_RUOLO"
                                    DataTextField="tx_RUOLO" DataValueField="ac_RUOLO" AppendDataBoundItems="true"
                                    CssClass="dnn dnnwide" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cambio_Downlist">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_RUOLO" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row" <%= If(IsDipendente(), "style='display: none'", "") %>>
                            <div class="label">
                                Profilo *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="ac_PROFILO" runat="server" DataSourceID="sdsac_PROFILO"
                                DataTextField="tx_PROFILO" DataValueField="ac_PROFILO" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide" AutoPostBack="true" OnSelectedIndexChanged="Cambio_Downlist">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_ac_PROFILO" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="label">
                                Professione/Disciplina ECM *
                            </div>
                            <div class="data">
                                <asp:DropDownList ID="id_DISCIPLINA" runat="server" DataSourceID="sdsid_DISCIPLINA"
                                DataTextField="tx_DISCIPLINA" DataValueField="id_DISCIPLINA" AppendDataBoundItems="false"
                                CssClass="ddn ddnwide" AutoPostBack="true" OnSelectedIndexChanged="cambio_downlist">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="error">
                                <asp:Label ID="err_id_DISCIPLINA" runat="server" EnableViewState="false" />
                            </div>
                        </div>
                    </div>
                    <div class="top20">
                        <asp:LinkButton ID="lnk_memorizzaStato" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClick="Memorizza_Informazioni_Utente">Aggiorna le tue informazioni</asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnl_User_Message" runat="server"> 
                    L'operazione di aggiornamento dei dati è avvenuta con successo. <br />
                    Ti gentilmente chiediamo di cliccare sul pulsante "Ritorna alla Homepage" ed effettuare nuovamente l'accesso 
                    <div class="top20">
                        <asp:LinkButton ID="lnk_ritornaLogin" runat="server" CssClass="btnlink btnlink_green" Font-Bold="true" OnClick="Ritorna_HomePage">Ritorna alla Homepage</asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sdsac_RUOLO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_reg_Ruoli">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_PROFILO" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_TryPreLogin_ProfiliRuolo">
        <SelectParameters>
            <asp:ControlParameter Name="ac_RUOLO" Type="String" ControlID="ac_RUOLO" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_DISCIPLINA" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_fo_TryPreLogin_DisciplineProfilo">
    </asp:SqlDataSource>
</asp:Content>