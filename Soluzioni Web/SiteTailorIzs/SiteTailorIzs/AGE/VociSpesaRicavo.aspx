<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="VociSpesaRicavo.aspx.vb" Inherits="Softailor.SiteTailorIzs.VociSpesaRicavo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updVOCISPESARICAVO_g" runat="server" Width="900px" Height="495px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdVOCISPESARICAVO" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_VOCESPESARICAVO" DataSourceID="sdsVOCISPESARICAVO_g"
                EnableViewState="False" ItemDescriptionPlural="tipologie" ItemDescriptionSingular="tipologia"
                Title="Tipologie Voci di Spesa e di Ricavo" BoundStlFormViewID="frmVOCISPESARICAVO"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_SPESARICAVO" HeaderText="Spesa/Ricavo" />
                    <asp:BoundField DataField="ac_VOCESPESARICAVO" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_VOCESPESARICAVO" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_VOCESPESARICAVO_erp" HeaderText="Codice ERP" />
                    <asp:TemplateField HeaderText="Su evento" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_EVENTO"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Su relatore" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_RELATORE"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsVOCISPESARICAVO_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_VOCISPESARICAVO WHERE ac_VOCESPESARICAVO = @ac_VOCESPESARICAVO"
                SelectCommand="SELECT * FROM vw_age_VOCISPESARICAVO_grid ORDER BY ac_SPESARICAVO, tx_VOCESPESARICAVO">
                <DeleteParameters>
                    <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updVOCISPESARICAVO_f" runat="server" Width="900px" Height="162px" Top="500px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmVOCISPESARICAVO" runat="server" DataKeyNames="ac_VOCESPESARICAVO"
                DataSourceID="sdsVOCISPESARICAVO_f" NewItemText="" BoundStlGridViewID="grdVOCISPESARICAVO">
                <EditItemTemplate>
                    <span class="flbl" style="width: 70px;">Spesa/Ricavo</span>
                    <asp:DropDownList ID="ac_SPESARICAVODropDownList" runat="server" 
                        SelectedValue='<%# Bind("ac_SPESARICAVO")%>' Width="124px">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Text="Spesa" Value="S" />
                        <asp:ListItem Text="Ricavo" Value="R" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 50px;">Codice</span>
                    <asp:TextBox ID="ac_VOCESPESARICAVOTextBox" runat="server"
                        Text='<%# Bind("ac_VOCESPESARICAVO")%>' Width="120px" />
                    <span class="slbl" style="width: 70px;">Descrizione</span>
                    <asp:TextBox ID="tx_VOCESPESARICAVOTextBox" runat="server"
                        Text='<%# Bind("tx_VOCESPESARICAVO")%>' Width="442px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 70px;">Codice ERP</span>
                    <asp:TextBox ID="ac_VOCESPESARICAVO_erpTextBox" runat="server"
                        Text='<%# Bind("ac_VOCESPESARICAVO_erp")%>' Width="120px" />
                    <span class="slbl" style="width: 60px;">Su evento</span>
                    <asp:CheckBox ID="fl_EVENTOCheckBox" runat="server"
                        Checked='<%# Bind("fl_EVENTO")%>' />
                    <span class="slbl" style="width: 64px;">Su relatore</span>
                    <asp:CheckBox ID="fl_RELATORECheckBox" runat="server"
                        Checked='<%# Bind("fl_RELATORE")%>' />
                    <br />
                    <asp:CheckBox ID="fl_AUTOADD_ECMCheckBox" runat="server"
                        Checked='<%# Bind("fl_AUTOADD_ECM")%>' />
                    <span class="flbl">Crea automaticamente la voce (a importo zero) quando viene creato un evento accreditato ECM</span>
                    <br />
                    <asp:CheckBox ID="fl_AUTOADD_NOECMCheckBox" runat="server"
                        Checked='<%# Bind("fl_AUTOADD_NOECM")%>' />
                    <span class="flbl">Crea automaticamente la voce (a importo zero) quando viene creato un evento non accreditato ECM</span>
                    <br />
                    <span class="flbl">Crea automaticamente la voce (a importo zero) quando viene aggiunto da backoffice:</span>
                    <br />
                    <span class="flbl">un&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ac_CATEGORIAECM_AUTOADD_1DropDownList" runat="server" SelectedValue='<%# Bind("ac_CATEGORIAECM_AUTOADD_1")%>' 
                        DataSourceID="sdsac_CATEGORIAECM" DataTextField="tx" DataValueField="ac" 
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl">oppure un&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ac_CATEGORIAECM_AUTOADD_2DropDownList" runat="server" SelectedValue='<%# Bind("ac_CATEGORIAECM_AUTOADD_2")%>' 
                        DataSourceID="sdsac_CATEGORIAECM" DataTextField="tx" DataValueField="ac" 
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl">oppure un&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ac_CATEGORIAECM_AUTOADD_3DropDownList" runat="server" SelectedValue='<%# Bind("ac_CATEGORIAECM_AUTOADD_3")%>' 
                        DataSourceID="sdsac_CATEGORIAECM" DataTextField="tx" DataValueField="ac" 
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>

                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsVOCISPESARICAVO_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_VOCISPESARICAVO (
    ac_VOCESPESARICAVO, 
    tx_VOCESPESARICAVO, 
    ac_SPESARICAVO, 
    ac_VOCESPESARICAVO_erp, 
    fl_EVENTO, 
    fl_RELATORE,
    fl_AUTOADD_ECM,
	fl_AUTOADD_NOECM,
    ac_CATEGORIAECM_AUTOADD_1,
    ac_CATEGORIAECM_AUTOADD_2,
    ac_CATEGORIAECM_AUTOADD_3,
    dt_CREAZIONE, 
    tx_CREAZIONE
) VALUES (
    @ac_VOCESPESARICAVO,
    @tx_VOCESPESARICAVO,
    @ac_SPESARICAVO,
    @ac_VOCESPESARICAVO_erp,
    @fl_EVENTO,
    @fl_RELATORE,
    @fl_AUTOADD_ECM,
	@fl_AUTOADD_NOECM,
    @ac_CATEGORIAECM_AUTOADD_1,
    @ac_CATEGORIAECM_AUTOADD_2,
    @ac_CATEGORIAECM_AUTOADD_3,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)                
)                
                "
                SelectCommand="SELECT * FROM age_VOCISPESARICAVO WHERE ac_VOCESPESARICAVO=@ac_VOCESPESARICAVO"
                UpdateCommand="
