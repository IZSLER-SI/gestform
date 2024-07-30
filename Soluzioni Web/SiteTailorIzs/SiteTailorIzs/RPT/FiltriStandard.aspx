<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="FiltriStandard.aspx.vb" Inherits="Softailor.SiteTailorIzs.FiltriStandard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script src="FiltriStandard.js"></script>
    <stl:StlUpdatePanel ID="updFONTI_g" runat="server" Width="350px" Height="406px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdFONTI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_FONTE" DataSourceID="sdsFONTI_g"
                EnableViewState="False" ItemDescriptionPlural="fonti" ItemDescriptionSingular="fonte"
                Title="Fonti Dati" AllowDelete="false"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:BoundField DataField="tx_FONTE" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFONTI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="
                SELECT      ac_FONTE, 
                            tx_FONTE 
                FROM        rpt_FONTI 
                WHERE       fl_FILTRI = 1
                ORDER BY    tx_FONTE">
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updFILTRI_g" runat="server" Width="860px" Height="700px" Top="0px" Left="370px">
        <ContentTemplate>
            <stl:StlGridView ID="grdFILTRI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_FILTRO" DataSourceID="sdsFILTRI_g"
                EnableViewState="False" ItemDescriptionPlural="filtri" ItemDescriptionSingular="filtro"
                Title="Filtri predefiniti per la fonte dati selezionata" 
                DeleteConfirmationQuestion="" ParentStlGridViewID="grdFONTI" BoundStlFormViewID="frmFILTRI">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="tx_FILTRO" HeaderText="Descrizione Filtro" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFILTRI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="
                    SELECT 
                        FLT.id_FILTRO,
                        FLT.ac_FONTE,
                        FLT.tx_FILTRO
                    FROM
                        rpt_FILTRI FLT
                    WHERE
                        FLT.ac_FONTE = @ac_FONTE
                    ORDER BY
                        FLT.tx_FILTRO"
                    DeleteCommand="DELETE FROM rpt_FILTRI WHERE id_FILTRO=@id_FILTRO"
                >
                <SelectParameters>
                    <asp:Parameter Name="ac_FONTE" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_FILTRO" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updFILTRI_f" runat="server" Width="860px" Height="48px" Top="710px" Left="370px">
        <ContentTemplate>
            <stl:StlFormView ID="frmFILTRI" runat="server" DataKeyNames="id_FILTRO"
                DataSourceID="sdsFILTRI_f" NewItemText="" BoundStlGridViewID="grdFILTRI">
                <EditItemTemplate>
                    <span class="flbl" style="width: 90px;">Descrizione filtro</span>
                    <asp:TextBox ID="tx_FILTROTextBox" runat="server"
                        Text='<%# Bind("tx_FILTRO")%>' Width="570px"  />
                    <span class="flbl" style="width: 5px;"> </span>
                    <bof:CtlSelettoreFiltroReport ID="ctlFiltro" runat="server"
                         FieldName="xm_FILTRO"
                        Value='<%# Bind("xm_FILTRO")%>' />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsFILTRI_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"

                SelectCommand="SELECT * FROM rpt_FILTRI WHERE id_FILTRO=@id_FILTRO"
                InsertCommand="
                INSERT INTO rpt_FILTRI
                    (ac_FONTE, tx_FILTRO, xm_FILTRO)
                VALUES
                    (@ac_FONTE, @tx_FILTRO, @xm_FILTRO)
                SET @id_FILTRO=SCOPE_IDENTITY()
                "
                UpdateCommand="
                UPDATE  rpt_FILTRI
                SET     tx_FILTRO = @tx_FILTRO,
                        xm_FILTRO = @xm_FILTRO
                WHERE   id_FILTRO = @id_FILTRO
                ">
                <SelectParameters>
                    <asp:Parameter Name="id_FILTRO" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_FONTE" Type="String" />
	                <asp:Parameter Name="tx_FILTRO" Type="String" />
                    <asp:Parameter Name="xm_FILTRO" Type="String" />
	                <asp:Parameter Name="id_FILTRO" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
	                <asp:Parameter Name="tx_FILTRO" Type="String" />
                    <asp:Parameter Name="xm_FILTRO" Type="String" />
	                <asp:Parameter Name="id_FILTRO" Type="Int32" />
                </UpdateParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <asp:UpdatePanel ID="updFonte" runat="server" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>
            <div style="display:none;">
                <asp:TextBox ID="txtFonte" runat="server" EnableViewState="false" ClientIDMode="Static" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
