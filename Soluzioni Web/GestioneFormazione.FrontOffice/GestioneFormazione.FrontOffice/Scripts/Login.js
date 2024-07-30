var login_button;
var login_box;
var login_open;
var login_txt_username;
var login_txt_password;
var login_lnk_login;

$(function () {
    login_open = false;
    setupLogin(null, null);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setupLogin);
});

function displayLogin(display)
{
    if (display) {
        login_box.slideToggle(200);
        login_open = true;
        login_button.toggleClass("act");
        login_button.toggleClass("enab");
        $("#login_txt_username").focus();
    }
    else {
        login_box.slideToggle(200);
        login_open = false;
        login_button.toggleClass("act");
        login_button.toggleClass("enab");
    }
}
function setupLogin(sender, args)
{
    login_button = $('#mnulogin');
    login_box = $('#loginddn');

    login_button.off();
    login_button.mouseup(function (ev) {
        if (!login_open) {
            displayLogin(true);
        }
        else {
            displayLogin(false);
        }
    });
    $(this).off();
    $(this).mouseup(function (ev) {
        if (login_open) {
            var tgt = $(ev.target);
            if (
                    tgt.prop('id') != 'loginddn' &&
                    tgt.prop('id') != 'mnulogin' &&
                    tgt.parents("#loginddn").length == 0) {
                displayLogin(false);
            }
        }
    });

    login_txt_username = $("#login_txt_username");
    login_txt_password = $("#login_txt_password");
    login_lnk_login = $("#login_lnk_login");

    login_txt_username.off();
    login_txt_username.keydown(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            focusEnter();
        };
    });
    login_txt_password.off();
    login_txt_password.keydown(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            focusEnter();
        };
    });
}
function focusEnter()
{
    login_lnk_login.focus();
}
