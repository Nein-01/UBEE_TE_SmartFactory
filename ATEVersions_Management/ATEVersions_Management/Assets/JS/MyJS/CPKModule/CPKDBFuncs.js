// ====== AJAX URLs ======
var URL_GET_CPKOnOffModelList = $('#URL_GET_CPKOnOffModelList').val();
var URL_GET_ListCPKModelStation = $('#URL_GET_ListCPKModelStation').val();
var URL_GET_CPKModelList = $('#URL_GET_CPKModelList').val();
var URL_GET_CPKStationList = $('#URL_GET_CPKStationList').val();
var URL_GET_PartialTableCPKModelStationItem = $('#URL_GET_PartialTableCPKModelStationItem').val();
var URL_GET_CPKChartSectionOfSelectedItem = $('#chartSectionUrl').val();
var URL_GET_ListMoByModel = $('#URL_GET_ListMoByModel').val();
var URL_GET_PCList = $('#URL_GET_PCList').val();
// ====== Global Variables ======

// ====== Event Triggering Functions ======
$(window).on('load', function () {
    SP_InitialTimeSpan();
    let timeSpanVal = SP_GetTimeSpanVal();
    AJAX_GET_CPKOnOffModelList(timeSpanVal.fromDate, timeSpanVal.toDate);
    FII_InitCustomSelect();
    //AJAX_GET_CPKModelList();    
    if ($('#fromDay').length > 0) {
        SP_InitialCPKInputField();
    }
    
});

$('#timeSpan').on('change', function () {
    let _model = $('#VAR_Model').val();
    let _station = $('#VAR_Station').val();
    let _timeSpanVal = SP_GetTimeSpanVal();
    //console.log(_model + ' | ' + _station);
    AJAX_GET_MOList(URL_GET_ListMoByModel, _model, _timeSpanVal.fromDate, _timeSpanVal.toDate);
    AJAX_GET_PCList(URL_GET_PCList, _model, _station, _timeSpanVal.fromDate, _timeSpanVal.toDate);
    //FII_InitCustomSelect();
});
$('#btnCPKModelOnline').on('click', function ()
{
    SP_ControlDivModelOnline();
    let timeSpanVal = SP_GetTimeSpanVal();
    AJAX_GET_CPKOnOffModelList(timeSpanVal.fromDate, timeSpanVal.toDate);
});
$('#btnCPKModelOffline').on('click', function () {
    SP_ControlDivModelOffline();
});
$('#cboCPKMO').on('change', function () {
    let _keyMO = $('#cboCPKMO').val();
    console.log(_keyMO);

});
$('#cboCPKModel').on('change', function () {
    let _model = $('#cboCPKModel').val();
    $('.tab-cpkmodel-offline').html('<div id="" class="div-tab-station ' + _model.replace('.', '') + ' w-"></div>');
    AJAX_GET_CPKStationList(_model, true);
});
$('#cboCPKStation').on('change', function () {
    var _model = $('#cboCPKModel').val();
    var _station = $('#cboCPKStation').val();
    AJAX_GET_PartialTableCPKModelStationItem(_model, _station);
});
$('#fromDay, #toDay, #pcsNumber').on('change', function () {
    //Control date time input
    var _fromDay = $('#fromDay').val();
    var _toDay = $('#toDay').val();
    var _fromDayVal = new Date(_fromDay);
    var _toDayVal = new Date(_toDay);
    var _dayBoundary = new Date(fixedToDay);

    _fromDay = $('#fromDay').val();
    _toDay = $('#toDay').val();
    //Call AJAX Functions
    AJAX_GET_MOList(URL_GET_ListMoByModel, _fromDay, _toDay);
    AJAX_GET_PCList(URL_GET_PCList, _fromDay, _toDay);
    //Control PCS Number input
    var _pcsNumber = parseInt($('#pcsNumber').val());
    if (_pcsNumber < 0) {
        $('#pcsNumber').val(_pcsNumber * -1);
    }
    if (isNaN(_pcsNumber)) {
        $('#pcsNumber').val(100000);
    }
});

