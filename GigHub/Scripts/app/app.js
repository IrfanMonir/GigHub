var AttendenceService = function () {
    var createAttendence = function (gigId,done,fail) {
        $.post("/api/attendences", { gigId: gigId })
            .done(done)
            .fail(fail);
    };

    var deleteAttendence = function (gigId,done,fail) {
        $.ajax({
            url: "/api/attendences/" + gigId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };
    return {
        createAttendence: createAttendence,
        deleteAttendence: deleteAttendence
    }
}();

var GigsController = function (attendenceService) {
    var button;
    var init = function () {
        $(".js-toggle-attendence").click(toggleAttendence);       
    };

    var toggleAttendence = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");
        if (button.hasClass("btn-default"))
            attendenceService.createAttendence(gigId, done,fail);
        else 
            attendenceService.deleteAttendence(gigId, done, fail);
        
    };

    

    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function () {
        alert("Something failed");
    };
    return {
        init: init
    }
}(AttendenceService);
