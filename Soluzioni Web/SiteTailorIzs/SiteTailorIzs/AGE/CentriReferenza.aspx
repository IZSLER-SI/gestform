<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CentriReferenza.aspx.vb" Inherits="Softailor.SiteTailorIzs.CentriReferenza" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updCENTRIREFERENZA_g" runat="server" Width="600px" Height="409px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCENTRIREFERENZA" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_CENTROREFERENZA" DataSourceID="sdsCENTRIREFERENZA_g"
                EnableViewState="False" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                Title="Centri di referenza" BoundStlFormViewID="frmCENTRIREFERENZA"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_CENTROREFERENZA" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsCENTRIREFERENZA_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_CENTRIREFERENZA WHERE id_CENTROREFERENZA = @id_CENTROREFERENZA"
                SelectCommand="SELECT * FROM vw_age_CENTRIREFERENZA_grid ORDER BY tx_CENTROREFERENZA">
                <DeleteParameters>
                    <asp:Parameter Name="id_CENTROREFERENZA" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCENTRIREFERENZA_f" runat="server" Width="600px" Height="48px" Top="414px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmCENTRIREFERENZA" runat="server" DataKeyNames="id_CENTROREFERENZA"
                DataSourceID="sdsCENTRIREFERENZA_f" NewItemText="" BoundStlGridViewID="grdCENTRIREFERENZA">
                <EditItemTemplate>
                    <span class="flbl" style="width: 80px;">Descrizione</span>
                    <asp:TextBox ID="tx_CENTROREFERENZATextBox" runat="server"
                        Text='<%# Bind("tx_CENTROREFERENZA")%>' Width="500px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsCENTRIREFERENZA_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_CENTRIREFERENZA (tx_CENTROREFERENZA, dt_CREAZIONE, tx_CREAZIONE) VALUES (
    @tx_CENTROREFERENZA,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)                
);
SET @id_CENTROREFERENZA = SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM age_CENTRIREFERENZA WHERE id_CENTROREFERENZA=@id_CENTROREFERENZA"
                UpdateCommand="
UPDATE  age_CENTRIREFERENZA
SET     tx_CENTROREFERENZA = @tx_CENTROREFERENZA,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   id_CENTROREFERENZA=@id_CENTROREFERENZA
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_CENTROREFERENZA" Type="Int32" />
                    <asp:Parameter Name="tx_CENTROREFERENZA" Type="String" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_CENTROREFERENZA" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_CENTROREFERENZA" Type="String" />
                    <asp:Parameter Name="id_CENTROREFERENZA" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
