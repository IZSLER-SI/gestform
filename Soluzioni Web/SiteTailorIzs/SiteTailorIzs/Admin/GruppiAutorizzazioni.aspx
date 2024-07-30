<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="GruppiAutorizzazioni.aspx.vb" 
    Inherits="Softailor.SiteTailorIzs.GruppiAutorizzazioni" 
    title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updGRUPPI_g" runat="server" Width="300" Height="300" Top="0" Left="0">
        <ContentTemplate>    
            <stl:StlGridView ID="grdGRUPPI" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataKeyNames="ID_GRUPP" DataSourceID="sdsGRUPPI_g" 
                EnableViewState="False" ItemDescriptionPlural="gruppi" ItemDescriptionSingular="gruppo" 
                Title="Gruppi" BoundStlFormViewID="frmGRUPPI">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="DESGRUPP" HeaderText="Descrizione" 
                        SortExpression="DESGRUPP" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsGRUPPI_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                SelectCommand="SELECT [ID_GRUPP], [DESGRUPP] FROM [ac_GRUPPI] ORDER BY [DESGRUPP]" 
                DeleteCommand="DELETE FROM [ac_GRUPPI] WHERE [ID_GRUPP] = @ID_GRUPP" >
                <DeleteParameters>
                    <asp:Parameter Name="ID_GRUPP" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
    
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updGRUPPI_f" runat="server" Width="300" Height="140" Top="310" Left="0">
        <ContentTemplate>
            <stl:StlFormView ID="frmGRUPPI" runat="server" DataKeyNames="ID_GRUPP" 
                DataSourceID="sdsGRUPPI_f" BoundStlGridViewID="grdGRUPPI">
                <EditItemTemplate>
                    <span class="flbl" style="width:70px;">Descrizione</span>
                    <asp:TextBox ID="DESGRUPPTextBox" runat="server" 
                        Text='<%# Bind("DESGRUPP") %>' Width="216px" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsGRUPPI_f" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                InsertCommand="INSERT INTO [ac_GRUPPI] ([DESGRUPP]) VALUES (@DESGRUPP); SELECT @ID_GRUPP = SCOPE_IDENTITY()" 
                SelectCommand="SELECT [ID_GRUPP], [DESGRUPP] FROM [ac_GRUPPI] WHERE ([ID_GRUPP] = @ID_GRUPP)" 
                UpdateCommand="UPDATE [ac_GRUPPI] SET [DESGRUPP] = @DESGRUPP WHERE [ID_GRUPP] = @ID_GRUPP">
                <UpdateParameters>
                    <asp:Parameter Name="DESGRUPP" Type="String" />
                    <asp:Parameter Name="ID_GRUPP" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_GRUPP" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="DESGRUPP" Type="String" />
                    <asp:Parameter Name="ID_GRUPP" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updAUTGRP_g" runat="server" Width="600" Height="300" Top="0" Left="320">
        <ContentTemplate>    
            <stl:StlGridView ID="grdAUTGRP" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataKeyNames="ID_AUTGR" DataSourceID="sdsAUTGRP_g" 
                EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
                Title="Autorizzazioni del gruppo" BoundStlFormViewID="frmAUTGRP" ParentStlGridViewID="grdGRUPPI" >
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="SGLAPPLI" HeaderText="Applicazione" 
                        SortExpression="SGLAPPLI" />
                    <asp:BoundField DataField="DESFUNZI" HeaderText="Funzione" 
                        SortExpression="DESFUNZI" />
                    <asp:BoundField DataField="LIVELLOA" HeaderText="Livello" 
                        SortExpression="LIVELLOA" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsAUTGRP_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                DeleteCommand="DELETE FROM ac_AUTGRP WHERE ID_AUTGR = @ID_AUTGR"                 
                SelectCommand="SELECT ac_AUTGRP.ID_AUTGR, ac_AUTGRP.ID_GRUPP, ac_APPLIC.SGLAPPLI, ac_FUNZIO.DESFUNZI, ac_AUTGRP.LIVELLOA FROM ac_AUTGRP INNER JOIN ac_FUNZIO ON ac_AUTGRP.ID_FUNZI=ac_FUNZIO.ID_FUNZI INNER JOIN ac_APPLIC ON ac_FUNZIO.ID_APPLI=ac_APPLIC.ID_APPLI
