<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="TipologieEventi.aspx.vb" Inherits="Softailor.SiteTailorIzs.TipologieEventi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updTIPOLOGIEEVENTI_g" runat="server" Width="900px" Height="409px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdTIPOLOGIEEVENTI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_TIPOLOGIAEVENTO" DataSourceID="sdsTIPOLOGIEEVENTI_g"
                EnableViewState="False" ItemDescriptionPlural="tipologie" ItemDescriptionSingular="tipologia"
                Title="Tipologie Eventi" BoundStlFormViewID="frmTIPOLOGIEEVENTI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_TIPOLOGIAEVENTO" HeaderText="Tipologia" ItemStyle-Font-Bold="true" />





                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsTIPOLOGIEEVENTI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_TIPOLOGIEEVENTI WHERE id_TIPOLOGIAEVENTO = @id_TIPOLOGIAEVENTO"
                SelectCommand="SELECT * FROM vw_age_TIPOLOGIEEVENTI_grid ORDER BY tx_TIPOLOGIAEVENTO">
                <DeleteParameters>
                    <asp:Parameter Name="id_TIPOLOGIAEVENTO" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updTIPOLOGIEEVENTI_f" runat="server" Width="900px" Height="70px" Top="414px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmTIPOLOGIEEVENTI" runat="server" DataKeyNames="id_TIPOLOGIAEVENTO"
                DataSourceID="sdsTIPOLOGIEEVENTI_f" NewItemText="" BoundStlGridViewID="grdTIPOLOGIEEVENTI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 80px;">Descrizione</span>
                    <asp:TextBox ID="tx_TIPOLOGIAEVENTOTextBox" runat="server"
                        Text='<%# Bind("tx_TIPOLOGIAEVENTO")%>' Width="310px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsTIPOLOGIEEVENTI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_TIPOLOGIEEVENTI (
    tx_TIPOLOGIAEVENTO,

    dt_CREAZIONE, 
    tx_CREAZIONE
) VALUES (
    @tx_TIPOLOGIAEVENTO,


    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)                
);
SET @id_TIPOLOGIAEVENTO = SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM age_TIPOLOGIEEVENTI WHERE id_TIPOLOGIAEVENTO=@id_TIPOLOGIAEVENTO"
                UpdateCommand="
UPDATE  age_TIPOLOGIEEVENTI
SET     tx_TIPOLOGIAEVENTO = @tx_TIPOLOGIAEVENTO,


        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   id_TIPOLOGIAEVENTO=@id_TIPOLOGIAEVENTO
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_TIPOLOGIAEVENTO" Type="Int32" />
                    <asp:Parameter Name="tx_TIPOLOGIAEVENTO" Type="String" />

                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_TIPOLOGIAEVENTO" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_TIPOLOGIAEVENTO" Type="String" />

                    <asp:Parameter Name="id_TIPOLOGIAEVENTO" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
