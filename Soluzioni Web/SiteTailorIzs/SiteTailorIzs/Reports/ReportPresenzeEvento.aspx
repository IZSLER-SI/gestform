<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ReportPresenzeEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.ReportPresenzeEvento" %>

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
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server" EnableViewState="false">
        <ContentTemplate>
            <div class="stl_gen_box">
                <div class="title">
                    Report Presenze Evento
                </div>
                <div class="content" style="padding: 6px 6px 10px 6px">
                    <div class="coldiv" style="width: 200px;">
                        <div class="searchLabel">
                            Data
                        </div>
                        <div class="clear">
                            <xsl:value-of select="''" />
                        </div>
                        <asp:RadioButtonList ID="rblData" EnableViewState="false" runat="server" CssClass="searchCbl" />
                    </div>
                    <div class="coldiv" style="width: 200px;">
                        <div class="searchLabel">
                            Dipendenti / Esterni
                        </div>
                        <div class="clear">
                            <xsl:value-of select="''" />
                        </div>
                        <asp:RadioButtonList ID="rblDipExt" EnableViewState="false" runat="server" CssClass="searchCbl">
                            <asp:ListItem Text="Dipendenti" Value="DIP" Selected="True" />
                            <asp:ListItem Text="Esterni" Value="EXT" />
                            <asp:ListItem Text="Dipendenti ed Esterni" Value="DIPEXT" />
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="commands">
                    <asp:Button ID="btnDoReport" runat="server" Text="Genera Report" CssClass="command" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDoReport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
