<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="QuoteIscrizione.aspx.vb" Inherits="Softailor.SiteTailorIzs.QuoteIscrizione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updQUOTE_g" runat="server" Width="800px" Height="409px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdQUOTE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_QUOTAISCRIZIONE" DataSourceID="sdsQUOTE_g"
                EnableViewState="False" ItemDescriptionPlural="quote" ItemDescriptionSingular="quota"
                Title="Quote di iscrizione" BoundStlFormViewID="frmQUOTE"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ac_QUOTAISCRIZIONE" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_QUOTAISCRIZIONE" HeaderText="Descrizione Quota" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="mo_IMPONIBILE" HeaderText="Imponibile €" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="mo_ALIQUOTA" HeaderText="IVA %" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="mo_PREZZOUN" HeaderText="Costo IVA inclusa €" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Valida dal.." DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="...al" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsQUOTE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM eve_QUOTEISCRIZIONE WHERE id_QUOTAISCRIZIONE = @id_QUOTAISCRIZIONE"
                SelectCommandType="StoredProcedure"
                SelectCommand="sp_eve_QUOTEISCRIZIONE_grid">
                <DeleteParameters>
                    <asp:Parameter Name="id_QUOTAISCRIZIONE" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updQUOTE_f" runat="server" Width="800px" Height="69px" Top="414px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmQUOTE" runat="server" DataKeyNames="id_QUOTAISCRIZIONE"
                DataSourceID="sdsQUOTE_f" NewItemText="" BoundStlGridViewID="grdQUOTE">
                <EditItemTemplate>
                    <span class="flbl" style="width: 70px;">Codice</span>
                    <asp:TextBox ID="ac_QUOTAISCRIZIONETextBox" runat="server"
                        Text='<%# Bind("ac_QUOTAISCRIZIONE")%>' Width="80px" />
                    <span class="slbl" style="width: 105px;">Descrizione Quota</span>
                    <asp:TextBox ID="tx_QUOTAISCRIZIONETextBox" runat="server" Font-Bold="true"
                        Text='<%# Bind("tx_QUOTAISCRIZIONE")%>' Width="524px" />
                    <br />
                    <span class="flbl" style="width: 70px;">Imponibile €</span>
                    <asp:TextBox ID="mo_IMPONIBILETextBox" runat="server"
                        Text='<%# Bind("mo_IMPONIBILE", "{0:#0.00}")%>' Width="80px" />
                    <span class="slbl" style="width: 30px;">IVA</span>
                    <asp:DropDownList ID="id_IVADropDownList" runat="server" SelectedValue='<%# Bind("id_IVA") %>'
                        DataSourceID="sdsid_IVA" DataTextField="tx_IVA" DataValueField="id_IVA" Width="350px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 65px;">Valida dal </span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server" CssClass="stl_dt_data_ddmmyyyy"
                        Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>' Width="80px" />
                    <span class="slbl" style="width: 20px;">al</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server" CssClass="stl_dt_data_ddmmyyyy"
                        Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>' Width="80px" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsQUOTE_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="

INSERT INTO eve_QUOTEISCRIZIONE (
    dt_CREAZIONE, 
    tx_CREAZIONE, 
    id_EVENTO, 
    ac_QUOTAISCRIZIONE, 
    tx_QUOTAISCRIZIONE, 
    mo_IMPONIBILE, 
    id_IVA, 
    dt_INIZIO, 
    dt_FINE
) VALUES (
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
    @id_EVENTO, 
    @ac_QUOTAISCRIZIONE, 
    @tx_QUOTAISCRIZIONE, 
    @mo_IMPONIBILE, 
    @id_IVA, 
    @dt_INIZIO, 
    @dt_FINE
);
SET @id_QUOTAISCRIZIONE = SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM eve_QUOTEISCRIZIONE WHERE id_QUOTAISCRIZIONE=@id_QUOTAISCRIZIONE"
                UpdateCommand="
UPDATE  eve_QUOTEISCRIZIONE
SET     dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
        ac_QUOTAISCRIZIONE = @ac_QUOTAISCRIZIONE,
        tx_QUOTAISCRIZIONE = @tx_QUOTAISCRIZIONE,
        mo_IMPONIBILE = @mo_IMPONIBILE,
        id_IVA = @id_IVA,
        dt_INIZIO = @dt_INIZIO,
        dt_FINE = @dt_FINE
WHERE   id_QUOTAISCRIZIONE = @id_QUOTAISCRIZIONE
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_QUOTAISCRIZIONE" Type="String" />
                    <asp:Parameter Name="tx_QUOTAISCRIZIONE" Type="String" />
                    <asp:Parameter Name="mo_IMPONIBILE" Type="Decimal" />
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_QUOTAISCRIZIONE" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_QUOTAISCRIZIONE" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                    <asp:Parameter Name="ac_QUOTAISCRIZIONE" Type="String" />
                    <asp:Parameter Name="tx_QUOTAISCRIZIONE" Type="String" />
                    <asp:Parameter Name="mo_IMPONIBILE" Type="Decimal" />
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_QUOTAISCRIZIONE" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:SqlDataSource ID="sdsid_IVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT id_IVA, tx_IVA FROM age_IVA ORDER BY tx_IVA">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
