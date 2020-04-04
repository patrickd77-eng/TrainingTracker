/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Contains scroll button functionality for reuse on several pages.
 |              
 *===========================================================================*/
function scrollBottom() {

    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');

};

function scrollUp() {

    $('html, body').animate({ scrollTop: 0 }, 'slow');

};
