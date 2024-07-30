<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="AnteprimaMail.aspx.vb" Inherits="Softailor.SiteTailorIzs.AnteprimaMail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <style type="text/css">
      .body
      {
      font-family: Arial, Helvetica, Sans-Serif;
      font-size:12px;
      background-color:#f0f0f0;
      color:#000000;
      margin-right:15px;
      margin-bottom:15px;
      }
      .bodyhdr
      {
      padding-bottom:10px;
      padding-top:10px;
      font-size:12px;
      text-align:left;
      }
      .bodytd
      {
      border:3px solid #0668a7;
      padding:11px;
      background-color:#ffffff;
      font-size:14px;
      line-height:18px;
      text-align:left;
      }
      .footertd
      {
      padding-top:10px;
      font-size:12px;
      line-height:15px;
      text-align:left;
      }
      .mailhead
      {
        font-size:14px;
        border:solid 1px #c0c0c0;
        padding:10px;
        background-color:#ffffff;
        margin-bottom:20px;
        margin-right:15px;
        text-align:left;
      }
        #popupContent
        {
            background-color:#f0f0f0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">
        Anteprima e-mail
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a ID="lnkClose" class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Chiudi
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">
    <asp:PlaceHolder ID="phdMailBody" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="phdOutOfForm" runat="server">
</asp:Content>
