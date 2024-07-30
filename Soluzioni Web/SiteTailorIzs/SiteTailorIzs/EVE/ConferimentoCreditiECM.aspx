<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="ConferimentoCreditiECM.aspx.vb" Inherits="Softailor.SiteTailorIzs.ConferimentoCreditiECM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .rowCmd
        {
            font-size:11px;
            line-height:10px;    
        }
        .rowBtn
        {
            color:#336699;
            font-weight:normal;
            text-decoration:none;    
        }
        .rowBtn:hover
        {
            color:#ff6600;
            text-decoration:none;    
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script src="../Scripts/jquery.qtip.min.js"></script>
    <link href="../Scripts/jquery.qtip.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $('#istruzioni').qtip({
                content: {
                    text: $('#istruzionicontent').html()
                },
                position: {
                    my: 'top left',
                    at: 'bottom left',
                    viewport: $(window),
                    target: 'mouse',
                    adjust: {
                        y: 18, mouse: true
                    }
                },
                style: {
                    classes: 'qtip-jtools',
                    width: 400

                }
            });
        });
    </script>
    <div id="istruzioni" style="position: absolute; left: 392px; top: 0px; width:47px;" class="infoDiv11">
        Istruzioni
    </div>
    <div id="istruzionicontent" style="display:none;">
        Questa pagina permette di <b>conferire i crediti ECM</b> ai nominativi <b>presenti</b> e <b>candidati al conseguimento dei crediti</b>.<br />
        I nominativi <b>non presenti</b> o <b>non candidati al conseguimento dei crediti non vengono visualizzati</b>.<br />
        <br />
        Nei 3 box sono visualizzati: i nominativi <b>candidati al conseguimento dei crediti</b>, i nominativi che <b>hanno conseguito i crediti</b> 
        e i nominativi che <b>non hanno conseguito i crediti</b>.<br />
        <br />
        Puoi <b>spostare i singoli nominativi</b> da un box all'altro <b>mediante i link</b> presenti accanto al nominativo, o effettuare uno
        <b>spostamento massivo</b> usando la funzione <b>"sposta tutti"</b> sotto al box "Candidati al conseguimento dei crediti".<br />
        <br />
        Lo spostamento massivo può essere fatto per <b>tutti i nominativi</b> oppure
        solo per i nominativi che hanno <b>superato il questionario</b> e/o <b>raggiunto il tempo minimo di presenza</b> (in caso di verifica delle presenze effettuata mediante scansione barcode).
    </div>
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px;width:300px">
        <div class="title">
            Conferimento Crediti ECM
        </div>
    </div>
    <stl:StlUpdatePanel ID="updCandidati" runat="server" Height="412px" Width="574px"
        Left="0px" Top="30px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCandidati" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsCandidati" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Candidati al conseguimento dei crediti" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" HtmlEncode="false" />
                     <asp:TemplateField HeaderText="Minuti&lt;br/&gt;Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Questionario">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOQUESTIONARIO") %>">
                            <%# Eval("tx_STATOQUESTIONARIO") & _
                                If(Eval("ac_STATOQUESTIONARIO") <> "NA", _
                                " (" & Eval("ni_RISPOSTEOK") & "/" & Eval("ni_RISPOSTE") & ")", _
                                "")
                                %>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="COK" Text="&gt;Crediti conseguiti" runat="server" ID="btnCOK" Font-Bold="true"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="CKO" Text="&gt;Crediti non conseguiti" runat="server" ID="btnCKO" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:UpdatePanel ID="updMoveAll" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="position:absolute; top:450px;left:0px;width:474px;font-size:11px;">
                Sposta
                <asp:DropDownList ID="ddnMoveAll_Orig" EnableViewState="false" runat="server" CssClass="ddn">
                    <asp:ListItem Text="i candidati al conseguimento dei crediti" Value="C" />
                    <asp:ListItem Text="coloro che hanno conseguito i crediti" Value="COK" />
                    <asp:ListItem Text="coloro che non hanno conseguito i crediti" Value="CKO" />
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="ddnMoveAll_Orig_Quest" EnableViewState="false" runat="server" CssClass="ddn">
                    <asp:ListItem Text="indipendentemente dal superamento del questionario" Value="" />
                    <asp:ListItem Text="che HANNO superato il questionario" Value="COK" />
                    <asp:ListItem Text="che NON HANNO superato il questionario" Value="CKO" />
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="ddnMoveAll_Orig_Presenza" EnableViewState="false" runat="server" CssClass="ddn">
                    <asp:ListItem Text="e indipendentemente dal tempo minimo di presenza" Value="" />
                    <asp:ListItem Text="e che HANNO raggiunto il tempo minimo di presenza" Value="OK" />
                    <asp:ListItem Text="e che NON HANNO raggiunto il tempo minimo di presenza" Value="KO" />
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="ddnMoveAll_Dest" EnableViewState="false" runat="server" CssClass="ddn">
                    <asp:ListItem Text="tra coloro che hanno conseguito i crediti" Value="COK" />
                    <asp:ListItem Text="tra coloro che non hanno conseguito i crediti" Value="CKO" />
                    <asp:ListItem Text="tra i candidati al conseguimento dei crediti" Value="C" />
                </asp:DropDownList>
                <asp:LinkButton CssClass="btnlink" ID="lnkMoveAll" runat="server" Font-Bold="true">Esegui</asp:LinkButton>
                <ajaxToolkit:ConfirmButtonExtender ID="cnfMoveAll" runat="server" TargetControlID="lnkMoveAll" ConfirmText="Confermi l'operazione?" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <stl:StlUpdatePanel ID="updCreditiOK" runat="server" Height="400px" Width="574px"
        Left="584px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCreditiOK" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsCreditiOK" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Nominativi che hanno conseguito i crediti" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" HtmlEncode="false" />
                    <asp:TemplateField HeaderText="Minuti&lt;br/&gt;Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Questionario">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOQUESTIONARIO") %>">
                            <%# Eval("tx_STATOQUESTIONARIO") & _
                                If(Eval("ac_STATOQUESTIONARIO") <> "NA", _
                                " (" & Eval("ni_RISPOSTEOK") & "/" & Eval("ni_RISPOSTE") & ")", _
                                "")
                                %>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="C" Text="&gt;Candidato al conseguimento" runat="server" ID="btnC"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="CKO" Text="&gt;Crediti non conseguiti" runat="server" ID="btnCKO" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updCreditiKO" runat="server" Height="155px" Width="574px"
        Left="584px" Top="409px">
        <ContentTemplate>
            <stl:StlGridView ID="grdCreditiKO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsCreditiKO" EnableViewState="False" ItemDescriptionPlural="persone"
                ItemDescriptionSingular="persona" Title="Nominativi che non hanno conseguito i crediti" AllowReselectSelectedRow="true" AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="tx_CATEGORIAECM" HeaderText="Ruolo" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" HtmlEncode="false" />
                     <asp:TemplateField HeaderText="Minuti&lt;br/&gt;Presenza" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOPRESENZA") %>">
                            <%# Eval("ni_TOTALEMINUTIISCRITTO") & "/" & Eval("ni_MINIMOMINUTIEVENTO")%>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Questionario">
                        <ItemTemplate>
                            <div style="color:<%# Eval("ac_RGBSTATOQUESTIONARIO") %>">
                            <%# Eval("tx_STATOQUESTIONARIO") & _
                                If(Eval("ac_STATOQUESTIONARIO") <> "NA", _
                                " (" & Eval("ni_RISPOSTEOK") & "/" & Eval("ni_RISPOSTE") & ")", _
                                "")
                                %>
                           </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Imposta a..." ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                            <asp:LinkButton CssClass="rowBtn" CommandName="C" Text="&gt;Candidato al conseguimento" runat="server" ID="btnC"
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <br />
                            <asp:LinkButton CssClass="rowBtn" CommandName="COK" Text="&gt;Crediti conseguiti" runat="server" ID="btnCOK" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>

    <stl:StlSqlDataSource ID="sdsCandidati" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE ac_STATOISCRIZIONE='P' AND id_EVENTO=@id_EVENTO AND ac_STATOECM=@ac_STATOECM ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOECM" Type="String" DefaultValue="C" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsCreditiOK" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE ac_STATOISCRIZIONE='P' AND id_EVENTO=@id_EVENTO AND ac_STATOECM=@ac_STATOECM ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOECM" Type="String" DefaultValue="COK" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsCreditiKO" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT * FROM vw_eve_ISCRITTI WHERE ac_STATOISCRIZIONE='P' AND id_EVENTO=@id_EVENTO AND ac_STATOECM=@ac_STATOECM ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
            <asp:Parameter Name="ac_STATOECM" Type="String" DefaultValue="CKO" />
        </SelectParameters>
    </stl:StlSqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
