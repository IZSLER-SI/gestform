<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatoPresenze.aspx.vb" Inherits="Softailor.SiteTailorIzs.StatoPresenze" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stato Presenze</title>
    <style type="text/css">
        body
        {
            padding:0px 0px 0px 0px;
            margin:15px 15px 15px 15px;
            font-family:Arial, Sans-Serif;
        }
        h1
        {
            font-family:Arial;
            font-weight:bold;
            font-size:30px;    
            color:#336699;
            margin-bottom:20px;
            text-align:center;
        }
        table
        {
            font-size:13px;    
        }
        table th
        {
            padding:3px 3px 3px 3px;
            border-bottom:solid 2px #c0c0c0;  
            text-align:left;
            font-weight:bold;  
        }
        table td
        {
            padding:3px 3px 3px 3px;
            border-bottom:solid 1px #c0c0c0;    
            text-align:left;
            font-weight:normal;
            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
    </div>
    </form>
</body>
</html>
