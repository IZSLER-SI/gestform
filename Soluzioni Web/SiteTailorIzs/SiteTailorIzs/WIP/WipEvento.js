var valueStringPopup;
var valueIntPopup;
var valueDatePopup;
var valueBooleanPopup;
var valueMoneyPopup;
var wipTBCPopup;
var wipOKPopup;
var wipKOPopup;
var wipNNPopup;
var wip;
var activeItem;
var validator;

 //funzione selezione range
$.fn.selectRange = function (start, end) {
    return this.each(function () {
        if (this.setSelectionRange) {
            this.focus();
            this.setSelectionRange(start, end);
        } else if (this.createTextRange) {
            var range = this.createTextRange();
            range.collapse(true);
            range.moveEnd('character', end);
            range.moveStart('character', start);
            range.select();
        }
    });
};

//funzione refresh parent
function reloadParent() {
    if (window.opener) {
        if (window.opener.loadWipEvento) {
            window.opener.loadWipEvento();
        }
    }

}

$(function () {

    popupActive = false;

    //aggancio i popup e i vari elementi
    wip = $("#wip");
    valueStringPopup = $("#valueStringPopup");
    valueIntPopup = $("#valueIntPopup");
    valueDatePopup = $("#valueDatePopup");
    valueBooleanPopup = $("#valueBooleanPopup");
    valueMoneyPopup = $("#valueMoneyPopup");
    wipTBCPopup = $("#wipTBCPopup");
    wipOKPopup = $("#wipOKPopup");
    wipKOPopup = $("#wipKOPopup");
    wipNNPopup = $("#wipNNPopup");

    //setup validator
    jQuery.validator.addMethod(
		"italianDate",
		function (value, element) { return this.optional(element) || value == "__/__/____" || Date.parseExact(value, "d/M/yyyy"); },
		"Data non valida"
	);

    jQuery.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.]\d{3})+)(?:[\,]\d+)?$/.test(value);
    }

    //validazione campi field
    validator = $("#form1").validate({
        rules: {
            valueInt: { digits: true },
            //valueDate: { italianDate: true },
            valueMoney: { number: true }
        },
        errorPlacement: function (error, element) { }
    });

    //evento clic su tutti i field
    $("div[data-controltype='parentfield']").click(function (e) {
        showValuePopup($(this));
        e.stopPropagation();
    });

    //evento clic su tutti i wip parent e child
    $("div[data-controltype='wip']").click(function (e) {
        showWipPopup($(this));
        e.stopPropagation();
    });

    //evento clic su tutti i popup
    $(".popup").click(function (e) {
        e.stopPropagation();
    });

    //clic su tutti gli annulla dei popup
    $(".cancel").click(function (e) {
        setActiveItem(null);
        hideAllPopup();
        e.stopPropagation();
    });

    //nascondi
    $(document).click(function (e) {
        hideAllPopup();
        setActiveItem(null);
    });

    //salvataggi
    $("#saveString").click(function () { saveValue("String"); });
    $("#saveInt").click(function () { saveValue("Int"); });
    $("#saveDate").click(function () { saveValue("Date"); });
    $("#saveBoolean").click(function () { saveValue("Boolean"); });
    $("#saveMoney").click(function () { saveValue("Money"); });


    //completamenti
    $("#TBC_OK").click(function () { Wip_OK(); });
    $("#KO_OK").click(function () { Wip_OK(); });
    $("#NN_OK").click(function () { Wip_OK(); });

    //non necessari
    $("#TBC_NN").click(function () { Wip_NN(); });
    $("#KO_NN").click(function () { Wip_NN(); });
    $("#OK_NN").click(function () { Wip_NN(); });

    //nuova scadenza
    $("#TBC_NEWSCAD").click(function () { Wip_NEWSCAD("#TBC_NewExpiry"); });
    $("#KO_NEWSCAD").click(function () { Wip_NEWSCAD("#KO_NewExpiry"); });

    //da completare
    $("#NN_TBC").click(function () { Wip_TBC(); });
    $("#OK_TBC").click(function () { Wip_TBC(); });

    //nascondo tutti i popup
    hideAllPopup();
   
});

function hideAllPopup() {
    valueStringPopup.hide();
    valueIntPopup.hide();
    valueDatePopup.hide();
    valueBooleanPopup.hide();
    valueMoneyPopup.hide();
    wipTBCPopup.hide();
    wipOKPopup.hide();
    wipKOPopup.hide();
    wipNNPopup.hide();
}

