// === AJAX Funtions URL ===
var URL_POST_MQTTPublishMessage = $('#URL_POST_MQTTPublishMessage').val();
// WOL Module
var URL_GET_TodayAllMachineInfor = $('#URL_GET_TodayAllMachineInfor').val();
var URL_GET_MachineOwner = $('#URL_GET_MachineOwner').val();
var URL_GET_IssueRecordByHostName = $('#URL_GET_IssueRecordByHostName').val();
var URL_GET_MachineChangeRecordByHostName = $('#URL_GET_MachineChangeRecordByHostName').val();
var URL_GET_TodayEnergyTotal = $('#URL_GET_TodayEnergyTotal').val();
var URL_GET_TimeRangeEnergyTotal = $('#URL_GET_TimeRangeEnergyTotal').val();
// AIR Module
var URL_GET_CurrentAirEnableMachine = $('#URL_GET_CurrentAirEnableMachine').val();

// === Global variable ===
var timeCheck = moment().format('YYYY/MM/DD hh:mm:ss a')
var intervalTime = 120 * 1000;
// === Events Triggering Functions===
$(window).on('load', function () {
    PageInitAndInterval();
});
function PageInitAndInterval() {
    //GetPowerSavingRecord();
    AJAX_GET_TodayAllMachineInfor();
    AJAX_GET_CurrentAirEnableMachine();
};
setInterval(PageInitAndInterval, intervalTime);
$('.btn-filter-history').on('click', function () {
    var _timeRange = $('#timeSpan').val();
    AJAX_GET_GET_TimeRangeEnergyTotal(_timeRange);
});
$('#timeSpan').on('change', function () {
    var _timeRange = $('#timeSpan').val();
    AJAX_GET_GET_TimeRangeEnergyTotal(_timeRange);
});
$('#search_hostname').on('keyup', function () {
    var val = this.value.toUpperCase();
    var content_item = document.getElementsByClassName('content-item');
    //console.log(val);
    for (var i = 0; i < content_item.length; i++) {
        if (content_item[i].textContent.indexOf(val) > -1) {
            content_item[i].style.display = 'block';
        } else {
            content_item[i].style.display = 'none';
        }
    }
});
function ReloadOnTabChange(tabTitle) {
    if (tabTitle === 'WOL') {
        //GetPowerSavingRecord();
        AJAX_GET_TodayAllMachineInfor();
    }
    if (tabTitle === 'AIR') {
        AJAX_GET_CurrentAirEnableMachine();
    }
}

