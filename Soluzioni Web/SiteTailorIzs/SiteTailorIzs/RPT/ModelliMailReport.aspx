<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ModelliMailReport.aspx.vb" Inherits="Softailor.SiteTailorIzs.ModelliMailReport" %>
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
                AutoGenerateColumns="False" DataKeyNames="ac_FONTE" DataSourceID="sdsFONTI_g"
                EnableViewState="False" ItemDescriptionPlural="fonti" ItemDescriptionSingular="fonte"
                Title="Fonti Dati" AllowDelete="false"
                DeleteConfirmationQuestion="">
                <Columns>
                    <asp:BoundField DataField="tx_FONTE" HeaderText="Descrizione" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Comandi">
                        <ItemTemplate>
                            <%# "<span class=""classicA"" onclick=""CampiFonte('" & Eval("ac_FONTE") & "');"">Elenco Campi</span>"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsFONTI_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="
                SELECT      ac_FONTE, 
                            tx_FONTE
                FROM        rpt_FONTI 
                WHERE       fl_MAIL = 1
                ORDER BY    tx_FONTE">
            </stl:StlSqlDataSource>
            
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updMAILREPORTS_g" runat="server" Width="860px" Height="330px" Top="0px" Left="370px">
        <ContentTemplate>
            <stl:StlGridView ID="grdMAILREPORTS" runat="server" AddCommandText=""
                AutoGenerateColumns="False" DataKeyNames="id_MAILREPORT" DataSourceID="sdsMAILREPORTS_g"
                EnableViewState="False" ItemDescriptionPlural="modelli" ItemDescriptionSingular="modello"
                Title="Modelli e-mail per la fonte dati selezionata" 
                DeleteConfirmationQuestion="" ParentStlGridViewID="grdFONTI" BoundStlFormViewID="frmMAILREPORTS">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="tx_REPORT" HeaderText="Descrizione Modello" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_FILTRO" HeaderText="Filtro predefinito" />
                    <asp:BoundField DataField="ac_ORDINAMENTO" HeaderText="Ordinamento predefinito" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsMAILREPORTS_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="
                    SELECT 
                        RPT.id_MAILREPORT,
                        RPT.ac_FONTE,
                        RPT.tx_REPORT,
                        FLT.tx_FILTRO,       
                        RPT.ac_ORDINAMENTO
                    FROM
                        rpt_MAILREPORTS RPT
                        LEFT OUTER JOIN rpt_FILTRI FLT ON RPT.id_FILTRO=FLT.id_FILTRO
                    WHERE
                        RPT.ac_FONTE = @ac_FONTE
                    ORDER BY
                        RPT.tx_REPORT"
                    DeleteCommand="DELETE FROM rpt_MAILREPORTS WHERE id_MAILREPORT=@id_MAILREPORT"
                >
                <SelectParameters>
                    <asp:Parameter Name="ac_FONTE" Type="String" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="id_MAILREPORT" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlUpdatePanel ID="updMAILREPORTS_f" runat="server" Width="860px" Height="442px" Top="340px" Left="370px">
        <ContentTemplate>
            <stl:StlFormView ID="frmMAILREPORTS" runat="server" DataKeyNames="id_MAILREPORT"
                DataSourceID="sdsMAILREPORTS_f" NewItemText="" BoundStlGridViewID="grdMAILREPORTS">
                <EditItemTemplate>
                    <span class="flbl" style="width: 130px;">Descrizione modello</span>
                    <asp:TextBox ID="tx_REPORTTextBox" runat="server"
                        Text='<%# Bind("tx_REPORT")%>' Width="710px" Font-Bold="true" />
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
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width: 130px;">Oggetto e-mail</span>
                    <asp:TextBox ID="tx_OGGETTOTextBox" runat="server"
                        Text='<%# Bind("tx_OGGETTO")%>' Width="710px" Font-Bold="true" />
                    <br />
                    <span class="flbl" style="width: 130px; vertical-align:top;">Corpo e-mail</span>
                    <stl:HtmlEditor ID="ht_CORPOHtmlEditor" runat="server" Width="712px" Height="315px"
                                                    ToolbarSet="Email" Value='<%# Bind("ht_CORPO")%>' 
                                                    FieldName="ht_CORPO" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsMAILREPORTS_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"

                SelectCommand="SELECT * FROM rpt_MAILREPORTS WHERE id_MAILREPORT=@id_MAILREPORT"
                InsertCommand="
                INSERT INTO rpt_MAILREPORTS
                    (ac_FONTE, tx_REPORT, tx_OGGETTO, ht_CORPO, id_FILTRO, ac_ORDINAMENTO)
                VALUES
                    (@ac_FONTE, @tx_REPORT, @tx_OGGETTO, @ht_CORPO, @id_FILTRO, @ac_ORDINAMENTO)
                SET @id_MAILREPORT=SCOPE_IDENTITY()
                "
                UpdateCommand="
                UPDATE  rpt_MAILREPORTS
                SET     tx_REPORT = @tx_REPORT,
                        tx_OGGETTO = @tx_OGGETTO,
                        ht_CORPO = @ht_CORPO,
                        id_FILTRO = @id_FILTRO,
                        ac_ORDINAMENTO = @ac_ORDINAMENTO
                WHERE   id_MAILREPORT = @id_MAILREPORT
                ">
                <SelectParameters>
                    <asp:Parameter Name="id_MAILREPORT" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="ac_FONTE" Type="String" />
	                <asp:Parameter Name="tx_REPORT" Type="String" />
                    <asp:Parameter Name="tx_OGGETTO" Type="String" />
	                <asp:Parameter Name="ht_CORPO" Type="String" />
                    <asp:Parameter Name="id_FILTRO" Type="Int32" />
                    <asp:Parameter Name="ac_ORDINAMENTO" Type="String" />
	                <asp:Parameter Name="id_MAILREPORT" Type="Int32" Direction="Output" />
                </InsertParameters>
                <UpdateParameters>
	                <asp:Parameter Name="tx_REPORT" Type="String" />
                    <asp:Parameter Name="tx_OGGETTO" Type="String" />
	                <asp:Parameter Name="ht_CORPO" Type="String" />
                    <asp:Parameter Name="id_FILTRO" Type="Int32" />
                    <asp:Parameter Name="ac_ORDINAMENTO" Type="String" />
	                <asp:Parameter Name="id_MAILREPORT" Type="Int32" />
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
