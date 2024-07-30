<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SpeseRicaviEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.SpeseRicaviEvento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updCOSTIRICAVI_EVENTO_g" runat="server" Width="1162px" Height="459px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCOSTIRICAVI_EVENTO" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_COSTORICAVO_EVENTO" DataSourceID="sdsCOSTIRICAVI_EVENTO_g"
                EnableViewState="False" ItemDescriptionPlural="voci" ItemDescriptionSingular="voce"
                Title="Voci di spesa e di ricavo" BoundStlFormViewID="frmCOSTIRICAVI_EVENTO"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_VOCESPESARICAVO" HeaderText="Tipologia" />
                    <asp:BoundField DataField="ac_PROTOCOLLO" HeaderText="N° Prot." />
                    <asp:BoundField DataField="dt_PROTOCOLLO" HeaderText="Data Prot." DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="tx_DESCRIZIONE" HeaderText="Descrizione/Note" />
                    <asp:BoundField DataField="tx_RAGIONESOCIALE" HeaderText="Nome Azienda" />
                    <asp:BoundField DataField="ac_CODICEFISCALE" HeaderText="C.F. / P.I." />
                    <asp:BoundField DataField="tx_DELIBERA" HeaderText="Delibera" />
                    <asp:BoundField DataField="mo_IMPORTOPREVISTO" HeaderText="Importo Previsto" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="mo_CONSUNTIVO" HeaderText="Importo a consuntivo" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="dt_PAGAMENTO" HeaderText="Data Pag" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="tx_TIPOPAG" HeaderText="Modalità Pagamento" />
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCOSTIRICAVI_EVENTO_f" runat="server" Top="466px" Left="0px" Width="1162px" Height="134px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmCOSTIRICAVI_EVENTO" DataSourceID="sdsCOSTIRICAVI_EVENTO_f"
                DataKeyNames="id_COSTORICAVO_EVENTO" BoundStlGridViewID="grdCOSTIRICAVI_EVENTO">
                <EditItemTemplate>
                    <span class="flbl" style="width: 100px">Tipologia</span>
                    <asp:DropDownList ID="ac_VOCESPESARICAVODropDownList" runat="server" SelectedValue='<%# Bind("ac_VOCESPESARICAVO")%>'
                        DataSourceID="sdsac_VOCESPESARICAVO" DataTextField="tx_VOCESPESARICAVO" DataValueField="ac_VOCESPESARICAVO" Width="210px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 50px">N° Prot.</span>
                    <asp:TextBox ID="ac_PROTOCOLLOTextBox" runat="server"
                        Text='<%# Bind("ac_PROTOCOLLO")%>' Width="60px" />
                    <span class="slbl" style="width: 65px">Data Prot.</span>
                    <asp:TextBox ID="dt_PROTOCOLLOTextBox" runat="server"
                        Text='<%# Bind("dt_PROTOCOLLO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width: 100px">Descrizione/note</span>
                    <asp:TextBox ID="tx_DESCRIZIONETextBox" runat="server"
                        Text='<%# Bind("tx_DESCRIZIONE")%>' Width="472px" />
                    <br />
                    <span class="flbl" style="width: 100px">Nome Azienda</span>
                    <asp:TextBox ID="tx_RAGIONESOCIALETextBox" runat="server"
                        Text='<%# Bind("tx_RAGIONESOCIALE")%>' Width="826px" />
                    <span class="slbl" style="width: 65px">C.F. / P.I.</span>
                    <asp:TextBox ID="ac_CODICEFISCALETextBox" runat="server"
                        Text='<%# Bind("ac_CODICEFISCALE")%>' Width="150px" />
                    <br />
                    <span class="flbl" style="width: 100px">Delibera</span>
                    <asp:DropDownList ID="id_DELIBERADropDownList" runat="server" SelectedValue='<%# Bind("id_DELIBERA")%>'
                        DataSourceID="sdsid_DELIBERA" DataTextField="tx_DELIBERA" DataValueField="id_DELIBERA" Width="1049px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 100px">Importo Previsto €</span>
                    <asp:TextBox ID="mo_IMPORTOPREVISTOTextBox" runat="server" Width="80px" Text='<%# Bind("mo_IMPORTOPREVISTO", "{0:#0.00}")%>' />
                    <span class="slbl" style="width: 80px">Consuntivo €</span>
                    <asp:TextBox ID="mo_CONSUNTIVOTextBox" runat="server" Width="80px" Text='<%# Bind("mo_CONSUNTIVO", "{0:#0.00}")%>' />
                    <span class="slbl" style="width: 98px">Data Pagamento</span>
                    <asp:TextBox ID="dt_PAGAMENTOTextBox" runat="server"
                        Text='<%# Bind("dt_PAGAMENTO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width: 113px">Modalità Pagamento</span>
                    <asp:DropDownList ID="ac_TIPOPAGDropDownList" runat="server" SelectedValue='<%# Bind("ac_TIPOPAG")%>'
                        DataSourceID="sdsac_TIPOPAG" DataTextField="tx_TIPOPAG" DataValueField="ac_TIPOPAG" Width="506px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />

                    <span class="flbl" style="width: 100px">Numero Ordine</span>
                    <asp:TextBox ID="ac_ORDINETextBox" runat="server"
                        Text='<%# Bind("ac_ORDINE")%>' Width="100px" />
                    <span class="slbl" style="width: 77px">Data Ordine</span>
                    <asp:TextBox ID="dt_ORDINETextBox" runat="server"
                        Text='<%# Bind("dt_ORDINE", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                
                    <span class="flbl" style="width: 40px">&nbsp;</span>
                    
                    <span class="slbl" style="width: 82px">Numero Bolla</span>
                    <asp:TextBox ID="ac_DDTTextBox" runat="server"
                        Text='<%# Bind("ac_DDT")%>' Width="100px" />
                    <span class="slbl" style="width: 68px">Data Bolla</span>
                    <asp:TextBox ID="dt_DDTTextBox" runat="server"
                        Text='<%# Bind("dt_DDT", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />

                    <span class="flbl" style="width: 41px">&nbsp;</span>

                    <span class="slbl" style="width: 97px">Numero Fattura</span>
                    <asp:TextBox ID="ac_FATTURATextBox" runat="server"
                        Text='<%# Bind("ac_FATTURA")%>' Width="100px" />
                    <span class="slbl" style="width: 80px">Data Fattura</span>
                    <asp:TextBox ID="dt_FATTURATextBox" runat="server"
                        Text='<%# Bind("dt_FATTURA", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                
                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlSqlDataSource ID="sdsCOSTIRICAVI_EVENTO_g" runat="server"
        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        DeleteCommand="DELETE FROM eve_COSTIRICAVI_EVENTO WHERE id_COSTORICAVO_EVENTO = @id_COSTORICAVO_EVENTO"
        SelectCommand="SELECT * FROM vw_eve_COSTIRICAVI_EVENTO_grid WHERE id_EVENTO = @id_EVENTO ORDER BY id_COSTORICAVO_EVENTO">
        <DeleteParameters>
            <asp:Parameter Name="id_COSTORICAVO_EVENTO" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsCOSTIRICAVI_EVENTO_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM eve_COSTIRICAVI_EVENTO WHERE id_COSTORICAVO_EVENTO=@id_COSTORICAVO_EVENTO"
        InsertCommand="
