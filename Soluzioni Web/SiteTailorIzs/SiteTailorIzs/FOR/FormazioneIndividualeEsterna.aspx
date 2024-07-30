<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="FormazioneIndividualeEsterna.aspx.vb" Inherits="Softailor.SiteTailorIzs.FormazioneIndividualeEsterna" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        function schedaAutocertificazione(id) {
            if (stl_appb_row2Select_action == null) {
                stl_sel_display_wh('Autocertificazione.aspx?id=' + id, 1102, 400, editAutocertificazione_callback);
            }
            else {
                if (stl_appb_row2Select_action != 'Delete') {
                    stl_sel_display_wh('Autocertificazione.aspx?id=' + id, 1102, 400, editAutocertificazione_callback);
                }
            }
        }
    </script>
    <script type="text/javascript">
        <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div style="display:none;">
        <asp:UpdatePanel ID="updHiddenCtls" runat="server" EnableViewState="true" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- controlli nascosti -->
                <asp:LinkButton ID="lnkReposition" runat="server">-</asp:LinkButton>
                <asp:TextBox ID="txtReposition" runat="server" Text="0"></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <stl:StlUpdatePanel ID="updsrcPARTECIPAZIONI" runat="server" Width="1230px" Height="96px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlSearchForm ID="srcPARTECIPAZIONI" runat="server"
                SearchName="Ricerca Autocertificazioni FO" LayoutType="Horizontal" 
                Title="Ricerca Auto-certificazioni" AllowAddNew="false" DontSelectFirstRecord="true">
            </stl:StlSearchForm>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPARTECIPAZIONI_g" runat="server" Width="1230px" Height="700px" Top="110px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPARTECIPAZIONI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_PARTECIPAZIONE" DataSourceID="sdsPARTECIPAZIONI_g"
                EnableViewState="False" ItemDescriptionPlural="auto-certificazioni" ItemDescriptionSingular="auto-certificazione"
                Title="Risultati Ricerca (primi 200)" AllowDelete="false" 
                DeleteConfirmationQuestion="" SqlStringProviderID="srcPARTECIPAZIONI" AllowReselectSelectedRow="true">
                <Columns>
                    <asp:TemplateField HeaderText="N°/Anno" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%# Eval("ni_NUMERO") & "/" & Eval("ni_ANNO")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dt_DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Stato">
                        <ItemTemplate>
                            <%# "<span style=""color:" & Eval("ac_RGB") & """>" &
                                Eval("tx_STATOPARTECIPAZIONE") & "</span>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_PERSONA" HeaderText="Persona" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="tx_TITOLO" HeaderText="Titolo Evento" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_TIPOLOGIAEVENTO" HeaderText="Tipologia Evento" />
                    <asp:TemplateField HeaderText="Date" ItemStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%# Softailor.Global.DateUtils.DataDalAl(Eval("dt_INIZIO"), Eval("dt_FINE"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsPARTECIPAZIONI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="DUMMY">
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
