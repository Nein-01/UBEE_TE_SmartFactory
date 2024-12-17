$(document).ready(function () {    
    var tabLinks = $('.tabLinks');
    tabLinks[0].click();
})
// Tab view functions
function tabChange(evt, tabName) {
    var divTabID = 'divTab' + tabName;
    $('.tabLinks').removeClass('active');
    $('.tabcontent').css('display', 'none');
    $('#' + divTabID).css('display', 'block');
    evt.currentTarget.className += ' active';

}