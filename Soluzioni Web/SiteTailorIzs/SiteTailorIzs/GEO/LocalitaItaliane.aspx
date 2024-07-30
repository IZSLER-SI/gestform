<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="LocalitaItaliane.aspx.vb" Inherits="Softailor.SiteTailorIzs.LocalitaItaliane" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updPROVINCE_g" runat="server" Width="350px" Height="570px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPROVINCE" runat="server" AddCommandText="" AllowDelete="false" 
                AutoGenerateColumns="False" DataKeyNames="ac_PROVINCIA" DataSourceID="sdsPROVINCE_g"
                EnableViewState="False" ItemDescriptionPlural="province" ItemDescriptionSingular="provincia"
                Title="Regioni e Province"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:BoundField DataField="tx_PROVINCIA" HeaderText="Provincia" />
                    <asp:BoundField DataField="ac_PROVINCIA" HeaderText="Sigla" />
                    <asp:BoundField DataField="tx_REGIONE" HeaderText="Regione" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsPROVINCE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT * FROM vw_geo_REGIONIITALIA_PROVINCEITALIA ORDER BY tx_PROVINCIA">
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCOMUNI_g" runat="server" Width="400px" Height="495px" Top="0px" Left="360px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCOMUNI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_COMUNE" DataSourceID="sdsCOMUNI_g"
                EnableViewState="False" ItemDescriptionPlural="comuni" ItemDescriptionSingular="comune"
                Title="Comuni della provincia selezionata" ParentStlGridViewID="grdPROVINCE" BoundStlFormViewID="frmCOMUNI">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_COMUNE" HeaderText="Comune" />
                    <asp:BoundField DataField="ac_COMUNE" HeaderText="Cod.Belfiore" />
                    <asp:BoundField DataField="ni_CODICEISTAT" HeaderText="Cod.ISTAT" />
                    <asp:TemplateField HeaderText="Obsoleto" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_ATTUALE"), "", "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsCOMUNI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT * FROM geo_COMUNIITALIA WHERE ac_PROVINCIA=@ac_PROVINCIA ORDER BY tx_COMUNE"
                DeleteCommand="DELETE FROM geo_COMUNIITALIA WHERE ac_COMUNE=@ac_COMUNE">
                <SelectParameters>
                    <asp:Parameter Name="ac_PROVINCIA" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="ac_COMUNE" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCOMUNI_f" runat="server" Width="400px" Height="70px" Top="500px" Left="360px">
        <ContentTemplate>
            <stl:StlFormView ID="frmCOMUNI" runat="server" DataKeyNames="ac_COMUNE"
                DataSourceID="sdsCOMUNI_f" NewItemText="" BoundStlGridViewID="grdCOMUNI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 80px;">Nome</span>
                    <asp:TextBox ID="tx_COMUNETextBox" runat="server"
                        Text='<%# Bind("tx_COMUNE")%>' Width="300px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 80px;">Codice Belfiore</span>
                    <asp:TextBox ID="ac_COMUNETextBox" runat="server"
                        Text='<%# Bind("ac_COMUNE")%>' Width="50px" />
                    <span class="slbl" style="width: 80px;">Codice ISTAT</span>
                    <asp:TextBox ID="ni_CODICEISTATTextBox" runat="server"
                        Text='<%# Bind("ni_CODICEISTAT")%>' Width="60px" />
                    <span class="slbl" style="width: 60px;">Obsoleto</span>
                    <asp:CheckBox ID="fl_OBSOLETOCheckBox" runat="server"
                        Checked='<%# Bind("fl_OBSOLETO")%>' />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsCOMUNI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO geo_COMUNIITALIA (ac_COMUNE, ac_PROVINCIA, tx_COMUNE, ni_CODICEISTAT, fl_ATTUALE)
VALUES (@ac_COMUNE, @ac_PROVINCIA, @tx_COMUNE, @ni_CODICEISTAT, 1-@fl_OBSOLETO)
                "
                SelectCommand="
SELECT      ac_COMUNE,
	        tx_COMUNE,
	        ac_PROVINCIA,
	        ni_CODICEISTAT,
	        1-fl_ATTUALE as fl_OBSOLETO
