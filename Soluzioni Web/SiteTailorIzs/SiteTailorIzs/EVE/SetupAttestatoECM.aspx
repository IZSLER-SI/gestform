<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SetupAttestatoECM.aspx.vb" Inherits="Softailor.SiteTailorIzs.SetupAttestatoECM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px;width:500px">
        <div class="title">
            Dati Attestato ECM
        </div>
    </div>
    <div style="position:absolute;top:30px;left:787px;width:230px;font-size:15px;font-weight:bold;font-family:Arial;">
        <asp:LinkButton ID="lnkAttestatoTest" runat="server" CssClass="btnlink">
            Anteprima (partecipante)
        </asp:LinkButton>
        <br />
        <br />
        <asp:LinkButton ID="lnkAttestatoTestDocente" runat="server" CssClass="btnlink">
            Anteprima (docente)
        </asp:LinkButton>
    </div>
    <div class="infoDiv11" style="position: absolute; left: 787px; top: 130px; width: 147px">
        Per poter <b>stampare correttamente</b> gli attestati ECM compila <b>tutti i campi richiesti</b> nel form a lato.<br /><br />
        <b>Fai molta attenzione al formato</b> da utilizzare per i vari campi; passa con il mouse sull'icona "i" per ulteriori informazioni.<br /><br />
        Mediante il pulsante <b>"Anteprima attestato"</b> puoi avere un'anteprima del formato degli attestati.
    </div>

    <stl:StlUpdatePanel ID="updEVENTI" runat="server" Top="30px" Left="0px" Width="777px"
        Height="630px">


        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmEVENTI" DataSourceID="sdsEVENTI" NewItemText="Nuovo evento"
                DataKeyNames="ID_EVENTO">
                <EditItemTemplate>

                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">Premesso che la Commissione Nazionale per la Formazione Continua ha accreditato il Provider</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Nome Provider</span>
                    
                    <asp:TextBox ID="tx_PROVIDERECM_ATTTextBox" runat="server" Text='<%# Eval("tx_PROVIDERECM_ATT")%>'
                        Width="604px" Enabled="false" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image4" ToolTip="Nome del provider (selezionato in fase di creazione dell'evento)"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Dati accreditamento</span>
                    <span class="flbl" style="width: 10px;">(</span>
                    <asp:TextBox ID="tx_DATIACCREDITAMENTOPROVIDERTextBox" runat="server" Text='<%# Eval("tx_DATIACCREDITAMENTOPROVIDER")%>'
                        Width="584px" Enabled="false" />
                    <span class="flbl" style="width: 10px;">&nbsp;)</span>
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image2" ToolTip="Numero ed eventuale scadenza dell'accreditamento (dati censiti nell'anagrafica dei provider ECM)"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">Premesso che</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Nome Organizzatore</span>
                    <asp:TextBox ID="tx_ORGANIZZATORE_ATTTextBox" runat="server" Text='<%# Eval("tx_ORGANIZZATORE_ATT")%>'
                        Width="604px" Enabled="false" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image1" ToolTip="Nome dell'organizzatore dell'evento, preceduto dall'articolo (dato censito nell'anagrafica degli organizzatori)"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />

                    <span class="flbl" style="width: 136px;">Eventuale collaborazione</span>
                    <asp:TextBox ID="ecm2_NOMECOLLABORAZIONETextBox" runat="server" Text='<%# Bind("ecm2_NOMECOLLABORAZIONE") %>'
                        Width="604px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image5" ToolTip="Se l'evento è stato organizzato in collaborazione con un altro soggetto, inserisci un testo analogo al seguente esempio: 'in collaborazione con xxxxxxxx'"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">ha organizzato l'evento formativo n°</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Codice Evento</span>
                    <asp:TextBox ID="ecm2_COD_EVETextBox" runat="server" Text='<%# Bind("ecm2_COD_EVE") %>' Width="150px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image6" ToolTip="Codice dell'evento, attribuito dal sistema di accreditamento ECM"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">edizione n°</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Codice Edizione</span>
                    <asp:TextBox ID="ecm2_COD_EDITextBox" runat="server" Text='<%# Bind("ecm2_COD_EDI") %>' Width="150px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image7" ToolTip="Numero di edizione. In caso di eventi con singola edizione, inserire '1'."
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">denominato</span>
                    <br />

                    <span class="flbl" style="width: 136px;">Denominazione dell'evento</span>
                    <asp:TextBox ID="ecm2_TITOLOEVENTOTextBox" runat="server" Text='<%# Bind("ecm2_TITOLOEVENTO") %>'
                        Width="604px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image8" ToolTip="Titolo dell'evento come inserito in fase di accreditamento ECM. Questo titolo può differire dal titolo inserito in fase di creazione dell'evento."
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Sede e date</span>
                    <asp:TextBox ID="ecm2_TENUTOSIADALALTextBox" runat="server" Text='<%# Bind("ecm2_TENUTOSIADALAL") %>'
                        Width="604px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image9" ToolTip="In caso di eventi NON FAD, inserisci la città e le date di svolgimento dell'evento, preceduta dal testo 'e tenutosi', esattamente come nel seguente esempio: 'e tenutosi a Milano dal 5 Gennaio al 7 Gennaio 2012'"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">avente come obiettivi didattico/formativi generali:</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Obiettivi</span>
                    <asp:DropDownList ID="ecm2_COD_OBIDropDownList" runat="server" SelectedValue='<%# Bind("ecm2_COD_OBI") %>'
                        DataSourceID="sdsCOD_OBI" DataTextField="txt" DataValueField="cod"
                        Width="608px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsCOD_OBI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                        SelectCommand="SELECT CODELEME as cod, DESELEME as txt FROM ut_ECMELE WHERE CODLISTA='COD_OBI' ORDER BY DESELEME">
                    </asp:SqlDataSource>
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image10" Style="vertical-align:top;" ToolTip="Seleziona un valore dall'elenco."
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">assegnando all'evento stesso</span>
                    <br />
                    <span class="flbl" style="width: 136px;">Crediti in cifre e lettere</span>
                    <span class="flbl" style="width: 15px;">N°</span>
                    <asp:TextBox ID="ecm2_NUM_CREDTextBox" runat="server" Text='<%# Bind("ecm2_NUM_CRED", "{0:#0.####}") %>' Width="60px" />
                    <span class="slbl" style="width: 15px;">(</span>
                    <asp:TextBox ID="ecm2_CREDITILETTERETextBox" runat="server" Text='<%# Bind("ecm2_CREDITILETTERE") %>' Width="200px" />
                    
                    <span class="flbl">&nbsp;) Crediti Formativi E.C.M.</span>
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image12" ToolTip="Numero di crediti (in cifre nel primo campo, in lettere nel secondo) conferiti ai partecipanti (gli eventuali crediti conferiti a docenti, tutor, relatori può essere differente)."
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 136px;">Nome del responsabile</span>
                    <asp:TextBox ID="tx_NOMERLPROVIDER_SOTTOSCRITTOTextBox" runat="server" Text='<%# Eval("tx_NOMERLPROVIDER_SOTTOSCRITTO")%>'
                        Width="604px" Enabled="false" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image11" ToolTip="Titolo e nome del rappresentante legale del provider, selezionato in fase di creazione dell'evento"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Qualifica del responsabile</span>
                    <asp:TextBox ID="tx_QUALIFICARLPROVIDERTextBox" runat="server" Text='<%# Eval("tx_QUALIFICARLPROVIDER")%>'
                        Width="604px" Enabled="false" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image13" ToolTip="Qualifica del rappresentante legale del provider, selezionato in fase di creazione dell'evento"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">ATTESTA</span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">che il Dr. XXX YYY, iscritto a... n...., nato a XXX il XX/XX/XXXX, in qualità di XXXXX, ha conseguito:</span>
                    <br />
                    <span class="flbl" style="width: 136px;">&nbsp;</span>
                    <span class="flbl" style="width: 600px;font-style:italic">N° XXX (XXXXX) Crediti formativi per l'anno XXXX</span>
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 136px;">Luogo</span>
                    <asp:TextBox ID="ecm2_LUOGODATAFIRMATextBox" runat="server" Text='<%# Bind("ecm2_LUOGODATAFIRMA") %>'
                        Width="604px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image18" ToolTip="Luogo di emissione dell'attestato, seguito da virgola. Es: 'Brescia,'" ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <div class="sep_hor">
                    </div>
                    <span class="flbl" style="width: 136px;">Qualif. firmatario Provider</span>
                    <asp:TextBox ID="tx_QUALIFICARLPROVIDER_ILTextBox" runat="server" Text='<%# Eval("tx_QUALIFICARLPROVIDER_IL")%>'
                        Width="604px" Enabled="false" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image15" ToolTip="Qualifica della persona che firma a nome del provider, selezionata in fase di creazione dell'evento"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Nome firmatario Provider</span>
                    <asp:TextBox ID="tx_NOMERLPROVIDERTextBox" runat="server" Text='<%# Eval("tx_NOMERLPROVIDER")%>'
                        Width="604px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image16" ToolTip="Titolo, cognome e nome della persona che firma a nome del provider, selezionata in fase di creazione dell'evento"
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlSqlDataSource ID="sdsEVENTI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        UpdateCommand="
UPDATE  eve_EVENTI
SET     ecm2_NOMECOLLABORAZIONE                 =@ecm2_NOMECOLLABORAZIONE,
        ecm2_COD_EVE                            =@ecm2_COD_EVE,
        ecm2_COD_EDI                            =@ecm2_COD_EDI,
        ecm2_TITOLOEVENTO                       =@ecm2_TITOLOEVENTO,
        ecm2_TENUTOSIADALAL                     =@ecm2_TENUTOSIADALAL,
        ecm2_COD_OBI                            =@ecm2_COD_OBI,
        ecm2_NUM_CRED                           =@ecm2_NUM_CRED,
        ecm2_CREDITILETTERE                     =@ecm2_CREDITILETTERE,
        ecm2_LUOGODATAFIRMA                     =@ecm2_LUOGODATAFIRMA,

        dt_MODIFICA=GETDATE(),
        tx_MODIFICA=@tx_MODIFICA

