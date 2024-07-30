<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="TipologieEventiEcm.aspx.vb" Inherits="Softailor.SiteTailorIzs.TipologieEventiEcm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updTIPOLOGIEECMEVENTI_g" runat="server" Width="500px" Height="409px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdTIPOLOGIEECMEVENTI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_TIPOLOGIAECMEVENTO" DataSourceID="sdsTIPOLOGIEECMEVENTI_g"
                EnableViewState="False" ItemDescriptionPlural="tipologie" ItemDescriptionSingular="tipologia"
                Title="Tipologie ECM Eventi" BoundStlFormViewID="frmTIPOLOGIEECMEVENTI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_NORMATIVAECM" HeaderText="Normativa ECM" />
                    <asp:BoundField DataField="ac_TIPOLOGIAECMEVENTO" HeaderText="Cod. Tipologia" />
                    <asp:BoundField DataField="tx_TIPOLOGIAECMEVENTO" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsTIPOLOGIEECMEVENTI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_TIPOLOGIEECMEVENTI WHERE id_TIPOLOGIAECMEVENTO = @id_TIPOLOGIAECMEVENTO"
                SelectCommand="SELECT * FROM vw_age_TIPOLOGIEECMEVENTI_grid ORDER BY tx_NORMATIVAECM, tx_TIPOLOGIAECMEVENTO">
                <DeleteParameters>
                    <asp:Parameter Name="id_TIPOLOGIAECMEVENTO" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updTIPOLOGIEECMEVENTI_f" runat="server" Width="500px" Height="70px" Top="414px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmTIPOLOGIEECMEVENTI" runat="server" DataKeyNames="id_TIPOLOGIAECMEVENTO"
                DataSourceID="sdsTIPOLOGIEECMEVENTI_f" NewItemText="" BoundStlGridViewID="grdTIPOLOGIEECMEVENTI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 80px;">Normativa</span>
                    <asp:DropDownList ID="ac_NORMATIVAECMDropDownList" runat="server" DataSourceID="sdsac_NORMATIVAECM"
                        DataTextField="tx_NORMATIVAECM" DataValueField="ac_NORMATIVAECM" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("ac_NORMATIVAECM")%>' Width="406px">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 80px;">Codice</span>
                    <asp:TextBox ID="ac_TIPOLOGIAECMEVENTOTextBox" runat="server"
                        Text='<%# Bind("ac_TIPOLOGIAECMEVENTO")%>' Width="80px" />
                    <span class="slbl" style="width: 80px;">Descrizione</span>
                    <asp:TextBox ID="tx_TIPOLOGIAECMEVENTOTextBox" runat="server"
                        Text='<%# Bind("tx_TIPOLOGIAECMEVENTO")%>' Width="238px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsTIPOLOGIEECMEVENTI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_TIPOLOGIEECMEVENTI (ac_NORMATIVAECM, ac_TIPOLOGIAECMEVENTO, tx_TIPOLOGIAECMEVENTO, dt_CREAZIONE, tx_CREAZIONE) VALUES (
    @ac_NORMATIVAECM,
    @ac_TIPOLOGIAECMEVENTO,
    @tx_TIPOLOGIAECMEVENTO,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)                
);
SET @id_TIPOLOGIAECMEVENTO = SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM age_TIPOLOGIEECMEVENTI WHERE id_TIPOLOGIAECMEVENTO=@id_TIPOLOGIAECMEVENTO"
                UpdateCommand="
UPDATE  age_TIPOLOGIEECMEVENTI
SET     ac_NORMATIVAECM = @ac_NORMATIVAECM,
        ac_TIPOLOGIAECMEVENTO = @ac_TIPOLOGIAECMEVENTO,
        tx_TIPOLOGIAECMEVENTO = @tx_TIPOLOGIAECMEVENTO,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   id_TIPOLOGIAECMEVENTO=@id_TIPOLOGIAECMEVENTO
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_NORMATIVAECM" Type="String" />
                    <asp:Parameter Name="ac_TIPOLOGIAECMEVENTO" Type="String" />
                    <asp:Parameter Name="tx_TIPOLOGIAECMEVENTO" Type="String" />
                    <asp:Parameter Name="id_TIPOLOGIAECMEVENTO" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_TIPOLOGIAECMEVENTO" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_NORMATIVAECM" Type="String" />
                    <asp:Parameter Name="ac_TIPOLOGIAECMEVENTO" Type="String" />
                    <asp:Parameter Name="tx_TIPOLOGIAECMEVENTO" Type="String" />
                    <asp:Parameter Name="id_TIPOLOGIAECMEVENTO" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <asp:SqlDataSource ID="sdsac_NORMATIVAECM" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_NORMATIVAECM, tx_NORMATIVAECM FROM age_NORMATIVEECM WHERE ac_NORMATIVAECM<>'NONE' ORDER BY tx_NORMATIVAECM">
    </asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
