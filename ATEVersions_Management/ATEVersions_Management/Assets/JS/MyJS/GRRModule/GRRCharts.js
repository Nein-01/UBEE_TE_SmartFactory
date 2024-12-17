// === Chart Presentation === 
function DrawChartOPInteraction(_listOperSamples) {
    var _samplesSize = _listOperSamples.length;
    var _colorList = ['dodgerblue','red','forestgreen'];
    var _listSeries = [];    
    for (var i = 0; i < _samplesSize; i++) {
        var serie = {
            label: {
                enabled: false,
            },
            name: 'Oper ' + (i + 1),
            type: 'line',//Data plot            
            data: _listOperSamples[i].OperSampleAVG,
            zIndex: 0,
            //lineWidth: 5,
            color: _colorList[i],
            dashStyle: 'dash'
            //showInLegend: false

        }
        _listSeries.push(serie);        
    }

    Highcharts.chart('divChartOPInteraction', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            shadow: false,
            zoomType: 'x',
            //height: chartHeight,
        },

        title: {
            text: 'Oper * Parts Interaction',
            style: {
                fontSize: '16px',
            }
        },

        tooltip: {
            valueDecimals: 2
        },

        xAxis: [{
            title: { text: '' },
            type: 'category',
            //categories: GraphDrawData.XCategories,
            labels: {
                formatter: function () {
                    return (this.value + 1);
                }
            },
            alignTicks: false,            
            visible: true,
            tickInterval: 1,
        }, {
            alignTicks: false,
            //softMax: GraphDrawData.Max,
            //softMin: GraphDrawData.Min,

        }],

        yAxis: [{
                title: { text: '' },
                //softMax: GraphDrawData.Max,
                //softMin: GraphDrawData.Min,
               

        }],
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false,
                    format: '{point.y}',
                    connectorColor: 'silver',
                    style: {
                        fontWeight: 'normal',
                        fontSize: '11px'
                    }
                }
            }
        },

        series: _listSeries,

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
}

function DrawChartVariationComponent(EV, AV, RR, PV, TV) {

    var dataValue = [{ color: 'green', y: RR }, { color: 'green', y: EV }, { color: 'orange', y: AV }, { color: 'orange', y: PV }];

    Highcharts.chart('divChartVariationComponent', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            shadow: false,
            type:'column',
            zoomType: 'x',
            //height: chartHeight,
        },

        title: {
            text: 'Variation Components',
            style: {
                fontSize: '16px',
            }
        },

        tooltip: {
            valueDecimals: 2
        },

        xAxis: [{
            title: { text: '' },
            type: 'category',
            categories: ['Gage R&R', 'Repeat', 'Reprod', 'Part-to-Part'],

            alignTicks: false,
            /*opposite: true,
            visible: false,*/
            tickInterval: 1,
        }, {
            alignTicks: false,
            //softMax: GraphDrawData.Max,
            //softMin: GraphDrawData.Min,

        }],

        yAxis: [
            {
                title: { text: '' },
                //softMax: GraphDrawData.Max,
                //softMin: GraphDrawData.Min,                


            }],
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true,
                    format: '{point.y}',
                    connectorColor: 'silver',
                    style: {
                        fontWeight: 'normal',
                        fontSize: '11px'
                    }
                }
            },
            column: {
                dataLabels: {
                    enabled: true,
                    format: '{point.y}%',
                    connectorColor: 'silver',
                    style: {
                        fontWeight: 'normal',
                        fontSize: '11px'
                    }
                }
                //stacking:'percent',
            }
        },

        series: [{
            name: 'Variations',
            //data: dataValue,
            data: dataValue.map((dt) => parseFloat(dt.y) < 30 ? { color: 'green', y: dt.y } : ((parseFloat(dt.y) >= 30 && parseFloat(dt.y) <= 60) ? { color: 'gold', y: dt.y } : { color: 'orange', y: dt.y })),
            showInLegend: false,
        },
        
        ],

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
}