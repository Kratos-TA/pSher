/*jslint white: true */

import {
    templates
}
from '../templates.js';

var scrollFixedHelper = (function() {
    /* use strict */
    var switchToFixed = function() {
        $('#backgroundContainer').css('display', 'block');
        $('#container').removeClass('searchResultsContainer');
        $('#container').addClass('cover');
    };

    var switchToScroll = function() {
        $('#backgroundContainer').css('display', 'none');
        $('#container').removeClass('cover');
        $('#container').addClass('searchResultsContainer');
    };

    var switchToUserFixed = function() {
        $('#backgroundContainer').css('display', 'none');
        $('#container').removeClass('searchResultsContainer');
        $('#container').addClass('cover');
    };

    return {
        switchToFixed,
        switchToScroll,
        switchToUserFixed
    };

}());

export {
    scrollFixedHelper
};
