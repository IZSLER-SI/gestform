<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="SchedaPartecipante.aspx.vb" Inherits="Softailor.SiteTailorIzs.SchedaPartecipante" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <script src="SchedaPartecipante.js"></script>
    <div class="tworows_1" style="display:block;width:400px;white-space:nowrap;overflow:hidden;">
        <asp:Label ID="lblTitolo1" runat="server" EnableViewState="false" />
    </div>
    <div class="tworows_2" style="display:block;width:400px;white-space:nowrap;overflow:hidden;">
        <asp:Label ID="lblTitolo2" runat="server" EnableViewState="false" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Conditional">
        <Triggers>
		    <asp:PostBackTrigger ControlID="lnkLetteraIncarico" />
            <asp:PostBackTrigger ControlID="lnkAttestatoECM" />
            <asp:PostBackTrigger ControlID="lnkAttestatoPART" />
        </Triggers>
        <ContentTemplate>
		    <div class="buttonsection" id="sezioneIncarico" runat="server">
			    <asp:LinkButton ID="lnkLetteraIncarico" runat="server" CssClass="tbbtn">
                    <span class="icon print"></span>
                    Lettera Incarico
                </asp:LinkButton>
		    </div>
            <div class="buttonsection" id="sezioneAttEcm" runat="server">
								
                <asp:LinkButton ID="lnkAttestatoECM" runat="server" CssClass="tbbtn">
                    <span class="icon print"></span>
                    Att.ECM
                </asp:LinkButton>
                <asp:LinkButton ID="lnkAttestatoPART" runat="server" CssClass="tbbtn">
                    <span class="icon print"></span>
                    Att.Partecipazione
                </asp:LinkButton>
            </div>
            <div class="buttonsection">
                <asp:LinkButton ID="lnkClose" runat="server" CssClass="tbbtn">
                    <span class="icon close"></span>
                    Chiudi
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updISCRITTI_f" runat="server" Top="10px" Left="15px" Width="848px" Height="430px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmISCRITTI" DataSourceID="sdsISCRITTI" NewItemText="Nuovo iscritto"
                DataKeyNames="id_ISCRITTO">
                <EditItemTemplate>
                    <div>
                        <span class="flbl" style="width: 220px;"><b>Dati Anagrafici</b></span>
                    </div>
                    <asp:Panel ID="pnlLock" runat="server" Visible='<%#isDipendente%>' CssClass="infoDiv11_small" style="margin-right:6px;margin-bottom:2px;">
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
                    <span class="flbl" style="width: 400px;"><b>Ruolo, profilo e dati lavorativi</b></span>
                    <div class="infoDiv11_small" style="margin-right:6px;margin-bottom:2px;">
                        Eventuali modifiche a questi dati non saranno riportate in automatico nelle anagrafiche generali.
                    </div>
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
                    <span class="flbl" style="width: 90px;">Matricola</span>
                    <asp:TextBox ID="ac_MATRICOLATextBox" runat="server" Text='<%# Eval("ac_MATRICOLA") %>' Enabled="false"
                        Width="80px" />
                    <span class="slbl" style="width: 100px;">Unità Operativa</span>
                    <asp:TextBox ID="tx_UNITAOPERATIVATextBox" runat="server" Text='<%# Eval("tx_UNITAOPERATIVA")%>' Enabled="false"
                        Width="556px" />
                    <br />
                    <span class="flbl" style="width: 90px;">Ordine/Ass.Prof.</span>
                    <asp:DropDownList ID="id_ALBODropDownList" runat="server" SelectedValue='<%# Bind("id_ALBO")%>'
                        DataSourceID="sdsid_ALBO" DataTextField="tx_ALBO_SHORT" DataValueField="id_ALBO" Width="431px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 85px;">N° Iscrizione</span>
                    <asp:TextBox ID="ac_ISCRIZIONEALBOTextBox" runat="server" Text='<%# Bind("ac_ISCRIZIONEALBO")%>'
                        Width="50px" />
                    <span class="slbl" style="width: 90px;">Codice Esterno</span>
                    <asp:TextBox ID="ac_CODICEESTERNOTextBox" runat="server" Text='<%# Bind("ac_CODICEESTERNO")%>'
                        Width="80px" />
                    <br />
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 406px;"><b>Dettagli Iscrizione</b></span>
                    <span class="flbl" style="width: 100px;">Data/Ora iscrizione</span>
                    <asp:TextBox ID="dt_CREAZIONETextBox" runat="server" Text='<%# Eval("dt_CREAZIONE","{0:dd/MM/yy HH:mm:ss}") %>'
                        Width="125px" Enabled="false" />
                    <span class="slbl" style="width: 95px;">Origine iscrizione</span>
                    <asp:TextBox ID="tx_ORIGINEISCRIZIONETextBox" runat="server" Text='<%# Eval("tx_ORIGINEISCRIZIONE") %>'
                        Width="100px" Enabled="false" /><br />
                    <span class="flbl" style="width: 90px;">Categoria</span>
                    <asp:DropDownList ID="ac_CATEGORIAECMDropDownList" runat="server" SelectedValue='<%# Bind("ac_CATEGORIAECM") %>' 
                        DataSourceID="sdsac_CATEGORIAECM" DataTextField="tx" DataValueField="ac" Width="290px" Font-Bold="true" 
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 90px;">Stato Iscrizione</span>
                    <asp:DropDownList ID="ac_STATOISCRIZIONEDropDownList" runat="server" SelectedValue='<%# Bind("ac_STATOISCRIZIONE") %>'
                        DataSourceID="sdsac_STATOISCRIZIONE" DataTextField="tx" DataValueField="ac" Width="364px" Font-Bold="true" 
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <br />
                    <span class="flbl" style="width: 90px;">Questionario</span>
                    <asp:DropDownList ID="ac_STATOQUESTIONARIODropDownList" runat="server" SelectedValue='<%# Eval("ac_STATOQUESTIONARIO") %>'
                        DataSourceID="sdsac_STATOQUESTIONARIO" DataTextField="tx" DataValueField="ac" Width="290px" Font-Bold="true" 
                        AppendDataBoundItems="true" Enabled="false">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 90px;">Risposte esatte</span>
                    <asp:TextBox ID="ni_RISPOSTEOKTextBox" runat="server" Text='<%# Eval("ni_RISPOSTEOK") %>'
                        Width="50px" Enabled="false" />
                    <span class="slbl" style="width: 92px;">Risposte errate</span>
                    <asp:TextBox ID="ni_RISPOSTEKOTextBox" runat="server" Text='<%# Eval("ni_RISPOSTEKO") %>'
                        Width="50px" Enabled="false" />
                    <span class="slbl" style="width: 110px;">Risposte non date</span>
                    <asp:TextBox ID="ni_RISPOSTENDTextBox" runat="server" Text='<%# Eval("ni_RISPOSTEND") %>'
                        Width="50px" Enabled="false" />
                    <br />
                    <span class="flbl" style="width: 90px;">Stato ECM</span>
                    <asp:DropDownList ID="ac_STATOECMDropDownList" runat="server" SelectedValue='<%# Bind("ac_STATOECM") %>'
                        DataSourceID="sdsac_STATOECM" DataTextField="tx" DataValueField="ac" Width="290px" Font-Bold="true" 
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width: 90px;">Numero crediti</span>
                    <asp:TextBox ID="nd_CREDITIECMTextBox" runat="server" Text='<%# Bind("nd_CREDITIECM","{0:#0.####}") %>' 
                        Width="50px" />
                    <asp:Image runat="server" ID="info1" style="vertical-align:top;margin-top:3px;margin-left:3px;" ToolTip="Immetti il numero di crediti conseguiti SOLO nel caso di docente, tutor, relatore. Docenti, tutor e relatori possono conseguire un numero di crediti ECM proporzionali alla durata delle relazioni."
                        ImageUrl="~/img/icoInfo.gif" />
                    <span class="slbl" style="width: 207px;">Data ottenimento crediti (solo per FAD)</span>
                    <asp:TextBox ID="dt_OTTENIMENTOCREDITIECMTextBox" runat="server" Text='<%# Bind("dt_OTTENIMENTOCREDITIECM","{0:dd/MM/yyyy}") %>'
                        Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 85px;"><b>Incar.Docenza</b></span>
                    <span class="slbl" style="width: 75px">N° Protocollo</span>
                    <asp:TextBox ID="ac_PROTOCOLLOTextBox" runat="server"
                        Text='<%# Bind("ac_PROTOCOLLO")%>' Width="60px" />
                    <span class="slbl" style="width: 85px">Data Protocollo</span>
                    <asp:TextBox ID="dt_PROTOCOLLOTextBox" runat="server"
                        Text='<%# Bind("dt_PROTOCOLLO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                    
                    <span class="flbl" style="width: 50px;">&nbsp;</span>
                    <span class="flbl" style="width: 120px;"><b>Eventuale Sponsor</b></span>
                    <asp:TextBox ID="tx_NOMESPONSORTextBox" runat="server"
                        Text='<%# Bind("tx_NOMESPONSOR")%>' Width="267px" />
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 85px;"><b>Valutazione</b></span>
                    <span class="slbl" style="width: 110px;">Ufficio Formazione</span>
                    <asp:TextBox ID="ni_VALUTAZIONE_UO" runat="server" MaxLength="1" class="txt" Text='<%# Bind("ni_VALUTAZIONE_UO", "{0:#0.####}") %>' 
                        Width="30px" Enabled='<%# Eval("ac_CATEGORIAECM") <> "P" %>'/>
                    <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_VALUTAZIONE_UO" runat="server"
                      TargetControlID="ni_VALUTAZIONE_UO"
                      FilterType="Custom"
                      ValidChars="12345 " />
					<span class="slbl">&nbsp;<i>(Inserire un punteggio da 1 a 5)</i></span>
                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    
    <div style="position: absolute; left: 15px; top: 450px;">
        <ajaxToolkit:TabContainer ID="tabContainer" runat="server" ActiveTabIndex="0" Height="300px"
            Width="850px">
            <ajaxToolkit:TabPanel ID="pnlRecapiti" runat="server">
                <HeaderTemplate>
                    Recapiti
                </HeaderTemplate>
                <ContentTemplate>
                     <stl:StlUpdatePanel ID="updRecapiti_g" runat="server" Top="23px" Left="3px" Width="842px"
                        Height="157px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdRecapiti" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsRecapiti_g" DeleteConfirmationQuestion="" EnableViewState="False" BoundStlFormViewID="frmRecapiti"
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
                    <stl:StlUpdatePanel ID="updRecapiti_f" runat="server" Top="184px" Left="3px" Width="842px" Height="133px">
                       <ContentTemplate>
                            <stl:StlFormView runat="server" ID="frmRecapiti" DataSourceID="sdsRecapiti_f" NewItemText="Nuovo recapito"
                                DataKeyNames="id_RECAPITO_PERSONA" BoundStlGridViewID="grdRecapiti">
                                <EditItemTemplate>
                                    <div style="display:block;float:left;width:584px;">
                                        <span class="flbl" style="width:90px;"><b>Tipo Recapito</b></span>
                                        <asp:DropDownList ID="ac_TIPORECAPITODropDownList" runat="server" SelectedValue='<%# Bind("ac_TIPORECAPITO") %>'
                                            DataSourceID="sdsac_TIPORECAPITO" DataTextField="tx_TIPORECAPITO" DataValueField="ac_TIPORECAPITO" Width="110px"
                                            AppendDataBoundItems="true" Font-Bold="true">
                                            <asp:ListItem Text="" Value="" />
                                        </asp:DropDownList>
                                        <span class="flbl" style="width:10px;">&nbsp;-</span>
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
                                    <div style="display:block;float:left;width:240px">
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
                                    <div style="clear:both">
                                    </div>
                                </EditItemTemplate>
                            </stl:StlFormView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlQuote" runat="server">
                <HeaderTemplate>
                    Quote di iscrizione
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updQUOTE_g" runat="server" Width="842px" Height="178px" Top="23px" Left="3px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdQUOTE" runat="server" AddCommandText=""
                                AutoGenerateColumns="False" DataKeyNames="id_QUOTAISCRIZIONE_ISCRITTO" DataSourceID="sdsQUOTE_g"
                                EnableViewState="False" ItemDescriptionPlural="quote" ItemDescriptionSingular="quota"
                                Title="Quote di iscrizione" BoundStlFormViewID="frmQUOTE"
                                DeleteConfirmationQuestion="">
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                                    <asp:BoundField DataField="ac_QUOTAISCRIZIONE" HeaderText="Codice" />
                                    <asp:BoundField DataField="tx_QUOTAISCRIZIONE" HeaderText="Descrizione Quota" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="mo_IMPONIBILE" HeaderText="Imponibile €" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="mo_ALIQUOTA" HeaderText="IVA %" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="mo_PREZZOUN" HeaderText="Costo IVA inclusa €" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="mo_VERSATO" HeaderText="Versato €" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="dt_PAGAMENTO" HeaderText="Data Pag" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="tx_TIPOPAG" HeaderText="Modalità Pagamento" />
                                </Columns>
                            </stl:StlGridView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                    <stl:StlUpdatePanel ID="updQUOTE_f" runat="server" Width="842px" Height="112px" Top="205px" Left="3px">
                        <ContentTemplate>
                            <stl:StlFormView ID="frmQUOTE" runat="server" DataKeyNames="id_QUOTAISCRIZIONE_ISCRITTO"
                                DataSourceID="sdsQUOTE_f" NewItemText="" BoundStlGridViewID="grdQUOTE">
                                <EditItemTemplate>
                                    <span class="flbl" style="width:100px">Quota di iscrizione</span>
                                    <asp:DropDownList ID="id_QUOTAISCRIZIONEDropDownList" runat="server" SelectedValue='<%# Bind("id_QUOTAISCRIZIONE")%>'
                                        DataSourceID="sdsid_QUOTAISCRIZIONE" DataTextField="tx_QUOTAISCRIZIONE" DataValueField="id_QUOTAISCRIZIONE" Width="728px"
                                        AppendDataBoundItems="true" ClientIDMode="Static">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <br />
                                    <span class="flbl" style="width:100px">Imponibile €</span>
                                    <asp:TextBox ID="txtImponibile" runat="server" Width="60px" CssClass="txt" Enabled="false" Text='<%# Eval("mo_IMPONIBILE", "{0:#0.00}")%>' ClientIDMode="Static" />
                                    <span class="slbl" style="width:30px">IVA</span>
                                    <asp:DropDownList ID="id_IVADropDownList" runat="server" SelectedValue='<%# Bind("id_IVA") %>'
                                        DataSourceID="sdsid_IVA" DataTextField="tx_IVA" DataValueField="id_IVA" Width="242px" ClientIDMode="Static"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <span class="slbl" style="width:110px">Costo IVA inclusa €</span>
                                    <asp:TextBox ID="txtCostoTotale" runat="server" Width="60px" CssClass="txt" Enabled="false" Text='<%# Eval("mo_PREZZOUN", "{0:#0.00}")%>' ClientIDMode="Static" Font-Bold="true" />
                                    <br />
                                    <span class="flbl" style="width:100px">Versato €</span>
                                    <asp:TextBox ID="mo_VERSATOTextBox" runat="server" Width="60px" Text='<%# Bind("mo_VERSATO", "{0:#0.00}")%>' ClientIDMode="Static" Font-Bold="true" />
                                    <span class="slbl" style="width:100px">Data Pagamento</span>
                                    <asp:TextBox ID="dt_PAGAMENTOTextBox" runat="server"
                                        Text='<%# Bind("dt_PAGAMENTO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                                    <span class="slbl" style="width:110px">Modalità Pagamento</span>
                                    <asp:DropDownList ID="ac_TIPOPAGDropDownList" runat="server" SelectedValue='<%# Bind("ac_TIPOPAG")%>'
                                        DataSourceID="sdsac_TIPOPAG" DataTextField="tx_TIPOPAG" DataValueField="ac_TIPOPAG" Width="152px"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <br />
                                    <span class="flbl" style="width:100px">Note</span>
                                    <asp:TextBox ID="tx_NOTEQUOTATextBox" runat="server" Width="724px" CssClass="txt" Text='<%# Bind("tx_NOTEQUOTA")%>' />
                                </EditItemTemplate>
                            </stl:StlFormView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlVociSpesa" runat="server">
                <HeaderTemplate>
                    Spese / Ricavi
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updCOSTIRICAVI_ISCRITTI_g" runat="server" Width="842px" Height="177px" Top="23px" Left="3px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdCOSTIRICAVI_ISCRITTI" runat="server" AddCommandText=""
                                AutoGenerateColumns="False" DataKeyNames="id_COSTORICAVO_ISCRITTO" DataSourceID="sdsCOSTIRICAVI_ISCRITTI_g"
                                EnableViewState="False" ItemDescriptionPlural="elementi" ItemDescriptionSingular="elemento"
                                Title="Spese / ricavi" BoundStlFormViewID="frmCOSTIRICAVI_ISCRITTI"
                                DeleteConfirmationQuestion="">
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                                    <asp:BoundField DataField="tx_VOCESPESARICAVO" HeaderText="Tipologia" />
                                    <asp:BoundField DataField="tx_DESCRIZIONE" HeaderText="Descrizione" />
                                    <asp:BoundField DataField="tx_DELIBERA" HeaderText="Delibera" />
                                    <asp:BoundField DataField="mo_IMPORTOPREVISTO" HeaderText="Importo Previsto" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="mo_CONSUNTIVO" HeaderText="Importo a consuntivo" DataFormatString="{0:#0.00}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="dt_PAGAMENTO" HeaderText="Data Pag" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="tx_TIPOPAG" HeaderText="Modalità Pagamento" />
                                </Columns>
                            </stl:StlGridView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                    <stl:StlUpdatePanel ID="updCOSTIRICAVI_ISCRITTI_f" runat="server" Top="204px" Left="3px" Width="842px" Height="113px">
                       <ContentTemplate>
                            <stl:StlFormView runat="server" ID="frmCOSTIRICAVI_ISCRITTI" DataSourceID="sdsCOSTIRICAVI_ISCRITTI_f"
                                DataKeyNames="id_COSTORICAVO_ISCRITTO" BoundStlGridViewID="grdCOSTIRICAVI_ISCRITTI">
                                <EditItemTemplate>
                                    <span class="flbl" style="width:100px">Tipologia</span>
                                    <asp:DropDownList ID="ac_VOCESPESARICAVODropDownList" runat="server" SelectedValue='<%# Bind("ac_VOCESPESARICAVO")%>'
                                        DataSourceID="sdsac_VOCESPESARICAVO" DataTextField="tx_VOCESPESARICAVO" DataValueField="ac_VOCESPESARICAVO" Width="270px"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <span class="slbl" style="width:75px">Descrizione</span>
                                    <asp:TextBox ID="tx_DESCRIZIONETextBox" runat="server"
                                        Text='<%# Bind("tx_DESCRIZIONE")%>' Width="379px" />
                                    <br />
                                    <span class="flbl" style="width:100px">Delibera</span>
                                    <asp:DropDownList ID="id_DELIBERADropDownList" runat="server" SelectedValue='<%# Bind("id_DELIBERA")%>'
                                        DataSourceID="sdsid_DELIBERA" DataTextField="tx_DELIBERA" DataValueField="id_DELIBERA" Width="728px"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <br />
                                    <span class="flbl" style="width:100px">Importo Previsto €</span>
                                    <asp:TextBox ID="mo_IMPORTOPREVISTOTextBox" runat="server" Width="80px" Text='<%# Bind("mo_IMPORTOPREVISTO", "{0:#0.00}")%>' />
                                    <span class="slbl" style="width:80px">Consuntivo €</span>
                                    <asp:TextBox ID="mo_CONSUNTIVOTextBox" runat="server" Width="80px" Text='<%# Bind("mo_CONSUNTIVO", "{0:#0.00}")%>' />
                                    <span class="slbl" style="width:98px">Data Pagamento</span>
                                    <asp:TextBox ID="dt_PAGAMENTOTextBox" runat="server"
                                        Text='<%# Bind("dt_PAGAMENTO", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                                    <span class="slbl" style="width:113px">Modalità Pagamento</span>
                                    <asp:DropDownList ID="ac_TIPOPAGDropDownList" runat="server" SelectedValue='<%# Bind("ac_TIPOPAG")%>'
                                        DataSourceID="sdsac_TIPOPAG" DataTextField="tx_TIPOPAG" DataValueField="ac_TIPOPAG" Width="185px"
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>

                                    <br />

                                    <span class="flbl" style="width: 100px">N° Ordine</span>
                                    <asp:TextBox ID="ac_ORDINETextBox" runat="server"
                                        Text='<%# Bind("ac_ORDINE")%>' Width="45px" />
                                    <span class="slbl" style="width: 73px">Data Ordine</span>
                                    <asp:TextBox ID="dt_ORDINETextBox" runat="server"
                                        Text='<%# Bind("dt_ORDINE", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />
                
                    
                                    <span class="slbl" style="width: 52px">N° Bolla</span>
                                    <asp:TextBox ID="ac_DDTTextBox" runat="server"
                                        Text='<%# Bind("ac_DDT")%>' Width="45px" />
                                    <span class="slbl" style="width: 64px">Data Bolla</span>
                                    <asp:TextBox ID="dt_DDTTextBox" runat="server"
                                        Text='<%# Bind("dt_DDT", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />


                                    <span class="slbl" style="width: 64px">N° Fattura</span>
                                    <asp:TextBox ID="ac_FATTURATextBox" runat="server"
                                        Text='<%# Bind("ac_FATTURA")%>' Width="45px" />
                                    <span class="slbl" style="width: 76px">Data Fattura</span>
                                    <asp:TextBox ID="dt_FATTURATextBox" runat="server"
                                        Text='<%# Bind("dt_FATTURA", "{0:dd/MM/yyyy}")%>' Width="80px" CssClass="stl_dt_data_ddmmyyyy" />

                                </EditItemTemplate>
                            </stl:StlFormView>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlInOut" runat="server">
                <HeaderTemplate>
                    Cronologia ingressi/uscite
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updACCESSIISCRITTI_g" runat="server" Top="23px" Left="3px" Width="410px"
                        Height="241px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdACCESSIISCRITTI" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsACCESSIISCRITTI_g" DeleteConfirmationQuestion="" EnableViewState="False"
                                AllowInsert="True" AllowDelete="True" ItemDescriptionPlural="accessi" ItemDescriptionSingular="accesso"
                                Title="Cronologia ingressi/uscite" DataKeyNames="id_ACCESSOISCRITTO" BoundStlFormViewID="frmACCESSIISCRITTI">
                                <Columns>
                                    <asp:CommandField />
                                    <asp:BoundField DataField="dt_DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="tm_INIZIO" HeaderText="Ora ingresso" DataFormatString="{0:HH:mm}" />
                                    <asp:BoundField DataField="tm_FINE" HeaderText="Ora uscita" DataFormatString="{0:HH:mm}" />
                                </Columns>
                            </stl:StlGridView>
                            <stl:StlSqlDataSource ID="sdsACCESSIISCRITTI_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                                SelectCommandType="Text" 
                                SelectCommand="SELECT id_ACCESSOISCRITTO, id_ISCRITTO, CAST(dt_DATA as datetime) as dt_DATA, CAST(tm_INIZIO as datetime) as tm_INIZIO, CAST(tm_FINE as datetime) as tm_FINE FROM eve_ACCESSIISCRITTI WHERE id_ISCRITTO=@id_ISCRITTO ORDER BY dt_DATA, tm_INIZIO"
                                DeleteCommand="DELETE FROM eve_ACCESSIISCRITTI WHERE id_ACCESSOISCRITTO=@id_ACCESSOISCRITTO">
                                <SelectParameters>
                                    <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="id_ACCESSOISCRITTO" Type="Int32" />
                                </DeleteParameters>
                            </stl:StlSqlDataSource>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                    <stl:StlUpdatePanel ID="updACCESSIISCRITTI_f" runat="server" Top="268px" Left="3px" Width="410px" Height="49px">
                       <ContentTemplate>
                            <stl:StlFormView runat="server" ID="frmACCESSIISCRITTI" DataSourceID="sdsACCESSIISCRITTI_f" NewItemText="Nuova riga"
                                DataKeyNames="id_ACCESSOISCRITTO" BoundStlGridViewID="grdACCESSIISCRITTI">
                                <EditItemTemplate>
                                    <span class="flbl" style="width: 37px;">Data</span>
                                    <asp:TextBox ID="dt_DATATextBox" runat="server" Text='<%# Bind("dt_DATA","{0:dd/MM/yyyy}") %>'
                                        Width="80px" />
                                    <ajaxToolkit:MaskedEditExtender ID="medDATA" runat="server" TargetControlID="dt_DATATextBox"
                                        Mask="99/99/9999" MaskType="Date" />
                                    <span class="slbl" style="width: 74px;">Ora ingresso</span>
                                    <asp:TextBox ID="tm_INIZIOTextBox" runat="server" Text='<%# Bind("tm_INIZIO", "{0:HH:mm:ss}")%>'
                                        Width="65px" />
                                    <ajaxToolkit:MaskedEditExtender ID="medINIZIO" runat="server" TargetControlID="tm_INIZIOTextBox"
                                        Mask="99:99:99" MaskType="Time" />
                                    <span class="slbl" style="width: 65px;">Ora uscita</span>
                                    <asp:TextBox ID="tm_FINETextBox" runat="server" Text='<%# Bind("tm_FINE", "{0:HH:mm:ss}")%>'
                                        Width="65px" />
                                    <ajaxToolkit:MaskedEditExtender ID="medFINET" runat="server" TargetControlID="tm_FINETextBox"
                                        Mask="99:99:99" MaskType="Time" />
                                </EditItemTemplate>
                            </stl:StlFormView>
                            <stl:StlSqlDataSource ID="sdsACCESSIISCRITTI_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                                
                                SelectCommand="SELECT
                                id_ACCESSOISCRITTO, id_ISCRITTO, CAST(dt_DATA as datetime) as dt_DATA, CAST(tm_INIZIO as datetime) as tm_INIZIO, CAST(tm_FINE as datetime) as tm_FINE
                                
                                FROM eve_ACCESSIISCRITTI WHERE id_ACCESSOISCRITTO=@id_ACCESSOISCRITTO"                                
                                InsertCommand="INSERT INTO eve_ACCESSIISCRITTI (id_ISCRITTO, dt_DATA, tm_INIZIO, tm_FINE)
                                                VALUES (@id_ISCRITTO, @dt_DATA, @tm_INIZIO, @tm_FINE); SELECT @id_ACCESSOISCRITTO = CAST(SCOPE_IDENTITY() as int)"
                                UpdateCommand="UPDATE   eve_ACCESSIISCRITTI
                                               SET      dt_DATA=@dt_DATA,
                                                        tm_INIZIO=@tm_INIZIO,
                                                        tm_FINE=@tm_FINE
                                               WHERE    id_ACCESSOISCRITTO=@id_ACCESSOISCRITTO"
                                >
                                <SelectParameters>
                                    <asp:Parameter Name="id_ACCESSOISCRITTO" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
                                    <asp:Parameter Name="dt_DATA" Type="DateTime" />
                                    <asp:Parameter Name="tm_INIZIO" Type="DateTime" />
                                    <asp:Parameter Name="tm_FINE" Type="DateTime" />
                                    <asp:Parameter Name="id_ACCESSOISCRITTO" Type="Int32" Direction="Output" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="dt_DATA" Type="DateTime" />
                                    <asp:Parameter Name="tm_INIZIO" Type="DateTime" />
                                    <asp:Parameter Name="tm_FINE" Type="DateTime" />
                                    <asp:Parameter Name="id_ACCESSOISCRITTO" Type="Int32" />
                                </UpdateParameters>
                            </stl:StlSqlDataSource>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                    <asp:UpdatePanel ID="updDettaglioTempi" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="phdDettaglioTempi" runat="server" EnableViewState="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="pnlStoricoModifiche" runat="server">
                <HeaderTemplate>
                    Storico Modifiche
                </HeaderTemplate>
                <ContentTemplate>
                    <stl:StlUpdatePanel ID="updStorico" runat="server" Top="23px" Left="3px" Width="842px"
                        Height="294px">
                        <ContentTemplate>
                            <stl:StlGridView ID="grdStorico" runat="server" AddCommandText="" AutoGenerateColumns="False"
                                DataSourceID="sdsStorico" DeleteConfirmationQuestion="" EnableViewState="False"
                                AllowInsert="False" AllowDelete="false" ItemDescriptionPlural="modifiche" ItemDescriptionSingular="modifica"
                                Title="Storico Modifiche" DataKeyNames="id_LOGISCRITTO">
                                <Columns>
                                    <asp:BoundField />
                                    <asp:BoundField DataField="dt_OPERAZIONE" HeaderText="Data/Ora" DataFormatString="{0:dd/MM/yy HH:mm:ss}" />
                                    <asp:BoundField DataField="tx_OPERAZIONE" HeaderText="Utente" />
                                   <asp:TemplateField HeaderText="Categoria">
                                          <ItemTemplate>
                                            <span style="color:<%# Eval("ac_RGBCATEGORIAECM") %>"><%# Eval("tx_CATEGORIAECM")%></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stato iscrizione">
                                        <ItemTemplate>
                                            <span style="color:<%# Eval("ac_RGBSTATOISCRIZIONE") %>"><%# Eval("tx_STATOISCRIZIONE")%></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stato ECM">
                                        <ItemTemplate>
                                            <span style="color:<%# Eval("ac_RGBSTATOECM") %>"><%# Eval("tx_STATOECM")%></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </stl:StlGridView>
                            <stl:StlSqlDataSource ID="sdsStorico" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                                SelectCommandType="StoredProcedure" SelectCommand="sp_eve_LogModificheIscrizione">
                                <SelectParameters>
                                    <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
                                </SelectParameters>
                            </stl:StlSqlDataSource>
                        </ContentTemplate>
                    </stl:StlUpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>

    <stl:StlSqlDataSource ID="sdsISCRITTI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
                        SELECT  * 
                        FROM    vw_eve_ISCRITTI 
                        WHERE   id_ISCRITTO=@id_ISCRITTO AND id_EVENTO=@id_EVENTO"
        UpdateCommand="sp_eve_UpdatePersonaEIscritto" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
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
            
            <asp:Parameter Name="ac_CATEGORIAECM" Type="String" />
            <asp:Parameter Name="ac_STATOISCRIZIONE" Type="String" />
            <asp:Parameter Name="ac_STATOECM" Type="String" />

            <asp:Parameter Name="nd_CREDITIECM" Type="Decimal" />
            <asp:Parameter Name="dt_OTTENIMENTOCREDITIECM" Type="DateTime" />
            <asp:Parameter Name="tx_NOMESPONSOR" Type="String" />

            <asp:Parameter Name="id_ALBO" Type="Int32" />
            <asp:Parameter Name="ac_ISCRIZIONEALBO" Type="String" />
            <asp:Parameter Name="ac_CODICEESTERNO" Type="String" />

            <asp:Parameter Name="ac_PROTOCOLLO" Type="String" />
            <asp:Parameter Name="dt_PROTOCOLLO" Type="DateTime" />

            <asp:Parameter Name="ni_VALUTAZIONE_UO" Type="Decimal" />
           
            
        </UpdateParameters>
    </stl:StlSqlDataSource>
    
    <stl:StlSqlDataSource ID="sdsQUOTE_g" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="DELETE FROM eve_QUOTEISCRIZIONE_ISCRITTI WHERE id_QUOTAISCRIZIONE_ISCRITTO = @id_QUOTAISCRIZIONE_ISCRITTO"
                SelectCommand="SELECT * FROM vw_eve_QUOTEISCRIZIONE_ISCRITTI_grid WHERE id_ISCRITTO = @id_ISCRITTO ORDER BY id_QUOTAISCRIZIONE_ISCRITTO">
                <DeleteParameters>
                    <asp:Parameter Name="id_QUOTAISCRIZIONE_ISCRITTO" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
    
    <stl:StlSqlDataSource ID="sdsQUOTE_f" runat="server"
                ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                InsertCommand="
