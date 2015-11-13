/*jslint white: true */

import {
    userData
}
from '../data/userData.js';

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

import {
    alertHelper
}
from '../helpers/alertHelper.js';

var userController = (function() {
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
                        return alertHelper.getOkAlert('Your username must be atleast six characters long.');
                    }

                    user = {
                        username,
                        password
                    };

                    userData.users.login(user)
                        .then(function() {
                            context.redirect('#/');
                        }, function(err) {
                            return alertHelper.getOkAlert('Unable to log user.');
                        });
                });
            });
    };

    var register = function(context) {
        var $container = $('#container');
        templates.get('RegisterTemplate')
            .then(function(template) {
                $container.html(template);
                scrollFixedHelper.switchToFixed();

                $('#registerBtn').on('click', function() {
                    var username = $('#registerUserName').val();
                    var password = $('#registerUserPassword').val();
                    var repeatedPassword = $('#repeateUserPassword').val();

                    var validUserInput = validateUserInput($container, username, password, repeatedPassword);

                    if (!validUserInput) {
                        return;
                    }

                    var user = {
                        username,
                        password
                    };

                    userData.users.register(user)
                        .then(function() {
                            context.redirect('#/');
                        }, function(err) {
                            return alertHelper.getOkAlert('User ' + err.statusText);
                        });
                });
            });
    };

    var logout = function(context) {
        userData.users.logout()
            .then(function() {
                $('#log').attr('href', '#/login');
                $('#log').html('Login');
                context.redirect('#/');
            });
    };

    var getProfile = function(context) {
        var $container = $('#container');
        var currentUsername = this.params['username'];
        activeLink.toggle('#profileLink');

        if (!localStorage.AUTHENTICATION_KEY) {
            return alertHelper.getOkAlert('You must be logged in order to view user profile.');
        }

        templates.get('ProfilePage')
            .then(function(template) {

                userData.users.getUser(currentUsername)
                    .then(function(userData) {
                        $container.html(template(userData));
                        scrollFixedHelper.switchToScroll();

                        $('#deleteProfile').on('click', function() {
                            return alertHelper.getOkAlert('Are you sure you want to delete your profile?')
                                .then(function() {
                                    $('#okBtn').on('click', function() {
                                        $('#noBtn').css('display', 'none');
                                        context.redirect('#/user/delete/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
                                    });
                                    $('#noBtn').on('click', function() {
                                        $('#noBtn').css('display', 'none');
                                        sammyApp.refresh();
                                    });
                                    $('#noBtn').css('display', 'inline-block');
                                });
                        });

                        $('#changeProfile').on('click', function() {
                            context.redirect('#/user/change/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
                        });
                    });
            });
    };

    var deleteUser = function(context) {
        var $container = $('#container');

        if (!localStorage.AUTHENTICATION_KEY) {
            return alertHelper.getOkAlert('You must be logged in order delete your profile.')
                .then(function() {
                    $('#okBtn').on('click', function() {
                        context.redirect('#/login');
                    });
                });
        }

        userData.users.delete()
            .then(function() {
                return alertHelper.getOkAlert('You have deleted your profile from PShare.')
                    .then(function() {
                        $('#okBtn').on('click', function() {
                            logout(context);
                        });
                    });
            }, function(err) {
                return alertHelper.getOkAlert('User ' + err.statusText)
                    .then(function() {
                        $('#okBtn').on('click', function() {
                            context.redirect('#/user/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
                        });
                    });
            });
    };

    var changeDetails = function(context) {
        // Not implemented!
        var $container = $('#container');
        var currentUsername = this.params['username'];

        templates.get('ChangeProfilePage')
            .then(function(template) {
                userData.users.getUser(currentUsername)
                    .then(function(userData) {
                        $container.html(template(userData));
                        scrollFixedHelper.switchToFixed();

                        $('#changeProfile').on('click', function() {
                            var username = $('#userPassword').val();
                            var password = $('#userPassword').val();
                            var repeatedPassword = $('#repeateUserPassword').val();

                            var validUserInput = validateUserInput($container, username, password, repeatedPassword);

                            if (!validUserInput) {
                                return;
                            }

                            var user = {
                                username,
                                password
                            };

                            userData.users.changeUser(user)
                                .then(function() {
                                    return alertHelper.getOkAlert('You have successfully changed your profile details.');
                                });
                        });
                    });
            });
    };

    var validateUserInput = function($container, username, password, repeatedPassword) {
        var valid = true;

        if (!validators.validateUsername(username)) {
            valid = false;
            alertHelper.getOkAlert('Ivalid Username! Username must be between 6 and 30 symbols.');
        } else if (!password) {
            valid = false;
            alertHelper.getOkAlert('You have not given a password!');
        } else if (password !== repeatedPassword) {
            valid = false;
            alertHelper.getOkAlert('You have given different input in the two password fields!');
        }

        return valid;
    };

    return {
        login,
        logout,
        register,
        getProfile,
        deleteUser,
        changeDetails
    };
}());

export {
    userController
};
