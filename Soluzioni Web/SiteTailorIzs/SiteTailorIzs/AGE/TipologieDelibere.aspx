<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="TipologieDelibere.aspx.vb" Inherits="Softailor.SiteTailorIzs.TipologieDelibere" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updTIPOLOGIEDELIBERE_g" runat="server" Width="840px" Height="450px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdTIPOLOGIEDELIBERE" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_TIPOLOGIADELIBERA" DataSourceID="sdsTIPOLOGIEDELIBERE_g"
                EnableViewState="False" ItemDescriptionPlural="tipologie" ItemDescriptionSingular="tipologia"
                Title="Tipologie Delibere" BoundStlFormViewID="frmTIPOLOGIEDELIBERE"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="ac_TIPOLOGIADELIBERA" HeaderText="Codice" />
                    <asp:BoundField DataField="tx_TIPOLOGIADELIBERA" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Piano Formativo" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_PIANOFORMATIVO"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Spese Evento" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_SPESEEVENTO"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Spese Relatori" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_SPESERELATORE"), "<img src=""" & Page.ResolveUrl("~/img/icoV.gif") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Utilizzata" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# If(Eval("fl_UTILIZZATA"), "<img src=""" & Page.ResolveUrl("~/img/icoExclOrange.png") & """ />", "")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsTIPOLOGIEDELIBERE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM age_TIPOLOGIEDELIBERE WHERE ac_TIPOLOGIADELIBERA=@ac_TIPOLOGIADELIBERA"
                SelectCommand="SELECT * FROM vw_age_TIPOLOGIEDELIBERE_grid ORDER BY tx_TIPOLOGIADELIBERA">
                <DeleteParameters>
                    <asp:Parameter Name="ac_TIPOLOGIADELIBERA" Type="String" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updTIPOLOGIEDELIBERE_f" runat="server" Width="840px" Height="114px" Top="455px" Left="0px">
        <ContentTemplate>
            <stl:StlFormView ID="frmTIPOLOGIEDELIBERE" runat="server" DataKeyNames="ac_TIPOLOGIADELIBERA"
                DataSourceID="sdsTIPOLOGIEDELIBERE_f" NewItemText="" BoundStlGridViewID="grdTIPOLOGIEDELIBERE">
                <EditItemTemplate>
                    <span class="flbl" style="width: 50px;">Codice</span>
                    <asp:TextBox ID="ac_TIPOLOGIADELIBERATextBox" runat="server"
                        Text='<%# Bind("ac_TIPOLOGIADELIBERA")%>' Width="100px" />
                    <span class="slbl" style="width: 80px;">Descrizione</span>
                    <asp:TextBox ID="tx_TIPOLOGIADELIBERATextBox" runat="server"
                        Text='<%# Bind("tx_TIPOLOGIADELIBERA")%>' Width="588px" Font-Bold="true" />
                    <br />
                    
                    <span class="flbl" style="width: 90px;">Piano Formativo</span>
                    <asp:CheckBox ID="fl_PIANOFORMATIVOCheckBox" runat="server"
                        Checked='<%# Bind("fl_PIANOFORMATIVO")%>' />
                    <br />
                    <span class="flbl" style="width: 90px;">Spese Evento</span>
                    <asp:CheckBox ID="fl_SPESEEVENTOCheckBox" runat="server"
                        Checked='<%# Bind("fl_SPESEEVENTO")%>' />
                    <br />
                    <span class="flbl" style="width: 90px;">Spese Relatore</span>
                    <asp:CheckBox ID="fl_SPESERELATORECheckBox" runat="server"
                        Checked='<%# Bind("fl_SPESERELATORE")%>' />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsTIPOLOGIEDELIBERE_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO age_TIPOLOGIEDELIBERE (
    ac_TIPOLOGIADELIBERA,
    tx_TIPOLOGIADELIBERA,
    fl_PIANOFORMATIVO,
    fl_SPESEEVENTO,
    fl_SPESERELATORE,
    dt_CREAZIONE,
    tx_CREAZIONE
) VALUES (
    @ac_TIPOLOGIADELIBERA,
    @tx_TIPOLOGIADELIBERA,
    @fl_PIANOFORMATIVO,
    @fl_SPESEEVENTO,
    @fl_SPESERELATORE,
    GETDATE(),
    (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
)
                "
                SelectCommand="SELECT * FROM age_TIPOLOGIEDELIBERE WHERE ac_TIPOLOGIADELIBERA=@ac_TIPOLOGIADELIBERA"
                UpdateCommand="
UPDATE  age_TIPOLOGIEDELIBERE
SET     tx_TIPOLOGIADELIBERA = @tx_TIPOLOGIADELIBERA,
        fl_PIANOFORMATIVO = @fl_PIANOFORMATIVO,
        fl_SPESEEVENTO = @fl_SPESEEVENTO,
        fl_SPESERELATORE = @fl_SPESERELATORE,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = (SELECT USERNAME FROM ac_UTENTI WHERE ID_UTENT=@id_UTENT)
WHERE   ac_TIPOLOGIADELIBERA = @ac_TIPOLOGIADELIBERA
                ">
                <UpdateParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_TIPOLOGIADELIBERA" Type="String" />
                    <asp:Parameter Name="tx_TIPOLOGIADELIBERA" Type="String" />
                    <asp:Parameter Name="fl_PIANOFORMATIVO" Type="Boolean" />
                    <asp:Parameter Name="fl_SPESEEVENTO" Type="Boolean" />
                    <asp:Parameter Name="fl_SPESERELATORE" Type="Boolean" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_TIPOLOGIADELIBERA" Type="String" DefaultValue="" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                    <asp:Parameter Name="ac_TIPOLOGIADELIBERA" Type="String" />
                    <asp:Parameter Name="tx_TIPOLOGIADELIBERA" Type="String" />
                    <asp:Parameter Name="fl_PIANOFORMATIVO" Type="Boolean" />
                    <asp:Parameter Name="fl_SPESEEVENTO" Type="Boolean" />
                    <asp:Parameter Name="fl_SPESERELATORE" Type="Boolean" />
                </InsertParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
