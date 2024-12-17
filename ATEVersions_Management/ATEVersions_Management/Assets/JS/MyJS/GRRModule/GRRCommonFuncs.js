// ====== AJAX URL Getters ======
var URL_GETListStation = $('#URL_GETListStation').val();
var URL_GETListItemInStation = $('#URL_GETListItemInStation').val();
var URL_GETOperSampleFromCPKTable = $('#URL_GETOperSampleFromCPKTable').val();
var URL_POSTGrrCalculateResult = $('#URL_POSTGrrCalculateResult').val();
// ====== Event Functions ======
$(window).ready(function () {
    ClearListDataObject();
    LoadTimeSpan();
});
$('#cboGageModel').on('change', function () {
    var _model = $('#cboGageModel').val();

    $.ajax({
        url: URL_GETListStation,
        data: { model: _model },
        success: function (listStation) {
            $('#cboGageName option:gt(0)').remove();
            $.each(listStation, function (idx = 0) {

                $('#cboGageName').append($('<option />').val(listStation[idx]).text(listStation[idx]));
                idx++;
            });
        },
        error: function () {
            console.log('Error on calling function!');
        }
    });

});
$('#cboGageName').on('change', function () {
    $('#txtPartName').val('');
    $('#txtSpecification').val('');
    var _model = $('#cboGageModel').val();
    var _station = $('#cboGageName').val();

    ClearListDataObject();
    ClearDisplayedGRRData();

    $('#txtGageNo').val(_station);

    $.ajax({
        url: URL_GETListItemInStation,
        data: { model: _model, station: _station },
        success: function (listItem) {
            var _contentGroup = listItem.GRRContentGroup;
            window.localStorage.setItem('contentGroup', JSON.stringify(_contentGroup));

            $('#dtlGRRParts option:gt(0)').remove();
            $.each(_contentGroup, function (idx = 0) {

                var dtlOptionHTML = '<option value="' + _contentGroup[idx].ItemName + '" data-id="' + idx + '"></option>';
                $('#dtlGRRParts').append(dtlOptionHTML);
                idx++;
            });
        },
        error: function (ex) {
            console.log('Error on calling function: ' + ex);
        }
    });

});
$('#txtPartName').on('change', function () {

    var _contentGroup = JSON.parse(window.localStorage.getItem('contentGroup'));

    var _valPartName = $('#txtPartName').val();
    var _idx = $('#dtlGRRParts option[value="' + _valPartName + '"]').attr('data-id');
    var _partContent = _contentGroup[_idx];

    var _valSpec = '(' + _partContent.SpecL + ',' + _partContent.SpecH + ')';
    $('#txtSpecification').val(_valSpec);
});

function GetSamplesToOperTable(_operNo,_tbodyOperID,_txtOperName) {

    var _valPartName = $('#txtPartName').val();
    var _idx = $('#dtlGRRParts option[value="' + _valPartName + '"]').attr('data-id');

    var _model = $('#cboGageModel').val();
    var _station = $('#cboGageName').val();
    var _operName = $('#' + _txtOperName).val();

    var timeVal = $('#timeSpan').val();
    var timeValSplit = timeVal.split('-');
    var _fromDay = moment(new Date(timeValSplit[0].trim())).format('YYYY-MM-DDTHH:mm');
    var _toDay = moment(new Date(timeValSplit[1].trim())).format('YYYY-MM-DDTHH:mm');

    /*var _fromDay = $('#fromDay').val();
    var _toDay = $('#toDay').val();*/

    var _contentGroup = JSON.parse(window.localStorage.getItem('contentGroup'));
    var _partContent = _contentGroup[_idx];

    var _lsl = parseFloat(_partContent.SpecL), _usl = parseFloat(_partContent.SpecH);
    //console.log(_lsl + '|' + _usl);

    if (_operName.length === 0 || _operName === undefined) {
        var errorHTML = '<tr align="center">' +
            '<td colspan="4">' +
            '<h6 class="text-danger">Must input operator\' name first!</h6>' +
            '</td>'
        '</tr>';
        $('#' + _tbodyOperID).html(errorHTML);
        return;
    }

    var _listPosMark = JSON.parse(window.sessionStorage.getItem('_listPosMark'));
    if (_listPosMark == null) {
        _listPosMark = [];
    }    
    AjaxGetSampleFromCPKTable(_operNo, _operName, _tbodyOperID, _model, _station, _idx, _lsl, _usl, _listPosMark, _fromDay, _toDay);
    
    //$('#btnGetGRRResult').click();
        
}
function GetGRRResult() {

    if (!IsListDataObjectEmpty()) {
        var _listObject = JSON.parse(window.localStorage.getItem('listObject'));
        AjaxGetGRRCalculateResult(_listObject);
    }
    $('#btnGRResultModal').click();

};
function GetGRRResultAtDetail(_JSONDataObject) {
    var _listObject = JSON.parse(_JSONDataObject);
    //console.log(_listObject);
    AjaxGetGRRCalculateResult(_listObject);
};