WHERE   id_EVENTO=@id_evento
        "
        SelectCommand="SELECT * FROM vw_eve_EVENTI_AttestatoECM WHERE id_EVENTO=@id_EVENTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="ecm2_NOMECOLLABORAZIONE" Type="String" />
            <asp:Parameter Name="ecm2_COD_EVE" Type="String" />
            <asp:Parameter Name="ecm2_COD_EDI" Type="String" />
            <asp:Parameter Name="ecm2_TITOLOEVENTO" Type="String" />
            <asp:Parameter Name="ecm2_TENUTOSIADALAL" Type="String" />
            <asp:Parameter Name="ecm2_COD_OBI" Type="String" />
            <asp:Parameter Name="ecm2_NUM_CRED" Type="Decimal" />
            <asp:Parameter Name="ecm2_CREDITILETTERE" Type="String" />
            <asp:Parameter Name="ecm2_LUOGODATAFIRMA" Type="String" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>
    <asp:SqlDataSource ID="sdsecm2_id_ELEME_PROVIDER" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
SELECT
	E.ID_ELEME,
	E.DESEL_TX
FROM
	bd_ELEMEN E
	INNER JOIN bd_CATEGO C ON E.ID_CATEG=C.ID_CATEG
WHERE
	E.ID_AZIEN=@id_azien AND
	C.CODCATEG='LOG_PRO'
