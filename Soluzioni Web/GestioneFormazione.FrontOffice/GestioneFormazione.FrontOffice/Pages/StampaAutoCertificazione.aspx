<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StampaAutoCertificazione.aspx.vb" Inherits="GestioneFormazione.FrontOffice.StampaAutoCertificazione" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dichiarazione sostitutiva di atto notorio</title>
    <style type="text/css">
        body
        {
            font-family:Arial, sans-serif;
            font-size:14px;
            text-align:left;
            padding:15px;
            margin:0px;
        }
    </style>
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript">
        $(function () {
            window.print();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
    </div>
    </form>
        

</body>
</html>
