//rotella attesa
var stl_ajaxwaiting;

//popup
var stl_popup_covering;
var stl_popup_iframe;

//variabili per gestione postback asincrono e griglie
var stl_appb_row2Select = null;
var stl_appb_row2Select_gridId = null;
var stl_appb_row2Select_rowIdx = null;
var stl_appb_row2Select_action = null;
var stl_appb_grid2Unselect_gridId = null;
var stl_appb_error = false;
var stl_appb_panels = null;

//variabili per confirm and postback
var capMSG = "";
var capCID = "";
var capARG = "";

//gestione messaggio post-postback per ricerche
function setCAP(MSG, CID, ARG) {
    capMSG = MSG;
    capCID = CID;
    capARG = ARG;
}


$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoadedHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    stl_grd_setupAllGridHeaders();
    stl_grd_attachRowClickEvents();
    //setup helpers
    stl_inputhelpers_SetupAllInputHelpers();
    //setup validatori
    stl_inputhelpers_SetupValidatorCriteria();
    stl_ajaxwaiting = $("#ajaxwaiting");
    $(window).resize(function () {
        position_stl_sel(false);
    });
});

function BeginRequestHandler(sender, args) {
    stl_ajaxwaiting.css("display", "block");
    stl_appb_panels = null;
}

function PageLoadedHandler(sender, args) {
    stl_appb_panels = args.get_panelsUpdated();
}

function EndRequestHandler(sender, args) {
    stl_ajaxwaiting.css("display", "none");
    //errore server
    if (args.get_error() != undefined) {
        var errorMessage = args.get_error().message;
        alert(errorMessage);
        stl_appb_row2Select = null;
        stl_appb_grid2Unselect_gridId = null;
        stl_appb_error = false;
        capCID = "";
        capMSG = "";
        capARG = "";
        return;
    }

    //ri-setup helpers
    stl_inputhelpers_SetupAllInputHelpers();

    //in caso di errore, resetto tutte le variabili e mi fermo
    if (stl_appb_error == true) {
        stl_appb_row2Select = null;
        stl_appb_grid2Unselect_gridId = null;
        stl_appb_error = false;
        capCID = "";
        capMSG = "";
        capARG = "";
        return;
    }

    if (stl_appb_row2Select != null) {

        //selezione riga dopo postback
        stl_grd_clientsel(stl_appb_row2Select, stl_appb_row2Select_gridId);

        //azzero il dato che compare nell'IF
        stl_appb_row2Select = null;

        //cancellazione?
        if (stl_appb_row2Select_action == "Delete") {
            if (window.confirm(stl_grd_deleteConfirmationQuestion(stl_appb_row2Select_gridId))) {
                stl_grd_eventRouter(stl_appb_row2Select_gridId, stl_appb_row2Select_rowIdx, "Delete");
            }
        }
    };

    if (stl_appb_grid2Unselect_gridId != null) {

        //deselezione riga dopo postback
        stl_grd_clientunsel(stl_appb_grid2Unselect_gridId);

        //azzero il dato che compare nell'IF
        stl_appb_grid2Unselect_gridId = null;

    };

    //impongo la risistemazione degli header per tutte le griglie che vengono aggiornate
    if (stl_appb_panels != null) {
        for (i = 0; i < stl_appb_panels.length; i++) {
            var panel = $(stl_appb_panels[i]);
            if (panel.hasClass("stl_upg")) {
                stl_grd_setupGridHeader(0, panel);
            }
        }
    };

    
    //eseguo eventuali postback richiesti
    if (capCID != "") {
        //azzeramento immediato
        var myCID = capCID; capCID = "";
        var myMSG = capMSG; capMSG = "";
        var myARG = capARG; capARG = "";
        if (window.confirm(myMSG)) {
            __doPostBack(myCID, myARG);
        }
    };

}

function stl_grd_setupAllGridHeaders() {
    $("div.stl_upg").each(function (index, element) {
        stl_grd_setupGridHeader(index, element);
    });
}

