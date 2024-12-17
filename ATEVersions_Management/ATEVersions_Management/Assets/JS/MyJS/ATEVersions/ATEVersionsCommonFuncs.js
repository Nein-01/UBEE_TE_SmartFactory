// ====== AJAX URL ======
var URL_GETTableVersions = $('#URL_GETTableVersions').val();
var URL_GET_ListLatestVersionOnline = $('#URL_GET_ListLatestVersionOnline').val();
var URL_GET_ListLatestVersionOnlineByProjectType = $('#URL_GET_ListLatestVersionOnlineByProjectType').val();
var URL_GET_VersionATEListByID = $('#URL_GET_VersionATEListByID').val();
// ====== Global Variables ======
// === Common Variables ===
var Var_SiteArea = $('#Var_SiteArea').val();
var gbl_arrProgramProjectType = ['CABLE', 'GPON', 'WIRELESS'];
var gbl_dAllCount = 0,
    gbl_dNormCount = 0,
    gbl_dAbnormCount = 0,
    gbl_dNaCount = 0;
// === Jquery Object Variables ===
var jqVersionNormalHTML = $('.version-normal');
var jqVersionAbnormalHTML = $('.version-abnormal');
var jqVersionNAHTML = $('.version-na');

// ====== Event Triggered Functions ======
$(window).ready(function (e) {
    var Var_ModelKeeper = $('#Var_ModelKeeper').val();
    LoadVersionTableByModel(Var_ModelKeeper);
    if (Var_SiteArea === 'Client') {
        //AJAX_GET_ListLatestVersionOnline();
        for (var i = 0; i < gbl_arrProgramProjectType.length; i++) {
            AJAX_GET_ListLatestVersionOnlineByProjectType(gbl_arrProgramProjectType[i]);
            gbl_dNormCount = gbl_dAllCount - gbl_dAbnormCount;
            jqVersionNormalHTML.html(gbl_dNormCount);
            jqVersionAbnormalHTML.html(gbl_dAbnormCount);
            jqVersionNAHTML.html(gbl_dNaCount);
        }        
    }    
})
$('#selectModels').on('change', function (e) {
    LoadVersionTableByModel();
});

async function LoadVersionTableByModel(Var_ModelKeeper) {
    var _model = $('#selectModels option:selected').text();
    var _tblVersionID = 'divTableVersions';
    var _prgID = ''; 

    var divLoaderHTML = '<div align="center">' +
        '<img src="/Images/loaderGifs/Book.gif" />' +
        '<h4 class="text-black">Loading...</h4>' +
        '</div>';

    $('#' + _tblVersionID).html(divLoaderHTML);       

    if (Var_ModelKeeper != undefined) {
        if (Var_ModelKeeper.trim().length != 0 && Var_ModelKeeper != null) {
            _model = Var_ModelKeeper;
            $('#selectModels').val(Var_ModelKeeper);
        }
    }
    $('#modalLblVersion').html(_model+' - VERSION LIST');
    try
    {
        await $.ajax({
            url: URL_GETTableVersions,
            data: { prgId: _prgID, searchStr: '', model: _model },
            success: function (versionTable) {
                $('#'+_tblVersionID).html(versionTable);
                createVesionDataTable();
            },
            error: function () {
                console.log('Error on calling function');
            },
            complete: function () {
                //
                $('.version-atelist').on('click', function (e) {
                    let ateVerID = $(this).attr('ateVerID');
                    let ateListTitle = $(this).attr('ateListTitle');
                    $('#modalLblVersionAteList').html(ateListTitle);
                    AJAX_GET_VersionATEListByID(ateVerID);

                });
            }
        });
    }
    catch (e)
    {
        console.log("Exception at: " + { e: e });
    }
    
};

