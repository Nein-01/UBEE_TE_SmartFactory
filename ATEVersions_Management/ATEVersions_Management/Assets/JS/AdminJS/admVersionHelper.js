//====== Auto add number tag ======
$('#evtReleaseNote').on('keypress', function (e) {
    var key = e.which;
    var thisVal = [];
    var valList = $('#evtReleaseNote').val();
    thisVal = valList.split('\n');
    if (key == 13) {
        var cutVal = thisVal[thisVal.length - 1];
        var count = parseInt(cutVal.substring(0, cutVal.indexOf("."))) + 1;
        //console.log(count);
        valList += '\n' + count + ". ";
        e.preventDefault();
        $('#evtReleaseNote').val(valList);
    }
});
$('#evtReleaseNote').on('change', function (e) {
    var releaseNote = $('#evtReleaseNote').val();
    if (releaseNote.length == 0) {
        $('#evtReleaseNote').val("1. ");
    }
});

//====== Autofill version's name at start ======
$(function () {
    $('#evtProgramList').change(function () {

        var prgFullname = $('#evtProgramList option:selected').text();
        var prgVal = $('#evtProgramList option:selected').val();
        var rsl = 'V1.';
        if (prgVal == 0)
            rsl = "";
        $('#verName').val(rsl);

    });
});
//====== Autofill releaseTime when buildeTime is selected ======
$('#evtBuildTime').on('change', function (e) {
    var buildTime = $('#evtBuildTime').val();
    $('#evtReleaseTime').val(buildTime);
});