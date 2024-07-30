<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SelezioneEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.SelezioneEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updRecent" runat="server" Top="0px" Left="0px" Width="1230px" Height="155px">
        <ContentTemplate>
            <stl:StlGridView ID="grdRecent" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataSourceID="sdsRecent" 
                DeleteConfirmationQuestion="" EnableViewState="False" AllowInsert="False" AllowDelete="false" ItemDescriptionPlural="eventi formativi" 
                ItemDescriptionSingular="evanto formativo" Title="Ultimi eventi formativi sui quali hai lavorato" DataKeyNames="id_EVENTO">
                <Columns>
                    <asp:BoundField DataField="id_EVENTO" HeaderText="ID" />
                    <asp:BoundField DataField="ecm2_COD_EVE" HeaderText="Agenas" />
                    <asp:BoundField DataField="tx_TITOLO" HeaderText="Titolo" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Agg.PF" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_NUOVOINPF"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_EDIZIONE" HeaderText="Edizione" />
                    <asp:BoundField DataField="tx_SEDE" HeaderText="Sede" />
                    <asp:TemplateField HeaderText="Date" ItemStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%# Softailor.Global.DateUtils.DataDalAl(Eval("dt_INIZIO"), Eval("dt_FINE"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_TIPOLOGIAEVENTO" HeaderText="Tipologia" />
                    <asp:BoundField DataField="tx_COGNOMTIT_RS" HeaderText="Responsabile Scientifico" />
                    <asp:BoundField DataField="tx_NORMATIVAECM" HeaderText="Accreditamento ECM" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsRecent" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                SelectCommandType="StoredProcedure"
                SelectCommand="sp_eve_EVENTI_RecentList">
                <SelectParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>           
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <div style="position:absolute;top:167px;width:300px;font-size:14px;">
        <asp:Panel ID="pnlNuovoEvento" runat="server" EnableViewState="false">
            <a class="btnlink" href="NuovoEvento.aspx" style="font-weight:bold;font-family:Arial;">
                Crea un nuovo evento
            </a>
        </asp:Panel>
    </div>
    <stl:StlUpdatePanel ID="updEVENTI_src" runat="server" Top="200px" Left="0px" Width="300px" Height="360px">
        <ContentTemplate>
            <stl:StlSearchForm ID="srcEVENTI" runat="server" Title="Cerca un evento" AllowAddNew="false" SearchName="Ricerca Eventi (selezione)" DontSelectFirstRecord="true" LayoutType="Vertical">
            </stl:StlSearchForm>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updEVENTI_g" runat="server" Top="200px" Left="315px" Width="915px" Height="360px">
        <ContentTemplate>
            <stl:StlGridView ID="grdEVENTI" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataSourceID="sdsEVENTI_g" SqlStringProviderID="srcEVENTI"
                DeleteConfirmationQuestion="" EnableViewState="False" AllowDelete="false" AllowInsert="false" ItemDescriptionPlural="eventi formativi" 
                ItemDescriptionSingular="evento formativo" Title="Risultati Ricerca" DataKeyNames="id_EVENTO">
                <Columns>
                    <asp:BoundField DataField="id_EVENTO" HeaderText="ID" />
                    <asp:BoundField DataField="ecm2_COD_EVE" HeaderText="Agenas" />
                    <asp:BoundField DataField="tx_TITOLO" HeaderText="Titolo" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Agg.PF" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_NUOVOINPF"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_EDIZIONE" HeaderText="Edizione" />
                    <asp:BoundField DataField="tx_SEDE" HeaderText="Sede" />
                    <asp:TemplateField HeaderText="Date" ItemStyle-Font-Bold="true">
                        <ItemTemplate>
                            <%# Softailor.Global.DateUtils.DataDalAl(Eval("dt_INIZIO"), Eval("dt_FINE"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_TIPOLOGIAEVENTO" HeaderText="Tipologia" />
                    <asp:BoundField DataField="tx_COGNOMTIT_RS" HeaderText="Responsabile Scientifico" />
                    <asp:BoundField DataField="tx_NORMATIVAECM" HeaderText="Accreditamento ECM" />
                </Columns>
            </stl:StlGridView>
             <stl:StlSqlDataSource ID="sdsEVENTI_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                SelectCommand="DUMMY">
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
