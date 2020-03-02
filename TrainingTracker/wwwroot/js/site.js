
function scrollBottom() {

    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');

};

function scrollUp() {

    $('html, body').animate({ scrollTop: 0 }, 'slow');

};

//appends an "active" class to .popup and .popup-content when the "Open" button is clicked
$(".open").on("click", function() {
    $(".popup-content").addClass("active");
    $(popupWarning).html("<span class=\"alert-success\">Unlocked.</span> <br> Proceed with caution.")
    $(openPopup).hide();
    $(closeButton).css("visibility", "unset");
    $(popupLockText).hide();
    $(popupContainer).css("height", "100%");
});

$(closeButton).on("click", function() {
    $(".popup-content, .popup-overlay").removeClass("active");
    $(popupWarning).html("<span class=\"alert-danger\">Locked.</span> <br> Proceed with caution.")
    $(openPopup).show();
    $(closeButton).css("visibility", "hidden");
    $(popupLockText).show();
    $(popupContainer).css("height", "0");
});
