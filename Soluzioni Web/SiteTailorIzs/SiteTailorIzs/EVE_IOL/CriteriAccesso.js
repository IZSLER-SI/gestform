function editCriterio(id)
{
    stl_sel_display_wh('CriterioAccesso.aspx?id=' + id, 1102, 850, editCriterio_callback);
}
function addCriterio()
{
    stl_sel_display_wh('CriterioAccesso.aspx?id=0', 1102, 850, editCriterio_callback);
}