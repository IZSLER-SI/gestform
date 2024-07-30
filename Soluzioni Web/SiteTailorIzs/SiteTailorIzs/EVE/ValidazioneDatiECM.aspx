<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ValidazioneDatiECM.aspx.vb" Inherits="Softailor.SiteTailorIzs.ValidazioneDatiECM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div style="display:none;">
        <asp:UpdatePanel ID="updHiddenCtls" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
            <!-- controlli nascosti -->
            <asp:LinkButton ID="lnkReposition" runat="server">-</asp:LinkButton>
            <asp:TextBox ID="txtReposition" runat="server" Text="0"></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script src="../Scripts/jquery.qtip.min.js"></script>
    <link href="../Scripts/jquery.qtip.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $('#istruzioni').qtip({
                content: {
                    text: $('#istruzionicontent').html()
                },
                position: {
                    my: 'top left',
                    at: 'bottom left',
                    viewport: $(window),
                    target: 'mouse',
                    adjust: {
                        y: 18, mouse: true
                    }
                },
                style: {
                    classes: 'qtip-jtools',
                    width: 400

                }
            });
        });
    </script>
    <div id="istruzioni" style="position: absolute; left: 876px; top: 0px; width:47px;" class="infoDiv11">
        Istruzioni
    </div>
    <div id="istruzionicontent" style="display:none;">
        E' necessario che per ogni partecipante sia stato inserito: un <b>codice fiscale</b> corretto,
        la <b>categoria lavorativa</b>, la <b>professione</b> ed il <b>numero di crediti</b> ottenuti (solo per docenti, tutor, relatori e moderatori).
        Eventuali nominativi con dati incompleti sono mostrati nel primo elenco. Clicca sul nome del partecipante per inserire i dati mancanti.<br />
        Vengono considerati solo i nominativi con stato ECM='Crediti conseguiti'.    </div>
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px; width:300px;">
        <div class="title">
            Verifica dati ECM
        </div>
    </div>
    <stl:StlUpdatePanel ID="updIscrittiKO_g" runat="server" Height="280px" Width="958px"
        Left="0px" Top="40px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIscrittiKO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsISCRITTIKO_g" EnableViewState="False" ItemDescriptionPlural="partecipanti"
                ItemDescriptionSingular="partecipante" Title="Elenco Partecipanti con dati ECM incompleti" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Codice Fiscale">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCODICEFISCALE") %>"><%# Eval("ac_CODICEFISCALE")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Categoria Lavorativa">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCATEGORIALAVORATIVA")%>"><%# Eval("tx_CATEGORIALAVORATIVA")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Professione">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBPROFESSIONE")%>"><%# Eval("tx_PROFESSIONE")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Disciplina">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBDISCIPLINA")%>"><%# Eval("tx_DISCIPLINA")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:TemplateField HeaderText="Crediti (solo per docenti, tutor, relatori, moderatori)">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCREDITIECM") %>"><%# Eval("tx_CREDITIECM")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updIscrittiOK_g" runat="server" Height="230px" Width="958px"
        Left="0px" Top="330px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIscrittiOK" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsISCRITTIOK_g" EnableViewState="False" ItemDescriptionPlural="partecipanti"
                ItemDescriptionSingular="partecipante" Title="Elenco Partecipanti con dati ECM completi" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Codice Fiscale">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCODICEFISCALE") %>"><%# Eval("ac_CODICEFISCALE")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Categoria Lavorativa">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCATEGORIALAVORATIVA")%>"><%# Eval("tx_CATEGORIALAVORATIVA")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Professione">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBPROFESSIONE")%>"><%# Eval("tx_PROFESSIONE")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Disciplina">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBDISCIPLINA")%>"><%# Eval("tx_DISCIPLINA")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:TemplateField HeaderText="Crediti (solo per docenti, tutor, relatori, moderatori)">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCREDITIECM") %>"><%# Eval("tx_CREDITIECM")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlSqlDataSource ID="sdsISCRITTIKO_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure" SelectCommand="sp_eve_ValidazioneECM_Nomi_OK_KO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="fl_OK" Type="Boolean" DefaultValue="False" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsISCRITTIOK_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure" SelectCommand="sp_eve_ValidazioneECM_Nomi_OK_KO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="fl_OK" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </stl:StlSqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
