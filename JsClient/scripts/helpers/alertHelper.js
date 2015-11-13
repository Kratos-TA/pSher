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
        var promise = new Promise(function(resolve, reject) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: message
                    }));
                    scrollFixedHelper.switchToFixed();
                    $('#okBtn').on('click', function() {
                        sammyApp.refresh();
                    });
                    resolve();
                });
        });
        return promise;
    };

    var getGoHomeAlert = function(message, context) {
        var $container = $('#container');
        var promise = new Promise(function(resolve, reject) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: message
                    }));
                    scrollFixedHelper.switchToFixed();
                    $('#okBtn').on('click', function() {
                        context.redirect('#/');
                    });
                    resolve();
                });
        });
        return promise;
    };

    var getChioseAlert = function(message, context, currentUsername) {
        var $container = $('#container');
        var promise = new Promise(function(resolve, reject) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: message
                    }));
                    scrollFixedHelper.switchToFixed();
                    $('#okBtn').on('click', function() {
                        $('#noBtn').css('display', 'none');
                        context.redirect('#/user/delete/:' + currentUsername);
                    });
                    $('#noBtn').on('click', function() {
                        $('#noBtn').css('display', 'none');
                        sammyApp.refresh();
                    });
                    $('#noBtn').css('display', 'inline-block');
                    resolve();
                });
        });
        return promise;
    };


    return {
        getOkAlert,
        getGoHomeAlert,
        getChioseAlert
    };
}());

export {
    alertHelper
};
