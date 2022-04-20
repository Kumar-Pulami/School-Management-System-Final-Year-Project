$(document).ready(function () {
    $("#Batch").change(function () {
        $.getJSON("/Marksheet/GetGradeListJson", { batch: $("#Batch").val() }, function (data) {
            $("#Grade").empty();
            $("#Grade").append("<option>Select Grade</option>");
            $("#Section").empty();
            $("#Section").append("<option>Select Section</option>");
            $.each(data, function (index, row) {
                $("#Grade").append("<option>" + row + "</option>");
            });
        });
    });


    $("#Grade").change(function () {
        $.getJSON("/Marksheet/GetSectionListJson", { batch: $("#Batch").val(), grade: $("#Grade").val() }, function (data) {
            $("#Section").empty();
            $("#Section").append("<option>Select Section</option>");
            $.each(data, function (index, row) {
                $("#Section").append("<option>" + row + "</option>");
            });
        });
    });
});