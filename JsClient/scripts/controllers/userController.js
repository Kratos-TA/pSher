/*jslint white: true */

import {
    scrollFixedHelper
}
from '../helpers/scrollFixedHelper.js';

import {
    activeLink
}
from '../helpers/toggleActiveLink.js';

import {
    templates
}
from '../templates.js';

var userController = (function() {
    /* use strict */

    var getUserProfilePage = function(context) {
        var $container = $('#container');
        activeLink.toggle('#profileLink');

        templates.get('ProfilePage')
            .then(function(template) {
                // Still not implemented
                $container.html(template);
                scrollFixedHelper.switchToScroll();
            });
    };

    return {
        getProfile: getUserProfilePage
    };
}());

export {
    userController
};
