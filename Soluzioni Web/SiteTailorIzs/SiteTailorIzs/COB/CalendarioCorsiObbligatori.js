$(function () {
    $(".head").click(function () {
        var head = $(this);
        var key = head.data("groupid");
        var body = $("div.body[data-groupid='" + key + "']");
        var expanded = head.data("expanded");
        if (expanded == "1") {
            body.slideUp(100);
            head.find(".arrow").html("►");
            head.data("expanded", "0");
        } else {
            body.slideDown(100);
            head.find(".arrow").html("▼");
            head.data("expanded", "1");
        }
    });
});

function openDetail(tcb, key, value) {

    var url = "DettaglioTipoCorso.aspx?tcb=" + tcb + "&" + key + "=" + value;
    stl_sel_display_wh(url, 1102, 850, openDetail_callback);

}

function openDetail_callback(code) {
}