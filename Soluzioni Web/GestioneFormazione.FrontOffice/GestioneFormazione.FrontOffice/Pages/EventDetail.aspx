<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MenuUserMP.master" CodeBehind="EventDetail.aspx.vb" Inherits="GestioneFormazione.FrontOffice.EventDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">

        var iscrizioneEvento;
        var iscrizioneEventoTop;

        $(function () {

            iscrizioneEvento = $("#updIscrizioneEvento");
            iscrizioneEventoTop = iscrizioneEvento.position().top;

            $(this).scroll(function () {
                var pageTop = $(this).scrollTop();
                if (pageTop > iscrizioneEventoTop - 25) {
                    iscrizioneEvento.css("position", "fixed");
                    iscrizioneEvento.css("top", "25px");
                }
                else {
                    iscrizioneEvento.css("position", "");
                    iscrizioneEvento.css("top", "");
                }
            });
        });

        function linksso()
        {
            eval($('#login_lnk_login_sso').attr('href'));
        }

        function openRegistration()
        {
            if($(this).scrollTop() > 151) {
                var completeCalled = false;
                $("html, body").animate(
                    { scrollTop: "0px" },
                    {
                        complete: function () {
                            if (!completeCalled) {
                                completeCalled = true;
                                displayLogin(true);
                            }
                        }
                    }
                );
            }
            else
            {
                displayLogin(true);
            }
        }
        function showRegistrationPopup(show)
        {
            if (show) {
                $("#reg_popup_covering").fadeIn(100, function () {
                    $("#reg_popup").fadeIn(250);
                });
                

            }
            else {
                $("#reg_popup").fadeOut(250, function () {
                    $("#reg_popup_covering").fadeOut(100);
                });
                
                
            }
            
        }
    </script>
    <div class="twocol_left">
        <asp:UpdatePanel ID="updEvento" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdEvento" runat="server" EnableViewState="false" />        
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="twocol_right">
        <asp:UpdatePanel ID="updIscrizioneEvento" runat="server" EnableViewState="true" UpdateMode="Conditional" ClientIDMode="Static">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdIscrizione" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="reg_popup_covering" style="display:none;"></div>
    <div id="reg_popup" style="display:none;">
        <asp:UpdatePanel ID="updPopupIscrizioneEvento" runat="server" EnableViewState="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phdPopupIscrizione" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="display:none">
        <asp:LinkButton ID="login_lnk_login_sso" ClientIDMode="Static" runat="server" CssClass="btn btn-secondary btn-block"/>
    </div>
    <div class="clear"></div>
</asp:Content>