UPDATE  age_VOCISPESARICAVO
SET     tx_VOCESPESARICAVO = @tx_VOCESPESARICAVO,
        ac_SPESARICAVO = @ac_SPESARICAVO,
        ac_VOCESPESARICAVO_erp = @ac_VOCESPESARICAVO_erp,
        fl_EVENTO = @fl_EVENTO,
        fl_RELATORE = @fl_RELATORE,
        fl_AUTOADD_ECM = @fl_AUTOADD_ECM,
        fl_AUTOADD_NOECM = @fl_AUTOADD_NOECM,
        ac_CATEGORIAECM_AUTOADD_1 = @ac_CATEGORIAECM_AUTOADD_1,
        ac_CATEGORIAECM_AUTOADD_2 = @ac_CATEGORIAECM_AUTOADD_2,
        ac_CATEGORIAECM_AUTOADD_3 = @ac_CATEGORIAECM_AUTOADD_3,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)   
WHERE   ac_VOCESPESARICAVO=@ac_VOCESPESARICAVO
                ">
                <UpdateParameters>
                    <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
                    <asp:Parameter Name="tx_VOCESPESARICAVO" Type="String" />
                    <asp:Parameter Name="ac_SPESARICAVO" Type="String" />
                    <asp:Parameter Name="ac_VOCESPESARICAVO_erp" Type="String" />
                    <asp:Parameter Name="fl_EVENTO" Type="Boolean" />
                    <asp:Parameter Name="fl_RELATORE" Type="Boolean" />
                    <asp:Parameter Name="fl_AUTOADD_ECM" Type="Boolean" />
                    <asp:Parameter Name="fl_AUTOADD_NOECM" Type="Boolean" />
                    <asp:Parameter Name="ac_CATEGORIAECM_AUTOADD_1" Type="String" />
                    <asp:Parameter Name="ac_CATEGORIAECM_AUTOADD_2" Type="String" />
                    <asp:Parameter Name="ac_CATEGORIAECM_AUTOADD_3" Type="String" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
                    <asp:Parameter Name="tx_VOCESPESARICAVO" Type="String" />
                    <asp:Parameter Name="ac_SPESARICAVO" Type="String" />
                    <asp:Parameter Name="ac_VOCESPESARICAVO_erp" Type="String" />
                    <asp:Parameter Name="fl_EVENTO" Type="Boolean" />
                    <asp:Parameter Name="fl_RELATORE" Type="Boolean" />
                    <asp:Parameter Name="fl_AUTOADD_ECM" Type="Boolean" />
                    <asp:Parameter Name="fl_AUTOADD_NOECM" Type="Boolean" />
                    <asp:Parameter Name="ac_CATEGORIAECM_AUTOADD_1" Type="String" />
                    <asp:Parameter Name="ac_CATEGORIAECM_AUTOADD_2" Type="String" />
                    <asp:Parameter Name="ac_CATEGORIAECM_AUTOADD_3" Type="String" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                </InsertParameters>
            </stl:StlSqlDataSource>
            <asp:SqlDataSource ID="sdsac_CATEGORIAECM" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT ac_CATEGORIAECM as ac, tx_CATEGORIAECM as tx FROM eve_CATEGORIEECM WHERE ac_CATEGORIAECM<>'P' ORDER BY ni_ORDINE">
            </asp:SqlDataSource>
        
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
