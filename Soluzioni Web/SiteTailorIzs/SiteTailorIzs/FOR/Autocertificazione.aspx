<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="Autocertificazione.aspx.vb" Inherits="Softailor.SiteTailorIzs.Autocertificazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">Dettagli Auto-certificazione</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
     <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="buttonsection">
                <asp:LinkButton ID="lnkClose" runat="server" CssClass="tbbtn">
                    <span class="icon close"></span>
                    Chiudi
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <stl:StlUpdatePanel ID="updPARTECIPAZIONI_f" runat="server" Width="1070px" Height="275px" Top="15px" Left="15px">
        <ContentTemplate>
            <stl:StlFormView ID="frmPARTECIPAZIONI" runat="server" DataKeyNames="id_PARTECIPAZIONE"
                DataSourceID="sdsPARTECIPAZIONI_f" NewItemText="">
                <EditItemTemplate>
                    <span class="flbl" style="width:140px">Autocertificazione numero</span>
                    <asp:TextBox ID="ni_NUMEROTextBox" runat="server" Font-Bold="true" Text='<%# Eval("ni_NUMERO") & "/" & Eval("ni_ANNO")%>'
                        Width="90px" Enabled="false" />
                    <span class="slbl" style="width:35px">del</span>
                    <asp:TextBox ID="dt_DATATextBox" runat="server" Text='<%# Eval("dt_DATA", "{0:dd/MM/yyyy}")%>'
                        Width="90px" Enabled="false" />
                    <span class="slbl" style="width:74px">Nominativo</span>
                    <asp:TextBox ID="tx_PERSONATextBox" runat="server" Font-Bold="true" Text='<%# Eval("tx_PERSONA")%>'
                        Width="460px" Enabled="false" />
                    <span class="slbl" style="width:61px">Matricola</span>
                    <asp:TextBox ID="ac_MATRICOLATextBox" runat="server" Text='<%# Eval("ac_MATRICOLA")%>'
                        Width="90px" Enabled="false" />
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width:60px">Ruolo</span>
                    <asp:TextBox ID="tx_CATEGORIAECMTextBox" runat="server" Text='<%# Eval("tx_CATEGORIAECM")%>'
                        Width="100px" Enabled="false" />
                    <span class="slbl" style="width:82px">Titolo Evento</span>
                    <asp:TextBox ID="tx_TITOLOTextBox" runat="server" Font-Bold="true" Text='<%# Eval("tx_TITOLO")%>'
                        Width="806px" Enabled="false" />
                    <br />
                    <span class="flbl" style="width:60px">Tipologia</span>
                    <asp:TextBox ID="tx_TIPOLOGIAEVENTOTextBox" runat="server" Text='<%# Eval("tx_TIPOLOGIAEVENTO")%>'
                        Width="270px" Enabled="false" />
                    <span class="slbl" style="width:45px">Sede</span>
                    <asp:TextBox ID="tx_SEDETextBox" runat="server" Text='<%# Eval("tx_SEDE")%>'
                        Width="350px" Enabled="false" />
                    <span class="slbl" style="width:70px">Data inizio</span>
                    <asp:TextBox ID="dt_INIZIOTextBox" runat="server" Font-Bold="true" Text='<%# Eval("dt_INIZIO", "{0:dd/MM/yyyy}")%>'
                        Width="90px" Enabled="false" />
                    <span class="slbl" style="width:65px">Data fine</span>
                    <asp:TextBox ID="dt_FINETextBox" runat="server" Font-Bold="true" Text='<%# Eval("dt_FINE", "{0:dd/MM/yyyy}")%>'
                        Width="90px" Enabled="false" />
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width:160px">Esame di verifica</span>
                    <asp:TextBox ID="tx_STATOVERIFICAAPPRENDIMENTOTextBox" runat="server" Text='<%# Eval("tx_STATOVERIFICAAPPRENDIMENTO")%>'
                        Width="550px" Enabled="false" />
                    <br />
                    <span class="flbl" style="width:160px">Accreditamento ECM evento</span>
                    <asp:TextBox ID="tx_NORMATIVAECMTextBox" runat="server" Text='<%# Eval("tx_NORMATIVAECM")%>'
                        Width="550px" Enabled="false" />
                    <br />
                    <span class="flbl" style="width:160px">Stato crediti persona</span>
                    <asp:TextBox ID="tx_STATOECMTextBox" runat="server" Text='<%# Eval("tx_STATOECM")%>'
                        Width="550px" Enabled="false" />
                    <br />
                    <span class="flbl" style="width:160px">Numero crediti conseguiti</span>
                    <asp:TextBox ID="nd_CREDITIECMTextBox" runat="server" Text='<%# Eval("nd_CREDITIECM", "{0:#0.##}")%>'
                        Width="90px" Enabled="false" />
                    <br />
                    <span class="flbl" style="width:160px">Data conseguimento crediti</span>
                    <asp:TextBox ID="dt_OTTENIMENTOCREDITIECMTextBox" runat="server" Font-Bold="true" Text='<%# Eval("dt_OTTENIMENTOCREDITIECM", "{0:dd/MM/yyyy}")%>'
                        Width="90px" Enabled="false" />
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width:40px">Stato</span>
                    <asp:DropDownList ID="ac_STATOPARTECIPAZIONEDropDownList" runat="server" SelectedValue='<%# Bind("ac_STATOPARTECIPAZIONE")%>'
                        DataSourceID="sdsac_STATOPARTECIPAZIONE" DataTextField="tx" DataValueField="ac"
                        Width="200px" AppendDataBoundItems="true">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <span class="slbl" style="width:40px">Note</span>
                    <asp:TextBox ID="tx_NOTEAVANZAMENTOTextBox" runat="server" Text='<%# Bind("tx_NOTEAVANZAMENTO")%>'
                        Width="772px" />
                    <div class="sep_hor"></div>
                    <span class="flbl" style="width:150px">Ultimo cambiamento stato il</span>
                    <asp:TextBox ID="dt_ULTIMOAVANZAMENTOTextBox" runat="server" Text='<%# Eval("dt_ULTIMOAVANZAMENTO", "{0:dd/MM/yyyy}")%>'
                        Width="86px" Enabled="false" />
                    <span class="slbl" style="width:74px">da parte di</span>
                    <asp:TextBox ID="tx_ULTIMOAVANZAMENTOTextBox" runat="server" Text='<%# Eval("tx_ULTIMOAVANZAMENTO")%>'
                        Width="250px" Enabled="false" />
                </EditItemTemplate>
            </stl:StlFormView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlSqlDataSource ID="sdsPARTECIPAZIONI_f" runat="server"
        ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_ext_Autocertificazioni WHERE id_PARTECIPAZIONE = @id_PARTECIPAZIONE"
        UpdateCommand="
