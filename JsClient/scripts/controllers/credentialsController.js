/*jslint white: true */

import {
    data
}
from '../data.js';

import {
    validators
}
from '../helpers/validators.js';

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

var credentialsController = (function() {
	/* use strict */

    var login = function(context) {
        var $container = $('#container');
        activeLink.toggle('#logLink');

        // Displlay login page
        templates.get('LoginTemplateNew')
            .then(function(template) {
                $container.html(template);
                scrollFixedHelper.switchToFixed();

                $('#loginBtn').on('click', function() {
                    var username = $('#userName').val();
                    var password = $('#userPassword').val();
                    var user;

                    if (!validators.validateUsername(username)) {
                        toastr.error('Ivalid Username! Username must be between 6 and 30 symbols!');
                        return new Error();
                    }

                    user = {
                        username,
                        password
                    };

                    data.users.login(user)
                        .then(function() {
                            context.redirect('#/');
                        }, function(err) {
                            $('#form-login').trigger("reset");
                            toastr.error(err.responseJSON);
                        });
                });
            });
    };

    var register = function(context) {
        var $container = $('#container');
        templates.get('RegisterTemplate')
            .then(function(template) {
                $container.html(template);

                $('#registerBtn').on('click', function() {
                    var username = $('#registerUserName').val();
                    var password = $('#registerUserPassword').val();
                    var repeatedPassword = $('#repeateUserPassword').val();

                    if (!validators.validateUsername(username)) {
                        toastr.error('Ivalid Username! Username must be between 6 and 30 symbols!');
                        return new Error();
                    }

                    if (password != repeatedPassword) {
                        toastr.error('You have given different input in the two password field!');
                        return new Error();
                    }

                    var user = {
                        username,
                        password
                    };

                    data.users.register(user)
                        .then(function() {
                            context.redirect('#/');
                        }, function(err) {
                            $('#form-register').trigger("reset");
                            toastr.error(err.responseJSON);
                        });
                });
            });
    };

    var logout = function(context) {
        data.users.logout()
            .then(function() {
                $('#log').attr('href', '#/login');
                $('#log').html('Login');
                context.redirect('#/');
            });
    };

    return {
        login,
        logout,
        register
    };
}());

export {
    credentialsController
};
