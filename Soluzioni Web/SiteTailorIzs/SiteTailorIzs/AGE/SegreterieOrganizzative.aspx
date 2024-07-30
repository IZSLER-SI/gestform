<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SegreterieOrganizzative.aspx.vb" Inherits="Softailor.SiteTailorIzs.SegreterieOrganizzative" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updSEGRETERIEORGANIZZATIVE_g" runat="server" Width="840px" Height="409px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdSEGRETERIEORGANIZZATIVE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_SEGRETERIAORGANIZZATIVA" DataSourceID="sdsSEGRETERIEORGANIZZATIVE_g"
                EnableViewState="False" ItemDescriptionPlural="segreterie" ItemDescriptionSingular="segreteria"
                Title="Segreterie Organizzative" BoundStlFormViewID="frmSEGRETERIEORGANIZZATIVE"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:TemplateField HeaderText="Default" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_DEFAULT"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_SEGRETERIAORGANIZZATIVA" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_RESPONSABILE" HeaderText="Responsabile" />
                    <asp:BoundField DataField="tx_INDIRIZZO_tratt" HeaderText="Indirizzo" />
                    <asp:BoundField DataField="tx_TELEFONO" HeaderText="Telefono" />
                    <asp:BoundField DataField="tx_EMAIL" HeaderText="E-mail" />
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsSEGRETERIEORGANIZZATIVE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_SEGRETERIEORGANIZZATIVE WHERE id_SEGRETERIAORGANIZZATIVA=@id_SEGRETERIAORGANIZZATIVA"
                SelectCommand="SELECT * FROM vw_age_SEGRETERIEORGANIZZATIVE_grid ORDER BY fl_DEFAULT desc, tx_SEGRETERIAORGANIZZATIVA">
                <DeleteParameters>
                    <asp:Parameter Name="id_SEGRETERIAORGANIZZATIVA" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updSEGRETERIEORGANIZZATIVE_f" runat="server" Width="840px" Height="155px" Top="414px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmSEGRETERIEORGANIZZATIVE" runat="server" DataKeyNames="id_SEGRETERIAORGANIZZATIVA"
                DataSourceID="sdsSEGRETERIEORGANIZZATIVE_f" NewItemText="" BoundStlGridViewID="grdSEGRETERIEORGANIZZATIVE">
                <EditItemTemplate>
                    <span class="flbl" style="width: 90px;">Nome</span>
                    <asp:TextBox ID="tx_SEGRETERIAORGANIZZATIVATextBox" runat="server"
                        Text='<%# Bind("tx_SEGRETERIAORGANIZZATIVA")%>' Width="664px" Font-Bold="true" />
                    <span class="slbl" style="width: 50px;">Default</span>
                    <asp:CheckBox ID="fl_DEFAULTCheckBox" runat="server"
                        Checked='<%# Bind("fl_DEFAULT")%>' />
                    <div style="display: block; float: left; width: 584px;">
                        <span class="flbl" style="width: 90px;">Responsabile</span>
                        <span class="flbl" style="width: 38px;">Titolo</span>
                        <asp:DropDownList ID="ac_TITOLORESPONSABILEDropDownList" runat="server" DataSourceID="sdsac_TITOLO"
                            DataTextField="tx_TITOLO" DataValueField="ac_TITOLO" AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("ac_TITOLORESPONSABILE")%>' Width="80px">
                            <asp:ListItem Text="" Value="" />
                        </asp:DropDownList>
                        <span class="slbl" style="width: 61px;">Cognome</span>
                        <asp:TextBox ID="tx_COGNOMERESPONSABILETextBox" runat="server"
                            Text='<%# Bind("tx_COGNOMERESPONSABILE")%>' Width="128px" Font-Bold="true" />
                        <span class="slbl" style="width: 42px;">Nome</span>
                        <asp:TextBox ID="tx_NOMERESPONSABILETextBox" runat="server"
                            Text='<%# Bind("tx_NOMERESPONSABILE")%>' Width="127px" Font-Bold="true" />
                        <br />
                        <bof:CtlSelettoreIndirizzo runat="server" ID="ctlSelettoreIndirizzo"
                            FieldName="tx_INDIRIZZO"
                            FirstLabelWidthPx="90"
                            ac_tipoindirizzo='<%# Bind("ac_TIPOINDIRIZZO") %>'
                            tx_indirizzo='<%# Bind("tx_INDIRIZZO")%>'
                            ac_cap_ac_comune='<%# Bind("ac_CAP_ac_COMUNE") %>'
                            tx_localita='<%# Bind("tx_LOCALITA") %>'
                            tx_postalcode='<%# Bind("tx_POSTALCODE") %>'
                            tx_city='<%# Bind("tx_CITY") %>'
                            tx_provincia='<%# Bind("tx_PROVINCIA") %>'
                            tx_stato='<%# Bind("tx_STATO") %>'
                            ac_nazione='<%# Bind("ac_NAZIONE") %>' />
                    </div>
                    <div style="display: block; float: left; width: 240px">
                        <span class="flbl" style="width: 36px;">Tel</span>
                        <asp:TextBox ID="tx_TELEFONOTextBox" runat="server" Text='<%# Bind("tx_TELEFONO") %>'
                            Width="200px" />
                        <br />
                        <span class="flbl" style="width: 36px;">Fax</span>
                        <asp:TextBox ID="tx_FAXTextBox" runat="server" Text='<%# Bind("tx_FAX")%>'
                            Width="200px" />
                        <br />
                        <span class="flbl" style="width: 36px;">Cell</span>
                        <asp:TextBox ID="tx_CELLULARETextBox" runat="server" Text='<%# Bind("tx_CELLULARE")%>'
                            Width="200px" />
                        <br />
                        <span class="flbl" style="width: 36px;">E-mail</span>
                        <stl:StlEmailTextBox runat="server" ID="tx_EMAILStlEmailTextBox"
                            FieldName="tx_EMAIL" Value='<%# Bind("tx_EMAIL") %>' Width="200px" />
                        <br />
                        <span class="flbl" style="width: 36px;">PEC</span>
                        <stl:StlEmailTextBox runat="server" ID="tx_EMAILPECStlEmailTextBox"
                            FieldName="tx_EMAILPEC" Value='<%# Bind("tx_EMAILPEC") %>' Width="200px" />
                    </div>
                    <div style="clear: both">
                    </div>
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsSEGRETERIEORGANIZZATIVE_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommandType="StoredProcedure"
                InsertCommand="sp_age_SEGRETERIEORGANIZZATIVE_Insert"
                SelectCommand="SELECT * FROM vw_age_SEGRETERIEORGANIZZATIVE_form where id_SEGRETERIAORGANIZZATIVA=@id_SEGRETERIAORGANIZZATIVA"
                UpdateCommandType="StoredProcedure"
                UpdateCommand="sp_age_SEGRETERIEORGANIZZATIVE_Update">
                <UpdateParameters>
                    <asp:Parameter Name="id_SEGRETERIAORGANIZZATIVA" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_SEGRETERIAORGANIZZATIVA" Type="String" />
                    <asp:Parameter Name="ac_TITOLORESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_NOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_COGNOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="fl_DEFAULT" Type="Boolean" />
                    <asp:Parameter Name="ac_TIPOINDIRIZZO" Type="String" />
                    <asp:Parameter Name="tx_ENTE" Type="String" />
                    <asp:Parameter Name="tx_DIPARTIMENTO" Type="String" />
                    <asp:Parameter Name="tx_INDIRIZZO" Type="String" />
                    <asp:Parameter Name="ac_CAP_ac_COMUNE" Type="String" />
                    <asp:Parameter Name="tx_POSTALCODE" Type="String" />
                    <asp:Parameter Name="tx_LOCALITA" Type="String" />
                    <asp:Parameter Name="tx_CITY" Type="String" />
                    <asp:Parameter Name="tx_PROVINCIA" Type="String" />
                    <asp:Parameter Name="tx_STATO" Type="String" />
                    <asp:Parameter Name="ac_NAZIONE" Type="String" />
                    <asp:Parameter Name="tx_TELEFONO" Type="String" />
                    <asp:Parameter Name="tx_TELEFONO2" Type="String" />
                    <asp:Parameter Name="tx_FAX" Type="String" />
                    <asp:Parameter Name="tx_CELLULARE" Type="String" />
                    <asp:Parameter Name="tx_CELLULARE2" Type="String" />
                    <asp:Parameter Name="tx_EMAIL" Type="String" />
                    <asp:Parameter Name="tx_EMAILPEC" Type="String" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_SEGRETERIAORGANIZZATIVA" Type="Int32" DefaultValue="" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_SEGRETERIAORGANIZZATIVA" Type="String" />
                    <asp:Parameter Name="ac_TITOLORESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_NOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_COGNOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="fl_DEFAULT" Type="Boolean" />
                    <asp:Parameter Name="ac_TIPOINDIRIZZO" Type="String" />
                    <asp:Parameter Name="tx_ENTE" Type="String" />
                    <asp:Parameter Name="tx_DIPARTIMENTO" Type="String" />
                    <asp:Parameter Name="tx_INDIRIZZO" Type="String" />
                    <asp:Parameter Name="ac_CAP_ac_COMUNE" Type="String" />
                    <asp:Parameter Name="tx_POSTALCODE" Type="String" />
                    <asp:Parameter Name="tx_LOCALITA" Type="String" />
                    <asp:Parameter Name="tx_CITY" Type="String" />
                    <asp:Parameter Name="tx_PROVINCIA" Type="String" />
                    <asp:Parameter Name="tx_STATO" Type="String" />
                    <asp:Parameter Name="ac_NAZIONE" Type="String" />
                    <asp:Parameter Name="tx_TELEFONO" Type="String" />
                    <asp:Parameter Name="tx_TELEFONO2" Type="String" />
                    <asp:Parameter Name="tx_FAX" Type="String" />
                    <asp:Parameter Name="tx_CELLULARE" Type="String" />
                    <asp:Parameter Name="tx_CELLULARE2" Type="String" />
                    <asp:Parameter Name="tx_EMAIL" Type="String" />
                    <asp:Parameter Name="tx_EMAILPEC" Type="String" />
                    <asp:Parameter Name="id_SEGRETERIAORGANIZZATIVA" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:SqlDataSource ID="sdsac_TITOLO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TITOLO, tx_TITOLO FROM age_TITOLI ORDER BY tx_TITOLO">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
