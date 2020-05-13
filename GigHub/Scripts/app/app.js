var GigsController = function () {
    var button;
    var init = function () {
        $(".js-toggle-attendence").click(toggleAttendence);       
    };

    var toggleAttendence = function (e) {
        button = $(e.target);
        if (button.hasClass("btn-default"))
            createAttendence();
        else 
            deleteAttendence();
        
    };

    var createAttendence = function () {
        $.post("/api/attendences", { gigId: button.attr("data-gig-id") })
            .done(done)
            .fail(fail);
    };

    var deleteAttendence = function () {
        $.ajax({
            url: "/api/attendences/" + button.attr("data-gig-id"),
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
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
}();
