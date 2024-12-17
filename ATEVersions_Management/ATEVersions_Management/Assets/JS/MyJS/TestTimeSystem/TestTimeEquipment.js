//=== Global Variable ===

//=== Event Functions ===
$(document).ready(function () {
    LoadTimeSpan();
    //GET_TETestData();          
});
$('#timeSpan').on('change', function () {
    var timeVal = $('#timeSpan').val();
    var timeValSplit = timeVal.split('-');
    var _fromDate = moment(new Date(timeValSplit[0].trim())).format('YYYY-MM-DDTHH:mm');
    var _toDate = moment(new Date(timeValSplit[1].trim())).format('YYYY-MM-DDTHH:mm');
    //console.log(_fromDate + ' , ' + _ toDate);
    AJAX_GET_TestDataOfStations(_fromDate, _toDate);

});
function GET_TETestData() {
    /*var _fromDate = $('#fromDay').val();
    var _toDate = $('#toDay').val();*/

    var timeVal = $('#timeSpan').val();
    var timeValSplit = timeVal.split('-');
    var _fromDate = moment(new Date(timeValSplit[0].trim())).format('YYYY-MM-DDTHH:mm');
    var _toDate = moment(new Date(timeValSplit[1].trim())).format('YYYY-MM-DDTHH:mm');

    AJAX_GET_TestDataOfStations(_fromDate, _toDate);    
};
//=== AJAX Requesting Functions ===
async function AJAX_GET_TestDataOfStations(_fromDate, _toDate) {
    try {
        $('#tabContentContainer').html('');
        $loaderDiv.show();
        await $.ajax({
            url: URL_GET_TETestData,
            data: { fromDate: _fromDate, toDate: _toDate },
            success: function (result) {
                $loaderDiv.hide();
                _listModelStation = result;

                window.localStorage.setItem('listModelStation', JSON.stringify(result));
                $.each(_listLine, function (lineIdx = 0) {
                    var divTabLineID = 'divTab' + _listLine[lineIdx];
                    var tableLineID = 'table' + _listLine[lineIdx];
                    var tbodyLineID = 'tbody' + _listLine[lineIdx];
                    var fnameTitle = 'Equipment Required of ' + _listLine[lineIdx];

                    var tabContentHTML = '<div class="tabcontent p-1" id="' + divTabLineID + '">' +
                        '<table class="table-custom table- text-white" id="' + tableLineID + '">' +
                        '<thead>' +
                        '<tr class="fw-bold text-white">' +
                        '<th class="fii-frame-rectangle">Model</th>' +
                        '<th class="fii-frame-rectangle">Station</th>' +
                        '<th class="fii-frame-rectangle">PCS</th>' +
                        '<th class="fii-frame-rectangle">Target Output</th>' +
                        '<th class="fii-frame-rectangle">Hours/Day</th>' +
                        '<th class="fii-frame-rectangle">Load Time</th>' +
                        '<th class="fii-frame-rectangle">Unit Test Time</th>' +
                        '<th class="fii-frame-rectangle">Equip Existed</th>' +
                        '<th class="fii-frame-rectangle">Equip Required</th>' +
                        '<th class="fii-frame-rectangle">Equip Gap</th>' +
                        '<th class="fii-frame-rectangle">Existed Output</th>' +
                        '</tr>' +
                        '</thead>' +
                        '<tbody id = "' + tbodyLineID + '">' +

                        '</tbody>' +
                        '</table>' +
                        '</div>';
                    $('#tabContentContainer').append(tabContentHTML);

                    $.each(_listModelStation, function (idx = 0) {
                        if (_listLine[lineIdx] == _listModelStation[idx].Line) {
                            var _dataID = _listModelStation[idx].DataID;
                            var _inpTargetOutputID = _dataID + '_inpTargetOutputID';
                            var _inpRunHoursID = _dataID + '_inpRunHoursID';
                            var _inpLoadTimeID = _dataID + '_inpLoadTimeID';
                            var _inpEquipExistedID = _dataID + '_inpEquipExistedID';
                            var _inpUnitTimeID = _dataID + '_inpUnitTimeID';
                            var tbodyDataHTML = '<tr id="' + _dataID + '">' +
                                '<td class="fii-frame-box fw-bold">' + _listModelStation[idx].Model + '</td>' +
                                '<td class="fii-frame-box fw-bold">' + _listModelStation[idx].Station + '</td>' +
                                '<td class="fii-frame-box fw-bold">' + _listModelStation[idx].PcsTotal + '</td>' +
                                '<td class="fii-frame-box">' + '<input type="number" class="w-100 fw-bold" id="' + _inpTargetOutputID + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                                '<td class="fii-frame-box">' + '<input type="number" class="w-100 fw-bold" id="' + _inpRunHoursID + '" value="' + _listModelStation[idx].RunHours + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                                '<td class="fii-frame-box">' + '<input type="number" class="w-100 fw-bold" id="' + _inpLoadTimeID + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                                '<td class="fii-frame-box">' + '<input type="number" class="w-100 fw-bold text-center" id="' + _inpUnitTimeID + '" value="' + _listModelStation[idx].UnitTestTime + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                                '<td class="fii-frame-box">' + '<input type="number" class="w-100 fw-bold" id="' + _inpEquipExistedID + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                                '<td class="fii-frame-box fw-bold">' + _listModelStation[idx].EquipRequired + '</td>' +
                                '<td class="fii-frame-box fw-bold">' + _listModelStation[idx].EquipGap + '</td>' +
                                '<td class="fii-frame-box fw-bold">' + _listModelStation[idx].TheorenticalExistedOutput + '</td>' +
                                '</tr>';
                            $('#' + tbodyLineID).append(tbodyDataHTML);
                        }
                        idx++;

                    });

                    dataTableExportable(tableLineID, fnameTitle);

                    lineIdx++;
                });
                var tablinks = $('.tabLinks');
                tablinks[0].click();
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            }
        });
    } catch (e) {
        console.log('JS caught exception: ' + { e: e });
    }
};
async function EquipOutputCalculate(_dataID) {
    var _listModelStation = JSON.parse(window.localStorage.getItem('listModelStation'));
    var _line = _listModelStation[_dataID].Line;
    var _station = _listModelStation[_dataID].Station;
    var _model = _listModelStation[_dataID].Model;
    var _pcsTotal = _listModelStation[_dataID].PcsTotal;
    var _fromDate = new Date(parseInt(_listModelStation[_dataID].FromDate.substr(6)));
    var _toDate = new Date(parseInt(_listModelStation[_dataID].ToDate.substr(6)));

    var tableLineID = 'table' + _line;
    var tbodyLineID = 'tbody' + _line;
    var fnameTitle = 'Equipment Required of ' + _line;

    var _inpTargetOutputID = _dataID + '_inpTargetOutputID';
    var _inpRunHoursID = _dataID + '_inpRunHoursID';
    var _inpLoadTimeID = _dataID + '_inpLoadTimeID';
    var _inpEquipExistedID = _dataID + '_inpEquipExistedID';
    var _inpUnitTimeID = _dataID + '_inpUnitTimeID';

    var _targetOutputVal = parseFloat($('#' + _inpTargetOutputID).val());
    var _runHoursVal = parseFloat($('#' + _inpRunHoursID).val());
    var _loadTimeVal = parseFloat($('#' + _inpLoadTimeID).val());
    var _equipExistedputVal = parseFloat($('#' + _inpEquipExistedID).val());
    var _unitTimeIDVal = parseFloat($('#' + _inpUnitTimeID).val());    

    if (Number.isNaN(_unitTimeIDVal) || _unitTimeIDVal == 0) {
        $('#' + _inpUnitTimeID).val(_listModelStation[_dataID].UnitTestTime);
    }
    if (Number.isNaN(_runHoursVal) || _runHoursVal == 0) {
        $('#' + _inpRunHoursID).val(_listModelStation[_dataID].RunHours);
    }
    
    if (!(Number.isNaN(_targetOutputVal) ||
        Number.isNaN(_runHoursVal) ||
        Number.isNaN(_loadTimeVal) ||
        Number.isNaN(_unitTimeIDVal) ||
        Number.isNaN(_equipExistedputVal)
    )) {
        try {
            await $.ajax({
                url: URL_POST_ReplaceTETestData,
                type: 'POST',
                data: { dataId: _dataID, line: _line, model: _model, station: _station, unitTestTime: _unitTimeIDVal, loadTime: _loadTimeVal, dailyTargetOutput: _targetOutputVal, equipExisted: _equipExistedputVal, runHours: _runHoursVal, pcsTotal: _pcsTotal },
                success: function (replaceData) {

                    var trIdContainer = $('#' + _dataID);
                    var trIdHTML = '<td class="fii-frame-box fw-bold">' + replaceData.Model + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + replaceData.Station + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + replaceData.PcsTotal + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + '<input type="number" class="w-100 fw-bold" id="' + _inpTargetOutputID + '" value="' + replaceData.DailyTargetOutput + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + '<input type="number" class="w-100 fw-bold" id="' + _inpRunHoursID + '" value="' + replaceData.RunHours + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + '<input type="number" class="w-100 fw-bold" id="' + _inpLoadTimeID + '" value="' + replaceData.LoadTime + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + '<input type="number" class="w-100 fw-bold text-center" id="' + _inpUnitTimeID + '" value="' + replaceData.UnitTestTime + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + '<input type="number" class="w-100 fw-bold" id="' + _inpEquipExistedID + '" value="' + replaceData.EquipExisted + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + replaceData.EquipRequired + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + replaceData.EquipGap + '</td>' +
                        '<td class="fii-frame-box fw-bold">' + replaceData.TheorenticalExistedOutput + '</td>';
                    trIdContainer.html(trIdHTML);

                    //ReInitialize DataTable
                    ReIniTestTimeEquipTable(tableLineID, tbodyLineID, fnameTitle);
                },
                error: function (error) {
                    console.log('Error on calling function: '+error);
                }
            });
        } catch (e) {
            console.log('Error: ' + { e: e });
        }
    }
};
async function AjaxPostOutputCalculate(_dataID, _line, _model, _station, _unitTimeIDVal, _loadTimeVal, _targetOutputVal, _equipExistedputVal, _runHoursVal, _pcsTotal) {
    try {
        await $.ajax({
            url: URL_POST_ReplaceTETestData,
            type: 'POST',
            data: { dataId: _dataID, line: _line, model: _model, station: _station, unitTestTime: _unitTimeIDVal, loadTime: _loadTimeVal, dailyTargetOutput: _targetOutputVal, equipExisted: _equipExistedputVal, runHours: _runHoursVal, pcsTotal: _pcsTotal },
            success: function (replaceData) {

                var trIdContainer = $('#' + _dataID);
                var trIdHTML = '<td class="fii-frame-box fw-bold">' + replaceData.Model + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + replaceData.Station + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + replaceData.PcsTotal + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + '<input type="number" class="inputSize" id="' + _inpTargetOutputID + '" value="' + replaceData.DailyTargetOutput + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + '<input type="number" class="inputSize" id="' + _inpRunHoursID + '" value="' + replaceData.RunHours + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + '<input type="number" class="inputSize" id="' + _inpLoadTimeID + '" value="' + replaceData.LoadTime + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + '<input type="number" class="inputSize" id="' + _inpUnitTimeID + '" value="' + replaceData.UnitTestTime + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + '<input type="number" class="inputSize" id="' + _inpEquipExistedID + '" value="' + replaceData.EquipExisted + '" onchange="EquipOutputCalculate(' + _dataID + ')" />' + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + replaceData.EquipRequired + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + replaceData.EquipGap + '</td>' +
                    '<td class="fii-frame-box fw-bold">' + replaceData.TheorenticalExistedOutput + '</td>';
                trIdContainer.html(trIdHTML);

                //ReInitialize DataTable
                ReIniTestTimeEquipTable(tableLineID, tbodyLineID, fnameTitle);
            },
            error: function () {
                console.log('error');
            }
        });
    } catch (e) {
        console.log('Error: ' + { e: e });
    }
};
function appendNewProductEstimate() {
    var productEstimateContainer = $('#containerNewProductEstimate');
    if (productEstimateContainer.hasClass('onUsage')) {
        
        var btnNewEstimateSectionHTML = '<div id="buttonsNewProductEstimate" class="mb-2 d-flex">' +
            '<button class="btn btn-outline-info" onclick="appendNewProductEstimate()">New Product Estimate</button>' +
            '</div>';
        productEstimateContainer.html(btnNewEstimateSectionHTML);
        productEstimateContainer.removeClass('onUsage');
    }

    var btnNewRowHTML = '<button class="btn btn-outline-info" onclick="appendEstimateRow()">' +
        '<i class="fas fa-plus" ></i>' +
        '<span> New Row</span>' +
        '</button>';

    $('#buttonsNewProductEstimate').append(btnNewRowHTML);
    productEstimateContainer.addClass('onUsage');

    var rowEstimateCount = 0;
    window.localStorage.setItem('rowEstimateCount', rowEstimateCount);
    var rowEstimateID = rowEstimateCount + '_rowEstimateID';

    var _inpEstModelID = rowEstimateCount + '_inpEstModelID';
    var _inpEstStationID = rowEstimateCount + '_inpEstStationID';
    var _inpEstTargetID = rowEstimateCount + '_inpEstTargetID';
    var _inpEstHoursID = rowEstimateCount + '_inpEstHoursID';
    var _inpEstLoadTimeID = rowEstimateCount + '_inpEstLoadTimeID';
    var _inpEstEquipExistedID = rowEstimateCount + '_inpEstEquipExistedID';
    var _inpEstUnitTimeID = rowEstimateCount + '_inpEstUnitTimeID';
    var tableID = 'dtblTestTimeEstID';
    var TETestEstimateHTML = '<div class="table-responsive border-left-blue border-bottom-blue p-1" id="tblTETestEstimate">' +
        '<table class="table table-bordered text-black" id="' + tableID +'">' +
        '<thead>' +
        '<tr class="bg-blue-opa75 text-white">' +
        '<th>Model</th>' +
        '<th>Station</th>' +
        '<th>Target Output</th>' +
        '<th>Hours/Day</th>' +
        '<th>Load Time</th>' +
        '<th>Unit Test Time</th>' +
        '<th>Equip Existed</th>' +
        '<th>Equip Required</th>' +
        '<th>Equip Gap</th>' +
        '<th>Existed Output</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody id="tbdTestTimeEstID">' +
        '<tr id="' + rowEstimateID + '" class="fw-bold">' +
        '<td class="w-10"><input type="text" class="w-100 fw-bold" id="' + _inpEstModelID + '"/></td>' +
        '<td class="w-10"><input type="text" class="w-100 fw-bold" id="' + _inpEstStationID + '"/></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstTargetID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstHoursID + '" value="18" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstLoadTimeID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstUnitTimeID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstEquipExistedID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td>' + 0 + '</td>' +
        '<td>' + 0 + '</td>' +
        '<td>' + 0 + '</td>' +
        '</tr>' +
        '</tbody>' +
        '</table>' +
        '</div>';

    $('#containerNewProductEstimate').append(TETestEstimateHTML);
    //Initialize Datatable
    var fnameTitle = 'New Product Estimate';
    dataTableExportable(tableID, fnameTitle);
};
function EquipOutputEstimate(rowEstimateCount) {
    var tableID = 'dtblTestTimeEstID';
    var tbodyEstimateID = $('#tbdTestTimeEstID');
    var rowEstimateID = rowEstimateCount + '_rowEstimateID';

    var _inpEstModelID = rowEstimateCount + '_inpEstModelID';
    var _inpEstStationID = rowEstimateCount + '_inpEstStationID';
    var _inpEstTargetID = rowEstimateCount + '_inpEstTargetID';
    var _inpEstHoursID = rowEstimateCount + '_inpEstHoursID';
    var _inpEstLoadTimeID = rowEstimateCount + '_inpEstLoadTimeID';
    var _inpEstEquipExistedID = rowEstimateCount + '_inpEstEquipExistedID';
    var _inpEstUnitTimeID = rowEstimateCount + '_inpEstUnitTimeID';

    var _EstModelVal = $('#' + _inpEstModelID).val();
    var _EstStationVal = $('#' + _inpEstStationID).val();
    var _EstTargetOutputVal = parseFloat($('#' + _inpEstTargetID).val());
    var _EstRunHoursVal = parseFloat($('#' + _inpEstHoursID).val());
    var _EstLoadTimeVal = parseFloat($('#' + _inpEstLoadTimeID).val());
    var _EstEquipExistedVal = parseFloat($('#' + _inpEstEquipExistedID).val());
    var _EstUnitTimeIDVal = parseFloat($('#' + _inpEstUnitTimeID).val());

    //console.log(_EstModelVal + '|' + _EstStationVal + '|' + _EstTargetOutputVal + '|' + _EstRunHoursVal + '|' + _EstLoadTimeVal + '|' + _EstUnitTimeIDVal + '|' + _EstEquipExistedVal + '|');

    if (Number.isNaN(_EstRunHoursVal) || _EstRunHoursVal == 0) {
        $('#' + _EstRunHoursVal).val(18);
    }

    if (!(Number.isNaN(_EstTargetOutputVal) ||
        Number.isNaN(_EstRunHoursVal) ||
        Number.isNaN(_EstLoadTimeVal) ||
        Number.isNaN(_EstEquipExistedVal) ||
        Number.isNaN(_EstUnitTimeIDVal)
    )) {
        $.ajax({
            url: URL_POST_ReplaceTETestData,
            type: 'POST',
            data: { dataId: rowEstimateCount, line: '', model: _EstModelVal, station: _EstStationVal, unitTestTime: _EstUnitTimeIDVal, loadTime: _EstLoadTimeVal, dailyTargetOutput: _EstTargetOutputVal, equipExisted: _EstEquipExistedVal, runHours: _EstRunHoursVal, pcsTotal: 0 },
            success: function (replaceData) {

                var trIdContainer = $('#' + rowEstimateID);
                var trIdHTML = '<td class="w-10"><input type="text" class="w-100 fw-bold" id="' + _inpEstModelID + '" value="' + replaceData.Model + '" /></td>' +
                    '<td class="w-10"><input type="text" class="w-100 fw-bold" id="' + _inpEstStationID + '" value="' + replaceData.Station + '" /></td>' +
                    '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstTargetID + '" value="' + replaceData.DailyTargetOutput + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
                    '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstHoursID + '" value="' + replaceData.RunHours + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
                    '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstLoadTimeID + '" value="' + replaceData.LoadTime + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
                    '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstUnitTimeID + '" value="' + replaceData.UnitTestTime + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
                    '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstEquipExistedID + '" value="' + replaceData.EquipExisted + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
                    '<td>' + replaceData.EquipRequired + '</td>' +
                    '<td>' + replaceData.EquipGap + '</td>' +
                    '<td>' + replaceData.TheorenticalExistedOutput + '</td>';
                trIdContainer.html(trIdHTML);

                //ReInitialize Datatable
                var tmpTbodyEstimateHTML = tbodyEstimateID.html();
                var fnameTitle = 'New Product Estimate';
                detroyDatatable(tableID);
                tbodyEstimateID.append(tmpTbodyEstimateHTML);
                dataTableExportable(tableID, fnameTitle);
            },
            error: function () {
                console.log('error');
            }
        });
    }

};
//=== Support Functions ===
function tabSelected(evt, line) {
    var divTabLineID = 'divTab' + line;
    $('.tabLinks').removeClass('active');
    $('.tabcontent').css('display', 'none');
    $('#' + divTabLineID).css('display', 'block');
    evt.currentTarget.className += ' active';
};
// Append new estimate row in datatable
function appendEstimateRow() {
    var tableID = 'dtblTestTimeEstID';
    var tbodyEstimateID = $('#tbdTestTimeEstID');

    var rowEstimateCount = parseInt(window.localStorage.getItem('rowEstimateCount', rowEstimateCount));
    rowEstimateCount += 1;
    window.localStorage.setItem('rowEstimateCount', rowEstimateCount);
    var rowEstimateID = rowEstimateCount + '_rowEstimateID';

    var _inpEstModelID = rowEstimateCount + '_inpEstModelID';
    var _inpEstStationID = rowEstimateCount + '_inpEstStationID';
    var _inpEstTargetID = rowEstimateCount + '_inpEstTargetID';
    var _inpEstHoursID = rowEstimateCount + '_inpEstHoursID';
    var _inpEstLoadTimeID = rowEstimateCount + '_inpEstLoadTimeID';
    var _inpEstEquipExistedID = rowEstimateCount + '_inpEstEquipExistedID';
    var _inpEstUnitTimeID = rowEstimateCount + '_inpEstUnitTimeID';

    var rowEstimateHTML = '<tr id="' + rowEstimateID + '" class="fw-bold">' +
        '<td class="w-10"><input type="text" class="w-100 fw-bold" id="' + _inpEstModelID + '"/></td>' +
        '<td class="w-10"><input type="text" class="w-100 fw-bold" id="' + _inpEstStationID + '"/></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstTargetID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstHoursID + '" value="18" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstLoadTimeID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstUnitTimeID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td class="w-10"><input type="number" class="w-100 fw-bold" id="' + _inpEstEquipExistedID + '" onchange="EquipOutputEstimate(' + rowEstimateCount + ')" /></td>' +
        '<td>' + 0 + '</td>' +
        '<td>' + 0 + '</td>' +
        '<td>' + 0 + '</td>' +
        '</tr>';
    tbodyEstimateID.append(rowEstimateHTML);

    //ReInitialize Datatable
    var tmpTbodyEstimateHTML = tbodyEstimateID.html();
    var fnameTitle = 'New Product Estimate';
    detroyDatatable(tableID);
    tbodyEstimateID.append(tmpTbodyEstimateHTML);
    dataTableExportable(tableID, fnameTitle);
};
// Re initialize data table
function ReIniTestTimeEquipTable(tableID, tbodyID, fnameTitle) {
    //
    var tbodyEstimateID = $('#' + tbodyID);

    var tmpTbodyEstimateHTML = tbodyEstimateID.html();
    detroyDatatable(tableID);
    tbodyEstimateID.append(tmpTbodyEstimateHTML);
    dataTableExportable(tableID, fnameTitle);
};
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