INSERT INTO eve_COSTIRICAVI_EVENTO (
	dt_CREAZIONE,
	tx_CREAZIONE,
	id_EVENTO,
	id_DELIBERA,
	ac_VOCESPESARICAVO,
	ac_TIPOPAG,
	ac_SPESARICAVO,
    ac_PROTOCOLLO,
    dt_PROTOCOLLO,
	tx_DESCRIZIONE,
	mo_IMPORTOPREVISTO,
	mo_CONSUNTIVO,
	dt_PAGAMENTO,
    tx_RAGIONESOCIALE,
    ac_CODICEFISCALE,
	ac_ORDINE,
	dt_ORDINE,
	ac_DDT,
	dt_DDT,
	ac_FATTURA,
	dt_FATTURA
) VALUES (
	GETDATE(),
	@tx_CREAZIONE,
	@id_EVENTO,
	@id_DELIBERA,
	@ac_VOCESPESARICAVO,
	@ac_TIPOPAG,
	(SELECT ac_SPESARICAVO FROM age_VOCISPESARICAVO WHERE ac_VOCESPESARICAVO=@ac_VOCESPESARICAVO),
	@ac_PROTOCOLLO,
    @dt_PROTOCOLLO,
    @tx_DESCRIZIONE,
	@mo_IMPORTOPREVISTO,
	@mo_CONSUNTIVO,
	@dt_PAGAMENTO,
    @tx_RAGIONESOCIALE,
    @ac_CODICEFISCALE,
	@ac_ORDINE,
	@dt_ORDINE,
	@ac_DDT,
	@dt_DDT,
	@ac_FATTURA,
	@dt_FATTURA
)
SELECT @id_COSTORICAVO_EVENTO = SCOPE_IDENTITY()
        "
        UpdateCommand="
