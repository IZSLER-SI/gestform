function confirmClose()
{
    if (confirm('Confermi l\'abbandono di eventuali dati inseriti/modificati?'))
        parent.stl_sel_done('');
}
$(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetupEvents);
    SetupEvents();
})

function SetupEvents()
{
    $("#rblac_DIPEXT_0").change(HandleDipExt);
    $("#rblac_DIPEXT_1").change(HandleDipExt);
    $("#rblac_DIPEXT_2").change(HandleDipExt);
    $("#rblac_DIPEXT_3").change(HandleDipExt);

    $("#rblfl_PROFILI_0").change(HandleProfili);
    $("#rblfl_PROFILI_1").change(HandleProfili);

    $("#spanSave").click(HandleSave);
}
function HandleDipExt()
{
    if($("#rblac_DIPEXT_3").prop("checked") == true)
        $("#uocontainer").css("display","block");
    else
        $("#uocontainer").css("display", "none");
}
function HandleProfili() {
    if ($("#rblfl_PROFILI_1").prop("checked") == true)
        $("#profilocontainer").css("display", "block");
    else
        $("#profilocontainer").css("display", "none");
}
function HandleSave()
{
    var uosel = 1;
    var prsel = 1;
    var valid=true;

    //validazione UO
    if ($("#rblac_DIPEXT_3").prop("checked") == true) {
        uosel = 0;
        $("#uocontainer input[type='checkbox']").each(function (index, element) {
            if ($(element).prop("checked") == true) uosel = uosel + 1;
        });
    }

    //validazione profilo
    if ($("#rblfl_PROFILI_1").prop("checked") == true) {
        prsel = 0;
        $("#profilocontainer input[type='checkbox']").each(function (index, element) {
            if ($(element).prop("checked") == true) prsel = prsel + 1;
        });
    }

    if (uosel == 0 && prsel == 0) {
        valid = false;
        alert('Devi selezionare almeno un\'unità operativa e almeno un profilo.');
    }
    if (uosel > 0 && prsel == 0) {
        valid = false;
        alert('Devi selezionare almeno un profilo.');
    }
    if (uosel == 0 && prsel > 0) {
        valid = false;
        alert('Devi selezionare almeno un\'unità operativa.');
    }

    if(valid) invokeSave();
}