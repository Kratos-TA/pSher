/*jslint white: true */
import {
    slider
}
from './slider.js';

import {
    templates
}
from '../templates.js';

var scrollFixedHelper = (function() {
    /* use strict */
    var switchToFixed = function() {
        getBackgroundSlider();
        $('#backgroundContainer').css('display', 'block');
        $('#container').removeClass('searchResultsContainer');
        $('#container').addClass('cover');
    };

    var switchToScroll = function() {
        $('#backgroundContainer').css('display', 'none');
        $('#container').removeClass('cover');
        $('#container').addClass('searchResultsContainer');
    };

    var getBackgroundSlider = function() {
        console.log('in background function');
        templates.get('SliderTemplate')
            .then(function(template) {
                var backgroundImageContainer = $('#backgroundContainer');
                //  backgroundImageContainer.html(template(imageUrlContainerObject));
                backgroundImageContainer.html(template());
                // Load slider
                jQuery(document).ready(slider.get());
            });
    };

    return {
        switchToFixed,
        switchToScroll
    };

}());

export {
    scrollFixedHelper
};
