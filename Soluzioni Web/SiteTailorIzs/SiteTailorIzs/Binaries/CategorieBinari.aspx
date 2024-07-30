<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CategorieBinari.aspx.vb" Inherits="Softailor.SiteTailorIzs.CategorieBinari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
<stl:StlUpdatePanel ID="updCATEGO_g" runat="server" Left="0" Top="0" Width="570px" 
        Height="300">
    <ContentTemplate>
      <stl:StlGridView ID="grdCATEGO" AutoGenerateColumns="false" Runat="server" DataKeyNames="ID_CATEG" DataSourceID="sdsCATEGO_g" EnableViewState="False" ItemDescriptionPlural="categorie" ItemDescriptionSingular="categoria" Title="Categorie immagini/allegati" BoundStlFormViewID="frmCATEGO">
        <Columns>
          <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
          <asp:BoundField DataField="CODCATEG" HeaderText="Codice" SortExpression="CODCATEG" />
          <asp:BoundField DataField="DESCATEG" HeaderText="Descrizione" SortExpression="DESCATEG" />
          <asp:BoundField DataField="MAX_SIZE" HeaderText="Dim max KB" ItemStyle-HorizontalAlign="Right" />
          <asp:TemplateField HeaderText="Dim min img">
            <ItemTemplate>
                <%# If(IsDBNull(Eval("MIN_WIDT")), "?", Eval("MIN_WIDT")) &
                                " x " &
                                If(IsDBNull(Eval("MIN_HEIG")), "?", Eval("MIN_HEIG"))                    
                %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Dim max img">
            <ItemTemplate>
                <%# If(isDbNull(Eval("MAX_WIDT")),"?",Eval("MAX_WIDT")) & _
                    " x " & _
                    If(isDbNull(Eval("MAX_HEIG")),"?",Eval("MAX_HEIG"))                    
                %>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
      </stl:StlGridView>
      <stl:StlSqlDataSource ID="sdsCATEGO_g" runat="server" 
      ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      DeleteCommand="DELETE FROM bd_CATEGO WHERE ID_CATEG = @ID_CATEG" 
      SelectCommand="SELECT ID_CATEG, ID_AZIEN, CODCATEG, DESCATEG, MAX_SIZE, MIN_WIDT, MAX_WIDT, MIN_HEIG, MAX_HEIG FROM bd_CATEGO WHERE ID_AZIEN = @ID_AZIEN ORDER BY DESCATEG">
        <DeleteParameters>
          <asp:Parameter Name="ID_CATEG" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <stl:StlUpdatePanel ID="updCATEGO_f" runat="server" Left="0" Top="305" 
        Width="570px" Height="307">
    <ContentTemplate>
      <stl:StlFormView ID="frmCATEGO" runat="server" DataSourceID="sdsCATEGO_f" BoundStlGridViewID="grdCATEGO" DataKeyNames="ID_CATEG">
        <EditItemTemplate>
          <span class="flbl" style="width:65px">Codice</span>
          <asp:TextBox ID="CODCATEGTextBox" runat="server" Text='<%# Bind("CODCATEG") %>' Width="128px" />
          <span class="slbl" style="width:70px">Descrizione</span>
          <asp:TextBox ID="DESCATEGTextBox" runat="server" Text='<%# Bind("DESCATEG") %>' Width="223px" /><br />
          
          <b><span class="flbl" style="width:486px">Vincoli immagine/allegato</span></b><br />
          <span class="flbl" style="width:65px">Largh. min</span>
          <asp:TextBox ID="MIN_WIDTTextBox" runat="server" Text='<%# Bind("MIN_WIDT") %>' Width="53px" />
          <span class="slbl" style="width:65px">Largh. max</span>
          <asp:TextBox ID="MAX_WIDTTextBox" runat="server" Text='<%# Bind("MAX_WIDT") %>' Width="53px" /><br />
          <span class="flbl" style="width:65px">Alt. min</span>
          <asp:TextBox ID="MIN_HEIGTextBox" runat="server" Text='<%# Bind("MIN_HEIG") %>' Width="53px" />
          <span class="slbl" style="width:65px">Alt. max</span>
          <asp:TextBox ID="MAX_HEIGTextBox" runat="server" Text='<%# Bind("MAX_HEIG") %>' Width="53px" />
          <span class="slbl" style="width:85px">Dim. max (KB)</span>
          <asp:TextBox ID="MAX_SIZETextBox" runat="server" Text='<%# Bind("MAX_SIZE") %>' Width="53px" />
          
          <b><span class="flbl" style="width:486px">Primo thumbnail</span></b><br />
          <span class="flbl" style="width:65px">Gen. auto</span>
          <asp:CheckBox ID="THUMB1_GCheckBox" runat="server" checked='<%# Bind("THUMB1_G") %>' Width="53px" />
          <span class="slbl" style="width:63px">Estensione</span>
            <asp:DropDownList ID="THUMB1_EDropDownList" runat="server" 
                SelectedValue='<%# Bind("THUMB1_E") %>' Width="57px" >
                <asp:ListItem Text="" Value="" />
                <asp:ListItem Text=".gif" Value=".gif" />
                <asp:ListItem Text=".jpg" Value=".jpg" />
                <asp:ListItem Text=".png" Value=".png" />
            </asp:DropDownList>
          <span class="slbl" style="width:85px">Qualità (jpeg)</span>
          <asp:TextBox ID="THUMB1_QTextBox" runat="server" Text='<%# Bind("THUMB1_Q") %>' Width="53px" /><br />
          <span class="flbl" style="width:65px">Larghezza</span>
          <asp:TextBox ID="THUMB1_WTextBox" runat="server" Text='<%# Bind("THUMB1_W") %>' Width="53px" />
          <span class="slbl" style="width:65px">Altezza</span>
          <asp:TextBox ID="THUMB1_HTextBox" runat="server" Text='<%# Bind("THUMB1_H") %>' Width="53px" /><br />
          <b><span class="flbl" style="width:486px">Secondo thumbnail</span></b><br />
          <span class="flbl" style="width:65px">Gen. auto</span>
          <asp:CheckBox ID="THUMB2_GCheckBox" runat="server" checked='<%# Bind("THUMB2_G") %>' Width="53px" />
          <span class="slbl" style="width:63px">Estensione</span>
            <asp:DropDownList ID="THUMB2_EDropDownList" runat="server" 
                SelectedValue='<%# Bind("THUMB2_E") %>' Width="57px" >
                <asp:ListItem Text="" Value="" />
                <asp:ListItem Text=".gif" Value=".gif" />
                <asp:ListItem Text=".jpg" Value=".jpg" />
                <asp:ListItem Text=".png" Value=".png" />
            </asp:DropDownList>
          <span class="slbl" style="width:85px">Qualità (jpeg)</span>
          <asp:TextBox ID="THUMB2_QTextBox" runat="server" Text='<%# Bind("THUMB2_Q") %>' Width="53px" /><br />
          <span class="flbl" style="width:65px">Larghezza</span>
          <asp:TextBox ID="THUMB2_WTextBox" runat="server" Text='<%# Bind("THUMB2_W") %>' Width="53px" />
          <span class="slbl" style="width:65px">Altezza</span>
          <asp:TextBox ID="THUMB2_HTextBox" runat="server" Text='<%# Bind("THUMB2_H") %>' Width="53px" /><br />
          <b><span class="flbl" style="width:486px">Terzo thumbnail</span></b><br />
          <span class="flbl" style="width:65px">Gen. auto</span>
          <asp:CheckBox ID="THUMB3_GCheckBox" runat="server" checked='<%# Bind("THUMB3_G") %>' Width="53px" />
          <span class="slbl" style="width:63px">Estensione</span>
            <asp:DropDownList ID="THUMB3_EDropDownList" runat="server" 
                SelectedValue='<%# Bind("THUMB3_E") %>' Width="57px" >
                <asp:ListItem Text="" Value="" />
                <asp:ListItem Text=".gif" Value=".gif" />
                <asp:ListItem Text=".jpg" Value=".jpg" />
                <asp:ListItem Text=".png" Value=".png" />
            </asp:DropDownList>
          <span class="slbl" style="width:85px">Qualità (jpeg)</span>
          <asp:TextBox ID="THUMB3_QTextBox" runat="server" Text='<%# Bind("THUMB3_Q") %>' Width="53px" /><br />
          <span class="flbl" style="width:65px">Larghezza</span>
          <asp:TextBox ID="THUMB3_WTextBox" runat="server" Text='<%# Bind("THUMB3_W") %>' Width="53px" />
          <span class="slbl" style="width:65px">Altezza</span>
          <asp:TextBox ID="THUMB3_HTextBox" runat="server" Text='<%# Bind("THUMB3_H") %>' Width="53px" /><br />
          
        </EditItemTemplate>
      </stl:StlFormView>
      <stl:StlSqlDataSource ID="sdsCATEGO_f" runat="server" 
      ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      InsertCommand="INSERT INTO [bd_CATEGO] ([ID_AZIEN], [CODCATEG], [DESCATEG], [THUMB1_G], [THUMB1_E], [THUMB1_Q], [THUMB1_W], [THUMB1_H], [THUMB2_G], [THUMB2_E], [THUMB2_Q], [THUMB2_W], [THUMB2_H], [THUMB3_G], [THUMB3_E], [THUMB3_Q], [THUMB3_W], [THUMB3_H], [MAX_SIZE], [MIN_WIDT], [MAX_WIDT], [MIN_HEIG], [MAX_HEIG])  VALUES (@ID_AZIEN, @CODCATEG, @DESCATEG, @THUMB1_G, @THUMB1_E, @THUMB1_Q, @THUMB1_W, @THUMB1_H, @THUMB2_G, @THUMB2_E, @THUMB2_Q, @THUMB2_W, @THUMB2_H, @THUMB3_G, @THUMB3_E, @THUMB3_Q, @THUMB3_W, @THUMB3_H, @MAX_SIZE, @MIN_WIDT, @MAX_WIDT, @MIN_HEIG, @MAX_HEIG); SELECT @ID_CATEG = SCOPE_IDENTITY();" SelectCommand="SELECT * FROM bd_CATEGO WHERE ID_CATEG = @ID_CATEG" UpdateCommand="UPDATE [bd_CATEGO] SET [CODCATEG] =  @CODCATEG, [DESCATEG] =  @DESCATEG, [THUMB1_G] =  @THUMB1_G, [THUMB1_E] =  @THUMB1_E, [THUMB1_Q] =  @THUMB1_Q, [THUMB1_W] =  @THUMB1_W, [THUMB1_H] =  @THUMB1_H, [THUMB2_G] =  @THUMB2_G, [THUMB2_E] =  @THUMB2_E, [THUMB2_Q] =  @THUMB2_Q, [THUMB2_W] =  @THUMB2_W, [THUMB2_H] =  @THUMB2_H, [THUMB3_G] =  @THUMB3_G, [THUMB3_E] =  @THUMB3_E, [THUMB3_Q] =  @THUMB3_Q, [THUMB3_W] =  @THUMB3_W, [THUMB3_H] =  @THUMB3_H, [MAX_SIZE] =  @MAX_SIZE, [MIN_WIDT] =  @MIN_WIDT, [MAX_WIDT] =  @MAX_WIDT, [MIN_HEIG] =  @MIN_HEIG, [MAX_HEIG] =  @MAX_HEIG WHERE [ID_CATEG] =  @ID_CATEG; ">
        <UpdateParameters>
          <asp:Parameter Name="CODCATEG" Type="String" />
          <asp:Parameter Name="DESCATEG" Type="String" />
          <asp:Parameter Name="THUMB1_G" Type="Boolean" />
          <asp:Parameter Name="THUMB1_E" Type="String" />
          <asp:Parameter Name="THUMB1_Q" Type="Int32" />
          <asp:Parameter Name="THUMB1_W" Type="Int32" />
          <asp:Parameter Name="THUMB1_H" Type="Int32" />
          <asp:Parameter Name="THUMB2_G" Type="Boolean" />
          <asp:Parameter Name="THUMB2_E" Type="String" />
          <asp:Parameter Name="THUMB2_Q" Type="Int32" />
          <asp:Parameter Name="THUMB2_W" Type="Int32" />
          <asp:Parameter Name="THUMB2_H" Type="Int32" />
          <asp:Parameter Name="THUMB3_G" Type="Boolean" />
          <asp:Parameter Name="THUMB3_E" Type="String" />
          <asp:Parameter Name="THUMB3_Q" Type="Int32" />
          <asp:Parameter Name="THUMB3_W" Type="Int32" />
          <asp:Parameter Name="THUMB3_H" Type="Int32" />
          <asp:Parameter Name="MAX_SIZE" Type="Int32" />
          <asp:Parameter Name="MIN_WIDT" Type="Int32" />
          <asp:Parameter Name="MAX_WIDT" Type="Int32" />
          <asp:Parameter Name="MIN_HEIG" Type="Int32" />
          <asp:Parameter Name="MAX_HEIG" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
          <asp:Parameter Name="ID_AZIEN" Type="Int32" />
          <asp:Parameter Name="CODCATEG" Type="String" />
          <asp:Parameter Name="DESCATEG" Type="String" />
          <asp:Parameter Name="THUMB1_G" Type="Boolean" />
          <asp:Parameter Name="THUMB1_E" Type="String" />
          <asp:Parameter Name="THUMB1_Q" Type="Int32" />
          <asp:Parameter Name="THUMB1_W" Type="Int32" />
          <asp:Parameter Name="THUMB1_H" Type="Int32" />
          <asp:Parameter Name="THUMB2_G" Type="Boolean" />
          <asp:Parameter Name="THUMB2_E" Type="String" />
          <asp:Parameter Name="THUMB2_Q" Type="Int32" />
          <asp:Parameter Name="THUMB2_W" Type="Int32" />
          <asp:Parameter Name="THUMB2_H" Type="Int32" />
          <asp:Parameter Name="THUMB3_G" Type="Boolean" />
          <asp:Parameter Name="THUMB3_E" Type="String" />
          <asp:Parameter Name="THUMB3_Q" Type="Int32" />
          <asp:Parameter Name="THUMB3_W" Type="Int32" />
          <asp:Parameter Name="THUMB3_H" Type="Int32" />
          <asp:Parameter Name="MAX_SIZE" Type="Int32" />
          <asp:Parameter Name="MIN_WIDT" Type="Int32" />
          <asp:Parameter Name="MAX_WIDT" Type="Int32" />
          <asp:Parameter Name="MIN_HEIG" Type="Int32" />
          <asp:Parameter Name="MAX_HEIG" Type="Int32" />
          <asp:Parameter Name="ID_CATEG" Type="Int32" Direction="Output" />
        </InsertParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_CATEG" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  <stl:StlUpdatePanel ID="updFORCAT_g" runat="server" Left="575px" Top="0" 
        Width="300px" Height="300">
    <ContentTemplate>
      <stl:StlGridView ID="grdFORCAT" AutoGenerateColumns="false" Runat="server" DataKeyNames="ID_FORCA" DataSourceID="sdsFORCAT_g" EnableViewState="False" DeleteConfirmationQuestion="" ItemDescriptionPlural="formati" ItemDescriptionSingular="formato" Title="Formati consentiti" BoundStlFormViewID="frmFORCAT" ParentStlGridViewID="grdCATEGO">
        <Columns>
          <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
          <asp:BoundField DataField="DESFORMA" HeaderText="Formato" SortExpression="CODFORMA" />
          <asp:BoundField DataField="MAX_SIZE" HeaderText="Max KB" SortExpression="MAX_SIZE" />
        </Columns>
      </stl:StlGridView>
      <stl:StlSqlDataSource ID="sdsFORCAT_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
      DeleteCommand="DELETE FROM bd_FORCAT WHERE ID_FORCA = @ID_FORCA" 
      SelectCommand="SELECT bd_FORCAT.ID_FORCA, bd_FORMAT.DESFORMA, bd_FORCAT.MAX_SIZE FROM bd_FORCAT INNER JOIN bd_FORMAT ON bd_FORCAT.CODFORMA=bd_FORMAT.CODFORMA WHERE bd_FORCAT.ID_CATEG = @ID_CATEG ORDER BY bd_FORMAT.DESFORMA">
        <DeleteParameters>
          <asp:Parameter Name="ID_FORCA" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_CATEG" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updFORCAT_f" runat="server" Left="575px" Top="305" 
        Width="300px" Height="68">
    <ContentTemplate>
      <stl:StlFormView ID="frmFORCAT" runat="server" DataSourceID="sdsFORCAT_f" BoundStlGridViewID="grdFORCAT" DataKeyNames="ID_FORCA" NewItemText="Nuovo">
        <EditItemTemplate>
          <span class="flbl" style="width:65px">Formato</span>
          <asp:DropdownList ID="CODFORMADropdownList" runat="server" SelectedValue='<%# Bind("CODFORMA") %>' Width="222px" DataSourceID="sdsFORCAT_CODFORMA" 
              DataTextField="DESFORMA" DataValueField="CODFORMA" AppendDataBoundItems="true">
            <asp:ListItem Text="" Value="" />
          </asp:DropdownList>
          <asp:SqlDataSource ID="sdsFORCAT_CODFORMA" runat="server" 
          ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
          SelectCommand="SELECT CODFORMA, DESFORMA FROM bd_FORMAT ORDER BY DESFORMA" />
          <span class="flbl" style="width:65px">Dim max KB</span>
          <asp:TextBox ID="MAX_SIZETextBox" runat="server" Text='<%# Bind("MAX_SIZE") %>' Width="100px" />
        </EditItemTemplate>
      </stl:StlFormView>
      <stl:StlSqlDataSource ID="sdsFORCAT_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" InsertCommand="INSERT INTO [bd_FORCAT] ([CODFORMA], [MAX_SIZE], [ID_CATEG])  VALUES (@CODFORMA, @MAX_SIZE, @ID_CATEG); SELECT @ID_FORCA = SCOPE_IDENTITY();" SelectCommand="SELECT * FROM bd_FORCAT WHERE ID_FORCA = @ID_FORCA" UpdateCommand="UPDATE [bd_FORCAT] SET [CODFORMA] =  @CODFORMA, [MAX_SIZE] =  @MAX_SIZE WHERE [ID_FORCA] =  @ID_FORCA; ">
        <UpdateParameters>
          <asp:Parameter Name="CODFORMA" Type="String" />
          <asp:Parameter Name="MAX_SIZE" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
          <asp:Parameter Name="CODFORMA" Type="String" />
          <asp:Parameter Name="MAX_SIZE" Type="Int32" />
          <asp:Parameter Name="ID_CATEG" Type="Int32" />
          <asp:Parameter Name="ID_FORCA" Type="Int32" Direction="Output" />
        </InsertParameters>
        <SelectParameters>
          <asp:Parameter Name="ID_FORCA" Type="Int32" />
        </SelectParameters>
      </stl:StlSqlDataSource>
    </ContentTemplate>
  </stl:StlUpdatePanel>
  
</asp:Content>
