<%@ Page Title="Selezione Immagine/Allegato" Language="vb" AutoEventWireup="false" 
    MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="ChooseBinary.aspx.vb" 
    Inherits="Softailor.SiteTailorIzs.ChooseBinary" Theme="SiteTailor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
        .category
        {
        	color:#336699;
        	padding-top:0px;
        	padding-bottom:10px;
        	clear:both;
        	font-size:16px;
        	font-weight:bold;
        }
        .element
        {
            display:block;
            float:left;
            width:275px;
            height:72px;
            margin-right:10px;
            margin-bottom:10px;
            background-color:#e0e0e0;
        }
        .imgtd
        {
        	width:72px;
        	height:72px;
            background-color:#ffffff;
            padding:0px 0px 0px 0px;
            margin:0px 0px 0px 0px;
        }
        .txttd
        {
            padding:0px 0px 0px 0px;
            margin:0px 0px 0px 0px;
            text-align:left;
            vertical-align:top;
        }
        .thumb_i
        {
            border:solid 1px #999999;
            width:70px;
            height:70px;
        }
        .desc
        {
            color:#000000;
            line-height:12px;
        }
        .fmt
        {
            color:#777777;
            line-height:11px;
            font-size:10px;
        }
        .chooseA
        {
            display:block;
            width:199px;
            height:68px;
            padding:2px 2px 2px 2px;
            overflow:hidden;
            text-decoration:none;
        }
        .chooseA:hover
        {
            background-color:#e3efff;
        }
    </style>
    <script type="text/javascript">
        function chooseElement(ID_ELEME) {
            parent.stl_sel_done(ID_ELEME);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">Selezione immagine / allegato</div>
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
    <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
</asp:Content>
