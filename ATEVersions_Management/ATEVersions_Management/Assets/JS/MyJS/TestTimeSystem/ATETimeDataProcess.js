// ====== Chart Drawing ======
function DrawATETimeCharts(_listStation, _listATETime) {
    $.each(_listStation, function (idx, station) {

        var pieChartID = station + '_PieChart',
            lineChartID = station + "_LineChart";
        //console.log(pieChartID + '|' + lineChartID);
        var GraphDrawData = ChartData_ATETimePrepare(station, _listATETime);
        //var GraphDrawData = TestTimeDataPrepare(station, _listATETime);
        //LineChart_StationsTestTime(GraphDrawData, lineChartID);
        SplineChart_ATETestAndLoadTime(lineChartID, GraphDrawData);
        //DrawTestTimeLineChart(GraphDrawData, lineChartID);
        DrawTestTimePieChart(GraphDrawData.PieData, pieChartID, GraphDrawData.Station);
    });
};
function ChartPie_ATETimePreview(_model, _listStation, _listATETimeData) {
    $.each(_listStation, function (idx, _station) {
        var pieChartID = 'div' + _model + _station;
        //console.log(pieChartID);
        var GraphDrawData = TestTimeDataPrepare(_station, _listATETimeData);
        DrawTestTimePieChart(GraphDrawData.PieData, pieChartID, GraphDrawData.Station);
    });    
}

// ====== Chart Data Processing ======
// === All Station ===
function TestTimeDataPrepare(station, _listATETime) {
    // Process data of station
    var _yPlotData = [];
    var _xCategories = [];
    var size = _listATETime.length;
    for (var i = 0; i < size; i++) {
        if (_listATETime[i].Station === station) {
            _yPlotData.push(parseFloat(_listATETime[i].MeanTime));
            _xCategories.push(_listATETime[i].ATEMachine);
        }
    }
    // Define chart component

    var _dataLabel = true;
    (_yPlotData.length > 15) ? _dataLabel = false : _dataLabel ;

    var _max = Math.max(..._yPlotData), _min = Math.min(..._yPlotData);
    _max = undefined, _min = undefined;
    var _avg = _yPlotData.reduce((a, b) => a + b) / _yPlotData.length;
    _avg = _avg.toFixed(2);
    var _dangerLine = parseFloat(_avg) + 10;
    // Station AVG and DANGER line 
    var _stationAVGDanger = { StationAVG: parseFloat(_avg), StationDanger: _dangerLine };
    window.localStorage.setItem(station + 'AVGDanger', JSON.stringify(_stationAVGDanger));
    // Get pie chart data
    var _pieData = TestTimePieDataPrepare(_yPlotData, _avg, _dangerLine);
    return {
        YPlotData: _yPlotData,
        XCategories: _xCategories,
        PieData: _pieData,
        Avg: _avg,
        Max: _max,
        Min: _min,
        DangerLine: _dangerLine,
        DataLabel: _dataLabel,
        Station: station
    }
}
function ChartData_ATETimePrepare(station, _listDataATETime) {
    // Initial and calculate chart data of station
    let _yATETimeData = [];
    let _yLoadTimeData = [];
    let _xCategories = [];
    let size = _listDataATETime.length;
    for (let i = 0; i < size; i++) {
        if (_listDataATETime[i].Station === station) {
            _yATETimeData.push(parseFloat(_listDataATETime[i].MeanTime));
            _yLoadTimeData.push(parseFloat(_listDataATETime[i].LoadTime));
            _xCategories.push(_listDataATETime[i].ATEMachine);
        }
    }    
    // Define chart component

    let _bVisibleDataLabel = true;
    (_yATETimeData.length > 15) ? _bVisibleDataLabel = false : _bVisibleDataLabel;

    let _max = Math.max(..._yATETimeData), _min = Math.min(..._yATETimeData);
    _max = undefined, _min = undefined;
    let _avg = _yATETimeData.reduce((a, b) => a + b) / _yATETimeData.length;
    _avg = _avg.toFixed(2);
    let _dangerSpec = parseFloat(_avg) + 10;
    // Station AVG and DANGER line 
    let _stationAVGDanger = { StationAVG: parseFloat(_avg), StationDanger: _dangerSpec };
    window.localStorage.setItem(station + 'AVGDanger', JSON.stringify(_stationAVGDanger));
    // Get pie chart data
    let _pieData = TestTimePieDataPrepare(_yATETimeData, _avg, _dangerSpec);
    return {
        YATETimeData: _yATETimeData,
        YLoadTimeData: _yLoadTimeData,
        XCategories: _xCategories,
        PieData: _pieData,
        Avg: _avg,
        Max: _max,
        Min: _min,
        DangerLine: _dangerSpec,
        DataLabel: _bVisibleDataLabel,
        Station: station
    }
}
function TestTimePieDataPrepare(_data, avg, dangerLine) {
    //Pie chart data processing
    //Divide data group
    var countNormal = 0, countAbnormal = 0, countWarn = 0;
    for (var i = 0; i < _data.length; i++) {
        if (_data[i] <= avg) countNormal++;
        if (_data[i] > avg && _data[i] <= dangerLine) countWarn++;
        if (_data[i] > dangerLine) countAbnormal++;
    }
    //Calculate group percentite
    var percentNorm = ((countNormal / _data.length) * 100);
    var percentAbnorm = ((countAbnormal / _data.length) * 100);
    var percentWarn = ((countWarn / _data.length) * 100);
    percentNorm = parseFloat(percentNorm.toFixed(2));
    percentAbnorm = parseFloat(percentAbnorm.toFixed(2));
    percentWarn = parseFloat(percentWarn.toFixed(2));
    //console.log(percentNorm + '|' + percentAbnorm + '|' + percentWarn);
    //Parse to data object
    var NormalPortion = { color: '#55bf3b', name: 'Normal', y: percentNorm };
    var AbnormalPortion = { color: '#ff0066', name: 'Abnormal', y: percentAbnorm };
    var WarningPortion = { color: 'gold', name: 'Warning', y: percentWarn };
    return [NormalPortion, WarningPortion, AbnormalPortion];
}
function TestTimeColStatusDataPrepare(_model, _listStation, _listATETimeData) {    
    let _listStatusNormal = [],
        _listStatusWarning = [],
        _listStatusAbnormal = [];
    $.each(_listStation, function (idx, _station) {               
        let TestTimeData = TestTimeDataPrepare(_station, _listATETimeData);
        _listStatusNormal.push(TestTimeData.PieData[0].y);
        _listStatusWarning.push(TestTimeData.PieData[1].y);
        _listStatusAbnormal.push(TestTimeData.PieData[2].y);
    });
    return {
        ListStation: _listStation,
        ListNormal: _listStatusNormal,
        ListWarning: _listStatusWarning,
        ListAbnormal: _listStatusAbnormal,
    }
}
// === ATE Invidual ===
function GetDataToModal(_modelHeader, _stationHeader) {
    // Get data to modal        

    // 
    _listATETime = JSON.parse(window.localStorage.getItem('listATETime'));

    // Define data table HMTL
    var ATETimeTableHTML = '<table class="table table-bordered text-white" id="tableATETimeList">' +
        '<thead>' +
        '<tr>' +
        '<th class="text-center">No</th>' +
        '<th>ATE</th>' +
        '<th>PCS</th>' +
        '<th>Mean</th>' +
        '<th>Min</th>' +
        '<th>Max</th>' +
        '<th>Range</th>' +
        '<th class="text-center">Graph</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody id="tbodyATETimeList">' +

        '</tbody>' +
        '</table>';
    $('#divTableATETimeList').html(ATETimeTableHTML);
    // Append data row to data table
    var count = 1;
    var StationAVGDanger = JSON.parse(window.localStorage.getItem(_stationHeader + 'AVGDanger'));
    var txtStationAVG = _modelHeader + ' - ' + _stationHeader + ' - AVG: ' + StationAVGDanger.StationAVG
    $('#spanStationAVG').text(txtStationAVG);
    //console.log(StationAVGDanger);
    $.each(_listATETime, function (idx = 0, data) {

        if (data.Station === _stationHeader) {
            var trColor = 'bg-success';
            (data.MeanTime > StationAVGDanger.StationDanger) ? trColor = 'bg-danger' :
                ((data.MeanTime <= StationAVGDanger.StationDanger &&
                    data.MeanTime > StationAVGDanger.StationAVG) ?
                    trColor = 'bg-warning' : trColor);
            // Define table tbody data row
            var trATETimeHTML = '<tr >' +
                '<td class="text-center">' + count + '</td>' +
                '<td>' + data.ATEMachine + '</td>' +
                '<td>' + data.PCS + '</td>' +
                '<td class="' + trColor + '">' + data.MeanTime + '</td>' +
                '<td>' + data.MinTime + '</td>' +
                '<td>' + data.MaxTime + '</td>' +
                '<td>' + data.RangeTime + '</td>' +
                '<td align="center">' +
                '<button class="btn btn-sm btn-outline-info" data-toggle="modal" data-target="#ATETimeChartModal" ' +
                ' onclick="PostATETimeByDate(\'' + _modelHeader + '\',\'' + _stationHeader + '\',\'' + data.ATEMachine + '\')" >' +
                '<i class="fas fa-chart-line"></i>' +
                '</button>' +
                '</td>' +
                '</tr>';
            count++;
            $('#tbodyATETimeList').append(trATETimeHTML);
        }

    });
}

