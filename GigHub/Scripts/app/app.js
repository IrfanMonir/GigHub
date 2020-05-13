var GigsController = function () {
    var init = function () {
        $(".js-toggle-attendence").click(toggleAttendence);       
    };

    var toggleAttendence = function (e) {
        var button = $(e.target);
        if (button.hasClass("btn-default")) {
            $.post("/api/attendences", { gigId: button.attr("data-gig-id") })
                .done(function () {
                    button
                        .removeClass("btn-default")
                        .addClass("btn-info")
                        .text("Going");

                })
                .fail(function () {
                    alert("You are already going.");
                });
        }
        else {
            $.ajax({
                url: "/api/attendences/" + button.attr("data-gig-id"),
                method: "DELETE"
            })
                .done(function () {
                    button
                        .removeClass("btn-info")
                        .addClass("btn-default")
                        .text("Going?");
                })
                .fail(function () {
                    alert("Something failed");
                });
        }
    };
    return {
        init: init
    }
}();
