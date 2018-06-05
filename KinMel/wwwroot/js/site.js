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
        coverTrigger: false, // Displays dropdown below the button
    });

    //Tooltip
    $(".tooltipped").tooltip({ delay: 50 });

    // details carousel
    $('.carousel.carousel-slider').carousel({
        fullWidth: true,
        indicators: true,
        //noWrap: true
    });


    lightbox.option({
        'resizeDuration': 200,
        'fadeDuration': 200,
        'imageFadeDuration': 200,
        'wrapAround': true,
        'disableScrolling': true
    });

    //var count = JSON.parse($.getJSON("notifications/notificationcount/"));
    $.getJSON("/notifications/notificationcount/",
        function (data) {
            document.getElementById("notificationCount").innerHTML = data;
        });

    ////carousel
    //$('.slider').slider({
    //    fullWidth: true,
    //    indicators: true,
    //    duration: 800,
    //    interval: 5000
    //});


});
