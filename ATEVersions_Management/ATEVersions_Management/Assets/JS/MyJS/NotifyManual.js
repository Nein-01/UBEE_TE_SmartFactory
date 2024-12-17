//Notify when ATEList status is changed
$(document).ready(function () {
    // Initialize variables
    var URL_GET_ATENotify = $('#UrlNotiChange').val();
    var URL_GRRApprove = $('#URL_GRRApprove').val();
    var URL_AllNotify = $('#URL_AllNotify').val();
    var URL_ATEListEdit = $('#UrlATEEdit').val();
    // Get notifyDropdown element
    let notifyDropdown = $('#notifyDropdown');
    
    // notifyDropdown != null || notifyDropdown != undefined
    if (notifyDropdown != null && notifyDropdown != undefined && notifyDropdown.length > 0) {        
        GET_ATEListNotify(URL_GET_ATENotify, URL_ATEListEdit, URL_GRRApprove, URL_AllNotify);
    }
});

// === AJAX Functions ===
async function GET_ATEListNotify(URL_GET_ATENotify, URL_ATEListEdit, URL_GRRApprove, URL_AllNotify) {
    try
    {
        await $.ajax({
            url: URL_GET_ATENotify,
            dataType: 'json',
            type: 'GET',
            success: function (data) {

                // Assign notify counter to icon
                let ATEListNotify = data.ATEListNotify;
                let GRRNotify = data.GRRNotify;
                let notifyCounter = ATEListNotify.length + GRRNotify.length;
                $('#notifyCounter').text(notifyCounter);
                window.localStorage.setItem('notifyCounter', notifyCounter);
                // Append link to notify tab
                if (ATEListNotify.length >= 3) {
                    AppendATEListNotifySummarize(ATEListNotify.length, URL_AllNotify);
                } else {
                    AppendATEListNotifyItem(ATEListNotify, URL_ATEListEdit);
                }
                if (GRRNotify.length >= 3) {
                    AppendGRRNotifySummarize(GRRNotify.length, URL_AllNotify);
                } else {
                    AppendGRRNotifyItem(GRRNotify, URL_GRRApprove);
                }    

            },
            error: function () {
                let msg = "Error on getting notifications!";
                //alert(msg);
                toastr.error(msg, 'Error');
            }
        });
    }
    catch (e)
    {
        console.log('Error: ' + { e: e });
    }
}
//
// === Support Functions ===
function AppendATEListNotifyItem(ATEListNotify, UrlToEdit) {
    $.each(ATEListNotify, function (idx = 0, ate) {
        var txtVerModel = ate.ModelName + '_' + ate.VersionName;
        var txtStatus, txtPre = ate.PreparedBy, txtCheck = 'Checker', txtAppr = 'Approver', txtPrNote = '...', txtChkNote = '...', txtAprNote = '...', color;
        var date = new Date(parseInt(ate.UpdatedAt.substr(6)));
        var dateFormat = date.toLocaleString('en-GB');

        if (ate.PreparerNote != null) {
            txtPrNote = ate.PreparerNote;
        }

        switch (ate.Status) {
            case 1:
                txtStatus = 'fa-file-alt';
                color = 'bg-warning';
                if ((ate.CheckerNote != null)) {
                    txtCheck = ate.CheckedBy;
                    txtChkNote = ate.CheckerNote;
                }
                if ((ate.ApproverNote != null)) {
                    txtAppr = ate.ApprovedBy;
                    txtAprNote = ate.ApproverNote;
                }
                break;
            case 2:
                txtStatus = 'fa-pen-square fa-2x';
                color = 'bg-info';
                txtCheck = ate.CheckedBy;
                txtChkNote = ate.CheckerNote;
                if ((ate.ApproverNote != null)) {
                    txtAppr = ate.ApprovedBy;
                    txtAprNote = ate.ApproverNote;
                }
                break;
        }

        var notifyLinkItemHTML = '<a href="' + UrlToEdit + '/' + ate.CheckListID + '" class="dropdown-item d-flex align-items-center" target="_blank">'
            + '<div class="mr-3"><div class="icon-circle ' + color + '"><i class="fas ' + txtStatus + ' text-white"></i></div></div>'
            + '<div>'
            + '<div class= "small text-gray-600" > '
            + 'Updated at: ' + dateFormat + '<br/> By: ' + ate.UpdatedBy
            + '</div>'
            + '<span class="font-weight-bold">'
            + 'Version: <span class="text-primary">' + txtVerModel + '</span>'
            + '</br>'
            + txtPre + ': ' + txtPrNote
            + '</br>'
            + txtCheck + ': ' + txtChkNote
            + '</br>'
            + txtAppr + ': ' + txtAprNote
            + '</span>'
            + '</div>' +
            '</a>';

        $('#notifyList').append(notifyLinkItemHTML);
        idx++;
        if (idx == 3) {
            return false;
        }
    });
};
function AppendGRRNotifyItem(GRRNotify, URL_GRRApprove) {    

    $.each(GRRNotify, function (idx = 0, grr) {
        var txtGageModel = grr.GageModel + '_' + grr.GageName + '_' + grr.PartName;
        var txtPre = grr.PreparedBy, txtAppr = 'Approver', txtPrNote = '...', txtAprNote = '...';
        var date = new Date(parseInt(grr.UpdatedAt.substr(6)));
        var dateFormat = date.toLocaleString('en-GB');

        if (grr.PreparedNote != null) {
            txtPrNote = grr.PreparedNote;
        }

        var notifyLinkItemHTML = '<a href="' + URL_GRRApprove + '/' + grr.GRR_ID + '" class="dropdown-item d-flex align-items-center" target="_blank">'
            + '<div class="mr-3"><div class="icon-circle bg-secondary"><i class="fas fa-chart-pie text-white"></i></div></div>'
            + '<div>'
            + '<div class= "small text-gray-600" > '
            + 'Prepared at: ' + dateFormat + '<br/> By: ' + grr.UpdatedBy
            + '</div>'
            + '<span class="font-weight-bold">'
            + 'Gage Report For: <span class="text-primary">' + txtGageModel + '</span>'
            + '</br>'
            + txtPre + ': ' + txtPrNote
            + '</br>'
            + txtAppr + ': ' + txtAprNote
            + '</span>'
            + '</div>' +
            '</a>';
        $('#notifyList').append(notifyLinkItemHTML);

        idx++;
        if (idx == 3) {
            return false;
        }
    });
};
function AppendATEListNotifySummarize(ATEListNotifyLength, URL_AllNotify) {
    var ATEListNotifySummerizeHTML = '<a href="' + URL_AllNotify +'" target="_blank" class="dropdown-item d-flex align-items-center border-bottom">'
        + '<div class="mr-3 icon-circle bg-orange"><i class="fas fa-file-alt text-white"></i></div>'
        + '<div class="font-weight-bold">ATE Check List</div>'
        + '<div class="icon-circle bg-danger text-white text-sm-center ml-2 p-1 div-sm"><i class="fas">' + ATEListNotifyLength + '</i></div></a>';

    $('#notifyList').append(ATEListNotifySummerizeHTML);
}
function AppendGRRNotifySummarize(GRRNotifyLength, URL_AllNotify) {
    var GRRNotifySummerizeHTML = '<a href="' + URL_AllNotify +'" target="_blank" class="dropdown-item d-flex align-items-center border-bottom">'
        + '<div class="mr-3 icon-circle bg-secondary"><i class="fas fa-chart-pie text-white"></i></div>'
        + '<div class="font-weight-bold">Gage Repeatability & Reproducibility Report</div>'
        + '<div class="icon-circle bg-danger text-white text-sm-center ml-2 div-sm"><i class="fas">' + GRRNotifyLength + '</i></div></a>';
    $('#notifyList').append(GRRNotifySummerizeHTML);
}
// 
