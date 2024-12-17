
//CPK Chart draw
Highcharts.chart('CPKChart', {
    chart: {
        zoomType: 'x',

    },

    title: {
        text: undefined
    },

    tooltip: {
        valueDecimals: 4
    },
    xAxis: [{
        title: { text: '' },
        alignTicks: false,
        opposite: true,
        tickInterval: 1,
    }, {
        alignTicks: false,
        /*tickInterval: 0.5,*/
        softMax: usl,
        softMin: lsl,
        plotLines: [{
            value: usl,
            color: 'red',
            dashStyle: 'longdash',
            width: 1,
            zIndex: 0,
            label: {
                text: 'USL',
                style: {
                    color: '#666666'
                }
            }
        }, {
            value: lsl,
            color: 'red',
            dashStyle: 'longdash',
            width: 1,
            zIndex: 0,
            label: {
                text: 'LSL',
                style: {
                    color: '#666666'
                }
            }
        }]
    }],

    yAxis: [{
        title: { text: '' },
        labels: {
            enabled: true
        },
        alignTicks: false,
        softMax: usl,
        softMin: lsl
    }, {
        title: { text: '' },
        opposite: true,
        labels: {
            enabled: false
        }
    }, {
        title: { text: '' },
        opposite: true,
        labels: {
            enabled: false
        }
    }],
    plotOptions: {
        histogram: {

        }
    },
    series: [{
        label: {
            enabled: false
        },
        name: 'Quantity',
        type: 'histogram',// Data bar
        xAxis: 1,
        yAxis: 2,
        baseSeries: 's1',
        binsNumber: binCount,
        binWidth: binWidth, //binWidth
        zIndex: 0,
        tooltip: {
            valueDecimals: 0
        },
        showInLegend: false,
        color: "#B3B3B3"
    }, {
        label: {
            enabled: false
        },
        name: 'Within',
        type: 'bellcurve',// Within
        xAxis: 1,
        yAxis: 1,
        data: _wth,
        zIndex: 1,
        fillOpacity: 0,
        color: "#FA0000"
    }, {

        label: {
            enabled: false
        },
        name: 'Overall',
        type: 'bellcurve',// Overall
        dashStyle: 'longdash',
        xAxis: 1,
        yAxis: 1,
        baseSeries: 's1',
        zIndex: 1,
        fillOpacity: 0,
        color: "#323933"
    }, {
        label: {
            enabled: false
        },
        name: 'Data',
        type: 'line',//Data plot
        id: 's1',
        data: _data,
        visible: false,
        showInLegend: false

    }],

    legend: {
        style: {
            fontSize: '11px'
        },
        layout: 'horizontal',
        align: 'center',
        verticalAlign: 'bottom'
    },
    credits: {
        enabled: false
    },
    navigation: {
        buttonOptions: {
            enabled: false
        }
    },
});