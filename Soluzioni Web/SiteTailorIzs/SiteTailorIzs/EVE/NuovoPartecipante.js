function tryIscriviEsistente(id_evento, id_persona, ac_categoriaecm) {
    $.ajax({
        url: stlPageWsBase + "ServiziEventi.asmx/VerificaIscrizionePersonaEsistente",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_EVENTO: id_evento,
            id_PERSONA: id_persona,
            ac_CATEGORIAECM: ac_categoriaecm
        }),
        success: function (data) {
            if (data.d.iscrizionePossibile) {
                //iscrizione possibile
                if (data.d.mostraWarning) {
                    if (confirm(data.d.testoErroreWarning + '\n\n' + data.d.testoRichiestaConferma)) invokeIscriviEsistente();
                }
                else {
                    //nessun warning necessario
                    invokeIscriviEsistente();
                }
            }
            else {
                //iscrizione non possibile
                alert(data.d.testoErroreWarning);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
}

function tryCreaIscriviNuovo(id_evento, ac_profilo, ac_categoriaecm) {
    $.ajax({
        url: stlPageWsBase + "ServiziEventi.asmx/VerificaIscrizionePersonaNuova",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_EVENTO: id_evento,
            ac_PROFILO: ac_profilo,
            ac_CATEGORIAECM: ac_categoriaecm
        }),
        success: function (data) {
            if (data.d.iscrizionePossibile) {
                //iscrizione possibile
                if (data.d.mostraWarning) {
                    if (confirm(data.d.testoErroreWarning + '\n\n' + data.d.testoRichiestaConferma)) invokeCreaIscriviNuovo();
                }
                else {
                    //nessun warning necessario
                    invokeCreaIscriviNuovo();
                }
            }
            else {
                //iscrizione non possibile
                alert(data.d.testoErroreWarning);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
}