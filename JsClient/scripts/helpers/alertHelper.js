/*jslint white: true */
import {
    templates
}
from '../templates.js';

import {
    scrollFixedHelper
}
from './scrollFixedHelper.js';

var alertHelper = (function() {
    /* use strict */

    var getOkAlert = function(message) {
        var $container = $('#container');
        var promise = new Promise(function() {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: message
                    }));
                    scrollFixedHelper.switchToFixed();
                    $('#okBtn').on('click', function() {
                        sammyApp.refresh();
                    });
                });
        });
        return promise;
    };

    var getChioseAlert = function(message) {

    };


    return {
        getOkAlert,
        getChioseAlert
    };
}());

export {
    alertHelper
};
