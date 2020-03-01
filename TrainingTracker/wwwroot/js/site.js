
function scrollBottom() {

    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');

};

function scrollUp() {

    $('html, body').animate({ scrollTop: 0 }, 'slow');

};


//appends an "active" class to .popup and .popup-content when the "Open" button is clicked
$(".open").on("click", function() {
    $(".popup-content").addClass("active");
    $(openPopup).hide();
});

//removes the "active" class to .popup and .popup-content when the "Close" button is clicked 
$(".close").on("click", function() {
    $(".popup-content").removeClass("active");
    $(openPopup).show();
});
