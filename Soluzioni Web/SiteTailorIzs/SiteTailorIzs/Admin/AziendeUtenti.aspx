<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" 
    CodeBehind="AziendeUtenti.aspx.vb" Inherits="Softailor.SiteTailorIzs.AziendeUtenti"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdContent" runat="server">
                <stl:StlUpdatePanel ID="updAZIEND_g" runat="server" 
                    Height="100px" Width="910px" Left="0px" Top="0px">
                    <ContentTemplate>
                        <stl:StlGridView ID="grdAZIEND" runat="server" AddCommandText="" 
                            AutoGenerateColumns="False" DataKeyNames="ID_AZIEN" DataSourceID="sdsAZIEND_g" 
                            EnableViewState="False" ItemDescriptionPlural="" 
                            ItemDescriptionSingular="" BoundStlFormViewID="frmAZIEND" 
                            Title="Aziende">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                <asp:BoundField DataField="ID_AZIEN" HeaderText="Id" 
                                    InsertVisible="False" ReadOnly="True" SortExpression="ID_AZIEN" />
                                <asp:BoundField DataField="NOMAZIEN" HeaderText="Codice" 
                                    SortExpression="NOMAZIEN" />
                                <asp:BoundField DataField="RAGSOCIA" HeaderText="Ragione Sociale" 
                                    SortExpression="RAGSOCIA" />
                                <asp:BoundField DataField="EMAIL_AZ" HeaderText="E-mail" 
                                    SortExpression="EMAIL_AZ" />
                                <asp:BoundField DataField="AZIDISAB" HeaderText="Disab" 
                                    SortExpression="AZIDISAB" />
                            </Columns>
                        </stl:StlGridView>
                        <stl:StlSqlDataSource ID="sdsAZIEND_g" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                            DeleteCommand="DELETE FROM [ac_AZIEND] WHERE [ID_AZIEN] = @ID_AZIEN" 
                            SelectCommand="SELECT [ID_AZIEN], [NOMAZIEN], [RAGSOCIA], [EMAIL_AZ], [AZIDISAB] FROM [ac_AZIEND] ORDER BY [ID_AZIEN]" >
                            <DeleteParameters>
                                <asp:Parameter Name="ID_AZIEN" Type="Int32" />
                            </DeleteParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
                <stl:StlUpdatePanel ID="updAZIEND_f" runat="server" Height="70px" Width="910px" Left="0px" Top="105px">
                    <ContentTemplate>
                        <stl:StlFormView ID="frmAZIEND" runat="server" DataKeyNames="ID_AZIEN" 
                            DataSourceID="sdsAZIEND_f" BoundStlGridViewID="grdAZIEND">
                            <EditItemTemplate>
                                <span class="flbl" style="width:30px;">Id</span>
                                <asp:Textbox ID="ID_AZIENTextBox" runat="server" Text='<%# Eval("ID_AZIEN") %>'  ReadOnly="true" Width="56" />
                                <span class="slbl" style="width:50px;">Codice</span>
                                <asp:TextBox ID="NOMAZIENTextBox" runat="server" 
                                    Text='<%# Bind("NOMAZIEN") %>' Width="90px"  />
                                <span class="slbl" style="width:100px;">Ragione Sociale</span>
                                <asp:TextBox ID="RAGSOCIATextBox" runat="server" 
                                    Text='<%# Bind("RAGSOCIA") %>' Width="460px" />
                                <span class="slbl" style="width:80px;">Disabilitata</span>
                                <asp:CheckBox ID="AZIDISABCheckBox" runat="server" 
                                    Checked='<%# Bind("AZIDISAB") %>' />
                                <br />
                                <span class="flbl" style="width:105px;">E-mail di riferimento</span>
                                <asp:TextBox ID="EMAIL_AZTextBox" runat="server" 
                                    Text='<%# Bind("EMAIL_AZ") %>' Width="788px" />
                            </EditItemTemplate>
                        </stl:StlFormView>
                        <stl:StlSqlDataSource ID="sdsAZIEND_f" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                            InsertCommand="INSERT INTO [ac_AZIEND] ([NOMAZIEN], [RAGSOCIA], [EMAIL_AZ], [AZIDISAB]) VALUES (@NOMAZIEN, @RAGSOCIA, @EMAIL_AZ, @AZIDISAB); SELECT @ID_AZIEN = SCOPE_IDENTITY()" 
                            SelectCommand="SELECT [ID_AZIEN], [NOMAZIEN], [RAGSOCIA], [EMAIL_AZ], [AZIDISAB] FROM [ac_AZIEND] WHERE ([ID_AZIEN] = @ID_AZIEN)" 
                            UpdateCommand="UPDATE [ac_AZIEND] SET [NOMAZIEN] = @NOMAZIEN, [RAGSOCIA] = @RAGSOCIA, [EMAIL_AZ] = @EMAIL_AZ, [AZIDISAB] = @AZIDISAB WHERE [ID_AZIEN] = @ID_AZIEN">
                            <UpdateParameters>
                                <asp:Parameter Name="NOMAZIEN" Type="String" />
                                <asp:Parameter Name="RAGSOCIA" Type="String" />
                                <asp:Parameter Name="EMAIL_AZ" Type="String" />
                                <asp:Parameter Name="AZIDISAB" Type="Boolean" />
                                <asp:Parameter Name="ID_AZIEN" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_AZIEN" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="NOMAZIEN" Type="String" />
                                <asp:Parameter Name="RAGSOCIA" Type="String" />
                                <asp:Parameter Name="EMAIL_AZ" Type="String" />
                                <asp:Parameter Name="AZIDISAB" Type="Boolean" />
                                <asp:Parameter Name="ID_AZIEN" Type="Int32" Direction="Output"/>
                            </InsertParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
          
                <stl:StlUpdatePanel ID="updUTENTI_g" runat="server" Height="150px" Width="400px" Left="0px" Top="185px">
                    <ContentTemplate>
                        <stl:StlGridView ID="grdUTENTI" runat="server" 
                            AddCommandText="" AutoGenerateColumns="False" DataKeyNames="ID_UTENT" 
                            DataSourceID="sdsUTENTI_g" EnableViewState="False" ItemDescriptionPlural="" 
                            ItemDescriptionSingular="" BoundStlFormViewID="frmUTENTI" 
                            ParentStlGridViewID="grdAZIEND" Title="Utenti dell'azienda">
                            <Columns>
                            <asp:CommandField ShowDeleteButton="true" 
                                    ShowSelectButton="True" />
                                <asp:BoundField DataField="ID_UTENT" HeaderText="Id" 
                                    InsertVisible="False" ReadOnly="True" SortExpression="ID_UTENT" />
                                <asp:BoundField DataField="COGNUTEN" HeaderText="Cognome" 
                                    SortExpression="COGNUTEN" />
                                <asp:BoundField DataField="NOMEUTEN" HeaderText="Nome" 
                                    SortExpression="NOMEUTEN" />
                                <asp:BoundField DataField="USERNAME" HeaderText="Username" 
                                    SortExpression="USERNAME" />
                                <asp:BoundField DataField="UTEDISAB" HeaderText="Disab" 
                                    SortExpression="UTEDISAB" />
                            </Columns>
                        </stl:StlGridView>
                        <stl:StlSqlDataSource ID="sdsUTENTI_g" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                            DeleteCommand="DELETE FROM [ac_UTENTI] WHERE [ID_UTENT] = @ID_UTENT" 
                            SelectCommand="SELECT [ID_UTENT], [ID_AZIEN], [COGNUTEN], [NOMEUTEN], [EMAIL_UT], [USERNAME], [UTEDISAB], [PASSWORD] FROM [ac_UTENTI] WHERE ([ID_AZIEN] = @ID_AZIEN) ORDER BY [COGNUTEN], [NOMEUTEN]" >
                            <DeleteParameters>
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_AZIEN" Type="Int32" />
                            </SelectParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
                <stl:StlUpdatePanel ID="updUTENTI_f" runat="server" Height="90px" Width="400px" Left="0px" Top="340px">
                    <ContentTemplate>
                        <stl:StlFormView ID="frmUTENTI" runat="server" BoundStlGridViewID="grdUTENTI" 
                            DataKeyNames="ID_UTENT" DataSourceID="sdsUTENTI_f">
                            <EditItemTemplate>
                                <span class="flbl" style="width:50px;">Cognome</span>
                                <asp:TextBox ID="COGNUTENTextBox" runat="server" 
                                    Text='<%# Bind("COGNUTEN") %>' Width="140px" Font-Bold="true" />
                                <span class="slbl" style="width:60px;">Nome</span>
                                <asp:TextBox ID="NOMEUTENTextBox" runat="server" 
                                    Text='<%# Bind("NOMEUTEN") %>' Font-Bold="true" Width="130"  />
                                <br />
                                <span class="flbl" style="width:50px;">Username</span>
                                <asp:TextBox ID="USERNAMETextBox" runat="server" 
                                    Text='<%# Bind("USERNAME") %>' Width="140px" />
                                <span class="slbl" style="width:60px;">Password</span>
                                <asp:TextBox ID="PASSWORDTextBox" runat="server" 
                                    Text='<%# Bind("PASSWORD") %>' Width="130px" />
                                <br />
                                <span class="flbl" style="width:50px;">E-mail</span>
                                <asp:TextBox ID="EMAIL_UTTextBox" runat="server" 
                                    Text='<%# Bind("EMAIL_UT") %>' Width="140px" />
                                <span class="slbl" style="width:60px;">Disabilitato</span>
                                <asp:CheckBox ID="UTEDISABCheckBox" runat="server" 
                                    Checked='<%# Bind("UTEDISAB") %>' />
                            </EditItemTemplate>
                        </stl:StlFormView>
                        <stl:StlSqlDataSource ID="sdsUTENTI_f" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                            InsertCommand="INSERT INTO [ac_UTENTI] ([ID_AZIEN], [COGNUTEN], [EMAIL_UT], [NOMEUTEN], [USERNAME], [UTEDISAB], [PASSWORD]) VALUES (@ID_AZIEN, @COGNUTEN, @EMAIL_UT, @NOMEUTEN, @USERNAME, @UTEDISAB, @PASSWORD); SELECT @ID_UTENT = SCOPE_IDENTITY()" 
                            SelectCommand="SELECT [ID_UTENT], [COGNUTEN], [EMAIL_UT], [NOMEUTEN], [USERNAME], [UTEDISAB], [PASSWORD] FROM [ac_UTENTI] WHERE ([ID_UTENT] = @ID_UTENT)" 
                            UpdateCommand="UPDATE [ac_UTENTI] SET [COGNUTEN] = @COGNUTEN, [EMAIL_UT] = @EMAIL_UT, [NOMEUTEN] = @NOMEUTEN, [USERNAME] = @USERNAME, [UTEDISAB] = @UTEDISAB, [PASSWORD] = @PASSWORD WHERE [ID_UTENT] = @ID_UTENT">
                            <UpdateParameters>
                                <asp:Parameter Name="COGNUTEN" Type="String" />
                                <asp:Parameter Name="EMAIL_UT" Type="String" />
                                <asp:Parameter Name="NOMEUTEN" Type="String" />
                                <asp:Parameter Name="USERNAME" Type="String" />
                                <asp:Parameter Name="UTEDISAB" Type="Boolean" />
                                <asp:Parameter Name="PASSWORD" Type="String" />
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ID_AZIEN" Type="Int32" />
                                <asp:Parameter Name="COGNUTEN" Type="String" />
                                <asp:Parameter Name="EMAIL_UT" Type="String" />
                                <asp:Parameter Name="NOMEUTEN" Type="String" />
                                <asp:Parameter Name="USERNAME" Type="String" />
                                <asp:Parameter Name="UTEDISAB" Type="Boolean" />
                                <asp:Parameter Name="PASSWORD" Type="String" />
                                <asp:Parameter Name="ID_UTENT" Type="Int32" Direction="Output" />
                            </InsertParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
        
                <stl:StlUpdatePanel ID="updAUTUTE_g" runat="server" Height="108px" Width="400px" Left="0px" Top="440px">
                    <ContentTemplate>
                        
                        <stl:StlGridView ID="grdAUTUTE" runat="server" 
                            AutoGenerateColumns="False" BoundStlFormViewID="frmAUTUTE" 
                            DataKeyNames="ID_AUTUT" DataSourceID="sdsAUTUTE_g" EnableViewState="False" 
                            ItemDescriptionPlural="" ItemDescriptionSingular="" 
                            ParentStlGridViewID="grdUTENTI" AddCommandText="" Title="Autorizzazioni Utente">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                                <asp:BoundField DataField="DESFUNZI" HeaderText="Descrizione" 
                                    SortExpression="DESFUNZI" />
                                <asp:BoundField DataField="LIVELLOA" HeaderText="Livello" 
                                    SortExpression="LIVELLOA" />
                            </Columns>
                        </stl:StlGridView>
                        
                        <stl:StlSqlDataSource ID="sdsAUTUTE_g" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" DeleteCommand="DELETE FROM ac_AUTUTE
