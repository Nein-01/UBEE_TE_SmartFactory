// === AJAX Funtions URL ===
var URL_GET_PartialTestPlanTable = $('#URL_GET_PartialTestPlanTable').val();
var URL_GET_ListModelOfTestPlan = $('#URL_GET_ListModelOfTestPlan').val();
var URL_GET_ListTestPlanPreview = $('#URL_GET_ListTestPlanPreview').val();
var URL_GET_ListTestPlanPreviewByProjectType = $('#URL_GET_ListTestPlanPreviewByProjectType').val();
// === Global variables ===
var gbl_bFlagError;
var gbl_arrTestPlanProjectType = ['CABLE', 'GPON', 'WIRELESS'];
// === Event Functions ===
$(window).on('load', function () {
    //AJAX_GET_ListTestPlanPreview();
    for (var i = 0; i < gbl_arrTestPlanProjectType.length; i++) {
        AJAX_GET_ListTestPlanPreviewByProjectType(gbl_arrTestPlanProjectType[i]);
    }    
});
$('#fileUpload').on('change', function () {
    gbl_bFlagError = false;
    $('#noticeFileError').html('');
    //
    let uploadedFile = $('#fileUpload').prop('files')[0];   
    let fileName = uploadedFile.name;
    let fileSize = (uploadedFile.size / (1024 * 1024)).toFixed(2);
    //    
    if (fileSize >= 5) {
        gbl_bFlagError = true;
        $('#noticeFileError').html('File size cannot exceed 5MB!');
    }
    //
    $('#preFileName').text(fileName);
    $('#FilePreviewSection').attr('src', URL.createObjectURL(uploadedFile));
});
$('#btnSubmitTestPlan').on('click', function (e) {
    if (gbl_bFlagError) {
        e.preventDefault();
        alert('Please fullfill required fields!');
    }    
});
// === AJAX Requesting Functions ===
async function AJAX_GET_PartialTestPlanTable(_model) {
    try {
        $.ajax({
            url: URL_GET_PartialTestPlanTable,
            data: { model: _model },
            async: false,
            success: function (testPlanTable) {
                //console.log(testPlanTable);
                $('#modalLblTestPlan').html(_model+' - TEST PLAN LIST');
                $('#divTableTestPlan').html(testPlanTable);
                createTestPlanDataTable();
            },
            error: function (error) {
                console.log('Error on calling functions: ' + error);
            },
            complete: function () {
                /*ChangePaginationColor();                
                $('.paginate_button').on('click', function () {
                    ChangePaginationColor();
                    console.log('clicked');
                });    */                                  
            }
        });
    } catch (ex) {
        console.log('Catched exception: ' + { ex: ex });
    }
}
async function AJAX_GET_ListModelOfTestPlan() {
    try {
        $.ajax({
            url: URL_GET_ListModelOfTestPlan,
            data: { },
            async: false,
            success: function (listModelTestPlan) {
                console.log(listModelTestPlan);
                
            },
            error: function (error) {
                console.log('Error on calling functions: ' + error);
            }
        });
    } catch (ex) {
        console.log('Catched exception: ' + { ex: ex });
    }
}
async function AJAX_GET_ListTestPlanPreview() {
    try {
        $.ajax({
            url: URL_GET_ListTestPlanPreview,
            data: {},
            async: false,
            success: function (listTestPlanPreview) {
                //console.log(listTestPlanPreview);
                var content_item = '';
                //var normCount = 0, abnormCount = 0, naCount = 0;
                $.each(listTestPlanPreview, function (idx, testPlanVersion) {
                    var content_status = 'info';                    

                    content_item +=
                        '<div id="" class="capsule-container d-flex mb-1 " status-all="-1" data-toggle="modal" data-target="#testPlanModal" ' +
                        ' onclick="AJAX_GET_PartialTestPlanTable(\'' + testPlanVersion.ModelName + '\')"' +
                        ' model="' + testPlanVersion.ModelName + '"' +
                        ' verlatest="' + testPlanVersion.VersionLatest + '"' +
                        ' veronline="">' +                        
                        '<div class=" m-auto capsule-case w-100 text-white ">' + testPlanVersion.ModelName + '</div>' +
                        '<div class=" m-auto capsule-case bg-success ' + content_status + ' text-center w-95">' + NullVal(testPlanVersion.VersionLatest) + '</div>' +

                        '</div>';
                });
                //normCount = listLatestVersions.length - abnormCount - naCount;
                $('#divShowTestPlan').html(content_item);

            },
            error: function (error) {
                console.log('Error on calling functions: ' + error);
            },
            complete: function () {
                //
                $('#txtKeyWord').on('keyup', function () {
                    var val = this.value.toUpperCase();
                    var content_item = document.getElementsByClassName('capsule-container');
                    //console.log(val);
                    //console.log(content_item);
                    $(content_item).removeClass('d-flex');
                    for (var i = 0; i < content_item.length; i++) {
                        if (content_item[i].textContent.indexOf(val) > -1) {

                            //content_item[i].style.display = 'block';
                            $(content_item[i]).addClass('d-flex');
                        } else {

                            //content_item[i].style.display = 'none';
                            $(content_item[i]).addClass('d-none');
                        }
                    }
                });
            }
        });
    } catch (ex) {
        console.log('Catched exception: ' + { ex: ex });
    }
}
async function AJAX_GET_ListTestPlanPreviewByProjectType(_projectType) {
    //
    let containerTestPlanID = 'containerTestPlan' + _projectType;
    //
    let jqContainerTestPlanHTML = $('#' + containerTestPlanID);
    //
    try {
        $.ajax({
            url: URL_GET_ListTestPlanPreviewByProjectType,
            data: { projectType: _projectType },
            async: false,
            success: function (listTestPlanPreview) {
                //console.log(listTestPlanPreview);
                var content_item = '';                
                $.each(listTestPlanPreview, function (idx, testPlanVersion) {
                    var content_status = 'info';

                    content_item +=
                        '<div id="" class="fii-capsule-container mb-1 " status-all="-1" data-toggle="modal" data-target="#testPlanModal" ' +
                        ' onclick="AJAX_GET_PartialTestPlanTable(\'' + testPlanVersion.ModelName + '\')"' +
                        ' model="' + testPlanVersion.ModelName + '"' +
                        ' verlatest="' + testPlanVersion.VersionLatest + '"' +
                        ' veronline="">' +
                        '<div class="fii-capsule-case ">' + testPlanVersion.ModelName + '</div>' +
                        '<div class="fii-capsule-case text-center bg-success ' + content_status + ' ">' + NullVal(testPlanVersion.VersionLatest) + '</div>' +
                        '</div>';
                });     
                //
                jqContainerTestPlanHTML.addClass('fii-capsule-wrapper');
                jqContainerTestPlanHTML.html(content_item);

            },
            error: function (error) {
                console.log('Error on calling functions: ' + error);
            },
            complete: function () {
                //
                $('#txtKeyWord').on('keyup', function () {
                    var val = this.value.toUpperCase();
                    var content_item = document.getElementsByClassName('fii-capsule-container');
                    //
                    $(content_item).removeClass('d-flex');
                    for (var i = 0; i < content_item.length; i++) {
                        if (content_item[i].textContent.indexOf(val) > -1) {
                            $(content_item[i]).addClass('d-flex');
                        }
                        else {
                            $(content_item[i]).addClass('d-none');
                        }
                    }
                });
            }
        });
    } catch (ex) {
        console.log('Catched exception: ' + { ex: ex });
    }
}
// === Support Functions ===
function DisplayPreFileName(_filePath) {
    var _subStart = _filePath.lastIndexOf('/') + 1;
    var _orgFileName = _filePath.substring(_subStart);
    $('#preFileName').text(_orgFileName);
}
function GetTestPlanFileData(_filePath) {
    $('#FilePreviewSection').attr('src', _filePath);
}
function ChangePaginationColor() {
    var css_page_link = {
        "position": "relative",
        "display": "block",
        "padding": "0.5rem 0.75rem",
        "margin-left": "-1px",
        "line-height": "1.25",
        "color": "red",
        "background-color": "#fff",
        "border": "1px solid #dddfeb"
    };
    var css_active_page_link = {
        "color": "#fff",
        "background-color": "rgba(250,160,160,1)",
        "border-color": "rgba(250,160,160,1)"
    };
    var css_disable_page_link = {
        "cursor": "auto",
        "pointer-events": "none",
        "color": "#858796",
        "background-color": "#fff",
        "border-color": "#dddfeb"
    };
    //console.log($('.paginate_button.page-item.active').children());
    $('.paginate_button.page-item').children().css(css_page_link);
    $('.paginate_button.page-item.active').children().css(css_active_page_link);
    $('.paginate_button.page-item.disabled').children().css(css_disable_page_link);
}
// Upload input on change to preview file
function NullVal(val) {
    if (val == null || val == undefined || val == '' || val == 'null' || val == 'NULL') {
        return 'N/A';
    }
    return val;
};
// Arrow function
/*fileUpload.onchange = evt => {
    const [file] = fileUpload.files;
    if (file) {
        FilePreviewSection.src = URL.createObjectURL(file);
    }
}*/
