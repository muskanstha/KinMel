// Write your JavaScript code.

$(document).ready(function () {
    M.AutoInit();
    // Materialize Dropdown
    $('.dropdown-trigger').dropdown({
        coverTrigger: false // Displays dropdown below the button
    });

    //Tooltip
    $(".tooltipped").tooltip({ delay: 50 });

    // details carousel
    $('.carousel.carousel-slider').carousel({
        fullWidth: true,
        indicators: true,
        noWrap: true
    });


    //lightbox.option({
    //    'resizeDuration': 200,
    //    'fadeDuration': 200,
    //    'imageFadeDuration': 200,
    //    'wrapAround': true,
    //    'disableScrolling': true
    //});

    //var count = JSON.parse($.getJSON("notifications/notificationcount/"));
    $.getJSON("/notifications/notificationcount/",
        function (count) {
            var notificationCount = $("#notificationCount");
            notificationCount.empty();
            if (count > 0) {
                notificationCount.css("display", "block");
                notificationCount.html(count);
            }
        });

    
    ////carousel
    //$('.slider').slider({
    //    fullWidth: true,
    //    indicators: true,
    //    duration: 800,
    //    interval: 5000
    //});
});
