<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="PianiFormativi.aspx.vb" Inherits="Softailor.SiteTailorIzs.PianiFormativi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updPIANIFORMATIVI_g" runat="server" Width="600px" Height="230px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdPIANIFORMATIVI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_PIANOFORMATIVO" DataSourceID="sdsPIANIFORMATIVI_g"
                EnableViewState="False" ItemDescriptionPlural="piani" ItemDescriptionSingular="piano"
                Title="Piani Formativi" BoundStlFormViewID="frmPIANIFORMATIVI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_PIANOFORMATIVO" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_DELIBERA" HeaderText="Delibera di pertinenza" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Valido dal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="al" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Utilizzato" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATO"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsPIANIFORMATIVI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_PIANIFORMATIVI WHERE id_PIANOFORMATIVO = @id_PIANOFORMATIVO"
                SelectCommand="SELECT * FROM vw_age_PIANIFORMATIVI_grid ORDER BY dt_INIZIO desc, id_PIANOFORMATIVO">
                <DeleteParameters>
                    <asp:Parameter Name="id_PIANOFORMATIVO" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPIANIFORMATIVI_f" runat="server" Width="620px" Height="230px" Top="0px" Left="610px">
        <ContentTemplate>
            <stl:StlFormView ID="frmPIANIFORMATIVI" runat="server" DataKeyNames="id_PIANOFORMATIVO"
                DataSourceID="sdsPIANIFORMATIVI_f" NewItemText="" BoundStlGridViewID="grdPIANIFORMATIVI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 150px;">Descrizione Piano Formativo</span>
                    <asp:TextBox ID="tx_PIANOFORMATIVOTextBox" runat="server"
                        Text='<%# Bind("tx_PIANOFORMATIVO")%>' Width="453px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 150px;">Validità temporale: dal</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server"
                        Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width: 25px;">al</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server"
                        Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <br />
                    <span class="flbl" style="width: 150px;">Delibera di pertinenza</span>
                    <asp:DropDownList ID="id_DELIBERADropDownList" runat="server" DataSourceID="sdsid_DELIBERA"
                        DataTextField="tx_DELIBERA" DataValueField="id_DELIBERA" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("id_DELIBERA")%>' Width="457px">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 150px;">Descrizione estesa / note</span>
                    <asp:TextBox ID="tx_PIANOFORMATIVO_LUNGOTextBox" runat="server"
                        Text='<%# Bind("tx_PIANOFORMATIVO_LUNGO")%>' Width="453px" TextMode="MultiLine" Height="130px" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsPIANIFORMATIVI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_PIANIFORMATIVI (
	id_DELIBERA,
	tx_PIANOFORMATIVO,
	tx_PIANOFORMATIVO_LUNGO,
	dt_INIZIO,
	dt_FINE,
	dt_CREAZIONE,
	tx_CREAZIONE
) VALUES (
	@id_DELIBERA,
	@tx_PIANOFORMATIVO,
	@tx_PIANOFORMATIVO_LUNGO,
	@dt_INIZIO,
	@dt_FINE,
	GETDATE(),
	(SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
);
SET @id_PIANOFORMATIVO=SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM age_PIANIFORMATIVI WHERE id_PIANOFORMATIVO=@id_PIANOFORMATIVO"
                UpdateCommand="
UPDATE  age_PIANIFORMATIVI
SET     id_DELIBERA = @id_DELIBERA,
	    tx_PIANOFORMATIVO = @tx_PIANOFORMATIVO,
	    tx_PIANOFORMATIVO_LUNGO = @tx_PIANOFORMATIVO_LUNGO,
	    dt_INIZIO = @dt_INIZIO,
	    dt_FINE = @dt_FINE,
	    dt_MODIFICA = GETDATE(),
	    tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT) 
