$(document).ready(function () {
    $("input[id^='MainContent_DGFullLooseLoad_txtTime']").timepicker({});
    $("input[id^='MainContent_DGFullLooseLoad_txtDOTime']").timepicker({});

    $("#MainContent_txtFromDate").datepicker({ minDate: new Date(2010, 1, 1), dateFormat: "dd/mm/yy",
        onSelect: function (dateText) {
            __doPostBack('MainContent_txtFromDate', dateText);
        }
    });

    $("#MainContent_txtToDate").datepicker({ minDate: new Date(2010, 1, 1), dateFormat: "dd/mm/yy",
        onSelect: function (dateText) {
            __doPostBack('MainContent_txtToDate', dateText);
        }
    });

});