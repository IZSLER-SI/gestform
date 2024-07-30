<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="DettaglioTipoCorso.aspx.vb" Inherits="Softailor.SiteTailorIzs.DettaglioTipoCorso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
<script type="text/javascript">
    function dettaglioPersona(tcb, id) {
        stl_sel_display_wh('DettaglioPersonaTipoCorso.aspx?tcb=' + tcb + '&id=' + id, 1052, 800, dettaglioPersona_callback);
    }
    function dettaglioPersona_callback(code){ }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        <asp:Label ID="lblTitolo" EnableViewState="false" runat="server">Titolo 1 ggg</asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Chiudi
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updsrcPERSONE" runat="server" Width="1070px" Height="96px" Top="15px" Left="15px">
        <ContentTemplate>
            <stl:StlSearchForm ID="srcPERSONE" runat="server"
                SearchName="Ricerca Persone Tipo COB" LayoutType="Horizontal" Title="Parametri Ricerca Persone Coinvolte" AllowAddNew="false" DontSelectFirstRecord="true">
            </stl:StlSearchForm>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPERSONE_g" runat="server" Width="1070px" Height="671px" Top="128px" Left="15px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPERSONE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_PERSONA" DataSourceID="sdsPERSONE_g"
                EnableViewState="False" ItemDescriptionPlural="persone" ItemDescriptionSingular="persona"
                Title="Risultati Ricerca" AllowDelete="false" AllowInsert="false"
                DeleteConfirmationQuestion="" SqlStringProviderID="srcPERSONE" AllowReselectSelectedRow="true">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_COGNOME" HeaderText="Cognome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_NOME" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="dt_ASSUNZIONE" HeaderText="Assunzione" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_NOMINA" HeaderText="Nomina" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Stato Corso Base">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBBASE")%>"><%# Eval("tx_STATUSBASE_PLUR")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dt_FREQUENZABASE" HeaderText="Data Freq. Corso Base" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_LIMITEBASE" HeaderText="Limite Freq. Corso Base" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Stato Corso Agg.">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBAGG")%>"><%# Eval("tx_STATUSAGG_PLUR")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dt_FREQUENZAAGG" HeaderText="Ultima Freq. Corso Agg." DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_LIMITEAGG" HeaderText="Prox Scad. Corso Agg." DataFormatString="{0:dd/MM/yyyy}" />
                    
                </Columns>
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
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phdOutOfForm" runat="server">
</asp:Content>
