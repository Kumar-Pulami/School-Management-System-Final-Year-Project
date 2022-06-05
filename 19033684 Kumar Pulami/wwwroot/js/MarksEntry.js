$(document).ready(function () {
    $('#updateButton').click(function () {
        console.log("clicked");
        var model = {};
        model["Batch"] = $('#Batch').val();
        model["Grade"] = $('#Grade').val();
        model["Section"] = $('#Section').val();
        model["Terminal"] = $('#Terminal').val();
        var list = [];
        //Rows
        $('#marksTable > tbody  > tr').each(function (index, row) {
            var std = {};
            var studentID = $(this).attr("id");
            var subjectList = [];
            //cell
            $(this.children).each(function (i, cell) {
                if (i >= 1) {
                    var subjectMark = {};
                    var subjectID = $(this).attr("id");
                    var mark = $(this).find('input:text').val();
                    subjectMark["SubjectID"] = subjectID;
                    subjectMark["Mark"] = mark;
                    subjectList.push(subjectMark)
                }
            });
            std["StudentID"] = studentID;
            std["SubjectMarks"] = subjectList;
            list.push(std);
        });
        model["StudentList"] = list;

        $.ajax({
            type: "POST",
            url: "/MarksEntry/MarksEntry",
            data: model,
            success: function (response) {
                if (response.redirect == "Index") {
                    window.location.href = response.redirect;
                } else {
                    window.location.reload();
                }                
            },
            failure: function (response) {
                window.location.href = "Index";
            },
            error: function (response) {
                window.location.href = "Index";
            },
        });
    });

    $("#Batch").change(function () {
        $.getJSON("/Marksheet/GetGradeListJson", { batch: $("#Batch").val() }, function (data) {
            $("#Grade").empty();
            $("#Grade").append("<option>Select Grade</option>");
            $("#Section").empty();
            $("#Section").append("<option>Select Section</option>");
            $("#Terminal").empty();
            $("#Terminal").append("<option value='0'>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#Grade").append("<option>" + row + "</option>");
            });
        });
    });


    $("#Grade").change(function () {
        $.getJSON("/Marksheet/GetSectionListJson", { batch: $("#Batch").val(), grade: $("#Grade").val() }, function (data) {
            $("#Section").empty();
            $("#Section").append("<option>Select Section</option>");
            $("#Terminal").empty();
            $("#Terminal").append("<option value='0'>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#Section").append("<option>" + row + "</option>");
            });
        });
    });


    $("#Section").change(function () {
        $.getJSON("/MarksEntry/GetTerminalJSON", { batch: $("#Batch").val(), grade: $("#Grade").val() }, function (data) {
            $("#Terminal").empty();
            $("#Terminal").append("<option value='0'>Select Terminal</option>");
            $.each(data, function (index, row) {
                $("#Terminal").append("<option value='" + row.terminalID + "'>" + row.terminalName + "</option>");
            });
        });
    });
});