UPDATE  eve_COSTIRICAVI_EVENTO
SET     dt_MODIFICA = GETDATE(),
	    tx_MODIFICA = @tx_MODIFICA,
	    id_DELIBERA = @id_DELIBERA,
	    ac_VOCESPESARICAVO = @ac_VOCESPESARICAVO,
	    ac_TIPOPAG = @ac_TIPOPAG,
        ac_SPESARICAVO = (SELECT ac_SPESARICAVO FROM age_VOCISPESARICAVO WHERE ac_VOCESPESARICAVO=@ac_VOCESPESARICAVO),
	    ac_PROTOCOLLO = @ac_PROTOCOLLO,
        dt_PROTOCOLLO = @dt_PROTOCOLLO,
        tx_DESCRIZIONE = @tx_DESCRIZIONE,
	    mo_IMPORTOPREVISTO = @mo_IMPORTOPREVISTO,
	    mo_CONSUNTIVO = @mo_CONSUNTIVO,
	    dt_PAGAMENTO = @dt_PAGAMENTO,
        tx_RAGIONESOCIALE = @tx_RAGIONESOCIALE,
        ac_CODICEFISCALE = @ac_CODICEFISCALE,
        ac_ORDINE = @ac_ORDINE,
	    dt_ORDINE = @dt_ORDINE,
	    ac_DDT = @ac_DDT,
	    dt_DDT = @dt_DDT,
	    ac_FATTURA = @ac_FATTURA,
	    dt_FATTURA = @dt_FATTURA
WHERE   id_COSTORICAVO_EVENTO = @id_COSTORICAVO_EVENTO
        ">
        <SelectParameters>
            <asp:Parameter Name="id_COSTORICAVO_EVENTO" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="tx_CREAZIONE" Type="String" />
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="id_DELIBERA" Type="Int32" />
            <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
            <asp:Parameter Name="ac_TIPOPAG" Type="String" />
            <asp:Parameter Name="ac_PROTOCOLLO" Type="String" />
            <asp:Parameter Name="dt_PROTOCOLLO" Type="DateTime" />
            <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
            <asp:Parameter Name="mo_IMPORTOPREVISTO" Type="Decimal" />
            <asp:Parameter Name="mo_CONSUNTIVO" Type="Decimal" />
            <asp:Parameter Name="dt_PAGAMENTO" Type="DateTime" />
            <asp:Parameter Name="tx_RAGIONESOCIALE" Type="String" />
            <asp:Parameter Name="ac_CODICEFISCALE" Type="String" />
            <asp:Parameter Name="ac_ORDINE" Type="String" />
	        <asp:Parameter Name="dt_ORDINE" Type="DateTime" />
	        <asp:Parameter Name="ac_DDT" Type="String" />
	        <asp:Parameter Name="dt_DDT" Type="DateTime" />
	        <asp:Parameter Name="ac_FATTURA" Type="String" />
	        <asp:Parameter Name="dt_FATTURA" Type="DateTime" />
            <asp:Parameter Name="id_COSTORICAVO_EVENTO" Type="Int32" Direction="Output" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="id_DELIBERA" Type="Int32" />
            <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
            <asp:Parameter Name="ac_TIPOPAG" Type="String" />
            <asp:Parameter Name="ac_PROTOCOLLO" Type="String" />
            <asp:Parameter Name="dt_PROTOCOLLO" Type="DateTime" />
            <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
            <asp:Parameter Name="mo_IMPORTOPREVISTO" Type="Decimal" />
            <asp:Parameter Name="mo_CONSUNTIVO" Type="Decimal" />
            <asp:Parameter Name="dt_PAGAMENTO" Type="DateTime" />
            <asp:Parameter Name="tx_RAGIONESOCIALE" Type="String" />
            <asp:Parameter Name="ac_CODICEFISCALE" Type="String" />
            <asp:Parameter Name="ac_ORDINE" Type="String" />
	        <asp:Parameter Name="dt_ORDINE" Type="DateTime" />
	        <asp:Parameter Name="ac_DDT" Type="String" />
	        <asp:Parameter Name="dt_DDT" Type="DateTime" />
	        <asp:Parameter Name="ac_FATTURA" Type="String" />
	        <asp:Parameter Name="dt_FATTURA" Type="DateTime" />
            <asp:Parameter Name="id_COSTORICAVO_EVENTO" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>
    <asp:SqlDataSource ID="sdsac_TIPOPAG" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPOPAG, tx_TIPOPAG FROM age_TIPIPAG ORDER BY tx_TIPOPAG">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_DELIBERA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
SELECT		D.id_DELIBERA,
			D.tx_DELIBERA + ' (' + CONVERT(nvarchar(10), D.dt_DATA, 103) + ')' as tx_DELIBERA
FROM		age_DELIBERE D
			INNER JOIN age_TIPOLOGIEDELIBERE TD ON D.ac_TIPOLOGIADELIBERA = TD.ac_TIPOLOGIADELIBERA 
WHERE		TD.fl_SPESEEVENTO = 1
ORDER BY	D.dt_DATA desc
        ">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_VOCESPESARICAVO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_VOCESPESARICAVO, tx_VOCESPESARICAVO + CASE ac_SPESARICAVO WHEN 'S' THEN ' (spesa)' WHEN 'R' THEN ' (ricavo)' END as tx_VOCESPESARICAVO FROM age_VOCISPESARICAVO WHERE fl_EVENTO=1 ORDER BY tx_VOCESPESARICAVO">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
