<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="Persona.aspx.vb" Inherits="Softailor.SiteTailorIzs.Persona" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        #popupContent
        {
            overflow:hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">Scheda Persona</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="buttonsection">
                <asp:LinkButton ID="lnkClose" runat="server" CssClass="tbbtn">
                    <span class="icon close"></span>
                    Chiudi
                </asp:LinkButton>
                <span style="display:none"><asp:LinkButton ID="lnkDownload" runat="server">-</asp:LinkButton></span>
            </div>
        </ContentTemplate>
           <Triggers>
		    <asp:PostBackTrigger ControlID="lnkDownload" />
            </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <link href="<%=Page.ResolveUrl("~/Scripts/shieldui/css/less/shieldui-all.min.css")%>" rel="stylesheet" />
		<script src="<%=Page.ResolveUrl("~/Scripts/shieldui/js/shieldui-all.min.js")%>"></script>
        <script>
        $(function () {
            $("#phdContent_frmPERSONE_lnkDownloadCv").click(function () {
                __doPostBack('ctl00$phdPopupButtons$lnkDownload','');
                
            });
        });
        </script>
		<style>
				.sui-rating-disabled {
						opacity: 1!important
				}
		</style>
		<script>
        function schedaAutocertificazione(id) {
            if (stl_appb_row2Select_action == null) {
                stl_sel_display_wh('../FOR/Autocertificazione.aspx?id=' + id, 1102, 400, editAutocertificazione_callback);
            }
            else {
                if (stl_appb_row2Select_action != 'Delete') {
                    stl_sel_display_wh('../FOR/Autocertificazione.aspx?id=' + id, 1102, 400, editAutocertificazione_callback);
                }
            }
        }
        function schedaPartecipazione(id) {
            if (stl_appb_row2Select_action == null) {
                stl_sel_display_wh('../FOR/Partecipazione.aspx?id=' + id, 1102, 800, editPartecipazione_callback);
            }
            else {
                if (stl_appb_row2Select_action != 'Delete') {
                    stl_sel_display_wh('../FOR/Partecipazione.aspx?id=' + id, 1102, 800, editPartecipazione_callback);
                }
            }
        }
        function schedaIscritto(id, ide) {
            if (stl_appb_row2Select_action == null) {
                stl_sel_display_wh('../../' + ide + '/EVE/SchedaPartecipante.aspx?id=' + id, 880, 780, editIscritto_callback);
            }
            else {
                if (stl_appb_row2Select_action != 'Delete') {
                    stl_sel_display_wh('../../' + ide + '/EVE/SchedaPartecipante.aspx?id=' + id, 880, 780, editIscritto_callback);
                }
            }
        }
    </script>
    <div style="display:none;">
        <script type="text/javascript">
            <asp:Literal ID="ltrRepositioning" runat="server" />    
        </script>
    </div>
    <asp:UpdatePanel ID="updHiddenCtls" runat="server" EnableViewState="true" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- controlli nascosti -->          
                <asp:LinkButton ID="lnkRepositionAutocertificazione" runat="server">-</asp:LinkButton>
                <asp:LinkButton ID="lnkRepositionPartecipazione" runat="server">-</asp:LinkButton>
                <asp:LinkButton ID="lnkRepositionIscritto" runat="server">-</asp:LinkButton>
                <asp:TextBox ID="txtReposition" runat="server" Text="0"></asp:TextBox>
                <asp:TextBox ID="txtRepositionEvento" runat="server" Text="0"></asp:TextBox>
            </ContentTemplate>
     
        </asp:UpdatePanel>
    <stl:StlUpdatePanel ID="updPERSONE_f" runat="server" Width="1070px" Height="390px" Top="15px" Left="15px">
        <ContentTemplate>
            <stl:StlFormView ID="frmPERSONE" runat="server" DataKeyNames="id_PERSONA"
                DataSourceID="sdsPERSONE_f" NewItemText="">
                <EditItemTemplate>
                    <div>
                        <span class="flbl" style="width: 220px;"><b>Dati Anagrafici</b></span>
                    </div>
                    <asp:Panel ID="pnlLock" runat="server" Visible='<%#isDipendente%>' CssClass="infoDiv11_small" Style="margin-right: 6px; margin-bottom: 2px;">
                        La persona è attualmente dipendente. Non tutti i dati anagrafici sono pertanto modificabili.
                    </asp:Panel>
                    <span class="flbl" style="width: 90px;">Titolo</span>
                    <asp:DropDownList ID="ac_TITOLODropDownList" runat="server" SelectedValue='<%# Bind("ac_TITOLO") %>'
                        DataSourceID="sdsac_TITOLO" DataTextField="tx_TITOLO" DataValueField="ac_TITOLO" Width="96px"
                        AppendDataBoundItems="true" Font-Bold="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 60px;">Cognome</span>
                    <asp:TextBox ID="tx_COGNOMETextBox" runat="server" Font-Bold="true" Text='<%# Bind("tx_COGNOME") %>'
                        Width="200px" Enabled='<%#Not isDipendente%>' />
                    <span class="slbl" style="width: 45px;">Nome</span>
                    <asp:TextBox ID="tx_NOMETextBox" runat="server" Font-Bold="true" Text='<%# Bind("tx_NOME") %>'
                        Width="200px" Enabled='<%#Not isDipendente%>' />
                    <span class="slbl" style="width: 55px;">Genere</span>
                    <asp:DropDownList ID="ac_GENEREDropDownList" runat="server" SelectedValue='<%# Bind("ac_GENERE") %>'
                        Width="80px" Enabled='<%#Not isDipendente%>'>
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem Value="M" Text="Maschio" />
                        <asp:ListItem Value="F" Text="Femmina" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 90px;">Data di nascita</span>
                    <asp:TextBox ID="dt_NASCITATextBox" runat="server" Text='<%# Bind("dt_NASCITA","{0:dd/MM/yyyy}") %>'
                        Width="80px" CssClass="stl_dt_data_ddmmyyyy" Enabled='<%#Not isDipendente%>' />
                    <span class="slbl" style="width: 45px;">Luogo:</span>
                    <bof:CtlSelettoreComuneItalia runat="server" ID="ac_COMUNENASCITASelettoreComuneItalia"
                        FieldName="ac_COMUNENASCITA" Value='<%# Bind("ac_COMUNENASCITA") %>' Enabled='<%#Not isDipendente%>' />
                    <br />
                    <span class="flbl" style="width: 90px;">Codice Fiscale</span>
                    <stl:StlItalianCFPITextBox runat="server" ID="ac_CODICEFISCALEStlItalianCFTextBox"
                        FieldName="ac_CODICEFISCALE" Value='<%# Bind("ac_CODICEFISCALE") %>' Enabled='<%#Not isDipendente%>' />
                    <span class="slbl" style="width: 45px;">E-mail</span>
                    <asp:TextBox ID="tx_EMAILTextBox" runat="server" Text='<%# Bind("tx_EMAIL") %>'
                        Width="243px" />
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 400px;"><b>Ruolo, profilo e dati ECM</b></span>
                    <br />
                    <asp:Panel ID="pnlLock2" runat="server" Visible='<%#isDipendente%>' CssClass="infoDiv11_small" Style="margin-right: 6px; margin-bottom: 2px;">
                        La persona è attualmente dipendente. Eventuali modifiche ai campi <em>profilo e categoria lavorativa</em> potrebbero essere sovrascritte dal sistema di sincronizzazione.
                    </asp:Panel>
                    <bof:CtlSelettoreRuoloProfilo ID="ruoloprofilo" runat="server" FieldName="ac_RUOLO"
                         ac_RUOLO='<%# Bind("ac_RUOLO")%>' ac_PROFILO='<%# Bind("ac_PROFILO")%>' />
                    <span class="slbl" style="width: 79px;">Cat.lavorativa</span>
                    <asp:DropDownList ID="ac_CATEGORIALAVORATIVADropDownList" runat="server" SelectedValue='<%# Bind("ac_CATEGORIALAVORATIVA")%>'
                        DataSourceID="sdsac_CATEGORIALAVORATIVA" DataTextField="tx" DataValueField="ac"
                        Width="140px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <bof:CtlSelettoreProfessioneDisciplina ID="profdisc" runat="server" FieldName="ac_PROFESSIONE"
                        ac_PROFESSIONE='<%# Bind("ac_PROFESSIONE")%>' id_DISCIPLINA='<%# Bind("id_DISCIPLINA") %>' />
                    <br />
                    <span class="flbl" style="width: 183px;">Ordine/Associazione Professionale</span>
                    <asp:DropDownList ID="id_ALBODropDownList" runat="server" SelectedValue='<%# Bind("id_ALBO")%>'
                        DataSourceID="sdsid_ALBO" DataTextField="tx_ALBO_SHORT" DataValueField="id_ALBO" Width="328px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 85px;">N° Iscrizione</span>
                    <asp:TextBox ID="ac_ISCRIZIONEALBOTextBox" runat="server" Text='<%# Bind("ac_ISCRIZIONEALBO")%>'
                        Width="60px" />
                    <span class="slbl" style="width: 90px;">Codice Esterno</span>
                    <asp:TextBox ID="ac_CODICEESTERNOTextBox" runat="server" Text='<%# Bind("ac_CODICEESTERNO")%>'
                        Width="80px" />
                    <br />
                    <span class="flbl" style="width: 136px;">Consultazione Albo Docenti</span>
                        <asp:CheckBox ID="fl_RESPONSABILE" runat="server" Text='' TextAlign="Left"  Checked='<%# Bind("fl_RESPONSABILE")%>'
                            Width="196px" />
                    <br />
                    <div class="sep_hor">
                    </div>

                    <div style="float:left;width:806px;">
                        <span class="flbl" style="width: 400px;"><b>Dati rapporto lavorativo</b></span>
                        <br />
                        <span class="flbl" style="width: 90px;">Matricola</span>
                        <asp:TextBox ID="ac_MATRICOLATextBox" runat="server" Text='<%# Eval("ac_MATRICOLA") %>' Enabled="false"
                            Width="80px" />
                        <span class="slbl" style="width: 100px;">Unità Operativa</span>
                        <asp:TextBox ID="tx_UNITAOPERATIVATextBox" runat="server" Text='<%# Eval("tx_UNITAOPERATIVA")%>' Enabled="false"
                            Width="506px" />
                        <br />
                        <span class="flbl" style="width: 90px;">Tipo Contratto</span>
                        <asp:TextBox ID="tx_TIPOCONTRATTOTextBox" runat="server" Text='<%# Eval("tx_TIPOCONTRATTO")%>' Enabled="false"
                            Width="196px" />
                        <span class="slbl" style="width: 60px;">Categoria</span>
                        <asp:TextBox ID="tx_CATEGORIACONTRATTOTextBox" runat="server" Text='<%# Eval("tx_CATEGORIACONTRATTO")%>' Enabled="false"
                            Width="100px" />
                        <span class="slbl" style="width: 55px;">Fascia</span>
                        <asp:TextBox ID="tx_FASCIACONTRATTOTextBox" runat="server" Text='<%# Eval("tx_FASCIACONTRATTO")%>' Enabled="false"
                            Width="100px" />
                        <br />
                        <span class="flbl" style="width: 145px;">Note da gestione personale</span>
                        <asp:TextBox ID="tx_NOTEGESTPERSONALETextBox" runat="server" Text='<%# Eval("tx_NOTEGESTPERSONALE")%>'
                            Width="635px" Height="30px" TextMode="MultiLine" Enabled="false" />
                    </div>
                    <div style="float:left;width:252px;">
                        <span class="flbl"><b>Curriculum Vitae</b></span>
                        <br />
                        <stl:BinaryElementBox ID="id_ELEME_CVBinaryElementBox" runat="server" FieldName="id_ELEME_CV"
                            Value='<%# Bind("id_ELEME_CV")%>' DefaultCODCATEG="CUR_VIT" 
                            DefaultDescriptionSourceTextBoxID="tx_COGNOMETextBox" DefaultDescriptionPreamble="CV " />
                    </div>
                    <div class="clear">
                    </div>

                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <div style="position: absolute; left: 15px; top: 420px;">                    
        <ajaxToolkit:TabContainer ID="tabContainer" runat="server" ActiveTabIndex="0" Height="360px"
            Width="1072px">
            <ajaxToolkit:TabPanel ID="pnlPortfolio" runat="server">
                <HeaderTemplate>
                    Portfolio Formativo

                </HeaderTemplate>
                <ContentTemplate>

                    <stl:StlUpdatePanel ID="updPORTFOLIO" runat="server" Top="23px" Left="3px" Width="1064px" Height="354px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdPORTFOLIO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsPORTFOLIO_g" DeleteConfirmationQuestion="" EnableViewState="False"
                                AllowInsert="false" AllowDelete="false" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                                Title="Portfolio Formativo" DataKeyNames="ac_ITEM" AllowReselectSelectedRow="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Data/date" ItemStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# Softailor.Global.DateUtils.DataDalAl(Eval("dt_INIZIO"), Eval("dt_FINE"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="tx_TIPO" HeaderText="Tipo partecipazione" />
                                    <asp:TemplateField HeaderText="Tipo, titolo e sede evento">
                                        <ItemTemplate>
                                            <%# Eval("tx_TIPOLOGIAEVENTO")%><br />
                                            <b><%# Server.HtmlEncode(Eval("tx_TITOLOEVENTO"))%></b>
                                            <%# If(IsDBNull(Eval("tx_EDIZIONE")), "", " - " & Eval("tx_EDIZIONE"))%>
                                            <%# If(IsDBNull(Eval("tx_SEDE")), "",
                                                "<br/>" & Server.HtmlEncode(Eval("tx_SEDE")) &
                                                If(IsDBNull(Eval("tx_DETTAGLISEDE")), "", " - " & Server.HtmlEncode(Eval("tx_DETTAGLISEDE"))))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                                    <asp:TemplateField HeaderText="ECM">
                                        <ItemTemplate>
                                            <%# If(
                                                    Eval("ac_NORMATIVAECM") = "NONE",
                                                    "Evento non accreditato ECM",
                                                    If(Eval("ac_STATOECM") = "C",
                                                        "Candidato al conseguimento",
                                                        If(Eval("ac_STATOECM") = "NC",
                                                            "Non candidato al conseguimento",
                                                            If(Eval("ac_STATOECM") = "COK",
                                                               "<b>" &
                                                               If(Eval("ac_CATEGORIAECM") = "P",
                                                                  If(Eval("nd_CREDITIECM_EVENTO") = 1D, "1 credito conseguito", Eval("nd_CREDITIECM_EVENTO", "{0:#0.####}") & " crediti conseguiti"),
                                                                  If(Eval("nd_CREDITIECM_ISCRITTO") = 1D, "1 credito conseguito", Eval("nd_CREDITIECM_ISCRITTO", "{0:#0.####}") & " crediti conseguiti")
                                                               ) &
                                                               "</b>",
                                                               If(Eval("ac_STATOECM") = "CKO",
                                                                "Crediti non conseguiti",
                                                                "Stato non definito"
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                                %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stato Partecipazione">
                                        <ItemTemplate>
                                            <%# "<b style=""color:" & Eval("ac_RGB") & """>" & Eval("tx_STATOPARTECIPAZIONE") & "</b>" &
                                                If(Eval("fl_KO") = True And Not IsDBNull(Eval("tx_NOTEAVANZAMENTO")), "<br/>" & Server.HtmlEncode(Eval("tx_NOTEAVANZAMENTO")), "")
                                                %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </stl:StlGridView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlRecapiti" runat="server">
                <HeaderTemplate>
                    Recapiti
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updRECAPITI_g" runat="server" Top="23px" Left="3px" Width="1064px"
                        Height="217px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdRECAPITI" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsRECAPITI_g" DeleteConfirmationQuestion="" EnableViewState="False" BoundStlFormViewID="frmRECAPITI"
                                AllowInsert="true" AllowDelete="true" ItemDescriptionPlural="recapiti" ItemDescriptionSingular="recapito"
                                Title="Recapiti" DataKeyNames="id_RECAPITO_PERSONA">
                                <Columns>
                                    <asp:CommandField />
                                    <asp:BoundField DataField="tx_TIPORECAPITO" HeaderText="Tipo Recapito" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="tx_ENTE" HeaderText="Ente" />
                                    <asp:BoundField DataField="tx_INDIRIZZO" HeaderText="Indirizzo" HtmlEncode="false" />
                                    <asp:BoundField DataField="tx_TELEFONO" HeaderText="Telefono" />
                                    <asp:BoundField DataField="tx_FAX" HeaderText="Fax" />
                                    <asp:BoundField DataField="tx_CELLULARE" HeaderText="Cellulare" />
                                    <asp:BoundField DataField="tx_EMAIL" HeaderText="E-mail" />
                                    <asp:BoundField DataField="tx_EMAILPEC" HeaderText="E-mail PEC" />
                                </Columns>
                            </stl:StlGridView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                    <stl:StlUpdatePanel ID="updRECAPITI_f" runat="server" Top="244px" Left="3px" Width="1064px" Height="133px">
                        <ContentTemplate>
                            <stl:StlFormView runat="server" ID="frmRECAPITI" DataSourceID="sdsRECAPITI_f" NewItemText="Nuovo recapito"
                                DataKeyNames="id_RECAPITO_PERSONA" BoundStlGridViewID="grdRECAPITI">
                                <EditItemTemplate>
                                    <div style="display: block; float: left; width: 584px;">
                                        <span class="flbl" style="width: 90px;"><b>Tipo Recapito</b></span>
                                        <asp:DropDownList ID="ac_TIPORECAPITODropDownList" runat="server" SelectedValue='<%# Bind("ac_TIPORECAPITO") %>'
                                            DataSourceID="sdsac_TIPORECAPITO" DataTextField="tx_TIPORECAPITO" DataValueField="ac_TIPORECAPITO" Width="110px"
                                            AppendDataBoundItems="true" Font-Bold="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                        <span class="flbl" style="width: 10px;">&nbsp;-</span>
                                        <bof:CtlSelettoreIndirizzoConEnte runat="server" ID="ctlSelettoreIndirizzo" FieldName="tx_INDIRIZZO"
                                            FirstLabelWidthPx="90"
                                            tx_ente='<%# Bind("tx_ENTE")%>'
                                            ac_tipoindirizzo='<%# Bind("ac_TIPOINDIRIZZO") %>'
                                            tx_indirizzo='<%# Bind("tx_INDIRIZZO")%>'
                                            ac_cap_ac_comune='<%# Bind("ac_CAP_ac_COMUNE") %>'
                                            tx_localita='<%# Bind("tx_LOCALITA") %>'
                                            tx_postalcode='<%# Bind("tx_POSTALCODE") %>'
                                            tx_city='<%# Bind("tx_CITY") %>'
                                            tx_provincia='<%# Bind("tx_PROVINCIA") %>'
                                            tx_stato='<%# Bind("tx_STATO") %>'
                                            ac_nazione='<%# Bind("ac_NAZIONE") %>' />
                                    </div>
                                    <div style="display: block; float: left; width: 240px">
                                        <span class="flbl" style="width: 36px;">Tel</span>
                                        <asp:TextBox ID="tx_TELEFONOTextBox" runat="server" Text='<%# Bind("tx_TELEFONO") %>'
                                            Width="200px" />
                                        <br />
                                        <span class="flbl" style="width: 36px;">Fax</span>
                                        <asp:TextBox ID="tx_FAXTextBox" runat="server" Text='<%# Bind("tx_FAX")%>'
                                            Width="200px" />
                                        <br />
                                        <span class="flbl" style="width: 36px;">Cell</span>
                                        <asp:TextBox ID="tx_CELLULARETextBox" runat="server" Text='<%# Bind("tx_CELLULARE") %>'
                                            Width="200px" />
                                        <br />
                                        <span class="flbl" style="width: 36px;">E-mail</span>
                                        <stl:StlEmailTextBox runat="server" ID="tx_EMAILStlEmailTextBox"
                                            FieldName="tx_EMAIL" Value='<%# Bind("tx_EMAIL") %>' Width="200px" />
                                        <br />
                                        <span class="flbl" style="width: 36px;">PEC</span>
                                        <stl:StlEmailTextBox runat="server" ID="tx_EMAILPECStlEmailTextBox"
                                            FieldName="tx_EMAILPEC" Value='<%# Bind("tx_EMAILPEC") %>' Width="200px" />
                                    </div>
                                    <div style="clear: both">
                                    </div>
                                </EditItemTemplate>
                            </stl:StlFormView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlStoricoRL" runat="server">
                <HeaderTemplate>
                    Storico Rapporto Lavorativo
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updSTORICOLAVORATIVO_g" runat="server" Width="1064px" Height="354px" Top="23px" Left="3px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdSTORICOLAVORATIVO" runat="server" AddCommandText=""
                                AutoGenerateColumns="False" DataKeyNames="id_STORICOLAVORATIVO_PERSONA" DataSourceID="sdsSTORICOLAVORATIVO_g"
                                EnableViewState="False" ItemDescriptionPlural="righe" ItemDescriptionSingular="riga"
                                Title="Cronologia rapporto lavorativo con XXXX" AllowDelete="false"
                                DeleteConfirmationQuestion="">
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Dal" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="dt_FINE" HeaderText="Al" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                                    <asp:BoundField DataField="tx_RUOLO" HeaderText="Ruolo" />
                                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                                    <asp:BoundField DataField="tx_UNITAOPERATIVA" HeaderText="Unità Operativa" />
                                    <asp:BoundField DataField="tx_TIPOCONTRATTO" HeaderText="Tipo Contratto" />
                                    <asp:BoundField DataField="tx_CATEGORIACONTRATTO" HeaderText="Categoria" />
                                    <asp:BoundField DataField="tx_FASCIACONTRATTO" HeaderText="Fascia" />
                                    <asp:BoundField DataField="tx_PROFESSIONE" HeaderText="Professione ECM" />
                                    <asp:BoundField DataField="tx_DISCIPLINA" HeaderText="Disciplina ECM" />
                                </Columns>
                            </stl:StlGridView>
                            <stl:StlSqlDataSource ID="sdsSTORICOLAVORATIVO_g" runat="server"
                                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                                SelectCommand="SELECT * FROM vw_age_STORICOLAVORATIVO_PERSONE_grid WHERE id_PERSONA=@id_PERSONA ORDER BY dt_INIZIO desc, id_STORICOLAVORATIVO_PERSONA desc">
                                <SelectParameters>
                                    <asp:Parameter Name="id_PERSONA" Type="Int32" />
                                </SelectParameters>
                            </stl:StlSqlDataSource>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlRuoliAziendali" runat="server">
                <HeaderTemplate>
                    Ruoli Aziendali
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updRUOLI_g" runat="server" Top="23px" Left="3px" Width="1064px"
                        Height="260px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdRUOLI" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsRUOLI_g" DeleteConfirmationQuestion="" EnableViewState="False" BoundStlFormViewID="frmRUOLI"
                                AllowInsert="true" AllowDelete="true" ItemDescriptionPlural="ruoli" ItemDescriptionSingular="ruolo"
                                Title="Ruoli Aziendali" DataKeyNames="id_RUOLO_PERSONA">
                                <Columns>
                                    <asp:CommandField />
                                    <asp:BoundField DataField="tx_RUOLO" HeaderText="Ruolo" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="dt_INIZIO" HeaderText="Data Inizio" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="dt_FINE" HeaderText="Data Fine" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="tx_UNITAOPERATIVA" HeaderText="UO/Reparto" />
                                    <asp:BoundField DataField="tx_RIFERIMENTO" HeaderText="Riferimento" />
                                    <asp:BoundField DataField="tx_NOTE" HeaderText="Note" />
                                </Columns>
                            </stl:StlGridView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                    <stl:StlUpdatePanel ID="updRUOLI_f" runat="server" Top="287px" Left="3px" Width="1064px" Height="90px">
                        <ContentTemplate>
                            <stl:StlFormView runat="server" ID="frmRUOLI" DataSourceID="sdsRUOLI_f" NewItemText="Nuovo ruolo"
                                DataKeyNames="id_RUOLO_PERSONA" BoundStlGridViewID="grdRUOLI">
                                <EditItemTemplate>
                                    <span class="flbl" style="width: 90px;">Ruolo Aziendale</span>
                                    <asp:DropDownList ID="ac_RUOLODropDownList" runat="server" SelectedValue='<%# Bind("ac_RUOLO") %>'
                                        DataSourceID="sdsac_RUOLOCOB" DataTextField="tx_RUOLO" DataValueField="ac_RUOLO" Width="400px"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <span class="slbl" style="width: 80px;">Data di inizio</span>
                                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server"
                                        Text='<%# Bind("dt_INIZIO", "{0:dd/MM/yyyy}")%>' Width="85px" CssClass="stl_dt_data_ddmmyyyy" />
                                    <span class="slbl" style="width: 75px;">Data di fine</span>
                                    <asp:TextBox ID="dt_FINETextBox" runat="server"
                                        Text='<%# Bind("dt_FINE", "{0:dd/MM/yyyy}")%>' Width="85px" CssClass="stl_dt_data_ddmmyyyy" />
                                    <br />
                                    <span class="flbl" style="width: 90px;">UO/Reparto</span>
                                    <asp:DropDownList ID="ac_UNITAOPERATIVADropDownList" runat="server" SelectedValue='<%# Bind("ac_UNITAOPERATIVA")%>'
                                        DataSourceID="sdsac_UNITAOPERATIVA" DataTextField="tx_UNITAOPERATIVA" DataValueField="ac_UNITAOPERATIVA" Width="400px"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <br />
                                    <span class="flbl" style="width: 90px;">Riferimento</span>
                                    <asp:TextBox ID="tx_RIFERIMENTOTextBox" runat="server"
                                        Text='<%# Bind("tx_RIFERIMENTO")%>' Width="396px" />
                                    <span class="slbl" style="width: 40px;">Note</span>
                                    <asp:TextBox ID="tx_NOTETextBox" runat="server"
                                        Text='<%# Bind("tx_NOTE")%>' Width="289px" />
                                </EditItemTemplate>
                            </stl:StlFormView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlPasswordReset" runat="server">
                <HeaderTemplate>
                    Reimpostazione Password
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="updPasswordReset" runat="server" EnableViewState="false">
                        <ContentTemplate>
                            <div style="padding:10px;font-size:12px;line-height:1.5em;">
                                Per reimpostare la password della persona, fai clic su "reimposta password".<br />
                                Verrà generata una nuova password, che verrà visualizzata a video (insieme al codice utente).<br />
                                Dovrai comunicare la nuova password alla persona e raccomandare di modificare la password generata al primo accesso.
                                <br />
                                <br />
                                <asp:LinkButton ID="lnkPasswordReset" runat="server" CssClass="btnlink">Reimposta Password</asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender ID="cnfReset" runat="server" TargetControlID="lnkPasswordReset" ConfirmText="Confermi la reimpostazione della password?" />
                                <br />
                                <br />
                                <asp:TextBox ID="txtPasswordResetResult" runat="server" CssClass="txt" TextMode="MultiLine" Width="500px" Height="90px" Visible="false" ReadOnly="true" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlPortfolioDocente" runat="server">
                <HeaderTemplate>
                    Portfolio Docente
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updPORTFOLIODOCENTE" runat="server" Top="23px" Left="3px" Width="1064px"
                        Height="354px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdPORTFOLIODOCENTE" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsPORTFOLIODOCENTE_g" DeleteConfirmationQuestion="" EnableViewState="False"
                                AllowInsert="false" AllowDelete="false" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                                Title="Portfolio Docente" DataKeyNames="ac_ITEM" AllowReselectSelectedRow="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Data/date" ItemStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <%# If(IsDBNull(Eval("dt_INIZIO")), "", Softailor.Global.DateUtils.DataDalAl(Eval("dt_INIZIO"), Eval("dt_FINE")))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="tx_TIPO" HeaderText="Tipo partecipazione" />
                                    <asp:TemplateField HeaderText="Tipo, titolo e sede evento">
                                        <ItemTemplate>
                                            <%# Eval("tx_TIPOLOGIAEVENTO")%><br />
                                            <b><%# Server.HtmlEncode(Eval("tx_TITOLOEVENTO"))%></b>
                                            <%# If(IsDBNull(Eval("tx_EDIZIONE")), "", " - " & Eval("tx_EDIZIONE"))%>
                                            <%# If(IsDBNull(Eval("tx_SEDE")), "",
															"<br/>" & Server.HtmlEncode(Eval("tx_SEDE")) &
															If(IsDBNull(Eval("tx_DETTAGLISEDE")), "", " - " & Server.HtmlEncode(Eval("tx_DETTAGLISEDE"))))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
									<asp:TemplateField HeaderText="Valutazione Partecipanti">
										<ItemTemplate>
											<%# If(
                                                                                                    Eval("valutazione") Is System.DBNull.Value = True,
                                                                                                    "<div style='text-align:center'>-</div>",
                                                                                                    "<script type='text/javascript'>" &
                                                                                                    "$(function () {" &
                                                                                                    "$('#rate_" & Eval("id_EVENTO") & "').shieldRating({" &
                                                                                                    "max: 5," &
                                                                                                    "step: 0.1," &
                                                                                                    "value:   0," &
                                                                                                    "markPreset:  false" &
                                                                                                    "});" &
                                                                                                    "$('#rate_" & Eval("id_EVENTO") & "').swidget().enabled(false);" &
                                                                                                    "$('#rate_" & Eval("id_EVENTO") & "' ).swidget().value(" & Eval("valutazione").ToString().Replace(",", ".") & ");})</script>" &
                                                                                                    "<div id='rate_" & Eval("id_EVENTO") & "' class='rate'></div>" &
                                                                                                    "<div style='text-align:center'>" & Eval("valutazione").ToString().Replace(",", ".") & "/5</div>"
                                                                                                )
											%>
										</ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Valutazione Ufficio Formazione">
										<ItemTemplate>
											<%# If(
                                                                            Eval("valutazione_UO") Is System.DBNull.Value = True,
                                                                            "<div style='text-align:center'>-</div>",
                                                                            "<script type='text/javascript'>" &
                                                                            "$(function () {" &
                                                                            "$('#rate2_" & Eval("id_EVENTO") & "').shieldRating({" &
                                                                            "max: 5," &
                                                                            "step: 0.1," &
                                                                            "value:   0," &
                                                                            "markPreset:  false" &
                                                                            "});" &
                                                                            "$('#rate2_" & Eval("id_EVENTO") & "').swidget().enabled(false);" &
                                                                            "$('#rate2_" & Eval("id_EVENTO") & "' ).swidget().value(" & Eval("valutazione_UO").ToString().Replace(",", ".") & ");})</script>" &
                                                                            "<div id='rate2_" & Eval("id_EVENTO") & "' class='rate'></div>" &
                                                                            "<div style='text-align:center'>" & Eval("valutazione_UO").ToString().Replace(",", ".") & "/5</div>"
                                                                        )
											%>
										</ItemTemplate>
                                    </asp:TemplateField>


									<asp:TemplateField HeaderText="Importi">
										<ItemTemplate>
											<%# Eval("importi") & "&nbsp;&euro;"%>
										</ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ECM">
                                        <ItemTemplate>
                                            <%# If(
																Eval("ac_NORMATIVAECM") = "NONE",
																"Evento non accreditato ECM",
																If(Eval("ac_STATOECM") = "C",
																	"Candidato al conseguimento",
																	If(Eval("ac_STATOECM") = "NC",
																		"Non candidato al conseguimento",
																		If(Eval("ac_STATOECM") = "COK",
																		   "<b>" &
																		   If(Eval("ac_CATEGORIAECM") = "P",
																			  If(Eval("nd_CREDITIECM_EVENTO") = 1D, "1 credito conseguito", Eval("nd_CREDITIECM_EVENTO", "{0:#0.####}") & " crediti conseguiti"),
																			  If(Eval("nd_CREDITIECM_ISCRITTO") = 1D, "1 credito conseguito", Eval("nd_CREDITIECM_ISCRITTO", "{0:#0.####}") & " crediti conseguiti")
																		   ) &
																		   "</b>",
																		   If(Eval("ac_STATOECM") = "CKO",
																			"Crediti non conseguiti",
																			"Stato non definito"
																			)
																		)
																	)
																)
															)
                                                %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stato Partecipazione">
                                        <ItemTemplate>
                                            <%# "<b style=""color:" & Eval("ac_RGB") & """>" & Eval("tx_STATOPARTECIPAZIONE") & "</b>" &
																		If(Eval("fl_KO") = True And Not IsDBNull(Eval("tx_NOTEAVANZAMENTO")), "<br/>" & Server.HtmlEncode(Eval("tx_NOTEAVANZAMENTO")), "")
                                                %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </stl:StlGridView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>

    <stl:StlSqlDataSource ID="sdsPERSONE_f" runat="server"
        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_age_PERSONE_form WHERE id_PERSONA = @id_PERSONA"
        UpdateCommandType="StoredProcedure"
        UpdateCommand="sp_age_PERSONE_update"
        >
        <UpdateParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="ac_TITOLO" Type="String" />
            <asp:Parameter Name="tx_COGNOME" Type="String" />
            <asp:Parameter Name="tx_NOME" Type="String" />
            <asp:Parameter Name="ac_GENERE" Type="String" />
            <asp:Parameter Name="dt_NASCITA" Type="DateTime" />
            <asp:Parameter Name="ac_COMUNENASCITA" Type="String" />
            <asp:Parameter Name="ac_CODICEFISCALE" Type="String" />
            <asp:Parameter Name="tx_EMAIL" Type="String" />
            <asp:Parameter Name="ac_RUOLO" Type="String" />
            <asp:Parameter Name="ac_PROFILO" Type="String" />
            <asp:Parameter Name="ac_CATEGORIALAVORATIVA" Type="String" />
            <asp:Parameter Name="ac_PROFESSIONE" Type="String" />
            <asp:Parameter Name="id_DISCIPLINA" Type="Int32" />
            <asp:Parameter Name="id_ALBO" Type="Int32" />
            <asp:Parameter Name="ac_ISCRIZIONEALBO" Type="String" />
            <asp:Parameter Name="ac_CODICEESTERNO" Type="String" />
            <asp:Parameter Name="id_ELEME_CV" Type="Int32" />
            <asp:Parameter Name="fl_RESPONSABILE" Type="Boolean" />
        </UpdateParameters>
        <InsertParameters>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsRECAPITI_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure" SelectCommand="sp_age_RecapitiPersona"
        DeleteCommand="DELETE FROM age_RECAPITI_PERSONE WHERE id_RECAPITO_PERSONA=@id_RECAPITO_PERSONA">
        <SelectParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id_RECAPITO_PERSONA" Type="Int32" />
        </DeleteParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsPORTFOLIO_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure" SelectCommand="sp_bo_Portfolio">
        <SelectParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
            <asp:Parameter Name="tx_NOMEENTE" Type="String" />
        </SelectParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsRECAPITI_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_age_RECAPITI_PERSONE WHERE id_RECAPITO_PERSONA=@id_RECAPITO_PERSONA"
        InsertCommandType="StoredProcedure"
        InsertCommand="sp_age_add_RECAPITI_PERSONE"
        UpdateCommandType="StoredProcedure"
        UpdateCommand="sp_age_upd_RECAPITI_PERSONE">
        <SelectParameters>
            <asp:Parameter Name="id_RECAPITO_PERSONA" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="tx_ENTE" Type="String" />
            <asp:Parameter Name="ac_TIPOINDIRIZZO" Type="String" />
            <asp:Parameter Name="tx_INDIRIZZO" Type="String" />
            <asp:Parameter Name="ac_CAP_ac_COMUNE" Type="String" />
            <asp:Parameter Name="tx_POSTALCODE" Type="String" />
            <asp:Parameter Name="tx_LOCALITA" Type="String" />
            <asp:Parameter Name="tx_CITY" Type="String" />
            <asp:Parameter Name="tx_PROVINCIA" Type="String" />
            <asp:Parameter Name="tx_STATO" Type="String" />
            <asp:Parameter Name="ac_NAZIONE" Type="String" />
            <asp:Parameter Name="tx_TELEFONO" Type="String" />
            <asp:Parameter Name="tx_FAX" Type="String" />
            <asp:Parameter Name="tx_CELLULARE" Type="String" />
            <asp:Parameter Name="tx_EMAIL" Type="String" />
            <asp:Parameter Name="tx_EMAILPEC" Type="String" />
            <asp:Parameter Name="ac_TIPORECAPITO" Type="String" />
            <asp:Parameter Name="tx_CREAZIONE" Type="String" />
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
            <asp:Parameter Name="id_RECAPITO_PERSONA" Type="Int32" Direction="Output" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="id_RECAPITO_PERSONA" Type="Int32" />
            <asp:Parameter Name="tx_ENTE" Type="String" />
            <asp:Parameter Name="ac_TIPOINDIRIZZO" Type="String" />
            <asp:Parameter Name="tx_INDIRIZZO" Type="String" />
            <asp:Parameter Name="ac_CAP_ac_COMUNE" Type="String" />
            <asp:Parameter Name="tx_POSTALCODE" Type="String" />
            <asp:Parameter Name="tx_LOCALITA" Type="String" />
            <asp:Parameter Name="tx_CITY" Type="String" />
            <asp:Parameter Name="tx_PROVINCIA" Type="String" />
            <asp:Parameter Name="tx_STATO" Type="String" />
            <asp:Parameter Name="ac_NAZIONE" Type="String" />
            <asp:Parameter Name="tx_TELEFONO" Type="String" />
            <asp:Parameter Name="tx_FAX" Type="String" />
            <asp:Parameter Name="tx_CELLULARE" Type="String" />
            <asp:Parameter Name="tx_EMAIL" Type="String" />
            <asp:Parameter Name="tx_EMAILPEC" Type="String" />
            <asp:Parameter Name="ac_TIPORECAPITO" Type="String" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
        </UpdateParameters>
    </stl:StlSqlDataSource>

		<stl:StlSqlDataSource ID="sdsPORTFOLIODOCENTE_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure" SelectCommand="sp_bo_Portfolio_Docente">
        <SelectParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
            <asp:Parameter Name="tx_NOMEENTE" Type="String" />
        </SelectParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsRUOLI_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_cob_RUOLI_PERSONE WHERE id_PERSONA=@id_PERSONA ORDER BY tx_RUOLO, dt_INIZIO, id_RUOLO_PERSONA"
        DeleteCommand="DELETE FROM cob_RUOLI_PERSONE WHERE id_RUOLO_PERSONA=@id_RUOLO_PERSONA">
        <SelectParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id_RUOLO_PERSONA" Type="Int32" />
        </DeleteParameters>
    </stl:StlSqlDataSource>

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
SET     ac_RUOLO = @ac_RUOLO,
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
            <asp:Parameter Name="ac_RUOLO" Type="String" />
            <asp:Parameter Name="dt_INIZIO" Type="DateTime" />
            <asp:Parameter Name="dt_FINE" Type="DateTime" />
            <asp:Parameter Name="ac_UNITAOPERATIVA" Type="String" />
            <asp:Parameter Name="tx_RIFERIMENTO" Type="String" />
            <asp:Parameter Name="tx_NOTE" Type="String" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="id_RUOLO_PERSONA" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>

    <asp:SqlDataSource ID="sdsac_TITOLO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TITOLO, tx_TITOLO FROM age_TITOLI ORDER BY tx_TITOLO"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_CATEGORIALAVORATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_CATEGORIALAVORATIVA as ac, tx_CATEGORIALAVORATIVA as tx FROM age_CATEGORIELAVORATIVE ORDER BY ni_ORDINE"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_RUOLO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_RUOLO, tx_RUOLO FROM age_RUOLI ORDER BY tx_RUOLO"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_ALBO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT id_ALBO, tx_ALBO_SHORT FROM age_ALBI ORDER BY tx_ALBO_SHORT"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_TIPORECAPITO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPORECAPITO, tx_TIPORECAPITO FROM mbg_TIPIRECAPITO WHERE fl_PERSONA=1 ORDER BY ni_PRIORITA desc"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_RUOLOCOB" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_RUOLO, tx_RUOLO FROM cob_RUOLI ORDER BY tx_RUOLO"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_UNITAOPERATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_UNITAOPERATIVA, tx_UNITAOPERATIVA FROM vw_age_UNITAOPERATIVE ORDER BY fl_OBSOLETA, tx_UNITAOPERATIVA_sort"></asp:SqlDataSource>
    

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