ORDER BY
	E.DESEL_TX
        ">
        <SelectParameters>
            <asp:Parameter Name="id_AZIEN" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsecm2_id_ELEME_FIRMA" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
SELECT
	E.ID_ELEME,
	E.DESEL_TX
FROM
	bd_ELEMEN E
	INNER JOIN bd_CATEGO C ON E.ID_CATEG=C.ID_CATEG
WHERE
	E.ID_AZIEN=@id_azien AND
	C.CODCATEG='FIR_ATT'
ORDER BY
	E.DESEL_TX
        ">
        <SelectParameters>
            <asp:Parameter Name="id_AZIEN" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sdsecm2_id_ELEME_FIRMA2" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
SELECT
	E.ID_ELEME,
	E.DESEL_TX
FROM
	bd_ELEMEN E
	INNER JOIN bd_CATEGO C ON E.ID_CATEG=C.ID_CATEG
	INNER JOIN eve_FIRME_ORGANIZZATORI FO ON E.id_ELEME=FO.id_ELEME
	INNER JOIN eve_ORGANIZZATORI O ON FO.id_ORGANIZZATORE=O.id_ORGANIZZATORE
	INNER JOIN eve_EVENTI EV ON EV.id_ORGANIZZATORE=O.id_ORGANIZZATORE
WHERE
	E.ID_AZIEN=@id_azien AND
	C.CODCATEG='FIR_AT2' AND
	EV.id_EVENTO=@id_evento
ORDER BY
	E.DESEL_TX
        ">
        <SelectParameters>
            <asp:Parameter Name="id_AZIEN" Type="Int32" />
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