WHERE ac_AUTGRP.ID_GRUPP = @ID_GRUPP
ORDER BY SGLAPPLI, DESFUNZI">
                <DeleteParameters>
                    <asp:Parameter Name="ID_AUTGR" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_GRUPP" />
                </SelectParameters>
            </stl:StlSqlDataSource>
    
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updAUTGRP_f" runat="server" Width="600" Height="140" Top="310" Left="320">
        <ContentTemplate>
    
            <stl:StlFormView ID="frmAUTGRP" runat="server" DataKeyNames="ID_AUTGR" 
                DataSourceID="sdsAUTGRP_f" BoundStlGridViewID="grdAUTGRP">
                <EditItemTemplate>
                    <span class="flbl" style="width:70px;">Funzione</span>
                    <asp:DropDownList ID="ID_FUNZIODropDownList" runat="server" 
                        DataSourceID="frmAUTGRP_desfunzi" DataTextField="DESFUNZI" 
                        DataValueField="ID_FUNZI" SelectedValue='<%# Bind("ID_FUNZI") %>' Width="516px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList><br />
                    <asp:SqlDataSource ID="frmAUTGRP_desfunzi" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                        SelectCommand="SELECT [ID_FUNZI], [DESFUNZI], [ID_APPLI] FROM [ac_FUNZIO] ORDER BY DESFUNZI">
                    </asp:SqlDataSource>
                    <span class="flbl" style="width:70px;">Livello</span>
                    <asp:DropDownList runat="server" ID="LIVELLOADropDownList" Width="50px" SelectedValue='<%# Bind("LIVELLOA") %>'>
                        <asp:ListItem Value="0" />
                        <asp:ListItem Value="1" />
                        <asp:ListItem Value="2" />
                        <asp:ListItem Value="3" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width:70px;vertical-align:top;">Note</span>
                    <asp:TextBox ID="ANNOTAZITextBox" runat="server" 
                        Text='<%# Bind("ANNOTAZI") %>' Width="512px" TextMode="MultiLine" Height="58px" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:stlSqlDataSource ID="sdsAUTGRP_f" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                InsertCommand="INSERT INTO [ac_AUTGRP] ([ID_GRUPP], [ID_FUNZI], [LIVELLOA], [ANNOTAZI]) VALUES (@ID_GRUPP, @ID_FUNZI, @LIVELLOA, @ANNOTAZI);SELECT @ID_AUTGR = SCOPE_IDENTITY()" 
                SelectCommand="SELECT [ID_AUTGR], [ID_FUNZI], [LIVELLOA], [ANNOTAZI] FROM [ac_AUTGRP] WHERE ([ID_AUTGR] = @ID_AUTGR)" 
                UpdateCommand="UPDATE [ac_AUTGRP] SET [ID_FUNZI] = @ID_FUNZI, [LIVELLOA] = @LIVELLOA, [ANNOTAZI] = @ANNOTAZI WHERE [ID_AUTGR] = @ID_AUTGR">
                <SelectParameters>
                    <asp:Parameter Name="ID_AUTGR" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                    <asp:Parameter Name="LIVELLOA" Type="Int32" />
                    <asp:Parameter Name="ANNOTAZI" Type="String" />
                    <asp:Parameter Name="ID_AUTGR" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="ID_GRUPP" Type="Int32" />
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                    <asp:Parameter Name="LIVELLOA" Type="Int32" />
                    <asp:Parameter Name="ANNOTAZI" Type="String" />
                    <asp:Parameter Name="ID_AUTGR" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
    
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