INSERT INTO eve_QUOTEISCRIZIONE_ISCRITTI (
    dt_CREAZIONE, 
    tx_CREAZIONE,
    id_ISCRITTO,
    id_QUOTAISCRIZIONE,
    id_IVA,
    mo_VERSATO,
    dt_PAGAMENTO,
    ac_TIPOPAG,
    tx_NOTEQUOTA 
) VALUES (
    GETDATE(),
    @tx_CREAZIONE,
    @id_ISCRITTO,
    @id_QUOTAISCRIZIONE,
    @id_IVA,
    @mo_VERSATO,
    @dt_PAGAMENTO,
    @ac_TIPOPAG,
    @tx_NOTEQUOTA 
);
SET @id_QUOTAISCRIZIONE_ISCRITTO = SCOPE_IDENTITY()                
                "
                SelectCommand="SELECT * FROM vw_eve_QUOTEISCRIZIONE_ISCRITTI_grid WHERE id_QUOTAISCRIZIONE_ISCRITTO=@id_QUOTAISCRIZIONE_ISCRITTO"
                UpdateCommand="
UPDATE  eve_QUOTEISCRIZIONE_ISCRITTI
SET     dt_MODIFICA = GETDATE(),
        tx_MODIFICA = @tx_MODIFICA,
        id_QUOTAISCRIZIONE = @id_QUOTAISCRIZIONE,
        id_IVA = @id_IVA,
        mo_VERSATO = @mo_VERSATO,
        dt_PAGAMENTO = @dt_PAGAMENTO,
        ac_TIPOPAG = @ac_TIPOPAG,
        tx_NOTEQUOTA = @tx_NOTEQUOTA 
