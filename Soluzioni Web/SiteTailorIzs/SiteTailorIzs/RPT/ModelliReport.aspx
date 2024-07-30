<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ModelliReport.aspx.vb" Inherits="Softailor.SiteTailorIzs.ModelliReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script>
        function CampiFonte(f) {
            var url = 'CampiFonte.aspx?f=' + f;
            wopen(url, 'showPersone', 800, 700, 1, 0, 1, 1, 1);
        }
    </script>
    <stl:StlUpdatePanel ID="updFONTI_g" runat="server" Width="350px" Height="660px" Top="0px" Left="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdFONTI" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="ac_FONTE, tx_SHAREPOINTFOLDERRELATIVE" DataSourceID="sdsFONTI_g"
                EnableViewState="False" ItemDescriptionPlural="fonti" ItemDescriptionSingular="fonte"
                Title="Fonti Dati" AllowDelete="false"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:BoundField DataField="tx_FONTE" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Comandi">
                        <ItemTemplate>
                            <%# "<a class=""classicA"" target=""_blank"" href=""" & Eval("tx_SHAREPOINTFOLDER") & """>Cartella Modelli</a>" &
                                "<br/>" &
                                "<span class=""classicA"" onclick=""CampiFonte('" & Eval("ac_FONTE") & "');"">Elenco Campi</span>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFONTI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="
                SELECT      ac_FONTE, 
                            tx_FONTE, 
                            @tx_SHAREPOINTBASE + '/' + tx_SHAREPOINTFOLDER as tx_SHAREPOINTFOLDER,
                            @tx_SHAREPOINTRELATIVEBASE + '/' + tx_SHAREPOINTFOLDER as tx_SHAREPOINTFOLDERRELATIVE 
                FROM        rpt_FONTI 
                WHERE       fl_WORDEXCEL = 1
                ORDER BY    tx_FONTE">
                <SelectParameters>
                    <asp:Parameter Name="tx_SHAREPOINTBASE" Type="String" />
                    <asp:Parameter Name="tx_SHAREPOINTRELATIVEBASE" Type="String" />
                </SelectParameters>
            </stl:StlSqlDataSource>
            
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updREPORTS_g" runat="server" Width="860px" Height="660px" Top="0px" Left="370px">
        <ContentTemplate>
            <stl:StlGridView ID="grdREPORTS" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_REPORT" DataSourceID="sdsREPORTS_g"
                EnableViewState="False" ItemDescriptionPlural="modelli" ItemDescriptionSingular="modello"
                Title="Modelli report per la fonte dati selezionata" 
                DeleteConfirmationQuestion="" ParentStlGridViewID="grdFONTI" BoundStlFormViewID="frmREPORTS">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="tx_REPORT" HeaderText="Descrizione Modello" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="ac_TIPOFILE" HeaderText="Tipo" />
                    <asp:TemplateField HeaderText="Modello">
                        <ItemTemplate>
                            <%# "<a class=""classicA"" href=""" & Eval("tx_URLFILE") & """>" & Eval("tx_FILE") & "</a>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tx_FILTRO" HeaderText="Filtro predefinito" />
                    <asp:BoundField DataField="ac_ORDINAMENTO" HeaderText="Ordinamento predefinito" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsREPORTS_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="
                    SELECT 
                        RPT.id_REPORT,
                        RPT.ac_FONTE,
                        RPT.tx_REPORT,
                        RPT.ac_TIPOFILE,
                        RPT.tx_FILE,
                        @tx_SHAREPOINTBASE + '/' + FNT.tx_SHAREPOINTFOLDER + '/' + tx_FILE as tx_URLFILE,
                        FLT.tx_FILTRO,       
                        RPT.ac_ORDINAMENTO
                    FROM
                        rpt_REPORTS RPT
                        INNER JOIN rpt_FONTI FNT ON RPT.ac_FONTE=FNT.ac_FONTE
                        LEFT OUTER JOIN rpt_FILTRI FLT ON RPT.id_FILTRO=FLT.id_FILTRO
                    WHERE
                        RPT.ac_FONTE = @ac_FONTE
                    ORDER BY
                        RPT.tx_REPORT"
                    DeleteCommand="DELETE FROM rpt_REPORTS WHERE id_REPORT=@id_REPORT"
                >
                <SelectParameters>
                    <asp:Parameter Name="ac_FONTE" Type="String" />
                    <asp:Parameter Name="tx_SHAREPOINTBASE" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_REPORT" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updREPORTS_f" runat="server" Width="860px" Height="112px" Top="670px" Left="370px">
        <ContentTemplate>
            <stl:StlFormView ID="frmREPORTS" runat="server" DataKeyNames="id_REPORT"
                DataSourceID="sdsREPORTS_f" NewItemText="" BoundStlGridViewID="grdREPORTS">
                <EditItemTemplate>
                    <span class="flbl" style="width: 130px;">Descrizione report</span>
                    <asp:TextBox ID="tx_REPORTTextBox" runat="server"
                        Text='<%# Bind("tx_REPORT")%>' Width="710px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 130px;">File modello</span>
                    <bof:CtlSelettoreFileSharepoint ID="ctlFileSP" runat="server" 
                         FieldName="tx_FILE" Width="716px"
                         Value='<%# Bind("tx_FILE")%>'
                         ac_TIPOFILE='<%# Bind("ac_TIPOFILE")%>'
                        FolderRelativeUrlTextBoxID="txtModelsRelativeUrl"
                         ExtensionList=".docx;.xlsx"
                        />
                    <br />
                    <span class="flbl" style="width: 130px;">Filtro predefinito</span>
                    <asp:DropDownList ID="id_FILTRODropDownList" runat="server" SelectedValue='<%# Bind("id_FILTRO")%>'
                        DataSourceID="sdsid_FILTRO" DataTextField="tx_FILTRO" DataValueField="id_FILTRO" Width="714px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 130px;">Ordinamento predefinito</span>
                    <asp:DropDownList ID="ac_ORDINAMENTODropDownList" runat="server" SelectedValue='<%# Bind("ac_ORDINAMENTO")%>'
                        DataSourceID="sdsac_ORDINAMENTO" DataTextField="ac_ORDINAMENTO" DataValueField="ac_ORDINAMENTO" Width="714px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsREPORTS_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"

                SelectCommand="SELECT * FROM rpt_REPORTS WHERE id_REPORT=@id_REPORT"
                InsertCommand="
                INSERT INTO rpt_REPORTS
                    (ac_FONTE, tx_REPORT, ac_TIPOFILE, tx_FILE, id_FILTRO, ac_ORDINAMENTO)
                VALUES
                    (@ac_FONTE, @tx_REPORT, @ac_TIPOFILE, @tx_FILE, @id_FILTRO, @ac_ORDINAMENTO)
                SET @id_REPORT=SCOPE_IDENTITY()
                "
                UpdateCommand="
                UPDATE  rpt_REPORTS
                SET     tx_REPORT = @tx_REPORT,
                        ac_TIPOFILE = @ac_TIPOFILE,
                        tx_FILE = @tx_FILE,
                        id_FILTRO = @id_FILTRO,
                        ac_ORDINAMENTO = @ac_ORDINAMENTO
                WHERE   id_REPORT = @id_REPORT
                ">
                <SelectParameters>
                    <asp:Parameter Name="id_REPORT" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_FONTE" Type="String" />
	                <asp:Parameter Name="tx_REPORT" Type="String" />
                    <asp:Parameter Name="ac_TIPOFILE" Type="String" />
	                <asp:Parameter Name="tx_FILE" Type="String" />
                    <asp:Parameter Name="id_FILTRO" Type="Int32" />
                    <asp:Parameter Name="ac_ORDINAMENTO" Type="String" />
	                <asp:Parameter Name="id_REPORT" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
	                <asp:Parameter Name="tx_REPORT" Type="String" />
                    <asp:Parameter Name="ac_TIPOFILE" Type="String" />
	                <asp:Parameter Name="tx_FILE" Type="String" />
                    <asp:Parameter Name="id_FILTRO" Type="Int32" />
                    <asp:Parameter Name="ac_ORDINAMENTO" Type="String" />
	                <asp:Parameter Name="id_REPORT" Type="Int32" />
                </UpdateParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <asp:UpdatePanel ID="updUrl" runat="server" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>
            <div style="display:none;">
                <asp:TextBox ID="txtModelsRelativeUrl" runat="server" EnableViewState="false" ClientIDMode="Static" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:ObjectDataSource ID="sdsac_ORDINAMENTO" runat="server" SelectMethod="GetOrdinamentoFonte" TypeName="Softailor.SiteTailorIzs.OrdinamentoFonteData">
        <SelectParameters>
            <asp:ControlParameter Name="ac_FONTE" ControlID="grdFONTI" Type="String" PropertyName="SelectedDataKey.Values(0)" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:SqlDataSource ID="sdsid_FILTRO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT id_FILTRO, tx_FILTRO FROM rpt_FILTRI WHERE ac_FONTE=@ac_FONTE ORDER BY tx_FILTRO">
        <SelectParameters>
            <asp:ControlParameter Name="ac_FONTE" ControlID="grdFONTI" Type="String" PropertyName="SelectedDataKey.Values(0)" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