function stl_tab_activetabchanged(sender, args) {
    //solo quando il tab diventa visibile, sistemo tutte le griglie
    $("#" + sender.get_activeTab().get_id()).find("div.stl_upg").each(function (index, element) {
        stl_grd_setupGridHeader(index, element);
    });
}

function stl_grd_clientsel(row, gridId) {
    var table = row.parent();
    var selClass = stl_grd_selectedRowClass(gridId);
    var selRow = table.find("tr." + selClass);
    selRow.addClass("r");
    selRow.removeClass(selClass);
    row.addClass(selClass);
    row.removeClass("r");
}

function stl_grd_clientunsel(gridId) {
    var table = $("#" + stl_grd_clientId(gridId));
    var selClass = stl_grd_selectedRowClass(gridId);
    var selRow = table.find("tr." + selClass);
    selRow.addClass("r");
    selRow.removeClass(selClass);
}

function stl_grd_clientscrolltop(gridId) {
    var table = $("#" + stl_grd_clientId(gridId));
    table.parents("div.stl_upg").scrollTop(0);
}

function stl_grd_clientensurevisible(gridId) {
    var table = $("#" + stl_grd_clientId(gridId));
    var selClass = stl_grd_selectedRowClass(gridId);
    var selRow = table.find("tr." + selClass);
    if (selRow.length) {
        var container = table.parents("div.stl_upg");
        if (selRow.position().top < 46)
        { container.scrollTop(container.scrollTop() + (selRow.position().top - 46)); }
        if (selRow.position().top + selRow.outerHeight() > container.innerHeight())
        { container.scrollTop(container.scrollTop() + (selRow.position().top + selRow.outerHeight() - container.innerHeight())); }
    }
}

function stl_grd_attachRowClickEvents() {
    //scatta anche ai reload successivi
    $("div.stl_upg").on("click", ".stl_grd .r td, .stl_grd .src td", function () {
        var origin = $(this);
        if (
            origin.hasClass("cmd1") ||
            origin.hasClass("cmd2") ||
            origin.hasClass("cmd3") ||
            origin.hasClass("cmd4") ||
            origin.hasClass("lastcmd")
        ) return;
        stl_grd_sel(origin);
    });
}

function stl_grd_sel(originCell) {

    //selezione di una riga
    var row = originCell.parent();
    var rowId = row.attr("id");
    var vals = rowId.split("$");
    var gridId = vals[0];
    var rowIdx = vals[1];

    //memorizzo l'origine e l'action
    stl_appb_row2Select = row;
    stl_appb_row2Select_gridId = gridId;
    stl_appb_row2Select_rowIdx = rowIdx;
    stl_appb_row2Select_action = "Select";

    //invoco il postback
    stl_grd_eventRouter(gridId, rowIdx, "Select");

}

function stl_grd_add(gridId) {

    //aggiunta di una riga

    //memorizzo l'origine e l'action
    stl_appb_grid2Unselect_gridId = gridId;

    //invoco il postback
    stl_grd_addNewRouter(gridId);

}

function stl_grd_edit(originSpan) {

    //selezione di una riga e contestuale editing
    var row = $(originSpan).parent().parent();
    var rowId = row.attr("id");
    var vals = rowId.split("$");
    var gridId = vals[0];
    var rowIdx = vals[1];

    //memorizzo l'origine e l'action
    stl_appb_row2Select = row;
    stl_appb_row2Select_gridId = gridId;
    stl_appb_row2Select_rowIdx = rowIdx;
    stl_appb_row2Select_action = "Edit";


    //invoco il postback
    stl_grd_eventRouter(gridId, rowIdx, "Edit");

}

function stl_grd_del(originSpan) {

    //invocazione di un select
    //selezione di una riga e contestuale editing
    var row = $(originSpan).parent().parent();
    var rowId = row.attr("id");
    var vals = rowId.split("$");
    var gridId = vals[0];
    var rowIdx = vals[1];

    //memorizzo l'origine e l'action
    stl_appb_row2Select = row;
    stl_appb_row2Select_gridId = gridId;
    stl_appb_row2Select_rowIdx = rowIdx;
    stl_appb_row2Select_action = "Delete";

    //invoco il postback con un select. il resto sarà eseguito da appb
    stl_grd_eventRouter(gridId, rowIdx, "Select");

}

