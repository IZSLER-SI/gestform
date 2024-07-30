//oggetti
var coveringDiv;
var waitingDiv;

$(function () {
    coveringDiv = $("#coveringDiv");
    waitingDiv = $("#waitingDiv");
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //setup helpers
    stl_inputhelpers_SetupAllInputHelpers();
});

function BeginRequestHandler(sender, args) {
    ActivateWaitingDiv(true);
}
function EndRequestHandler(sender, args) {
    ActivateWaitingDiv(false);
    //errore server
    if (args.get_error() != undefined) {
        var errorMessage = args.get_error().message;
        alert(errorMessage);
        return;
    }

    //ri-setup helpers
    stl_inputhelpers_SetupAllInputHelpers();

}

function ActivateWaitingDiv(activate) {

    if (activate) {
        coveringDiv.css("display", "block");
        waitingDiv.css("display", "block");
    }
    else {
        //disattivazione
        coveringDiv.css("display", "none");
        waitingDiv.css("display", "none");
    }
}

function stl_inputhelpers_SetupAllInputHelpers() {
    //date dd/mm/yyyyy
    $(".stl_dt_data_ddmmyyyy").mask("?99/99/9999");
    //ore HH:mm
    $(".stl_dt_ora_hhmm").mask("?99.99");
}