WHERE   id_PIANOFORMATIVO = @id_PIANOFORMATIVO
                ">
                <UpdateParameters>
                     <asp:Parameter Name="id_DELIBERA" Type="Int32" />
                    <asp:Parameter Name="tx_PIANOFORMATIVO" Type="String" />
                    <asp:Parameter Name="tx_PIANOFORMATIVO_LUNGO" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_PIANOFORMATIVO" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_PIANOFORMATIVO" Type="Int32" DefaultValue="" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_DELIBERA" Type="Int32" />
                    <asp:Parameter Name="tx_PIANOFORMATIVO" Type="String" />
                    <asp:Parameter Name="tx_PIANOFORMATIVO_LUNGO" Type="String" />
                    <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
                    <asp:Parameter Name="dt_FINE" Type="DateTime" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
	                <asp:Parameter Name="id_PIANOFORMATIVO" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updBUDGET_PIANIFORMATIVI_g" runat="server" Width="600px" Height="320px" Top="250px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdBUDGET_PIANIFORMATIVI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_BUDGET_PIANOFORMATIVO" DataSourceID="sdsBUDGET_PIANIFORMATIVI_g"
                EnableViewState="False" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                Title="Budget del piano formativo selezionato" BoundStlFormViewID="frmBUDGET_PIANIFORMATIVI" ParentStlGridViewID="grdPIANIFORMATIVI"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_BUDGET_PIANOFORMATIVO" HeaderText="Descrizione voce" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_PERSONA_RESPONSABILE" HeaderText="Responsabile" />
                    <asp:BoundField DataField="mo_FONDO" HeaderText="Fondo (€)" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsBUDGET_PIANIFORMATIVI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_BUDGET_PIANIFORMATIVI WHERE id_BUDGET_PIANOFORMATIVO = @id_BUDGET_PIANOFORMATIVO"
                SelectCommand="SELECT * FROM vw_age_BUDGET_PIANIFORMATIVI_grid WHERE id_PIANOFORMATIVO = @id_PIANOFORMATIVO ORDER BY id_BUDGET_PIANOFORMATIVO">
                <DeleteParameters>
                    <asp:Parameter Name="id_BUDGET_PIANOFORMATIVO" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_PIANOFORMATIVO" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updBUDGET_PIANIFORMATIVI_f" runat="server" Width="620px" Height="91px" Top="250px" Left="610px">
        <ContentTemplate>
            <stl:StlFormView ID="frmBUDGET_PIANIFORMATIVI" runat="server" DataKeyNames="id_BUDGET_PIANOFORMATIVO"
                DataSourceID="sdsBUDGET_PIANIFORMATIVI_f" NewItemText="" BoundStlGridViewID="grdBUDGET_PIANIFORMATIVI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 100px;">Descrizione voce</span>
                    <asp:TextBox ID="tx_BUDGET_PIANOFORMATIVOTextBox" runat="server"
                        Text='<%# Bind("tx_BUDGET_PIANOFORMATIVO")%>' Width="503px"  />
                    <br />
                    <span class="flbl" style="width: 100px;">Responsabile</span>
                    <bof:CtlSelettorePersonaGForm ID="id_PERSONA_RESPONSABILECtlSelettorePersonaGForm" runat="server"
                        Width="510px" FieldName="id_PERSONA_RESPONSABILE" Value='<%# Bind("id_PERSONA_RESPONSABILE")%>' SoloDipendenti="true" />
                    <br />
                    <span class="flbl" style="width: 100px;">Fondo (€)</span>
                    <asp:TextBox ID="mo_FONDOTextBox" runat="server"
                        Text='<%# Bind("mo_FONDO", "{0:#,##0.00}")%>' Width="100px" Style="padding-right:2px;text-align:right;" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsBUDGET_PIANIFORMATIVI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT * FROM age_BUDGET_PIANIFORMATIVI WHERE id_BUDGET_PIANOFORMATIVO=@id_BUDGET_PIANOFORMATIVO"
                InsertCommand="
INSERT INTO age_BUDGET_PIANIFORMATIVI (
    id_PIANOFORMATIVO,
    tx_BUDGET_PIANOFORMATIVO,
    id_PERSONA_RESPONSABILE,
    mo_FONDO,
    dt_CREAZIONE,
    tx_CREAZIONE
) VALUES (
    @id_PIANOFORMATIVO,
    @tx_BUDGET_PIANOFORMATIVO,
    @id_PERSONA_RESPONSABILE,
    @mo_FONDO,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
)
SET @id_BUDGET_PIANOFORMATIVO = SCOPE_IDENTITY()
                "
                UpdateCommand="
UPDATE  age_BUDGET_PIANIFORMATIVI
SET     tx_BUDGET_PIANOFORMATIVO = @tx_BUDGET_PIANOFORMATIVO,
        id_PERSONA_RESPONSABILE = @id_PERSONA_RESPONSABILE,
        mo_FONDO = @mo_FONDO,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
WHERE   id_BUDGET_PIANOFORMATIVO = @id_BUDGET_PIANOFORMATIVO
                ">
                <InsertParameters>
                    <asp:Parameter Name="id_PIANOFORMATIVO" Type="Int32" />
                    <asp:Parameter Name="tx_BUDGET_PIANOFORMATIVO" Type="String" />
                    <asp:Parameter Name="id_PERSONA_RESPONSABILE" Type="Int32" />
                    <asp:Parameter Name="mo_FONDO" Type="Decimal" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_BUDGET_PIANOFORMATIVO" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="tx_BUDGET_PIANOFORMATIVO" Type="String" />
                    <asp:Parameter Name="id_PERSONA_RESPONSABILE" Type="Int32" />
                    <asp:Parameter Name="mo_FONDO" Type="Decimal" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="id_BUDGET_PIANOFORMATIVO" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_BUDGET_PIANOFORMATIVO" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <asp:SqlDataSource ID="sdsid_DELIBERA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
 SELECT
	D.id_DELIBERA,
	D.tx_DELIBERA + ' (' + D.ac_DELIBERA + ')' as tx_DELIBERA
FROM
	age_DELIBERE D
	INNER JOIN age_TIPOLOGIEDELIBERE TD ON D.ac_TIPOLOGIADELIBERA = TD.ac_TIPOLOGIADELIBERA 
WHERE
	TD.fl_PIANOFORMATIVO=1
ORDER BY
	D.dt_DATA
        ">
    </asp:SqlDataSource>


   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
