<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="Persone.aspx.vb" Inherits="Softailor.SiteTailorIzs.Persone"  validateRequest="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        function schedaPersona(id) {
            if (stl_appb_row2Select_action == null) {
                stl_sel_display_wh('Persona.aspx?id=' + id, 1102, 850, editPersona_callback);
            }
            else {
                if (stl_appb_row2Select_action != 'Delete') {
                    stl_sel_display_wh('Persona.aspx?id=' + id, 1102, 850, editPersona_callback);
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
    <stl:StlUpdatePanel ID="updsrcPERSONE" runat="server" Width="1230px" Height="96px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlSearchForm ID="srcPERSONE" runat="server"
                SearchName="Ricerca Persone (anagrafica)" LayoutType="Horizontal" Title="Ricerca Persone" AllowAddNew="true" DontSelectFirstRecord="true">
            </stl:StlSearchForm>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPERSONE_g" runat="server" Width="1230px" Height="700px" Top="110px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPERSONE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_PERSONA" DataSourceID="sdsPERSONE_g"
                EnableViewState="False" ItemDescriptionPlural="persone" ItemDescriptionSingular="persona"
                Title="Risultati Ricerca" OnRowCommand="grdPERSONE_RowCommand"
                DeleteConfirmationQuestion="" SqlStringProviderID="srcPERSONE" AllowReselectSelectedRow="False" AllowInsert="True">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />

                    <asp:ButtonField Text="Portfolio" CommandName="genera_portfolio" runat="server" />


                    <asp:BoundField DataField="tx_COGNOMENOMETITOLO" HeaderText="Cognome e nome" ItemStyle-Font-Bold="true" >
                    <ItemStyle Font-Bold="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" ItemStyle-Font-Bold="true" >
                    <ItemStyle Font-Bold="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="tx_UNITAOPERATIVA" HeaderText="Unità Operativa" />
                    <asp:BoundField DataField="ac_CODICEFISCALE" HeaderText="Codice Fiscale" />

                </Columns>
                <SelectedRowStyle CssClass="src" />
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsPERSONE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommandType="StoredProcedure"
                DeleteCommand="sp_age_PERSONE_delete"
                SelectCommand="DUMMY">
                <DeleteParameters>
                    <asp:Parameter Name="id_PERSONA" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">

</asp:Content>
