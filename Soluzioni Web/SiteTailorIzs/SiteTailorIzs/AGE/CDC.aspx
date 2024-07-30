<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CDC.aspx.vb" Inherits="Softailor.SiteTailorIzs.CDC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updCDC_g" runat="server" Width="900px" Height="495px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCDC" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_CDC" DataSourceID="sdsCDC_g"
                EnableViewState="False" ItemDescriptionPlural="centri di costo" ItemDescriptionSingular="centro di costo"
                Title="Centri di Costo" BoundStlFormViewID="frmCDC"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ac_CDC" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_CDC" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_CDC_erp" HeaderText="Codice ERP" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Valido dal..." DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="...al" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsCDC_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_CDC WHERE ac_CDC = @ac_CDC"
                SelectCommand="SELECT * FROM vw_age_CDC_grid ORDER BY tx_CDC">
                <DeleteParameters>
                    <asp:Parameter Name="ac_CDC" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCDC_f" runat="server" Width="900px" Height="70px" Top="500px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmCDC" runat="server" DataKeyNames="ac_CDC"
                DataSourceID="sdsCDC_f" NewItemText="" BoundStlGridViewID="grdCDC">
                <EditItemTemplate>
                    <span class="flbl" style="width: 70px;">Codice</span>
                    <asp:TextBox ID="ac_CDCTextBox" runat="server"
                        Text='<%# Bind("ac_CDC")%>' Width="120px" />
                    <span class="slbl" style="width: 70px;">Descrizione</span>
                    <asp:TextBox ID="tx_CDCTextBox" runat="server"
                        Text='<%# Bind("tx_CDC")%>' Width="615px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 70px;">Codice ERP</span>
                    <asp:TextBox ID="ac_CDC_erpTextBox" runat="server"
                        Text='<%# Bind("ac_CDC_erp")%>' Width="120px" />
                    <span class="slbl" style="width: 70px;">Valido dal...</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server"
                        Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width: 30px;">..al</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server"
                        Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsCDC_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_CDC (ac_CDC, tx_CDC, ac_CDC_erp, dt_INIZIO, dt_FINE, dt_CREAZIONE, tx_CREAZIONE) VALUES (
    @ac_CDC,
    @tx_CDC,
    @ac_CDC_erp,
    @dt_INIZIO,
    @dt_FINE,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)                
)                
                "
                SelectCommand="SELECT * FROM age_CDC WHERE ac_CDC=@ac_CDC"
                UpdateCommand="
UPDATE  age_CDC
SET     tx_CDC = @tx_CDC,
        ac_CDC_erp = @ac_CDC_erp,
        dt_INIZIO = @dt_INIZIO,
        dt_FINE = @dt_FINE,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   ac_CDC=@ac_CDC
                ">
                <UpdateParameters>
                    <asp:Parameter Name="ac_CDC" Type="String" />
                    <asp:Parameter Name="tx_CDC" Type="String" />
                    <asp:Parameter Name="ac_CDC_erp" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_CDC" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_CDC" Type="String" />
                    <asp:Parameter Name="tx_CDC" Type="String" />
                    <asp:Parameter Name="ac_CDC_erp" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
