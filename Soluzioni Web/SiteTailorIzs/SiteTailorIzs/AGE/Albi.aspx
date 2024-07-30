<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="Albi.aspx.vb" Inherits="Softailor.SiteTailorIzs.Albi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updALBI_g" runat="server" Width="840px" Height="450px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdALBI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_ALBO" DataSourceID="sdsALBI_g"
                EnableViewState="False" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                Title="Ordini/Associazioni Professionali" BoundStlFormViewID="frmALBI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_ALBO_SHORT" HeaderText="Nome breve" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_ALBO_LONG" HeaderText="Nome completo" />
                    <asp:BoundField DataField="tx_ALL" HeaderText="Al/All'/Allo/Alla" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsALBI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_ALBI WHERE id_ALBO=@id_ALBO"
                SelectCommand="SELECT * FROM age_ALBI ORDER BY tx_ALBO_SHORT">
                <DeleteParameters>
                    <asp:Parameter Name="id_ALBO" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updALBI_f" runat="server" Width="840px" Height="91px" Top="455px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmALBI" runat="server" DataKeyNames="id_ALBO"
                DataSourceID="sdsALBI_f" NewItemText="" BoundStlGridViewID="grdALBI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 105px;">Nome breve</span>
                    <asp:TextBox ID="tx_ALBO_SHORTTextBox" runat="server"
                        Text='<%# Bind("tx_ALBO_SHORT")%>' Width="300px" />
                    <br />
                    <span class="flbl" style="width: 105px;">Nome completo</span>
                    <asp:TextBox ID="tx_ALBO_LONGTextBox" runat="server"
                        Text='<%# Bind("tx_ALBO_LONG")%>' Width="718px" />
                    <br />
                    <span class="flbl" style="width: 105px;">Al/All'/Allo/Alla</span>
                    <asp:DropDownList ID="tx_ALLDropDownList" runat="server" EnableViewState="false" SelectedValue='<%# Bind("tx_ALL")%>'
                        Width="100px">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="allo " Value="allo \" />
                        <asp:ListItem Text="alla " Value="alla \" />
                        <asp:ListItem Text="all'" Value="all'\" />
                        <asp:ListItem Text="al " Value="al \" />
                    </asp:DropDownList>
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsALBI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_ALBI (
    dt_CREAZIONE,
    tx_CREAZIONE,
    tx_ALBO_SHORT,
    tx_ALBO_LONG,
    tx_ALL
) VALUES (
    GETDATE(),
    @tx_CREAZIONE,
    @tx_ALBO_SHORT,
    @tx_ALBO_LONG,
    REPLACE(@tx_ALL,'\','')
);
SELECT @id_ALBO=SCOPE_IDENTITY()
                "
                SelectCommand="
                    SELECT 
                        id_ALBO,
                        tx_ALBO_SHORT,
                        tx_ALBO_LONG,
                        tx_ALL + '\' as tx_ALL
                    FROM
                        age_ALBI
                    WHERE
                        id_ALBO=@id_ALBO
                    "
                UpdateCommand="
UPDATE  age_ALBI
SET     dt_MODIFICA = GETDATE(),
        tx_MODIFICA = @tx_MODIFICA,
        tx_ALBO_SHORT = @tx_ALBO_SHORT,
        tx_ALBO_LONG = @tx_ALBO_LONG,
        tx_ALL = REPLACE(@tx_ALL,'\','')
WHERE   id_ALBO=@id_ALBO
                ">
                <UpdateParameters>
                    <asp:Parameter Name="tx_MODIFICA" Type="String" />
                    <asp:Parameter Name="tx_ALBO_SHORT" Type="String" />
                    <asp:Parameter Name="tx_ALBO_LONG" Type="String" />
                    <asp:Parameter Name="tx_ALL" Type="String" />
                    <asp:Parameter Name="id_ALBO" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_ALBO" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="tx_CREAZIONE" Type="String" />
                    <asp:Parameter Name="tx_ALBO_SHORT" Type="String" />
                    <asp:Parameter Name="tx_ALBO_LONG" Type="String" />
                    <asp:Parameter Name="tx_ALL" Type="String" />
                    <asp:Parameter Name="id_ALBO" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
