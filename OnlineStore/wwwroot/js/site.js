$(function () {
    if ($("a.confirm-deletion").length) {
        $("a.confirm-deletion").click(() => {
            if (!confirm("Confirm deletion")) {
                return false;
            }
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});