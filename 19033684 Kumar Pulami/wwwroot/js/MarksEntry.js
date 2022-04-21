$(document).ready(function () {
    $('#updateButton').click(function () {
        console.log("clicked");
        var model = {};
        model["Batch"] = $('#Batch').val();
        model["Grade"] = $('#Grade').val();
        model["Terminal"] = $('#Terminal').val();
        var list = [];        
        $('#marksTable > tbody  > tr').each(function (index, row) {
            var std = {};
            var studentID = $(this).attr("id");
            var subjectList = [];
            $(this.children).each(function (i, cell) {
                if (i >= 1) {
                    var subjectMark = {};
                    var subjectID = $(this).attr("id");
                    var mark; 
                    subjectMark["SubjectID"] = subjectID;
                    $(this.children).each(function (i, cell) {
                        if ($(this).is('input:text')) {
                            mark = $(this).val();
                        }
                    });
                    subjectMark["Mark"] = mark;
                    subjectList.push(subjectMark)
                }
            });
            std["StudentID"] = studentID;
            std["SubjectMarks"] = subjectList;
            list.push(std);
        });
        model["StudentList"] = list;
        console.log(model);
    });
});