function setActiveItem(what) {

    //classe
    if (activeItem) {
        activeItem.toggleClass("inactive");
        activeItem.toggleClass("active");
    }

    if (what) {
        what.toggleClass("inactive");
        what.toggleClass("active");
    }

    activeItem = what;

}
function showValuePopup(value) {
    //se ho ri-cliccato > nascondo
    if (value.is(activeItem)) {
        setActiveItem(null);
        hideAllPopup();
        return;
    }

    setActiveItem(value);

    //nascondo tutti
    hideAllPopup();

    //quale popup?
    switch (value.data("datatype")) {
        case "string":
            showValueStringPopup(value);
            break;
        case "int":
            showValueIntPopup(value);
            break;
        case "date":
            showValueDatePopup(value);
            break;
        case "boolean":
            showValueBooleanPopup(value);
            break;
        case "money":
            showValueMoneyPopup(value);
            break;
    }
}

function showValueStringPopup(value) {

    //mostro il popup
    valueStringPopup.show();

    //scrittura dati
    var pValue = $("#valueString");
    var plabel = $("#labelString");
    var pUpdated = $("#updatedString");

    pValue.val(value.data("value"));
    plabel.text(value.data("label"));

    setUpdatedText(value, pUpdated);

    //posiziono
    positionPopup(value, valueStringPopup, pValue);
}

function showValueIntPopup(value) {

    //mostro il popup
    valueIntPopup.show();

    //scrittura dati
    var pValue = $("#valueInt");
    var plabel = $("#labelInt");
    var pUpdated = $("#updatedInt");

    pValue.val(value.data("value"));
    plabel.text(value.data("label"));

    setUpdatedText(value, pUpdated);

    //posiziono
    positionPopup(value, valueIntPopup, pValue);
} 

function showValueDatePopup(value) {

    //mostro il popup
    valueDatePopup.show();

    //scrittura dati
    var pValue = $("#valueDate");
    var plabel = $("#labelDate");
    var pUpdated = $("#updatedDate");

    pValue.val(value.data("value"));
    plabel.text(value.data("label"));
    setUpdatedText(value, pUpdated);

    //calendario
    doCal(pValue, $("#valueDateCalendar"), true, true);

    //posiziono
    positionPopup(value, valueDatePopup, pValue);

}

function showValueBooleanPopup(value) {

    //mostro il popup
    valueBooleanPopup.show();

    //scrittura dati
    var pValue = $("#valueBoolean");
    var plabel = $("#labelBoolean");
    var pUpdated = $("#updatedBoolean");

    if (value.data("value") == "1") {
        pValue.prop("checked", true);
    }
    else {
        pValue.prop("checked", false);
    }

    pValue.val(value.data("value"));
    plabel.text(value.data("label"));

    setUpdatedText(value, pUpdated);

    //posiziono
    positionPopup(value, valueBooleanPopup, pValue);
}

function showValueMoneyPopup(value) {

    //mostro il popup
    valueMoneyPopup.show();

    //scrittura dati
    var pValue = $("#valueMoney");
    var plabel = $("#labelMoney");
    var pUpdated = $("#updatedMoney");

    pValue.val(value.data("value"));
    plabel.text(value.data("label"));

    setUpdatedText(value, pUpdated);

    //posiziono
    positionPopup(value, valueMoneyPopup, pValue);
}

function setUpdatedText(divData, divText) {
    var uOn = divData.data("updatedon");
    var uBy = divData.data("updatedby");

    if (uOn != "") {
        divText.html("Ultimo aggiornamento: <b>" + uBy + "</b> - <b>" + uOn + "</b>");
    }
    else {
        divText.html("");
    }
}

