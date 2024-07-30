function stl_cus_grd_setupGridHeader(index, element) {
    //posizionamento (element è l'updatepanel griglia)
    var container = $(element);

    //esco se il container non è visible (per i panel)
    if (container.is(":hidden")) return;

    var containerPosition = container.position();

    var table = container.find("table.stl_cus_grd");
    var tableWidth = table.width();

    var headerRow = container.find("tr.h");
    var outHeaderRow = headerRow.clone(false);

    /* clonazione titolo normale, se c'è */
    var title = container.find("div.stl_cus_grd_hdr");
    if (title.length > 0) {
        var titleId = title.prop("id");
        var outTitle = title.clone(false);
        var outTitleId = titleId + "_out";

        outTitle.prop("id", outTitleId);
        outTitle.addClass("stl_cus_grd_hdr_out");
        outTitle.removeClass("stl_cus_grd_hdr");
        outTitle.css({
            position: "absolute",
            display: "block",
            top: containerPosition.top + 1,
            left: containerPosition.left + 1,
            width: tableWidth - 6
        });
        $("#" + outTitleId).detach();
        container.after(outTitle);
    }

    /* clonazione titolo "grigio", se c'è */
    var titleGrey = container.find("div.stl_cus_grd_hdr_grey");
    if (titleGrey.length > 0) {
        var titleGreyId = titleGrey.prop("id");
        var outTitleGrey = titleGrey.clone(false);
        var outTitleGreyId = titleGreyId + "_out";

        outTitleGrey.prop("id", outTitleGreyId);
        outTitleGrey.addClass("stl_cus_grd_hdr_grey_out");
        outTitleGrey.removeClass("stl_cus_grd_hdr_grey");
        outTitleGrey.css({
            position: "absolute",
            display: "block",
            top: containerPosition.top + 1,
            left: containerPosition.left + 1,
            width: tableWidth
        });
        $("#" + outTitleGreyId).detach();
        container.after(outTitleGrey);
    }

    /* clonazione footer, se c'è */
    var footer = container.find("div.stl_cus_grd_ftr");
    if (footer.length > 0) {
        var footerId = footer.prop("id");
        var outFooter = footer.clone(true);
        var outFooterId = titleId + "_out";
        //elimino eventuali controlli asp.net che altrimenti verrebbero duplicati
        footer.find("select").detach();
        outFooter.prop("id", outFooterId);
        outFooter.addClass("stl_cus_grd_ftr_out");
        outFooter.removeClass("stl_cus_grd_ftr");
        outFooter.css({
            position: "absolute",
            display: "block",
            top: containerPosition.top + container.height() - 24,
            left: containerPosition.left + 1,
            width: tableWidth
        });
        $("#" + outFooterId).detach();
        container.after(outFooter);
    }

    var outHeaderId = titleId + "_outhdr";
    var outHeader = $("<table class=\"stl_cus_outgrd\"></table>");
    outHeader.prop("id", outHeaderId);
    outHeader.append(outHeaderRow);

    var origCols = headerRow.find("th");
    var destCols = outHeaderRow.find("th");

    origCols.each(function (index, element) {
        $(destCols[index]).css({ width: $(element).width() });
    });

    var outHeaderTop;
    if (titleGrey.length == 0 && title.length == 0)
        outHeaderTop = containerPosition.top + 1;
    else
        outHeaderTop = containerPosition.top + 24;

    outHeader.css({
        position: "absolute",
        display: "block",
        top: outHeaderTop,
        left: containerPosition.left + 1,
        width: tableWidth
    });

    //distruzione copie precedentemente create

    $("#" + outHeaderId).detach();
    container.after(outHeader);
}