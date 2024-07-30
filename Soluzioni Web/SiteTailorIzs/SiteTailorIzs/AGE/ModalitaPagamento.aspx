<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ModalitaPagamento.aspx.vb" Inherits="Softailor.SiteTailorIzs.ModalitaPagamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updTIPIPAG_g" runat="server" Width="600px" Height="515px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdTIPIPAG" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_TIPOPAG" DataSourceID="sdsTIPIPAG_g"
                EnableViewState="False" ItemDescriptionPlural="tipologie" ItemDescriptionSingular="tipologia"
                Title="Sezioni Documenti Sito Pubblico" BoundStlFormViewID="frmTIPIPAG"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ac_TIPOPAG" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_TIPOPAG" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsTIPIPAG_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_TIPIPAG WHERE ac_TIPOPAG = @ac_TIPOPAG"
                SelectCommand="SELECT * FROM age_TIPIPAG ORDER BY tx_TIPOPAG">
                <DeleteParameters>
                    <asp:Parameter Name="ac_TIPOPAG" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updTIPIPAG_f" runat="server" Width="600px" Height="50px" Top="520px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmTIPIPAG" runat="server" DataKeyNames="ac_TIPOPAG"
                DataSourceID="sdsTIPIPAG_f" NewItemText="" BoundStlGridViewID="grdTIPIPAG">
                <EditItemTemplate>
                    <span class="flbl" style="width: 45px;">Codice</span>
                    <asp:TextBox ID="ac_TIPOPAGTextBox" runat="server"
                        Text='<%# Bind("ac_TIPOPAG")%>' Width="100px" />
                    <span class="slbl" style="width: 70px;">Descrizione</span>
                    <asp:TextBox ID="tx_TIPOPAGTextBox" runat="server"
                        Text='<%# Bind("tx_TIPOPAG")%>' Width="360px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsTIPIPAG_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_TIPIPAG (ac_TIPOPAG, tx_TIPOPAG) 
VALUES (@ac_TIPOPAG, @tx_TIPOPAG)
                "
                SelectCommand="SELECT * FROM age_TIPIPAG WHERE ac_TIPOPAG=@ac_TIPOPAG"
                UpdateCommand="
UPDATE  age_TIPIPAG
SET     tx_TIPOPAG = @tx_TIPOPAG
WHERE   ac_TIPOPAG=@ac_TIPOPAG
                ">
                <UpdateParameters>
                    <asp:Parameter Name="tx_TIPOPAG" Type="String" />
                    <asp:Parameter Name="ac_TIPOPAG" Type="String" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_TIPOPAG" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_TIPOPAG" Type="String" />
                    <asp:Parameter Name="tx_TIPOPAG" Type="String" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
