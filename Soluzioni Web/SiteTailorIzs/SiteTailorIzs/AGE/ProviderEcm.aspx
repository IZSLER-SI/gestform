<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ProviderEcm.aspx.vb" Inherits="Softailor.SiteTailorIzs.ProviderEcm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">

    <stl:StlUpdatePanel ID="updPROVIDERECM_g" runat="server" Width="740px" Height="380px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPROVIDERECM" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_PROVIDERECM" DataSourceID="sdsPROVIDERECM_g"
                EnableViewState="False" ItemDescriptionPlural="provider" ItemDescriptionSingular="provider"
                Title="Provider ECM" BoundStlFormViewID="frmPROVIDERECM"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />

                    <asp:BoundField DataField="tx_NORMATIVAECM" HeaderText="Normativa ECM" />
                    <asp:BoundField DataField="tx_PROVIDERECM" HeaderText="Descrizione Provider" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_ACCREDITAMENTO" HeaderText="N°Accred." />
                    <asp:TemplateField HeaderText="Accr.provvisorio" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_ACCREDITAMENTOPROVVISORIO"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dt_SCADENZAACCREDITAMENTO" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Valido fino al" />
                    <asp:TemplateField HeaderText="Logo">
                        <ItemTemplate>
                            <a href='<%# Page.ResolveUrl("~/Binaries/ElementPreview.aspx?id=" & Eval("id_ELEME_LOGO"))%>' target="_blank">
                                <img width="70" height="70" border="0" src='<%# Page.ResolveUrl("~/Binaries/BOThumbnail.aspx?id=" & Eval("id_ELEME_LOGO"))%>' />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsPROVIDERECM_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_PROVIDERECM WHERE id_PROVIDERECM=@id_PROVIDERECM"
                SelectCommand="SELECT * FROM vw_age_PROVIDERECM_grid ORDER BY tx_PROVIDERECM">
                <DeleteParameters>
                    <asp:Parameter Name="id_PROVIDERECM" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPROVIDERECM_f" runat="server" Width="740px" Height="186px" Top="385px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmPROVIDERECM" runat="server" DataKeyNames="id_PROVIDERECM"
                DataSourceID="sdsPROVIDERECM_f" NewItemText="" BoundStlGridViewID="grdPROVIDERECM">
                <EditItemTemplate>
                    <span class="flbl" style="width: 100px;">Normativa</span>
                    <asp:DropDownList ID="ac_NORMATIVAECMDropDownList" runat="server" DataSourceID="sdsac_NORMATIVAECM"
                        DataTextField="tx_NORMATIVAECM" DataValueField="ac_NORMATIVAECM" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("ac_NORMATIVAECM")%>' Width="200px">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 115px;">Descrizione Provider</span>
                    <asp:TextBox ID="tx_PROVIDERECMTextBox" runat="server"
                        Text='<%# Bind("tx_PROVIDERECM")%>' Width="307px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 100px;">N°Accreditamento</span>
                    <asp:TextBox ID="ac_ACCREDITAMENTOTextBox" runat="server"
                        Text='<%# Bind("ac_ACCREDITAMENTO")%>' Width="80px" />
                    
                    <span class="slbl" style="width: 145px;">Accreditamento provvisorio</span>
                    <asp:CheckBox ID="fl_ACCREDITAMENTOPROVVISORIOCheckBox" runat="server"
                        Checked='<%# Bind("fl_ACCREDITAMENTOPROVVISORIO")%>' />
                    <span class="slbl" style="width: 85px;">Valido fino al</span>
                    <asp:TextBox ID="dt_SCADENZAACCREDITAMENTOTextBox" runat="server"
                        Text='<%# Bind("dt_SCADENZAACCREDITAMENTO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <br />
                    <span class="flbl">Nome su attestati ECM, senza articolo
                        <asp:Image runat="server" ID="Image16" ToolTip="Nome per esteso del Provider, senza articolo"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <asp:TextBox ID="tx_PROVIDERECM_ATTTextBox" runat="server"
                        Text='<%# Bind("tx_PROVIDERECM_ATT")%>' Width="723px" />
                    <br />
                    <span class="flbl" style="width: 100px;">Logo</span>
                    <stl:BinaryElementBox ID="id_ELEME_LOGOBinaryElementBox" runat="server" FieldName="id_ELEME_LOGO"
                        Value='<%# Bind("id_ELEME_LOGO")%>' DefaultCODCATEG="LOG_PRO" 
                        DefaultDescriptionSourceTextBoxID="tx_PROVIDERECMTextBox" DefaultDescriptionPreamble="Logo " />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsPROVIDERECM_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_PROVIDERECM (
	ac_NORMATIVAECM,
	tx_PROVIDERECM,
	ac_ACCREDITAMENTO,
	fl_ACCREDITAMENTOPROVVISORIO,
	dt_SCADENZAACCREDITAMENTO,
	tx_PROVIDERECM_ATT,
	id_ELEME_LOGO,
	dt_CREAZIONE,
	tx_CREAZIONE
) VALUES (
	@ac_NORMATIVAECM,
	@tx_PROVIDERECM,
	@ac_ACCREDITAMENTO,
	@fl_ACCREDITAMENTOPROVVISORIO,
	@dt_SCADENZAACCREDITAMENTO,
	@tx_PROVIDERECM_ATT,
	@id_ELEME_LOGO,
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
);
SET @id_PROVIDERECM = SCOPE_IDENTITY()              
                "
                SelectCommand="SELECT * FROM age_PROVIDERECM WHERE id_PROVIDERECM=@id_PROVIDERECM"
                UpdateCommand="