// ====== WOL Machine Information Processing ======
// === AJAX Requesting Functions ===
async function AJAX_GET_TodayAllMachineInfor() {
    try {
        await $.ajax({
            url: URL_GET_TodayAllMachineInfor,
            data: {},
            async:false,
            success: function (_listMachine) {
                //console.log(_listMachine);
                window.sessionStorage.setItem('_listMachine', JSON.stringify(_listMachine));
                var _listLineMachine = GetListLineMachineInfor(_listMachine,'WOL');
                var totalEnergySaved = GetIdleTotalEnergySave(_listMachine);
                //console.log(_listLineMachine);
                //console.log(totalEnergySaved);
                GetPowerSavingRecord(totalEnergySaved);
                // Present Data to webview 
                $('.tb-custom').html('');
                
                if (_listLineMachine.length != 0) {                    
                    var totalNormal = 0;
                    var totalAbnormal = 0;
                    var totalWarning = 0;
                    var totalHighRisk = 0;
                    var totalIdle = 0;                    
                    var totalAll = 0;

                    var lineContent = '';

                    $.each(_listLineMachine, function (idx,line) {
                        //console.log(line);
                        var line_name = line.Line;
                        var content_item = '';

                        var totalLineNormal = 0;
                        var totalLineHighRisk = 0;
                        var totalLineIdle = 0;
                        var totalLineAbnormal = 0;
                        var totalLineWarning = 0;

                        var _listMachine = line.ListMachine

                        $.each(_listMachine, function (idx, machine) {
                            var status = '';

                            if (machine.ActiveStatus == 0) {
                                status = 'normal';
                                totalNormal++;
                                totalLineNormal++;
                            }
                            if (machine.ActiveStatus == 1) {
                                status = 'warning';
                                totalLineWarning++;
                                totalWarning++;
                            }
                            if (machine.ActiveStatus == 2) {
                                status = 'idle';
                                totalIdle++;
                                totalLineIdle++;
                            }
                            if (machine.ActiveStatus == 3) {
                                status = 'hrisk';
                                
                                totalHighRisk++;
                                totalLineHighRisk++;
                            }
                            if (machine.ActiveStatus == 4) {
                                status = 'abnormal';                                
                                totalLineAbnormal++;
                                totalAbnormal++;
                            }

                            content_item +=
                                '<div id="' + machine.HOST_NAME + '" class="content-item content-item-' + status + '" status-all="-1" data-toggle="modal" data-target="#modal-detail" stage="' +
                                '" line="' + machine.LINE +
                                '" mac="' + machine.MAC +
                                '" osversion="' + machine.OS_VERSION +
                                '" osname="' + machine.OS_NAME +
                                '" owner="' + '' +
                                '" status="' + machine.ActiveStatus +
                            '" idletime="' + machine.IdleTime +
                                '" idtester="' +
                                '" hostname="' + machine.HOST_NAME +
                                '" ipaddress="' + machine.IP + '">' +

                                '<span class="hostname">' + NullVal(machine.HOST_NAME) + '</span>' +

                                '<div class="tooltip-text-media">' +
                                '<table id="tblHover">' +
                                '<tr><td class="title-hover">Host Name: </td><td>' + NullVal(machine.HOST_NAME) + '</td></tr>' +
                                '<tr><td class="title-hover">IP Address: </td><td>' + NullVal(machine.IP) + '</td></tr>' +
                                '<tr><td class="title-hover">Mac Address: </td><td>' + NullVal(machine.MAC) + '</td></tr>' +
                                '<tr><td class="title-hover">OS Name: </td><td>' + NullVal(machine.OS_NAME) + '</td></tr>' +
                                '<tr><td class="title-hover">OS Version: </td><td>' + NullVal(machine.OS_VERSION) + '</td></tr>' +
                                '<tr><td class="title-hover">Owner: </td><td id="tltOwner' + machine.HOST_NAME + '">' + NullVal() + '</td></tr>' +
                                '<tr><td class="title-hover">Idle Time: </td><td>' + NullVal(machine.IdleTime+'h') + '</td></tr>' +
                                '<tr><td class="title-hover">CPU Temp: </td><td>' + (machine.CPU_TEMP != null ? NullVal(machine.CPU_TEMP) + '&#8451' : NullVal(machine.CPU_TEMP)) + '</td></tr>' +
                                '<tr><td class="title-hover">Ram: </td><td>' + NullVal(machine.RAM) + '</td></tr>' +
                                '<tr><td class="title-hover">CPU Name: </td><td>' + NullVal(machine.CPU) + '</td></tr>' +
                                '<tr><td class="title-hover">Hard Disk: </td><td>' + NullVal(machine.HARD_DRIVE) + '</td></tr>' +
                                '</table>' +
                                '</div>' +
                                '</div>';
                        });
                        
                        lineContent +=
                            '<div class="col-md-12 my-0 all-tester">' +
                            '<div class="row ct-line">' +
                            '<div class="col-md-1 px-0 line-name">' +
                            '<span>LINE: ' + line_name + '</span>' +
                            '<div style="display:inline-block">(' +
                            '<span style="color: #008000;">' + totalLineNormal + '</span> - ' +
                            '<span style="color: #d1a62f;">' + totalLineWarning + '</span> - ' +
                            '<span style="color: #4e6672;">' + totalLineIdle + '</span> - ' +
                            '<span style="color: #e0470e;">' + totalLineHighRisk + '</span> - ' +
                            '<span style="color: #d40707;">' + totalLineAbnormal + '</span>' +
                            ')</div>' +
                        '<button class="btn btn-warning mb-2 btn-line " onclick="postLineAction(\'' + idx + '\',\'' + line_name + '\',\'wakeup\'' + ')"><i class="fa fa-clock"></i>Awake</button>' +
                        '<button class="btn btn-primary mb-2 btn-line " onclick="postLineAction(\'' + idx + '\',\'' + line_name + '\',\'sleep\'' + ')"><i class="fa fa-moon"></i>Sleep</button>' +
                            '</div>' +
                            '<div class="col-md-11 px-0 line-content d-flex align-items-center ">' +
                            content_item +
                            '</div>' +
                            '</div>' +
                            '</div>';
                    });                    
                    

                    var containerLineContent =
                        '<div class="row container-custom">' +                        
                        lineContent +
                        '</div>';

                    $('.tb-custom').append(containerLineContent);
                    $('.total-normal').html(totalNormal);
                    $('.total-warning').html(totalWarning);
                    $('.total-idle').html(totalIdle);
                    $('.total-hrisk').html(totalHighRisk);
                    $('.total-abnormal').html(totalAbnormal);

                    var boxShadowing = 'box-shadow-blink';
                    if (totalNormal > 0) {
                        $('.total-normal').toggleClass('box-shadow-normal');
                    }
                    if (totalWarning > 0) {
                        $('.header__item.warning').toggleClass('box-shadow-warning');
                    }
                    if (totalIdle > 0) {
                        $('.header__item.idle').toggleClass('box-shadow-idol');
                    }
                    if (totalHighRisk > 0) {
                        $('.header__item.hrisk').toggleClass('box-shadow-hrisk');
                    }
                    if (totalAbnormal > 0) {
                        $('.header__item.abnormal').toggleClass('box-shadow-abnormal');
                    }

                    totalAll = totalNormal + totalHighRisk + totalIdle + totalAbnormal + totalWarning;
                    $('.total-all').html(totalAll);

                } else {
                    $('.header__item.all .total-all').html('N/A');
                    $('.header__item.normal .total-normal').html('N/A');
                    $('.header__item.warning .total-warning').html('N/A');
                    $('.header__item.idle .total-idle').html('N/A');
                    $('.header__item.hrisk .total-hrisk').html('N/A');
                    $('.header__item.abnormal .total-abnormal').html('N/A');
                    $('.tb-custom').html('<div class="col-md-12 text-center py-2">--No data to display--</div>');
                }

            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                // Create onclick event for content-item
                $('.content-item').on('click', function () {

                    // Modal data filling

                    var status = $(this).attr('status');
                    var idleTime = $(this).attr('idletime');
                    var idTesterInfo = $(this).attr('idtester');
                    var hostname = $(this).attr('hostname');
                    var ipaddress = $(this).attr('ipaddress');
                    var stage = $(this).attr('stage');
                    var line = $(this).attr('line');
                    var mac = $(this).attr('mac');
                    var osname = $(this).attr('osname');
                    var osversion = $(this).attr('osversion');
                    var owner = $(this).attr('owner');
                    // console.log(stage + ' - ' + line + ' - ' + mac + ' - ' + osname + ' - ' + osversion + ' - ' + owner + ' - ')

                    //$('#modal-detail').modal('show');
                    $('#modal-detail .title-modal').html(ipaddress + ' / ' + hostname);

                    $('.stage-info').html(NullVal(stage));
                    $('.line-info').html(NullVal(line));
                    $('.host-info').html(NullVal(hostname));
                    $('.ipaddress-info').html(NullVal(ipaddress));
                    $('.mac-info').html(NullVal(mac));
                    $('.osname-info').html(NullVal(osname));
                    $('.osversion-info').html(NullVal(osversion));
                    $('.idle-info').html(NullVal(idleTime+'h'));
                    //$('.owner-info').html(NullVal(owner));

                    /*$('.btnUpdateIdleReason').attr('data-idlereason', idleReason);
                    $('.btnUpdateIdleReason').attr('data-idtesterinfo', idTesterInfo);

                    $('.btnLockTester').attr('data-idtesterinfo', idTesterInfo);
                    $('.btnDeleteTester').attr('data-idtesterinfo', idTesterInfo);
                    

                    loadListIssues(idTesterInfo, idleReason, status, hostname, ipaddress, stage, line, mac, osname, osversion, owner);*/
                    $('.btn-wakeup').attr('onclick', 'postMachineAction(\'' + ipaddress + '\',\'wakeup\', \'' + mac + '\',\'' + hostname + '\')');
                    $('.btn-sleep').attr('onclick', 'postMachineAction(\'' + ipaddress + '\',\'sleep\', \'' + mac + '\',\'' + hostname + '\')');
                    AJAX_GET_MachineOwner(hostname, line, timeCheck);
                    AJAX_GET_IssueRecordByHostName(hostname, line);
                    AJAX_GET_MachineChangeRecordByHostName(hostname);
                });
                // Create hover event for content-item
                // on hovering
                $('.content-item').on('mouseover', function () {
                    var hostname = $(this).attr('hostname');
                    var line = $(this).attr('line');                    
                    //console.log(hostname);
                    AJAX_GET_MachineOwner(hostname, line, timeCheck);
                    $('.content-item[hostname="' + hostname + '"] .tooltip-text-media').css('visibility', 'visible');                    
                    
                });
                // on out
                $('.content-item').on('mouseout', function () {
                    var hostname = $(this).attr('hostname');
                    //console.log(hostname);
                    $('.content-item[hostname="' + hostname + '"] .tooltip-text-media').css('visibility', 'hidden');

                });
            }
        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};
async function AJAX_GET_MachineOwner(_hostname, _line, _timeCheck) {
    try {
        await $.ajax({
            url: URL_GET_MachineOwner,
            data: { hostname: _hostname, line: _line, timeCheck: _timeCheck },
            async: false,
            success: function (machineOwner) {
               /* console.clear()
                console.log(machineOwner);*/
                $('#tltOwner' + _hostname).html(NullVal(machineOwner));
                $('.owner-info').html(machineOwner);    
            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },


        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};
async function AJAX_GET_IssueRecordByHostName(_hostname,_line) {
    try {
        await $.ajax({
            url: URL_GET_IssueRecordByHostName,
            data: { hostname: _hostname },
            async: false,
            success: function (_listIssues) {
                //console.log(_listIssues);
                var tbodyHTML = '<tr><td colspan="11">No data to display!</td></tr>';
                if (_listIssues.length > 0) {
                    tbodyHTML = '';
                    $.each(_listIssues, function (idx, issue) {
                        tbodyHTML += '<tr>' +
                            '<td>' + (idx + 1) + '</td>' +
                            '<td>' + NullVal(_line) + '</td>' +
                            '<td>' + NullVal(issue.HOST_NAME) + '</td>' +
                            '<td>' + NullVal(issue.IP) + '</td>' +
                            '<td>' + NullVal(issue.MAC) + '</td>' +
                            '<td>' + NullVal(issue.ISSUE) + '</td>' +
                            '<td>' + NullVal('') + '</td>' +
                            '<td>' + NullVal(issue.STR_DETECT_TIME) + '</td>' +
                            '<td>' + NullVal(issue.STR_DEAL_TIME) + '</td>' +
                            '<td>' + NullVal(issue.STATUS) + '</td>' +                            
                            '</tr>';

                        
                    });
                    
                }
                $('#tblListIssues tbody').html(tbodyHTML);
            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },


        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};
async function AJAX_GET_MachineChangeRecordByHostName(_hostname) {
    try {
        await $.ajax({
            url: URL_GET_MachineChangeRecordByHostName,
            data: { hostname: _hostname },
            async: false,
            success: function (_listChange) {
                //console.log(_listChange);
                var tbodyHTML = '<tr><td colspan="11">No data to display!</td></tr>';
                if (_listChange.length > 0) {  
                    tbodyHTML = '';
                    $.each(_listChange, function (idx, change) {
                        tbodyHTML += '<tr>' +
                            '<td>' + (idx + 1) + '</td>' +
                            '<td>' + NullVal(change.HOST_NAME) + '</td>' +
                            '<td><pre>' + NullVal(change.ISSUE) + '</pre></td>' +
                            '<td>' + NullVal(change.STR_TIME_CHECK) + '</td>' +                            
                            '</tr>';
                    });
                    
                }
                $('#tblListHardwareChanged tbody').html(tbodyHTML);
            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },


        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};
async function AJAX_GET_TodayEnergyTotal() {
    try {
        await $.ajax({
            url: URL_GET_TodayEnergyTotal,
            data: { },
            async: false,
            success: function (todayEnergyTotal) {
                 //console.clear()
                //console.log(todayEnergyTotal); 
                $('#erDate').html(todayEnergyTotal.WorkDate);
                $('#erMachineQty').html(todayEnergyTotal.TotalMachine);
                $('#erWorkTime').html(todayEnergyTotal.TotalActive.toFixed(2) +'H');
                $('#erIdleTime').html(todayEnergyTotal.TotalIdle.toFixed(2) + 'H');
                $('#erPowerSave').html(RMBCurrencyFormat(todayEnergyTotal.PowerSave) + ' W');
                $('#erCostDown').html(RMBCurrencyFormat(todayEnergyTotal.CostDown) +' RMB');
                
            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },

        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};
async function AJAX_GET_GET_TimeRangeEnergyTotal(_timeRange) {
    try {
        var timeRangeTotal;
        await $.ajax({
            url: URL_GET_TimeRangeEnergyTotal,
            data: { timeRange: _timeRange },
            async: false,
            success: function (timeRangeEnergyTotal) {
                //console.clear()
                //console.log(timeRangeEnergyTotal);
                timeRangeTotal = getTimeRangeTotalEnergyRecord(timeRangeEnergyTotal);
                //console.log(timeRangeTotal);
                $('.date-time').html(_timeRange);
                $('.machine-qty').html(timeRangeTotal.TotalRangeMachine);
                $('.tr-working-time').html(timeRangeTotal.TotalRangeActive + 'h');
                $('.tr-idle-time').html(timeRangeTotal.TotalRangeIdle + 'h');
                $('.tr-power-save').html(timeRangeTotal.TotalRangePowerSave + ' kWh');
                $('.tr-cost-down').html(timeRangeTotal.TotalRangeCostDown + ' RMB');



            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                Highcharts.chart('power-history-chart', {
                    chart: {
                        backgroundColor: 'rgba(0,0,0,0)',
                        // styledMode: true
                    },
                    title: {
                        text: 'Power History By Time',
                        style: {
                            color: '#f8f8f8'
                        }
                    },
                    xAxis: {
                        type: 'category',
                        //categories: timeRangeTotal.DaysRange,
                        labels: {
                            style: {
                                color: '#92b7de',
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
                                var label_custom = this.value + 'h';
                                return label_custom;
                            },
                            style: {
                                color: '#92b7de',
                                fontWeight: 'bold'
                            }
                        },
                    }],
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        padding: 0,
                        itemStyle: {
                            color: '#fff'
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
                        backgroundColor: '#343A40',
                        borderColor: '#343A40',
                        useHTML: true,
                        shared: true,
                        formatter: function () {
                            var label = '';
                            var arrPoints = this.points;
                            label += '<span style="font-weight:bold;color: #f8f8f8;">' + arrPoints[0].point.name + '</span><br/>';
                            //label += '<span style="color: #f8f8f8;">Machine Qty: ' + arrPoints[0].point.options.totalMachine + '</span><br/>';
                            label += '<span style="color: #f8f8f8;">Working Time: ' + arrPoints[0].y + 'h</span><br/>';
                            label += '<span style="color: #f8f8f8;">Idle Time: ' + arrPoints[1].y + 'h</span><br/>';
                            label += '<span style="color: #f8f8f8;">Total Power Saved: ' + arrPoints[0].point.options.totalPowerSave + ' kWh</span><br/>';
                            label += '<span style="color: #f8f8f8;">Total Cost Down: ' + arrPoints[0].point.options.totalEnergyCostDown + ' RMB</span><br/>';
                            return label;
                        },
                    },
                    plotOptions: {
                        series: {
                            cursor: 'pointer',
                        },
                        column: {
                            maxPointWidth: 100,
                            dataLabels: {
                                enabled: true,
                                format: '{y:%.2f}%'
                            },
                            borderWidth: 0
                        },
                        spline: {
                            dataLabels: {
                                enabled: true,
                                format: '{point.y}h'
                            },
                            lineWidth: 1,
                        }
                    },
                    series: [{
                        type: 'spline',
                        name: 'Working time',
                        data: timeRangeTotal.ActiveRange,
                        color: '#00D823',
                        shadow: {
                            width: 6,
                            opacity: 0.1,
                            color: '#62bb76'
                        },
                        yAxis: 0,
                    },
                    {
                        type: 'spline',
                        name: 'Idle time',
                        data: timeRangeTotal.IdleRange,
                        color: '#ef0606',
                        shadow: {
                            width: 6,
                            opacity: 0.1,
                            color: '#d17c7c'
                        },
                        yAxis: 0,
                    }
                    ],
                    responsive: {
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
                    }
                });
            }

        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};

// === Support Functions ===
function GetListLineMachineInfor(_listMachine, _module) {
    var machineCount = _listMachine.length;
    var _listLineMachine = [];
    var _listMachineInLine = [];
    var currentLine = _listMachine[0].LINE;    

    for (var i = 0; i < machineCount; i++) {

        if (currentLine == _listMachine[i].LINE) {
            _listMachineInLine.push(_listMachine[i]);
        }
        if ((currentLine != _listMachine[i].LINE) || (i == machineCount-1)) {
            var objMachineLine = { Line: currentLine, ListMachine: _listMachineInLine };
            _listLineMachine.push(objMachineLine);
            currentLine = _listMachine[i].LINE;
            _listMachineInLine = [];
            _listMachineInLine.push(_listMachine[i]);
        }
    }
    if (_module === 'WOL') {
        window.sessionStorage.setItem('_listLineWOL', JSON.stringify(_listLineMachine));
    }
    if (_module === 'AIR') {
        window.sessionStorage.setItem('_listLineAIR', JSON.stringify(_listLineMachine));
    }
    return _listLineMachine;
};
function GetIdleTotalEnergySave(_listMachine) {
    var totalIdleCount = 0, totalWorkTime = 0, totalIdleTime = 0;
    var machineCount = _listMachine.length;

    for (var i = 0; i < machineCount; i++) {
        if (_listMachine[i].ActiveStatus == 2) {
            totalIdleCount++;
            totalWorkTime += parseFloat(_listMachine[i].WorkTime);
            totalIdleTime += parseFloat(_listMachine[i].IdleTime);
        }
    }

    var powerSave = totalIdleTime * (0.35 + 0.0116) * 0.8,
        costDown = powerSave * 0.64;

    return {
        MachineQty: totalIdleCount,
        WorkTime: totalWorkTime.toFixed(0),
        IdleTime: totalIdleTime.toFixed(0),
        PowerSave: powerSave.toFixed(0),
        CostDown: costDown.toFixed(0)
    };
};
function GetPowerSavingRecord(_totalEnergySaved) {    
    LoadTimeSpan();
    var todayFormat = moment().format('YYYY/MM/DD');    
    $('#erDate').html(todayFormat);
    $('#erMachineQty').html(_totalEnergySaved.MachineQty);
    $('#erWorkTime').html(RMBCurrencyFormat(_totalEnergySaved.WorkTime)+'h');
    $('#erIdleTime').html(RMBCurrencyFormat(_totalEnergySaved.IdleTime) + 'h');
    $('#erPowerSave').html(RMBCurrencyFormat(_totalEnergySaved.PowerSave) + ' kWh');
    $('#erCostDown').html(RMBCurrencyFormat(_totalEnergySaved.CostDown) + ' RMB');
    
    //AJAX_GET_TodayEnergyTotal();
};
function LoadTimeSpan() {
    var currentDate = new Date();
    var endDate = moment(currentDate).subtract('1', 'day').format('YYYY/MM/DD');
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
}

function NullVal(val) {
    if (val == null || val == undefined || val == '' || val == 'null' || val == 'NULL') {
        return 'N/A';
    }
    return val;
};
function RMBCurrencyFormat(val) {   
   
    return Intl.NumberFormat('cn-CN').format(val);
}
function filterByStatus(status) {
    flag = status;
    var content_item = document.getElementsByClassName('content-item');
    if (flag != -1) {
        for (var i = 0; i < content_item.length; i++) {
            if (content_item[i].getAttribute('status').indexOf(status) > -1) {
                content_item[i].style.display = 'block';
            } else {
                content_item[i].style.display = 'none';
            }
        }
    } else {
        for (var i = 0; i < content_item.length; i++) {
            if (content_item[i].getAttribute('status-all').indexOf(status) > -1) {
                content_item[i].style.display = 'block';
            } else {
                content_item[i].style.display = 'none';
            }
        }
    }
}

function loadPowerHistoryChart() {
    var timeRange = $('#timeSpan').val();
    //console.log(timeRange);
    AJAX_GET_GET_TimeRangeEnergyTotal(timeRange);
};
function getTimeRangeTotalEnergyRecord(_timeRangeEnergyTotal) {
    var totalRangeMachine = 0,
        totalRangeActive = 0,
        totalRangeIdle = 0,
        totalRangePowerSave = 0,
        totalRangeCostDown = 0;
    //var timeRangeDays = [];
    var timeRangeActiveTime = [];
    var timeRangeIdleTime = [];

    var listSize = _timeRangeEnergyTotal.length;

    for (var i = 0; i < listSize; i++) {
        totalRangeMachine += parseFloat(_timeRangeEnergyTotal[i].TotalMachine);
        totalRangeActive += parseFloat(_timeRangeEnergyTotal[i].TotalActive.toFixed(2));
        totalRangeIdle += parseFloat((_timeRangeEnergyTotal[i].TotalIdle * 10).toFixed(2));
        

        //timeRangeDays.push(_timeRangeEnergyTotal[i].WorkDate);
        timeRangeActiveTime.push({
                                name: _timeRangeEnergyTotal[i].WorkDate,
                                y: parseFloat(_timeRangeEnergyTotal[i].TotalActive.toFixed(2)),
                                totalMachine: parseFloat(_timeRangeEnergyTotal[i].TotalMachine),
                                totalPowerSave: RMBCurrencyFormat( parseFloat(_timeRangeEnergyTotal[i].PowerSave.toFixed(0))),
                                totalEnergyCostDown: RMBCurrencyFormat( parseFloat(_timeRangeEnergyTotal[i].CostDown.toFixed(0)))
        });

        timeRangeIdleTime.push({
                                name: _timeRangeEnergyTotal[i].WorkDate,
                                y: parseFloat((_timeRangeEnergyTotal[i].TotalIdle * 10).toFixed(2)),
                                totalMachine: parseFloat(_timeRangeEnergyTotal[i].TotalMachine),
                                totalPowerSave: RMBCurrencyFormat( parseFloat(_timeRangeEnergyTotal[i].PowerSave.toFixed(0))),
                                totalEnergyCostDown: RMBCurrencyFormat(parseFloat(_timeRangeEnergyTotal[i].CostDown.toFixed(0)))
        });

    }
    totalRangePowerSave = totalRangeIdle * (0.35 + 0.0116) * 0.8;
    totalRangeCostDown = totalRangePowerSave * 0.64;
    return {
        TotalRangeMachine: totalRangeMachine,
        TotalRangeActive: RMBCurrencyFormat(totalRangeActive.toFixed(0)),
        TotalRangeIdle: RMBCurrencyFormat(totalRangeIdle.toFixed(0)),
        TotalRangePowerSave: RMBCurrencyFormat(totalRangePowerSave.toFixed(0)),
        TotalRangeCostDown: RMBCurrencyFormat(totalRangeCostDown.toFixed(0)),
        //DaysRange: timeRangeDays,
        ActiveRange: timeRangeActiveTime,
        IdleRange: timeRangeIdleTime
    };

}
function postMachineAction(_ipaddress,_state,_mac,_hostname) {
    Swal.fire({
        title: "This might disterupt testing process, are you sure want to do it?",
        type: "question",
        showCancelButton: true,
        confirmButtonText: 'OK',
    }).then((result) => {

        if (result.value) {
            var MQTTObject = {
                ipaddress: _ipaddress,
                mac: _mac,
                state: _state
            };
            var MQTTSend = [MQTTObject];
            //console.log(JSON.stringify(ObjectSend));
            AJAX_POST_MQTTPublishMessage('WOL',JSON.stringify(MQTTSend));
        }
    });
    
};
function postLineAction(_pos, _line, _state) {

    Swal.fire({
        title: "This might disterupt testing process, are you sure want to do it?",
        type: "question",
        showCancelButton: true,
        confirmButtonText: 'OK',
    }).then((result) => {

        if (result.value) {
            //console.clear();
            //console.log(_pos + ' | ' + _line + ' | ' + _state);
            var _listLine = JSON.parse(window.sessionStorage.getItem('_listLineWOL'));
            //console.log(_listLine);
            var _currentLine = _listLine[_pos];
            //console.log(_currentLine);

            var MQTTSend = [];
            $.each(_currentLine.ListMachine, function (idx, machine) {
                var MQTTObject = {
                    ipaddress: machine.IP,
                    mac: machine.MAC,
                    state: _state
                };
                MQTTSend.push(MQTTObject);
            });
            //console.log(MQTTSend);
            AJAX_POST_MQTTPublishMessage('WOL', JSON.stringify(MQTTSend));
        }
    });    
};

// ====== AIR Control Processing ======
// === AJAX Requesting Functions ===
async function AJAX_GET_CurrentAirEnableMachine() {
    try {
        await $.ajax({
            url: URL_GET_CurrentAirEnableMachine,
            data: {},
            async: false,
            success: function (listAIREnble) {
                //console.clear()
                //console.log(listAIREnble);
                var _listLineMachine = GetListLineMachineInfor(listAIREnble, 'AIR');
                //console.log(_listLineMachine);
                $('.tb-air-custom').html('');
                if (_listLineMachine.length != 0) {
                    var totalAirOn = 0;
                    var totalAirOff = 0;

                    var lineContent = '';
                    $.each(_listLineMachine, function (idx, line) {
                        var totalLineAirOn = 0;
                        var totalLineAirOff = 0;

                        var line_name = line.Line;
                        var content_item = '';

                        var _listMachine = line.ListMachine

                        $.each(_listMachine, function (idx, machine) {
                            var status = '';

                            if (machine.AIR_STATUS == 0) {
                                status = 'normal';
                                totalAirOn++;
                                totalLineAirOn++;
                            }
                            if (machine.AIR_STATUS == 1) {
                                status = 'idle';
                                totalAirOff++;
                                totalLineAirOff++;
                            }

                            content_item +=
                                '<div id="AIR_' + machine.HOST_NAME + '" class="content-item content-item-' + status + '" status-all="-1" stage="' +
                                '" line="' + machine.LINE +                                
                                '" status="' + machine.AIR_STATUS +                                
                                '" ipaddress="' + machine.IP +                                
                                '" hostname="' + machine.HOST_NAME +

                                '><span class="hostname">' + NullVal(machine.HOST_NAME) + '</span>' +

                                '<div class="tooltip-text-media">' +
                                '<table id="tblHover">' +
                                '<tr><td class="title-hover">Host Name: </td><td>' + NullVal(machine.HOST_NAME) + '</td></tr>' +                                                                
                                '<tr><td class="title-hover">IP Address: </td><td>' + NullVal(machine.IP) + '</td></tr>' +                                                                
                            '<tr><td class="">' +
                            '<button class="btn btn-sm btn-success mb-2 btn-line" onclick="postMachineAIRAction(\'' + machine.HOST_NAME + '\',\'ON\'' + ')"><i class="fa fa-lg fa-toggle-on"></i> ON</button></td>' +
                            '<td><button class="btn btn-sm btn-secondary mb-2 btn-line" onclick="postMachineAIRAction(\'' + machine.HOST_NAME + '\',\'OFF\'' + ')"><i class="fa fa-lg fa-toggle-off"></i> OFF</button></td></tr>' +                                                                
                                '</table>' +
                                '</div>' +
                                '</div>';

                        });
                        lineContent +=
                            '<div class="col-md-12 my-0 all-tester">' +
                            '<div class="row ct-line">' +
                            '<div class="col-md-1 px-0 line-name">' +
                            '<span>LINE: ' + line_name + '</span>' +
                            '<div style="display:inline-block">(' +
                        '<span style="color: #008000;">' + totalLineAirOn + '</span> - ' +                            
                        '<span style="color: #4e6672;">' + totalLineAirOff + '</span>)</div>' +
                        '<button class="btn btn-success mb-2 btn-line " onclick="postLineAIRAction(\'' + idx + '\',\'' + line_name + '\',\'ON\'' + ')"><i class="fa fa-lg fa-toggle-on"></i> ON</button>' +
                        '<button class="btn btn-secondary mb-2 btn-line " onclick="postLineAIRAction(\'' + idx + '\',\'' + line_name + '\',\'OFF\'' + ')"><i class="fa fa-lg fa-toggle-off"></i> OFF</button>' +
                            '</div>' +
                            '<div class="col-md-11 px-0 line-content d-flex align-items-center ">' +
                            content_item +
                            '</div>' +
                            '</div>' +
                            '</div>';
                    });
                    var containerLineContent =
                        '<div class="row container-custom">' +
                        lineContent +
                        '</div>';

                    $('.tb-air-custom').append(containerLineContent);
                    $('.total-air-on').html(totalAirOn);                    
                    $('.total-air-off').html(totalAirOff);                    
                    
                    $('.total-air-all').html(totalAirOn + totalAirOff);
                }
                else {
                    $('.tb-air-custom').html('<div class="col-md-12 text-center py-2">--No data to display--</div>');
                }
                

            },
            error: function (error) {
                alert('Error on calling function: ' + error);
            },
            complete: function () {
                // Create hover event for content-item
                // on hovering
                $('.content-item').on('mouseover', function () {
                    var hostname = $(this).attr('hostname');
                    var line = $(this).attr('line');
                    //console.log(hostname);                    
                    $('.content-item[hostname="' + hostname + '"] .tooltip-text-media').css('visibility', 'visible');

                });
                // on out
                $('.content-item').on('mouseout', function () {
                    var hostname = $(this).attr('hostname');
                    //console.log(hostname);
                    $('.content-item[hostname="' + hostname + '"] .tooltip-text-media').css('visibility', 'hidden');

                });
            } 
        });
    } catch (e) {
        console.log('Error: ' + e);
    }
};
// === Support Functions ===
function postMachineAIRAction(_hostname, _state) {

    Swal.fire({
        title: "This action might cause danger for operators, do you still want to process?",
        type: "question",
        showCancelButton: true,
        confirmButtonText: 'Process',
    }).then((result) => {
        
        if (result.value) {
            var MQTTAirObject = {
                hostname: _hostname,
                state: _state
            };
            var MQTTSend = [MQTTAirObject];
            //console.log(JSON.stringify(ObjectSend));
            AJAX_POST_MQTTPublishMessage('AIR',JSON.stringify(MQTTSend));
        }
        /*console.log(result);*/
    });

    
};
function postLineAIRAction(_pos, _line, _state) {   
    Swal.fire({
        title: "This action might cause danger for operators, do you still want to process?",
        type: "question",
        showCancelButton: true,
        confirmButtonText: 'Process',
    }).then((result) => {

        if (result.value) {
            var _listLine = JSON.parse(window.sessionStorage.getItem('_listLineAIR'));
            //console.log(_listLine);
            var _currentLine = _listLine[_pos];
            //console.log(_currentLine);

            var MQTTSend = [];
            $.each(_currentLine.ListMachine, function (idx, machine) {
                var MQTTAirObject = {
                    hostname: machine.HOST_NAME,
                    state: _state
                };
                MQTTSend.push(MQTTAirObject);
            });
            //console.log(MQTTSend);
            AJAX_POST_MQTTPublishMessage('AIR', JSON.stringify(MQTTSend));
        }
        //console.log(result);
        
    });

    
};
// ====== MQTT Messages Sending Action ======
function OnclickBtnConnect() {
    var _message = $('#txtMessage').val();
    //AJAX_POST_MQTTPublishMessage(_message);
};
function AJAX_POST_MQTTPublishMessage(_topic,_message) {
    try {
        $.ajax({
            url: URL_POST_MQTTPublishMessage,
            method: 'POST',
            data: { topic: _topic, message: _message },
            success: function (result) {
                //console.log(result);
                if (result) {
                    Swal.fire({
                        title: "Command is sent successfully!",
                        type: "success",                        
                    })
                } else {
                    Swal.fire({
                        title: "There was error on sending command via MQTT!",
                        type: "error",                       
                    })
                }
            },
            error: function (error) {
                alert('Error on calling function: \n' + error);
            }
        });
    } catch (e) {
        console.log('Error: ' + e);
    }

};