// ====== AJAX Requesting Functions ======
async function AjaxGetSampleFromCPKTable(_operNo, _operName, _tbodyOperID, _model, _station, _pos, _lsl, _usl, _listPosMark, _fromDay, _toDay) {
    try {
       
        var divLoaderHTML = '<tr align="center">' +
            '<td colspan="4">' +
            '<img src="/Images/loaderGifs/Spinner-3.gif" />' +
            '<h4 class="text-black">Loading...</h4>' +
            '</td>'
        '</tr>';
        $('#' + _tbodyOperID).html(divLoaderHTML);

        console.log(_listPosMark);

        await $.ajax({
            url: URL_GETOperSampleFromCPKTable,
            type: "POST",
            data: { operNo: _operNo, operName: _operName, model: _model, station: _station, pos: _pos, lsl: _lsl, usl: _usl, listPosMark: _listPosMark, fromDay: _fromDay, toDay: _toDay },
            success: function (GRROperSample) {

                console.log(GRROperSample);
                //console.log(GRROperSample.OperSamplesResult);

                // Save OperSample posMark
                
                _listPosMark = GRROperSample.ListPosMark;
                
                window.sessionStorage.setItem('_listPosMark', JSON.stringify(_listPosMark));

                // Append Oper's Sample to table
                OperSamplesResult = GRROperSample.OperSamplesResult;
                $('#' + _tbodyOperID).html('');
                
                ProcessListOperSamples(OperSamplesResult);
                var _operSamples = OperSamplesResult.OperSamples;
                var _sampleRanges = OperSamplesResult.OperSampleRanges;
                var _sampleAVG = OperSamplesResult.OperSampleAVG;

                var sampleSize = _operSamples.length;

                for (var i = 0; i < sampleSize; i++) {
                    var _childSample = _operSamples[i];
                    var trSamplesHTML = '<tr>';                    
                    for (var j = 0; j < _childSample.length; j++) {
                                                
                        trSamplesHTML += '<td>' + _childSample[j] + '</td>';
                    }                    
                    trSamplesHTML += '<td>' + _sampleRanges[i] + '</td>' +
                                     '<td>' + _sampleAVG[i] + '</td>';
                    trSamplesHTML += '</tr>';
                    $('#' + _tbodyOperID).append(trSamplesHTML);
                }

            },
            error: function (error) {
                var errorHTML = '<tr align="center">' +
                    '<td colspan="4">' +
                    '<h6 class="text-danger">Couldn\'t get samples!</h6>' +
                    '</td>'
                '</tr>';
                $('#' + _tbodyOperID).html(errorHTML);
                console.log('Error on calling function: ' + error);
            }
        });
    }
    catch (e) {
        console.log({ e: e });
    }

}
async function AjaxGetGRRCalculateResult(_dataObject) {
    //console.clear();
    try {
        await $.ajax({
            url: URL_POSTGrrCalculateResult,
            type:"POST",
            data: { dataObject: _dataObject },
            success: function (GRRCalResult) {
                //console.log(GRRCalResult);
                //GRRAVGRange_ResultDisplay(GRRCalResult);
                GRRANOVA_ResultDisplay(GRRCalResult);
            },
            error: function (error) {
                console.log('Error on calling function: \n' + error);
            },
        });
    }
    catch (e) {
        console.log({ e: e });
    }
};

