// ========= AJAX URLs =========
var URL_GETDataTableFATP = $('#URL_GETDataTableFATP').val();
var URL_GETFATPDetail = $('#URL_GETFATPDetail').val();
var URL_GET_AllFATPLine = $('#URL_GET_AllFATPLine').val();
var URL_GET_AllTabFATPLine = $('#URL_GET_AllTabFATPLine').val();
var URL_GET_AllTabFATPLineStation = $('#URL_GET_AllTabFATPLineStation').val();
var URL_GET_AllFATPLineModel = $('#URL_GET_AllFATPLineModel').val();
var URL_GET_AllFATPLineStation = $('#URL_GET_AllFATPLineStation').val();
var URL_GET_ChangeFATPLockStatus = $('#URL_GET_ChangeFATPLockStatus').val();
var URL_SET_ResetFAILNUMbyFAILBUFFER = $('#URL_SET_ResetFAILNUMbyFAILBUFFER').val();
// ========= Global Variables =========
var intervalTime = 120 * 1000;
var _divTableID = 'DivTableFATP';
//var intervalGetDataTableFATP =
// ========= Events Handling Functions =========
$(window).ready(function () {
    //AJAX_GETAllFATPLine(URL_GET_AllFATPLine);
    //AJAX_GETAllTabFATPLine(URL_GET_AllTabFATPLine);
    AJAX_GETAllTabFATPLineStation(URL_GET_AllTabFATPLineStation);
    var tabLine = $('.linerun.tabLine');
    if (tabLine.length == 0) {
        tabLine = $('.tabLine');
    }
    //console.log(tabLine);
    tabLine[0].click();
});
setInterval(GetDataTableFATP, intervalTime);
$('#cboFATPLine').on('change', function () {
    var _line = $('#cboFATPLine').val();
    //console.log(_line);
    AJAX_GETAllFATPLineStation(URL_GET_AllFATPLineStation, _line);
    //AJAX_GETAllFATPLineModel(URL_GET_AllFATPLineModel, _line);

    //clearInterval(intervalGetDataTableFATP);
});

$('#cboFATPLineStation').on('change', function () {
    //GetDataTableFATP();        
    AutoClickBtnGetFATPData();
});
function tabLineSelected(evt, _line, _strStationOnline) {
    var divTabLineID = 'divTab' + _line;
    $('.tabLine').removeClass('active');    
    $('#' + divTabLineID).css('display', 'block');
    evt.currentTarget.className += ' active';
    //
    var _divTableID = 'DivTableFATP';
    $('#' + _divTableID).html('');
    AJAX_GETAllFATPLineStation(URL_GET_AllFATPLineStation, _line, _strStationOnline)
    
}
function tabStationSelected(evt, _line, _station) {
    var divTabStationID = 'divTab' + _station;
    $('.tabStation').removeClass('active lineActive');    
    $('#' + divTabStationID).css('display', 'block');
    evt.currentTarget.className += ' active lineActive';
    //
    var _divTableID = 'DivTableFATP';
    AJAX_GETFATPDataTable(URL_GETDataTableFATP, _divTableID, _line, _station)
}
function AutoClickBtnGetFATPData() {
    var btnGetFATPData = document.getElementById('btnGetFATPData');
    btnGetFATPData.click();
    
    //setTimeout(AutoClickBtnGetFATPData, intervalTime);
}
function ChangeFATPLockStatus(_userName, _line, _station, _model, _atePC, _lockStatus) {
    AJAX_GETChangeFATPLockStatus(_userName, _line, _station, _model, _atePC, _lockStatus);
}
function GetDataTableFATP() {
    /*var _line = $('#cboFATPLine').val();
    var _station = $('#cboFATPLineStation').val();*/

    var _line = $('.lineActive').data('line');
    var _station = $('.lineActive').data('station');
    

    AJAX_GETFATPDataTable(URL_GETDataTableFATP, _divTableID, _line, _station);
};
function GetFATPDetail(_FATP_ID, _FATPDetailHeader) {
    $('#spanFATPStation').html(_FATPDetailHeader);    
    AJAX_GETFATPDetail(_FATP_ID);
};
function SetResetFAILNUMbyFAILBUFFER(_atePC, _ateIP, _model, _line, _station) {
    AJAX_SET_ResetFAILNUMbyFAILBUFFER(_atePC, _ateIP, _model, _line, _station);
};

