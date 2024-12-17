// ========= Column chart model station status test time =========
function ColChart_StationsTestTime(_model, _colChartStatusData, _chartID) {
    Highcharts.chart(_chartID, {
        chart: {
            type: 'column', // bar , column
            height: '500vh',
            backgroundColor: '#ffffff00',
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            shadow: false,
            zoomType: 'x',
            
        },

        title: {
            text: "", //_model + ' - Stations ATE Time Status'
            style: {
                fontSize: "1vw",
                fontWeight: "bold",
                color: "#fff"
            }
        },

        tooltip: {
            style: {                
                fontWeight: "bold",
                color: "#000"
            },
            valueDecimals: 2,
            valueSuffix: '%',

        },

        xAxis: [{
            title: { text: '' },
            /*labels: {
                style: {
                    fontSize: '10px'
                },
            },*/
            type: 'category',
            categories: _colChartStatusData.ListStation,
            labels: {
                style: {
                    color: '#fff',
                    fontSize: '0.8vw',
                    fontWeight: 'bold'
                }
            },
            alignTicks: false,
            opposite: false,            
            tickInterval: 1,
        },],

        yAxis: [
            {
                title: { text: '' },
                labels: {
                    style: {
                        color: '#fff',
                        fontSize: '0.8vw',
                        fontWeight: 'bold'
                    }
                },
                gridLineColor: '#e6e6e677',
                gridLineDashStyle: 'dash',
                /*softMax: _colChartStatusData.Max,
                softMin: _colChartStatusData.Min,
                plotLines: [{
                    value: _colChartStatusData.Avg,
                    color: 'darkorange',
                    dashStyle: 'line',
                    width: 2,
                    zIndex: 1,
                    label: {
                        text: _colChartStatusData.Avg,
                        //align: 'right',
                        *//*x:-26,
                        y:20,*//*
                        style: {
                            color: '#666666',
                            fontSize: '11px',

                        }
                    }
                }, {
                    value: _colChartStatusData.Max,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Max',
                        style: {
                            color: '#666666'
                        }
                    }
                }, {
                    value: _colChartStatusData.Min,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Min',
                        style: {
                            color: '#666666'
                        }
                    }
                }]*/


            }],
        plotOptions: {
            series: {
                stacking:'percent',
                dataLabels: {
                    enabled: true,
                    format: '{point.y}%',  
                    style: {
                        color: '#000',                        
                        fontSize: '0.8rem',
                        fontWeight: 'bold',
                    }
                    
                },
                pointWidth: 45,
                pointPadding: 0,
                groupPadding: 0,
                /*borderWidth: 0*/
            }
        },

        series: [{
            name: 'Abnormal',
            data: _colChartStatusData.ListAbnormal,
            color: '#ff0066'
        },{            
            name: 'Warning',            
            data: _colChartStatusData.ListWarning,
            color:'gold'
        },{            
            name: 'Normal',            
            data: _colChartStatusData.ListNormal,  
            color:'#55bf3b'
        }],

        legend: {            
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            itemStyle: {
                color: '#fff',
                fontSize: '0.8vw',
                fontWeight: 'bold'
            },
            itemHoverStyle: {
                color: '#fff'
            }
        },
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },

            }]
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
};
// ========= Spline, line chart station test time =========
function SplineChart_ATETestAndLoadTime(_chartID, _chartData) {
    //console.log(_chartData);
    Highcharts.chart(_chartID, {
        chart: {
            backgroundColor: '#ffffff00',
            /*plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,*/
            shadow: false,
            zoomType: 'x',
            //height: '500vh',
        },

        title: {
            text: _chartData.Station +' - ATE Times',
            style: {
                fontSize: "1vw",
                fontWeight: "bold",                
                color: "#fff"
            }
        },

        tooltip: {
            valueDecimals: 2,
            /*format: 'Test time: {point.y}',*/
            valueSuffix: 's',

        },

        xAxis: [{
            title: { text: '' }, 
            /*labels: {
                style: {
                    fontSize: '10px'
                },
            },*/
            type: 'category',
            categories: _chartData.XCategories,
            labels: {
                style: {
                    color: '#fff',
                    fontSize: '0.8vw',
                    fontWeight: 'bold'
                }
            },
            alignTicks: false,
            opposite: true,
            visible: _chartData.DataLabel,
            tickInterval: 1,
        }, {
            alignTicks: false,
            /*tickInterval: 0.5,*/
            softMax: _chartData.Max,
            softMin: _chartData.Min,

        }],

        yAxis: [
            {
                title: { text: '' },   
                labels: {
                    style: {
                        color: '#fff',
                        fontWeight: 'bold'
                    }
                },
                gridLineColor:'#e6e6e677',
                gridLineDashStyle:'dot',
                softMax: _chartData.Max,
                softMin: _chartData.Min,
                plotLines: [{
                    value: _chartData.Avg,
                    color: '#ffcc66',
                    dashStyle: 'line',
                    width: 2,
                    zIndex: 1,
                    label: {
                        text:  _chartData.Avg + 's',
                        //align: 'right',
                        //x:-20,
                        /*y:20,*/
                        style: {
                            color: '#fff',
                            fontSize: '0.8vw',
                            fontWeight: 'bold'
                        }
                    }
                }, {
                    value: _chartData.Max,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Max',
                        style: {
                            color: '#666666'
                        }
                    }
                }, {
                    value: _chartData.Min,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Min',
                        style: {
                            color: '#666666'
                        }
                    }
                }]


            }],
        plotOptions: {
            series: {
                dataLabels: {
                    enabled: true,
                    format: '{point.y}s',
                    connectorColor: 'silver',
                    style: {
                        color: '#fff',
                        fontWeight: 'bold',
                        fontSize: '0.7vw'
                    }
                }
            }
        },

        series: [{
            label: {
                enabled: false
            },
            name: 'Test Time',
            type: 'spline',// chart type
            id: 'splTestTime',
            color: 'skyblue',
            shadow: {
                width: 6,
                opacity: 0.1,
                color: '#62bb76'
            },
            data: _chartData.YATETimeData.map((dt) => dt <= _chartData.Avg ? { color: '#33cc00', y: dt } : ((dt > _chartData.Avg && dt <= _chartData.DangerLine) ? { color: 'gold', y: dt } : { color: '#ff0066', y: dt })),
            zIndex: 0,
            /*lineWidth: '5px',*/
            /*showInLegend: false*/

        },{
            label: {
                enabled: false
            },
            name: 'Free Time',
            type: 'spline',// chart type
            id: 'splLoadTime',
            color: 'violet',
            shadow: {
                width: 6,
                opacity: 0.1,
                color: '#62bb76'
            },
            data: _chartData.YLoadTimeData,
            /*zIndex: 0,
            lineWidth: '5px',
            showInLegend: false*/

        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },

            }]
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            itemStyle: {
                color: '#fff',
                fontSize: '0.8vw',
                fontWeight: 'bold'
            },
            itemHoverStyle: {
                color: '#fff'
            }
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
};
function DrawTestTimeLineChart(LineData, lineChartID) {
    Highcharts.chart(lineChartID, {
        chart: {
            backgroundColor: '#ffffff00',
            /*plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,*/
            shadow: false,
            zoomType: 'x',
            height: chartHeight,
        },

        title: {
            text: LineData.Station + ' - ATE Times',
            style: {
                fontSize: "1vw",
                fontWeight: "bold",
                color: "#fff"
            }
        },

        tooltip: {
            valueDecimals: 2,
            /*format: 'Test time: {point.y}',*/
            valueSuffix: 's'
        },

        xAxis: [{
            title: { text: '' },
            /*labels: {
                style: {
                    fontSize: '10px'
                },
            },*/
            type: 'category',
            categories: LineData.XCategories,
            labels: {
                style: {
                    color: '#fff',
                    fontSize: '0.8vw',
                    fontWeight: 'bold'
                }
            },
            alignTicks: false,
            opposite: true,
            visible: LineData.DataLabel,
            tickInterval: 1,
        }, {
            alignTicks: false,
            /*tickInterval: 0.5,*/
            softMax: LineData.Max,
            softMin: LineData.Min,

        }],

        yAxis: [
            {
                title: { text: '' },
                labels: {
                    style: {
                        color: '#fff',
                        fontWeight: 'bold'
                    }
                },
                gridLineColor: '#e6e6e677',
                gridLineDashStyle: 'dot',
                softMax: LineData.Max,
                softMin: LineData.Min,
                plotLines: [{
                    value: LineData.Avg,
                    color: '#ffcc66',
                    dashStyle: 'line',
                    width: 2,
                    zIndex: 1,
                    label: {
                        text: LineData.Avg + 's',
                        //align: 'right',
                        /*x:-26,
                        y:20,*/
                        style: {
                            color: '#fff',
                            fontSize: '0.8vw',
                            fontWeight: 'bold'
                        }
                    }
                }, {
                    value: LineData.Max,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Max',
                        style: {
                            color: '#666666'
                        }
                    }
                }, {
                    value: LineData.Min,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Min',
                        style: {
                            color: '#666666'
                        }
                    }
                }]


            }],
        plotOptions: {
            series: {
                dataLabels: {
                    enabled: true,
                    format: '{point.y}s',
                    connectorColor: 'silver',
                    style: {
                        color: '#fff',
                        fontWeight: 'bold',
                        fontSize: '0.7vw'
                    }
                }
            }
        },

        series: [{
            label: {
                enabled: false
            },
            name: 'Test Time',
            type: 'spline',// chart type
            id: 'splTestTime',
            color: 'skyblue',
            shadow: {
                width: 6,
                opacity: 0.1,
                color: '#62bb76'
            },
            data: LineData.YPlotData.map((dt) => dt <= LineData.Avg ? { color: '#33cc00', y: dt } : ((dt > LineData.Avg && dt <= LineData.DangerLine) ? { color: 'gold', y: dt } : { color: '#ff0066', y: dt })),
            zIndex: 0,
            /*lineWidth: '5px',*/
            /*showInLegend: false*/

        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },

            }]
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            itemStyle: {
                color: '#fff',
                fontSize: '0.8vw',
                fontWeight: 'bold'
            },
            itemHoverStyle: {
                color: '#fff'
            }
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
};
// ========= Pie chart station test time =========
function DrawTestTimePieChart(PieData, pieChartID, _station) {
    Highcharts.chart(pieChartID, {
        chart: {
            backgroundColor: '#ffffff00',
            /*plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            shadow: false,*/
            margin: 20,
            type: 'pie',
            /*height: chartHeight,*/
        },
        title: {
            text: _station + ' - Status Percentiles',
            style: {
                fontSize: "1vw",
                fontWeight: "bold",
                color: "#fff"
            }
        },

        tooltip: {
            pointFormat: '<b>{series.name}{point.y}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.y}%',
                    connectorColor: 'white',
                    style: {
                        color: '#fff',
                        fontWeight: 'normal',
                        fontFamily: 'Roboto-Medium',
                        fontSize: '0.8vw'
                    }
                }
            }
        },
        series: [{
            name: '',
            data: PieData,
            size: '30%',
            innerSize: '70%',
            showInLegend: true
        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                
            }]
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            itemStyle: {
                color: '#fff',
                fontSize: '0.8vw',
                fontWeight: 'bold'
            },
            itemHoverStyle: {
                color: '#fff'
            }
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
};
// ========= Line chart ATE machine test time =========
function DrawATETimeDateLineChart(ListATETimeDate, ATETimeDateChartID, Model, Station, ATE) {
    Highcharts.chart(ATETimeDateChartID, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            shadow: false,
            zoomType: 'x',
            height: chartHeight,
        },

        title: {
            text: Model + ' - ' + Station + ' - ' + ATE,
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
            categories: ListATETimeDate.XCategories,

            alignTicks: false,
            opposite: true,
            tickInterval: 1,
        }, {
            alignTicks: false,
            /*tickInterval: 0.5,*/
        }],

        yAxis: [
            {
                title: { text: '' },
                //tickInterval: 30,
                plotLines: [{
                    value: ListATETimeDate.Avg,
                    color: 'darkorange',
                    dashStyle: 'line',
                    width: 2,
                    zIndex: 1,
                    label: {
                        text: ListATETimeDate.Avg,
                        //align: 'right',
                        /*x:-26,
                        y:20,*/
                        style: {
                            color: '#666666',
                            fontSize: '11px',

                        }
                    }
                }, {
                    value: undefined, // undefined
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Max',
                        style: {
                            color: '#666666'
                        }
                    }
                }, {
                    value: undefined,
                    color: 'red',
                    dashStyle: 'line',
                    width: 1,
                    zIndex: 1,
                    label: {
                        text: 'Min',
                        style: {
                            color: '#666666'
                        }
                    }
                }]


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
            }
        },

        series: [{
            label: {
                enabled: false
            },
            name: 'Data',
            type: 'line',//Data plot
            id: 's1',
            lineWidth: 5,
            data: ListATETimeDate.YPlotData.map((dt) => dt <= ListATETimeDate.Avg ? { color: 'green', y: dt } : ((dt > ListATETimeDate.Avg && dt <= ListATETimeDate.DangerSpec) ? { color: 'gold', y: dt } : { color: 'red', y: dt })),
            zIndex: 0,
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
};