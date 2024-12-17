//=== AJAX URL ===
var URL_GET_LCRShiftData = $('#URL_GET_LCRShiftData').val();
var URL_GET_LCRDataInTimeRange = $('#URL_GET_LCRDataInTimeRange').val();
var URL_GET_Partial_IPQCLCRDataTable = $('#URL_GET_Partial_IPQCLCRDataTable').val();
var URL_GET_LCRDataByMaterialId = $('#URL_GET_LCRDataByMaterialId').val();

//=== Global Variable ===
var _fromDate, _toDate;
//=== Event Functions ===
$(window).on('load', function () {
    LoadTimeSpan();
    $('#btnTimeSpan').click();    
});
$('#btnTimeSpan').on('click', function () {
    var timeSpanVal = $('#timeSpan').val();
    var timeSpanSplit = timeSpanVal.split('-');
    _fromDate = moment(new Date(timeSpanSplit[0].trim())).format('YYYY-MM-DD');
    _toDate = moment(new Date(timeSpanSplit[1].trim())).format('YYYY-MM-DD');

    AJAX_GET_LCRShiftData();
});
//=== AJAX Requesting Functions ===
function AJAX_GET_LCRShiftData() {
    try {
        //console.log(_fromDate + ' | ' + _toDate);
        $('#divLCRChartSection').html('')
        var divLoaderHTML = '<div align="center">' +
            '<img src="/Images/loaderGifs/Spinner-3.gif" />' +
            '<h4 class="text-white">Loading...</h4>' +
            '</div>';
        $('#divLCRChartSection').html(divLoaderHTML);
        
        $.ajax({
            url: URL_GET_LCRShiftData,
            data: { fromDate: _fromDate, toDate: _toDate },            
            success: function (listLCRShiftData) {
                
                DrawLCRShiftCompareChart(listLCRShiftData);
                GetTotalLCRPCSTable(listLCRShiftData);
            },
            error: function (error) {
                console.log('Error: ' + error);
            }
        });
    } catch (e) {
        console.log('Exception caught: ' + { e: e });
    }
};
async function AJAX_GET_LCRDataInTimeRange() {
    var timeSpanVal = $('#timeSpan').val();
    var timeSpanSplit = timeSpanVal.split('-');
    _fromDate = moment(new Date(timeSpanSplit[0].trim())).format('YYYY-MM-DD');
    _toDate = moment(new Date(timeSpanSplit[1].trim())).format('YYYY-MM-DD');
    try {        

        await $.ajax({
            url: URL_GET_LCRDataInTimeRange,
            data: { fromDate: _fromDate, toDate: _toDate },
            async: false,
            success: function (listLCRData) {

                console.log(listLCRData);
            },
            error: function (error) {
                console.log('Error: ' + error);
            }
        });
    } catch (e) {
        console.log('Exception caught: ' + { e: e });
    }
};
async function AJAX_GET_Partial_IPQCLCRDataTable() {
    var timeSpanVal = $('#timeSpan').val();
    var timeSpanSplit = timeSpanVal.split('-');
    _fromDate = moment(new Date(timeSpanSplit[0].trim())).format('YYYY-MM-DD');
    _toDate = moment(new Date(timeSpanSplit[1].trim())).format('YYYY-MM-DD');
    try {

        await $.ajax({
            url: URL_GET_Partial_IPQCLCRDataTable,
            data: { fromDate: _fromDate, toDate: _toDate },
            async: false,
            success: function (PartialLCRData) {

                //console.log(PartialLCRData);
                $('#divTblLCRData').html(PartialLCRData);
                createLCRDataTable();
            },
            error: function (error) {
                console.log('Error: ' + error);
            }
        });
    } catch (e) {
        console.log('Exception caught: ' + { e: e });
    }
};
async function AJAX_GET_LCRDataByMaterialId() {
    var _materialId = 'E65899';
    
    try {

        await $.ajax({
            url: URL_GET_LCRDataByMaterialId,
            data: { materialId: _materialId },
            async: false,
            success: function (LCRMaterialData) {

                console.log(LCRMaterialData);
            },
            error: function (error) {
                console.log('Error: ' + error);
            }
        });
    } catch (e) {
        console.log('Exception caught: ' + { e: e });
    }
};
//=== Support Functions ===
function LoadTimeSpan() {
    var currentDate = new Date();
    var endDate = moment(currentDate).format('YYYY/MM/DD');
    var startDate = moment(currentDate).subtract('7', 'day').format('YYYY/MM/DD');

    $('#timeSpan').daterangepicker({
        timePicker: false,
        opens: "right",
        applyClass: 'bg-slate-600',
        cancelClass: 'btn-default',
        startDate: startDate,
        endDate: endDate,
        /*maxDate: endDate,*/
        locale: {
            format: 'YYYY/MM/DD'
        }
    });

    
};
function DrawLCRShiftCompareChart(_listLCRShiftData) {
    Highcharts.chart('divLCRChartSection', {
        chart: {
            backgroundColor: 'rgba(0,0,0,0)',
            type:'column'
            // styledMode: true
        },
        title: {
            text: 'IPQC LCR Work Shifts Comparing',
            style: {
                color: 'black'
            }
        },
        xAxis: {
            type: 'category',
            categories: _listLCRShiftData[0].ListWorkDay,
            labels: {
                style: {
                    color: 'black',
                    fontWeight: 'bold'
                }
            }
        },
        yAxis: [{
            min: 0,
            softMax: 30,
            lineWidth: 1,
            gridLineWidth: 1,
            gridLineColor: '#6a6d70',
            title: {
                text: ''
            },
            labels: {
                formatter: function () {
                    var label_custom = this.value + 'PCS';
                    return label_custom;
                },
                style: {
                    color: 'black',
                    fontWeight: 'bold'
                }
            },
        }],
        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            padding: 0,
            itemStyle: {
                color: 'black',
                fontWeight: 'bold'
            }
        },
        navigation: {
            buttonOptions: {
                enabled: false
            }
        },
        credits: {
            enabled: false
        },
        tooltip: {
            /*backgroundColor: '#343A40',
            borderColor: '#343A40',*/
            color: '#f8f8f8',
            /*useHTML: true,
            shared: true,
            formatter: function () {
                var label = '';
                var arrPoints = this.points;
                label += '<span style="font-weight:bold;color: #f8f8f8;">' + this.x + '</span><br/>';
                label += '<span style="color: #f8f8f8;">'+series.name+' : '+this.y+'</span><br/>';
                label += '<span style="color: #f8f8f8;">Total: '+point.stackTotal+'</span><br/>';
                
                return label;
            },*/
            format: '<b>{key}</b><br/>{series.name}: {y}<br/>',
        },
        plotOptions: {
            series: {
                cursor: 'pointer',
            },
            column: {
                //stacking:'normal',
                maxPointWidth: 0,
                dataLabels: {
                    enabled: true,
                    color: 'black',
                    format: '{point.y} PCS'
                },
                borderWidth: 0
            },
            spline: {
                dataLabels: {
                    enabled: true,
                    format: '{point.y} PCS'
                },
                lineWidth: 1,
            }
        },
        series: [{
            /*type: 'spline',*/
            name: 'Day shift',
            data: _listLCRShiftData[0].ListPCS,
            color: '#f4d03f',
            shadow: {
                width: 6,
                opacity: 0.1,
                color: '#f4d03f'
            },
            yAxis: 0,
        },
        {
            /*type: 'spline',*/
            name: 'Night shift',
            data: _listLCRShiftData[1].ListPCS,
            color: '#2471a3',
            shadow: {
                width: 6,
                opacity: 0.1,
                color: '#2471a3'
            },
            yAxis: 0,
        }
        ],
        /*responsive: {
            rules: [{
                condition: {
                    maxWidth: 1365
                },
                chartOptions: {
                    legend: {
                        itemStyle: {
                            fontSize: '0.6rem',
                        }
                    },
                    plotOptions: {
                        spline: {
                            dataLabels: {
                                style: {
                                    fontSize: '0.6rem',
                                },
                            }
                        }
                    }
                }
            }]
        }*/
    });
};
function GetTotalLCRPCSTable(_listLCRShiftData) {    
    $('#tbdLCRDayTotal').html('');
    $('#tbdLCRShiftTotal').html('');
    var LCRWorkDay = _listLCRShiftData[0].ListWorkDay;
    var LCRDataLength = LCRWorkDay.length;
    var LCRDayShiftPCS = _listLCRShiftData[0].ListPCS;
    var LCRNightShiftPCS = _listLCRShiftData[1].ListPCS;
    //console.log(LCRWorkDay);
    //console.log(LCRDayShiftPCS);
    //console.log(LCRNightShiftPCS);
    var dayShiftPCSTotal = 0;
    var nightShiftPCSTotal = 0;
    // LCR Day total table
    var tbdLCRDayTotalHTML = '<tr id="tbdRowLCRDay" class="bg-info text-white fw-bold" ></tr><tr id="tbdRowLCRPCS"></tr>';
    $('#tbdLCRDayTotal').html(tbdLCRDayTotalHTML);
    var tbdRowLCRDayHTML = '<td class="w-10" >Days</td>';
    var tbdRowLCRPCSHTML = '<td class="bg-info w-10 text-white fw-bold">Total PCS</td>';
    for (var i = 0; i < LCRDataLength; i++) {
        tbdRowLCRDayHTML += '<td>' + LCRWorkDay[i] + '</td>';
        tbdRowLCRPCSHTML += '<td>' + (LCRDayShiftPCS[i] + LCRNightShiftPCS[i]) + '</td>';
        dayShiftPCSTotal += LCRDayShiftPCS[i];
        nightShiftPCSTotal += LCRNightShiftPCS[i];
    }
    $('#tbdRowLCRDay').html(tbdRowLCRDayHTML);
    $('#tbdRowLCRPCS').html(tbdRowLCRPCSHTML);
    // LCR Shift total table
    var tbdLCRShiftTotalHTML = '<tr id="tbdRowLCRShift" class="fw-bold text-white"  >' +
                                '<td class="bg-info w-10" >Shifts</td>'+
                                '<td style="background-color: #f4d03f;">Day Shift</td>'+
                                '<td style="background-color: #2471a3;">Night Shift</td>'+
                                '</tr>' +
        '<tr id="tbdRowLCRShiftPCS">' +
        '<td class="bg-info w-10 text-white fw-bold" >Total PCS</td>' +
        '<td class="" >' + dayShiftPCSTotal +'</td>' +
        '<td class="" >' + nightShiftPCSTotal +'</td>' +
        '</tr>';
    $('#tbdLCRShiftTotal').html(tbdLCRShiftTotalHTML);

}