UPDATE  age_PROVIDERECM
SET     ac_NORMATIVAECM = @ac_NORMATIVAECM,
        tx_PROVIDERECM = @tx_PROVIDERECM,
        ac_ACCREDITAMENTO = @ac_ACCREDITAMENTO,
        fl_ACCREDITAMENTOPROVVISORIO = @fl_ACCREDITAMENTOPROVVISORIO,
        dt_SCADENZAACCREDITAMENTO = @dt_SCADENZAACCREDITAMENTO,
        tx_PROVIDERECM_ATT = @tx_PROVIDERECM_ATT,
        id_ELEME_LOGO = @id_ELEME_LOGO,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   id_PROVIDERECM=@id_PROVIDERECM
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_NORMATIVAECM" Type="String" />
                    <asp:Parameter Name="tx_PROVIDERECM" Type="String" />
                    <asp:Parameter Name="ac_ACCREDITAMENTO" Type="String" />
                    <asp:Parameter Name="fl_ACCREDITAMENTOPROVVISORIO" Type="Boolean" />
                    <asp:Parameter Name="dt_SCADENZAACCREDITAMENTO" Type="DateTime" />
                    <asp:Parameter Name="tx_PROVIDERECM_ATT" Type="String" />
                    <asp:Parameter Name="id_ELEME_LOGO" Type="Int32" />
                    <asp:Parameter Name="id_PROVIDERECM" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_PROVIDERECM" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_NORMATIVAECM" Type="String" />
                    <asp:Parameter Name="tx_PROVIDERECM" Type="String" />
                    <asp:Parameter Name="ac_ACCREDITAMENTO" Type="String" />
                    <asp:Parameter Name="fl_ACCREDITAMENTOPROVVISORIO" Type="Boolean" />
                    <asp:Parameter Name="dt_SCADENZAACCREDITAMENTO" Type="DateTime" />
                    <asp:Parameter Name="tx_PROVIDERECM_ATT" Type="String" />
                    <asp:Parameter Name="id_ELEME_LOGO" Type="Int32" />
                    <asp:Parameter Name="id_PROVIDERECM" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRAPPRLEGALI_PROVIDERECM_g" runat="server" Width="480px" Height="401px" Top="0px" Left="750px">
        <ContentTemplate>
            <stl:StlGridView ID="grdRAPPRLEGALI_PROVIDERECM" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_RAPPRLEGALE_PROVIDERECM" DataSourceID="sdsRAPPRLEGALI_PROVIDERECM_g"
                EnableViewState="False" ItemDescriptionPlural="persone" ItemDescriptionSingular="persona"
                Title="Rappresentanti del provider selezionato" BoundStlFormViewID="frmRAPPRLEGALI_PROVIDERECM"
                DeleteConfirmationQuestion="" ParentStlGridViewID="grdPROVIDERECM">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_RAPPRLEGALE" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_QUALIFICA" HeaderText="Testo Qualifica" />
                    <asp:TemplateField HeaderText="Firma">
                        <ItemTemplate>
                            <a href='<%# Page.ResolveUrl("~/Binaries/ElementPreview.aspx?id=" & Eval("id_ELEME_FIRMA"))%>' target="_blank">
                                <img width="70" height="70" border="0" src='<%# Page.ResolveUrl("~/Binaries/BOThumbnail.aspx?id=" & Eval("id_ELEME_FIRMA"))%>' />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsRAPPRLEGALI_PROVIDERECM_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_RAPPRLEGALI_PROVIDERECM WHERE id_RAPPRLEGALE_PROVIDERECM=@id_RAPPRLEGALE_PROVIDERECM"
                SelectCommand="SELECT * FROM vw_age_RAPPRLEGALI_PROVIDERECM_grid WHERE id_PROVIDERECM=@id_PROVIDERECM ORDER BY id_RAPPRLEGALE_PROVIDERECM">
                <SelectParameters>
                    <asp:Parameter Name="id_PROVIDERECM" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_RAPPRLEGALE_PROVIDERECM" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRAPPRLEGALI_PROVIDERECM_f" runat="server" Width="480px" Height="165px" Top="406px" Left="750px">
        <ContentTemplate>
            <stl:StlFormView ID="frmRAPPRLEGALI_PROVIDERECM" runat="server" DataKeyNames="id_RAPPRLEGALE_PROVIDERECM"
                DataSourceID="sdsRAPPRLEGALI_PROVIDERECM_f" NewItemText="" BoundStlGridViewID="grdRAPPRLEGALI_PROVIDERECM">
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
                    <span class="flbl">Qualifica su attestato ECM, senza articolo
                        <asp:Image runat="server" ID="Image16" ToolTip="Qualifica che comparirà sugli attestati, senza articolo (esempio: Rappresentante Legale del Provider)"
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
            <stl:StlSqlDataSource ID="sdsRAPPRLEGALI_PROVIDERECM_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_RAPPRLEGALI_PROVIDERECM (
	id_PROVIDERECM,
	ac_TITOLO,
	tx_COGNOME,
	tx_NOME,
	tx_QUALIFICA,
	id_ELEME_FIRMA,
	dt_CREAZIONE,
	tx_CREAZIONE
) VALUES (
	@id_PROVIDERECM,
	@ac_TITOLO,
	@tx_COGNOME,
	@tx_NOME,
	@tx_QUALIFICA,
	@id_ELEME_FIRMA,
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE id_UTENT=@id_UTENT)
);
SELECT @id_RAPPRLEGALE_PROVIDERECM = SCOPE_IDENTITY()               
                "
                SelectCommand="SELECT * FROM age_RAPPRLEGALI_PROVIDERECM WHERE id_RAPPRLEGALE_PROVIDERECM=@id_RAPPRLEGALE_PROVIDERECM"
                UpdateCommand="

