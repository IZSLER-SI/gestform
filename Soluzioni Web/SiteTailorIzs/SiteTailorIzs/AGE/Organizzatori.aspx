<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="Organizzatori.aspx.vb" Inherits="Softailor.SiteTailorIzs.Organizzatori" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
     <stl:StlUpdatePanel ID="updORGANIZZATORI_g" runat="server" Width="840px" Height="306px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdORGANIZZATORI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_ORGANIZZATORE" DataSourceID="sdsORGANIZZATORI_g"
                EnableViewState="False" ItemDescriptionPlural="organizzatori" ItemDescriptionSingular="organizzatore"
                Title="Organizzatori Eventi" BoundStlFormViewID="frmORGANIZZATORI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:TemplateField HeaderText="Default" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_DEFAULT"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_ORGANIZZATORE" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_RESPONSABILE" HeaderText="Responsabile" />
                    <asp:BoundField DataField="tx_INDIRIZZO_tratt" HeaderText="Indirizzo" />
                    <asp:BoundField DataField="tx_TELEFONO" HeaderText="Telefono" />
                    <asp:BoundField DataField="tx_EMAIL" HeaderText="E-mail" />
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsORGANIZZATORI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_ORGANIZZATORI WHERE id_ORGANIZZATORE=@id_ORGANIZZATORE"
                SelectCommand="SELECT * FROM vw_age_ORGANIZZATORI_grid ORDER BY fl_DEFAULT desc, tx_ORGANIZZATORE">
                <DeleteParameters>
                    <asp:Parameter Name="id_ORGANIZZATORE" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updORGANIZZATORI_f" runat="server" Width="840px" Height="260px" Top="310px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmORGANIZZATORI" runat="server" DataKeyNames="id_ORGANIZZATORE"
                DataSourceID="sdsORGANIZZATORI_f" NewItemText="" BoundStlGridViewID="grdORGANIZZATORI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 90px;">Nome</span>
                    <asp:TextBox ID="tx_ORGANIZZATORETextBox" runat="server"
                        Text='<%# Bind("tx_ORGANIZZATORE")%>' Width="664px" Font-Bold="true" />
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
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width:195px;">Nome su attestati, con articolo
                        <asp:Image runat="server" ID="Image16" ToolTip="Nome che comparirà sugli attestati, preceduto dall'articolo in minuscolo (esempio: l'Istituto XXX YYY)"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <asp:TextBox ID="tx_ORGANIZZATORE_ATTTextBox" runat="server"
                        Text='<%# Bind("tx_ORGANIZZATORE_ATT")%>' Width="625px" />
                    <br />
                    <span class="flbl" style="width: 90px;">Logo</span>
                    <stl:BinaryElementBox ID="id_ELEME_LOGOBinaryElementBox" runat="server" FieldName="id_ELEME_LOGO"
                        Value='<%# Bind("id_ELEME_LOGO")%>' DefaultCODCATEG="LOG_ORG" 
                        DefaultDescriptionSourceTextBoxID="tx_ORGANIZZATORETextBox" DefaultDescriptionPreamble="Logo " />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsORGANIZZATORI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommandType="StoredProcedure"
                InsertCommand="sp_age_ORGANIZZATORI_Insert"
                SelectCommand="SELECT * FROM vw_age_ORGANIZZATORI_form where id_ORGANIZZATORE=@id_ORGANIZZATORE"
                UpdateCommandType="StoredProcedure"
                UpdateCommand="sp_age_ORGANIZZATORI_Update">
                <UpdateParameters>
                    <asp:Parameter Name="id_ORGANIZZATORE" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_ORGANIZZATORE" Type="String" />
                    <asp:Parameter Name="tx_ORGANIZZATORE_ATT" Type="String" />
                    <asp:Parameter Name="ac_TITOLORESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_NOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_COGNOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="id_ELEME_LOGO" Type="Int32" />
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
                    <asp:Parameter Name="id_ORGANIZZATORE" Type="Int32" DefaultValue="" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="tx_ORGANIZZATORE" Type="String" />
                    <asp:Parameter Name="tx_ORGANIZZATORE_ATT" Type="String" />
                    <asp:Parameter Name="ac_TITOLORESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_NOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="tx_COGNOMERESPONSABILE" Type="String" />
                    <asp:Parameter Name="id_ELEME_LOGO" Type="Int32" />
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
                    <asp:Parameter Name="id_ORGANIZZATORE" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRAPPRLEGALI_ORGANIZZATORI_g" runat="server" Width="480px" Height="400px" Top="0px" Left="850px">
        <ContentTemplate>
            <stl:StlGridView ID="grdRAPPRLEGALI_ORGANIZZATORI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_RAPPRLEGALE_ORGANIZZATORE" DataSourceID="sdsRAPPRLEGALI_ORGANIZZATORI_g"
                EnableViewState="False" ItemDescriptionPlural="persone" ItemDescriptionSingular="persona"
                Title="Rappresentanti dell'organizzatore selezionato" BoundStlFormViewID="frmRAPPRLEGALI_ORGANIZZATORI"
                DeleteConfirmationQuestion="" ParentStlGridViewID="grdORGANIZZATORI">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_RAPPRLEGALE" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_QUALIFICA" HeaderText="Testo Qualifica" />
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsRAPPRLEGALI_ORGANIZZATORI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_RAPPRLEGALI_ORGANIZZATORI WHERE id_RAPPRLEGALE_ORGANIZZATORE=@id_RAPPRLEGALE_ORGANIZZATORE"
                SelectCommand="SELECT * FROM vw_age_RAPPRLEGALI_ORGANIZZATORI_grid WHERE id_ORGANIZZATORE=@id_ORGANIZZATORE ORDER BY id_RAPPRLEGALE_ORGANIZZATORE">
                <SelectParameters>
                    <asp:Parameter Name="id_ORGANIZZATORE" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_RAPPRLEGALE_ORGANIZZATORE" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRAPPRLEGALI_ORGANIZZATORI_f" runat="server" Width="480px" Height="165px" Top="405px" Left="850px">
        <ContentTemplate>
            <stl:StlFormView ID="frmRAPPRLEGALI_ORGANIZZATORI" runat="server" DataKeyNames="id_RAPPRLEGALE_ORGANIZZATORE"
                DataSourceID="sdsRAPPRLEGALI_ORGANIZZATORI_f" NewItemText="" BoundStlGridViewID="grdRAPPRLEGALI_ORGANIZZATORI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 38px;">Titolo</span>
                        <asp:DropDownList ID="ac_TITOLODropDownList" runat="server" DataSourceID="sdsac_TITOLO"
                            DataTextField="tx_TITOLO" DataValueField="ac_TITOLO" AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("ac_TITOLO")%>' Width="77px">
                            <asp:ListItem Text="" Value="" />
                        </asp:DropDownList>
                        <span class="slbl" style="width: 61px;">Cognome</span>
                        <asp:TextBox ID="tx_COGNOMETextBox" runat="server"
                            Text='<%# Bind("tx_COGNOME")%>' Width="120px" Font-Bold="true" />
                        <span class="slbl" style="width: 42px;">Nome</span>
                        <asp:TextBox ID="tx_NOMETextBox" runat="server"
                            Text='<%# Bind("tx_NOME")%>' Width="119px" Font-Bold="true" />
                    <br />
                    <span class="flbl">Qualifica su attestati, con articolo
                        <asp:Image runat="server" ID="Image16" ToolTip="Qualifica che comparirà sugli attestati, preceduto dall'articolo con prima lettera maiuscola (esempio: Il Rappresentante Legale dell'Organizzatore)"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <asp:TextBox ID="tx_QUALIFICATextBox" runat="server"
                        Text='<%# Bind("tx_QUALIFICA")%>' Width="461px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 38px;">Firma</span>
                    <stl:BinaryElementBox ID="id_ELEME_FIRMABinaryElementBox" runat="server" FieldName="id_ELEME_FIRMA"
                        Value='<%# Bind("id_ELEME_FIRMA")%>' DefaultCODCATEG="FIR_ATT" 
                        DefaultDescriptionSourceTextBoxID="tx_COGNOMETextBox" DefaultDescriptionPreamble="Firma " />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsRAPPRLEGALI_ORGANIZZATORI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_RAPPRLEGALI_ORGANIZZATORI (
	id_ORGANIZZATORE,
	ac_TITOLO,
	tx_COGNOME,
	tx_NOME,
	tx_QUALIFICA,
	id_ELEME_FIRMA,
	dt_CREAZIONE,
	tx_CREAZIONE
) VALUES (
	@id_ORGANIZZATORE,
	@ac_TITOLO,
	@tx_COGNOME,
	@tx_NOME,
	@tx_QUALIFICA,
	@id_ELEME_FIRMA,
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE id_UTENT=@id_UTENT)
);
SELECT @id_RAPPRLEGALE_ORGANIZZATORE = SCOPE_IDENTITY()               
                "
                SelectCommand="SELECT * FROM age_RAPPRLEGALI_ORGANIZZATORI WHERE id_RAPPRLEGALE_ORGANIZZATORE=@id_RAPPRLEGALE_ORGANIZZATORE"
                UpdateCommand="

