<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="PartecipantiEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.PartecipantiEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
     <style type="text/css">
        .searchLabel
        {
            padding-top:2px;
            padding-bottom:2px;
            float:left;
            font-weight:bold;
            color:#333333;
            line-height:12px;
        }
        .searchLabelCtl
        {
            padding-top:5px;
            padding-bottom:0px;
            float:left;
            font-weight:bold;
            color:#333333;
            line-height:12px;
        }
        .searchCmds
        {
            padding-top:2px;
            padding-bottom:2px;
            float:right;
            line-height:12px;
        }
        .searchCbl
        {
            display:block;
            font-size:11px;
            border:solid 1px #b0b0b0;
            background-color:#ffffff;
            line-height:17px;
        }
        .searchCbl input
        {
            margin:2px 3px 0px 3px;
            padding:0px 0px 0px 0px;
            vertical-align:top;
        }
    </style>
    <script type="text/javascript">
        <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
    <script type="text/javascript">
        function aggiungiPartecipante(tipo){
            stl_sel_display_wh('NuovoPartecipante.aspx?t=' + tipo,1100,800,addIscritto_callback);
        }
        function schedaPartecipante(id)
        {
            if (stl_appb_row2Select_action==null)
            {
                stl_sel_display_wh('SchedaPartecipante.aspx?id=' + id, 880, 780, editIscritto_callback);
            }
            else
            {
                if (stl_appb_row2Select_action!='Delete')
                {
                    stl_sel_display_wh('SchedaPartecipante.aspx?id=' + id, 880, 780, editIscritto_callback);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div style="display:none;">
        <asp:UpdatePanel ID="updHiddenCtls" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- controlli nascosti -->
                <asp:LinkButton ID="lnkReposition" runat="server">-</asp:LinkButton>
                <asp:LinkButton ID="lnkAfterNew" runat="server">-</asp:LinkButton>
                <asp:TextBox ID="txtReposition" runat="server" Text="0"></asp:TextBox>
                <asp:TextBox ID="txtAfterNew" runat="server" Text=""></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <stl:StlUpdatePanel ID="updEVENTI_src" runat="server" Top="0px" Left="0px" Width="250px">
        <ContentTemplate>
            <bof:SearchIscrittiGF ID="searchIscritti" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updIscritti_g" runat="server" Height="760px" Width="970px"
        Left="258px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIscritti" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsISCRITTI_g" EnableViewState="False" ItemDescriptionPlural="partecipanti"
                ItemDescriptionSingular="partecipante" Title="Elenco Partecipanti" SqlStringProviderID="searchIscritti" AllowReselectSelectedRow="true" DeleteConfirmationQuestion="Confermi l'eliminazione del nominativo?">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="tx_COGNOMTIT_PRO" HeaderText="Cognome e nome / Profilo / Rep. o ente" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data / Ora iscrizione" DataFormatString="{0:dd/MM/yy HH:mm}" />
                    <asp:TemplateField HeaderText="Origine">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBORIGINEISCRIZIONE") %>"><%# Eval("tx_ORIGINEISCRIZIONE") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Categoria">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBCATEGORIAECM") %>"><%# Eval("tx_CATEGORIAECM")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stato Iscrizione">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBSTATOISCRIZIONE") %>"><%# Eval("tx_STATOISCRIZIONE")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Minuti Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Questionario">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOQUESTIONARIO") %>">
                            <%# Eval("tx_STATOQUESTIONARIO") &
                                                                                            If(Eval("ac_STATOQUESTIONARIO") <> "NA",
                                                                                            " (" & Eval("ni_RISPOSTEOK") & "/" & Eval("ni_RISPOSTE") & ")",
                                                                                            "")
                                %>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stato ECM">
                        <ItemTemplate>
                            <span style="color:<%# Eval("ac_RGBSTATOECM") %>"><%# Eval("tx_STATOECM") & If(Not IsDBNull(Eval("dt_OTTENIMENTOCREDITIECM")), "<br/><span style=""color:#000000"">" & Eval("dt_OTTENIMENTOCREDITIECM", "{0:dd/MM/yyyy}") & "</span>", "")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Azioni">
                        <ItemTemplate>

                            <asp:LinkButton style="padding-left: 5px; padding-right: 5px; width: 108px; text-align: center;" Visible= <%# If(Not IsDBNull(DataBinder.Eval(Container.DataItem, "tx_EMAIL")) And DataBinder.Eval(Container.DataItem, "tx_STATOISCRIZIONE") = "Presente" And DataBinder.Eval(Container.DataItem, "tx_STATOECM") = "Crediti conseguiti" And DataBinder.Eval(Container.DataItem, "fl_ATTECMONLINE") = "true", "true", "false") %> ID="LinkButton2"  OnClick="mailAttEcm_Click" runat="server" CommandArgument='<%# Eval("id_ISCRITTO")%>' CssClass="btnlink">
                                Mail attest. ECM <%# If(Not IsDBNull(Eval("dt_INVIOMAILATTECM")), "Inviata: " & Eval("dt_INVIOMAILATTECM", "{0:dd/MM/yyyy}"), "") %>
                            </asp:LinkButton>

                            <asp:LinkButton style="padding-left: 5px; margin-top: 2px; padding-right: 5px; width: 108px; text-align: center;" Visible= <%# If(Not IsDBNull(DataBinder.Eval(Container.DataItem, "tx_EMAIL")) And DataBinder.Eval(Container.DataItem, "tx_STATOISCRIZIONE") = "Presente" And Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_MATRICOLA")) And DataBinder.Eval(Container.DataItem, "ac_CATEGORIAECM") = "P" And DataBinder.Eval(Container.DataItem, "fl_ATTPARTONLINE_P_DIP") = "true", "true", "false") %>  ID="LinkButton1"  OnClick="mailAttPart_Click" runat="server" CssClass="btnlink" CommandArgument='<%# Eval("id_ISCRITTO")%>'>
                                Mail attest. Part. <%# If(Not IsDBNull(Eval("dt_INVIOMAILATTPART")), "Inviato: " & Eval("dt_INVIOMAILATTPART", "{0:dd/MM/yyyy}"), "") %>
                            </asp:LinkButton>
                            <asp:LinkButton style="padding-left: 5px; margin-top: 2px; padding-right: 5px; width: 108px; text-align: center;" Visible= <%# If(Not IsDBNull(DataBinder.Eval(Container.DataItem, "tx_EMAIL")) And DataBinder.Eval(Container.DataItem, "tx_STATOISCRIZIONE") = "Presente" And IsDBNull(DataBinder.Eval(Container.DataItem, "ac_MATRICOLA")) And DataBinder.Eval(Container.DataItem, "ac_CATEGORIAECM") = "P" And DataBinder.Eval(Container.DataItem, "fl_ATTPARTONLINE_P_EST") = "true", "true", "false") %>  ID="LinkButton3"  OnClick="mailAttPart_Click" runat="server" CssClass="btnlink" CommandArgument='<%# Eval("id_ISCRITTO")%>'>
                                Mail attest. Part. <%# If(Not IsDBNull(Eval("dt_INVIOMAILATTPART")), "Inviato: " & Eval("dt_INVIOMAILATTPART", "{0:dd/MM/yyyy}"), "") %>
                            </asp:LinkButton>
                            <asp:LinkButton style="padding-left: 5px; margin-top: 2px; padding-right: 5px; width: 108px; text-align: center;" Visible= <%# If(Not IsDBNull(DataBinder.Eval(Container.DataItem, "tx_EMAIL")) And DataBinder.Eval(Container.DataItem, "tx_STATOISCRIZIONE") = "Presente" And Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_MATRICOLA")) And DataBinder.Eval(Container.DataItem, "ac_CATEGORIAECM") <> "P" And DataBinder.Eval(Container.DataItem, "fl_ATTPARTONLINE_DRMT_DIP") = "true", "true", "false") %>  ID="LinkButton4"  OnClick="mailAttPart_Click" runat="server" CssClass="btnlink" CommandArgument='<%# Eval("id_ISCRITTO")%>'>
                                Mail attest. Part. <%# If(Not IsDBNull(Eval("dt_INVIOMAILATTPART")), "Inviato: " & Eval("dt_INVIOMAILATTPART", "{0:dd/MM/yyyy}"), "") %>
                            </asp:LinkButton>
                            <asp:LinkButton style="padding-left: 5px; margin-top: 2px; padding-right: 5px; width: 108px; text-align: center;" Visible= <%# If(Not IsDBNull(DataBinder.Eval(Container.DataItem, "tx_EMAIL")) And DataBinder.Eval(Container.DataItem, "tx_STATOISCRIZIONE") = "Presente" And IsDBNull(DataBinder.Eval(Container.DataItem, "ac_MATRICOLA")) And DataBinder.Eval(Container.DataItem, "ac_CATEGORIAECM") <> "P" And DataBinder.Eval(Container.DataItem, "fl_ATTPARTONLINE_DRMT_EST") = "true", "true", "false") %>  ID="LinkButton5"  OnClick="mailAttPart_Click" runat="server" CssClass="btnlink" CommandArgument='<%# Eval("id_ISCRITTO")%>'>
                                Mail attest. Part. <%# If(Not IsDBNull(Eval("dt_INVIOMAILATTPART")), "Inviato: " & Eval("dt_INVIOMAILATTPART", "{0:dd/MM/yyyy}"), "") %>
                            </asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsISCRITTI_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="sp_eve_DeleteISCRITTI" DeleteCommandType="StoredProcedure" SelectCommand="Dummy">
                <DeleteParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                    <asp:Parameter Name="id_ISCRITTO" Type="Int32" />
                    <asp:Parameter Name="id_UTENT" Type="Int32" />
                </DeleteParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
     <div style="position:absolute;top:770px;left:260px;width:970px;text-align:right;font-size:14px;font-family:Arial;">
         <b>Aggiungi</b>&nbsp;&nbsp;
         <asp:HyperLink runat="server" ID="lnkAggiungiDocente" CssClass="btnlink" NavigateUrl="javascript:aggiungiPartecipante('D');">Docente</asp:HyperLink>
         <asp:HyperLink runat="server" ID="lnkAggiungiRelatore" CssClass="btnlink" NavigateUrl="javascript:aggiungiPartecipante('R');">Relatore</asp:HyperLink>
         <asp:HyperLink runat="server" ID="lnkAggiungiModeratore" CssClass="btnlink" NavigateUrl="javascript:aggiungiPartecipante('M');">Moderatore</asp:HyperLink>
         <asp:HyperLink runat="server" ID="lnkAggiungiTutor" CssClass="btnlink" NavigateUrl="javascript:aggiungiPartecipante('T');">Tutor</asp:HyperLink>
         <asp:HyperLink runat="server" ID="lnkAggiungiResponsabileScientifico" CssClass="btnlink" NavigateUrl="javascript:aggiungiPartecipante('RS');">Resp. Scientifico</asp:HyperLink>
         <asp:HyperLink runat="server" ID="lnkAggiungiPartecipante" CssClass="btnlink" NavigateUrl="javascript:aggiungiPartecipante('P');" Font-Bold="true">Partecipante</asp:HyperLink>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
