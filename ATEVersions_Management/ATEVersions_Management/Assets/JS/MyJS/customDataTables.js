// Initial dataTable with datatable.js

$(document).ready(function () {
    $('#dataTable').DataTable();
    createBasicDataTable();
    createVesionDataTable();
    createATEListDataTable();
    
});
// Custom version dataTable
function createBasicDataTable() {
    $('#basicDataTable').DataTable({
        "aLengthMenu": [[6, 25, 50, -1], [6, 25, 50, "All"]]
	});
}
function createVesionDataTable() {
    $('#dtblVersion').DataTable({
        "aoColumns": [
            {
                "sType": "Model",
                "bSortable": true,
            },
            {
                "sType": "Version",
                "bSortable": true,
            },
            {
                "bSortable": false,
            },
            {
                "bSortable": false,
            },
            {
                "sType": "DateTime",
                "bSortable": true,
            },
            {
                "bSortable": false,
            },
            {
                "bSortable": false,
            },
            {
                "sType": "Usage",
                "bSortable": true,
            },
            {
                "bSortable": false,
            },
        ],
        "aaSorting": [[4, "desc"]],
    });
};
function createATEListDataTable() {
    $('#dtblATEList').DataTable({
        "aoColumns": [
            {
                "sType": "Model",
                "bSortable": true,
            },
            {
                "sType": "Version",
                "bSortable": true,
            },
            {
                "bSortable": false,
            },
            {
                "bSortable": false,
            },
            {
                "bSortable": false,
            },
            {
                "sType": "Status",
                "bSortable": true,
            },
            {
                "sType": "Action",
                "bSortable": false,
            },
        ],
        "aaSorting": [[5, "asc"]],
    });
};
function createTestPlanDataTable() {
    $('#dtblTestPlan').DataTable({
        "aoColumns": [
            {
                "sType": "Model",
                "bSortable": false,
            },
            {
                "sType": "Version",
                "bSortable": true,
            },
            {
                "sType": "DateTime",
                "bSortable": true,
            },
            {
                "bSortable": false,
            },
            {                
                "bSortable": false,
            },
            {
                "bSortable": false,
            },
            {
                "bSortable": false,
            },            
        ],
        "aaSorting": [[2, "desc"]],
    });
};
function createFATPDataTable() {
    $('#DataTableFATP').DataTable({
        "aoColumns": [
            {                
                "bSortable": true,
            },
            {                
                "bSortable": true,
            },
            {
                "bSortable": true,
            },
            {
                "bSortable": true,
            },
            {
                "bSortable": true,
            },
            {                
                "bSortable": true,
            },
            {
                "bSortable": true,
            },
            {
                "sType": "DateTime",
                "bSortable": true,
            },
            /*{
                "sType": "DateTime",
                "bSortable": true,
            },*/
            {
                "bSortable": true,
            },
            {
                "sType": "",
                "bSortable": true,
            },
            {
                "sType": "Action",
                "bSortable": false,
            },
        ],
        "aaSorting": [[6, "desc"]],
        "aLengthMenu": [[50, 75, 100, -1], [50, 75, 100, "All"]]
    });
};
function createLCRDataTable() {
    $('#tableLCRData').DataTable();
};
// Custom sort for version dataTable
$.extend($.fn.dataTableExt.oSort, {
    // Sort by DateTime
    "DateTime-asc": function (x, y) {
        x = new Date(x);
        y = new Date(y);
        //console.log(x + '|' + y);
        return ((x < y) ? -1 : (x > y) ? 1 : 0);
    },
    "DateTime-desc": function (x, y) {
        x = new Date(x);
        y = new Date(y);        
        return ((x < y) ? 1 : (x > y) ? -1 : 0);
    },
    // Sort by Version
    "Version-asc": function (x, y) {
        x = parseFloat(x.substr(3));
        y = parseFloat(y.substr(3));
        /*console.log(x + '|' + y);*/
        return ((x < y) ? -1 : (x > y) ? 1 : 0);
    },
    "Version-desc": function (x, y) {
        x = parseFloat(x.substr(3));
        y = parseFloat(y.substr(3));
        /*console.clear();*/
        return ((x < y) ? 1 : (x > y) ? -1 : 0);
    },
    //Sort by Version Usage
    "Usage-asc": function (x, y) {
        x = GetUsageCode(x);
        y = GetUsageCode(y);
        /*console.log(x + '|' + y); */       
        return ((x < y) ? 1 : (x > y) ? -1 : 0);
    },
    "Usage-desc": function (x, y) {
        x = GetUsageCode(x);
        y = GetUsageCode(y);
        /*console.clear();*/
        return ((x < y) ? -1 : (x > y) ? 1 : 0);
    },
    //Sort by AListStatus
    "Status-asc": function (x, y) {
        x = GetStatusCode(x);
        y = GetStatusCode(y);
        /*console.log(x + '|' + y);*/
        return ((x < y) ? -1 : (x > y) ? 1 : 0);
    },
    "Status-desc": function (x, y) {
        x = GetStatusCode(x);
        y = GetStatusCode(y);
        /*console.clear();*/
        return ((x < y) ? 1 : (x > y) ? -1 : 0);
    },
});

function GetStatusCode(val) {
    val = $(val).text();
    switch (val.trim().toLowerCase()) {
        case 'normal':
        case 'prepared': return 1;
        case 'warning': 
        case 'checked': return 2;
        case 'abnormal':
        case 'approved': return 3;

    }
};

function GetUsageCode(val) {
    val = $(val).text();
    switch (val) {
        case 'Unused': return 0;
        case 'In use': return 1;       
    }
};

//Custom datatable with export button
var detroyDatatable = function (tableID) {
    $('#' + tableID).DataTable().clear().destroy();
};
var dataTableExportable = function (tableID, fnameTitle) {
    if (fnameTitle == undefined || fnameTitle === '') {
        fnameTitle = 'Data Exported';
    }
    $('#' + tableID).DataTable({
        info: false,
        ordering: false,
        searching: false,
        paging: false,
        dom: 'Bfrtip',
        buttons: [
            $.extend(true, {}, buttonCommon, {
                extend: 'excel',
                title: fnameTitle
            }),
        ]
    });
};

var DataTable = $.fn.dataTable;
$.extend(true, DataTable.Buttons.defaults, {
    dom: {
        container: {
            className: 'dt-buttons btn-group flex-wrap'
        },
        button: {
            className: 'btn btn-success',
            active: 'active'
        },
    },
});
var buttonCommon = {
    exportOptions: {
        format: {
            body: function (data, row, col, node) {
                if ($(node).children('input').length > 0) return $(node).children('input').first().val()
                return data
            }
        }
    }
};