// ====== AJAX Requesting Functions ======
async function AJAX_GET_ListLatestVersionOnline() {
    var divLoaderHTML = '<div align="center">' +
        '<img src="/Images/loaderGifs/Loading-bar.gif" />' +
        '<h4 class="text-black">Loading...</h4>' +
        '</div>';

    $('#divLoader').html(divLoaderHTML);
    try
    {
        await $.ajax({
            url: URL_GET_ListLatestVersionOnline,
            data: {},
            async: false,
            success: function (listLatestVersions) {
                $('#divLoader').html('');
                //console.log(listLatestVersions);
                var content_item = '';
                var normCount = 0, abnormCount = 0, naCount = 0;
                $.each(listLatestVersions, function (idx, version) {
                    var content_status = 'success'
                    //console.log(version.VersionLatest + ' compare: ' + (NullVal(version.VersionLatest) === 'N/A'));
                    
                    if (!version.IsVersionsMatched || (version.UncheckCount > 0) || (version.NoATEListCount > 0)) {
                        content_status = 'danger';
                        abnormCount++;
                    }
                    //console.log(NullVal(version.VersionLatest));
                    if (NullVal(version.VersionLatest) === 'N/A' || NullVal(version.VersionLatest) === null) {
                        naCount++;
                        return true; // return true == continue
                        content_status = 'secondary';
                        
                    }
                    

                    content_item +=
                        '<div id="" class="capsule-container d-flex mb-1" status-all="-1" data-toggle="modal" data-target="#versionModal" ' +
                    ' onclick="LoadVersionTableByModel(\'' + version.ModelName+'\')"' +                                                
                        ' model="' + version.ModelName+'"' +                                                
                        ' verlatest="' + version.VersionLatest+'"' +                                                
                    ' veronline="' + version.VersionOnline + '">' +
                    '<div class="tooltip-text-media">' +
                    '<table id="tblHover" class="">' +
                    '<thead></thead>' +
                    '<tbody>' +
                    '<tr><td class="title-hover">Model: </td><td>' + NullVal(version.ModelName) + '</td></tr>' +
                    '<tr><td class="title-hover">Version Latest: </td><td>' + NullVal(version.VersionLatest) + '</td></tr>' +
                    '<tr><td class="title-hover">Version Online: </td><td class="fw-bold text-' + content_status +'">' + NullVal(version.VersionOnline) + '</td></tr>' +
                    '<tr><td class="title-hover">No ATEList: </td><td class="fw-bold text-' + content_status + '">' + version.NoATEListCount + '</td></tr>' +
                    '<tr><td class="title-hover">Unchecked: </td><td class="fw-bold text-' + content_status + '">' + version.UncheckCount + '</td></tr>' +                    
                    '</tbody>' +
                    '</table>' +
                    '</div>' +
                            '<div class=" m-auto capsule-case w-100 ">' + version.ModelName + '</div>' +
                            '<div class=" m-auto capsule-case bg-' + content_status + ' text-center w-95">' + NullVal(version.VersionLatest)  + '</div>' +                                                                                                                                      
                    
                        '</div>';
                })
                normCount = listLatestVersions.length - abnormCount - naCount;
                $('#divShowVersion').html(content_item);
                $('.version-normal').html(normCount);
                $('.version-abnormal').html(abnormCount);
                $('.version-na').html(naCount);
            },
            error: function (error) {
                console.log('Error on calling function: \n' + error);
            },
            complete: function () {
                // Create hover event for content-item
                // on hovering
                $('.capsule-container').on('mouseover', function () {
                    var model = $(this).attr('model');                    
                    //console.log(model);
                    $('.capsule-container[model="' + model + '"] .tooltip-text-media').css('visibility', 'visible');

                });
                // on out
                $('.capsule-container').on('mouseout', function () {
                    var model = $(this).attr('model');
                    //console.log(model);
                    $('.capsule-container[model="' + model + '"] .tooltip-text-media').css('visibility', 'hidden');

                });
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
    }
    catch (e)
    {
        console.log('Exception : ' + {e:e});
    }
};
function AJAX_GET_ListLatestVersionOnlineByProjectType(_projectType) {
    let containerProgramID = 'containerProgram' + _projectType;
    let divLoaderHTML = '';
    //
    let jqContainerProgramHTML = $('#' + containerProgramID);
   
    //
    
    //
    try {
        $.ajax({
            url: URL_GET_ListLatestVersionOnlineByProjectType,
            data: { projectType: _projectType },
            async: false,
            success: function (listLatestVersions) {                
                //console.log(listLatestVersions);
                let content_item = '';
                //
                $.each(listLatestVersions, function (idx, version) {
                    let content_status = 'success'
                    gbl_dAllCount++;
                    //console.log(version.VersionLatest + ' compare: ' + (NullVal(version.VersionLatest) === 'N/A'));
                    if (!version.IsVersionsMatched || (version.UncheckCount > 0) || (version.NoATEListCount > 0)) {
                        content_status = 'danger';
                        gbl_dAbnormCount++;
                    }
                    //console.log(NullVal(version.VersionLatest));
                    if (NullVal(version.VersionLatest) === 'N/A' || NullVal(version.VersionLatest) === null) {
                        gbl_dNaCount++;
                        gbl_dAllCount--;
                        return true; // return true == continue
                        content_status = 'secondary';

                    }
                    content_item +=
                        '<div id="" class="fii-capsule-container fii-tooltip-container text-white" status-all="-1" data-toggle="modal" data-target="#versionModal" ' +
                        ' onclick="LoadVersionTableByModel(\'' + version.ModelName + '\')"' +
                        ' model="' + version.ModelName + '"' +
                        ' verlatest="' + version.VersionLatest + '"' +
                        ' veronline="' + version.VersionOnline + '">' +
                        '<div class="fii-capsule-case ">' + version.ModelName + '</div>' +
                        '<div class="fii-tooltip fii-tooltip-item bg-black ">' +
                        '<table id="" class="text-center">' +
                        '<thead></thead>' +
                        '<tbody>' +
                        '<tr><td class="w-50">Model: </td><td>' + NullVal(version.ModelName) + '</td></tr>' +
                        '<tr><td class="w-50">Version Latest: </td><td>' + NullVal(version.VersionLatest) + '</td></tr>' +
                        '<tr><td class="w-50">Version Online: </td><td class="fw-bold text-' + content_status + '">' + NullVal(version.VersionOnline) + '</td></tr>' +
                        '<tr><td class="w-50">No ATEList: </td><td class="fw-bold text-' + content_status + '">' + version.NoATEListCount + '</td></tr>' +
                        '<tr><td class="w-50">Unchecked: </td><td class="fw-bold text-' + content_status + '">' + version.UncheckCount + '</td></tr>' +
                        '</tbody>' +
                        '</table>' +
                        '</div>' +
                        
                        '<div class="fii-capsule-case bg-' + content_status + ' ">' + NullVal(version.VersionLatest) + '</div>' +

                        '</div>';
                })                
                //
                jqContainerProgramHTML.addClass('fii-capsule-wrapper');
                jqContainerProgramHTML.html(content_item);
                
            },
            error: function (error) {
                console.log('Error on calling function: \n' + error);
            },
            complete: function () {
                // Create hover event for content-item
                // on hovering
                $('.capsule-container').on('mouseover', function () {
                    var model = $(this).attr('model');
                    //console.log(model);
                    $('.capsule-container[model="' + model + '"] .tooltip-text-media').css('visibility', 'visible');

                });
                // on out
                $('.capsule-container').on('mouseout', function () {
                    var model = $(this).attr('model');
                    //console.log(model);
                    $('.capsule-container[model="' + model + '"] .tooltip-text-media').css('visibility', 'hidden');

                });
                //
                $('#txtKeyWord').on('keyup', function () {
                    var val = this.value.toUpperCase();
                    var content_item = document.getElementsByClassName('fii-capsule-container');
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
    }
    catch (e) {
        console.log('Exception : ' + { e: e });
    }
};
function AJAX_GET_VersionATEListByID(_ateVerID) {
    try {
        $.ajax({
            url: URL_GET_VersionATEListByID,
            data: { ateVerID: _ateVerID },            
            success: function (versionATEListHTML) {
                $('#divContentVersionAteList').html(versionATEListHTML);
            },
            error: function (error) {
                console.log('Error on calling function: \n' + error);
            },
            complete: function () {
                
            }
        });
    }
    catch (e) {
        console.log('Exception : ' + { e: e });
    }
};
// ====== Support Functions ======
function NullVal(val) {
    if (val == null || val == undefined || val == '' || val == 'null' || val == 'NULL') {
        return 'N/A';
    }
    return val;
};