var reDrawCPK = function (e) {
    var _modelname = $('#VAR_Model').val();
    var _stationname = $('#VAR_Station').val();
    var _pos = parseInt($('#VAR_Pos').val());
    var _pcsNumber = parseInt($('#pcsNumber').val());
    var _moNumber = $('#cboMo').val();
    var _atePC = $('#cboPC').val();
    var _fromDay = $('#fromDay').val();
    var _toDay = $('#toDay').val();
    //console.log(_modelname + '|' + _stationname + '|' + _pos + '|' + _pcsNumber + '|' + _fromDay + '|' + _toDay);
    AJAX_GET_RedrawCPKChart(_modelname, _stationname, _pos, _pcsNumber, _moNumber, _atePC, _fromDay, _toDay);
}
function EVT_DrawCPKChartOfSelectedItem(element,_modelname, _stationname, _pos,) {

    //
    let grandParentEle = element.parentNode.parentNode;
    $('tr').removeClass('bg-info');
    $(grandParentEle).addClass('bg-info');
    //console.log();
    //
    let _keyMO = $('#cboCPKMO').val();
    let _keyPC = $('#cboCPKPC').val();
    let _timeSpanVal = SP_GetTimeSpanVal();
        
    AJAX_GET_RedrawCPKChart(_modelname, _stationname, _pos, 0, _keyMO, _keyPC, _timeSpanVal.fromDate, _timeSpanVal.toDate);
}
// ====== AJAX Requesting Functions ======
async function AJAX_GET_CPKOnOffModelList(_fromDate, _toDate)
{
   
    try {
        //
        let countOnline = 0;
        //
        await $.ajax({
            url: URL_GET_CPKOnOffModelList,
            data: { fromDate: _fromDate, toDate: _toDate },
            async: false,
            success: function (cpkModels)
            {
                //console.log(cpkOnOffModels);
                SP_AppendFiiThemeCPKModel('#divTabCPKModel', cpkModels.cpkOnlineModels);
                SP_AppendFiiThemeModelSelection(cpkModels.cpkOfflineModels);
                //
                countOnline = cpkModels.cpkOnlineModels.length;
                
                
            },
            error: function (error)
            {
                alert('Error on calling function: \n' + error);
            },
            complete: function ()
            {
                $('.tab-cpk-model').on('click', function () {

                    $('#divCPKChartSection').html('');
                    //
                    if ($(this).hasClass('active')) {
                        $('.tab-cpk-model').removeClass('active');
                        $('.div-tab-station').addClass('d-none');
                    }
                    else {
                        $('.tab-cpk-model').removeClass('active');
                        $(this).addClass('active');


                        //
                        var cpk_model = $(this).data('model');
                        $('.div-tab-station.' + cpk_model.replace('.', '')).html('');
                        $('.div-tab-station').addClass('d-none');
                        $('.div-tab-station.' + cpk_model.replace('.', '')).removeClass('d-none');
                        /*console.log('clicked');
                        console.log(cpk_model);*/
                        AJAX_GET_CPKStationList(cpk_model, false);
                    }

                });
                if (countOnline < 1) {
                    SP_ControlDivModelOffline();
                } else {
                    $('.tab-cpk-model')[0].click();
                }
            }
        });
    }
    catch (e) {
        console.log('AJAX Exception: ' + { e: e });
    }
};
async function AJAX_GET_ListCPKModelStation() {
    try {
        await $.ajax({
            url: URL_GET_ListCPKModelStation,
            data: {},
            async: false,
            success: function (listCPKModelStation) {
                //console.log(listCPKModelStation);                
                var listGroupedCPKModelStation = GET_ListGroupedCPKModelStation(listCPKModelStation);
                console.log(listGroupedCPKModelStation);
                var cpkModelTitleHTML = '';
                $.each(listGroupedCPKModelStation, function (idx, ModelStation) {
                    cpkModelTitleHTML += '<div class="fii-frame-title col-sm-2 m-1"><img class="fii-icon-cube" src="/Assets/Vendor/fii-template/fii-theme/fii-icon-cube.png" /><img class="fii-icon-cube-active" src="/Assets/Vendor/fii-template/fii-theme/fii-icon-cube-active.png" />' +
                        ModelStation.Model + '</div>';

                });
                $('#divtTabCPKModel').html(cpkModelTitleHTML);
            },
            error: function (errorMessage) {
                alert('Error on calling function: \n' + errorMessage);
            }
        });
    }
    catch (e) {
        console.log('Exception at: ' + { e: e });
    }
};
async function AJAX_GET_CPKModelList() {
    try {
        await $.ajax({
            url: URL_GET_CPKModelList,            
            success: function (cpkModels) {
                /*$('#cboCPKModel option:gt(0)').remove();
                $.each(cpkModels, function (idx = 0) {

                    $('#cboCPKModel').append($('<option />').val(cpkModels[idx]).text(cpkModels[idx]));
                    idx++;
                });*/
                SP_AppendFiiThemeCPKModel('#divTabCPKModel',cpkModels);
            },
            error: function (errorMessage) {
                alert('Error on calling function: \n' + errorMessage);
            },
            complete: function () {                
                
                $('.tab-cpk-model').on('click', function () {
                    //

                    if ($(this).hasClass('active')) {
                        $('.tab-cpk-model').removeClass('active');
                        $('.div-tab-station').addClass('d-none');
                    }
                    else {
                        $('.tab-cpk-model').removeClass('active');
                        $(this).addClass('active');


                        //
                        var cpk_model = $(this).data('model');
                        $('.div-tab-station.' + cpk_model.replace('.', '')).html('');
                        $('.div-tab-station').addClass('d-none');
                        $('.div-tab-station.' + cpk_model.replace('.', '')).removeClass('d-none');
                        /*console.log('clicked');
                        console.log(cpk_model);*/
                        AJAX_GET_CPKStationList(cpk_model);
                    }
                    
                });
            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GET_CPKStationList(_model, _isOffline) {
    try {
        await $.ajax({
            url: URL_GET_CPKStationList,
            data: { model: _model },
            success: function (cpkStations) {
                /*$('#cboCPKStation option:gt(0)').remove();
                $.each(cpkStations, function (idx = 0) {

                    $('#cboCPKStation').append($('<option />').val(cpkStations[idx]).text(cpkStations[idx]));
                    idx++;
                });*/
                SP_AppendFiiThemeCPKStation(_model, cpkStations);

            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                $('.tab-cpk-station').on('click', function () {
                    $('#divCPKChartSection').html('');
                    //
                    $('.tab-cpk-station .fii-content-item').removeClass('active');
                    $(this).find('.fii-content-item').addClass('active');
                    //
                    let cpk_station = $(this).data('station');
                    let _timeSpanVal = SP_GetTimeSpanVal();                    
                    //console.log(timeSpanVal);
                    AJAX_GET_MOList(URL_GET_ListMoByModel, _model, _timeSpanVal.fromDate, _timeSpanVal.toDate);
                    AJAX_GET_PCList(URL_GET_PCList, _model, cpk_station, _timeSpanVal.fromDate, _timeSpanVal.toDate);
                    //FII_InitCustomSelect();
                    AJAX_GET_PartialTableCPKModelStationItem(_model, cpk_station, 0, '0', '0', _timeSpanVal.fromDate, _timeSpanVal.toDate);

                });
                // 
                let firstStationSelected;
                if (_isOffline) {
                    firstStationSelected = $('.tab-cpkmodel-offline .div-tab-station .tab-cpk-station')[0];                    
                    
                } else {
                    //let onlineStation = $('#divTabCPKModel .div-tab-station .tab-cpk-station');
                    firstStationSelected = $('.div-tab-station.' + _model.replace('.', '') + ' .tab-cpk-station')[0];
                    //console.log(firstStationSelected);
                }
                //
                if (firstStationSelected != undefined) {
                    firstStationSelected.click();
                } else {
                    $('#divTableModelStationItem').html('<div class="fii-nodata"><i class="fa fa-meh"></i><span> NO DATA TO DISPLAY!</span></div>');
                }
            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GET_PartialTableCPKModelStationItem(_model, _station, _pcsNum, _keyMO, _keyPC, _fromDay, _toDay)
{
    var divLoaderHTML = '<div class="page-loader d-flex justify-content-center align-items-center">' +        
                        '<img src="/Images/loaderGifs/Blue-tick.gif" />' + 
                        '</div>';
    $('#divTableModelStationItem').html(divLoaderHTML);
    try {
        await $.ajax({
            url: URL_GET_PartialTableCPKModelStationItem,
            data: { model: _model, station: _station, pcsNum: _pcsNum, keyMO: _keyMO, keyPC: _keyPC, fromDay: _fromDay, toDay: _toDay },
            success: function (partTableItemHTML) {
                $('#divTableModelStationItem').html(partTableItemHTML);
                $('#dataTable').DataTable();
            },
            error: function (error) {
                $('#divTableModelStationItem').html('<div class="fii-nodata"><i class="fa fa-meh"></i><span> NO DATA TO DISPLAY!</span></div>');
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                //
                /*$('.fii-form-select').html('');*/
                
                //
                
                $('#btnReCPKTable').on('click', function () {
                    //
                    $('#divCPKChartSection').html('');
                    //
                    let _model = $('#VAR_Model').val();
                    let _station = $('#VAR_Station').val();
                    let _keyMO = $('#cboCPKMO').val();
                    let _keyPC = $('#cboCPKPC').val();
                    let _timeSpanVal = SP_GetTimeSpanVal();
                    //console.log(cboMOVal + ' | ' + cboPCVal);
                    AJAX_GET_PartialTableCPKModelStationItem(_model, _station, 0, _keyMO, _keyPC, _timeSpanVal.fromDate, _timeSpanVal.toDate);
                });
            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GET_RedrawCPKChart(_modelname, _stationname, _pos, _pcsNumber, _moNumber, _atePC, _fromDay, _toDay) {
    var divLoaderHTML = '<div align="center" class="fii-frame-card mt-5 p-5">' +
        '<img src="/Images/loaderGifs/Blue-tick.gif" class="" width="100"/>' +        
        '</div>';
    $('#chartSection').html(divLoaderHTML);
    $('#divCPKChartSection').html(divLoaderHTML);

    try {
        await $.ajax({
            url: URL_GET_CPKChartSectionOfSelectedItem,
            type: 'GET',
            data: { model: _modelname, station: _stationname, pos: _pos, pcsNumber: _pcsNumber, moNumber: _moNumber, atePC: _atePC, fromDay: _fromDay, toDay: _toDay },
            success: function (cpkChartSectionHTML) {
                /*console.log(response);*/
                $('#chartSection').html(cpkChartSectionHTML);
                $('#divCPKChartSection').html(cpkChartSectionHTML);
            },
            error: function (error) {
                alert('Error on calling function');
            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GET_MOList(URL_GET_ListMoByModel, _modelname, _fromDay, _toDay) {
    $('.cbo-cpk-mo').html('<select id="cboCPKMO"><option value="0"> --- All --- </option></select> ');
    try {
        await $.ajax({
            url: URL_GET_ListMoByModel,
            type: 'GET',
            async: false,
            data: { model: _modelname, fromDay: _fromDay, toDay: _toDay },
            success: function (listMO) {
                $('#cboMo option:gt(0)').remove();
                $('#cboCPKMO option:gt(0)').remove();               
                $.each(listMO, function (idx = 0) {

                    $('#cboMo').append($('<option />').val(listMO[idx]).text(listMO[idx]));
                    $('#cboCPKMO').append($('<option />').val(listMO[idx]).text(listMO[idx]));
                    idx++;
                });
            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                
            }
        });
    }
    catch (e)
    {
        console.log('Error: ' + e);
    }
    
};
async function AJAX_GET_PCList(URL_GET_PCList, _modelname, _stationname, _fromDay, _toDay) {
    $('.cbo-cpk-pc').html('<select id="cboCPKPC"><option value="0"> --- All --- </option></select> ');
    try {
        await $.ajax({
            url: URL_GET_PCList,
            type: 'GET',
            async: false,
            data: { model: _modelname, station: _stationname, fromDay: _fromDay, toDay: _toDay },
            success: function (listPC) {
                $('#cboPC option:gt(0)').remove();
                $('#cboCPKPC option:gt(0)').remove();
                $.each(listPC, function (idx = 0) {

                    $('#cboPC').append($('<option />').val(listPC[idx]).text(listPC[idx]));
                    $('#cboCPKPC').append($('<option />').val(listPC[idx]).text(listPC[idx]));
                    idx++;
                });
            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                
            }
        });
    }
    catch (e) {
        console.log('Error: ' + e );
    }

};
// ====== Data Handling Functions ======
function GET_ListGroupedCPKModelStation(_listCPKModelStation)
{
    var listSize = _listCPKModelStation.length;
    var _listGroupedModel = [];
    var _checkedModel = '';

    for (var i = 0; i < listSize; i++) {
        var _listStation = [];
        var currentModel = _listCPKModelStation[i].ModelName;

        if (!_checkedModel.includes(currentModel)) {
            for (var j = 0; j < listSize; j++) {
                if (currentModel == _listCPKModelStation[j].ModelName) {
                    _listStation.push(_listCPKModelStation[j].Station);
                }
            }
            var objModelStation = { Model: currentModel, ListStation: _listStation };
            _listGroupedModel.push(objModelStation);
            _checkedModel += currentModel + ',';
        }


    }
    //window.sessionStorage.setItem('_listLineWOL', JSON.stringify(_listGroupedModel));

    return _listGroupedModel;
};
// ====== Support Functions ======
function SP_InitialCPKInputField() {
    var _fromDay = $('#fromDay').val();
    var _toDay = $('#toDay').val();
    var _modelname = $('#VAR_Model').val();
    var _stationname = $('#VAR_Station').val();
    AJAX_GET_MOList(URL_GET_ListMoByModel, _modelname, _fromDay, _toDay);
    AJAX_GET_PCList(URL_GET_PCList, _modelname, _stationname, _fromDay, _toDay);
};
function SP_InitialTimeSpan() {
    var currentDate = new Date();
    var endDate = moment(currentDate).format('YYYY/MM/DD 07:30');
    var startDate = moment(currentDate).subtract('1', 'day').format('YYYY/MM/DD 07:30');

    $('#timeSpan').daterangepicker({
        opens: "right",
        applyClass: 'bg-slate-600',
        cancelClass: 'btn-default',
        startDate: startDate,
        endDate: endDate,
        /*maxDate: endDate,*/
        timePicker: true,
        timePicker24h: true,
        timePcikerIncrement: 30,
        locale: {
            format: 'YYYY/MM/DD HH:mm'
        }
    });
};
function SP_GetTimeSpanVal() {
    var timeVal = $('#timeSpan').val();
    var timeValSplit = timeVal.split('-');
    var _fromDate = moment(new Date(timeValSplit[0].trim())).format('YYYY-MM-DDTHH:mm');
    var _toDate = moment(new Date(timeValSplit[1].trim())).format('YYYY-MM-DDTHH:mm');
    return {
        fromDate: _fromDate,
        toDate: _toDate
    };
};
function SP_ControlDivModelOnline()
{
    let divTabCPKOfflineModel = $('#divTabCPKOfflineModel');
    divTabCPKOfflineModel.addClass('d-none');

    let divTabCPKModel = $('#divTabCPKModel');
    if (divTabCPKModel.hasClass('d-none'))
    {
        divTabCPKModel.removeClass('d-none');
    }
    else
    {
        divTabCPKModel.addClass('d-none');
    }
};
function SP_ControlDivModelOffline() {
    let divTabCPKModel = $('#divTabCPKModel');
    divTabCPKModel.addClass('d-none');
    //
    let divTabCPKOfflineModel = $('#divTabCPKOfflineModel');
    if (divTabCPKOfflineModel.hasClass('d-none'))
    {
        divTabCPKOfflineModel.removeClass('d-none');
    }
    else
    {
        divTabCPKOfflineModel.addClass('d-none');
    }
    //
    let cboCPKModel = $('#cboCPKModel');
    let firstOfflineCPKModel = $('#cboCPKModel option:gt(2)').val();
    cboCPKModel.val(firstOfflineCPKModel);
    $('.tab-cpkmodel-offline').html('<div id="" class="div-tab-station ' + firstOfflineCPKModel.replace('.', '') + ' w-"></div>');
    AJAX_GET_CPKStationList(firstOfflineCPKModel,true);    
};
function SP_AppendFiiThemeModelSelection(_listModel)
{
    $('.cbo-cpk-model').html('<select id="cboCPKModel"><option value="0"> --- All --- </option></select> ');
    $.each(_listModel, function (idx, _model) {
        
        $('#cboCPKModel').append($('<option />').val(_model).text(_model));
        
    });

};
function SP_AppendFiiThemeCPKModel(_divContainerID, _listModel)
{
    var cpkModelTitleHTML = '';
    $.each(_listModel, function (idx, _model) {
        cpkModelTitleHTML += '<div class="tab-cpk-model fii-frame-title" data-model="' + _model + '"><img class="fii-icon-cube" src="/Assets/Vendor/fii-template/fii-theme/fii-icon-cube.png" /><img class="fii-icon-cube-active" src="/Assets/Vendor/fii-template/fii-theme/fii-icon-cube-active.png" />' +
            _model + '</div><div id="" class="div-tab-station ' + _model.replace('.','') + ' w-"></div>';

    });
    $(_divContainerID).html(cpkModelTitleHTML);
};
function SP_AppendFiiThemeCPKStation(_model,_listStation) {
    var cpkStationTitleHTML = '';
    $.each(_listStation, function (idx, _station) {
        /*cpkStationTitleHTML += '<div class="tab-cpk-station fii-frame-arrow  m-1" data-station="' + _station + '">' +
            _station + '</div>';*/
        cpkStationTitleHTML += '<div class="tab-cpk-station fii-frame-content m-1" data-station="' + _station + '">' +
            '<div class="fii-content-item ">' +
            '<a href="#">' +
            '<img class="fii-point" src="/Assets/Vendor/fii-template/fii-theme/fii-point-square.png" />' +
            '<span class="fii-content-item-name ml-2">' + _station +'</span>' +
            '</a></div>' +
            '</div>';

    });
    
    $('.div-tab-station.' + _model.replace('.', '')).html(cpkStationTitleHTML);
};

// ====== JS Data Models ======
class CPKModelStation
{
    model;
    listStation;
    constructor(_model, _listStation){
        this.model = _model;
        this.listStation = _listStation;
    }
};