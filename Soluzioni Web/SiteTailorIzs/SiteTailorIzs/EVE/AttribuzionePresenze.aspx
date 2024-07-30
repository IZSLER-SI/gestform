<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="AttribuzionePresenze.aspx.vb" Inherits="Softailor.SiteTailorIzs.AttribuzionePresenze" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script src="../Scripts/jquery.qtip.min.js"></script>
    <link href="../Scripts/jquery.qtip.min.css" rel="stylesheet" />
    <div class="stl_dfo">
        <div class="title" style="position: absolute; left: 0px; top: 0px; width:370px;">
            Registrazione Presenze/assenze
        </div>
    </div>
    <stl:StlUpdatePanel ID="updIscritti" runat="server" Height="485px" Width="574px"
        Left="0px" Top="30px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIscritti" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsIscritti" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Iscritti" AllowReselectSelectedRow="true" AllowDelete="false" CommandsInLastColumn="true">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Minuti&lt;br/&gt;Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="P" Text="&gt;Presente" runat="server" ID="btnP" Font-Bold="true"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="PP" Text="&gt;Parzialmente presente" runat="server" ID="btnPP"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="AI" Text="&gt;Assente Ingiustificato" runat="server" ID="btnAI" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="AG" Text="&gt;Assente Giustificato" runat="server" ID="btnAG" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:UpdatePanel ID="updMoveAll" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="position:absolute; top:523px;left:0px;width:474px;font-size:11px;">
                Sposta tutti
                <asp:DropDownList ID="ddnMoveAll_Orig" EnableViewState="false" runat="server" CssClass="ddn">
                    <asp:ListItem Text="gli iscritti (indipendentemente dalla lettura del barcode)" Value="I" />
                    <asp:ListItem Text="gli iscritti ai quali è stato letto il barcode almeno una volta" Value="I_BC" />
                    <asp:ListItem Text="i presenti" Value="P" />
                    <asp:ListItem Text="i parzialmente presenti" Value="PP" />
                    <asp:ListItem Text="gli assenti ingiustificati" Value="AI" />
                    <asp:ListItem Text="gli assenti giustificati" Value="AG" />
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="ddnMoveAll_Dest" EnableViewState="false" runat="server" CssClass="ddn">
                    <asp:ListItem Text="nei presenti" Value="P" />
                    <asp:ListItem Text="nei parzialmente presenti" Value="PP" />
                    <asp:ListItem Text="negli iscritti" Value="I" />
                    <asp:ListItem Text="negli assenti ingiustificati" Value="AI" />
                    <asp:ListItem Text="negli assenti giustificati" Value="AG" />
                </asp:DropDownList>
                <asp:LinkButton CssClass="btnlink" ID="lnkMoveAll" runat="server" Font-Bold="true">Esegui</asp:LinkButton>
                <ajaxToolkit:ConfirmButtonExtender ID="cnfMoveAll" runat="server" TargetControlID="lnkMoveAll" ConfirmText="Confermi l'operazione?" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <stl:StlUpdatePanel ID="updPresenti" runat="server" Height="203px" Width="574px"
        Left="584px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPresenti" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsPresenti" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Presenti" AllowReselectSelectedRow="true" AllowDelete="false" CommandsInLastColumn="true">
                <Columns>
                    <asp:BoundField />
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Minuti&lt;br/&gt;Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritto" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="PP" Text="&gt;Parzialmente presente" runat="server" ID="btnPP"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="AI" Text="&gt;Assente Ingiustificato" runat="server" ID="btnAI" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="AG" Text="&gt;Assente Giustificato" runat="server" ID="btnAG" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPresentiParziali" runat="server" Height="159px" Width="574px"
        Left="584px" Top="208px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPresentiParziali" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsPresentiParziali" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Parzialmente Presenti" AllowReselectSelectedRow="true" AllowDelete="false" CommandsInLastColumn="true">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritto" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="P" Text="&gt;Presente" runat="server" ID="btnP" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="AI" Text="&gt;Assente Ingiustificato" runat="server" ID="btnAI" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="AG" Text="&gt;Assente Giustificato" runat="server" ID="btnAG" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />  
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updAssentiIngiustificati" runat="server" Height="157px" Width="574px"
        Left="584px" Top="372px">
        <ContentTemplate>
            <stl:StlGridView ID="grdAssentiIngiustificati" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsAssentiIngiustificati" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Assenti Ingiustificati" AllowReselectSelectedRow="true" AllowDelete="false" CommandsInLastColumn="true">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Minuti&lt;br/&gt;Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritto" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="P" Text="&gt;Presente" runat="server" ID="btnP" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="PP" Text="&gt;Parzialmente presente" runat="server" ID="btnPP"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="AG" Text="&gt;Assente Giustificato" runat="server" ID="btnAG" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updAssentiGiustificati" runat="server" Height="159px" Width="574px"
        Left="584px" Top="534px">
        <ContentTemplate>
            <stl:StlGridView ID="grdAssentiGiustificati" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsAssentiGiustificati" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Assenti Giustificati" AllowReselectSelectedRow="true" AllowDelete="false" CommandsInLastColumn="true">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="I" Text="&gt;Iscritto" runat="server" ID="btnI"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="P" Text="&gt;Presente" runat="server" ID="btnP" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br /> 
                            <asp:LinkButton CssClass="rowBtn" CommandName="PP" Text="&gt;Parzialmente presente" runat="server" ID="btnPP"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="AI" Text="&gt;Assente Ingiustificato" runat="server" ID="btnAI" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlSqlDataSource ID="sdsIscritti" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE AND ac_CATEGORIAECM<>'RS' ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="I" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsPresenti" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE AND ac_CATEGORIAECM<>'RS' ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="P" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsPresentiParziali" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE AND ac_CATEGORIAECM<>'RS' ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="PP" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsAssentiIngiustificati" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE AND ac_CATEGORIAECM<>'RS' ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="AI" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsAssentiGiustificati" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE AND ac_CATEGORIAECM<>'RS' ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" DefaultValue="AG" />
        </SelectParameters>
    </stl:StlSqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
