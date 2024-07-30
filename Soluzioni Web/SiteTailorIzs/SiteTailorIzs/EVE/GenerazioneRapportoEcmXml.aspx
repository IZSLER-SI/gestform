<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="GenerazioneRapportoEcmXml.aspx.vb" Inherits="Softailor.SiteTailorIzs.GenerazioneRapportoEcmXml" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px; width:400px;">
        <div class="title">
            Intestazione Rapporto
        </div>
    </div>
    <div class="infoDiv11" style="position: absolute; left: 787px; top: 30px; width: 170px">
        Tutti i dati, salvo il codice edizione, sono obbligatori.<br />
        Puoi temporaneamente omettere alcuni dati se non ne disponi, ma per generare il tabulato è necessario che tutti i campi siano compilati.
    </div>
    <stl:StlUpdatePanel ID="updEVENTI" runat="server" Top="30px" Left="0px" Width="777px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmEVENTI" DataSourceID="sdsEVENTI" NewItemText="Nuovo evento"
                DataKeyNames="ID_EVENTO">
                <EditItemTemplate>

                    <span class="flbl" style="width: 136px;">Ente Accreditante</span>
                    <asp:DropDownList ID="ecm2_COD_ACCRDropDownList" runat="server" SelectedValue='<%# Bind("ecm2_COD_ACCR") %>'
                        DataSourceID="sdsCOD_ACCR" DataTextField="txt" DataValueField="cod"
                        Width="624px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsCOD_ACCR" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                        SelectCommand="SELECT CODELEME as cod, DESELEME as txt FROM ut_ECMELE WHERE CODLISTA='COD_ACCR' ORDER BY CASE WHEN CODELEME='000' THEN 0 ELSE 1 END, DESELEME">
                    </asp:SqlDataSource>
                    <br />

                    <span class="flbl" style="width: 136px;">Codice Evento</span>
                    <asp:TextBox ID="ecm2_COD_EVETextBox" runat="server" Text='<%# Bind("ecm2_COD_EVE") %>' Width="150px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image6" ToolTip="Codice dell'evento, attribuito dal sistema di accreditamento ECM"
                            ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />

                    <span class="flbl" style="width: 136px;">Codice Edizione</span>
                    <asp:TextBox ID="ecm2_COD_EDITextBox" runat="server" Text='<%# Bind("ecm2_COD_EDI") %>' Width="150px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image7" ToolTip="Numero di edizione. In caso di eventi con singola edizione, inserire '1'."
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Codice Provider</span>
                    <asp:TextBox ID="ecm2_COD_ORGTextBox" runat="server" Width="150px" Text='<%# Bind("ac_ACCREDITAMENTO") %>' Enabled="false"  />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image8" ToolTip="Codice del provider dell'evento. Viene impostato in automatico in base al provider selezionato in fase di creazione dell'evento."
                        ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Date evento</span> <span class="flbl" style="width: 35px;">
                        Inizio</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server" Text='<%# Eval("dt_INIZIO", "{0:dd/MM/yyyy}")%>'
                        Width="80px" Enabled="false" />
                    <span class="slbl" style="width: 35px;">Fine</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server" Text='<%# Eval("dt_FINE", "{0:dd/MM/yyyy}")%>'
                        Width="80px" Enabled="false" />
                    <br />

                    <span class="flbl" style="width: 136px;">N. Crediti (per partecipanti)</span>
                    <asp:TextBox ID="ecm2_NUM_CREDTextBox" runat="server" Text='<%# Bind("ecm2_NUM_CRED", "{0:#0.####}") %>' Width="150px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image9" ToolTip="Numero di crediti conferiti ai partecipanti (gli eventuali crediti conferiti a docenti, tutor, relatori può essere differente)."
                            ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Durata in ore</span>
                    <asp:TextBox ID="ecm2_NUM_ORETextBox" runat="server" Text='<%# Bind("ecm2_NUM_ORE") %>' Width="150px" />
                    <span class="flbl">
                        <asp:Image runat="server" ID="Image10" CssClass="ud_tooltip" ToolTip="Durata in ore dell'evento formativo, come dichiarato in fase di accreditamento."
                            ImageUrl="~/img/icoInfo.gif" />
                    </span>
                    <br />
                    <span class="flbl" style="width: 136px;">Tipo Formazione</span>
                    <asp:TextBox ID="tx_TIPOLOGIAECMEVENTOTextBox" runat="server" Text='<%# Eval("tx_TIPOLOGIAECMEVENTO")%>' Width="150px" Enabled="false" />
                    <br />

                    <span class="flbl" style="width: 136px;">Tipo Evento</span>
                    <asp:DropDownList ID="ecm2_TIPO_EVEDropDownList" runat="server" SelectedValue='<%# Bind("ecm2_TIPO_EVE") %>'
                        DataSourceID="sdsTIPO_EVE" DataTextField="txt" DataValueField="cod"
                        Width="624px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsTIPO_EVE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                        SelectCommand="SELECT CODELEME as cod, DESELEME as txt FROM ut_ECMELE WHERE CODLISTA='TIPO_EVE' ORDER BY DESELEME">
                    </asp:SqlDataSource>
                    <br />

                    <span class="flbl" style="width: 136px;">Ambito/Obiettivo Formativo</span>
                    <asp:DropDownList ID="ecm2_COD_OBIDropDownList" runat="server" SelectedValue='<%# Bind("ecm2_COD_OBI") %>'
                        DataSourceID="sdsCOD_OBI" DataTextField="txt" DataValueField="cod"
                        Width="624px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsCOD_OBI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                        SelectCommand="SELECT CODELEME as cod, DESELEME as txt FROM ut_ECMELE WHERE CODLISTA='COD_OBI' ORDER BY DESELEME">
                    </asp:SqlDataSource>
                    <br />

                    <span class="flbl" style="width: 136px;">Tipologia Formativa</span>
                    <asp:DropDownList ID="ecm2_TFORM_EVEDropDownList" runat="server" SelectedValue='<%# Bind("ecm2_TFORM_EVE") %>'
                        DataSourceID="sdsTFORM_EVE" DataTextField="txt" DataValueField="cod"
                        Width="624px" AppendDataBoundItems="true" Enabled=<%# If(Eval("id_TIPOLOGIAECMEVENTO") <> 5, True, False) %>>
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <stl:StlSqlDataSource ID="sdsTFORM_EVE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                        SelectCommand="SELECT CODELEME as cod, DESELEME as txt FROM ut_ECMELE WHERE CODLISTA='COD_TIPOLOGIA_FORM' ORDER BY DESELEME">
                    </stl:StlSqlDataSource>
                    <br />

                    <span runat="server" class="flbl" style="width: 136px;vertical-align: top;" Visible=<%# If(Eval("id_TIPOLOGIAECMEVENTO") = 5, True, False) %>>Tipologie Formative Secondarie</span>
                    <div runat="server" style="display:inline-block;" Visible=<%# If(Eval("id_TIPOLOGIAECMEVENTO") = 5, True, False) %>>
                        <asp:CheckBoxList ID="ecm2_TFORM_EVE_SEC" runat="server" class="ecm2_TFORM_EVE_SEC" DataSourceID="sdsTFORM_EVE_SEC" 
                            DataTextField="txt" DataValueField="cod" AppendDataBoundItems="true" Enabled="False"></asp:CheckBoxList>
                    </div>
                    <br />
                   
                    <span class="flbl" style="width: 136px;">Data conseguimento crediti</span>
                    <asp:TextBox ID="ecm2_DATA_ACQTextBox" runat="server" Text='<%# Bind("ecm2_DATA_ACQ","{0:dd/MM/yyyy}") %>'
                        Width="80px" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="ecm2_DATA_ACQTextBox"
                        PopupPosition="BottomLeft" ClearTime="true" Format="dd/MM/yyyy" />
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="ecm2_DATA_ACQTextBox"
                        Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" UserDateFormat="DayMonthYear" />
                    <ajaxToolkit:MaskedEditValidator runat="server" ID="MaskedEditValidator3" ControlToValidate="ecm2_DATA_ACQTextBox"
                        ControlExtender="MaskedEditExtender3" Display="dynamic" IsValidEmpty="False" InvalidValueMessage="*" />
                    <br />
                    <br />
                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:PlaceHolder runat="server" ID="phdRiassunto" EnableViewState="false" />
    <div style="position:absolute;top:630px;left:180px;width:600px;text-align:right;font-size:15px;font-weight:bold;font-family:Arial;">
      <asp:LinkButton ID="lnkGenera" runat="server" CssClass="btnlink">Genera Rapporto ECM XML</asp:LinkButton>
    </div>

    <stl:StlSqlDataSource ID="sdsTFORM_EVE_SEC" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT t1.CODELEME as cod, t1.DESELEME as txt FROM ut_ECMELE AS t1 INNER JOIN eve_EVENTI_TIPOLOGIEECM AS t2 ON t1.CODELEME = t2.CODELEME AND t1.CODLISTA = t2.CODLISTA AND t2.id_EVENTO = @id_evento WHERE t1.CODLISTA='COD_TIPOLOGIA_FORM' ORDER BY DESELEME">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>


    <stl:StlSqlDataSource ID="sdsEVENTI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        UpdateCommand="
                    UPDATE  eve_EVENTI
                    SET     ecm2_COD_ACCR           = @ecm2_COD_ACCR,
                            ecm2_COD_EVE            = @ecm2_COD_EVE,
                            ecm2_COD_EDI            = @ecm2_COD_EDI,
                            ecm2_NUM_CRED           = @ecm2_NUM_CRED,
                            ecm2_NUM_ORE            = @ecm2_NUM_ORE,
                            ecm2_TIPO_EVE           = @ecm2_TIPO_EVE,
                            ecm2_COD_OBI            = @ecm2_COD_OBI,
                            ecm2_DATA_ACQ           = @ecm2_DATA_ACQ,
                            ecm2_TFORM_EVE          = @ecm2_TFORM_EVE,
                            dt_MODIFICA             = GETDATE(),
                            tx_MODIFICA             = @tx_MODIFICA
                    WHERE   id_EVENTO = @id_EVENTO
                "
                SelectCommand="SELECT * FROM vw_eve_EVENTI_DatiEcmXml WHERE id_EVENTO=@id_EVENTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="ecm2_COD_ACCR" Type="String" />
            <asp:Parameter Name="ecm2_COD_EVE" Type="String" />
            <asp:Parameter Name="ecm2_COD_EDI" Type="String" />
            <asp:Parameter Name="ecm2_NUM_CRED" Type="Decimal" />
            <asp:Parameter Name="ecm2_NUM_ORE" Type="Int32" />
            <asp:Parameter Name="ecm2_TIPO_EVE" Type="String" />
            <asp:Parameter Name="ecm2_COD_OBI" Type="String" />
            <asp:Parameter Name="ecm2_DATA_ACQ" Type="DateTime" />
            <asp:Parameter Name="ecm2_TFORM_EVE" Type="String" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>
    <script type="text/javascript">
        var allInputs = document.querySelectorAll("input[name*='ecm2_TFORM_EVE_SEC']");
        for (var i = 0, max = allInputs.length; i < max; i++){
            if (allInputs[i].type === 'checkbox')
                allInputs[i].checked = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
