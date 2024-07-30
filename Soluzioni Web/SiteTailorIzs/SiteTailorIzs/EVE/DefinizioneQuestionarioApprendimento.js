function gonext(item, nextitem) {

    var baseId = item.id.substr(0, item.id.length - 3);
    if (item.value.length == 1) {
        if ($get(baseId + nextitem)) {
            $get(baseId + nextitem).focus();
        }
    }
    
}