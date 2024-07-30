$(function () {
    AggancioEventi(null, null);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(AggancioEventi);
});

function AggancioEventi(sender, args) {
    $("#id_QUOTAISCRIZIONEDropDownList").change(function () { cambioQuota(); });
    $("#id_IVADropDownList").change(function () { cambioIva(); });
}

function cambioQuota()
{
    /*
        se vuota svuoto tutto
        altrimenti
        recupero prezzo unitario
        recupero IVA default
        recupero totale default
        scrivo prezzo unitario
        scrivo IVA default
        scrivo totale default
        scrivo versato default
    */
    var idQuota = $("#id_QUOTAISCRIZIONEDropDownList").val();
    if (idQuota == "")
    {
        $("#id_IVADropDownList").val("");
        $("#txtImponibile").val("");
        $("#txtCostoTotale").val("");
        $("#mo_VERSATOTextBox").val("");
    }
    else
    {
        $.ajax({
            url: stlPageWsBase + "AutoCompleteQuote.asmx/GetDatiQuota",
            type: "POST",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                id_quotaiscrizione: idQuota,
                id_iva: ""
            }),
            success: function (data) {
                $("#id_IVADropDownList").val(data.d.id_iva);
                $("#txtImponibile").val(data.d.imponibile);
                $("#txtCostoTotale").val(data.d.totale);
                $("#mo_VERSATOTextBox").val(data.d.totale);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            }
        });
    }
}
function cambioIva()
{
    /*
        se quota vuota: 
            faccio nulla
        se quota popolata
            calcolo totale con nuova IVA
            riscrivo versato
   */
    var idQuota = $("#id_QUOTAISCRIZIONEDropDownList").val();
    var idIva = $("#id_IVADropDownList").val();
    if (idQuota != "")
    {
        if (idIva == "")
        {
            $("#txtCostoTotale").val("");
            $("#mo_VERSATOTextBox").val("");
        }
        else
        {
            $.ajax({
                url: stlPageWsBase + "AutoCompleteQuote.asmx/GetDatiQuota",
                type: "POST",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    id_quotaiscrizione: idQuota,
                    id_iva: idIva
                }),
                success: function (data) {
                    $("#txtImponibile").val(data.d.imponibile);
                    $("#txtCostoTotale").val(data.d.totale);
                    $("#mo_VERSATOTextBox").val(data.d.totale);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(thrownError);
                }
            });
        }
    }
}

