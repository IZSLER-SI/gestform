<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CorrezioneQuestionariApprendimento.aspx.vb" Inherits="Softailor.SiteTailorIzs.CorrezioneQuestionariApprendimento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .rowCmd
        {
            font-size:11px;
            line-height:10px;    
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
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px;width:474px">
        <div class="title">
            Correzione Questionari Apprendimento
        </div>
    </div>
    <stl:StlUpdatePanel ID="updNonCorretti" runat="server" Height="534px" Width="474px"
        Left="0px" Top="30px">
        <ContentTemplate>
            <stl:StlGridView ID="grdNonCorretti" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsNonCorretti" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Nominativi con questionario da correggere" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Comandi" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="EDIT" Text="&gt;Correggi Questionario" runat="server" ID="btnCorreggi" Font-Bold="true"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCorrettiOK" runat="server" Height="400px" Width="604px"
        Left="484px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCorrettiOK" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsCorrettiOK" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Nominativi che hanno superato il questionario" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="dt_CORREZIONEQUESTIONARIO" HeaderText="Data/ora comp/correz" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Esatte/Tot" ItemStyle-ForeColor="#009900" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("ni_RISPOSTEOK") & " / " & Eval("ni_RISPOSTE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comandi" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="EDIT" Text="&gt;Visualizza/modifica quest." runat="server" ID="btnModifica"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="CLEAR" Text="&gt;Cancella dati questionario" runat="server" ID="btnAzzera" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCorrettiKO" runat="server" Height="155px" Width="604px"
        Left="484px" Top="409px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCorrettiKO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsCorrettiKO" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Nominativi che non hanno superato il questionario" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="dt_CORREZIONEQUESTIONARIO" HeaderText="Data/ora comp/correz" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Esatte/Tot" ItemStyle-ForeColor="#990000" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("ni_RISPOSTEOK") & " / " & Eval("ni_RISPOSTE")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Comandi" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="EDIT" Text="&gt;Visualizza/modifica quest." runat="server" ID="btnModifica"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="CLEAR" Text="&gt;Cancella dati questionario" runat="server" ID="btnAzzera" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlSqlDataSource ID="sdsNonCorretti" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE ac_CATEGORIAECM='P' AND ac_STATOISCRIZIONE IN ('I','P') AND id_EVENTO=@id_EVENTO AND ac_STATOQUESTIONARIO=@ac_STATOQUESTIONARIO ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOQUESTIONARIO" Type="String" DefaultValue="NA" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsCorrettiOK" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE ac_CATEGORIAECM='P' AND ac_STATOISCRIZIONE IN ('I','P') AND id_EVENTO=@id_EVENTO AND ac_STATOQUESTIONARIO=@ac_STATOQUESTIONARIO ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOQUESTIONARIO" Type="String" DefaultValue="COK" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsCorrettiKO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE ac_CATEGORIAECM='P' AND ac_STATOISCRIZIONE IN ('I','P') AND id_EVENTO=@id_EVENTO AND ac_STATOQUESTIONARIO=@ac_STATOQUESTIONARIO ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOQUESTIONARIO" Type="String" DefaultValue="CKO" />
        </SelectParameters>
    </stl:StlSqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
