<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="Delibere.aspx.vb" Inherits="Softailor.SiteTailorIzs.Delibere" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updsrcDELIBERE" runat="server" Width="940px" Height="75px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlSearchForm ID="srcDELIBERE" runat="server"
                SearchName="Ricerca Delibere" LayoutType="Horizontal" Title="Ricerca/Creazione Delibere" CreationConfirmationText="Confermi la creazione della delibera?">
            </stl:StlSearchForm>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updDELIBERE_g" runat="server" Width="940px" Height="365px" Top="80px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdDELIBERE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_DELIBERA" DataSourceID="sdsDELIBERE_g"
                EnableViewState="False" ItemDescriptionPlural="delibere" ItemDescriptionSingular="delibera"
                Title="Risultati Ricerca" BoundStlFormViewID="frmDELIBERE"
                DeleteConfirmationQuestion="" SqlStringProviderID="srcDELIBERE">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="dt_DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ac_DELIBERA" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_DELIBERA" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_TIPOLOGIADELIBERA" HeaderText="Tipologia" />
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsDELIBERE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_DELIBERE WHERE id_DELIBERA=@id_DELIBERA"
                SelectCommand="DUMMY">
                <DeleteParameters>
                    <asp:Parameter Name="id_DELIBERA" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updDELIBERE_f" runat="server" Width="940px" Height="118px" Top="450px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmDELIBERE" runat="server" DataKeyNames="id_DELIBERA"
                DataSourceID="sdsDELIBERE_f" NewItemText="" BoundStlGridViewID="grdDELIBERE">
                <EditItemTemplate>
                    <span class="flbl" style="width: 60px;">Data</span>
                    <asp:TextBox ID="dt_DATATextBox" runat="server"
                        Text='<%# Bind("dt_DATA", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width: 45px;">Codice</span>
                    <asp:TextBox ID="ac_DELIBERATextBox" runat="server"
                        Text='<%# Bind("ac_DELIBERA")%>' Width="80px" />
                    <span class="slbl" style="width: 70px;">Descrizione</span>
                    <asp:TextBox ID="tx_DELIBERATextBox" runat="server"
                        Text='<%# Bind("tx_DELIBERA")%>' Width="579px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 60px;">Tipologia</span>
                    <asp:DropDownList ID="ac_TIPOLOGIADELIBERADropDownList" runat="server" DataSourceID="sdsac_TIPOLOGIADELIBERA"
                        DataTextField="tx_TIPOLOGIADELIBERA" DataValueField="ac_TIPOLOGIADELIBERA" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("ac_TIPOLOGIADELIBERA")%>' Width="866px">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 60px;">Note</span>
                    <asp:TextBox ID="tx_NOTETextBox" runat="server"
                        Text='<%# Bind("tx_NOTE")%>' Width="862px" TextMode="MultiLine" Height="40px" />

                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsDELIBERE_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT * FROM age_DELIBERE WHERE id_DELIBERA=@id_DELIBERA"
                UpdateCommand="
UPDATE  age_DELIBERE
SET     dt_DATA=@dt_DATA,
        ac_DELIBERA = @ac_DELIBERA,
        tx_DELIBERA = @tx_DELIBERA,
        ac_TIPOLOGIADELIBERA = @ac_TIPOLOGIADELIBERA,
        tx_NOTE = @tx_NOTE,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
WHERE   id_DELIBERA = @id_DELIBERA
                ">
                <UpdateParameters>
                    <asp:Parameter Name="dt_DATA" Type="DateTime" />
                    <asp:Parameter Name="ac_DELIBERA" Type="String" />
                    <asp:Parameter Name="tx_DELIBERA" Type="String" />
                    <asp:Parameter Name="ac_TIPOLOGIADELIBERA" Type="String" />
                    <asp:Parameter Name="tx_NOTE" Type="String" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_DELIBERA" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_DELIBERA" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:SqlDataSource ID="sdsac_TIPOLOGIADELIBERA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPOLOGIADELIBERA, tx_TIPOLOGIADELIBERA FROM age_TIPOLOGIEDELIBERE ORDER BY tx_TIPOLOGIADELIBERA">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
