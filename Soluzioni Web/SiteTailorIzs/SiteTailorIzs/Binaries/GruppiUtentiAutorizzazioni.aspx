<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="GruppiUtentiAutorizzazioni.aspx.vb" Inherits="Softailor.SiteTailorIzs.GruppiUtentiAutorizzazioni" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
<stl:StlUpdatePanel ID="updGRUPPI_g" runat="server" Left="0" Top="0" Width="300" Height="300">
    <ContentTemplate>
      <stl:StlGridView ID="grdGRUPPI" AutoGenerateColumns="false" Runat="server" DataKeyNames="ID_GRUPP" 
      DataSourceID="sdsGRUPPI_g" EnableViewState="False" DeleteConfirmationQuestion="Cancellare?" 
      ItemDescriptionPlural="gruppi" ItemDescriptionSingular="gruppo" Title="Gruppi" BoundStlFormViewID="frmGRUPPI">
        <Columns>
          <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
          <asp:BoundField DataField="DESGRUPP" HeaderText="Desc gruppo" SortExpression="DESGRUPP" />
        </Columns>
      </stl:StlGridView>
      <stl:StlSqlDataSource ID="sdsGRUPPI_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
        DeleteCommand="DELETE FROM s8_GRUPPI WHERE ID_GRUPP = @ID_GRUPP" SelectCommand="SELECT * FROM s8_GRUPPI WHERE ID_AZIEN = @ID_AZIEN">
        <DeleteParameters>
          <asp:Parameter Name="ID_GRUPP" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <stl:StlUpdatePanel ID="updGRUPPI_f" runat="server" Left="0" Top="305" Width="300" Height="87">
    <ContentTemplate>
      <stl:StlFormView ID="frmGRUPPI" runat="server" DataSourceID="sdsGRUPPI_f" BoundStlGridViewID="grdGRUPPI" DataKeyNames="ID_GRUPP" NewItemText="Nuovo">
        <EditItemTemplate>
          <span class="flbl" style="width:65px">Desc gruppo</span>
          <asp:TextBox ID="DESGRUPPTextBox" runat="server" Text='<%# Bind("DESGRUPP") %>' Width="221px" /><br />
          <span class="flbl" style="width:65px">Desc estesa</span>
          <asp:TextBox ID="EXTGRUPPTextBox" runat="server" Text='<%# Bind("EXTGRUPP") %>' Width="221px" TextMode="MultiLine" />
        </EditItemTemplate>
      </stl:StlFormView>
      <stl:StlSqlDataSource ID="sdsGRUPPI_f" runat="server" 
      ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      InsertCommand="INSERT INTO [s8_GRUPPI] ([DESGRUPP], [EXTGRUPP], [ID_AZIEN])  VALUES (@DESGRUPP, @EXTGRUPP, @ID_AZIEN); SELECT @ID_GRUPP = SCOPE_IDENTITY();" 
      SelectCommand="SELECT * FROM s8_GRUPPI WHERE ID_GRUPP = @ID_GRUPP" 
      UpdateCommand="UPDATE [s8_GRUPPI] SET [DESGRUPP] =  @DESGRUPP, [EXTGRUPP] =  @EXTGRUPP WHERE [ID_GRUPP] =  @ID_GRUPP; ">
        <UpdateParameters>
          <asp:Parameter Name="DESGRUPP" Type="String" />
          <asp:Parameter Name="EXTGRUPP" Type="String" />
        </UpdateParameters>
        <InsertParameters>
          <asp:Parameter Name="DESGRUPP" Type="String" />
          <asp:Parameter Name="EXTGRUPP" Type="String" />
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
          <asp:Parameter Name="ID_GRUPP" Type="Int32" Direction="Output" />
        </InsertParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_GRUPP" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