// ====== Support Functions ======
// == GRR Range Average Method ==
function GRRAVGRange_ResultDisplay(_GRRCalResult) {
    //GET XbarP
    var _XbarP = _GRRCalResult.XbarP;
    AppendXbarP(_XbarP);
    //GET XbarA, XbarB, XbarC
    var _XbarABC = _GRRCalResult.XbarABC;
    //console.log(_listAVGEachSamples);
    //GET RbarA, RbarB, RbarC
    var _RbarABC = _GRRCalResult.RbarABC;
    //console.log(_listAVGEachRange);
    AppendXbarRbarABC(_XbarABC, _RbarABC);
    //GET XbarDiff
    var _Xdiff = _GRRCalResult.Xdiff;
    //console.log(_diffAVGEachSamples);
    //GET R2bar
    var _R2bar = _GRRCalResult.R2bar;
    //console.log(_AVGEachRange);
    //GET Rp
    var _Rp = _GRRCalResult.Rp;
    //console.log(_rangeAVGAllSamples);
    AppendXdiffR2barRp(_Xdiff, _R2bar, _Rp);
    //GET GRR variations
    AppendGRRRangeAVGVariation(_GRRCalResult);
    // Draw chart pressentation
    DrawChartOPInteraction(_GRRCalResult.ListOperSamples);
    DrawChartVariationComponent(_GRRCalResult.PercentEV, _GRRCalResult.PercentAV, _GRRCalResult.PercentGRR, _GRRCalResult.PercentPV, _GRRCalResult.PercentTV);
};
function AppendXbarP(_XbarP) {

    var tbodyAVGSammples = $('#tbodyAVGSamples');
    tbodyAVGSammples.html('');

    var _dataSize = _XbarP.length;
    for (var i = 0; i < _dataSize; i++) {
        var trAVGDataHTML = '<tr><td>' + _XbarP[i] + '</tr></td>';
        tbodyAVGSammples.append(trAVGDataHTML);
    }
};
function AppendXbarRbarABC(_XbarABC, _RbarABC) {
    var arrSize = _XbarABC.length;

    for (var i = 0; i < arrSize; i++) {
        var posMark = GetPosCode(i);
        var tdDataID = i + '_SampleExt';
        var tdDataHTML = '<label>' +
            '<span class=" fw-bold">X&#772;' + posMark + ': </span>' + _XbarABC[i]+
            '</label>' + '<br/>';
        tdDataHTML += '<label>' +
            '<span class=" fw-bold">R&#772;' + posMark + ': </span>' + _RbarABC[i] +
            '</label>';
        $('#' + tdDataID).html(tdDataHTML);
    }
};
function GetPosCode(_pos) {
    switch (_pos) {
        case 0: return 'a';
        case 1: return 'b';
        case 2: return 'c';
    }
}
function AppendXdiffR2barRp(_Xdiff, _R2bar, _Rp) {
    //AVG each difference
    var tdDataHTML ='<br/><label>' +
        '<span class=" fw-bold">X&#772;diff: </span>' + _Xdiff +
        '</label>';
    $('#0_SampleExt').append(tdDataHTML);
    //AVG each range
    tdDataHTML = '<br/><label>' +
        '<span class=" fw-bold">R&#772;&#772;: </span>' + _R2bar +
        '</label>';
    $('#1_SampleExt').append(tdDataHTML);
    //Range AVG of each range
    tdDataHTML = '<br/><label>' +
        '<span class=" fw-bold">Rp: </span>' + _Rp +
        '</label>';
    $('#2_SampleExt').append(tdDataHTML);
};
function AppendGRRRangeAVGVariation(_GRRVariationResult) {
    //Modal Title GRR Part Name
    $('#modalTilGRRPartName').html($('#txtPartName').val());

    //EV
    var tdDataEV_HTML = '<span class="fw-bold">EV: </span>' + _GRRVariationResult.EV;
    $('#tdDataEV').html(tdDataEV_HTML);
    //AV
    var tdDataAV_HTML = '<span class="fw-bold">AV: </span>' + _GRRVariationResult.AV;
    $('#tdDataAV').html(tdDataAV_HTML);
    //RR
    var tdDataRR_HTML = '<span class="fw-bold">RR: </span>' + _GRRVariationResult.GRR;
    $('#tdDataRR').html(tdDataRR_HTML);
    //PV
    var tdDataPV_HTML = '<span class="fw-bold">PV: </span>' + _GRRVariationResult.PV;
    $('#tdDataPV').html(tdDataPV_HTML);
    //TV
    var tdDataTV_HTML = '<span class="fw-bold">TV: </span>' + _GRRVariationResult.TV;
    $('#tdDataTV').html(tdDataTV_HTML);
    //NDC
    var tdDataNDC_HTML = '<span class="fw-bold">NDC: </span>' + _GRRVariationResult.NDC;
    $('#tdDataNDC').html(tdDataNDC_HTML);
    //Percent EV
    var tdPercentEV_HTML = '<span class="fw-bold">%EV: </span>' + _GRRVariationResult.PercentEV + '%';
    $('#tdPercentEV').html(tdPercentEV_HTML);
    //Percent AV
    var tdPercentAV_HTML = '<span class="fw-bold">%AV: </span>' + _GRRVariationResult.PercentAV + '%';
    $('#tdPercentAV').html(tdPercentAV_HTML);
    //Percent RR
    var tdPercentRR_HTML = '<span class="fw-bold">%RR: </span>' + _GRRVariationResult.PercentGRR + '%';
    $('#tdPercentRR').html(tdPercentRR_HTML);
    //Percent PV
    var tdPercentPV_HTML = '<span class="fw-bold">%PV: </span>' + _GRRVariationResult.PercentPV + '%';
    $('#tdPercentPV').html(tdPercentPV_HTML);

};

