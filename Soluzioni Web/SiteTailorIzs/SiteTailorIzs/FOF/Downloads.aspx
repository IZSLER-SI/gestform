<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="Downloads.aspx.vb" Inherits="Softailor.SiteTailorIzs.Downloads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updFILES_g" runat="server" Width="970px" Height="500px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdFILES" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_FILE" DataSourceID="sdsFILES_g"
                EnableViewState="False" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                Title="Download sito pubblico" BoundStlFormViewID="frmFILES"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_DESCRIZIONE" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Vis.dal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="al" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="File">
                        <ItemTemplate>
                            <a href='<%# Page.ResolveUrl("~/Binaries/ElementPreview.aspx?id=" & Eval("id_ELEME"))%>' target="_blank">
                                <img width="70" height="70" border="0" src='<%# Page.ResolveUrl("~/Binaries/BOThumbnail.aspx?id=" & Eval("id_ELEME"))%>' />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFILES_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM fof_FILES WHERE id_FILE = @id_FILE"
                SelectCommand="SELECT * FROM fof_FILES ORDER BY tx_DESCRIZIONE, id_FILE">
                <DeleteParameters>
                    <asp:Parameter Name="id_FILE" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updFILES_f" runat="server" Width="970px" Top="510px" Left="0px" Height="178px">
        <ContentTemplate>
            <stl:StlFormView ID="frmFILES" runat="server" DataKeyNames="id_FILE"
                DataSourceID="sdsFILES_f" NewItemText="" BoundStlGridViewID="grdFILES">
                <EditItemTemplate>
                    <span class="flbl" style="width:72px;">Descrizione</span>
                    <asp:TextBox ID="tx_DESCRIZIONETextBox" runat="server" Text='<%# Bind("tx_DESCRIZIONE")%>'
                                            Width="605px" />
                    <span class="slbl" style="width:83px;">Visualizza dal</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server" Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>'
                                            Width="80px" CssClass="stl_dt_data_ddmmyyyy" />

                    <span class="slbl" style="width:23px;">al</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server" Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>'
                                            Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <br />
                    <div style="float:left;margin-right:10px;">
                        <span class="flbl">File/elemento</span><br />
                        <stl:BinaryElementBox ID="id_ELEMEBinaryElementBox" runat="server" FieldName="id_ELEME"
                        Value='<%# Bind("id_ELEME")%>' DefaultCODCATEG="DOC_GEN" 
                        DefaultDescriptionSourceTextBoxID="tx_DESCRIZIONETextBox" />
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
INSERT INTO fof_FILES (
	dt_CREAZIONE,
	tx_CREAZIONE,
	id_ELEME,
	tx_DESCRIZIONE,
    dt_INIZIO,
    dt_FINE,
	ht_NOTE
) VALUES (
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
	@id_ELEME,
	@tx_DESCRIZIONE,
    @dt_INIZIO,
    @dt_FINE,
	@ht_NOTE
)
SET @id_FILE = SCOPE_IDENTITY()         
                "
                SelectCommand="
SELECT	id_FILE,
		dt_CREAZIONE,
		tx_CREAZIONE,
		dt_MODIFICA,
		tx_MODIFICA,
		id_ELEME,
		tx_DESCRIZIONE,
		ht_NOTE,
		CAST(dt_INIZIO as datetime) as dt_INIZIO,
		CAST(dt_FINE as datetime) as dt_FINE
FROM	fof_FILES
WHERE   id_FILE = @id_FILE
                "
                UpdateCommand="
UPDATE  fof_FILES
SET     dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
        id_ELEME = @id_ELEME,
	    tx_DESCRIZIONE = @tx_DESCRIZIONE,
        dt_INIZIO = @dt_INIZIO,
        dt_FINE = @dt_FINE,
	    ht_NOTE = @ht_NOTE
WHERE   id_FILE=@id_FILE
                ">
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_ELEME" Type="Int32" />
	                <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
	                <asp:Parameter Name="ht_NOTE" Type="String" />
                    <asp:Parameter Name="id_FILE" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_ELEME" Type="Int32" />
	                <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
	                <asp:Parameter Name="ht_NOTE" Type="String" />
                    <asp:Parameter Name="id_FILE" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_FILE" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
