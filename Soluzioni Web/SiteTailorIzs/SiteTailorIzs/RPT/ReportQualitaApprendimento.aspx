<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ReportQualitaApprendimento.aspx.vb" Inherits="Softailor.SiteTailorIzs.ReportQualitaApprendimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div style="font-size: 12px;">
        <div>
            <b>Anno di competenza:</b>
            &nbsp;
        <asp:DropDownList ID="ddnAnno" runat="server" EnableViewState="false" CssClass="ddn" Font-Bold="true" Width="60px">
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
            <br />
        </div>
        <div>
            Riepilogo valutazione qualità percepita per evento
        <asp:LinkButton ID="btnValutazioniEventi" runat="server" CssClass="btnlink">Genera</asp:LinkButton>
        </div>
        <div>
            <br />
        </div>
        <div>
            Valutazione docenti: Evento &gt; docente &gt; valutazione
            <asp:LinkButton ID="btnValutazioniEventiDocenti" runat="server" CssClass="btnlink">Genera</asp:LinkButton>
        </div>
        <div>
            <br />
        </div>
        <div>
            Valutazione docenti: Docente &gt; evento &gt; valutazione
            <asp:LinkButton ID="btnValutazioniDocentiEventi" runat="server" CssClass="btnlink">Genera</asp:LinkButton>
        </div>
        <div>
            <br />
        </div>
        <div>
            Valutazione docenti: Docente &gt; valutazione
            <asp:LinkButton ID="btnValutazioniDocenti" runat="server" CssClass="btnlink">Genera</asp:LinkButton>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
