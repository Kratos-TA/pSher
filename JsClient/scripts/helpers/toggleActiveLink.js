/*jslint white: true */

var activeLink = (function() {
    /* use strict */
    console.log('toggleActiveLink loaded successfully');


    var toggleActiveLink = function(linkId) {
        $('#homeLink').removeClass('active');
        $('#profileLink').removeClass('active');
        $('#imagesLink').removeClass('active');
        $('#logLink').removeClass('active');
        $(linkId).addClass('active');
    };

    return {
        toggle: toggleActiveLink
    };
}());

export {
    activeLink
};
