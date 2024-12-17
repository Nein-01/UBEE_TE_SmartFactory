
var dltModal = $('#deleteModal');
var DltConfirm = function (id, name, prename) {
    var _id = parseInt(id);
    dltModal.find('.idValModal').val(_id);
    dltModal.find('.idTxtModal').text(_id);
    
    dltModal.find('.nameTxtModal').text(name);
    if (prename != undefined) {
        dltModal.find('.nameTxtModal').text(prename + '_' + name);
    }
    
}

$('#CancelBtn').click(function(){
    dltModal.find('.idValModal').val('');
    dltModal.find('.idTxtModal').text('');
    dltModal.find('.nameTxtModal').text('');
})