WHERE ID_AUTUT = @ID_AUTUT" SelectCommand="SELECT ID_AUTUT, ID_UTENT, DESFUNZI, LIVELLOA
FROM ac_AUTUTE
INNER JOIN ac_FUNZIO ON ac_AUTUTE.ID_FUNZI = ac_FUNZIO.ID_FUNZI
WHERE ID_UTENT = @ID_UTENT
ORDER BY DESFUNZI">
                            <DeleteParameters>
                                <asp:Parameter Name="ID_AUTUT" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                            </SelectParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
                <stl:StlUpdatePanel ID="updAUTUTE_f" runat="server" Height="108px" Width="505px" Left="405px" Top="440px">
                    <ContentTemplate>
                        <stl:StlFormView ID="frmAUTUTE" runat="server" BoundStlGridViewID="grdAUTUTE" 
                            DataKeyNames="ID_AUTUT" DataSourceID="sdsAUTUTE_f">
                            <EditItemTemplate>
                                <span class="flbl" style="width:60px;">Funzione</span>
                                <asp:DropdownList ID="ID_FUNZIDropdownList" runat="server" 
                                    SelectedValue='<%# Bind("ID_FUNZI") %>' DataSourceID="sdsAUTUTE_ID_FUNZI" 
                                    DataTextField="DESFUNZI" DataValueField="ID_FUNZI" Width="431" AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropdownList>
                                <asp:SqlDataSource ID="sdsAUTUTE_ID_FUNZI" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                                    SelectCommand="SELECT [ID_FUNZI], [DESFUNZI] FROM [ac_FUNZIO] ORDER BY [DESFUNZI]">
                                </asp:SqlDataSource>
                                <br />
                                <span class="flbl" style="width:60px;">Livello</span>
                                <asp:TextBox ID="LIVELLOATextBox" runat="server" 
                                    Text='<%# Bind("LIVELLOA") %>' Width="427" />
                                <br />
                                <span class="flbl" style="width:60px;vertical-align:top;">Note</span>
                                <asp:TextBox ID="ANNOTAZITextBox" runat="server" 
                                    Text='<%# Bind("ANNOTAZI") %>'  Width="427px" Height="30px" TextMode="MultiLine" />
                            </EditItemTemplate>
                        </stl:StlFormView>
                        <stl:StlSqlDataSource ID="sdsAUTUTE_f" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                            InsertCommand="INSERT INTO [ac_AUTUTE] ([ID_UTENT], [ID_FUNZI], [LIVELLOA], [ANNOTAZI]) VALUES (@ID_UTENT, @ID_FUNZI, @LIVELLOA, @ANNOTAZI); SELECT @ID_AUTUT = SCOPE_IDENTITY()" 
                            SelectCommand="SELECT [ID_AUTUT], [ID_FUNZI], [LIVELLOA], [ANNOTAZI] FROM [ac_AUTUTE] WHERE ([ID_AUTUT] = @ID_AUTUT)" 
                            UpdateCommand="UPDATE [ac_AUTUTE] SET [ID_FUNZI] = @ID_FUNZI, [LIVELLOA] = @LIVELLOA, [ANNOTAZI] = @ANNOTAZI WHERE [ID_AUTUT] = @ID_AUTUT">
                            <UpdateParameters>
                                <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                                <asp:Parameter Name="LIVELLOA" Type="Int32" />
                                <asp:Parameter Name="ANNOTAZI" Type="String" />
                                <asp:Parameter Name="ID_AUTUT" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_AUTUT" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                                <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                                <asp:Parameter Name="LIVELLOA" Type="Int32" />
                                <asp:Parameter Name="ANNOTAZI" Type="String" />
                                <asp:Parameter Name="ID_AUTUT" Type="Int32" Direction="Output" />
                            </InsertParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
      
                <stl:StlUpdatePanel ID="updUTEGRP_g" runat="server" Height="150px" Width="495px" Left="415px" Top="185px">
                    <ContentTemplate>
                        <stl:StlGridView ID="grdUTEGRP" runat="server" 
                            DataSourceID="sdsUTEGRP_g" EnableViewState="False" 
                            ItemDescriptionPlural="" ItemDescriptionSingular="" 
                            BoundStlFormViewID="frmUTEGRP" DataKeyNames="ID_UTEGR" 
                            AutoGenerateColumns="False" ParentStlGridViewID="grdUTENTI" 
                            AddCommandText="" Title="Membro dei gruppi...">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                                <asp:BoundField DataField="DESGRUPP" HeaderText="Descrizione" 
                                    SortExpression="DESGRUPP" />
                            </Columns>
                        </stl:StlGridView>
                        <stl:StlSqlDataSource ID="sdsUTEGRP_g" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" DeleteCommand="DELETE FROM ac_UTEGRP
