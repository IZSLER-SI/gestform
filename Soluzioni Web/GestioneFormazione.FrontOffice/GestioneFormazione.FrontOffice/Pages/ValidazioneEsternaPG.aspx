<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="ValidazioneEsternaPG.aspx.vb" Inherits="GestioneFormazione.FrontOffice.ValidazioneEsternaPG" %>

<asp:Content ID="Content666" ContentPlaceHolderID="cphContent" runat="server">
    <asp:PlaceHolder ID="phdContent666" runat="server" EnableViewState="false" />
    
    <div style='margin-left: 20px'>    
        <p>Qui di seguito la lista degli eventi per cui è necessario confermare l'avvenuta partecipazione oppure richiedere l'annullamento</p>
        <asp:GridView ID="tabellaEventiEsterniPG" runat="server" AutoGenerateColumns="False" GridLines="None">
            <Columns>
                 <%-- Parte dedicata al titolo dell'evento (il suo nome) --%>
                 <asp:BoundField DataField="@tx_titolo" HeaderText="" />
                 <%-- Parte dedicata alle azioni dell'utente: "Conferma presenza" oppure "Richiesta annullamento" --%>
                 <asp:TemplateField HeaderText="">
                     <ItemTemplate>
                     <asp:LinkButton ID="btnConfermaEvento"  runat="server" style='margin-left: 20px;' CssClass="btnlink btnlink_green" OnClick="confermaPartecipazione_click" CommandArgument='<%# Eval("@id_partecipazione")%>' >
                        Conferma la partecipazione all'evento
                     </asp:LinkButton>
                     <asp:LinkButton ID="btnAnnullaEvento"  runat="server" style='margin-left: 10px;' CssClass="btnlink btnlink_orange" OnClick="annullaPartecipazione_click" CommandArgument='<%# Eval("@id_partecipazione")%>'>
                        Richiedo l'annullamento
                     </asp:LinkButton>
					 <br><br/>
                     </ItemTemplate>
                 </asp:TemplateField>        
            </Columns>
        </asp:GridView>
    </div>


</asp:Content>





