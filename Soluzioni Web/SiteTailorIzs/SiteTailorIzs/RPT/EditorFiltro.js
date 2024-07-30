function cblFiltroAll(id) {
    $("#" + id + " input").prop("checked", true);
}
function cblFiltroNone(id) {
    $("#" + id + " input").prop("checked", false);
}
function LoadFiltroFromParent() {
    //carico XML dal parent
    var xml = parent.getXmlFiltro();
    $("#txtParentXml").val(xml);
    //lancio clic pulsante
    ClickLoadParent();
}