WHERE   id_QUOTAISCRIZIONE_ISCRITTO = @id_QUOTAISCRIZIONE_ISCRITTO
                ">
                <UpdateParameters>
                    <asp:Parameter Name="tx_MODIFICA" Type="String" />
                    <asp:Parameter Name="id_QUOTAISCRIZIONE" Type="String" />
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                    <asp:Parameter Name="mo_VERSATO" Type="Decimal" />
                    <asp:Parameter Name="dt_PAGAMENTO" Type="DateTime" />
                    <asp:Parameter Name="ac_TIPOPAG" Type="String" />
                    <asp:Parameter Name="tx_NOTEQUOTA" Type="String" />
                    <asp:Parameter Name="id_QUOTAISCRIZIONE_ISCRITTO" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_QUOTAISCRIZIONE_ISCRITTO" Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="tx_CREAZIONE" Type="String" />
                    <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
                    <asp:Parameter Name="id_QUOTAISCRIZIONE" Type="String" />
                    <asp:Parameter Name="id_IVA" Type="Int32" />
                    <asp:Parameter Name="mo_VERSATO" Type="Decimal" />
                    <asp:Parameter Name="dt_PAGAMENTO" Type="DateTime" />
                    <asp:Parameter Name="ac_TIPOPAG" Type="String" />
                    <asp:Parameter Name="tx_NOTEQUOTA" Type="String" />
                    <asp:Parameter Name="id_QUOTAISCRIZIONE_ISCRITTO" Type="Int32" Direction="Output" />
                </InsertParameters>
            </stl:StlSqlDataSource>
    
    <stl:StlSqlDataSource ID="sdsRecapiti_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommandType="StoredProcedure" SelectCommand="sp_age_RecapitiPersona"
        DeleteCommand="DELETE FROM age_RECAPITI_PERSONE WHERE id_RECAPITO_PERSONA=@id_RECAPITO_PERSONA">
        <SelectParameters>
            <asp:Parameter Name="id_PERSONA" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id_RECAPITO_PERSONA" Type="Int32" />
        </DeleteParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsRECAPITI_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_age_RECAPITI_PERSONE WHERE id_RECAPITO_PERSONA=@id_RECAPITO_PERSONA"
                                
        InsertCommandType="StoredProcedure"
        InsertCommand="sp_age_add_RECAPITI_PERSONE"
        UpdateCommandType="StoredProcedure"
        UpdateCommand="sp_age_upd_RECAPITI_PERSONE"
        >
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

    <stl:StlSqlDataSource ID="sdsCOSTIRICAVI_ISCRITTI_g" runat="server"
        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        DeleteCommand="DELETE FROM eve_COSTIRICAVI_ISCRITTI WHERE id_COSTORICAVO_ISCRITTO = @id_COSTORICAVO_ISCRITTO"
        SelectCommand="SELECT * FROM vw_eve_SPESE_ISCRITTI_grid WHERE id_ISCRITTO = @id_ISCRITTO ORDER BY id_COSTORICAVO_ISCRITTO">
        <DeleteParameters>
            <asp:Parameter Name="id_COSTORICAVO_ISCRITTO" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>

    <stl:StlSqlDataSource ID="sdsCOSTIRICAVI_ISCRITTI_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM eve_COSTIRICAVI_ISCRITTI WHERE id_COSTORICAVO_ISCRITTO=@id_COSTORICAVO_ISCRITTO"
        InsertCommand="
