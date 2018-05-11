// Write your JavaScript code.
$(document).ready(function () {
    M.AutoInit();
    //preloader
    $(window).load(function () {
        setTimeout(function () {
            $("body").addClass("loaded");
        }, 150);
    }); 
    // Materialize Dropdown
    $('.dropdown-trigger').dropdown({
        coverTrigger: false // Displays dropdown below the button
    });
    //Tooltip
    $(document).ready(function () {
        $(".tooltipped").tooltip({ delay: 50 });

        //var count = JSON.parse($.getJSON("notifications/notificationcount/"));
        //console.log(1);
        $.getJSON("notifications/notificationcount/",
            function(data) {
                document.getElementById("notificationCount").innerHTML = data;
            });
    });
    //$('.sidenav').sidenav();

    //$('.parallax').parallax();

    
    ////console.log(1);
    //$('select').formSelect();


});

    function DeleteNotification(id) {
        $.getJSON("notifications/Delete/" + id);
       
        //$.get("/Notifications/NotificationViewComponent", function (data) { container.html(data); });

    };