function positionPopup(value, popup, toFocus) {

    var offsetY = 2;
    var valueTop = value.offset().top;
    var valueBottom = value.offset().top + value.outerHeight();
    var valueLeft = value.offset().left;

    var popupHeight = popup.outerHeight();
    var popupWidth = popup.outerWidth();
    var viewportHeight = $(window).height();
    var viewportScrollTop = $(document).scrollTop();
    var viewportWidth = $(window).width();
    var viewportScrollLeft = $(document).scrollLeft();

    var destLeft = valueLeft;
    if ((valueLeft + popupWidth) > viewportWidth) {
        destLeft = viewportWidth - popupWidth;
    }

    //sotto o sopra?
    if (valueBottom + popupHeight > viewportScrollTop + viewportHeight) {
        //sopra
        popup.offset({
            top: valueTop - popupHeight + offsetY,
            left: destLeft
        });
    }
    else {
        //sotto
        popup.offset({
            top: valueBottom - offsetY,
            left: destLeft
        });
    }

    //selezione testo: cursore all'inizio
    //rimuovo eventuale colore di sfondo
    //ma solo se ho un toFocus
    if (toFocus) {
        if (value.data("datatype") != "boolean") {
            toFocus.css("background-color", "#ffffff");
            toFocus.selectRange(0, 0);
        }
        //focus
        toFocus.focus();
    }
}
function saveValue(dataType)
{
    var dataControl = $("#value" + dataType);

    //è valido? non mi preoccupo dei boolean...
    if(!validator.element(dataControl)) {
        dataControl.css("background-color", "#ffff00");
        return;
    }

    var div = activeItem;
    var value;
    if (dataType == "Boolean") {
        if (dataControl.is(":checked")) { value = "1"; } else { value = "0"; }
    }
    else {
        value = dataControl.val();
    }

    $.ajax({
        url: "WipEvento.aspx/SetValue",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_SCHEDA: wip.data("formid"),
            id_ITEM: div.data("itemid"),
            ac_TIPODATO: dataType,
            xx_DATO: value
        }),
        success: function(data) {
            setFieldData(div, data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
    
}

function setFieldData(div, data)
{
    //dati
    div.data("value", data.d.value);
    div.data("updatedon", data.d.updatedon);
    div.data("updatedby", data.d.updatedby);

    //valore: e se è BOOLEAN?
    if (div.data("datatype") == "boolean") {
        if (data.d.value == "1") {
            div.text("X");
        }
        else {
            div.html("&nbsp;");
        }
    }
    else {
        div.text(data.d.value);
    }

    setActiveItem(null);
    hideAllPopup();
}

function showWipPopup(wipElement) {

    //se ho ri-cliccato > nascondo
    if (wipElement.is(activeItem)) {
        setActiveItem(null);
        hideAllPopup();
        return;
    }

    setActiveItem(wipElement);

    //nascondo tutti
    hideAllPopup();

    //quale popup?
    switch (wipElement.data("statuscode")) {
        case "TBC":
            showWipTBC(wipElement);
            break;
        case "OK":
            showWipOK(wipElement);
            break;
        case "KO":
            showWipKO(wipElement);
            break;
        case "NN":
            showWipNN(wipElement);
            break;
    }
}

function showWipTBC(wipElement) {

    //mostro il popup
    wipTBCPopup.show();

    //titolo e secondo titolo
    $("#TBCTitle").text(wipElement.data("label"));
    if (wipElement.data("childlabel"))
        { $("#TBCTitle2").text(wipElement.data("childlabel")); }
    else
        { $("#TBCTitle2").text(""); }
    //data di scadenza
    $("#TBCExpiry").text(
        wipElement.data("expiry") + 
        ' (' + wipElement.data("daystoexpiry") +
        (wipElement.data("daystoexpiry") == 1 ? " giorno)" : " giorni)")
    );

    //riporto la data di scadenza nel textbox
    $("#TBC_NewExpiry").val(wipElement.data("expiry"));

    //calendario
    doCal($("#TBC_NewExpiry"), $("#TBC_Calendar"), true, false);

    //posiziono
    positionPopup(wipElement, wipTBCPopup, null);


}

function showWipOK(wipElement) {

    //mostro il popup
    wipOKPopup.show();

    //titolo e secondo titolo
    $("#OKTitle").text(wipElement.data("label"));
    if (wipElement.data("childlabel"))
        { $("#OKTitle2").text(wipElement.data("childlabel")); }
    else
        { $("#OKTitle2").text(""); }

    //completato da
    $("#OKCompletedOn").text(wipElement.data("updatedon"));
    $("#OKCompletedBy").text(wipElement.data("updatedby"));

    //posiziono
    positionPopup(wipElement, wipOKPopup, null);

}
function showWipKO(wipElement) {

    //mostro il popup
    wipKOPopup.show();

    //titolo e secondo titolo
    $("#KOTitle").text(wipElement.data("label"));
    if (wipElement.data("childlabel"))
    { $("#KOTitle2").text(wipElement.data("childlabel")); }
    else
    { $("#KOTitle2").text(""); }
    //data di scadenza
    $("#KOExpiry").text(
        wipElement.data("expiry") +
        ' (' + (-wipElement.data("daystoexpiry")) +
        (wipElement.data("daystoexpiry") == -1 ? " giorno fa)" : " giorni fa)")
    );

    //riporto la data di scadenza nel textbox
    $("#KO_NewExpiry").val(wipElement.data("expiry"));

    //calendario
    doCal($("#KO_NewExpiry"), $("#KO_Calendar"), true, false);

    //posiziono
    positionPopup(wipElement, wipKOPopup, null);

}
function showWipNN(wipElement) {

    //mostro il popup
    wipNNPopup.show();

    //titolo e secondo titolo
    $("#NNTitle").text(wipElement.data("label"));
    if (wipElement.data("childlabel"))
    { $("#NNTitle2").text(wipElement.data("childlabel")); }
    else
    { $("#NNTitle2").text(""); }

    //completato da
    $("#NNCompletedOn").text(wipElement.data("updatedon"));
    $("#NNCompletedBy").text(wipElement.data("updatedby"));

    //posiziono
    positionPopup(wipElement, wipNNPopup, null);

}

function doCal(textbox, div, showToday, showClear)
{
    textbox.Zebra_DatePicker({
        always_visible: div,
        format: "d/m/Y",
        header_captions: {
            'days': 'F Y',
            'months': 'Y',
            'years': 'Y1 - Y2'
        },
        days: ['Domenica', 'Lunedì', 'Martedì', 'Mercoledì', 'Giovedì', 'Venerdì', 'Sabato'],
        months: ['Gennaio', 'Febbraio', 'Marzo', 'Aprile', 'Maggio', 'Giugno', 'Luglio', 'Agosto', 'Settembre', 'Ottobre', 'Novembre', 'Dicembre'],
        show_clear_date: showClear,
        lang_clear_date: "Cancella data",
        show_select_today: (showToday ? "Oggi" : false),
        select_other_months: true
    });
}

//salvataggi vari
function Wip_OK() {

    var div = activeItem;

    $.ajax({
        url: "WipEvento.aspx/SetWipOK",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_SCHEDA: wip.data("formid"),
            id_ITEM: div.data("itemid"),
            id_ITEM_FIGLIO: div.data("childitemid")
        }),
        success: function (data) {
            setWipData(div, data);
            reloadParent();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });

}

function Wip_NN() {

    var div = activeItem;

    $.ajax({
        url: "WipEvento.aspx/SetWipNN",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_SCHEDA: wip.data("formid"),
            id_ITEM: div.data("itemid"),
            id_ITEM_FIGLIO: div.data("childitemid")
        }),
        success: function (data) {
            setWipData(div, data);
            reloadParent();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });

}


