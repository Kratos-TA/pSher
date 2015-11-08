jQuery(document).ready(function($) {
    var slideWidth = '100vw';
    var slideHeight = '100vh';
    var sliderUlWidth = '400vw';
    var windowWidth = $(window).width();

    $(window).resize(function() {
        windowWidth = $(window).width();
        $('#slider ul').css({
            marginLeft: -windowWidth
        });
        selectSliderImages(windowWidth);
    });

    setInterval(function() {
        moveRight();
    }, 7000);

    selectSliderImages(windowWidth);

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

    function selectSliderImages(windowWidth) {
        var windowHeight = $(window).height();
        var aspectRatio = windowWidth / windowHeight;
        var $slide1 = $("#slide1");
        var $slide2 = $("#slide2");
        var $slide3 = $("#slide3");
        var $slide4 = $("#slide4");
        var imagePath;

        if (aspectRatio <= 1) {
            imagePath = "0_75";
        } else if (aspectRatio <= 1.70) {
            imagePath = "1_33";
        } else if (aspectRatio <= 2.25) {
            imagePath = "1_75";
        } else {
            imagePath = "2_30";
        }

        $slide1.css('background-image', 'url(' + './images/' + imagePath + '/image1.jpg' + ')');
        $slide2.css('background-image', 'url(' + './images/' + imagePath + '/image2.jpg' + ')');
        $slide3.css('background-image', 'url(' + './images/' + imagePath + '/image3.jpg' + ')');
        $slide4.css('background-image', 'url(' + './images/' + imagePath + '/image4.jpg' + ')');
    }

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
