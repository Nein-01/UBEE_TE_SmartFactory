// === AJAX URLs ===
var URL_GET_ATETimeModels = $('#URL_GET_ATETimeModels').val();
var URL_GET_ATETimeModelStations = $('#URL_GET_ATETimeModelStations').val();
var URL_GET_ATETimeModelStationData = $('#URL_GET_ATETimeModelStationData').val();
var URL_ATETimeChartSection = $('#URL_ATETimeChartSection').val();
var URL_GET_ATETimeOfMachine = $('#URL_GET_ATETimeOfMachine').val();
var URL_POST_ATETimeByWorkDate;
// === Global Variables ===
var chartHeight = 280 + 'px';
// Define global data tranfer variable 
var _listATETime, _listStation;
// === Event Functions ===
$(window).on('load', function () {
   
    LoadTimeSpan();
    $('#divATETimeChart').addClass('d-none');
    GET_LoadATETimeGraph();
});
$('#timeSpan').on('change', function () {
    GET_LoadATETimeGraph();
});
$('#btnTimeSpan').on('click', function () {
    GET_LoadATETimeGraph();
});
$('#txtKeyWord').on('keyup', function () {
    const val = this.value.toUpperCase();
    let content_item = document.getElementsByClassName('container-atetime-preview');
    let have_any_content_selected = document.getElementsByClassName('selected-atetime-preview');
    //console.log(val);
    //console.log(content_item);
    //$(content_item).addClass('d-none');
    if (have_any_content_selected.length === 0) {
        for (var i = 0; i < content_item.length; i++) {
            if (content_item[i].textContent.indexOf(val) > -1) {

                //content_item[i].style.display = 'block';
                $(content_item[i]).removeClass('d-none');
            } else {

                //content_item[i].style.display = 'none';
                $(content_item[i]).addClass('d-none');
            }
        }
    }
    
});

