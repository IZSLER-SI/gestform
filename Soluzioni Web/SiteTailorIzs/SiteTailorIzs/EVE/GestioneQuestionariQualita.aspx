<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="GestioneQuestionariQualita.aspx.vb" Inherits="Softailor.SiteTailorIzs.GestioneQuestionariQualita" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
     <script type="text/javascript">
         <asp:Literal ID="ltrRepositioning" runat="server" />    
    </script>
    <script type="text/javascript">
        function OpenQuestionario(id)
        {
            if (stl_appb_row2Select_action==null)
            {
                stl_sel_display_high('GestioneQuestionarioQualita.aspx?id=' + id, editQuestionario_callback);
            }
            else
            {
                if (stl_appb_row2Select_action!='Delete')
                {
                    stl_sel_display_high('GestioneQuestionarioQualita.aspx?id=' + id, editQuestionario_callback);
                }
            }
        }
    </script>
    <style type="text/css">
        .rtable
        {
            font-size:11px;
            font-weight:normal;
            width:100%;
            border-collapse:collapse;
        }
        .rtable td
        {
            padding:1px 3px 2px 3px;
            border-bottom:solid 1px #c0c0c0;
        }
        .rtable .lbl
        {
            text-align:left;
            vertical-align:middle;
        }
        .rtable .val
        {
            text-align:center;
            vertical-align:middle;
            font-weight:bold;
            white-space:nowrap;
        }
        .rtable .exp
        {
            text-align:left;
            vertical-align:middle;
            font-weight:normal;
            color:#666666;
        }
        .rtable .indent
        {
            padding-left:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <div style="display: none;">
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
    <stl:StlUpdatePanel ID="updQUESTIONARIQUALITA_g" runat="server" Height="530px" Width="500px"
        Left="0px" Top="0px">
        <ContentTemplate>
            <stl:StlGridView ID="grdQUESTIONARIQUALITA" runat="server" AddCommandText="" AutoGenerateColumns="False"
                DataKeyNames="id_QUESTIONARIOQUALITA" DataSourceID="sdsQUESTIONARIQUALITA_g"
                EnableViewState="False" ItemDescriptionPlural="questionari" ItemDescriptionSingular="questionari"
                Title="Questionari inseriti" AllowReselectSelectedRow="true" DeleteConfirmationQuestion="Confermi l'eliminazione del questionario?">
                <Columns>
                    <asp:CommandField />
                    <asp:BoundField DataField="ni_QUESTIONARIOQUALITA" HeaderText="N° Progr." ItemStyle-Font-Bold="true"
                        ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="dt_CREAZIONE" HeaderText="Data/Ora inserimento" DataFormatString="{0:dd/MM/yy HH:mm}" />
                    <asp:BoundField DataField="tx_COGNOMTIT" HeaderText="Cognome e nome" />
                </Columns>
            </stl:StlGridView>
            <stl:StlSqlDataSource ID="sdsQUESTIONARIQUALITA_g" runat="server" ConnectionString="<%$ ConnectionStrings:SiteTailorDbConnectionString %>"
                DeleteCommand="sp_eve_DeleteQUESTIONARIOQUALITA" DeleteCommandType="StoredProcedure"
                SelectCommand="SELECT * FROM vw_eve_QUESTIONARIQUALITA WHERE id_EVENTO=@id_EVENTO ORDER BY ni_QUESTIONARIOQUALITA">
                <DeleteParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                    <asp:Parameter Name="id_QUESTIONARIOQUALITA" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="id_EVENTO" Type="Int32" />
                </SelectParameters>
            </stl:StlSqlDataSource>
        </ContentTemplate>
    </stl:StlUpdatePanel>
    <div style="position: absolute; top: 535px; left: 0px; width:300px;font-size:12px;font-weight:bold;">
        <asp:HyperLink runat="server" ID="lnkNuovoQuestionario" CssClass="btnlink" NavigateUrl="javascript:stl_sel_display_high('GestioneQuestionarioQualita.aspx?id=0',addQuestionario_callback);">Carica nuovo questionario</asp:HyperLink>
        <asp:LinkButton runat="server" ID="uxSearch" CssClass="btn btn-default" >
            Scarica risultati
        </asp:LinkButton>
    </div>
    <div class="stl_dfo" style="position: absolute; left: 520px; top: 0px; width: 400px;">
        <div class="title">
            Sintesi Risultati
        </div>
    </div> 
    <div class="stl_gen_box" style="position: absolute; width: 437px; top: 30px; left: 520px;">
        <div class="content" style="padding:10px;height:480px;">
            <asp:UpdatePanel ID="updRisultati" runat="server" EnableViewState="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:PlaceHolder ID="phdRisultati" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div style="position: absolute; top: 535px; left: 520px;width:250px;font-size:12px;font-weight:bold;display:none;">
        <asp:LinkButton ID="lnkStampaPdf" runat="server" EnableViewState="false" CssClass="btnlink">Stampa Questionario</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
