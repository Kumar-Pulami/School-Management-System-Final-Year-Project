$('#printPDF').click(function () {
    $('#printsection').printThis({
        debug: false,
        ImportCSS: true,
        ImportStyle: true,
        printContainer: true,
        loadCSS: "",
        pagelitle: "Print Document",
        removeInLine: false,
        printDelay: 333,
        header: null,
        footer: null,
        formValues: false,
        canvas: false,
        base: false,
        doctypestring: '<DOCTYPE html>',
        removeScripts: false,
        copyTagClasses: false,
    });
})