function LoadModel() {
    var _model = $('#cboModel').val();
    var _fromDate = $('#fromDay').val();
    var _toDate = $('#toDay').val();
    /*console.clear();
    console.log(_model);*/
    $.ajax({
        url: URL_ATETimeChartSection,
        data: { model: _model, fromDate: _fromDate, toDate: _toDate },
        success: function (Partial_ATETimeGraphs) {
            $('#divATETimeChart').html(Partial_ATETimeGraphs);
        },
        error: function () {
            console.log('Error on calling function!');
        }
    });

};
function GET_LoadATETimeGraph() {
    

    if ($('#divATETimeChart').hasClass('d-none')) {

        AJAX_GET_ATETimeModels();
    } else {
        var atetime_model = $('.selected-atetime-preview').attr('atetime_model');
        //console.log(atetime_model);
        AJAX_GET_PartialATETimeChartSection(atetime_model);
    }
}
function GET_DivModelStation() {
    var _valTimeSpan = GetValTimeSpan();
    $.each(_listATEModel, function (idx, _model) {        
        AJAX_GET_ATETimeModelStations(_model, _valTimeSpan.fromDate, _valTimeSpan.toDate);
    });
}
// === AJAX Requesting Functions ===
function AJAX_GET_ATETimeModels() {
    $('#divATETimePreview').removeClass('d-none');
    $('#divATETimeChart').addClass('d-none');

    var divLoaderHTML = '<div class="page-loader d-flex justify-content-center align-items-center">' +
        '<img src="/Images/loaderGifs/Blue-tick.gif" class="w-10" />' +
        '</div>';
    $('#divATETimePreview').html(divLoaderHTML);
    
    var _valTimeSpan = GetValTimeSpan();
    var _listATEModel = [];
    try {
        $.ajax({
            url: URL_GET_ATETimeModels,
            data: { fromDate: _valTimeSpan.fromDate, toDate: _valTimeSpan.toDate },
            //async: false,
            success: function (listATEModel) {

                //console.log(listATEModel);
                _listATEModel = listATEModel;
                var divATEModelHTML = '';
                $.each(listATEModel, function (idx, _model) {
                    divATEModelHTML += '<div class="fii-frame-subcontent container-atetime-preview w-30 p-1 m-1" atetime_model="'+_model+'" >' +
                        '<h5 class="fii-chart-title header-atetime p-1 rounded font-weight-bold text-center border-black"><span class="atelist-return-group float-start"><i class="fa "></i> <span class="atelist-return-content"></span> </span> <span>' + _model + ' ATE Times</span></h5>' +
                        '<div id="div' + _model.replace('.', '_') + '" class="content-atetime-preview w-100 p-1"> </div></div>';
                    /*$('#divATETimePreview').append(divATEModelHTML);
                    AJAX_GET_ATETimeModelStations(_model, _valTimeSpan.fromDate, _valTimeSpan.toDate);*/
                });    
                $('#divATETimePreview').html(divATEModelHTML);
            },
            error: function (errorMess) {
                var errorHTML = '<tr align="center">' +
                    '<td colspan="4">' +
                    '<h6 class="text-danger">Error!</h6>' +
                    '</td>'
                '</tr>';
                //$('#').html(errorHTML);
                console.log('Error on calling function: ' + errorMess);
            },
            complete: function () {
                // Append pie charts of each active model
                $.each(_listATEModel, function (idx, _model) {                    
                    //AJAX_GET_ATETimeModelStationData(_model, _valTimeSpan.fromDate, _valTimeSpan.toDate);
                    var _listATEModelStationData = AJAX_GET_ATETimeModelStationData(_model, _valTimeSpan.fromDate, _valTimeSpan.toDate);                    
                    //console.log(_listATEModelStationData);
                    AJAX_GET_ATETimeModelStations(_model, _valTimeSpan.fromDate, _valTimeSpan.toDate, _listATEModelStationData);

                    // === Testing ===
                    //TEST_GET_ATETimeOfMachine(_model, _valTimeSpan.fromDate, _valTimeSpan.toDate);
                });   
                // Create event function to display detail ATETime charts of a model
                $('.container-atetime-preview').on('click', function () {
                    let atetime_model = $(this).attr('atetime_model');
                    //console.log(atetime_model);
                    if (!$(this).hasClass('selected-atetime-preview')) {
                        $('#divATETimeChart').removeClass('d-none');
                        $('.fa').addClass('fa-arrow-alt-circle-left');
                        $('.atelist-return-content').html('Back');
                        $('.header-atetime').addClass('selected');
                        $('.container-atetime-preview,.content-atetime-preview').addClass('d-none');
                        $(this).removeClass('w-30');
                        $(this).addClass('w-100');
                        $(this).removeClass('d-none');
                        $(this).addClass('selected-atetime-preview');

                        AJAX_GET_PartialATETimeChartSection(atetime_model);

                    } else {
                        $(this).removeClass('w-100');
                        $(this).addClass('w-30');
                        $(this).removeClass('selected-atetime-preview');
                        $('.fa').removeClass('fa-arrow-alt-circle-left');
                        $('.atelist-return-content').html('');
                        $('.header-atetime').removeClass('selected');
                        $('.container-atetime-preview,.content-atetime-preview').removeClass('d-none');
                        $('#divATETimeChart').addClass('d-none');
                        let ThisModelAteTimeStatusPanel = document.getElementById('div' + atetime_model.replace('.', '_'));// $('#div' + atetime_model.replace('.', '_'));
                        ThisModelAteTimeStatusPanel.parentElement.scrollIntoView(true);
                    }
                    

                });
            }
        }); 
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
};
async function AJAX_GET_ATETimeModelStations(_model, _fromDate, _toDate, _listATEModelStationData) {
    var _listATEModelStation = [];
    try {
        await $.ajax({
            url: URL_GET_ATETimeModelStations,
            data: { model: _model, fromDate: _fromDate, toDate: _toDate },
            async: false,
            success: function (listATEModelStation) {

                //console.log(listATEModelStation);
                _listATEModelStation = listATEModelStation;
                var divATEModelStationHTML = '';
                _model = _model.replace('.', '_'); // id attribute doesn't accept '.' character
                $.each(listATEModelStation, function (idx, _station) {
                    divATEModelStationHTML += '<div id="div' + _model + _station + '" class="fii-border w-20 rounded border- mb-2"> </div>';
                });
                                
                $('#div' + _model).append(divATEModelStationHTML);
                
            },
            error: function (errorMess) {
                var errorHTML = '<tr align="center">' +
                    '<td colspan="4">' +
                    '<h6 class="text-danger">Error!</h6>' +
                    '</td>'
                '</tr>';
                //$('#').html(errorHTML);
                console.log('Error on calling function: ' + errorMess);
            },
            complete: function () {

                //ChartPie_ATETimePreview(_model, _listATEModelStation, _listATEModelStationData);
                let _colChartStatusData = TestTimeColStatusDataPrepare(_model, _listATEModelStation, _listATEModelStationData);
                //console.log(_colChartStatusData);
                let _colChartID = 'div' + _model;
                ColChart_StationsTestTime(_model.replace('_', '.'), _colChartStatusData, _colChartID);
            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
};
function AJAX_GET_ATETimeModelStationData(_model, _fromDate, _toDate) {
    var _listATEModelStationData = [];
    try {
         $.ajax({
            url: URL_GET_ATETimeModelStationData,
            data: { model: _model, fromDate: _fromDate, toDate: _toDate },
            async: false,
            success: function (listATEModelStationData) {

                //console.log(listATEModelStationData);                
                _listATEModelStationData = listATEModelStationData;
            },
            error: function (errorMess) {
                var errorHTML = '<tr align="center">' +
                    '<td colspan="4">' +
                    '<h6 class="text-danger">Error!</h6>' +
                    '</td>'
                '</tr>';
                //$('#').html(errorHTML);
                console.log('Error on calling function: ' + errorMess);
            },
            complete: function () {
                
            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
    return _listATEModelStationData;
};
function AJAX_GET_PartialATETimeChartSection(_model) {

    // GET Model ATE Times Graphs
    var _valTimeSpan = GetValTimeSpan();
    var _fromDate = _valTimeSpan.fromDate,
        _toDate = _valTimeSpan.toDate;

    try {
        $.ajax({
            url: URL_ATETimeChartSection,
            data: { model: _model, fromDate: _fromDate, toDate: _toDate },
            success: function (Partial_ATETimeGraphs) {
                $('#divATETimeChart').html(Partial_ATETimeGraphs);
            },
            error: function (errorMessage) {
                console.log('Error on calling function: ' + errorMessage);
            }
        });
    }
    catch (e) {
        console.log('Exception: ' + e);
    }

}
// === Support Functions ===
function LoadTimeSpan() {
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
function GetValTimeSpan() {
    var timeVal = $('#timeSpan').val();
    var timeValSplit = timeVal.split('-');
    var _fromDate = moment(new Date(timeValSplit[0].trim())).format('YYYY-MM-DDTHH:mm');
    var _toDate = moment(new Date(timeValSplit[1].trim())).format('YYYY-MM-DDTHH:mm');
    return {
        fromDate: _fromDate,
        toDate: _toDate
    };
};
// === Testing Functions ===
function TEST_GET_ATETimeOfMachine(_model, _fromDate, _toDate) {
    try {
        $.ajax({
            url: URL_GET_ATETimeOfMachine,
            data: { model: _model, fromDate: _fromDate, toDate: _toDate },
            async: false,
            success: function (listATETimeOfMachine) {

                console.log(listATETimeOfMachine);                                
            },
            error: function (errorMess) {
                var errorHTML = '<tr align="center">' +
                    '<td colspan="4">' +
                    '<h6 class="text-danger">Error!</h6>' +
                    '</td>'
                '</tr>';
                //$('#').html(errorHTML);
                console.log('Error on calling function: ' + errorMess);
            },
            complete: function () {

            }
        });
    }
    catch (e) {
        console.log('Error: ' + { e: e });
    }
}