<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SediAule.aspx.vb" Inherits="Softailor.SiteTailorIzs.SediAule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updSEDI_g" runat="server" Width="840px" Height="430px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdSEDI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_SEDE" DataSourceID="sdsSEDI_g"
                EnableViewState="False" ItemDescriptionPlural="sedi" ItemDescriptionSingular="sede"
                Title="Sedi" BoundStlFormViewID="frmSEDI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:TemplateField HeaderText="Abituale" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_ABITUALE"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_SEDE" HeaderText="Nome" ItemStyle-Font-Bold="true" />
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
            <stl:StlSqlDataSource ID="sdsSEDI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_SEDI WHERE id_SEDE=@id_SEDE"
                SelectCommand="SELECT * FROM vw_age_SEDI_grid ORDER BY fl_ABITUALE desc, tx_SEDE">
                <DeleteParameters>
                    <asp:Parameter Name="id_SEDE" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updSEDI_f" runat="server" Width="840px" Height="134px" Top="435px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmSEDI" runat="server" DataKeyNames="id_SEDE"
                DataSourceID="sdsSEDI_f" NewItemText="" BoundStlGridViewID="grdSEDI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 90px;">Nome Sede</span>
                    <asp:TextBox ID="tx_SEDETextBox" runat="server"
                        Text='<%# Bind("tx_SEDE")%>' Width="664px" Font-Bold="true" />
                    <span class="slbl" style="width: 50px;">Abituale</span>
                    <asp:CheckBox ID="fl_ABITUALECheckBox" runat="server"
                        Checked='<%# Bind("fl_ABITUALE")%>' />
                    <br />
                    <div style="display: block; float: left; width: 584px;">
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
            <stl:StlSqlDataSource ID="sdsSEDI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"

                InsertCommandType="StoredProcedure"
                InsertCommand="sp_age_SEDI_Insert"
                SelectCommand="SELECT * FROM vw_age_SEDI_form where id_SEDE=@id_SEDE"
                UpdateCommandType="StoredProcedure"
                UpdateCommand="sp_age_SEDI_Update">
                <UpdateParameters>
                    <asp:Parameter Name="id_SEDE" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_SEDE" Type="String" />
                    <asp:Parameter Name="fl_ABITUALE" Type="Boolean" />
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
                    <asp:Parameter Name="id_SEDE" Type="Int32" DefaultValue="" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="tx_SEDE" Type="String" />
                    <asp:Parameter Name="fl_ABITUALE" Type="Boolean" />
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
	                <asp:Parameter Name="id_SEDE" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updAULE_g" runat="server" Width="380px" Height="430px" Top="0px" Left="850px">
        <ContentTemplate>
            <stl:StlGridView ID="grdAULE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_AULA" DataSourceID="sdsAULE_g"
                EnableViewState="False" ItemDescriptionPlural="aule" ItemDescriptionSingular="aula"
                Title="Aule della sede selezionata" BoundStlFormViewID="frmAULE"
                DeleteConfirmationQuestion="" ParentStlGridViewID="grdSEDI">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_AULA" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ni_CAPIENZAPAX" HeaderText="Capienza" />
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsAULE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_AULE WHERE id_AULA=@id_AULA"
                SelectCommand="SELECT * FROM vw_age_AULE_grid WHERE id_SEDE=@id_SEDE ORDER BY tx_AULA">
                <SelectParameters>
                    <asp:Parameter Name="id_SEDE" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_AULA" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updAULE_f" runat="server" Width="380px" Height="134px" Top="435px" Left="850px">
        <ContentTemplate>
            <stl:StlFormView ID="frmAULE" runat="server" DataKeyNames="id_AULA"
                DataSourceID="sdsAULE_f" NewItemText="" BoundStlGridViewID="grdAULE">
                <EditItemTemplate>
                    <span class="flbl" style="width: 110px;">Nome Aula</span>
                    <asp:TextBox ID="tx_AULATextBox" runat="server"
                        Text='<%# Bind("tx_AULA")%>' Width="250px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 110px;">Capienza (persone)</span>
                    <asp:TextBox ID="ni_CAPIENZAPAXTextBox" runat="server"
                        Text='<%# Bind("ni_CAPIENZAPAX")%>' Width="90px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsAULE_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_AULE
    (id_SEDE, tx_AULA, ni_CAPIENZAPAX, dt_CREAZIONE, tx_CREAZIONE)
VALUES (
	@id_SEDE, @tx_AULA, @ni_CAPIENZAPAX,
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE id_UTENT=@id_UTENT)
);
SELECT @id_AULA = SCOPE_IDENTITY()               
                "
                SelectCommand="SELECT * FROM age_AULE WHERE id_AULA=@id_AULA"
                UpdateCommand="
UPDATE  age_AULE
SET     tx_AULA = @tx_AULA,
        ni_CAPIENZAPAX = @ni_CAPIENZAPAX,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE id_UTENT=@id_UTENT)
WHERE   id_AULA = @id_AULA   
                ">
                <UpdateParameters>
	                <asp:Parameter Name="tx_AULA" Type="String" />
                    <asp:Parameter Name="ni_CAPIENZAPAX" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_AULA" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_AULA" Type="Int32" DefaultValue="" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_SEDE" Type="Int32" />
	                <asp:Parameter Name="tx_AULA" Type="String" />
                    <asp:Parameter Name="ni_CAPIENZAPAX" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_AULA" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