function stl_grd_setupGridHeader(index, element) {
    //posizionamento (element è l'updatepanel griglia)
    var container = $(element);

    //esco se il container non è visible (per i panel)
    if (container.is(":hidden")) return;

    var containerPosition = container.position();

    var table = container.find("table.stl_grd");
    var tableWidth = table.width();

    var title = container.find("div.stl_grd_hdr");
    var titleId = title.prop("id");

    var headerRow = container.find("tr.h");

    var outTitle = title.clone(false);
    var outTitleId = titleId + "_out";
    var outHeaderRow = headerRow.clone(false);

    outTitle.prop("id", outTitleId);
    outTitle.addClass("stl_grd_hdr_out");
    outTitle.removeClass("stl_grd_hdr");
    outTitle.css({
        position: "absolute",
        display: "block",
        top: containerPosition.top + 1,
        left: containerPosition.left + 1,
        width: tableWidth - 6
    });

    var outHeaderId = titleId + "_outhdr";
    var outHeader = $("<table class=\"stl_outgrd\"></table>");
    outHeader.prop("id", outHeaderId);
    outHeader.append(outHeaderRow);

    var origCols = headerRow.find("th");
    var destCols = outHeaderRow.find("th");

    origCols.each(function (index, element) {
        $(destCols[index]).css({ width: $(element).width() });
    });

    outHeader.css({
        position: "absolute",
        display: "block",
        top: containerPosition.top + 24,
        left: containerPosition.left + 1,
        width: tableWidth
    });

    //distruzione copie precedentemente create
    $("#" + outTitleId).detach();
    $("#" + outHeaderId).detach();

    container.after(outTitle);
    container.after(outHeader);
}


//lancio errori
function stl_formview_error(formID, message, exceptionMessage) {
    stl_appb_error = true;
    document.getElementById(formID).style.backgroundColor = "#ffff99";
    window.alert(message + '\n\nErrore restituito:\n\n' + exceptionMessage);
}
function stl_formview_presaveerror(formID, message) {
    stl_appb_error = true;
    window.alert(message);
}
function stl_gridview_error(formID, message, exceptionMessage) {
    stl_appb_error = true;
    window.alert(message + '\n\nErrore restituito:\n\n' + exceptionMessage);
}
function stl_searchform_error(formID, message) {
    stl_appb_error = true;
    window.alert(message);
}

//gestione binaries
function stl_binaryelementbox_setID_ELEME(newID_ELEME) {
    //chiamata da AddBinary.aspx e da ChooseBinary.aspx 
    //a seguito di una selezione

    //leggo i valori di stato e li resetto
    var baseclientid = stl_binaryelementbox_currentbaseclientid;
    var clientidseparator = stl_binaryelementbox_currentclientidseparator;
    var refreshjsclientid = stl_binaryelementbox_refreshjsclientid;

    stl_binaryelementbox_currentbaseclientid = '';
    stl_binaryelementbox_currentclientidseparator = '';
    stl_binaryelementbox_refreshjsclientid = '';

    //imposto il valore e chiamo un refresh
    var ID_ELEME = $get(baseclientid + clientidseparator + 'ID_ELEME');
    ID_ELEME.value = newID_ELEME;
    __doPostBack(refreshjsclientid, '');
}