// ========= AJAX Functions =========
async function AJAX_GETAllFATPLine(URL_GET_AllFATPLine) {
    try {
        await $.ajax({
            url: URL_GET_AllFATPLine,  
            async:false,
            success: function (listFATPLine) {
                $('#cboFATPLine option:gt(0)').remove();
                $.each(listFATPLine, function (idx = 0) {

                    $('#cboFATPLine').append($('<option />').val(listFATPLine[idx]).text(listFATPLine[idx]));
                    idx++;
                });
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETAllTabFATPLine(URL_GET_AllTabFATPLine) {
    var tabLineHTML = '<ul class="nav nav-tabs nav-tabs-fatp d-flex justify-content-center mb-1" role="tablist">';
    try {
        await $.ajax({
            url: URL_GET_AllTabFATPLine,
            async: false,
            success: function (tabFATPLine) {
                $('#tabFATPLine').html('');
                $.each(tabFATPLine, function (idx = 0, tabLine) {

                    /*$('#cboFATPLine').append($('<option />').val(listFATPLine[idx]).text(listFATPLine[idx]));
                    idx++;*/
                    var tabID = 'tab' + line, tabPane = 'tpane' + line;
                    var tab_bg = '';

                    if (tabLine.StationNum > 0) {
                        tab_bg = 'success';
                    }
                    //tabLineHTML = '<div class="m-auto bg-' + tab_bg + '" ><button class="tabLinks tabLine" onclick="tabLineSelected(event,\'' + tabLine.Line +'\')">' + tabLine.Line + '</button></div>';
                    tabLineHTML +=  '<li class="nav-item m-auto">' +
                                    '<button type="button" id="' + tabID + '" class="nav-link "' +
                                    'data-bs-toggle="tab" data-bs-target="#' + tabPane + '"' +
                                    'role="tab" aria-controls="' + tabPane + '" aria-selected="true"' +
                        'onclick="tabLineSelected(event,\'' + tabLine.Line + '\')">' + tabLine.Line +
                                    '</button>'+
                                    '</li>';
                    //$('#tabFATPLine').append(tabLineHTML);

                });
                tabLineHTML += '</ul>';
                $('#tabFATPLine').html(tabLineHTML);
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETAllFATPLineStation(URL_GET_AllFATPLineStation, _line, _strStationOnline) {
    try {
        await $.ajax({
            url: URL_GET_AllFATPLineStation,
            data: { line: _line},
            success: function (listFATPLineStation) {
                //
                $('#cboFATPLineStation option:gt(0)').remove();
                // Get list station of line
                var _arrStationOnline = _strStationOnline.split(',');
                //console.log(_arrStationOnline);
                //
                var tabLineStationHTML = '<ul class="nav nav-tabs nav-tabs-fatp d-flex justify-content-center mb-1 " role="tablist">';
                $('#tabFATPLineStation').html(tabLineStationHTML);
                                
                $.each(listFATPLineStation, function (idx = 0, _station) {
                    //console.log(_strStationOnline + '|' + _station + '|' + (parseInt(_strStationOnline.indexOf(_station)) != -1));
                    var tabID = 'tab' + _station, tabPane = 'tpane' + _station;
                    var line_bg = 'lineoff';
                    if (parseInt(_arrStationOnline.indexOf(_station)) != -1) {
                        
                        line_bg = 'linerun';
                    }

                    //tabLineStationHTML = '<div class="ml-5 mr-5 bg-' + line_bg +'"><button class="tabLinks tabStation" onclick="tabStationSelected(event,\'' + _line + '\',\'' + _station + '\')" data-line="' + _line + '" data-station="' + _station + '">' + _station + '</button></div>';

                    tabLineStationHTML += '<li class="nav-item ' + line_bg + ' ml-5 mr-5">' +
                        '<button type="button" id="' + tabID + '" class="nav-link tabStation ' + line_bg + '"' +
                        'data-bs-toggle="tab" data-bs-target="#' + tabPane + '"' +
                        'role="tab" aria-controls="' + tabPane + '" aria-selected="true"' +
                        'data-line="' + _line + '" data-station="' + _station + '"'+
                        'onclick="tabStationSelected(event,\'' + _line + '\',\'' + _station + '\')">' + _station +
                        '</button>' +
                        '</li>';;

                    //$('#cboFATPLineStation').append($('<option />').val(_station).text(_station));
                    //$('#tabFATPLineStation').append(tabLineStationHTML);
                    
                    //idx++;
                });
                tabLineStationHTML += '</ul>';
                $('#tabFATPLineStation').html(tabLineStationHTML);
                // 
                if (listFATPLineStation.length > 0) {
                    var tabStation = $('.tabStation.linerun');
                    if (tabStation.length == 0) {
                        tabStation = $('.tabStation');
                    }                    
                    tabStation[0].click();
                };
                
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETAllTabFATPLineStation(URL_GET_AllTabFATPLineStation) {
    var tabLineHTML = '<ul class="nav nav-tabs nav-tabs-fatp d-flex justify-content-center mb-1 " role="tablist">';
    try {
        await $.ajax({
            url: URL_GET_AllTabFATPLineStation,
            async: false,
            success: function (tabFATPLineStation) {
                $('#divTestFATPLine').html('');
                //Iterating each Line in list
                //console.log(tabFATPLineStation);
                $.each(tabFATPLineStation, function (idx = 0, _line) {
                    var tabID = 'tab' + _line.Line, tabPane = 'tpane' + _line.Line;
                    // Create TabLine for each Line                    
                    var tab_bg = 'lineoff';
                    if (_line.IsLineActive == 0) {
                        tab_bg = 'linerun';
                    }

                    // Create TabStation for each Station in Line
                    var listStation = _line.ListStation;
                    var _strStationOnline = '';
                    $.each(listStation, function (idx, _station) {
                        if (_station.ActivePCNum) {
                            _strStationOnline += _station.Station + ',';
                        }
                    });

                    //console.log(_strStationOnline);
                    //tabLineHTML = '<div class="m-auto bg-' + tab_bg + '" ><button class="tabLinks tabLine" onclick="tabLineSelected(event,\'' + _line.Line + '\',\'' + _strStationOnline + '\')">' + _line.Line + '</button></div>';
                    tabLineHTML += '<li class="nav-item ' + tab_bg +' m-auto ">' +
                        '<button type="button" id="' + tabID + '" class="nav-link tabLine ' + tab_bg +'"' +
                        'data-bs-toggle="tab" data-bs-target="#' + tabPane + '"' +
                        'role="tab" aria-controls="' + tabPane + '" aria-selected="true"' +
                        'onclick="tabLineSelected(event,\'' + _line.Line + '\',\'' + _strStationOnline + '\')">' + _line.Line +
                        '</button>' +
                        '</li>';
                    //$('#tabFATPLine').append(tabLineHTML);
                });
                tabLineHTML += '</ul>';
                $('#tabFATPLine').html(tabLineHTML);
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETAllFATPLineModel(URL_GET_AllFATPLineModel, _line) {
    try {
        await $.ajax({
            url: URL_GET_AllFATPLineModel,
            data: { line: _line },
            success: function (listFATPLineModel) {
                $('#cboFATPLineModel option:gt(0)').remove();
                $.each(listFATPLineModel, function (idx = 0) {

                    $('#cboFATPLineModel').append($('<option />').val(listFATPLineModel[idx]).text(listFATPLineModel[idx]));
                    idx++;
                });
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETFATPDataTable(URL_GETDataTableFATP, _divTableID, _line, _station) {
    try {
        await $.ajax({
            url: URL_GETDataTableFATP,
            data: { line: _line, station: _station },
            success: function (DataTableFATP) {                
                $('#' + _divTableID).html(DataTableFATP);
                createFATPDataTable();
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETFATPDetail(_FATP_ID) {
    try {
        var divLoaderHTML = '<div align="center">' +
            '<img src="/Images/loaderGifs/Spinner-3.gif" />' +
            '<h4 class="text-white">Loading...</h4>' +
            '</div>';
        $('#divFATPDetailModal').html(divLoaderHTML); 
        await $.ajax({
            url: URL_GETFATPDetail,
            data: { FATP_ID: _FATP_ID},
            success: function (FATPDetail) {
                $('#divFATPDetailModal').html(FATPDetail);                
            },
            error: function (response) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GETChangeFATPLockStatus(_userName, _line, _station, _model, _atePC, _lockStatus) {
    try {
        await $.ajax({
            url: URL_GET_ChangeFATPLockStatus,
            data: { userName: _userName, model: _model, atePC: _atePC, lockStatus: _lockStatus },
            success: function (ActionResult) {
                if (ActionResult) {

                    AJAX_GETFATPDataTable(URL_GETDataTableFATP, _divTableID, _line, _station);
                    return;
                }                
                Swal.fire({
                    title: "Error on excecuting SQL Command to change status!",
                    type: "error",
                });
            },
            error: function (ActionResult) {
                alert('Error on calling function');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_SET_ResetFAILNUMbyFAILBUFFER(_atePC, _ateIP, _model, _line, _station) {
    try {
        await $.ajax({
            url: URL_SET_ResetFAILNUMbyFAILBUFFER,
            data: { atePC: _atePC, ateIP: _ateIP, model: _model },
            success: function (isResetOk) {
                if (isResetOk) {
                    /*console.log('FAIL reset result: ' + isResetOk);*/
                    AJAX_GETFATPDataTable(URL_GETDataTableFATP, _divTableID, _line, _station);
                    return;
                }
                Swal.fire({
                    title: "Error on excecuting SQL Command to reset FAIL number!",
                    type: "error",
                });                
            },
            error: function (ErrorMessage) {
                alert('Error on calling function: ' + ErrorMessage);
            }
        });
    } catch (e) {
        console.log('AJAX Exception: ' + { e: e });
    }
};
// ========= Support Functions =========
function ButtonTest() {
    AJAX_GETAllTabFATPLineStation(URL_GET_AllTabFATPLineStation);
}
