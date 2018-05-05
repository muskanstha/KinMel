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
    });
    //$('.sidenav').sidenav();

    //$('.parallax').parallax();

    
    ////console.log(1);
    //$('select').formSelect();


});