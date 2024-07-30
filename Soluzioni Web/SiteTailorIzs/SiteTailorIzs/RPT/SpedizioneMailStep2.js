function PrevStep() {
    //copio tutti i dati all'indietro
    $("#sd3_ac_FONTE").val($("#sd_ac_FONTE").val());
    $("#sd3_id_MAILREPORT").val($("#sd_id_MAILREPORT").val());
    $("#sd3_tx_VALOREFILTROBASE").val($("#sd_tx_VALOREFILTROBASE").val());
    $("#sd3_xm_FILTRO").val($("#sd_xm_FILTRO").val());
    $("#sd3_ac_ORDINAMENTO").val($("#sd_ac_ORDINAMENTO").val());
    $("#sd3_tx_ORDINAMENTO1").val($("#sd_tx_ORDINAMENTO1").val());
    $("#sd3_tx_ORDINAMENTO2").val($("#sd_tx_ORDINAMENTO2").val());
    $("#sd3_tx_ORDINAMENTO3").val($("#sd_tx_ORDINAMENTO3").val());
    $("#sd3_tx_ORDINAMENTO4").val($("#sd_tx_ORDINAMENTO4").val());
    $("#sd3_tx_ORDINAMENTO5").val($("#sd_tx_ORDINAMENTO5").val());
    $("#sd3_tx_OGGETTO").val($("#sd_tx_OGGETTO").val());
    $("#sd3_ht_CORPO").val($("#sd_ht_CORPO").val());
    $("#sd3_ragionesociale").val($("#sd_ragionesociale").val());
    $("#sd3_indirizzocompleto").val($("#sd_indirizzocompleto").val());
    $("#sd3_tel").val($("#sd_tel").val());
    $("#sd3_fax").val($("#sd_fax").val());
    $("#sd3_email").val($("#sd_email").val());
    $("#dataform").submit();
}

function MailPreview(itemKey)
{
    $("#sd4_ac_FONTE").val($("#sd_ac_FONTE").val());
    $("#sd4_tx_VALOREFILTROBASE").val($("#sd_tx_VALOREFILTROBASE").val());
    $("#sd4_tx_OGGETTO").val($("#sd_tx_OGGETTO").val());
    $("#sd4_ht_CORPO").val($("#sd_ht_CORPO").val());
    $("#sd4_ac_KEY").val(itemKey);
    $("#sd4_ragionesociale").val($("#sd_ragionesociale").val());
    $("#sd4_indirizzocompleto").val($("#sd_indirizzocompleto").val());
    $("#sd4_tel").val($("#sd_tel").val());
    $("#sd4_fax").val($("#sd_fax").val());
    $("#sd4_email").val($("#sd_email").val());
    $("#previewform").submit();

    stl_sel_display_wh_nourl(680, 700, null);

}
$(function () {
    stl_cus_grd_setupAllGridHeaders();
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cus_EndRequestHandler);
});

function stl_cus_grd_setupAllGridHeaders() {
    $("div.stl_cus_upg").each(function (index, element) {
        stl_cus_grd_setupGridHeader(index, element);
    });
}

function cus_EndRequestHandler(sender, args) {
    //errore server
    if (args.get_error() != undefined) {
        return;
    }

    //in caso di errore, resetto tutte le variabili e mi fermo
    if (stl_appb_error == true) {
        return;
    }

    //impongo la risistemazione degli header per tutte le griglie che vengono aggiornate
    stl_cus_grd_setupAllGridHeaders();
}

function SelAll()
{
    $(".selezcb input").prop("checked", true);
}

function SelNone() {
    $(".selezcb input").prop("checked", false);
}

function ConfirmSpedizione() {

    var totsel = $(".selezcb input:checkbox:checked").length;
    if (totsel == 0) {
        alert("Seleziona almeno un destinatario.");
        return false;
    } else {
        if (!confirm("Confermi la spedizione di " + totsel + " e-mail?")) return false;
    }

}