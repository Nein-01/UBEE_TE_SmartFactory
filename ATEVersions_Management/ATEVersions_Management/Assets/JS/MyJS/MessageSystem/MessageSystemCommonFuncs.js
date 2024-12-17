// ========= Global Variables =========
var gblLoggedUserID = parseInt($('#chatFloatIcon').attr('loggedUserID'));
var gblIsChatHistoryFirstLoad;
//
var gblIntervalID_GET_allUnreadConut;
var gblIntervalID_GET_listSingleRecipient;
var gblIntervalID_GET_messageOf2User;
var gblIntervalID_GET_listGroupRecipient;
var gblIntervalID_GET_messageInGroup;
var gblIntervalDelay = 5 * 1000;
var gbliUnreadCount;
// ========= Browser Event Functions =========
$(document).ready(function () {  
    gblIsChatHistoryFirstLoad = true;
    AJAX_GET_MessageUnreadCountOfLoggedUser(gblLoggedUserID);
    gblIntervalID_GET_allUnreadConut = setInterval(AJAX_GET_MessageUnreadCountOfLoggedUser, gblIntervalDelay, gblLoggedUserID);
});

//
$('#chatFloatIcon').on('click', function (e) {
    let eleChatMainContainer = $('#chatMainContainer');
    if (eleChatMainContainer.hasClass('d-none')) {
        //
        //clearInterval(gblIntervalID_GET_allUnreadConut);
        // Modify elements appearances
        eleChatMainContainer.removeClass('d-none');
        // 
        let loggedUserID = parseInt($(this).attr('loggedUserID'));
        //AJAX_GET_ListUserExceptLoggedUser(loggedUserID);
        AJAX_GET_ListSingleRecipientOfLoggedUser(loggedUserID);
        gblIntervalID_GET_listSingleRecipient = setInterval(AJAX_GET_ListSingleRecipientOfLoggedUser, gblIntervalDelay, loggedUserID);
        AJAX_GET_ListGroupContainLoggedUser(loggedUserID);
        gblIntervalID_GET_listGroupRecipient = setInterval(AJAX_GET_ListGroupContainLoggedUser, gblIntervalDelay, loggedUserID);
        //
        
    }
    else {
        eleChatMainContainer.addClass('d-none');
        //
        //gblIntervalID_GET_allUnreadConut = setInterval(AJAX_GET_MessageUnreadCountOfLoggedUser, gblIntervalDelay, gblLoggedUserID);
        clearInterval(gblIntervalID_GET_listSingleRecipient);
        clearInterval(gblIntervalID_GET_messageOf2User);
        clearInterval(gblIntervalID_GET_listGroupRecipient);
        clearInterval(gblIntervalID_GET_messageInGroup);
    }
});
$('#searchRecipient').on('keyup', function (e) {
    var val = this.value.toLowerCase();
    var item_recipient = document.getElementsByClassName('item-recipient');
    //console.log(val);
    for (var i = 0; i < item_recipient.length; i++) {
        if (item_recipient[i].textContent.toLowerCase().indexOf(val) > -1) {
            item_recipient[i].style.display = 'block';
        } else {
            item_recipient[i].style.display = 'none';
        }
    }
});
$('#searchGroupMember').on('keyup', function (e) {
    var val = this.value.toLowerCase();
    var item_addmember = document.getElementsByClassName('item-addmember');
    //console.log(val);
    for (var i = 0; i < item_addmember.length; i++) {
        if (item_addmember[i].textContent.toLowerCase().indexOf(val) > -1) {
            item_addmember[i].style.display = 'block';
        } else {
            item_addmember[i].style.display = 'none';
        }
    }
});
$('#messageContent').on('keypress', function (e) {    
    
    let pressedKey = e.which;    
    $(this).attr('messageType', 'text');
    if (pressedKey === 13) {
        // key code 13 is Enter key
        e.preventDefault();
        //
        Action_SubmitMessageContent();
        //
        
    }
});
$('#btnSendMessageContent').on('click', function (e) {
    Action_SubmitMessageContent();
});
$('#btnCreateNewGroup').on('click', function (e) {
    let jqGroupMembersSection = $('#groupMembersSection');
    if (jqGroupMembersSection.hasClass('d-none')) {
        jqGroupMembersSection.removeClass('d-none');
    } else {
        jqGroupMembersSection.addClass('d-none');
    }
    //
    $('.modify-member-error').addClass('d-none'); 
    $('#inputGroupName').val('');
    $('#containerInputGroupName').removeClass('d-none');
    $('.container-add-member').removeClass('d-none');    
    $('#btnSubmitAddMember').attr('submitType', 'create');
    $('.container-group-member').addClass('d-none'); 
    //
    let eleLiUser = '';
    /*
    let listUserExceptLoggedUser = JSON.parse(sessionStorage.getItem('listUserExceptLoggedUser'));        
    $.each(listUserExceptLoggedUser, function (idx, user) {
        let decorStatus = 'offline';
        if ((idx % 2) == 0) {
            decorStatus = 'online';
        }

        eleLiUser += '<li class="clearfix item-addmember " addUserId="' + user.UserID + '" addUserName="' + user.UserName + '" addUserFullName="' + user.FullName + '" addUserRoleName="' + user.RoleName + '" >' +
            '<div class="about" >' +
            '<div class="name">' + user.FullName + '</div>' +
            '<div class="status"><i class="fa fa-circle ' + decorStatus + '"></i> ' + user.UserName + ' (' + user.RoleName + ') </div>' +
            '</div>' +
            '</li>';
    });
    */
    
    let listSingleRecipientLoggedUser = JSON.parse(sessionStorage.getItem('listSingleRecipientLoggedUser'));
    $.each(listSingleRecipientLoggedUser, function (idx, recipient) {
        let decorStatus = 'offline';
        if ((idx % 2) == 0) {
            decorStatus = 'online';
        }

        eleLiUser += '<li class="clearfix item-addmember " addUserId="' + recipient.USER_ID + '" addUserName="' + recipient.USER_NAME + '" addUserFullName="' + recipient.FULL_NAME + '" addUserRoleName="' + recipient.ROLE_NAME + '" >' +
            '<div class="about" >' +
            '<div class="name">' + recipient.FULL_NAME + '</div>' +
            '<div class="status"><i class="fa fa-circle ' + decorStatus + '"></i> ' + recipient.USER_NAME + ' (' + recipient.ROLE_NAME + ') </div>' +
            '</div>' +
            '</li>';
    });
    
    $('#listAddMember').html(eleLiUser);    
    $('#selectedMemberNum').html('0');
    //
    $('.item-addmember').on('click', function (e) {
        let thisEle = $(this);
        if (thisEle.hasClass('active')) {
            thisEle.removeClass('active');
        }
        else {
            thisEle.addClass('active');
        }
        let selectedItemMember = $('.item-addmember.active').length;
        $('#selectedMemberNum').html(selectedItemMember);
    });
});
$('#btnAddMember').on('click', function (e) {
    //
    let jqContainerAddMember = $('.container-add-member');
    let jqBtnSubmitAddMember = $('#btnSubmitAddMember');
    let jqChatFloatIcon = $('#chatFloatIcon'); 
    let jqChatHeaderRecipient = $('#chatHeaderRecipient'); 
    let objModifyMember = $('.modify-member-error');
    //
    jqBtnSubmitAddMember.attr('submitType', 'add');
    //
    if (jqContainerAddMember.hasClass('d-none')) {
        jqContainerAddMember.removeClass('d-none');
        //
        let iSelectedGroupID = jqChatHeaderRecipient.attr('groupRecipientGroupID');
        let iLoggedUserID = jqChatFloatIcon.attr('loggedUserID');
        AJAX_GET_ListUserNotInGroup(iSelectedGroupID, iLoggedUserID);
    }
    else {
        jqContainerAddMember.addClass('d-none');
        objModifyMember.html('');
    }
    //

});
$('#btnSubmitAddMember').on('click', function (e) {
    //
    let submitMemberType = $(this).attr('submitType');
    //
    let loggedUserID = parseInt($('#chatFloatIcon').attr('loggedUserID'));
    let txtGroupName = $('#inputGroupName').val();
    let errorMessage = '';
    let objModifyMember = $('.modify-member-error');    
    let listObjMemberToAdd = $('.item-addmember.active');
    //
    if (submitMemberType === 'create') {
        if (NullVal(txtGroupName) == null) {
            errorMessage += 'Must input group name!\n';
        }
        if (listObjMemberToAdd.length == 0) {
            errorMessage += 'Must select member to add!\n';
        }
        if (NullVal(errorMessage) != null) {
            objModifyMember.removeClass('d-none');
            objModifyMember.html(errorMessage);
        }
        else {
            objModifyMember.addClass('d-none');
            //console.log(listObjMemberToAdd);
            let arrGroupUserIDExceptSender = [];
            listObjMemberToAdd.each(function (idx, member) {
                //console.log(member);
                arrGroupUserIDExceptSender.push(parseInt($(member).attr('addUserId')));
            });
            //console.log(arrGroupUserIDExceptSender);

            AJAX_POST_CreateNewMessageGroup(loggedUserID, JSON.stringify(arrGroupUserIDExceptSender), txtGroupName);

            //
            let jqItemAddMember = $('.item-addmember');
            jqItemAddMember.removeClass('active');
            $('#inputGroupName').val('');
            $('#selectedMemberNum').html('0');
        }
    }
    
    //
    if (submitMemberType === 'add') {
        if (listObjMemberToAdd.length == 0) {
            errorMessage += 'Must select member to add!\n';
        }
        if (NullVal(errorMessage) != null) {
            objModifyMember.removeClass('d-none');
            objModifyMember.html(errorMessage);
        }
        else {
            objModifyMember.addClass('d-none');
            console.log('Add member check');
            //
            let iGroupID = parseInt($('#chatHeaderRecipient').attr('groupRecipientGroupID'));
            console.log(iGroupID);
            let arrGroupUserIDExceptSender = [];
            listObjMemberToAdd.each(function (idx, member) {
                //console.log(member);
                arrGroupUserIDExceptSender.push(parseInt($(member).attr('addUserId')));
            });
            //console.log(arrGroupUserIDExceptSender);
            //
            AJAX_POST_AddNewMemberToGroup(iGroupID, JSON.stringify(arrGroupUserIDExceptSender), loggedUserID);
            
        }
        
    }
    
});
$('#btnGetMessageFile').on('click', function (e) {
    let jqValMessageFile = $('#valMessageFile');
    let jqMessageContent = $('#messageContent');
    //
    jqValMessageFile.click();
    jqMessageContent.attr('disabled', 'disabled');
});
$('#valMessageFile').on('change', function (e) {
    let jqMessageContent = $('#messageContent');
    let jqValMessageFile = $('#valMessageFile');
    let fobjValMessageFile = jqValMessageFile.prop('files')[0];
    //console.log(fobjValMessageFile);
    
    if (fobjValMessageFile.type.indexOf('image') >= 0) {
        jqMessageContent.attr('messageType', 'img');
    }
    else {
        jqMessageContent.attr('messageType', 'file');
    }
    
    jqMessageContent.val(fobjValMessageFile.name);
});
// ========= Event Needed Funtions =========
function NullVal(val) {

    if (val == null || val == undefined || val == '' || val == 'null' || val == 'NULL') {
        return null;
    }
    return val;
};
function GET_FileNameFromPath(_fullFilePath) {
    return _fullFilePath.replace(/^.*[\\/]/, '');
};
function GET_FileExtension(_fileNameOrPath) {
    return _fileNameOrPath.substring(_fileNameOrPath.lastIndexOf('.') + 1);
};
function GET_FileFromServer(_messageDataID) {
    //console.log(_messageDataID);
    //window.location = '/MessageSystem/GET_DownloadMessageFileFromServer/?messageID=' + _messageDataID;
    window.open('/MessageSystem/GET_DownloadMessageFileFromServer/?messageID=' + _messageDataID, '_blank');
};
function Action_SubmitMessageContent() {
    let jqChatFloatIcon = $('#chatFloatIcon');
    let jqChatHeaderRecipient = $('#chatHeaderRecipient');
    let jqMessageContent = $('#messageContent');
    let jqValMessageFile = $('#valMessageFile');

    let sendTarget = jqMessageContent.attr('sendTarget');
    //
    let senderID = jqChatFloatIcon.attr('loggedUserID');
    let senderUserName = jqChatFloatIcon.attr('loggedUserName');
    let messageContent = jqMessageContent.val();
    let messageType = jqMessageContent.attr('messageType');
    let messageFile = jqValMessageFile.prop('files')[0];

    //
    let mqttPublishTopic = '';
    let mqttSenderInfo = jqChatFloatIcon.attr('loggedUserFullName') + ' (' + jqChatFloatIcon.attr('loggedUserName') + ')';
    let mqttRecipientInfo = '';
    //
    if (messageContent.length !== 0 && messageContent !== '' && messageContent !== null && messageContent !== undefined) {
        if (sendTarget === 'single') {
            //
            let recipientID = jqChatHeaderRecipient.attr('singleRecipientID');
            let recipientUserName = jqChatHeaderRecipient.attr('singleRecipientUserName');
            //AJAX_POST_SendMessageBetween2User(senderID, recipientID, messageType, messageContent); 

            AJAX_POST_SendMessageBetween2User(senderID, senderUserName, recipientID, recipientUserName, messageType, messageContent, messageFile);
            //console.log(senderID + ' | ' + recipientID + ' | ' + messageType + ' | ' + messageContent+ ' | ' + messageFile);
            //
            mqttPublishTopic = 'ChatSystem/Single/';
            mqttRecipientInfo = jqChatHeaderRecipient.attr('singleRecipientFullName') + ' (' + jqChatHeaderRecipient.attr('singleRecipientUserName') + ')';
        }
        if (sendTarget === 'group') {
            //
            let groupRecipientGroupName = jqChatHeaderRecipient.attr('groupRecipientGroupName');
            let groupRecipientGroupID = jqChatHeaderRecipient.attr('groupRecipientGroupID');
            let groupRecipientLoggedUserGroupID = jqChatHeaderRecipient.attr('groupRecipientLoggedUserGroupID');
            let strArrGroupUserIDExceptSender = jqChatHeaderRecipient.attr('arrGroupUserIDExceptSender');
            //AJAX_POST_SendMessageToGroup(senderID, strArrGroupUserIDExceptSender, messageType, messageContent, groupRecipientGroupID, groupRecipientLoggedUserGroupID);                
            AJAX_POST_SendMessageToGroup(senderID, senderUserName, groupRecipientGroupName, strArrGroupUserIDExceptSender, messageType, messageContent, messageFile, groupRecipientGroupID, groupRecipientLoggedUserGroupID);
            //
            mqttPublishTopic = 'ChatSystem/Group/';
            mqttRecipientInfo = '';
        }
        //console.log(jqChatHeaderRecipient);
        //
        //AJAX_MQTT_SendNoticeMessage(mqttPublishTopic, mqttSenderInfo, mqttRecipientInfo, messageContent);
        jqMessageContent.val('');
        jqMessageContent.removeAttr('disabled');
    }
};
function TriggerNotification(notiContent) {
    if (!('Notification' in window)) {
        alert('Browser does not support desktop notification');
    }
    Notification.requestPermission().then((permission) => {
        if (permission === 'granted') {
            const notification = new Notification('Smart Factory Notification', {
                body: 'You got ' + notiContent + ' new messages!'
            });
        }
    });

};
function Interval_GET_MessageBetween2Users(_loggedUserID, _recipientID) {
    let liSingleRecipientID = 'singleRecipient' + _recipientID;
    let jqSingleRecipient = $('#' + liSingleRecipientID);
    let singleRecipientData = {
        UserID: jqSingleRecipient.attr('singleRecipientID'),
        UserName: jqSingleRecipient.attr('singleRecipientUserName'),
        FullName: jqSingleRecipient.attr('singleRecipientFullName'),
        RoleName: jqSingleRecipient.attr('singleRecipientRoleName'),
        UnreadNum: parseInt(jqSingleRecipient.attr('singleRecipientUnreadNum'))
    };
    //console.log(singleRecipientData);
    if (singleRecipientData.UnreadNum > 0) {
        AJAX_GET_MessageRecordBetween2Users(_loggedUserID, _recipientID);
    }
    //
};
function Interval_GET_MessageRecordOfUsersInGroup(_groupID, _loggedUserID, _loggedUserGroupID) {
    let liGroupRecipientID = 'groupRecipient' + _groupID;
    let jqGroupRecipient = $('#' + liGroupRecipientID);
    let groupRecipientData = {
        GroupID: jqGroupRecipient.attr('groupRecipientID'),
        LoggedUserGroupID: jqGroupRecipient.attr('groupRecipientLoggedUserGroupID'),
        GroupName: jqGroupRecipient.attr('groupRecipientName'),
        CreateDate: jqGroupRecipient.attr('groupRecipientCreateDate'),
        MemberNum: jqGroupRecipient.attr('groupRecipientMemberNum'),
        UnreadNum: parseInt(jqGroupRecipient.attr('groupRecipientUnreadNum')),
    };
    //console.log(groupRecipientData);
    if (groupRecipientData.UnreadNum > 0) {
        AJAX_GET_MessageRecordOfUsersInGroup(_groupID, _loggedUserID, _loggedUserGroupID);
    }
    //
};
// ========= AJAX Requests URLs =========
var URL_GET_ListAllUser = $('#URL_GET_ListAllUser').val();
var URL_GET_ListUserExceptLoggedUser = $('#URL_GET_ListUserExceptLoggedUser').val();
var URL_GET_ListSingleRecipientOfLoggedUser = $('#URL_GET_ListSingleRecipientOfLoggedUser').val();
var URL_GET_MessageRecordBetween2Users = $('#URL_GET_MessageRecordBetween2Users').val();
var URL_GET_ListGroupContainLoggedUser = $('#URL_GET_ListGroupContainLoggedUser').val();
var URL_GET_ListUserInGroup = $('#URL_GET_ListUserInGroup').val();
var URL_GET_ListUserNotInGroup = $('#URL_GET_ListUserNotInGroup').val();
var URL_GET_MessageRecordOfUsersInGroup = $('#URL_GET_MessageRecordOfUsersInGroup').val();
var URL_GET_MessageUnreadCountOfLoggedUser = $('#URL_GET_MessageUnreadCountOfLoggedUser').val();
//
var URL_POST_SendMessageBetween2User = $('#URL_POST_SendMessageBetween2User').val();
var URL_POST_UpdateUnreadMessagesBetween2User = $('#URL_POST_UpdateUnreadMessagesBetween2User').val();
var URL_POST_SendMessageToGroup = $('#URL_POST_SendMessageToGroup').val();
var URL_POST_UpdateUnreadMessagesInGroup = $('#URL_POST_UpdateUnreadMessagesInGroup').val();
var URL_POST_CreateNewMessageGroup = $('#URL_POST_CreateNewMessageGroup').val();
var URL_POST_AddNewMemberToGroup = $('#URL_POST_AddNewMemberToGroup').val();
//
var URL_MQTT_SendNoticeMessage = $('#URL_MQTT_SendNoticeMessage').val();
// ========= AJAX Functions =========
function AJAX_GET_ListAllUser() {
    try {        
        $.ajax({
            url: URL_GET_ListAllUser,
            method: 'GET',
            data: { },
            success: function (listAllUser) {
                console.log(listAllUser);
                let eleLiUser = '';
                //$('#listSingleRecipient').html(listAllUser);
                $.each(listAllUser, function (idx, user) {
                    eleLiUser += '<li class="clearfix item-single-recipient" singleRecipientId="' + user.UserID + '" singleRecipientUserName="' + user.UserName + '" singleRecipientFullName="' + user.FullName + '" singleRecipientRoleName="' + user.RoleName +'">'+
                    '<div class="about" >'+
                    '<div class="name">' + user.FullName + '</div>'+
                    '<div class="status"> '+user.UserName+' ('+user.RoleName+') </div>'+
                    '</div>'+
                    '</li>';
                });
                //eleLiUser += '</ul>';
                $('#listSingleRecipient').html(eleLiUser);
                //$('.list-single-recipient').html(eleLiUser);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            }
        });
    } catch (e) {
        console.log('AJAX exception: ' + { e: e });
    }
};
function AJAX_GET_ListUserExceptLoggedUser(_loggedUserID) {
    try {
        $.ajax({
            url: URL_GET_ListUserExceptLoggedUser,
            method: 'GET',
            data: { loggedUserId: _loggedUserID },
            success: function (listUserExceptLoggedUser) {
                //console.log(listUserExceptLoggedUser);
                sessionStorage.setItem('listUserExceptLoggedUser', JSON.stringify(listUserExceptLoggedUser));
                //
                let eleLiUser = '';                
                $.each(listUserExceptLoggedUser, function (idx, user) {
                    let decorStatus = 'offline';
                    if ((idx % 2) == 0) {
                        decorStatus = 'online';
                    }                                   

                    eleLiUser += '<li class="clearfix item-recipient single-recipient" singleRecipientId="' + user.UserID + '" singleRecipientUserName="' + user.UserName + '" singleRecipientFullName="' + user.FullName + '" singleRecipientRoleName="' + user.RoleName + '" >' +
                        '<div class="about" >' +
                        '<div class="name">' + user.FullName + '</div>' +
                        '<div class="status"><i class="fa fa-circle ' + decorStatus +'"></i> ' + user.UserName + ' (' + user.RoleName + ') </div>' +
                        '</div>' +
                        '</li>';
                });                
                $('#listSingleRecipient').html(eleLiUser);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {
                $('.single-recipient').on('click', function () {   
                    $('.item-recipient').removeClass('active');
                    $(this).addClass('active');
                    //
                    let loggedUserData = {
                        UserID: $('#chatFloatIcon').attr('loggedUserID'),
                        UserName: $('#chatFloatIcon').attr('loggedUserName'),
                    }
                    let singleRecipientData = {
                        UserID: $(this).attr('singleRecipientID'),
                        UserName: $(this).attr('singleRecipientUserName'),
                        FullName: $(this).attr('singleRecipientFullName'),
                        RoleName: $(this).attr('singleRecipientRoleName'),
                        
                    };
                    //console.log(loggedUserData);
                    //
                    let jqChatHeaderRecipient = $('#chatHeaderRecipient');
                    let chatHeaderRecipientHTML = '<h5 class="fw-bold">' + singleRecipientData.FullName + '</h5><small id="">' + singleRecipientData.UserName + ' (' + singleRecipientData.RoleName + ')</small>';                                        
                    jqChatHeaderRecipient.html(chatHeaderRecipientHTML);
                    AJAX_GET_MessageRecordBetween2Users(loggedUserData.UserID, singleRecipientData.UserID);
                    //
                    jqChatHeaderRecipient.attr('singleRecipientID', singleRecipientData.UserID);
                    jqChatHeaderRecipient.attr('singleRecipientUserName', singleRecipientData.UserName);
                    jqChatHeaderRecipient.attr('singleRecipientFullName', singleRecipientData.FullName);
                    $('#messageContent').attr('sendTarget', 'single');
                    //
                    if (!$('#groupMembersSection').hasClass('d-none')) {
                        $('#groupMembersSection').addClass('d-none')
                    }
                });

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_ListSingleRecipientOfLoggedUser(_loggedUserID) {
    try {
        $.ajax({
            url: URL_GET_ListSingleRecipientOfLoggedUser,
            method: 'GET',
            data: { loggedUserId: _loggedUserID },
            success: function (listSingleRecipientLoggedUser) {
                //console.log(listSingleRecipientLoggedUser);
                sessionStorage.setItem('listSingleRecipientLoggedUser', JSON.stringify(listSingleRecipientLoggedUser));
                //
                let eleLiUser = '';
                $.each(listSingleRecipientLoggedUser, function (idx, recipient) {
                    let decorStatus = 'offline';                    
                    if ((idx % 2) == 0) {
                        decorStatus = 'online';
                    }
                    let unreadStatus = 'primary';
                    if (recipient.UNREAD_NUM != 0) {
                        unreadStatus = 'danger';
                    }
                    let liSingleRecipientID = 'singleRecipient' + recipient.USER_ID;
                    eleLiUser += '<li id="' + liSingleRecipientID + '" class="clearfix item-recipient single-recipient" singleRecipientId="' + recipient.USER_ID + '" singleRecipientUserName="' + recipient.USER_NAME + '" singleRecipientFullName="' + recipient.FULL_NAME + '" singleRecipientRoleName="' + recipient.ROLE_NAME + '" singleRecipientUnreadNum = "' + recipient.UNREAD_NUM + '" >' +
                        '<div class="about" >' +
                        '<div class="name">' + recipient.FULL_NAME + ' <span class="fw-bold text-' + unreadStatus +'">(' + recipient.UNREAD_NUM+')</span></div>' +
                        '<div class="status"><i class="fa fa-circle ' + decorStatus + '"></i> ' + recipient.USER_NAME + ' (' + recipient.ROLE_NAME + ') </div>' +
                        '</div>' +
                        '</li>';
                });
                $('#listSingleRecipient').html(eleLiUser);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {
                $('.single-recipient').on('click', function () {
                    gblIsChatHistoryFirstLoad = true;
                    $('.item-recipient').removeClass('active');
                    $(this).addClass('active');
                    //
                    let loggedUserData = {
                        UserID: $('#chatFloatIcon').attr('loggedUserID'),
                        UserName: $('#chatFloatIcon').attr('loggedUserName'),
                    }
                    let singleRecipientData = {
                        UserID: $(this).attr('singleRecipientID'),
                        UserName: $(this).attr('singleRecipientUserName'),
                        FullName: $(this).attr('singleRecipientFullName'),
                        RoleName: $(this).attr('singleRecipientRoleName'),
                        UnreadNum: parseInt($(this).attr('singleRecipientUnreadNum'))
                    };
                    //console.log(loggedUserData);
                    //
                    let jqChatHeaderRecipient = $('#chatHeaderRecipient');
                    let chatHeaderRecipientHTML = '<h5 class="fw-bold">' + singleRecipientData.FullName + '</h5><small id="">' + singleRecipientData.UserName + ' (' + singleRecipientData.RoleName + ')</small>';
                    jqChatHeaderRecipient.html(chatHeaderRecipientHTML);
                    //
                    AJAX_GET_MessageRecordBetween2Users(loggedUserData.UserID, singleRecipientData.UserID);                    
                    clearInterval(gblIntervalID_GET_messageInGroup);  
                    clearInterval(gblIntervalID_GET_messageOf2User);
                    //
                    if (singleRecipientData.UnreadNum > 0) {
                        AJAX_POST_UpdateUnreadMessagesBetween2User(loggedUserData.UserID, singleRecipientData.UserID);
                        
                    }
                    gblIntervalID_GET_messageOf2User = setInterval(Interval_GET_MessageBetween2Users, 1 * 1000, loggedUserData.UserID, singleRecipientData.UserID);
                    //
                    jqChatHeaderRecipient.attr('recipientTarget', 'single');
                    jqChatHeaderRecipient.attr('singleRecipientID', singleRecipientData.UserID);
                    jqChatHeaderRecipient.attr('singleRecipientUserName', singleRecipientData.UserName);
                    jqChatHeaderRecipient.attr('singleRecipientFullName', singleRecipientData.FullName);
                    $('#messageContent').attr('sendTarget', 'single');
                    //
                    if (!$('#groupMembersSection').hasClass('d-none')) {
                        $('#groupMembersSection').addClass('d-none')
                    }
                });

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_MessageRecordBetween2Users(_loggedUserID, _recipientID) {
    try {
        $.ajax({
            url: URL_GET_MessageRecordBetween2Users,
            method: 'GET',
            data: { senderID: _loggedUserID, recipientID: _recipientID },
            success: function (messageRecordBetween2Users) {
                //console.log(messageRecordBetween2Users);
                let chatHistoryContentHTML = '';
                $.each(messageRecordBetween2Users, function (idx, messageData) {
                    let messageSendDate = moment(new Date(parseInt(messageData.SEND_DATE.substr(6)))).format('YYYY/MM/DD HH:mm');
                    if (messageData.SENDER_ID == _loggedUserID) {
                        if (messageData.MESSAGE_TYPE === 'text') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data text-right"><span class="message-data-time text-success">' + messageSendDate + '</span></div><div class="message my-message float-right">' + messageData.MESSAGE_CONTENT + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'file') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data text-right"><span class="message-data-time text-success">' + messageSendDate + '</span></div><div class="message my-message message-file float-right" onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><i class="fas fa-file-invoice"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) +'</div></li>';
                        }  
                        if (messageData.MESSAGE_TYPE === 'img') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data text-right"><span class="message-data-time text-success">' + messageSendDate + '</span></div><div class="message my-message message-file float-right " onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><img src="' + messageData.MESSAGE_CONTENT + '" id="" class="w-100" /><i class="fas fa-file-image"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                    }
                    else {                        
                        if (messageData.MESSAGE_TYPE === 'text') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data "><span class="message-data-time ">' + messageSendDate + '</span></div><div class="message other-message ">' + messageData.MESSAGE_CONTENT + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'file') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data "><span class="message-data-time ">' + messageSendDate + '</span></div><div class="message other-message message-file " onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><i class="fas fa-file-invoice"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'img') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data "><span class="message-data-time ">' + messageSendDate + '</span></div><div class="message other-message message-file " onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><img src="' + messageData.MESSAGE_CONTENT + '" id="" class="w-100" /><i class="fas fa-file-image"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                    }
                });
                $('#chatHistoryContent').html(chatHistoryContentHTML);
                //if (gblIsChatHistoryFirstLoad) {                    
                let chatHistoryContainer = document.getElementById('chatHistory');
                chatHistoryContainer.scrollTop = chatHistoryContainer.scrollHeight;
                //console.log($('#chatHistory'));
                gblIsChatHistoryFirstLoad = false;
                //}                
                
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {                
                
            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_ListGroupContainLoggedUser(_loggedUserID) {
    try {
        $.ajax({
            url: URL_GET_ListGroupContainLoggedUser,
            method: 'GET',
            data: { loggedUserID: _loggedUserID },
            success: function (listGroupContainLoggedUser) {
                //console.log(listGroupContainLoggedUser);
                let eleLiGroup = '';
                $.each(listGroupContainLoggedUser, function (idx, group) {
                    let decorStatus = 'offline';
                    if ((idx % 2) == 0) {
                        decorStatus = 'online';
                    }
                    let unreadStatus = 'primary';
                    if (group.UNREAD_NUM != 0) {
                        unreadStatus = 'danger';
                    }
                    //
                    let liGroupRecipientID = 'groupRecipient' + group.GROUP_ID;
                    eleLiGroup += '<li id="' + liGroupRecipientID +'" class="clearfix item-recipient group-recipient" groupRecipientID="' + group.GROUP_ID + '" groupRecipientLoggedUserGroupID = "' + group.RECIPIENT_GROUP_ID + '" groupRecipientName="' + group.GROUP_NAME + '" groupRecipientMemberNum="' + group.MEMBER_NUM + '" groupRecipientCreateDate="' + group.CREATE_DATE + '" groupRecipientUnreadNum="' + group.UNREAD_NUM + '" messageType="group">' +
                        '<div class="about" >' +
                        '<div class="name">' + group.GROUP_NAME + ' <span class="fw-bold text-' + unreadStatus + '">(' + group.UNREAD_NUM +')</span> </div>' +
                        '<div class="status"><i class="fa fa-circle ' + decorStatus + '"></i> ' + group.MEMBER_NUM + ' Member(s) </div>' +
                        '</div>' +
                        '</li>';

                });
                $('#listGroupRecipient').html(eleLiGroup);
                
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {
                $('.group-recipient').on('click', function () {
                    gblIsChatHistoryFirstLoad = true;
                    $('.item-recipient').removeClass('active');
                    $(this).addClass('active');
                    //
                    let loggedUserData = {
                        UserID: $('#chatFloatIcon').attr('loggedUserID'),
                        UserName: $('#chatFloatIcon').attr('loggedUserName'),
                    }
                    let groupRecipientData = {
                        GroupID: $(this).attr('groupRecipientID'),
                        LoggedUserGroupID: $(this).attr('groupRecipientLoggedUserGroupID'),
                        GroupName: $(this).attr('groupRecipientName'),
                        CreateDate: $(this).attr('groupRecipientCreateDate'),
                        MemberNum: $(this).attr('groupRecipientMemberNum'),
                        UnreadNum: parseInt($(this).attr('groupRecipientUnreadNum')),
                    };                    
                    //
                    AJAX_GET_ListUserInGroup(groupRecipientData.GroupID, loggedUserData.UserID);                    
                    $('#messageContent').attr('sendTarget', 'group');                    
                    let chatHeaderRecipientHTML = '<h5 class="fw-bold">' + groupRecipientData.GroupName + '</h5><small id="">' + groupRecipientData.MemberNum + ' Member(s) </small>';
                    $('#chatHeaderRecipient').attr('recipientTarget', 'group');
                    $('#chatHeaderRecipient').attr('groupRecipientGroupName', groupRecipientData.GroupName);
                    $('#chatHeaderRecipient').attr('groupRecipientGroupID', groupRecipientData.GroupID);
                    $('#chatHeaderRecipient').attr('groupRecipientLoggedUserGroupID', groupRecipientData.LoggedUserGroupID);
                    $('#chatHeaderRecipient').html(chatHeaderRecipientHTML);
                    //
                    AJAX_GET_MessageRecordOfUsersInGroup(groupRecipientData.GroupID, loggedUserData.UserID, groupRecipientData.LoggedUserGroupID);
                    clearInterval(gblIntervalID_GET_messageOf2User);
                    clearInterval(gblIntervalID_GET_messageInGroup);
                    
                    //
                    if (groupRecipientData.UnreadNum != 0) {
                        AJAX_POST_UpdateUnreadMessagesInGroup(groupRecipientData.GroupID, loggedUserData.UserID);
                        
                    }
                    gblIntervalID_GET_messageInGroup = setInterval(Interval_GET_MessageRecordOfUsersInGroup, 1 * 1000, groupRecipientData.GroupID, loggedUserData.UserID, groupRecipientData.LoggedUserGroupID);
                    //
                    let containerGroupMember = $('.container-group-member');
                    let containerAddMember = $('.container-add-member');
                    let containerInputGroupName = $('#containerInputGroupName');
                    if ($('#groupMembersSection').hasClass('d-none')) {
                        $('#groupMembersSection').removeClass('d-none');

                        if (containerGroupMember.hasClass('d-none')) {
                            containerGroupMember.removeClass('d-none');
                        }
                        if (!containerAddMember.hasClass('d-none')) {
                            containerInputGroupName.addClass('d-none');
                            containerAddMember.addClass('d-none');
                        }
                    }
                    /*let chatHeaderExtension = '<a href="javascript:void(0);" class="btn "><i class="fa fa-user-plus"></i></a>';
                    $('#chatHeaderExtension').html(chatHeaderExtension);*/
                    //
                    if (!$('.container-add-member').hasClass('d-none')) {
                        $('#containerInputGroupName').addClass('d-none');
                        $('.container-add-member').addClass('d-none');
                        $('.container-group-member').removeClass('d-none');
                    }
                });
            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_ListUserInGroup(_groupID, _loggedUserID) {
    try {
        $.ajax({
            url: URL_GET_ListUserInGroup,
            method: 'GET',
            data: { groupID: _groupID },
            success: function (listUserInGroup) {
                //console.log(listUserInGroup);
                let eleLiGroupMember = '';
                let arrGroupUserIDExceptSender = [];
                $.each(listUserInGroup, function (idx, user) {
                    //
                    let decorStatus = 'online';
                    if (user.USER_ID != _loggedUserID) {                        
                        arrGroupUserIDExceptSender.push(user.USER_GROUP_ID);                        
                    }
                    //
                    if (user.USER_ID == _loggedUserID) {
                        decorStatus = 'me';                        
                    }                   
                    eleLiGroupMember += '<li class="clearfix item-addmember " memberUserId="' + user.USER_ID + '" memberUserName="' + user.USER_NAME + '" memberFullName="' + user.FULL_NAME + '" memberRoleName="' + user.ROLE_NAME + '" memberGroupRole ="' + user.ROLE_IN_GROUP + '">' +
                        '<div class="about" >' +
                        '<div class="name">' + user.FULL_NAME + '</div>' +
                        '<div class="status"><i class="fa fa-circle ' + decorStatus + '"></i> ' + user.USER_NAME + ' (' + user.ROLE_IN_GROUP + ') </div>' +
                        '</div>' +
                        '</li>';
                });
                $('#chatHeaderRecipient').attr('arrGroupUserIDExceptSender', JSON.stringify(arrGroupUserIDExceptSender));
                $('#listGroupMember').html(eleLiGroupMember);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {
                
            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_ListUserNotInGroup(_groupID, _loggedUserID) {
    try {
        $.ajax({
            url: URL_GET_ListUserNotInGroup,
            method: 'GET',
            data: { groupID: _groupID, loggedUserID: _loggedUserID },
            success: function (listUserNotInGroup) {
                console.log(listUserNotInGroup);
                let eleLiUser = '';
                $.each(listUserNotInGroup, function (idx, user) {
                    let decorStatus = 'offline';
                    if ((idx % 2) == 0) {
                        decorStatus = 'online';
                    }

                    eleLiUser += '<li class="clearfix item-addmember " addUserId="' + user.USER_ID + '" addUserName="' + user.USER_NAME + '" addUserFullName="' + user.FULL_NAME + '" addUserRoleName="' + user.ROLE_NAME + '" >' +
                        '<div class="about" >' +
                        '<div class="name">' + user.FULL_NAME + '</div>' +
                        '<div class="status"><i class="fa fa-circle ' + decorStatus + '"></i> ' + user.USER_NAME + ' (' + user.ROLE_NAME + ') </div>' +
                        '</div>' +
                        '</li>';
                });
                $('#listAddMember').html(eleLiUser);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {
                $('.item-addmember').on('click', function (e) {
                    let thisEle = $(this);
                    if (thisEle.hasClass('active')) {
                        thisEle.removeClass('active');
                    }
                    else {
                        thisEle.addClass('active');
                    }
                    let selectedItemMember = $('.item-addmember.active').length;
                    $('#selectedMemberNum').html(selectedItemMember);
                });
            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_MessageRecordOfUsersInGroup(_groupID, _loggedUserID, _loggedUserGroupID) {
    try {
        $.ajax({
            url: URL_GET_MessageRecordOfUsersInGroup,
            method: 'GET',
            data: { groupID: _groupID, senderID: _loggedUserID, loggedUserGroupID: _loggedUserGroupID },
            success: function (messageRecordOfUsersInGroup) {
                //console.log(messageRecordOfUsersInGroup);
                let chatHistoryContentHTML = '';
                $.each(messageRecordOfUsersInGroup, function (idx, messageData) {
                    
                    let dateSendDate = moment(new Date(parseInt(messageData.SEND_DATE.substr(6)))).format('YYYY/MM/DD HH:mm');
                    let messageSendDateAndUser = messageData.SENDER_NAME + ', ' + dateSendDate;
                    if (messageData.SENDER_ID == _loggedUserID) {
                        messageSendDateAndUser = dateSendDate + ', ' + messageData.SENDER_NAME;
                        
                        if (messageData.MESSAGE_TYPE === 'text') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data text-right"><span class="message-data-time text-success">' + messageSendDateAndUser + '</span></div><div class="message my-message float-right">' + messageData.MESSAGE_CONTENT + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'file') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data text-right"><span class="message-data-time text-success">' + messageSendDateAndUser + '</span></div><div class="message my-message message-file float-right" onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><i class="fas fa-file-invoice"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'img') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data text-right"><span class="message-data-time text-success">' + messageSendDateAndUser + '</span></div><div class="message my-message message-file float-right" onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><img id="" class="w-100 text-warning" src="' + messageData.MESSAGE_CONTENT + '" alt="404 Not Found!" /><i class="fas fa-file-image"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                    } else {
                        
                        if (messageData.MESSAGE_TYPE === 'text') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data "><span class="message-data-time ">' + messageSendDateAndUser + '</span></div><div class="message other-message ">' + messageData.MESSAGE_CONTENT + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'file') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data "><span class="message-data-time ">' + messageSendDateAndUser + '</span></div><div class="message other-message message-file " onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><i class="fas fa-file-invoice"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                        if (messageData.MESSAGE_TYPE === 'img') {
                            chatHistoryContentHTML += '<li class="clearfix"><div class="message-data "><span class="message-data-time ">' + messageSendDateAndUser + '</span></div><div class="message other-message message-file " onclick="GET_FileFromServer(' + messageData.MESSAGE_ID + ')"><img id="" class="w-100 text-danger" src="' + messageData.MESSAGE_CONTENT + '" alt="404 Not Found!"  /><i class="fas fa-file-image"></i> ' + GET_FileNameFromPath(messageData.MESSAGE_CONTENT) + '</div></li>';
                        }
                    }
                });
                $('#chatHistoryContent').html(chatHistoryContentHTML);
    
                let chatHistoryContainer = document.getElementById('chatHistory');
                chatHistoryContainer.scrollTop = chatHistoryContainer.scrollHeight;
                
                gblIsChatHistoryFirstLoad = false;

            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {
                
            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_GET_MessageUnreadCountOfLoggedUser(_loggedUserID) {
    try {
        $.ajax({
            url: URL_GET_MessageUnreadCountOfLoggedUser,
            method: 'GET',
            data: { loggedUserID: _loggedUserID },
            success: function (iUnreadCount) {      
                let objChatFloatIconUnreadNum = $('#chatFloatIconUnreadNum');                
                
                objChatFloatIconUnreadNum.html(iUnreadCount);
                if ((iUnreadCount !== 0) && (typeof iUnreadCount === 'number') && (gbliUnreadCount !== iUnreadCount)) {
                    TriggerNotification(iUnreadCount);
                    gbliUnreadCount = iUnreadCount;
                }
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
//
function AJAX_POST_SendMessageBetween2User(_senderID, _senderUserName, _recipientID, _recipientUserName, _messageType, _messageContent, _messageFile) {

    try {
        let formDataSendMessage = new FormData();
        formDataSendMessage.append('senderID', _senderID);
        formDataSendMessage.append('senderUserName', _senderUserName);
        formDataSendMessage.append('recipientID', _recipientID);
        formDataSendMessage.append('recipientUserName', _recipientUserName);
        formDataSendMessage.append('messageType', _messageType);
        formDataSendMessage.append('messageContent', _messageContent);
        formDataSendMessage.append('messageFile', _messageFile);
        //console.log(formDataSendMessage);
        //
        $.ajax({
            url: URL_POST_SendMessageBetween2User,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            //data: { senderID: _senderID, recipientID: _recipientID, messageType: _messageType, messageContent: _messageContent },
            data: formDataSendMessage,
            success: function (messageSentResult) {
                console.log(messageSentResult);
                AJAX_GET_MessageRecordBetween2Users(_senderID, _recipientID);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_POST_UpdateUnreadMessagesBetween2User(_loggedUserID, _recipientID) {
    try {
        $.ajax({
            url: URL_POST_UpdateUnreadMessagesBetween2User,
            method: 'POST',
            data: { loggedUserID: _loggedUserID, recipientID: _recipientID },
            success: function (messageUpdatedResult) {
                //console.log(messageUpdatedResult);
                if (messageUpdatedResult) {
                    AJAX_GET_ListSingleRecipientOfLoggedUser(_loggedUserID);
                    AJAX_GET_MessageUnreadCountOfLoggedUser(_loggedUserID);
                }
                else {
                    console.log('Cannot update unread status: \n' + messageUpdatedResult);
                }
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_POST_SendMessageToGroup(_senderID, _senderUserName, _groupName, _strArrGroupUserIDExceptSender, _messageType, _messageContent, _messageFile, _groupID, _loggedUserGroupID) {
    try {

        let formDataSendMessage = new FormData();
        formDataSendMessage.append('senderID', _senderID);
        formDataSendMessage.append('senderUserName', _senderUserName);
        formDataSendMessage.append('groupName', _groupName);
        formDataSendMessage.append('strArrGroupUserIDExceptSender', _strArrGroupUserIDExceptSender);
        formDataSendMessage.append('messageType', _messageType);
        formDataSendMessage.append('messageContent', _messageContent);
        formDataSendMessage.append('messageFile', _messageFile);

        $.ajax({
            url: URL_POST_SendMessageToGroup,
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            //data: { senderID: _senderID, strArrGroupUserIDExceptSender: _strArrGroupUserIDExceptSender, messageType: _messageType, messageContent: _messageContent },
            data: formDataSendMessage,
            success: function (messageSentResult) {
                if (messageSentResult) {
                    AJAX_GET_MessageRecordOfUsersInGroup(_groupID, _senderID, _loggedUserGroupID);
                }
                else {
                    toastr.error('Error on getting message!', 'Error');
                }
                
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_POST_UpdateUnreadMessagesInGroup(_groupID, _loggedUserID) {
    try {
        $.ajax({
            url: URL_POST_UpdateUnreadMessagesInGroup,
            method: 'POST',
            data: { groupID: _groupID, loggedUserID: _loggedUserID },
            success: function (messageUpdatedResult) {
                //console.log(messageUpdatedResult);
                if (messageUpdatedResult) {
                    AJAX_GET_ListGroupContainLoggedUser(_loggedUserID);
                    AJAX_GET_MessageUnreadCountOfLoggedUser(_loggedUserID);
                }
                else {
                    console.log('Cannot update unread status: \n' + messageUpdatedResult);
                }
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_POST_CreateNewMessageGroup(_loggedUserID, _strArrGroupUserIDExceptSender, _groupName) {
    try {
        $.ajax({
            url: URL_POST_CreateNewMessageGroup,
            method: 'POST',
            data: { loggedUserID: _loggedUserID, strArrGroupUserIDExceptSender: _strArrGroupUserIDExceptSender, groupName: _groupName},
            success: function (groupCreatedResult) {
                if (groupCreatedResult) {
                    AJAX_GET_ListGroupContainLoggedUser(_loggedUserID);
                }
                else {
                    console.log(groupCreatedResult);
                }
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
function AJAX_POST_AddNewMemberToGroup(_groupID, _strArrGroupUserIDExceptSender, _loggedUserID) {
    try {
        $.ajax({
            url: URL_POST_AddNewMemberToGroup,
            method: 'POST',
            data: { groupID: _groupID, strArrGroupUserIDExceptSender: _strArrGroupUserIDExceptSender },
            success: function (memberAddedResult) {
                if (memberAddedResult) {
                    AJAX_GET_ListGroupContainLoggedUser(_loggedUserID);
                    AJAX_GET_ListUserInGroup(_groupID, _loggedUserID);
                    AJAX_GET_ListUserNotInGroup(_groupID, _loggedUserID);
                    $('#selectedMemberNum').html('0');
                }
                else {
                    console.log(groupCreatedResult);
                }
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};
//
function AJAX_MQTT_SendNoticeMessage(_mqttPublishTopic, _senderInfo, _recipientInfo, _messageContent) {
    try {
        $.ajax({
            url: URL_MQTT_SendNoticeMessage,
            data: { mqttPublishTopic: _mqttPublishTopic, senderInfo: _senderInfo, recipientInfo: _recipientInfo, messageContent: _messageContent },
            success: function (mqttSentNoticeResult) {
                //console.log(mqttSentNoticeResult);
            },
            error: function (error) {
                console.log('Error on calling function: ' + error);
            },
            complete: function () {

            },
        });
    } catch (e) {
        console.log('AJAX exception: ');
        console.log(e);
    }
};

