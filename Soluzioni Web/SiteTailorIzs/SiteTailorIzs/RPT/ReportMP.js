function editFiltro(fonte) {
    $("#rbFiltroPers").prop("checked", true);
    stl_sel_display_wh('EditorFiltro.aspx?load=parent&fonte=' + fonte, 1100, 850, editFiltro_CallBack);
}
function editFiltro_CallBack(codice) {
    if (codice != '') {
        $("#xmlFiltroPers").val(codice);
    }
}
function getXmlFiltro() {
    return $("#xmlFiltroPers").val();
}