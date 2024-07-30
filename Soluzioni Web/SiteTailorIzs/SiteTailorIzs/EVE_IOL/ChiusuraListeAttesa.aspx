<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ChiusuraListeAttesa.aspx.vb" Inherits="Softailor.SiteTailorIzs.ChiusuraListeAttesa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
<style type="text/css">
    .numtable
    {
        font-size:11px;
        width:100%;
    }
    .numtable td
    {
        border-bottom:solid 1px #c0c0c0;
        empty-cells:show;
    }
    .numtable .int
    {
        padding:1px 2px 1px 2px;
        width:175px;
        text-align:left;
    }
    .numtable .num
    {
        width:42px;
        text-align:right;
        font-weight:bold;
        padding:1px 5px 1px 5px;
        background-color:#f0f0f0;
    }
    .numtable .not
    {
        font-style:oblique;
        padding:1px 2px 1px 20px;
    }
    .rowCmd
    {
        font-size:10px;
        line-height:9px;
        padding-top:0px;
        padding-bottom:0px;    
    }
    .rowBtn
    {
        color:#336699;
        font-weight:normal;
        text-decoration:none;    
    }
    .rowBtn:hover
    {
        color:#ff6600;
        text-decoration:none;    
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updStatus" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdStatus" runat="server" />    
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updOrdinamento" runat="server" EnableViewState="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="position:absolute;background-color:#6593cf;top:74px;left:0px;width:832px;height:25px;color:#ffffff;font-weight:bold;">
                <div style="padding-left:5px;padding-top:2px;">
                Ordina le liste per&nbsp;
                <asp:DropDownList ID="ddnOrdinamento" runat="server" CssClass="ddn" AutoPostBack="true" Font-Bold="true" Width="160px">
                    <asp:ListItem Text="Data Iscrizione" Value="DI" Selected="True" />
                    <asp:ListItem Text="Cognome e Nome" Value="CN" />
                </asp:DropDownList>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <stl:StlUpdatePanel ID="updI" runat="server" Height="355px" Width="608px"
        Left="0px" Top="103px">
        <ContentTemplate>
            <stl:StlGridView ID="grdI" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsI" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Iscritti (accettati)" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="ni_POSIZIONE" HeaderText="Pos" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data/Ora iscr" DataFormatString="{0:dd/MM/yy HH:mm}" />
                    <asp:TemplateField HeaderText="Sposta in..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="NA" Text="&gt;Non Acc" runat="server" ID="btnNA"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="LAP" Text="&gt;Lista Pri" runat="server" ID="btnLAP" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="LAS" Text="&gt;Lista Sec" runat="server" ID="btnLAS" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updNA" runat="server" Height="355px" Width="608px"
        Left="618px" Top="103px">
        <ContentTemplate>
            <stl:StlGridView ID="grdNA" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsNA" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Non Accettati" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="ni_POSIZIONE" HeaderText="Pos" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data/Ora iscr" DataFormatString="{0:dd/MM/yy HH:mm}" />
                    <asp:TemplateField HeaderText="Sposta in..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritti" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="LAP" Text="&gt;Lista Pri" runat="server" ID="btnLAP" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="LAS" Text="&gt;Lista Sec" runat="server" ID="btnLAS" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

     <stl:StlUpdatePanel ID="updLAP" runat="server" Height="355px" Width="608px"
        Left="0px" Top="468px">
        <ContentTemplate>
            <stl:StlGridView ID="grdLAP" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsLAP" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Lista att.prioritaria/unica" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="ni_POSIZIONE" HeaderText="Pos" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data/Ora iscr" DataFormatString="{0:dd/MM/yy HH:mm}" />
                    <asp:TemplateField HeaderText="Sposta in..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                           <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritti" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                             <asp:LinkButton CssClass="rowBtn" CommandName="NA" Text="&gt;Non Acc" runat="server" ID="btnNA"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="LAS" Text="&gt;Lista Sec" runat="server" ID="btnLAS" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updLAS" runat="server" Height="355px" Width="608px"
        Left="618px" Top="468px">
        <ContentTemplate>
            <stl:StlGridView ID="grdLAS" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsLAS" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Lista att.secondaria" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="ni_POSIZIONE" HeaderText="Pos" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data/Ora iscr" DataFormatString="{0:dd/MM/yy HH:mm}" />
                    <asp:TemplateField HeaderText="Sposta in..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritti" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="NA" Text="&gt;Non Acc" runat="server" ID="btnNA"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="LAP" Text="&gt;Lista Pri" runat="server" ID="btnLAP" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlSqlDataSource ID="sdsI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        DeleteCommand=""
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_eve_iol_ElencoListaAttesa">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="I" />
            <asp:ControlParameter Name="ac_ORDINAMENTO" Type="String" ControlID="ddnOrdinamento" PropertyName="SelectedValue" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsLAP" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        DeleteCommand=""
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_eve_iol_ElencoListaAttesa">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="LAP" />
            <asp:ControlParameter Name="ac_ORDINAMENTO" Type="String" ControlID="ddnOrdinamento" PropertyName="SelectedValue" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsLAS" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        DeleteCommand=""
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_eve_iol_ElencoListaAttesa">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="LAS" />
            <asp:ControlParameter Name="ac_ORDINAMENTO" Type="String" ControlID="ddnOrdinamento" PropertyName="SelectedValue" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsNA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        DeleteCommand=""
        SelectCommandType="StoredProcedure"
        SelectCommand="sp_eve_iol_ElencoListaAttesa">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="NA" />
            <asp:ControlParameter Name="ac_ORDINAMENTO" Type="String" ControlID="ddnOrdinamento" PropertyName="SelectedValue" />
        </SelectParameters>
    </stl:StlSqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