function PostATETimeByDate(_modelHeader, _stationHeader, _ateMachine) {
    var _valTimeSpan = GetValTimeSpan();
    var _fromDate = _valTimeSpan.fromDate, // $('#fromDay').val() $('#toDay').val()
        _toDate = _valTimeSpan.toDate;

    var ATETimeDateChartID = 'ATETimeChartModalBody';
    $.ajax({
        url: URL_POST_ATETimeByWorkDate,
        data: { model: _modelHeader, station: _stationHeader, ateMachine: _ateMachine, fromDate: _fromDate, toDate: _toDate },
        success: function (result) {
            var _listATETimeDate = ATETimeByDatePrepare(result);
            DrawATETimeDateLineChart(_listATETimeDate, ATETimeDateChartID, _modelHeader, _stationHeader, _ateMachine);
        },
        error: function () {
            console.log('Error on calling function!');
        }
    });
}

function ATETimeByDatePrepare(_listAteTimeDate) {
    // Process data of passed list
    var _yPlotData = [];
    var _xCategories = [];
    var size = _listAteTimeDate.length;
    for (var i = 0; i < size; i++) {
        _yPlotData.push(parseFloat(_listAteTimeDate[i].MeanTime));
        _xCategories.push(FormartWorkDate(_listAteTimeDate[i].WorkDate));
    }
    //
    var _avg = _yPlotData.reduce((a, b) => a + b) / _yPlotData.length;
    _avg = _avg.toFixed(2);
    var _dangerSpec = parseFloat(_avg) + 10;
    //
    return {
        YPlotData: _yPlotData,
        XCategories: _xCategories,
        Avg: _avg,
        DangerSpec: _dangerSpec,
    }
};

function FormartWorkDate(WorkDate) {
    return WorkDate.substr(6, 2) + '/' + WorkDate.substr(4, 2) + '/' + WorkDate.substr(0, 4);
}