<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ReportRimborsiDocenze.aspx.vb" Inherits="Softailor.SiteTailorIzs.ReportRimborsiDocenze" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div style="font-size: 12px;">
        <div>
            <b>
                Report Rimborsi e Docenze (importi a consuntivo)
            </b>
        </div>
        <div><br /></div>
        <div>
            Anno di competenza:
            &nbsp;
            <asp:DropDownList ID="ddnAnno" runat="server" EnableViewState="false" CssClass="ddn">
                <asp:ListItem Text="2014" Value="2014" />
                <asp:ListItem Text="2015" Value="2015" />
                <asp:ListItem Text="2016" Value="2016" />
                <asp:ListItem Text="2017" Value="2017" />
                <asp:ListItem Text="2018" Value="2018" />
                <asp:ListItem Text="2019" Value="2019" />
                <asp:ListItem Text="2020" Value="2020" />
                <asp:ListItem Text="2021" Value="2021" />
                <asp:ListItem Text="2022" Value="2022" />
                <asp:ListItem Text="2023" Value="2023" />
                <asp:ListItem Text="2024" Value="2024" />
                <asp:ListItem Text="2025" Value="2025" />
            </asp:DropDownList>
        </div>
        <div>
            Periodo:
            &nbsp;
            <asp:DropDownList ID="ddnTrimestre" runat="server" EnableViewState="false" CssClass="ddn">
                <asp:ListItem Text="Intero Anno" Value="0" />
                <asp:ListItem Text="1° Trimestre" Value="1" />
                <asp:ListItem Text="2° Trimestre" Value="2" />
                <asp:ListItem Text="3° Trimestre" Value="3" />
                <asp:ListItem Text="4° Trimestre" Value="4" />
            </asp:DropDownList>
        </div>
        <div>
            Opzioni:
            &nbsp;
            <asp:DropDownList ID="ddnRipetiEvento" runat="server" EnableViewState="false" CssClass="ddn">
                <asp:ListItem Text="Visualizza i dati dell'evento solo sulla prima riga" Value="0" />
                <asp:ListItem Text="Visualizza i dati dell'evento su tutte le righe" Value="1" />
            </asp:DropDownList>
        </div>
        <div>
            <br />
        </div>
        <div>
            <asp:LinkButton ID="lnkGenera" runat="server" CssClass="btnlink">Genera Report</asp:LinkButton>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
