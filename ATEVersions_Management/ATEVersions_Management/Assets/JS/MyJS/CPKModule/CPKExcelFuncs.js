var excelGlobalData;
$('#cpkExcelUpload').on('submit', function (e) {
    e.preventDefault();
    $('#excelItemList').html('');
    //console.log('ok');
    $.ajax({
        url: this.action,
        method: this.method,
        data: new FormData(this),
        cache: false,
        contentType: false,
        processData: false,
        success: function (excelData) {
            excelGlobalData = excelData;
            //console.log(excelData);
            $.each(excelData, function (idx = 0, data) {
                var specLOf = 'specLOfID_' + idx;
                var specHOf = 'specHOfID_' + idx;
                var trStruct = '<tr>' +
                    '<td class="text-center">' +
                    (parseInt(idx + 1)) +
                    '</td>' +
                    '<td>' +
                    data.ColName +
                    '</td>' +
                    '<td>' +
                    '<input type="number" class="inputSize" id="' + specLOf + '"' + 'size="5"/>' +
                    '</td>' +
                    '<td>' +
                    '<input type="number" class="inputSize" id="' + specHOf + '"' + 'size="5"/>' +
                    '</td>' +
                    '<td class="text-center">' +
                    '<a href="#" class="text-info" title="Draw Item CPK" data-toggle="modal" data-target="#CPKChartModal" onclick="DrawCPKFunc(' + data.ID + ')">' +
                    '<i class="fas fa-chart-line"/>' +
                    '</a>' +
                    '</td>'
                    + '</tr>';
                $('#excelItemList').append(trStruct);
                idx++;
            });
        },
        error: function (response) {
            console.log('error');
        }

    });
});

var DrawCPKFunc = function (id) {
    var specLOf = 'specLOfID_' + id;
    var specHOf = 'specHOfID_' + id;
    var valSpecL = $('#' + specLOf).val();
    var valSpecH = $('#' + specHOf).val();
    var rawValueArray = excelGlobalData[id].RowValues;
    var doubleArray = [];
    for (var i = 0; i < rawValueArray.length; i++) {
        var tmpVal = parseFloat(rawValueArray[i]);
        if (Number.isNaN(tmpVal)) {
            doubleArray.push(0);
            continue;
        }
        doubleArray.push(tmpVal);
    }
    //console.log(doubleArray);
    var btnCopy = '<button type="button" class="mb-2 btn btn-success" id="btnCopyImg" ><i class="fas fa-copy" ></i ><span>Copy</span></button > ';
    var _ItemName = excelGlobalData[id].ColName;
    var _SpecL = valSpecL;
    var _SpecH = valSpecH;
    //console.clear();
    $('#CPKChartSection').html('');
    //console.log('Data of Item: ' + cpkItemContent.ItemName + ' | SpecL: ' + cpkItemContent.SpecL + ' | SpecH: ' + cpkItemContent.SpecH + ' | ' + cpkItemContent.Value);
    //console.log(JSON.stringify(_cpkItemContent));
    $.ajax({
        url: cpkChartSectionUrl,
        method: 'POST',
        data: { ItemName: _ItemName, SpecL: _SpecL, SpecH: _SpecH, Value: doubleArray },
        success: function (result) {
            //console.log(JSON.stringify(excelData));
            $('#CPKChartSection').append(btnCopy);
            $('#CPKChartSection').append(result);
        },
        error: function (response) {
            console.log('error');
        }
    });
};