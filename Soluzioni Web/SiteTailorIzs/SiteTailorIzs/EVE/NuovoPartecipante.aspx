<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="NuovoPartecipante.aspx.vb" Inherits="Softailor.SiteTailorIzs.NuovoPartecipante" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <script type="text/javascript">
        <asp:Literal ID="ltrScripts" runat="server" />    
    </script>
    <script src="NuovoPartecipante.js"></script>
    <div class="singlerow">
        <asp:Label ID="lblTitle" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Annulla
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <div class="stl_dfo" style="position: absolute; left: 15px; top: 15px; width: 600px;">
        <div class="title">
            Aggiunta nominativo esistente in archivio
        </div>
    </div>
    <stl:StlUpdatePanel ID="updPERSONE_src" runat="server" Top="50px" Left="15px" Width="290px"
        Height="230px">
        <ContentTemplate>
            <%  If checkDocente() = "0" Then %>
            <stl:StlSearchForm ID="srcPERSONE" runat="server" Title="Parametri Ricerca" AllowAddNew="false"
                SearchName="Ricerca Persone (iscrizione)" DontSelectFirstRecord="true">
            </stl:StlSearchForm>
						<% ELSE %>
						<stl:StlSearchForm ID="srcDOCENTI" runat="server" Title="Parametri Ricerca" AllowAddNew="false"
										SearchName="Ricerca Docenti (iscrizione)" DontSelectFirstRecord="true">
								</stl:StlSearchForm>
						<% End If%>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updPERSONE_g" runat="server" Height="570px" Width="763px"
        Left="320px" Top="50px">
        <ContentTemplate>
            <%  If checkDocente() = "0" Then %>
						<stl:StlGridView ID="grdPERSONE" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_PERSONA" DataSourceID="sdsPERSONE_g" EnableViewState="False"
                ItemDescriptionPlural="nominativi" ItemDescriptionSingular="nominativo" Title="Risultati Ricerca (primi 300)"
                SqlStringProviderID="srcPERSONE" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="tx_UNITAOPERATIVA" HeaderText="Unità Operativa" />
                </Columns>
            </stl:StlGridView>
						<% ELSE %>
						<stl:StlGridView ID="grdDOCENTI" runat="server" AddCommandText="" AutoGenerateColumns="False"
										DataKeyNames="id_PERSONA" DataSourceID="sdsDOCENTI_g" EnableViewState="False"
										ItemDescriptionPlural="nominativi" ItemDescriptionSingular="nominativo" Title="Risultati Ricerca (primi 300)"
										SqlStringProviderID="srcDOCENTI" AllowReselectSelectedRow="true" AllowDelete="false">
										<Columns>
												<asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" ItemStyle-Font-Bold="true" />
												<asp:BoundField DataField="tx_PROFILO" HeaderText="Profilo" />
												<asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
												<asp:BoundField DataField="tx_UNITAOPERATIVA" HeaderText="Unità Operativa" />
											<asp:BoundField DataField="dt_scadenza_docente" HeaderText="Scadenza Candidatura" DataFormatString="{0:dd/MM/yyyy}"/>
										</Columns>
						</stl:StlGridView>
						<% End If%>
            <stl:StlSqlDataSource ID="sdsPERSONE_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="Dummy">
						</stl:StlSqlDataSource>
						<stl:StlSqlDataSource ID="sdsDOCENTI_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="Dummy">
            </stl:StlSqlDataSource>
            <div style="display:none;">
                <asp:LinkButton ID="lnkIscriviEsistente" runat="server">ok</asp:LinkButton>
            </div>
        </ContentTemplate>
    </stl:StlUpdatePanel>


		<%  If checkDocente() = "0" Then %>
				<div>
		<% End If%>
		<%  If checkDocente() = "1" Then %> 
				<div style="display: none">
		<% End If%>
				<div class="stl_dfo" style="position: absolute; left: 15px; top: 645px; width: 1068px;">
						<div class="title">
								Creazione nuovo nominativo <span style="font-size:15px;">(funzione da utilizzare SOLO se il nominativo non esiste già in archivio)</span>
						</div>
				</div>
				<div class="stl_gen_box" style="position: absolute; top: 680px; left: 15px; width: 1068px;">
						<asp:UpdatePanel ID="updNuovo" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
										<asp:Panel ID="pnlDatiNuovo" runat="server" DefaultButton="btnAdd" CssClass="content padall">
												<span class="flbl" style="width:45px;">Titolo</span>
												<asp:DropDownList Font-Bold="true" ID="ddnTitolo" runat="server" EnableViewState="false" CssClass="ddn" width="85px" />
												<span class="slbl" style="width:60px;">Cognome</span>
												<asp:TextBox Font-Bold="true" ID="txtCognome" runat="server" EnableViewState="false" CssClass="txt" MaxLength="100" width="260px" />
												<span class="slbl" style="width:45px;">Nome</span>
												<asp:TextBox Font-Bold="true" ID="txtNome" runat="server" EnableViewState="false" CssClass="txt" MaxLength="100" width="260px" />
												<span class="slbl" style="width:85px;">Codice Fiscale</span>
												<asp:TextBox Font-Bold="true" ID="txtCodiceFiscale" runat="server" EnableViewState="false" CssClass="txt" MaxLength="16" width="177px" />
												<br />
												<span class="flbl" style="width:45px;">Profilo</span>
												<asp:DropDownList ID="ddnProfilo" runat="server" EnableViewState="false" CssClass="ddn" width="552px" />
												<span class="slbl" style="width:120px;">Categoria Lavorativa</span>
												<asp:DropDownList ID="ddnCategoriaLavorativa" runat="server" EnableViewState="false" CssClass="ddn" width="326px" />
										</asp:Panel>
										<div class="commands">
												<asp:Button ID="btnAdd" runat="server" Text="Crea e aggiungi" CssClass="command" />
												<ajaxToolkit:ConfirmButtonExtender ID="confirmBtnAdd" runat="server" ConfirmText="Confermi la creazione del nominativo?" TargetControlID="btnAdd" />
										</div>
										<div style="display:none;">
												<asp:LinkButton ID="lnkCreaIscriviNuovo" runat="server">ok</asp:LinkButton>
										</div>
								</ContentTemplate>
						</asp:UpdatePanel>
				</div>
    </div>  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
