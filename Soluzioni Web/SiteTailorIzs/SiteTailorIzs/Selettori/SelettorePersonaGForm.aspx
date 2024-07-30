<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="SelettorePersonaGForm.aspx.vb" Inherits="Softailor.SiteTailorIzs.SelettorePersonaGForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        <asp:Literal ID="ltrTitolo" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Annulla
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updsrcPERSONE" runat="server" Top="15px" Left="15px" Width="240px"
        Height="130px">
        <ContentTemplate>
            <stl:StlSearchForm ID="srcPERSONE" runat="server" Title="Parametri Ricerca" AllowAddNew="false"
                SearchName="Ricerca Persone (selezione)" DontSelectFirstRecord="true">
            </stl:StlSearchForm>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPERSONE_g" runat="server" Height="395px" Width="585px"
        Left="270px" Top="15px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPERSONE" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_PERSONA" DataSourceID="sdsPERSONE_g" EnableViewState="False"
                ItemDescriptionPlural="nominativi" ItemDescriptionSingular="nominativo" Title="Risultati Ricerca (primi 300)"
                SqlStringProviderID="srcPERSONE" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="tx_TIPOCONTRATTO" HeaderText="Tipo Contratto" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsPERSONE_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="Dummy">
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