FROM	    geo_COMUNIITALIA
WHERE	    ac_COMUNE=@ac_COMUNE
"
                UpdateCommand="
UPDATE  geo_COMUNIITALIA
SET     tx_COMUNE=@tx_COMUNE,
        ni_CODICEISTAT=@ni_CODICEISTAT,
        fl_ATTUALE=1-@fl_OBSOLETO
WHERE   ac_COMUNE=@ac_COMUNE
                ">
                <UpdateParameters>
                    <asp:Parameter Name="tx_COMUNE" Type="String" />
                    <asp:Parameter Name="ni_CODICEISTAT" Type="Int32" />
                    <asp:Parameter Name="fl_OBSOLETO" Type="Boolean" />
                    <asp:Parameter Name="ac_COMUNE" Type="String" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_COMUNE" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_COMUNE" Type="String" />
                    <asp:Parameter Name="ac_PROVINCIA" Type="String" />
                    <asp:Parameter Name="tx_COMUNE" Type="String" />
                    <asp:Parameter Name="ni_CODICEISTAT" Type="Int32" />
                    <asp:Parameter Name="fl_OBSOLETO" Type="Boolean" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCAP_g" runat="server" Width="400px" Height="495px" Top="0px" Left="770px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCAP" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_CAP" DataSourceID="sdsCAP_g"
                EnableViewState="False" ItemDescriptionPlural="comuni" ItemDescriptionSingular="comune"
                Title="CAP del comune selezionato" ParentStlGridViewID="grdCOMUNI" BoundStlFormViewID="frmCAP">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ac_CAP" HeaderText="CAP" />
                    <asp:TemplateField HeaderText="Obsoleto" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_ATTUALE"), "", "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsCAP_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT * FROM geo_CAPITALIA WHERE ac_COMUNE=@ac_COMUNE ORDER BY ac_CAP"
                DeleteCommand="DELETE FROM geo_CAPITALIA WHERE id_CAP=@id_CAP">
                <SelectParameters>
                    <asp:Parameter Name="ac_COMUNE" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_CAP" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCAP_f" runat="server" Width="400px" Height="70px" Top="500px" Left="770px">
        <ContentTemplate>
            <stl:StlFormView ID="frmCAP" runat="server" DataKeyNames="id_CAP"
                DataSourceID="sdsCAP_f" NewItemText="" BoundStlGridViewID="grdCAP">
                <EditItemTemplate>
                    <span class="flbl" style="width: 40px;">CAP</span>
                    <asp:TextBox ID="ac_CAPTextBox" runat="server"
                        Text='<%# Bind("ac_CAP")%>' Width="50px" Font-Bold="true" />
                    <span class="slbl" style="width: 60px;">Obsoleto</span>
                    <asp:CheckBox ID="fl_OBSOLETOCheckBox" runat="server"
                        Checked='<%# Bind("fl_OBSOLETO")%>' />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsCAP_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO geo_CAPITALIA (ac_CAP, ac_COMUNE, fl_ATTUALE)
VALUES (@ac_CAP, @ac_COMUNE, 1-@fl_OBSOLETO);
SELECT @id_CAP=SCOPE_IDENTITY()
                "
                SelectCommand="
SELECT      id_CAP,
	        ac_CAP,
	        1-fl_ATTUALE as fl_OBSOLETO
FROM	    geo_CAPITALIA
WHERE	    id_CAP=@id_CAP
"
                UpdateCommand="
UPDATE  geo_CAPITALIA
SET     ac_CAP=@ac_CAP,
        fl_ATTUALE=1-@fl_OBSOLETO
WHERE   id_CAP=@id_CAP
                ">
                <UpdateParameters>
                    <asp:Parameter Name="ac_CAP" Type="String" />
                    <asp:Parameter Name="fl_OBSOLETO" Type="Boolean" />
                    <asp:Parameter Name="id_CAP" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_CAP" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_CAP" Type="String" />
                    <asp:Parameter Name="ac_COMUNE" Type="String" />
                    <asp:Parameter Name="fl_OBSOLETO" Type="Boolean" />
                    <asp:Parameter Name="id_CAP" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
