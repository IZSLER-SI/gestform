<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="FilesEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.FilesEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updFILES_g" runat="server" Width="970px" Height="500px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdFILES" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_FILE_EVENTO" DataSourceID="sdsFILES_g"
                EnableViewState="False" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                Title="Materiale evento per sito pubblico" BoundStlFormViewID="frmFILES"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_SEZIONEDOCEVENTO_PLUR" HeaderText="Sezione" />
                    <asp:BoundField DataField="ni_ORDINE" HeaderText="Ordine" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="tx_DESCRIZIONE" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_VISIBILITA" HeaderText="Visibilità" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Vis.dal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="al" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="File">
                        <ItemTemplate>
                            <a target="_blank" href="../../Binaries/ElementPreview.aspx?id=<%#Eval("id_ELEME")%>">
                                <img style="width:70px;height:70px;border-style:none;"src="../../Binaries/BOThumbnail.aspx?id=<%#Eval("id_ELEME")%>" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFILES_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM fof_FILES_EVENTI WHERE id_FILE_EVENTO = @id_FILE_EVENTO"
                SelectCommand="SELECT * FROM vw_fof_FILES_EVENTI_grid WHERE id_EVENTO=@id_EVENTO ORDER BY ni_ORDINE_SEZ, ni_ORDINE, id_FILE_EVENTO">
                <SelectParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_FILE_EVENTO" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updFILES_f" runat="server" Width="970px" Top="510px" Left="0px" Height="200px">
        <ContentTemplate>
            <stl:StlFormView ID="frmFILES" runat="server" DataKeyNames="id_FILE_EVENTO"
                DataSourceID="sdsFILES_f" NewItemText="" BoundStlGridViewID="grdFILES">
                <EditItemTemplate>
                    <span class="flbl" style="width:51px;">Sezione</span>
                    <asp:DropDownList ID="ac_SEZIONEDOCEVENTODropDownList" runat="server" SelectedValue='<%# Bind("ac_SEZIONEDOCEVENTO")%>'
                        DataSourceID="sdsac_SEZIONEDOCEVENTO" DataTextField="tx_SEZIONEDOCEVENTO_PLUR" DataValueField="ac_SEZIONEDOCEVENTO"
                        Width="180px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width:52px;">Ordine</span>
                    <asp:TextBox ID="ni_ORDINETextBox" runat="server" Text='<%# Bind("ni_ORDINE")%>'
                                            Width="40px" />

                    <span class="slbl" style="width:72px;">Descrizione</span>
                    <asp:TextBox ID="tx_DESCRIZIONETextBox" runat="server" Text='<%# Bind("tx_DESCRIZIONE")%>'
                                            Width="552px" />
                    <br />
                    <span class="flbl" style="width:51px;">Visibilità</span>
                    <asp:DropDownList ID="ac_VISIBILITADropDownList" runat="server" SelectedValue='<%# Bind("ac_VISIBILITA")%>'
                        DataSourceID="sdsac_VISIBILITA" DataTextField="tx_VISIBILITA" DataValueField="ac_VISIBILITA"
                        Width="240px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width:83px;">Visualizza dal</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server" Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>'
                                            Width="80px" CssClass="stl_dt_data_ddmmyyyy" />

                    <span class="slbl" style="width:23px;">al</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server" Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>'
                                            Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width:93px;">Link per e-mail</span>
                    <asp:TextBox ID="tx_URLTextBox" Readonly="true" runat="server" Text='<%# Eval("tx_URL")%>'
                                            Width="293px" />
                    <br />
                    <div style="float:left;margin-right:10px;">
                        <span class="flbl">File/elemento</span><br />
                        <stl:BinaryElementBox ID="id_ELEMEBinaryElementBox" runat="server" FieldName="id_ELEME"
                        Value='<%# Bind("id_ELEME")%>' DefaultCODCATEG="DOC_EVE" 
                        DefaultDescriptionSourceTextBoxID="tx_DESCRIZIONETextBox" DefaultDescriptionPreamble='<%# GetFilePreamble()%>' />
                    </div>
                    <div style="float:left">
                        <span class="flbl">Note/testo aggiuntivo</span><br />
                        <stl:HtmlEditor ID="ht_NOTEHtmlEditor" runat="server" Width="693px" Height="105px"
                                                        ToolbarSet="Minimal" Value='<%# Bind("ht_NOTE") %>' 
                                                        FieldName="ht_NOTE" />

                    </div>
                    <div style="clear:both;">
                    </div>
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsFILES_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO fof_FILES_EVENTI (
	dt_CREAZIONE,
	tx_CREAZIONE,
	id_EVENTO,
	id_ELEME,
	ac_SEZIONEDOCEVENTO,
	ni_ORDINE,
	tx_DESCRIZIONE,
	ac_VISIBILITA,
    dt_INIZIO,
    dt_FINE,
	ht_NOTE
) VALUES (
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
	@id_EVENTO,
	@id_ELEME,
	@ac_SEZIONEDOCEVENTO,
	@ni_ORDINE,
	@tx_DESCRIZIONE,
	@ac_VISIBILITA,
    @dt_INIZIO,
    @dt_FINE,
	@ht_NOTE
)
SET @id_FILE_EVENTO = SCOPE_IDENTITY()         
                "
                SelectCommand="