<stl:StlUpdatePanel ID="updUTEGRP_g" runat="server" Left="0" Top="397" Width="300" Height="150">
    <ContentTemplate>
      <stl:StlGridView ID="grdUTEGRP" AutoGenerateColumns="false" Runat="server" DataKeyNames="ID_UTEGR" DataSourceID="sdsUTEGRP_g" EnableViewState="False" DeleteConfirmationQuestion="Cancellare?" ItemDescriptionPlural="utenti" ItemDescriptionSingular="utente" Title="Utenti" BoundStlFormViewID="frmUTEGRP" ParentStlGridViewID="grdGRUPPI">
        <Columns>
          <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
          <asp:BoundField DataField="NOMECOGN" HeaderText="Utente" SortExpression="NOMECOGN" />
        </Columns>
      </stl:StlGridView>
      <stl:StlSqlDataSource ID="sdsUTEGRP_g" runat="server" 
      ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      DeleteCommand="DELETE FROM s8_UTEGRP WHERE ID_UTEGR = @ID_UTEGR" 
      SelectCommand="SELECT ID_UTEGR, s8_UTEGRP.ID_UTENT, ac_UTENTI.COGNUTEN + ' ' + ac_UTENTI.NOMEUTEN AS NOMECOGN  FROM s8_UTEGRP INNER JOIN ac_UTENTI ON s8_UTEGRP.ID_UTENT = ac_UTENTI.ID_UTENT AND s8_UTEGRP.ID_AZIEN = ac_UTENTI.ID_AZIEN WHERE ID_GRUPP = @ID_GRUPP">
        <DeleteParameters>
          <asp:Parameter Name="ID_UTEGR" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_GRUPP" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <stl:StlUpdatePanel ID="updUTEGRP_f" runat="server" Left="0" Top="552" Width="300" Height="50">
    <ContentTemplate>
      <stl:StlFormView ID="frmUTEGRP" runat="server" DataSourceID="sdsUTEGRP_f" BoundStlGridViewID="grdUTEGRP" DataKeyNames="ID_UTEGR" NewItemText="Nuovo">
        <EditItemTemplate>
          <span class="flbl" style="width:65px">Utente</span>
          <asp:DropdownList ID="ID_UTENTDropdownList" runat="server" SelectedValue='<%# Bind("ID_UTENT") %>' Width="221px" DataSourceID="sdsUTEGRP_ID_UTENT" DataTextField="NOMECOGN" DataValueField="ID_UTENT" AppendDataBoundItems="true">
            <asp:ListItem Text="" Value="" />
          </asp:DropdownList>
        </EditItemTemplate>
      </stl:StlFormView>
      <stl:StlSqlDataSource ID="sdsUTEGRP_f" runat="server" 
      ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      InsertCommand="INSERT INTO [s8_UTEGRP] ([ID_AZIEN],[ID_UTENT], [ID_GRUPP])  VALUES (@ID_AZIEN, @ID_UTENT, @ID_GRUPP); SELECT @ID_UTEGR = SCOPE_IDENTITY();" 
      SelectCommand="SELECT * FROM s8_UTEGRP WHERE ID_UTEGR = @ID_UTEGR" 
      UpdateCommand="UPDATE [s8_UTEGRP] SET [ID_UTENT] =  @ID_UTENT WHERE [ID_UTEGR] =  @ID_UTEGR; ">
        <UpdateParameters>
          <asp:Parameter Name="ID_UTENT" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
          <asp:Parameter Name="ID_UTENT" Type="Int32" />
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
          <asp:Parameter Name="ID_GRUPP" Type="Int32" />
          <asp:Parameter Name="ID_UTEGR" Type="Int32" Direction="Output" />
        </InsertParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_UTEGR" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <stl:StlUpdatePanel ID="updAUTGRP_g" runat="server" Left="305" Top="0" Width="500" Height="300">
    <ContentTemplate>
      <stl:StlGridView ID="grdAUTGRP" AutoGenerateColumns="False" Runat="server" 
            DataKeyNames="ID_AUTGR" DataSourceID="sdsAUTGRP_g" EnableViewState="False" 
            DeleteConfirmationQuestion="Cancellare?" ItemDescriptionPlural="autorizzazioni" 
            ItemDescriptionSingular="autorizzazione" Title="Autorizzazioni" 
            BoundStlFormViewID="frmAUTGRP" ParentStlGridViewID="grdGRUPPI" 
            AddCommandText="">
        <Columns>
          <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
          <asp:BoundField DataField="DESCATEG" HeaderText="Categoria" SortExpression="DESCATEG" />
          <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# If(Eval("CAN_VIEW")=1,"<img src=""" & Page.resolveURL("~/img/icoV.gif") & """ />","")%>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Insert" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# If(Eval("CAN_INSE")=1,"<img src=""" & Page.resolveURL("~/img/icoV.gif") & """ />","")%>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Update" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# If(Eval("CAN_UPDA")=1,"<img src=""" & Page.resolveURL("~/img/icoV.gif") & """ />","")%>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%# If(Eval("CAN_DELE")=1,"<img src=""" & Page.resolveURL("~/img/icoV.gif") & """ />","")%>
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
      </stl:StlGridView>
      <stl:StlSqlDataSource ID="sdsAUTGRP_g" runat="server" 
      ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      DeleteCommand="DELETE FROM bd_AUTGRP WHERE ID_AUTGR = @ID_AUTGR" 
      SelectCommand="SELECT ID_AUTGR, ID_GRUPP, bd_CATEGO.DESCATEG, CAN_VIEW, CAN_INSE, CAN_UPDA, CAN_DELE 
                     FROM bd_AUTGRP INNER JOIN bd_CATEGO ON bd_AUTGRP.ID_CATEG = bd_CATEGO.ID_CATEG AND bd_AUTGRP.ID_AZIEN  = bd_CATEGO.ID_AZIEN WHERE bd_AUTGRP.ID_AZIEN = @ID_AZIEN AND bd_AUTGRP.ID_GRUPP = @ID_GRUPP ORDER BY bd_CATEGO.DESCATEG">
        <DeleteParameters>
          <asp:Parameter Name="ID_AUTGR" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
          <asp:Parameter Name="ID_GRUPP" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <stl:StlUpdatePanel ID="updAUTGRP_f" runat="server" Left="305" Top="305" Width="500" Height="87">
    <ContentTemplate>
      <stl:StlFormView ID="frmAUTGRP" runat="server" DataSourceID="sdsAUTGRP_f" BoundStlGridViewID="grdAUTGRP" DataKeyNames="ID_AUTGR" NewItemText="Nuovo">
        <EditItemTemplate>
          <span class="flbl" style="width:65px">Categoria</span>
          <asp:DropdownList ID="ID_CATEGDropdownList" runat="server" SelectedValue='<%# Bind("ID_CATEG") %>' Width="421px" DataSourceID="sdsAUTGRP_ID_CATEG" DataTextField="DESCATEG" DataValueField="ID_CATEG" AppendDataBoundItems="true">
            <asp:ListItem Text="" Value="" />
          </asp:DropdownList>
          <br />
          <span class="flbl" style="width:65px">View</span>
            <asp:DropDownList ID="CAV_VIEWDropDownList" runat="server" 
                SelectedValue='<%# Bind("CAN_VIEW") %>' Width="178px" >
                <asp:ListItem Text="Consentito" Value="1" />
                <asp:ListItem Text="Non consentito" Value="0" />
            </asp:DropDownList>
          <span class="slbl" style="width:65px">Insert</span>
            <asp:DropDownList ID="CAN_INSEDropDownList" runat="server" 
                SelectedValue='<%# Bind("CAN_INSE") %>' Width="178px" >
                <asp:ListItem Text="Consentito" Value="1" />
                <asp:ListItem Text="Non consentito" Value="0" />
            </asp:DropDownList>
          <br />
          <span class="flbl" style="width:65px">Update</span>
            <asp:DropDownList ID="CAN_UPDADropDownList" runat="server" 
                SelectedValue='<%# Bind("CAN_UPDA") %>' Width="178px" >
                <asp:ListItem Text="Consentito" Value="1" />
                <asp:ListItem Text="Non consentito" Value="0" />
            </asp:DropDownList>
          <span class="slbl" style="width:65px">Delete</span>
            <asp:DropDownList ID="CAN_DELEDropDownList" runat="server" 
                SelectedValue='<%# Bind("CAN_DELE") %>' Width="178px" >
                <asp:ListItem Text="Consentito" Value="1" />
                <asp:ListItem Text="Non consentito" Value="0" />
            </asp:DropDownList>
        </EditItemTemplate>
      </stl:StlFormView>
      <stl:StlSqlDataSource ID="sdsAUTGRP_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" InsertCommand="INSERT INTO [bd_AUTGRP] ([ID_CATEG], [CAN_VIEW], [CAN_INSE], [CAN_UPDA], [CAN_DELE], [ID_AZIEN], [ID_GRUPP])  VALUES (@ID_CATEG, @CAN_VIEW, @CAN_INSE, @CAN_UPDA, @CAN_DELE, @ID_AZIEN, @ID_GRUPP); SELECT @ID_AUTGR = SCOPE_IDENTITY();" SelectCommand="SELECT * FROM bd_AUTGRP WHERE ID_AUTGR = @ID_AUTGR" UpdateCommand="UPDATE [bd_AUTGRP] SET [ID_CATEG] =  @ID_CATEG, [CAN_VIEW] =  @CAN_VIEW, [CAN_INSE] =  @CAN_INSE, [CAN_UPDA] =  @CAN_UPDA, [CAN_DELE] =  @CAN_DELE WHERE [ID_AUTGR] =  @ID_AUTGR; ">
        <UpdateParameters>
          <asp:Parameter Name="ID_CATEG" Type="Int32" />
          <asp:Parameter Name="CAN_VIEW" Type="Int32" />
          <asp:Parameter Name="CAN_INSE" Type="Int32" />
          <asp:Parameter Name="CAN_UPDA" Type="Int32" />
          <asp:Parameter Name="CAN_DELE" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
          <asp:Parameter Name="ID_CATEG" Type="Int32" />
          <asp:Parameter Name="CAN_VIEW" Type="Int32" />
          <asp:Parameter Name="CAN_INSE" Type="Int32" />
          <asp:Parameter Name="CAN_UPDA" Type="Int32" />
          <asp:Parameter Name="CAN_DELE" Type="Int32" />
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
          <asp:Parameter Name="ID_GRUPP" Type="Int32" />
          <asp:Parameter Name="ID_AUTGR" Type="Int32" Direction="Output" />
        </InsertParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_AUTGR" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <asp:SqlDataSource ID="sdsUTEGRP_ID_UTENT" runat="server" 
  ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
  SelectCommand="SELECT ac_UTENTI.COGNUTEN + ' ' + ac_UTENTI.NOMEUTEN AS NOMECOGN, ID_UTENT FROM ac_UTENTI WHERE ID_AZIEN = @ID_AZIEN ORDER BY COGNUTEN, NOMEUTEN">
    <SelectParameters>
      <asp:Parameter Name="ID_AZIEN" Type="Int32" />
    </SelectParameters>
  </asp:SqlDataSource>
  <asp:SqlDataSource ID="sdsAUTGRP_ID_CATEG" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" SelectCommand="SELECT ID_CATEG, DESCATEG FROM bd_CATEGO WHERE ID_AZIEN = @ID_AZIEN ORDER BY DESCATEG">
    <SelectParameters>
      <asp:Parameter Name="ID_AZIEN" Type="Int32" />
    </SelectParameters>
  </asp:SqlDataSource>
</asp:Content>