UPDATE  age_RAPPRLEGALI_PROVIDERECM
SET     ac_TITOLO = @ac_TITOLO,
	    tx_COGNOME = @tx_COGNOME,
	    tx_NOME = @tx_NOME,
	    tx_QUALIFICA = @tx_QUALIFICA,
	    id_ELEME_FIRMA = @id_ELEME_FIRMA,
	    dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE id_UTENT=@id_UTENT)
WHERE   id_RAPPRLEGALE_PROVIDERECM = @id_RAPPRLEGALE_PROVIDERECM
                ">
                <UpdateParameters>
	                <asp:Parameter Name="ac_TITOLO" Type="String" />
	                <asp:Parameter Name="tx_COGNOME" Type="String" />
	                <asp:Parameter Name="tx_NOME" Type="String" />
	                <asp:Parameter Name="tx_QUALIFICA" Type="String" />
	                <asp:Parameter Name="id_ELEME_FIRMA" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_RAPPRLEGALE_PROVIDERECM" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_RAPPRLEGALE_PROVIDERECM" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_PROVIDERECM" Type="Int32" />
	                <asp:Parameter Name="ac_TITOLO" Type="String" />
	                <asp:Parameter Name="tx_COGNOME" Type="String" />
	                <asp:Parameter Name="tx_NOME" Type="String" />
	                <asp:Parameter Name="tx_QUALIFICA" Type="String" />
	                <asp:Parameter Name="id_ELEME_FIRMA" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_RAPPRLEGALE_PROVIDERECM" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <asp:SqlDataSource ID="sdsac_NORMATIVAECM" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_NORMATIVAECM, tx_NORMATIVAECM FROM age_NORMATIVEECM ORDER BY tx_NORMATIVAECM">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsac_TITOLO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TITOLO, tx_TITOLO FROM age_TITOLI ORDER BY tx_TITOLO">
    </asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
