$(document).ready(function () {
    $("#Batch").change(function () {
        $.getJSON("/Result/GetGradeListJson", { batch: $("#Batch").val() }, function (data) {
            $("#Grade").empty();
            $("#Grade").append("<option>Select Grade</option>");
            $("#Terminal").empty();
            $("#Terminal").append("<option>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#Grade").append("<option>" + row + "</option>");
            });
        });
    });


    $("#Grade").change(function () {
        $.getJSON("/Result/GetTerminalListJson", { batch: $("#Batch").val(), grade: $("#Grade").val() }, function (data) {
            $("#Terminal").empty();
            $("#Terminal").append("<option>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#Terminal").append("<option value='" + row.terminalID + "'>" + row.terminalName +"</option>");
            });
        });                
    });
});