function Wip_TBC() {

    var div = activeItem;

    $.ajax({
        url: "WipEvento.aspx/SetWipTBC",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_SCHEDA: wip.data("formid"),
            id_ITEM: div.data("itemid"),
            id_ITEM_FIGLIO: div.data("childitemid")
        }),
        success: function (data) {
            setWipData(div, data);
            reloadParent();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });

}

function Wip_NEWSCAD(dateBoxSelector) {
    var div = activeItem;

    $.ajax({
        url: "WipEvento.aspx/SetWipDeadline",
        type: "POST",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            id_SCHEDA: wip.data("formid"),
            id_ITEM: div.data("itemid"),
            id_ITEM_FIGLIO: div.data("childitemid"),
            dt_SCADENZA: $(dateBoxSelector).val()
        }),
        success: function (data) {
            setWipData(div, data);
            reloadParent();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
}

function setWipData(div, data) {
    //dati
    div.data("statuscode", data.d.statuscode);
    div.data("expiry", data.d.expiry);
    div.data("daystoexpiry", data.d.daystoexpiry);
    div.data("updatedon", data.d.updatedon);
    div.data("updatedby", data.d.updatedby);

    //set classe
    div.attr("class", "wip active wip_" + data.d.statuscode);

    setActiveItem(null);
    hideAllPopup();

}