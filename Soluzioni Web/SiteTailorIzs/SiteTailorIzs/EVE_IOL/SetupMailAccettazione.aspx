<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="SetupMailAccettazione.aspx.vb" Inherits="Softailor.SiteTailorIzs.SetupMailAccettazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .previewOuter
        {
            padding-left:2px;
            padding-top:2px;
        }
        .previewDiv
        {
            width:659px;
            height:559px;
            padding:10px;
            border:solid 1px #c0c0c0;
            background-color:#f0f0f0;
            overflow-y:scroll;
        }
    </style>
    <style type="text/css">
      .body
      {
      font-family: Arial, Helvetica, Sans-Serif;
      font-size:12px;
      background-color:#f0f0f0;
      color:#000000;
      }
      .bodyhdr
      {
      padding-bottom:10px;
      padding-top:10px;
      font-size:12px;
      text-align:left;
      }
      .bodytd
      {
      border:3px solid #0668a7;
      padding:11px;
      background-color:#ffffff;
      font-size:14px;
      line-height:18px;
      text-align:left;
      }
      .footertd
      {
      padding-top:10px;
      font-size:12px;
      line-height:15px;
      text-align:left;
      }
      .mailhead
      {
        font-size:14px;
        border:solid 1px #c0c0c0;
        padding:10px;
        background-color:#ffffff;
        margin-bottom:20px;
        text-align:left;
      }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updEVENTI" runat="server" Top="0px" Left="0px" Width="510px"
        Height="681px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmEVENTI" DataSourceID="sdsEVENTI" NewItemText="Nuovo evento"
                DataKeyNames="ID_EVENTO">
                <EditItemTemplate>
                                <div style="padding-bottom:5px;">
                                    <b>A. Mail di promemoria partecipazione</b><br />
                                    La mail viene inviata a tutti i partecipanti la cui iscrizione (on line o da back office) è stata immediatamente confermata.<br />
                                    Eventuali note/messaggi aggiuntivi da inserire in questa mail:
                                </div>
                                <div>
                                    <stl:HtmlEditor ID="ht_NOTEMAILPROMEMORIAHtmlEditor" runat="server" Width="490px" Height="105px"
                                                    ToolbarSet="Minimal" Value='<%# Bind("ht_NOTEMAILPROMEMORIA") %>' 
                                                    FieldName="ht_NOTEMAILPROMEMORIA" />
                                </div>
                                <div style="padding-bottom:5px;padding-top:5px;">
                                    <b>B. Mail di notifica accettazione</b><br />
                                    La mail viene inviata a tutti i partecipanti la cui iscrizione era in lista d'attesa ed è stata confermata.<br />
                                    Eventuali note/messaggi aggiuntivi da inserire in questa mail:
                                </div>
                                <div>
                                    <stl:HtmlEditor ID="ht_NOTEMAILACCETTAZIONEHtmlEditor" runat="server" Width="490px" Height="105px"
                                                    ToolbarSet="Minimal" Value='<%# Bind("ht_NOTEMAILACCETTAZIONE") %>' 
                                                    FieldName="ht_NOTEMAILACCETTAZIONE" />
                                </div>
                                <div style="padding-bottom:5px;padding-top:5px;">
                                    <b>C. Mail di non accettazione</b><br />
                                    La mail viene inviata a tutti i partecipanti la cui iscrizione era in lista d'attesa e non è stata confermata per esaurimento dei posti.<br />
                                    Eventuali note/messaggi aggiuntivi da inserire in questa mail:
                                </div>
                                <div>
                                    <stl:HtmlEditor ID="ht_NOTEMAILNONACCETTAZIONEHtmlEditor" runat="server" Width="490px" Height="105px"
                                                    ToolbarSet="Minimal" Value='<%# Bind("ht_NOTEMAILNONACCETTAZIONE") %>' 
                                                    FieldName="ht_NOTEMAILNONACCETTAZIONE" />
                                </div>
                                <div style="padding-bottom:5px;padding-top:5px;">
                                    <b>D. Mail promemoria docenza</b><br />
                                    La mail viene inviata a tutti i docenti / relatori / moderatori.<br />
                                    Eventuali note/messaggi aggiuntivi da inserire in questa mail:
                                </div>
                                <div>
                                    <stl:HtmlEditor ID="ht_NOTEMAILPROMEMORIADOCENZAHtmlEditor" runat="server" Width="490px" Height="105px"
                                                    ToolbarSet="Minimal" Value='<%# Bind("ht_NOTEMAILPROMEMORIADOCENZA")%>' 
                                                    FieldName="ht_NOTEMAILPROMEMORIADOCENZA" />
                                </div>
                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlSqlDataSource ID="sdsEVENTI" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        UpdateCommand="
                    UPDATE  eve_EVENTI
                    SET     ht_NOTEMAILPROMEMORIA            = @ht_NOTEMAILPROMEMORIA,
                            ht_NOTEMAILACCETTAZIONE          = @ht_NOTEMAILACCETTAZIONE,
                            ht_NOTEMAILNONACCETTAZIONE       = @ht_NOTEMAILNONACCETTAZIONE,
                            ht_NOTEMAILPROMEMORIADOCENZA     = @ht_NOTEMAILPROMEMORIADOCENZA
                    WHERE   id_EVENTO = @id_EVENTO
                "
                SelectCommand="SELECT * FROM eve_EVENTI WHERE id_EVENTO=@id_EVENTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="ht_NOTEMAILPROMEMORIA" Type="String" />
            <asp:Parameter Name="ht_NOTEMAILACCETTAZIONE" Type="String" />
            <asp:Parameter Name="ht_NOTEMAILNONACCETTAZIONE" Type="String" />
            <asp:Parameter Name="ht_NOTEMAILPROMEMORIADOCENZA" Type="String" />
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </UpdateParameters>
    </stl:StlSqlDataSource>
    <div style="position:absolute;top:0px;left:520px;">
        <ajaxToolkit:TabContainer ID="cntPreview" runat="server" ActiveTabIndex="0" Height="585px" Width="687px">
            <ajaxToolkit:TabPanel ID="previewA" runat="server">
                <HeaderTemplate>
                    Anteprima e-mail A
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="previewOuter">
                        <div class="previewDiv">
                            <asp:UpdatePanel ID="updA" runat="server" EnableViewState="false" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:PlaceHolder ID="phdA" runat="server" EnableViewState="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="previewB" runat="server">
                <HeaderTemplate>
                    Anteprima e-mail B
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="previewOuter">
                        <div class="previewDiv">
                            <asp:UpdatePanel ID="updB" runat="server" EnableViewState="false" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:PlaceHolder ID="phdB" runat="server" EnableViewState="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="previewC" runat="server">
                <HeaderTemplate>
                    Anteprima e-mail C
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="previewOuter">
                        <div class="previewDiv">
                            <asp:UpdatePanel ID="updC" runat="server" EnableViewState="false" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:PlaceHolder ID="phdC" runat="server" EnableViewState="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="previewD" runat="server">
                <HeaderTemplate>
                    Anteprima e-mail D
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="previewOuter">
                        <div class="previewDiv">
                            <asp:UpdatePanel ID="updD" runat="server" EnableViewState="false" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:PlaceHolder ID="phdD" runat="server" EnableViewState="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
    <div style="position:absolute;top:615px;left:520px;width:500px;font-size:11px;">
        <b>Testi predefiniti</b>
        <br />
        <br />
        La Formazione controllerà la <strong>presenza dei partecipanti</strong> (dipendenti e non dipendenti <asp:Literal runat="server" ID="ltrCompanyName" />) a tutti i Corsi con un sistema elettronico che utilizza come mezzo di identificazione la tessera sanitaria.<br />
        <span style="color:rgb(255, 0, 0)"><strong>Ti invitiamo pertanto a presentarti ai Corsi munito della tua tessera sanitaria.</strong></span>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