SELECT	FEV.id_FILE_EVENTO,
		FEV.dt_CREAZIONE,
		FEV.tx_CREAZIONE,
		FEV.dt_MODIFICA,
		FEV.tx_MODIFICA,
		FEV.id_EVENTO,
		FEV.id_ELEME,
		FEV.ac_SEZIONEDOCEVENTO,
		FEV.ni_ORDINE,
		FEV.tx_DESCRIZIONE,
		FEV.ac_VISIBILITA,
		FEV.ht_NOTE,
		CAST(FEV.dt_INIZIO as datetime) as dt_INIZIO,
		CAST(FEV.dt_FINE as datetime) as dt_FINE,
        @tx_BASEURL + CAST(FEV.id_FILE_EVENTO as nvarchar(10)) + F.EXTE_GET as tx_URL
FROM	fof_FILES_EVENTI FEV
        LEFT OUTER JOIN bd_ELEMEN E WITH(NOLOCK) ON FEV.id_ELEME = E.id_ELEME
	    LEFT OUTER JOIN bd_CATEGO C WITH(NOLOCK) ON E.ID_CATEG = C.ID_CATEG
	    LEFT OUTER JOIN bd_FORMAT F WITH(NOLOCK) ON E.CODFORMA = F.CODFORMA
WHERE   FEV.id_FILE_EVENTO = @id_FILE_EVENTO
                "
                UpdateCommand="
UPDATE  fof_FILES_EVENTI
SET     dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
        id_ELEME = @id_ELEME,
	    ac_SEZIONEDOCEVENTO = @ac_SEZIONEDOCEVENTO,
	    ni_ORDINE = @ni_ORDINE,
	    tx_DESCRIZIONE = @tx_DESCRIZIONE,
	    ac_VISIBILITA = @ac_VISIBILITA,
        dt_INIZIO = @dt_INIZIO,
        dt_FINE = @dt_FINE,
	    ht_NOTE = @ht_NOTE
WHERE   id_FILE_EVENTO=@id_FILE_EVENTO
                ">
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
	                <asp:Parameter Name="id_ELEME" Type="Int32" />
	                <asp:Parameter Name="ac_SEZIONEDOCEVENTO" Type="String" />
	                <asp:Parameter Name="ni_ORDINE" Type="Int32" />
	                <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
	                <asp:Parameter Name="ac_VISIBILITA" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
	                <asp:Parameter Name="ht_NOTE" Type="String" />
                    <asp:Parameter Name="id_FILE_EVENTO" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_ELEME" Type="Int32" />
	                <asp:Parameter Name="ac_SEZIONEDOCEVENTO" Type="String" />
	                <asp:Parameter Name="ni_ORDINE" Type="Int32" />
	                <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
	                <asp:Parameter Name="ac_VISIBILITA" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
	                <asp:Parameter Name="ht_NOTE" Type="String" />
                    <asp:Parameter Name="id_FILE_EVENTO" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_FILE_EVENTO" Type="Int32" />
                    <asp:Parameter Name="tx_BASEURL" Type="String" />
                </SelectParameters>
                
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:SqlDataSource ID="sdsac_SEZIONEDOCEVENTO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_SEZIONEDOCEVENTO, tx_SEZIONEDOCEVENTO_PLUR FROM fof_SEZIONIDOCEVENTI ORDER BY ni_ORDINE"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_VISIBILITA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_VISIBILITA, tx_VISIBILITA FROM fof_VISIBILITAFILES ORDER BY ni_ORDINE"></asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