UPDATE	ext_PARTECIPAZIONI 
SET		dt_MODIFICA=GETDATE(),
		tx_MODIFICA=@tx_MODIFICA,
		dt_ULTIMOAVANZAMENTO=CAST(GETDATE() as date),
		tx_ULTIMOAVANZAMENTO=@tx_MODIFICA,
		ac_STATOPARTECIPAZIONE=@ac_STATOPARTECIPAZIONE,
		tx_NOTEAVANZAMENTO=@tx_NOTEAVANZAMENTO
WHERE	id_PARTECIPAZIONE=@id_PARTECIPAZIONE
        "
        >
        <UpdateParameters>
            <asp:Parameter Name="id_PARTECIPAZIONE" Type="Int32" />
            <asp:Parameter Name="tx_MODIFICA" Type="String" />
            <asp:Parameter Name="ac_STATOPARTECIPAZIONE" Type="String" />
            <asp:Parameter Name="tx_NOTEAVANZAMENTO" Type="String" />
        </UpdateParameters>
        <InsertParameters>
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Name="id_PARTECIPAZIONE" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <asp:SqlDataSource ID="sdsac_STATOPARTECIPAZIONE" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT ac_STATOPARTECIPAZIONE as ac, tx_STATOPARTECIPAZIONE as tx FROM ext_STATIPARTECIPAZIONI WHERE fl_FRONTOFFICE = 1 ORDER BY ni_ORDINE"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