INSERT INTO eve_COSTIRICAVI_ISCRITTI (
	dt_CREAZIONE,
	tx_CREAZIONE,
	id_ISCRITTO,
	id_DELIBERA,
	ac_VOCESPESARICAVO,
	ac_TIPOPAG,
	ac_SPESARICAVO,
	tx_DESCRIZIONE,
	mo_IMPORTOPREVISTO,
	mo_CONSUNTIVO,
	dt_PAGAMENTO,
    ac_ORDINE,
	dt_ORDINE,
	ac_DDT,
	dt_DDT,
	ac_FATTURA,
	dt_FATTURA
) VALUES (
	GETDATE(),
	@tx_CREAZIONE,
	@id_ISCRITTO,
	@id_DELIBERA,
	@ac_VOCESPESARICAVO,
	@ac_TIPOPAG,
	(SELECT ac_SPESARICAVO FROM age_VOCISPESARICAVO WHERE ac_VOCESPESARICAVO=@ac_VOCESPESARICAVO),
	@tx_DESCRIZIONE,
	@mo_IMPORTOPREVISTO,
	@mo_CONSUNTIVO,
	@dt_PAGAMENTO,
    @ac_ORDINE,
	@dt_ORDINE,
	@ac_DDT,
	@dt_DDT,
	@ac_FATTURA,
	@dt_FATTURA
)
SELECT @id_COSTORICAVO_ISCRITTO = SCOPE_IDENTITY()
        "
        UpdateCommand="
