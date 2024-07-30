<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ApplicazioniFunzioni.aspx.vb" 
    Inherits="Softailor.SiteTailorIzs.ApplicazioniFunzioni" 
    title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updAPPLIC_g" runat="server" Height="350px" Width="300px" Left="0px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdAPPLIC" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataKeyNames="ID_APPLI" DataSourceID="sdsAPPLIC_g" 
                EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
                Title="Applicazioni" BoundStlFormViewID="frmAPPLIC">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="ID_APPLI" HeaderText="Id" 
                        InsertVisible="False" ReadOnly="True" SortExpression="ID_APPLI" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="SGLAPPLI" HeaderText="Sigla" 
                        SortExpression="SGLAPPLI" />
                    <asp:BoundField DataField="DESAPPLI" HeaderText="Descrizione" 
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
    <stl:StlUpdatePanel ID="updAPPLIC_f" runat="server" Height="92px" Width="300px" Left="0px" Top="360px">
        <ContentTemplate>
            <stl:StlFormView ID="frmAPPLIC" runat="server" DataKeyNames="ID_APPLI" 
                DataSourceID="sdsAPPLIC_f" BoundStlGridViewID="grdAPPLIC">
                <EditItemTemplate>
                    <span class="flbl" style="width:30px;">Id</span>
                    <asp:TextBox ReadOnly="true" ID="ID_APPLITextBox" runat="server" Text='<%# Eval("ID_APPLI") %>' Width="56" />
                    <span class="slbl" style="width:40px;">Sigla</span>
                    <asp:TextBox ID="SGLAPPLITextBox" runat="server" 
                        Text='<%# Bind("SGLAPPLI") %>' Width="160" />
                        <br />
                    <span class="flbl" style="width:60px;">Descrizione</span>
                    <asp:TextBox ID="DESAPPLITextBox" runat="server" 
                        Text='<%# Bind("DESAPPLI") %>' Width="226" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsAPPLIC_f" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                InsertCommand="INSERT INTO [ac_APPLIC] ([SGLAPPLI], [DESAPPLI]) VALUES (@SGLAPPLI, @DESAPPLI); SELECT @ID_APPLI = SCOPE_IDENTITY()" 
                SelectCommand="SELECT [ID_APPLI], [SGLAPPLI], [DESAPPLI] FROM [ac_APPLIC] WHERE ([ID_APPLI] = @ID_APPLI)" 
                UpdateCommand="UPDATE [ac_APPLIC] SET [SGLAPPLI] = @SGLAPPLI, [DESAPPLI] = @DESAPPLI WHERE [ID_APPLI] = @ID_APPLI">
                <UpdateParameters>
                    <asp:Parameter Name="SGLAPPLI" Type="String" />
                    <asp:Parameter Name="DESAPPLI" Type="String" />
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="SGLAPPLI" Type="String" />
                    <asp:Parameter Name="DESAPPLI" Type="String" />
                    <asp:Parameter Name="ID_APPLI" Type="Int32" Direction="Output"/>
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updFUNZIO_g" runat="server" Height="350px" Width="800px" Left="320px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdFUNZIO" runat="server" AddCommandText="" 
                AutoGenerateColumns="False" DataKeyNames="ID_FUNZI" DataSourceID="sdsFUNZIO_g" 
                EnableViewState="False" ItemDescriptionPlural="" ItemDescriptionSingular="" 
                Title="Funzioni" ParentStlGridViewID="grdAPPLIC" BoundStlFormViewID="frmFUNZIO" >
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ID_FUNZI" HeaderText="Id" 
                        InsertVisible="False" ReadOnly="True" SortExpression="ID_FUNZI" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="DESFUNZI" HeaderText="Descrizione Funzione" 
                        SortExpression="DESFUNZI" />
                    <asp:BoundField DataField="PATFUNZI" HeaderText="Percorso Relativo" 
                        SortExpression="PATFUNZI" />
                    <asp:BoundField DataField="PAGFUNZI" HeaderText="Nome Pagina aspx" 
                        SortExpression="PAGFUNZI" /> 
                    <asp:BoundField DataField="CODFUNZI" HeaderText="Codice" 
                        SortExpression="CODFUNZI" />   
                    <asp:TemplateField HeaderText="Custom Context" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("CUSTCTXT"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>                 
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFUNZIO_g" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                DeleteCommand="DELETE FROM [ac_FUNZIO] WHERE [ID_FUNZI] = @ID_FUNZI" 
                SelectCommand="SELECT [ID_FUNZI], [ID_APPLI], [DESFUNZI], [PAGFUNZI], [PATFUNZI], [CODFUNZI], [CUSTCTXT] FROM [ac_FUNZIO] WHERE ([ID_APPLI] = @ID_APPLI) ORDER BY [DESFUNZI]" >
                <DeleteParameters>
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updFUNZIO_f" runat="server" Height="112px" Width="800px" Left="320px" Top="360px">
        <ContentTemplate>
            <stl:StlFormView ID="frmFUNZIO" runat="server" DataKeyNames="ID_FUNZI" 
                DataSourceID="sdsFUNZIO_f" BoundStlGridViewID="grdFUNZIO">
                <EditItemTemplate>
                    <span class="flbl" style="width:30px;">Id</span>
                    <asp:TextBox ID="ID_FUNZITextBox" runat="server" Text='<%# Eval("ID_FUNZI") %>' Width="56px" ReadOnly="true" />
                    <span class="slbl" style="width:80px;">Descrizione</span>
                    <asp:TextBox ID="DESFUNZITextBox" runat="server" 
                        Text='<%# Bind("DESFUNZI") %>' Width="420px"  />
                    <br />
                    <span class="flbl" style="width:210px;">Percorso (null o, ad esempio, admin/ )</span>
                    <asp:TextBox ID="PATFUNZITextBox" runat="server" 
                        Text='<%# Bind("PATFUNZI") %>' Width="376px" />
                    <br />
                    <span class="flbl" style="width:210px;">Nome Pagina ASPX</span>
                    <asp:TextBox ID="PAGFUNZITextBox" runat="server" 
                        Text='<%# Bind("PAGFUNZI") %>' Width="376px" />
                    <br />
                    <span class="flbl" style="width:210px;">Codice funzione (se necessario)</span>
                    <asp:TextBox ID="CODFUNZITextBox" runat="server" 
                        Text='<%# Bind("CODFUNZI") %>' Width="180px" />
                    <span class="slbl" style="width:100px;">Custom Context</span>
                    <asp:CheckBox ID="CUSTCTXTCheckBox" runat="server" Checked='<%# Bind("CUSTCTXT") %>' />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsFUNZIO_f" runat="server" 
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>" 
                InsertCommand="INSERT INTO [ac_FUNZIO] ([ID_APPLI], [DESFUNZI], [PATFUNZI], [PAGFUNZI], [CODFUNZI], [CUSTCTXT]) VALUES (@ID_APPLI, @DESFUNZI, @PATFUNZI, @PAGFUNZI, @CODFUNZI, @CUSTCTXT);SELECT @ID_FUNZI = SCOPE_IDENTITY()" 
                SelectCommand="SELECT [ID_FUNZI], [DESFUNZI], [PATFUNZI], [PAGFUNZI], [CODFUNZI], [CUSTCTXT] FROM [ac_FUNZIO] WHERE ([ID_FUNZI] = @ID_FUNZI)" 
                UpdateCommand="UPDATE [ac_FUNZIO] SET  [DESFUNZI] = @DESFUNZI, [PATFUNZI] = @PATFUNZI, [PAGFUNZI] = @PAGFUNZI, [CODFUNZI] = @CODFUNZI, [CUSTCTXT] = @CUSTCTXT WHERE [ID_FUNZI] = @ID_FUNZI">
                <UpdateParameters>
                    <asp:Parameter Name="DESFUNZI" Type="String" />
                    <asp:Parameter Name="PATFUNZI" Type="String" />
                    <asp:Parameter Name="PAGFUNZI" Type="String" />
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                    <asp:Parameter Name="CODFUNZI" Type="String" />
                    <asp:Parameter Name="CUSTCTXT" Type="Boolean" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ID_APPLI" Type="Int32" />
                    <asp:Parameter Name="DESFUNZI" Type="String" />
                    <asp:Parameter Name="PATFUNZI" Type="String" />
                    <asp:Parameter Name="PAGFUNZI" Type="String" />
                    <asp:Parameter Name="CODFUNZI" Type="String" />
                    <asp:Parameter Name="CUSTCTXT" Type="Boolean" />
                    <asp:Parameter Name="ID_FUNZI" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>    
</asp:Content>
