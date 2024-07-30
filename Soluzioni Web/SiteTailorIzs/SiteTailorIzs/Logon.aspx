<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logon.aspx.vb" Inherits="Softailor.SiteTailorIzs.Logon" %>

<!DOCTYPE html>
<html lang="it-it">

<head runat="server">
    <title>EvenTailor - Login</title>
    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <style type="text/css">
        body
        {
            font-family: Verdana;
            font-size: 11px;
            margin: 0px;
            padding: 0px;
        }
        .tbl
        {
            font-family:Verdana;
            font-size:11px;
            
        }
        .tbl td, .tbl th
        {
            padding:0px;
        }
        .lbl
        {
            text-align:left;
            vertical-align:middle;
            width:90px;
            height:26px;
        }
        .ctl
        {
            text-align:left;
            vertical-align:middle;
            width:150px;
        }
        .tbox
        {
            font-family:Verdana, Arial, Sans-Serif;
	        font-size:11px;
            padding:2px 0px 2px 2px;
            margin:1px 1px 1px 0px;
            border:1px solid #888888;
            background-color:#ffffff;
            color:#000000;
            width:150px;    
        }
        .val
        {
            text-align:left;
            vertical-align:middle;
            padding-left:5px;
        }
        .err
        {
            color:#ff0000;
            padding-top:10px;
        }
        td.lgn
        {
            padding-top:3px;
        }
        .blgn
        {
            font-family:Verdana;
            font-size:11px;
            font-weight:bold;
            margin:0px;
            padding:3px 8px 3px 8px;
        }
    </style>
    <script>
        $(function () {
            if ($.browser.msie) {
                if ($.browser.version == '7.0' || $.browser.version == '6.0') {
                    $("#logindiv").css("display", "none");
                    $("#noie7div").css("display", "block");
                }
            }
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="display: block; height: 36px; padding-top: 2px; background-color: #000000;">
        <asp:Image runat="server" ID="imgTitle" EnableViewState="false" />
    </div>
    <div style="padding: 12px;display:block;" id="logindiv">
        <asp:Login ID="Login1" runat="server" DisplayRememberMe="False">
            <LayoutTemplate>
                <table border="0" class="tbl">
                    <tr>
                        <td class="lbl">Nome Utente</td>
                        <td class="ctl"><asp:TextBox ID="UserName" CssClass="tbox" runat="server" /></td>
                        <td class="val"><asp:requiredfieldvalidator id="UserNameRequired" runat="server" ControlToValidate="UserName" Text="*" /></td>
                    </tr>
                    <tr>
                        <td class="lbl">Password</td>
                        <td class="ctl"><asp:TextBox ID="Password" CssClass="tbox" runat="server" TextMode="Password" /></td>
                        <td class="val"><asp:requiredfieldvalidator id="PasswordRequired" runat="server" ControlToValidate="Password" Text="*" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right" class="lgn">
                            <asp:Button ID="submitLoginBtn" CommandName="Login" runat="server" Text="Accedi" CssClass="blgn" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="err">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </div>
    <div style="padding:12px;display:none;font-size:13px;line-height:22px;" id="noie7div">
        <img src="Img/BigWarning.png" style="float:left;margin-right:15px;" />
        <div style="color:#ff0000;font-weight:bold;padding-top:8px;">Stai utilizzando Internet Explorer versione 7.</div>
        <div>Questa applicazione non può essere utilizzata con la versione 7 di Internet Explorer.</div>
        <div>Utilizza <strong>Google Chrome</strong> oppure una <strong>versione più recente di Internet Explorer</strong>.</div>
    </div>
    </form>
</body>
</html>