function stl_binaryelementbox_new(formbaseclientid, baseclientid, refreshjsclientid, clientidseparator, binariesbaseurl) {
    //apertura finestra
    var url;
    var CODCATEG;
    var PREAMBLE;
    var POSTAMBLE;
    var TEXTBOXID;
    var MIDDLE = '';
    var DESEL_TX;

    //costruzione dei dati per la chiamata alla finestra
    CODCATEG = $get(baseclientid + clientidseparator + 'hidDefaultCODCATEG').value;
    PREAMBLE = $get(baseclientid + clientidseparator + 'hidDefaultDescriptionPreamble').value;
    POSTAMBLE = $get(baseclientid + clientidseparator + 'hidDefaultDescriptionPostamble').value;
    TEXTBOXID = $get(baseclientid + clientidseparator + 'hidDefaultDescriptionSourceTextBoxClientID').value;
    if (TEXTBOXID != '') {
        //leggo il testo nel textbox
        MIDDLE = $get(TEXTBOXID).value.substring(0, 130);
    }
    DESEL_TX = PREAMBLE + MIDDLE + POSTAMBLE;

    //costruzione dell'URL
    url = binariesbaseurl + 'AddBinary.aspx' +
          '?CODCATEG=' + encodeURIComponent(CODCATEG) +
          '&DESEL_TX=' + encodeURIComponent(DESEL_TX);

    //imposto le variabili per l'eventuale chiamata
    stl_binaryelementbox_currentbaseclientid = baseclientid;
    stl_binaryelementbox_currentclientidseparator = clientidseparator;
    stl_binaryelementbox_refreshjsclientid = refreshjsclientid;

    stl_sel_display_wh(url, 580, 570, stl_binaryelementbox_setID_ELEME);

}
function stl_binaryelementbox_choose(formbaseclientid, baseclientid, refreshjsclientid, clientidseparator, binariesbaseurl) {
    //apertura finestra
    var url;
    var CODCATEG;

    //costruzione dei dati per la chiamata alla finestra
    CODCATEG = $get(baseclientid + clientidseparator + 'hidDefaultCODCATEG').value;

    //costruzione dell'URL
    url = binariesbaseurl + 'ChooseBinary.aspx' +
          '?CODCATEG=' + encodeURIComponent(CODCATEG);

    //imposto le variabili per l'eventuale chiamata
    stl_binaryelementbox_currentbaseclientid = baseclientid;
    stl_binaryelementbox_currentclientidseparator = clientidseparator;
    stl_binaryelementbox_refreshjsclientid = refreshjsclientid;

    stl_sel_display_wh(url, 900, 600, stl_binaryelementbox_setID_ELEME);

}
function stl_binaryelementbox_clear(formbaseclientid, baseclientid, refreshjsclientid, clientidseparator, binariesbaseurl) {
    //rimozione dell'ID_ELEME
    var ID_ELEME = $get(baseclientid + clientidseparator + 'ID_ELEME');
    if (ID_ELEME.value != '') {
        if (window.confirm('Confermi la rimozione dell\'immagine/allegato?')) {
            ID_ELEME.value = '';
            __doPostBack(refreshjsclientid, '');
        }
    }
}

//gestione selettori

var stl_sel_callback;

function position_stl_sel(alsoInvisible) {

    var doit = false;

    if (alsoInvisible) {
        doit = true;
    } else {
        stl_popup_iframe = $("#stl_popup_iframe");
        if (stl_popup_iframe.css("display") == "block") doit = true;
    }
    if (doit) {
        stl_popup_iframe.css({
            top: Math.max($(window).scrollTop(), (($(window).height() - stl_popup_iframe.outerHeight()) / 2) +
                                                $(window).scrollTop()),
            left: Math.max($(window).scrollLeft(), (($(window).width() - stl_popup_iframe.outerWidth()) / 2) +
                                                    $(window).scrollLeft())
        });
    }
}

function stl_sel_display_wh(url, width, height, callbackFunction) {
    //div coprente
    stl_popup_covering = $("<div id=\"stl_popup_covering\"></div>");
    $("body").append(stl_popup_covering);
    stl_popup_iframe = $("#stl_popup_iframe")

    stl_popup_iframe.css({
        "display": "block",
        "width": width,
        "height": height
    });
    position_stl_sel(true);
    //$("body").append(stl_popup_iframe);
    stl_popup_iframe.prop("src", url);
    stl_sel_callback = callbackFunction;
}

