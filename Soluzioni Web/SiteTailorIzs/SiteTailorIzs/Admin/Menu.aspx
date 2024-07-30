<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="menu.aspx.vb" 
    Inherits="Softailor.SiteTailorIzs.menu" 
    title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdContent" runat="server">
     <stl:StlUpdatePanel ID="updAPPLIC_g" runat="server" Height="200px" Width="250px" Left="0px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdAPPLIC" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataKeyNames="ID_APPLI" DataSourceID="sdsAPPLIC_g" 
                EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
                Title="Applicazioni" BoundStlFormViewID="" AllowDelete="False" AllowInsert="False">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField />
                    <asp:BoundField DataField="DESAPPLI" HeaderText="Applicazione" 
                        SortExpression="DESAPPLI" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsAPPLIC_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                DeleteCommand="DELETE FROM [ac_APPLIC] WHERE [ID_APPLI] = @ID_APPLI" 
                SelectCommand="SELECT [ID_APPLI], [SGLAPPLI], [DESAPPLI] FROM [ac_APPLIC] ORDER BY [ID_APPLI]" >
                <DeleteParameters>
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updMENUBA_g" runat="server"  Height="140px" Width="360px" Left="270px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdMENUBA" runat="server" AddCommandText="" 
                DataSourceID="sdsMENUBA_g" EnableViewState="False" ItemDescriptionPlural="" 
                ItemDescriptionSingular="" Title="Intestazioni menu" AutoGenerateColumns="False" 
                DataKeyNames="ID_MENUB" BoundStlFormViewID="frmMENUBA" ParentStlGridViewID="grdAPPLIC">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="NRORDIN" HeaderText="N.Ordine" 
                        SortExpression="NRORDIN" />
                    <asp:BoundField DataField="DESMENUB" HeaderText="Descrizione" 
                        SortExpression="DESMENUB" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsMENUBA_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                DeleteCommand="DELETE FROM [ac_MENUBA] WHERE [ID_MENUB] = @ID_MENUB" 
                SelectCommand="SELECT [ID_MENUB], [ID_APPLI], [DESMENUB], [NRORDIN] FROM [ac_MENUBA] WHERE ([ID_APPLI] = @ID_APPLI) ORDER BY [NRORDIN]"  >
                <DeleteParameters>
                    <asp:Parameter Name="ID_MENUB" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updMENUBA_f" runat="server"  Height="50px" Width="360px" Left="270px" Top="150px">
        <ContentTemplate>
            <stl:StlFormView ID="frmMENUBA" runat="server" DataKeyNames="ID_MENUB" 
                DataSourceID="sdsMENUBA_f" BoundStlGridViewID="grdMENUBA">
                <EditItemTemplate>
                    <span class="flbl" style="width:60px;">Descrizione</span>
                    <asp:TextBox ID="DESMENUBTextBox" runat="server" 
                        Text='<%# Bind("DESMENUB") %>' Width="180" />
                    <span class="slbl" style="width:60px;">N.Ordine</span>
                    <asp:TextBox ID="NRORDINTextBox" runat="server" Text='<%# Bind("NRORDIN") %>' Width="36" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsMENUBA_f" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                InsertCommand="INSERT INTO [ac_MENUBA] ([ID_APPLI], [DESMENUB], [NRORDIN]) VALUES (@ID_APPLI, @DESMENUB, @NRORDIN);SELECT @ID_MENUB = SCOPE_IDENTITY()" 
                SelectCommand="SELECT [ID_MENUB], [ID_APPLI], [DESMENUB], [NRORDIN] FROM [ac_MENUBA] WHERE ([ID_MENUB] = @ID_MENUB)" 
                UpdateCommand="UPDATE [ac_MENUBA] SET  [DESMENUB] = @DESMENUB, [NRORDIN] = @NRORDIN WHERE [ID_MENUB] = @ID_MENUB">
                <UpdateParameters>
                    <asp:Parameter Name="DESMENUB" Type="String" />
                    <asp:Parameter Name="NRORDIN" Type="Int32" />
                    <asp:Parameter Name="ID_MENUB" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_MENUB" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                    <asp:Parameter Name="DESMENUB" Type="String" />
                    <asp:Parameter Name="NRORDIN" Type="Int32" />
                    <asp:Parameter Name="ID_MENUB" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updMENUIT_g" runat="server"  Height="200px" Width="630px" Left="0px" Top="220px">
        <ContentTemplate>
            <stl:StlGridView ID="grdMENUIT" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataKeyNames="ID_MENUI" DataSourceID="sdsMENUIT_g" 
                EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
                Title="Voci del menu selezionato" BoundStlFormViewID="frmMENUIT" ParentStlGridViewID="grdMENUBA">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="NRORDII" HeaderText="N.Ordine" 
                        SortExpression="NRORDII" />
                    <asp:BoundField DataField="DESMENUI" HeaderText="Descrizione" 
                        SortExpression="DESMENUI" />
                    <asp:BoundField DataField="DESFUNZI" HeaderText="Funzione" 
                        SortExpression="DESFUNZI" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsMENUIT_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" DeleteCommand="DELETE FROM ac_MENUIT 
