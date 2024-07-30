<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SpedizioneMailProxy.aspx.vb" Inherits="Softailor.SiteTailorIzs.SpedizioneMailProxy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:Literal ID="ltrJquery" runat="server" EnableViewState="false" />
    <script type="text/javascript">
        $(function () {
            //lettura dati dal parent
            $("#sd_ac_FONTE").val(parent.$("#sd_ac_FONTE").val());
            $("#sd_id_MAILREPORT").val(parent.$("#sd_id_MAILREPORT").val());
            $("#sd_tx_VALOREFILTROBASE").val(parent.$("#sd_tx_VALOREFILTROBASE").val());
            $("#sd_xm_FILTRO").val(parent.$("#sd_xm_FILTRO").val());
            $("#sd_ac_ORDINAMENTO").val(parent.$("#sd_ac_ORDINAMENTO").val());
            $("#sd_tx_ORDINAMENTO1").val(parent.$("#sd_tx_ORDINAMENTO1").val());
            $("#sd_tx_ORDINAMENTO2").val(parent.$("#sd_tx_ORDINAMENTO2").val());
            $("#sd_tx_ORDINAMENTO3").val(parent.$("#sd_tx_ORDINAMENTO3").val());
            $("#sd_tx_ORDINAMENTO4").val(parent.$("#sd_tx_ORDINAMENTO4").val());
            $("#sd_tx_ORDINAMENTO5").val(parent.$("#sd_tx_ORDINAMENTO5").val());

            $("#sd_ragionesociale").val(parent.$("#sd_ragionesociale").val());
            $("#sd_indirizzocompleto").val(parent.$("#sd_indirizzocompleto").val());
            $("#sd_tel").val(parent.$("#sd_tel").val());
            $("#sd_fax").val(parent.$("#sd_fax").val());
            $("#sd_email").val(parent.$("#sd_email").val());

            $("#dataform").submit();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    </form>
    <form method="post" action="SpedizioneMailStep1.aspx" id="dataform" style="display:none;">
        <input type="text" id="sd_ac_FONTE" name="sd_ac_FONTE" />
        <input type="text" id="sd_id_MAILREPORT" name="sd_id_MAILREPORT" />
        <input type="text" id="sd_tx_VALOREFILTROBASE" name="sd_tx_VALOREFILTROBASE" />
        <textarea id="sd_xm_FILTRO" name="sd_xm_FILTRO"></textarea>
        <input type="text" id="sd_ac_ORDINAMENTO" name="sd_ac_ORDINAMENTO" />
        <input type="text" id="sd_tx_ORDINAMENTO1" name="sd_tx_ORDINAMENTO1" />
        <input type="text" id="sd_tx_ORDINAMENTO2" name="sd_tx_ORDINAMENTO2" />
        <input type="text" id="sd_tx_ORDINAMENTO3" name="sd_tx_ORDINAMENTO3" />
        <input type="text" id="sd_tx_ORDINAMENTO4" name="sd_tx_ORDINAMENTO4" />
        <input type="text" id="sd_tx_ORDINAMENTO5" name="sd_tx_ORDINAMENTO5" />
        <input type="text" id="sd_ragionesociale" name="sd_ragionesociale" />
        <input type="text" id="sd_indirizzocompleto" name="sd_indirizzocompleto" />
        <input type="text" id="sd_tel" name="sd_tel" />
        <input type="text" id="sd_fax" name="sd_fax" />
        <input type="text" id="sd_email" name="sd_email" />
    </form>
</body>
</html>
