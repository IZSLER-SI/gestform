<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SezioniDocumentiEventi.aspx.vb" Inherits="Softailor.SiteTailorIzs.SezioniDocumentiEventi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updSEZIONIDOCEVENTI_g" runat="server" Width="900px" Height="474px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdSEZIONIDOCEVENTI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_SEZIONEDOCEVENTO" DataSourceID="sdsSEZIONIDOCEVENTI_g"
                EnableViewState="False" ItemDescriptionPlural="tipologie" ItemDescriptionSingular="tipologia"
                Title="Sezioni Documenti Sito Pubblico" BoundStlFormViewID="frmSEZIONIDOCEVENTI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ni_ORDINE" HeaderText="N° Ordine" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ac_SEZIONEDOCEVENTO" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_SEZIONEDOCEVENTO_PLUR" HeaderText="Descrizione (plurale)" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_SEZIONEDOCEVENTO_SING" HeaderText="Descrizione (singolare)" ItemStyle-Font-Bold="true" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsSEZIONIDOCEVENTI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM fof_SEZIONIDOCEVENTI WHERE ac_SEZIONEDOCEVENTO = @ac_SEZIONEDOCEVENTO"
                SelectCommand="SELECT * FROM fof_SEZIONIDOCEVENTI ORDER BY ni_ORDINE">
                <DeleteParameters>
                    <asp:Parameter Name="ac_SEZIONEDOCEVENTO" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updSEZIONIDOCEVENTI_f" runat="server" Width="900px" Height="91px" Top="479px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmSEZIONIDOCEVENTI" runat="server" DataKeyNames="ac_SEZIONEDOCEVENTO"
                DataSourceID="sdsSEZIONIDOCEVENTI_f" NewItemText="" BoundStlGridViewID="grdSEZIONIDOCEVENTI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 60px;">N° Ordine</span>
                    <asp:TextBox ID="ni_ORDINETextBox" runat="server"
                        Text='<%# Bind("ni_ORDINE")%>' Width="56px" />

                    <span class="slbl" style="width: 45px;">Codice</span>
                    <asp:TextBox ID="ac_SEZIONEDOCEVENTOTextBox" runat="server"
                        Text='<%# Bind("ac_SEZIONEDOCEVENTO")%>' Width="120px" />
                    
                    <br />
                    <span class="flbl" style="width: 125px;">Descrizione (plurale)</span>
                    <asp:TextBox ID="tx_SEZIONEDOCEVENTO_PLURTextBox" runat="server"
                        Text='<%# Bind("tx_SEZIONEDOCEVENTO_PLUR")%>' Width="757px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 125px;">Descrizione (singolare)</span>
                    <asp:TextBox ID="tx_SEZIONEDOCEVENTO_SINGTextBox" runat="server"
                        Text='<%# Bind("tx_SEZIONEDOCEVENTO_SING")%>' Width="757px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsSEZIONIDOCEVENTI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO fof_SEZIONIDOCEVENTI (ac_SEZIONEDOCEVENTO, tx_SEZIONEDOCEVENTO_SING, tx_SEZIONEDOCEVENTO_PLUR, ni_ORDINE) 
VALUES (@ac_SEZIONEDOCEVENTO, @tx_SEZIONEDOCEVENTO_SING, @tx_SEZIONEDOCEVENTO_PLUR, @ni_ORDINE)
                "
                SelectCommand="SELECT * FROM fof_SEZIONIDOCEVENTI WHERE ac_SEZIONEDOCEVENTO=@ac_SEZIONEDOCEVENTO"
                UpdateCommand="
UPDATE  fof_SEZIONIDOCEVENTI
SET     tx_SEZIONEDOCEVENTO_SING = @tx_SEZIONEDOCEVENTO_SING,
        tx_SEZIONEDOCEVENTO_PLUR = @tx_SEZIONEDOCEVENTO_PLUR,
        ni_ORDINE = @ni_ORDINE
WHERE   ac_SEZIONEDOCEVENTO=@ac_SEZIONEDOCEVENTO
                ">
                <UpdateParameters>
                    <asp:Parameter Name="tx_SEZIONEDOCEVENTO_SING" Type="String" />
                    <asp:Parameter Name="tx_SEZIONEDOCEVENTO_PLUR" Type="String" />
                    <asp:Parameter Name="ni_ORDINE" Type="Int32" />
                    <asp:Parameter Name="ac_SEZIONEDOCEVENTO" Type="String" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_SEZIONEDOCEVENTO" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_SEZIONEDOCEVENTO" Type="String" />
                    <asp:Parameter Name="tx_SEZIONEDOCEVENTO_SING" Type="String" />
                    <asp:Parameter Name="tx_SEZIONEDOCEVENTO_PLUR" Type="String" />
                    <asp:Parameter Name="ni_ORDINE" Type="Int32" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
