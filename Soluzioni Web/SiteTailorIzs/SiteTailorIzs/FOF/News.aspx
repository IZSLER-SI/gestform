<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="News.aspx.vb" Inherits="Softailor.SiteTailorIzs.News" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updNEWS_g" runat="server" Width="970px" Height="500px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdNEWS" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_NEWS" DataSourceID="sdsNEWS_g"
                EnableViewState="False" ItemDescriptionPlural="news" ItemDescriptionSingular="news"
                Title="News sito pubblico" BoundStlFormViewID="frmNEWS"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="tx_DESCRIZIONE" HeaderText="Titolo" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Vis.dal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="al" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsNEWS_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM fof_NEWS WHERE id_NEWS = @id_NEWS"
                SelectCommand="SELECT * FROM fof_NEWS ORDER BY dt_CREAZIONE, id_NEWS">
                <DeleteParameters>
                    <asp:Parameter Name="id_NEWS" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updNEWS_f" runat="server" Width="970px" Top="510px" Left="0px" Height="178px">
        <ContentTemplate>
            <stl:StlFormView ID="frmNEWS" runat="server" DataKeyNames="id_NEWS"
                DataSourceID="sdsNEWS_f" NewItemText="" BoundStlGridViewID="grdNEWS">
                <EditItemTemplate>
                    <span class="flbl" style="width:72px;">Titolo</span>
                    <asp:TextBox ID="tx_DESCRIZIONETextBox" runat="server" Text='<%# Bind("tx_DESCRIZIONE")%>'
                                            Width="605px" />
                    <span class="slbl" style="width:83px;">Visualizza dal</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server" Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>'
                                            Width="80px" CssClass="stl_dt_data_ddmmyyyy" />

                    <span class="slbl" style="width:23px;">al</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server" Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>'
                                            Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <br />
                    <span class="flbl" style="width:72px;vertical-align:top;">Testo</span>
                    <stl:HtmlEditor ID="ht_NOTEHtmlEditor" runat="server" Width="881px" Height="123px"
                                                    ToolbarSet="Minimal" Value='<%# Bind("ht_NOTE") %>' 
                                                    FieldName="ht_NOTE" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsNEWS_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO fof_NEWS (
	dt_CREAZIONE,
	tx_CREAZIONE,
	tx_DESCRIZIONE,
    dt_INIZIO,
    dt_FINE,
	ht_NOTE
) VALUES (
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
	@tx_DESCRIZIONE,
    @dt_INIZIO,
    @dt_FINE,
	@ht_NOTE
)
SET @id_NEWS = SCOPE_IDENTITY()         
                "
                SelectCommand="
SELECT	id_NEWS,
		dt_CREAZIONE,
		tx_CREAZIONE,
		dt_MODIFICA,
		tx_MODIFICA,
		tx_DESCRIZIONE,
		ht_NOTE,
		CAST(dt_INIZIO as datetime) as dt_INIZIO,
		CAST(dt_FINE as datetime) as dt_FINE
FROM	fof_NEWS
WHERE   id_NEWS = @id_NEWS
                "
                UpdateCommand="
UPDATE  fof_NEWS
SET     dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT),
	    tx_DESCRIZIONE = @tx_DESCRIZIONE,
        dt_INIZIO = @dt_INIZIO,
        dt_FINE = @dt_FINE,
	    ht_NOTE = @ht_NOTE
WHERE   id_NEWS = @id_NEWS
                ">
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
	                <asp:Parameter Name="ht_NOTE" Type="String" />
                    <asp:Parameter Name="id_NEWS" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
	                <asp:Parameter Name="ht_NOTE" Type="String" />
                    <asp:Parameter Name="id_NEWS" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_NEWS" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
