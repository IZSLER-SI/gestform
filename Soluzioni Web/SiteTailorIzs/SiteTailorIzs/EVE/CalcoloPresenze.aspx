<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="CalcoloPresenze.aspx.vb" Inherits="Softailor.SiteTailorIzs.CalcoloPresenze" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script type="text/javascript">
        <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
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
    <div id="istruzioni" style="position: absolute; left: 318px; top: 0px; width:47px;" class="infoDiv11">
        Istruzioni
    </div>
    <div id="istruzionicontent" style="display:none;">
        Questa pagina permette di calcolare i <b>minuti di presenza</b> di ciascun partecipante.<br />
        Gli <b>annullati</b> e/o i <b>non accettati</b> non vengono considerati.<br />
        La presenza effettiva è calcolata in base agli orari effettivi dell'evento: ad esempio, se l'evento inizia alle 9:00
        e un partecipante entra alle 8:50, il conteggio dei minuti inizia comunque dalle 9:00.<br />
        A seguito del rilevamento degli <b>orari</b> e del <b>tempo minimo di presenza</b> il sistema verifica se gli
        iscritti hanno o non hanno accumulato la presenza minima richiesta.
        I nominativi che non hanno raggiunto la presenza minima sono ordinati per tempo di presenza decrescente.
    </div>
    <div style="display: none;">
        <asp:UpdatePanel ID="updHiddenCtls" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- controlli nascosti -->
                <asp:LinkButton ID="lnkReposition" runat="server">-</asp:LinkButton>
                <asp:TextBox ID="txtReposition" runat="server" Text="0"></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 0px; width:300px;">
        <div class="title">
            Orari svolgimento evento
        </div>
    </div>
    <stl:StlUpdatePanel ID="updORARIEVENTO_g" runat="server" Top="30px" Left="0px"
        Width="400px" Height="420px">
        <ContentTemplate>
            <stl:StlGridView ID="grdORARIEVENTO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataSourceID="sdsORARIEVENTO_g" DeleteConfirmationQuestion="" EnableViewState="False"
                AllowInsert="False" AllowDelete="False" ItemDescriptionPlural="righe" ItemDescriptionSingular="riga"
                Title="Orari" DataKeyNames="id_ORARIOEVENTO">
                <Columns>
                    <asp:BoundField DataField="dt_DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="tm_INIZIO" HeaderText="Ora inizio" />
                    <asp:BoundField DataField="tm_FINE" HeaderText="Ora fine" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsORARIEVENTO_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommandType="Text" SelectCommand="SELECT id_ORARIOEVENTO, id_EVENTO, dt_DATA, tm_INIZIO, tm_FINE FROM eve_ORARIEVENTO WHERE id_EVENTO=@id_EVENTO ORDER BY dt_DATA, tm_INIZIO">
                <SelectParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <asp:UpdatePanel ID="updTotale" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div style="position:absolute;top:456px;left:0px;width:400px; font-size:14px">
                Durata totale dell'evento (minuti):
                <b>
                    <asp:Label ID="lblDurataTotale" runat="server" EnableViewState="false" />
                </b>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="stl_dfo" style="position: absolute; left: 0px; top: 480px; width:400px;">
        <div class="title">
            Presenza minima richiesta
        </div>
    </div>
    <stl:StlUpdatePanel ID="updEVENTO_f" runat="server" Top="510px" Left="0px"
        Width="400px" Height="50px">
        <ContentTemplate>
            <stl:StlFormView runat="server" ID="frmEVENTO" DataSourceID="sdsEVENTO_f"
                NewItemText="Nuova riga" DataKeyNames="id_EVENTO">
                <EditItemTemplate>
                    <span class="flbl" style="width: 330px;">Presenza minima richiesta per l'ottenimento dei crediti ECM (min.)</span>
                    <asp:TextBox ID="ni_MINIMOMINUTITextBox" runat="server" Text='<%# Bind("ni_MINIMOMINUTI") %>'
                        Width="50px" Font-Bold="true" />
                </EditItemTemplate>
            </stl:StlFormView>
            <stl:StlSqlDataSource ID="sdsEVENTO_f" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                SelectCommand="SELECT id_EVENTO, ni_MINIMOMINUTI FROM eve_EVENTI WHERE id_EVENTO=@id_EVENTO"
                UpdateCommand="UPDATE eve_EVENTI SET ni_MINIMOMINUTI=@ni_MINIMOMINUTI WHERE id_EVENTO=@id_EVENTO">
                <SelectParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                    <asp:Parameter Name="ni_MINIMOMINUTI" Type="Int32" />
                </UpdateParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updIscrittiKO_g" runat="server" Height="280px" Width="548px"
        Left="410px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIscrittiKO" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsISCRITTIKO_g" EnableViewState="False"
                ItemDescriptionPlural="persone" ItemDescriptionSingular="persona" Title="Partecipanti che NON hanno raggiunto la presenza minima"
                AllowDelete="false">
                <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Presenza / minimo (min)" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <span style="color:#ff0000">
                                <b>
                                    <%# Eval("ni_TOTALEMINUTIISCRITTO") & " / " & Eval("ni_MINIMOMINUTIEVENTO")%>
                                </b>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comandi" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                                <asp:LinkButton CssClass="rowBtn" CommandName="E" Text="&gt;Scheda" runat="server" ID="btnE"
                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlUpdatePanel ID="updIscrittiOK_g" runat="server" Height="270px" Width="548px"
        Left="410px" Top="290px">
        <ContentTemplate>
            <stl:StlGridView ID="grdIscrittiOK" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_ISCRITTO" DataSourceID="sdsISCRITTIOK_g" EnableViewState="False"
                ItemDescriptionPlural="persone" ItemDescriptionSingular="persona" Title="Partecipanti che hanno raggiunto la presenza minima"
                AllowReselectSelectedRow="true" AllowDelete="false">
               <Columns>
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" HtmlEncode="false" />
                    <asp:BoundField DataField="ac_MATRICOLA" HeaderText="Matricola" />
                    <asp:TemplateField HeaderText="Presenza / minimo (min)" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <span style="color:#00aa00">
                                <b>
                                    <%# Eval("ni_TOTALEMINUTIISCRITTO") & " / " & Eval("ni_MINIMOMINUTIEVENTO")%>
                                </b>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comandi" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div class="rowCmd">
                                <asp:LinkButton CssClass="rowBtn" CommandName="E" Text="&gt;Scheda" runat="server" ID="btnE"
                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </stl:StlGridView>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <stl:StlSqlDataSource ID="sdsISCRITTIKO_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="
        SELECT      * 
        FROM        vw_eve_ISCRITTI 
        WHERE       id_EVENTO=@id_EVENTO AND 
                    ac_STATOISCRIZIONE IN ('I','P') AND
                    ac_STATOPRESENZA='KO'
        ORDER BY    ni_TOTALEMINUTIISCRITTO desc, tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>
    <stl:StlSqlDataSource ID="sdsISCRITTIOK_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
        SelectCommand="SELECT      * 
                        FROM        vw_eve_ISCRITTI 
                        WHERE       id_EVENTO=@id_EVENTO AND 
                                    ac_STATOISCRIZIONE IN ('I','P') AND
                                    ac_STATOPRESENZA='OK'
                        ORDER BY    tx_COGNOME, tx_NOME, id_ISCRITTO">
        <SelectParameters>
            <asp:Parameter Name="id_EVENTO" Type="Int32" />
        </SelectParameters>
    </stl:StlSqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
