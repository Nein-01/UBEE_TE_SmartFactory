// ====== Global Variables ======
var gblUrlSystemSite = 'client';
// ====== Global URLs ======
var URL_AdminDashboard = $('#URL_AdminDashboard').val();
var URL_HomePage = $('#URL_HomePage').val();
// ====== Events Handling Functions ======
$(window).on('load', function () {
    GET_ToastrNotification();
    GET_URLSystemSite();
});

$('.to-upper-text').on('input', function () {
        var org = $(this).val();
        var upperVal = org.toUpperCase();
        $(this).val(upperVal);

    });

$('#srcKey').on('keypress', function (e) {
    var key = e.which;
    if (key == 13) {
        $('#searchBtn').click();
        return false;
    }
});

// ====== Events Calling Functions ======
function GET_URLSystemSite() {
    let urlSystemSite = $('#urlSystemSite');
    let txtSystemSite = $('#txtSystemSite');
    gblUrlSystemSite = sessionStorage.getItem('systemsite');
    switch (gblUrlSystemSite) {        
        case 'admin':
            urlSystemSite.attr('href', URL_HomePage);
            txtSystemSite.html('Home Page');
            sessionStorage.setItem('systemsite', 'client');           
            break;
        default: 
            urlSystemSite.attr('href', URL_AdminDashboard);
            txtSystemSite.html('Manager Dashboard');
            sessionStorage.setItem('systemsite', 'admin');
            break;
    }
}
function GET_ToastrNotification() {
    // Get notify data
    const msg_type = $('input[name=MsgType]').val();
    const msg = $('input[name=Msg]').val();
    const msg1s_type = $('input[name=Msg1sType]').val();
    const msg1s = $('input[name=Msg1s]').val();
    // Setup popup options    
    toastr.options.positionClass = 'toast-top-center';

    // Notifi 2s
    if (msg_type != '' && msg != '') {
        toastr.options.timeOut = 2000
        switch (msg_type) {
            case 'success':
                toastr.success(msg, 'Success');
                break;
            case 'info':
                toastr.info(msg, 'Info');
                break;
            case 'warning':
                toastr.warning(msg, 'Warning');
                break;
            case 'danger':
                toastr.error(msg, 'Error');
                break;
        }
    }

    // Notify 1s
    if (msg1s_type != '' && msg1s != '') {
        toastr.options.timeOut = 1000
        switch (msg1s_type) {
            case 'success':
                toastr.success(msg1s, 'Success');
                break;
            case 'info':
                toastr.info(msg1s, 'Info');
                break;
            case 'warning':
                toastr.warning(msg1s, 'Warning');
                break;
            case 'danger':
                toastr.error(msg1s, 'Error');
                break;
        }
    }
};

function SearchAction() {
    var keyWord = $('#srcKey').val();
    var Url = $('#searchURL').val();

    $.ajax({
        url: Url,
        data: { searchStr: keyWord },
        success: function (response) {
            console.log(response);
            $('#srcKey').val('');
            $('#tableFill').html(response);
        },
        error: function (response) {
            alert('Error on calling function');
        }
    });
};
function onclick_NavigateURL(url) {
    window.location = url;
}
function onclick_FullScreen() {
    let isInFullScreen = (document.fullscreenElement && document.fullscreenElement !== null) ||
        (document.webkitFullscreenElement && document.webkitFullscreenElement !== null) ||
        (document.msFullscreenElement && document.msFullscreenElement !== null) ||
        (document.mozFullScreenElement && document.mozFullScreenElement !== null);

    let docEle = document.documentElement;
    if (isInFullScreen) {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
        if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }        
        if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
        if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        }
    }
    else {
        if (docEle.requestFullscreen) {
            docEle.requestFullscreen();
        }
        if (docEle.webkitRequestFullscreen) {
            docEle.webkitRequestFullscreen();
        }        
        if (docEle.msRequestFullscreen) {
            docEle.msRequestFullscreen();
        }
        if (docEle.mozRequestFullScreen) {
            docEle.mozRequestFullScreen();
        }
    }
};
function TEST_DesktopNotification() {
    //console.log(('Notification' in window));
    if (!('Notification' in window)) {
        alert('Browser does not support desktop notification');
    }
    Notification.requestPermission().then((permission) => {
        if (permission === 'granted') {
            const notification = new Notification('This is a testing line');
            console.log(notification);
        }
    });
    /*else if (Notification.permission === 'granted') {
        console.log('test');
        const notification = new Notification('This is a testing line');
        
    }
    else if (Notification.permission === 'denied') {
        Notification.requestPermission().then((permission) => {
            if (permission === 'granted') {
                const notification = new Notification('This is a testing line');
                console.log(notification);
            }
        });
    }*/
}