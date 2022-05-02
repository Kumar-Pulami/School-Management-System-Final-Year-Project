$(document).ready(function () {
    $("#Batch").change(function () {
        $.getJSON("/Marksheet/GetGradeListJson", { batch: $("#Batch").val() }, function (data) {
            $("#Grade").empty();
            $("#Grade").append("<option>Select Grade</option>");
            $("#TerminalID").empty();
            $("#TerminalID").append("<option>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#Grade").append("<option>" + row + "</option>");
            });
        });
    });


    $("#Grade").change(function () {
        $.getJSON("/ClassTerminal/GetNonAssignedTerminalJSON", { batch: $("#Batch").val(), grade: $("#Grade").val() }, function (data) {
            $("#TerminalID").empty();
            $("#TerminalID").append("<option>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#TerminalID").append("<option value='" + row.terminalID + "'>" + row.terminalName + "</option>");
            });
        });
    });
});