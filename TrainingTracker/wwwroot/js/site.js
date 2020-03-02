
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
    $(popupLockText).hide();
    $(popupContainer).css("height", "100%");
});