WHERE ID_MENUI = @ID_MENUI" SelectCommand="SELECT ac_MENUIT.ID_MENUI, ac_MENUIT.DESMENUI, ac_MENUIT.NRORDII, ac_FUNZIO.DESFUNZI
FROM ac_MENUIT INNER JOIN ac_FUNZIO ON ac_MENUIT.ID_FUNZI=ac_FUNZIO.ID_FUNZI 
WHERE ac_MENUIT.ID_MENUB = @ID_MENUB
ORDER BY NRORDII">
                <SelectParameters>
                    <asp:Parameter Name="ID_MENUB" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="ID_MENUI" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>    
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updMENUIT_f" runat="server"  Height="72px" Width="630px" Left="0px" Top="430px">
        <ContentTemplate>
            <stl:StlFormView ID="frmMENUIT" runat="server" DataKeyNames="ID_MENUI" 
                DataSourceID="sdsMENUIT_f" BoundStlGridViewID="grdMENUIT">
                <EditItemTemplate>
                    <span class="flbl" style="width:60px;">Descrizione</span>
                    <asp:TextBox ID="DESMENUITextBox" runat="server" 
                        Text='<%# Bind("DESMENUI") %>' Width="427px" />
                    <span class="slbl" style="width:60px;">N. Ordine</span>
                    <asp:TextBox ID="NRORDIITextBox" runat="server" Text='<%# Bind("NRORDII") %>' Width="60" />
                    <br />
                    <span class="flbl" style="width:60px;">Funzione</span>
                    <asp:DropDownList ID="ID_FUNZIDropDownList" runat="server" 
                        DataSourceID="frmMENUIT_desfunzi" DataTextField="DESFUNZI" 
                        DataValueField="ID_FUNZI" SelectedValue='<%# Bind("ID_FUNZI") %>' Width="555px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="frmMENUIT_desfunzi" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                        SelectCommand="SELECT [ID_FUNZI], [DESFUNZI], [ID_APPLI] FROM [ac_FUNZIO] ORDER BY DESFUNZI">
                    </asp:SqlDataSource>
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsMENUIT_f" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                InsertCommand="INSERT INTO [ac_MENUIT] ([ID_MENUB], [DESMENUI], [ID_FUNZI], [NRORDII]) VALUES (@ID_MENUB, @DESMENUI, @ID_FUNZI, @NRORDII); SELECT @ID_MENUI = SCOPE_IDENTITY()" 
                SelectCommand="SELECT [ID_MENUI], [ID_MENUB], [DESMENUI], [ID_FUNZI], [NRORDII] FROM [ac_MENUIT] WHERE ([ID_MENUI] = @ID_MENUI)" 
                UpdateCommand="UPDATE [ac_MENUIT] SET [DESMENUI] = @DESMENUI, [ID_FUNZI] = @ID_FUNZI, [NRORDII] = @NRORDII WHERE [ID_MENUI] = @ID_MENUI">
                <SelectParameters>
                    <asp:Parameter Name="ID_MENUI" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="DESMENUI" Type="String" />
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                    <asp:Parameter Name="NRORDII" Type="Int32" />
                    <asp:Parameter Name="ID_MENUI" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="ID_MENUB" Type="Int32" />
                    <asp:Parameter Name="DESMENUI" Type="String" />
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                    <asp:Parameter Name="NRORDII" Type="Int32" />
                    <asp:Parameter Name="ID_MENUI" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
