<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="IscrizioniAttive.aspx.vb" Inherits="GestioneFormazione.FrontOffice.IscrizioniAttive" %>
<asp:Content ID="contentContent" ContentPlaceHolderID="cphContent" runat="server">
    <div class="onecol">
		<h3>
			<asp:Panel ID="pnl1" runat="server">
                <span style="font-size:18px; line-height:24px; color :#c00">ATTENZIONE:<br /> 
				    PER VISUALIZZARE <b><u>TUTTI I CORSI FAD</u></b> A CUI SEI ISCRITTO, DEVI ACCEDERE AL SEGUENTE INDIRIZZO:<br />
                    <a href="https://fad.izsler.it/" target="_blank">https://fad.izsler.it</a>
			    </span>
            </asp:Panel>
            <asp:Panel ID="pnl2" runat="server">
                <span style="font-size:18px; line-height:24px; color :#c00">ATTENZIONE:<br /> 
				    PER VISUALIZZARE <b><u>TUTTI I CORSI FAD</u></b> A CUI SEI ISCRITTO, CLICCA SU:<br />
                    <asp:LinkButton ID="login_lnk_login_sso" ClientIDMode="Static" runat="server" CssClass="btn btn-secondary btn-block">
                        ACCEDI ALLA PIATTAFORMA E-LEARNING
                    </asp:LinkButton>
			    </span>
            </asp:Panel>
            
		</h3>
		<hr />
        <div class="title green bottom20">
            Gli eventi a cui sono iscritto
        </div>


        <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
    </div>
</asp:Content>
