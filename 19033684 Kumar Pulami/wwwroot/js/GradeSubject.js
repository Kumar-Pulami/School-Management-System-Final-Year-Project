$(document).ready(function () {
    $("#Batch").change(function () {
        $.getJSON("/Marksheet/GetGradeListJson", { batch: $("#Batch").val() }, function (data) {
            $("#Grade").empty();
            $("#Grade").append("<option>Select Grade</option>");
            $("#SubjectID").empty();
            $("#SubjectID").append("<option>Select Subject</option>");
            $.each(data, function (index, row) {
                $("#Grade").append("<option>" + row + "</option>");
            });
        });
    });


    $("#Grade").change(function () {
        $.getJSON("/ClassSubject/GetNonAssignedSubjectJSON", { batch: $("#Batch").val(), grade: $("#Grade").val() }, function (data) {
            $("#SubjectID").empty();
            $("#SubjectID").append("<option>Select Subject</option>");
            $.each(data, function (index, row) {
                $("#SubjectID").append("<option value='" + row.subjectID + "'>" + row.subjectName + "</option>");
            });
        });
    });
});