// == GRR ANOVA Method ==
function GRRANOVA_ResultDisplay(_GRRCalResult) {
    
    //GET GRR variations
    AppendGRRRangeAVGVariation(_GRRCalResult);
    // Draw chart pressentation
    DrawChartOPInteraction(_GRRCalResult.ListOperSamples);
    DrawChartVariationComponent(_GRRCalResult.PercentEV, _GRRCalResult.PercentAV, _GRRCalResult.PercentGRR, _GRRCalResult.PercentPV, _GRRCalResult.PercentTV);
};
function AppendGRRANOVAVariation(_GRRCalResult) {

}
// == Temporarily Saved Data Function == 
function ClearDisplayedGRRData() {
    //tbodyAVGSammples
    var tbodyAVGSammples = $('#tbodyAVGSamples');
    tbodyAVGSammples.html('');
    //Oper A
    $('#0_SampleExt').html('');
    //Oper B
    $('#1_SampleExt').html('');
    //Oper C
    $('#2_SampleExt').html('');

};
function ProcessListOperSamples(_dataObject) {
    var _listObject = JSON.parse(window.localStorage.getItem('listObject'));

    var _dataID = _dataObject.ID;
    _listObject[_dataID] = _dataObject;
    var stringifyListObject = JSON.stringify(_listObject);
    window.localStorage.setItem('listObject', stringifyListObject);
    $('#json_OperTestResult').val(stringifyListObject);
    //console.log($('#json_OperTestResult').val());
};
function ClearListDataObject() {
    var _clearListObject = [];
    window.localStorage.setItem('listObject', JSON.stringify(_clearListObject));
    window.sessionStorage.setItem('_listPosMark', JSON.stringify(_clearListObject));
};
function IsListDataObjectEmpty() {
    var _listObject = JSON.parse(window.localStorage.getItem('listObject'));

    if (typeof _listObject === 'undefined' || _listObject.length < 3) return true;

    for (var i = 0; i < _listObject.length; i++) {
        if (typeof _listObject[i] === 'undefined' || _listObject[i] == null) return true;
    }

    return false;
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