function stl_sel_display_wh_nourl(width, height, callbackFunction) {
    //div coprente
    stl_popup_covering = $("<div id=\"stl_popup_covering\"></div>");
    $("body").append(stl_popup_covering);
    stl_popup_iframe = $("#stl_popup_iframe")

    stl_popup_iframe.css({
        "display": "block",
        "width": width,
        "height": height
    });
    position_stl_sel(true);
    stl_sel_callback = callbackFunction;
}


function stl_sel_display(url, callbackFunction) {
    stl_sel_display_wh(url, 870, 460, callbackFunction);
}
function stl_sel_display_high(url, callbackFunction) {
    stl_sel_display_wh(url, 870, 580, callbackFunction);
}
function stl_sel_display_1280(url, callbackFunction) {
    stl_sel_display_wh(url, 1100, 700, callbackFunction);
}
function stl_sel_display_1280_768(url, callbackFunction) {
    stl_sel_display_wh(url, 1100, 590, callbackFunction);
}
function stl_sel_done(code) {
    //chiamo il callback, se ricevo un codice
    if (stl_sel_callback)
        if (code != '')
            stl_sel_callback(code);
    stl_popup_iframe.attr("src", "");
    stl_popup_iframe.css({
        "display": "none",
        "width": "10px",
        "height": "10px"
    });
    stl_popup_covering.detach();
}
function stl_sel_displaycover(display) {
    if (display) {
        //div coprente
        stl_popup_covering = $("<div id=\"stl_popup_covering\"></div>");
        $("body").append(stl_popup_covering);
    } else {
        stl_popup_covering.detach();
    }
}
//gestione scroll trees
function stl_tree_getScroll(divid, hidid) {
    $get(hidid).value =
    $get(divid).scrollTop;
}
function stl_tree_setScroll(divid, hidid) {
    $get(divid).scrollTop =
    $get(hidid).value;
}

//default button form ricerca
function stl_search_FireDefaultButton(event, target) {
    //focus
    if (event.keyCode == 13) {
        $get(target).focus();
        return WebForm_FireDefaultButton(event, target);
    }
}

//wopen
function wopen(url, name, w, h, status, toolbar, menubar, resizable, scrollbars) {
    //menu:1 o 0; resize: 1 o 0; scroll: 1 o 0; 
    w += 32;
    h += 96;
    wleft = (screen.width - w) / 2;
    wtop = (screen.height - h) / 2;
    if (wleft < 0) {
        w = screen.width;
        wleft = 0;
    }
    if (wtop < 0) {
        h = screen.height;
        wtop = 0;
    }
    var win = window.open(url,
      name,
      'width=' + w + ',height=' + h +
      ',left=' + wleft + ',top=' + wtop + ',status=' + status + ',toolbar=' + toolbar + ',menubar=' + menubar + ',resizable=' + resizable + ',scrollbars=' + scrollbars);
    win.focus();
}

//helpers
function stl_inputhelpers_SetupAllInputHelpers() {
    setupAutocompleteCapComuneProvincia();
    //date dd/mm/yyyyy
    $(".stl_dt_data_ddmmyyyy").mask("?99/99/9999");
    $(".stl_dt_ora_hhmm").mask("?99.99");
    $(".stl_dt_ora_hhmmss").mask("?99.99.99");
    $(".stl_dt_dataora_ddmmyyyyhhmmss").mask("?99/99/9999 99.99.99");
}

function setupAutocompleteCapComuneProvincia() {

    $(".stl_ac_capcomuneprovincia").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: stlPageWsBase + "AutoCompleteCapComuneProvincia.asmx/GetSuggestions",
                type: "POST",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    prefixText: request.term,
                    count: 20
                }),
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item,
                            value: item
                        }
                    }));
                }
            });
        },
        minLength: 3,
        delay: 100,
        position: { offset: '0 1', collision: "flip" }
    })
}

function stl_inputhelpers_SetupValidatorCriteria() {
    jQuery.validator.addMethod(
		"italianDate",
		function (value, element) { return this.optional(element) || value == "__/__/____" || Date.parseExact(value, "d/M/yyyy"); },
		"Data non valida"
	);

    jQuery.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.]\d{3})+)(?:[\,]\d+)?$/.test(value);
    }
}