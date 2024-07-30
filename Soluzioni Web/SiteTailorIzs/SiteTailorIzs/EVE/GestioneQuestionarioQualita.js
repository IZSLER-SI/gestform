function gonext(item, nextitem) {

    if (item.value.length == 1) {
        if ($get(nextitem)) {
            $get(nextitem).focus();
        }
    }
    
}