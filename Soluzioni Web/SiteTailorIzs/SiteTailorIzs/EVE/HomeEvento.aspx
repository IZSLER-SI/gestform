<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorMP.Master" CodeBehind="HomeEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.HomeEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .menuitem
        {
            font-size:11px;
            line-height:12px;
            padding:0px 5px 0px 5px;
            border-bottom:solid 1px #c0c0c0;
            color:#666666;
        }
        .hdrGroupIsc
        {
            padding-top:2px;
            padding-bottom:1px; 
            margin-bottom:2px; 
            border-bottom:solid 1px #c0c0c0;
        }
        .itemstable
        {
            font-size:11px;line-height:14px;width:100%;
        }
        .tooltip
        {
            margin-right:5px;
            vertical-align:top;
        }
        .wipevento ul
        {
            padding:0px;
            margin:0px 0px 0px 20px;
        }
        .wipevento .expired
        {
            color:#ff0000;
        }
        .wipevento .active
        {
            color:#000000;
        }
        .wipevento .normal
        {
            color:#000000;
        }


    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdContent" runat="server">
    <script type="text/javascript">
				

        function loadWipEvento() {
            $.ajax({
                url: "../WIP/WipEvento.aspx/GetWipEvento",
                type: "POST",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    id_EVENTO: $("#spanIdEvento").text()
                }),
                success: function (data) {
                    $("#wipEventoDiv").html(data.d);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //non faccio nulla
                }
            });
				}
				$(function () {
						loadWipEvento();
						$("#phdContent_lnkdownload_").click(function () {
								setTimeout(function () {
										stl_ajaxwaiting.css("display", "none");
								}, 1000);
						});
				});
    </script>

    <asp:UpdatePanel ID="updDatiEventoEWorkFlow" runat="server" EnableViewState="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdDatiEvento" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="wipEventoDiv">
    </div>
    <asp:UpdatePanel ID="updIscritti" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phdIscritti" runat="server" EnableViewState="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