UPDATE  eve_COSTIRICAVI_ISCRITTI
SET     dt_MODIFICA = GETDATE(),
	    tx_MODIFICA = @tx_MODIFICA,
	    id_DELIBERA = @id_DELIBERA,
	    ac_VOCESPESARICAVO = @ac_VOCESPESARICAVO,
	    ac_TIPOPAG = @ac_TIPOPAG,
        ac_SPESARICAVO = (SELECT ac_SPESARICAVO FROM age_VOCISPESARICAVO WHERE ac_VOCESPESARICAVO=@ac_VOCESPESARICAVO),
	    tx_DESCRIZIONE = @tx_DESCRIZIONE,
	    mo_IMPORTOPREVISTO = @mo_IMPORTOPREVISTO,
	    mo_CONSUNTIVO = @mo_CONSUNTIVO,
	    dt_PAGAMENTO = @dt_PAGAMENTO,
        ac_ORDINE = @ac_ORDINE,
	    dt_ORDINE = @dt_ORDINE,
	    ac_DDT = @ac_DDT,
	    dt_DDT = @dt_DDT,
	    ac_FATTURA = @ac_FATTURA,
	    dt_FATTURA = @dt_FATTURA
WHERE   id_COSTORICAVO_ISCRITTO = @id_COSTORICAVO_ISCRITTO       
        "
        >
        <SelectParameters>
            <asp:Parameter Name="id_COSTORICAVO_ISCRITTO" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="tx_CREAZIONE" Type="String" />
            <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
            <asp:Parameter Name="id_DELIBERA" Type="Int32" />
            <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
            <asp:Parameter Name="ac_TIPOPAG" Type="String" />
            <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
            <asp:Parameter Name="mo_IMPORTOPREVISTO" Type="Decimal" />
            <asp:Parameter Name="mo_CONSUNTIVO" Type="Decimal" />
            <asp:Parameter Name="dt_PAGAMENTO" Type="DateTime" />
            <asp:Parameter Name="ac_ORDINE" Type="String" />
	        <asp:Parameter Name="dt_ORDINE" Type="DateTime" />
	        <asp:Parameter Name="ac_DDT" Type="String" />
	        <asp:Parameter Name="dt_DDT" Type="DateTime" />
	        <asp:Parameter Name="ac_FATTURA" Type="String" />
	        <asp:Parameter Name="dt_FATTURA" Type="DateTime" />
	        <asp:Parameter Name="id_COSTORICAVO_ISCRITTO" Type="Int32" Direction="Output" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="id_DELIBERA" Type="Int32" />
            <asp:Parameter Name="ac_VOCESPESARICAVO" Type="String" />
            <asp:Parameter Name="ac_TIPOPAG" Type="String" />
            <asp:Parameter Name="tx_DESCRIZIONE" Type="String" />
            <asp:Parameter Name="mo_IMPORTOPREVISTO" Type="Decimal" />
            <asp:Parameter Name="mo_CONSUNTIVO" Type="Decimal" />
            <asp:Parameter Name="dt_PAGAMENTO" Type="DateTime" />
            <asp:Parameter Name="ac_ORDINE" Type="String" />
	        <asp:Parameter Name="dt_ORDINE" Type="DateTime" />
	        <asp:Parameter Name="ac_DDT" Type="String" />
	        <asp:Parameter Name="dt_DDT" Type="DateTime" />
	        <asp:Parameter Name="ac_FATTURA" Type="String" />
	        <asp:Parameter Name="dt_FATTURA" Type="DateTime" />
            <asp:Parameter Name="id_COSTORICAVO_ISCRITTO" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>

    <asp:SqlDataSource ID="sdsac_CATEGORIAECM" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_CATEGORIAECM as ac, tx_CATEGORIAECM as tx FROM eve_CATEGORIEECM ORDER BY ni_ORDINE">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_STATOISCRIZIONE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_STATOISCRIZIONE as ac, tx_STATOISCRIZIONE as tx FROM eve_STATIISCRIZIONE ORDER BY ni_ORDINE">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_STATOECM" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_STATOECM as ac, tx_STATOECM as tx FROM eve_STATIECM ORDER BY ni_ORDINE">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_STATOQUESTIONARIO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_STATOQUESTIONARIO as ac, tx_STATOQUESTIONARIO as tx FROM eve_STATIQUESTIONARIO ORDER BY ni_ORDINE">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_CATEGORIALAVORATIVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_CATEGORIALAVORATIVA as ac, tx_CATEGORIALAVORATIVA as tx FROM age_CATEGORIELAVORATIVE ORDER BY ni_ORDINE">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_TITOLO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TITOLO, tx_TITOLO FROM age_TITOLI ORDER BY tx_TITOLO">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_QUOTAISCRIZIONE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT id_QUOTAISCRIZIONE, tx_QUOTAISCRIZIONE FROM vw_eve_QUOTEISCRIZIONE_ddn WHERE id_EVENTO=@id_EVENTO ORDER BY tx_QUOTAISCRIZIONE">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_IVA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT id_IVA, tx_IVA FROM age_IVA ORDER BY tx_IVA">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_TIPOPAG" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPOPAG, tx_TIPOPAG FROM age_TIPIPAG ORDER BY tx_TIPOPAG">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_TIPORECAPITO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_TIPORECAPITO, tx_TIPORECAPITO FROM mbg_TIPIRECAPITO WHERE fl_PERSONA=1 ORDER BY ni_PRIORITA desc">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_DELIBERA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
SELECT		D.id_DELIBERA,
			D.tx_DELIBERA + ' (' + CONVERT(nvarchar(10), D.dt_DATA, 103) + ')' as tx_DELIBERA
FROM		age_DELIBERE D
			INNER JOIN age_TIPOLOGIEDELIBERE TD ON D.ac_TIPOLOGIADELIBERA = TD.ac_TIPOLOGIADELIBERA 
WHERE		TD.fl_SPESERELATORE = 1
ORDER BY	D.dt_DATA desc
        ">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsac_VOCESPESARICAVO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_VOCESPESARICAVO, tx_VOCESPESARICAVO + CASE ac_SPESARICAVO WHEN 'S' THEN ' (spesa)' ELSE ' (ricavo)' END AS tx_VOCESPESARICAVO FROM age_VOCISPESARICAVO WHERE fl_RELATORE=1 ORDER BY ac_SPESARICAVO desc, tx_VOCESPESARICAVO">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsid_ALBO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT id_ALBO, tx_ALBO_SHORT FROM age_ALBI ORDER BY tx_ALBO_SHORT">
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