UPDATE  age_RAPPRLEGALI_ORGANIZZATORI
SET     ac_TITOLO = @ac_TITOLO,
	    tx_COGNOME = @tx_COGNOME,
	    tx_NOME = @tx_NOME,
	    tx_QUALIFICA = @tx_QUALIFICA,
	    id_ELEME_FIRMA = @id_ELEME_FIRMA,
	    dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE id_UTENT=@id_UTENT)
WHERE   id_RAPPRLEGALE_ORGANIZZATORE = @id_RAPPRLEGALE_ORGANIZZATORE
                ">
                <UpdateParameters>
	                <asp:Parameter Name="ac_TITOLO" Type="String" />
	                <asp:Parameter Name="tx_COGNOME" Type="String" />
	                <asp:Parameter Name="tx_NOME" Type="String" />
	                <asp:Parameter Name="tx_QUALIFICA" Type="String" />
	                <asp:Parameter Name="id_ELEME_FIRMA" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_RAPPRLEGALE_ORGANIZZATORE" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_RAPPRLEGALE_ORGANIZZATORE" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_ORGANIZZATORE" Type="Int32" />
	                <asp:Parameter Name="ac_TITOLO" Type="String" />
	                <asp:Parameter Name="tx_COGNOME" Type="String" />
	                <asp:Parameter Name="tx_NOME" Type="String" />
	                <asp:Parameter Name="tx_QUALIFICA" Type="String" />
	                <asp:Parameter Name="id_ELEME_FIRMA" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_RAPPRLEGALE_ORGANIZZATORE" Type="Int32" Direction="Output" />
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
