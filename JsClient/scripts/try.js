jQuery(document).ready(function($) {

    $(window).resize(function() {
        windowWidth = $(window).width();
        $('#slider ul').css({
            marginLeft: -windowWidth
        });
    });

    setInterval(function() {
        moveRight();
    }, 7000);

    var slideWidth = '100vw';
    var slideHeight = '100vh';
    var sliderUlWidth = '400vw';
    var windowWidth = $(window).width();

    $('#slider').css({
        width: slideWidth,
        height: slideHeight
    });

    $('#slider ul').css({
        width: sliderUlWidth,
        marginLeft: -windowWidth
    });

    $('#slider ul li').css({
        width: slideWidth,
        height: slideHeight
    });

    $('#slider ul li:last-child').prependTo('#slider ul');

    function moveLeft() {
        $('#slider ul').animate({
            left: +windowWidth
        }, 600, function() {
            $('#slider ul li:last-child').prependTo('#slider ul');
            $('#slider ul').css('left', '');
        });
    }

    function moveRight() {
        $('#slider ul').animate({
            left: -windowWidth
        }, 600, function() {
            $('#slider ul li:first-child').appendTo('#slider ul');
            $('#slider ul').css('left', '');
        });
    }

    $('a.control_prev').click(function() {
        moveLeft();
    });

    $('a.control_next').click(function() {
        moveRight();
    });

});