WHERE ID_UTEGR = @ID_UTEGR" SelectCommand="SELECT ID_UTEGR, ID_UTENT, DESGRUPP
FROM ac_UTEGRP
INNER JOIN ac_GRUPPI ON ac_UTEGRP.ID_GRUPP = ac_GRUPPI.ID_GRUPP
WHERE ID_UTENT = @ID_UTENT
ORDER BY DESGRUPP">
                            <DeleteParameters>
                                <asp:Parameter Name="ID_UTEGR" Type="Int32"/>
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                            </SelectParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
                
                <stl:StlUpdatePanel ID="updUTEGRP_f" runat="server" Height="90px" Width="495px" Left="415px" Top="340px">
                    <ContentTemplate>
                        <stl:StlFormView ID="frmUTEGRP" runat="server" BoundStlGridViewID="grdUTEGRP" 
                            DataKeyNames="ID_UTEGR" DataSourceID="sdsUTEGRP_f">
                            <EditItemTemplate>
                                <span class="flbl" style="width:60px;">Descrizione</span>
                                <asp:DropDownList ID="ID_GRUPPDropDownList" runat="server" 
                                    SelectedValue='<%# Bind("ID_GRUPP") %>' DataSourceID="sdsUTEGRP_ID_GRUPP" 
                                    DataTextField="DESGRUPP" DataValueField="ID_GRUPP" Width="350px" AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                <asp:SqlDataSource ID="sdsUTEGRP_ID_GRUPP" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                                    SelectCommand="SELECT [ID_GRUPP], [DESGRUPP] FROM [ac_GRUPPI] ORDER BY [DESGRUPP]">
                                </asp:SqlDataSource>
                            </EditItemTemplate>
                            </stl:StlFormView>
                        <stl:StlSqlDataSource ID="sdsUTEGRP_f" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                            InsertCommand="INSERT INTO [ac_UTEGRP] ([ID_UTENT], [ID_GRUPP]) VALUES (@ID_UTENT, @ID_GRUPP); SELECT @ID_UTEGR = SCOPE_IDENTITY()" 
                            SelectCommand="SELECT [ID_UTEGR], [ID_GRUPP] FROM [ac_UTEGRP] WHERE ([ID_UTEGR] = @ID_UTEGR)" 
                            UpdateCommand="UPDATE [ac_UTEGRP] SET  [ID_GRUPP] = @ID_GRUPP WHERE [ID_UTEGR] = @ID_UTEGR">
                            <UpdateParameters>
                                <asp:Parameter Name="ID_GRUPP" Type="Int32" />
                                <asp:Parameter Name="ID_UTEGR" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:Parameter Name="ID_UTEGR" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ID_UTENT" Type="Int32" />
                                <asp:Parameter Name="ID_GRUPP" Type="Int32" />
                                <asp:Parameter Name="ID_UTEGR" Type="Int32" Direction="Output"/>
                            </InsertParameters>
                        </stl:StlSqlDataSource>
                    </ContentTemplate>
                </stl:StlUpdatePanel>
           
</asp:Content>
