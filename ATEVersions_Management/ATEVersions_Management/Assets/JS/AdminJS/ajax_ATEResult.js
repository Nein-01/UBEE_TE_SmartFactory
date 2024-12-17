//ATECheckList/ChangeResult
$(function () {

    $('.editResult').change(function () {

        var self = $(this);
        var ateID = $('#ateID').val();
        var getUrl = $('#ChangeResultUrl').val();
        var itemID = self.data('id');
        var rsl = self.val();

        /*alert(ateID + ' / '+ itemID + ' / ' + rsl)*/
        $.ajax({
            url: getUrl,
            data: { ateId: ateID, itemId: itemID, eResult: rsl },
            dataType: "json",
            type: "POST",
            success: function (response) {
                console.log(response);
                alert("Change success");
            },
            error: function (response) {
                alert(response + ' fail to active with: ' + ateID + ' / ' + itemID + ' / ' + rsl);
            }
        });
    });

});
//ATECheckList/ChangeResultAll
$(function () {

    $('#fullResult').change(function () {

        var self = $(this);
        var ateID = $('#ateID').val();
        var getUrl = $('#ChangeResultAllUrl').val();
        var rsl = self.val();
        $.ajax({
            url: getUrl,
            data: { ateId: ateID, eResult: rsl },
            dataType: "json",
            type: "POST",
            success: function (response) {
                console.log(response);
                $('.editResult').val(rsl);
                alert("Change success");
            },
            error: function (response) {
                alert(response);
            }
        });
    });
});

//Auto fill version name
$(function () {
    $(function () {
        $('#evtProgramList').change(function () {
            var data = $('#evtProgramList option:selected').text();
            var rsl = data.trim() + '_V';
            var sw = data.substring(4, 7).trim() + 'HSW';
            $('#verName').val(rsl);
            $('#swInput').val(sw);

        });
    });
});