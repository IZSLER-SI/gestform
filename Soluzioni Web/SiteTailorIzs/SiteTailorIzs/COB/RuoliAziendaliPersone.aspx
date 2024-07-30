<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="RuoliAziendaliPersone.aspx.vb" Inherits="Softailor.SiteTailorIzs.RuoliAziendaliPersone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">

    <div style="font-size: 11px; border: 1px solid #666666; background-color: #f0f0f0; width: 1082px; padding: 9px;">
        <b>Ruolo</b>&nbsp;
        <asp:DropDownList ID="ddnac_RUOLO" runat="server" EnableViewState="true" AutoPostBack="true" CssClass="ddn" Width="550px" />
        &nbsp;&nbsp;&nbsp;
        <b>Visualizza</b>&nbsp;
        <asp:DropDownList ID="ddnac_TIPOVIS" runat="server" EnableViewState="true" AutoPostBack="true" CssClass="ddn" Width="180px">
            <asp:ListItem Value="ATT" Text="Situazione ad oggi" />
            <asp:ListItem Value="ALL" Text="Situazione complessiva" />
        </asp:DropDownList>
    </div>

    <stl:StlUpdatePanel ID="updRUOLI_g" runat="server" Width="1100px" Height="500px" Top="50px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdRUOLI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_RUOLO_PERSONA" DataSourceID="sdsRUOLI_g"
                EnableViewState="False" ItemDescriptionPlural="persone" ItemDescriptionSingular="persona"
                Title="Persone associate al ruolo selezionato"
                DeleteConfirmationQuestion="" BoundStlFormViewID="frmRUOLI">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="tx_COGNOME" HeaderText="Cognome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_NOME" HeaderText="Nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Dal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_FINE" HeaderText="Al" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="tx_UNITAOPERATIVA" HeaderText="UO/Reparto" />
                    <asp:BoundField DataField="tx_RIFERIMENTO" HeaderText="Riferimento" />
                    <asp:BoundField DataField="tx_NOTE" HeaderText="Note" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="dt_ASSUNZ" HeaderText="Assunzione" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="dt_CESSAZ" HeaderText="Cessazione" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsRUOLI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM cob_RUOLI_PERSONE WHERE id_RUOLO_PERSONA = @id_RUOLO_PERSONA"
                SelectCommandType="StoredProcedure"
                SelectCommand="sp_cob_RuoliPersone">
                <DeleteParameters>
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="ac_RUOLO" Type="String" />
                    <asp:Parameter Name="ac_TIPOVIS" Type="String" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updRUOLI_f" runat="server" Top="560px" Left="0px" Width="1100px" Height="90px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmRUOLI" DataSourceID="sdsRUOLI_f" NewItemText="Nuovo ruolo"
                DataKeyNames="id_RUOLO_PERSONA" BoundStlGridViewID="grdRUOLI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 90px;">Persona</span>
                    <bof:CtlSelettorePersonaGForm ID="id_PERSONACtlSelettorePersonaGForm" runat="server"
                        Width="510px" FieldName="id_PERSONA" Value='<%# Bind("id_PERSONA")%>' SoloDipendenti="true" />
                    <span class="slbl" style="width: 80px;">Data di inizio</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server"
                        Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>' Width="85px" CssClass="stl_dt_data_ddmmyyyy" />
                    <span class="slbl" style="width: 75px;">Data di fine</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server"
                        Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>' Width="85px" CssClass="stl_dt_data_ddmmyyyy" />
                    <br />
                    <span class="flbl" style="width: 90px;">UO/Reparto</span>
                    <asp:DropDownList ID="ac_UNITAOPERATIVADropDownList" runat="server" SelectedValue='<%# Bind("ac_UNITAOPERATIVA")%>'
                        DataSourceID="sdsac_UNITAOPERATIVA" DataTextField="tx_UNITAOPERATIVA" DataValueField="ac_UNITAOPERATIVA" Width="472px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 90px;">Riferimento</span>
                    <asp:TextBox ID="tx_RIFERIMENTOTextBox" runat="server"
                        Text='<%# Bind("tx_RIFERIMENTO")%>' Width="468px" />
                    <span class="slbl" style="width: 40px;">Note</span>
                    <asp:TextBox ID="tx_NOTETextBox" runat="server"
                        Text='<%# Bind("tx_NOTE")%>' Width="327px" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsRUOLI_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM cob_RUOLI_PERSONE WHERE id_RUOLO_PERSONA=@id_RUOLO_PERSONA"
        InsertCommand="
INSERT INTO cob_RUOLI_PERSONE (
    id_PERSONA,
    ac_RUOLO,
    dt_INIZIO,
    dt_FINE,
    ac_UNITAOPERATIVA,
    tx_RIFERIMENTO,
    tx_NOTE,
    dt_CREAZIONE,
    tx_CREAZIONE
) VALUES (
    @id_PERSONA,
    @ac_RUOLO,
    @dt_INIZIO,
    @dt_FINE,
    @ac_UNITAOPERATIVA,
    @tx_RIFERIMENTO,
    @tx_NOTE,
    GETDATE(),
    @tx_CREAZIONE           
);
SET @id_RUOLO_PERSONA = SCOPE_IDENTITY();   
        "
        UpdateCommand="
UPDATE  cob_RUOLI_PERSONE
SET     id_PERSONA = @id_PERSONA,
        dt_INIZIO = @dt_INIZIO,
        dt_FINE = @dt_FINE,
        ac_UNITAOPERATIVA = @ac_UNITAOPERATIVA,
        tx_RIFERIMENTO = @tx_RIFERIMENTO,
        tx_NOTE = @tx_NOTE,
        dt_MODIFICA = GETDATE(),
        tx_MODIFICA = @tx_MODIFICA
WHERE   id_RUOLO_PERSONA = @id_RUOLO_PERSONA
        ">
        <SelectParameters>
            <asp:Parameter Name="id_RUOLO_PERSONA" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
            <asp:Parameter Name="ac_RUOLO" Type="String" />
            <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
            <asp:Parameter Name="dt_FINE" Type="DateTime" />
            <asp:Parameter Name="ac_UNITAOPERATIVA" Type="String" />
            <asp:Parameter Name="tx_RIFERIMENTO" Type="String" />
            <asp:Parameter Name="tx_NOTE" Type="String" />
            <asp:Parameter Name="tx_CREAZIONE" Type="String" />
            <asp:Parameter Name="id_RUOLO_PERSONA" Type="Int32" Direction="Output" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
            <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
            <asp:Parameter Name="dt_FINE" Type="DateTime" />
            <asp:Parameter Name="ac_UNITAOPERATIVA" Type="String" />
            <asp:Parameter Name="tx_RIFERIMENTO" Type="String" />
            <asp:Parameter Name="tx_NOTE" Type="String" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="id_RUOLO_PERSONA" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>

        </ContentTemplate>
    </stl:StlUpdatePanel>
        <asp:SqlDataSource ID="sdsac_UNITAOPERATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_UNITAOPERATIVA, tx_UNITAOPERATIVA FROM vw_age_UNITAOPERATIVE ORDER BY fl_OBSOLETA, tx_UNITAOPERATIVA_sort"></asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
