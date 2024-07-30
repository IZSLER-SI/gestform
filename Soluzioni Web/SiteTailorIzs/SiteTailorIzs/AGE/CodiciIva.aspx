<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CodiciIva.aspx.vb" Inherits="Softailor.SiteTailorIzs.CodiciIva" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updIVA_g" runat="server" Width="800px" Height="409px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIVA" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_IVA" DataSourceID="sdsIVA_g"
                EnableViewState="False" ItemDescriptionPlural="aliquote" ItemDescriptionSingular="aliquota"
                Title="Aliquote IVA" BoundStlFormViewID="frmIVA"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ac_IVA" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_IVA" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="mo_ALIQUOTA" HeaderText="Aliquota %" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="Esente" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_ESENTE"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Default" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_DEFAULT"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ac_IVA_erp" HeaderText="Codice ERP" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsIVA_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_IVA WHERE id_IVA = @id_IVA"
                SelectCommand="SELECT * FROM vw_age_IVA_grid ORDER BY ac_IVA">
                <DeleteParameters>
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updIVA_f" runat="server" Width="800px" Height="69px" Top="414px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmIVA" runat="server" DataKeyNames="id_IVA"
                DataSourceID="sdsIVA_f" NewItemText="" BoundStlGridViewID="grdIVA">
                <EditItemTemplate>
                    <span class="flbl" style="width: 60px;">Codice</span>
                    <asp:TextBox ID="ac_IVATextBox" runat="server"
                        Text='<%# Bind("ac_IVA")%>' Width="80px" />
                    <span class="slbl" style="width: 70px;">Descrizione</span>
                    <asp:TextBox ID="tx_IVATextBox" runat="server"
                        Text='<%# Bind("tx_IVA")%>' Width="370px" />
                    <span class="slbl" style="width: 65px;">Aliquota %</span>
                    <asp:TextBox ID="mo_ALIQUOTATextBox" runat="server"
                        Text='<%# Bind("mo_ALIQUOTA","{0:#0.00}")%>' Width="50px" />
                    <span class="slbl" style="width: 50px;">Esente</span>
                    <asp:CheckBox ID="fl_ESENTECheckBox" runat="server"
                        Checked='<%# Bind("fl_ESENTE")%>' />
                    <br />
                    <span class="flbl" style="width: 60px;">Codice ERP</span>
                    <asp:TextBox ID="ac_IVA_erpTextBox" runat="server"
                        Text='<%# Bind("ac_IVA_erp")%>' Width="80px" />
                    <span class="slbl" style="width: 50px;">Default</span>
                    <asp:CheckBox ID="fl_DEFAULTCheckBox" runat="server"
                        Checked='<%# Bind("fl_DEFAULT")%>' />

                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsIVA_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_IVA (ac_IVA, tx_IVA, mo_ALIQUOTA, fl_ESENTE, ac_IVA_erp, fl_DEFAULT, dt_CREAZIONE, tx_CREAZIONE) VALUES (
    @ac_IVA, @tx_IVA, @mo_ALIQUOTA, @fl_ESENTE, @ac_IVA_erp, @fl_DEFAULT,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)                
);
SET @id_IVA = SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM age_IVA WHERE id_IVA=@id_IVA"
                UpdateCommand="
UPDATE  age_IVA
SET     ac_IVA = @ac_IVA,
        tx_IVA = @tx_IVA,
        mo_ALIQUOTA = @mo_ALIQUOTA,
        fl_ESENTE = @fl_ESENTE,
        ac_IVA_erp = @ac_IVA_erp,
        fl_DEFAULT = @fl_DEFAULT,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   id_IVA = @id_IVA
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_IVA" Type="String" />
                    <asp:Parameter Name="tx_IVA" Type="String" />
                    <asp:Parameter Name="mo_ALIQUOTA" Type="Decimal" />
                    <asp:Parameter Name="fl_ESENTE" Type="Boolean" />
                    <asp:Parameter Name="ac_IVA_erp" Type="String" />
                    <asp:Parameter Name="fl_DEFAULT" Type="Boolean" />
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_IVA" Type="String" />
                    <asp:Parameter Name="tx_IVA" Type="String" />
                    <asp:Parameter Name="mo_ALIQUOTA" Type="Decimal" />
                    <asp:Parameter Name="fl_ESENTE" Type="Boolean" />
                    <asp:Parameter Name="ac_IVA_erp" Type="String" />
                    <asp:Parameter Name="fl_DEFAULT" Type="Boolean" />
                    <asp:Parameter Name="id_IVA" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
