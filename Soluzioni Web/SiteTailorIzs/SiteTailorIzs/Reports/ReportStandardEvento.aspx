<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ReportStandardEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.ReportStandardEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .searchLabel
        {
            padding-top: 3px;
            padding-bottom: 1px;
            float: left;
            font-weight: bold;
            color: #333333;
        }
        .searchCmds
        {
            padding-top: 3px;
            padding-bottom: 1px;
            float: right;
        }
        .clear
        {
            clear: both;
        }
        .searchCbl
        {
            font-size: 11px;
            border: solid 1px #b0b0b0;
            background-color: #ffffff;
            width: 100%;
        }
        .searchCbl input
        {
            
        }
        .coldiv
        {
            display: block;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <ajaxToolkit:TabContainer runat="server" ID="TabStrip" Width="957px" Height="550px">
        <ajaxToolkit:TabPanel ID="pnlTabulatoIscritti" runat="server" HeaderText="Tabulato Nominativi">
            <ContentTemplate>
                <asp:UpdatePanel ID="updTI" runat="server" EnableViewState="false">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phdTabulatoIscritti" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnTIDoReport" />
                    </Triggers>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="pnlFogliFirme" runat="server" HeaderText="Fogli Firme">
            <ContentTemplate>
                <asp:UpdatePanel ID="updFF" runat="server" EnableViewState="false">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phdFoglioFirme" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnFFDoReport" />
                    </Triggers>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="pnlBadge" runat="server" HeaderText="Badge">
            <ContentTemplate>
                <asp:UpdatePanel ID="updBA" runat="server" EnableViewState="false">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="PhdBadges" runat="server" EnableViewState="false" />
                        <asp:Panel ID="Panel2" runat="server" CssClass="ud_info" style="margin:0px 17px 0px 15px">
                            I <b>badge</b> vengono prodotti in formato <b>Adobe PDF</b> e devono essere stampati sui moduli 2 x 5 indicati da Fnovi ConServizi.<br />
                            <br />
                            Puoi <b>filtrare i nominativi</b> intervenendo mediante il box "eventuali filtri". Ad esempio puoi decidere di escludere gli iscritti all'Ordine organizzatore
                            qualora questi siano dotati di tesserino di riconoscimento.<br />
                            <br />
                            Il sistema <b>esclude automaticamente</b> gli <b>annullati</b> (cioè gli iscritti che hanno annullato la propria iscrizione) ed i <b>non accettati</b>.
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnBADoReport" />
                        <asp:PostBackTrigger ControlID="btnBADoReportBC" />
                    </Triggers>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="pnlAttestatiEcm" runat="server" HeaderText="Attestati ECM">
            <ContentTemplate>
                <asp:UpdatePanel ID="updAE" runat="server" EnableViewState="false">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phdAttestatiEcm" runat="server" EnableViewState="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAEDoReport" />
                    </Triggers>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
