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
                        templates.get('AlertTemplate')
                            .then(function(template) {
                                $container.html(template({
                                    alertText: 'Your username must be atleast six characters long.'
                                }));
                                scrollFixedHelper.switchToFixed();
                                $('#okBtn').on('click', function() {
                                    sammyApp.refresh();
                                });
                            });
                        return;
                    }

                    user = {
                        username,
                        password
                    };

                    userData.users.login(user)
                        .then(function() {
                            context.redirect('#/');
                        }, function(err) {
                            templates.get('AlertTemplate')
                                .then(function(template) {
                                    $container.html(template({
                                        //not sure wether it works like this
                                        alertText: err.responseJSON.toString()
                                    }));
                                    scrollFixedHelper.switchToFixed();
                                    $('#okBtn').on('click', function() {
                                        context.redirect('#/login');
                                    });

                                    return;
                                });
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
                            $container.html(template({
                                //not sure wether it works like this
                                alertText: err.responseJSON.toString()
                            }));
                            $('#okBtn').on('click', function() {
                                context.redirect('#/register');
                            });

                            return;
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
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: 'You must be logged in order to view user profile.'
                    }));
                    $('#okBtn').on('click', function() {
                        context.redirect('#/');
                    });
                });
            return;
        }

        templates.get('ProfilePage')
            .then(function(template) {

                userData.users.getUser(currentUsername)
                    .then(function(userData) {
                        $container.html(template(userData));
                        scrollFixedHelper.switchToScroll();

                        $('#deleteProfile').on('click', function() {
                            templates.get('AlertTemplate')
                                .then(function(template) {
                                    $container.html(template({
                                        alertText: 'You are about to delete your PShare profile. Are you sure?'
                                    }));
                                    $('#okBtn').on('click', function() {
                                        $('#noBtn').css('display', 'none');
                                        context.redirect('#/user/delete/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
                                    });
                                    $('#noBtn').on('click', function() {
                                        $('#noBtn').css('display', 'none');
                                        context.redirect('#/user/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
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

        // Add question Are you sure?
        // Ask to give pass!

        if (!localStorage.AUTHENTICATION_KEY) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: 'You must be logged in order delete your profile.'
                    }));
                    scrollFixedHelper.switchToFixed();
                    $('#okBtn').on('click', function() {
                        context.redirect('#/login');
                    });

                    return;
                });
        }

        userData.users.delete()
            .then(function() {
                templates.get('AlertTemplate')
                    .then(function(template) {
                        $container.html(template({
                            alertText: 'You have deleted your profile from PShare.'
                        }));
                        $('#okBtn').on('click', function() {
                            logout(context);
                        }, function(err) {
                            $container.html(template({
                                //not sure wether it works like this
                                alertText: err.responseJSON.toString()
                            }));
                            $('#okBtn').on('click', function() {
                                context.redirect('#/user/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
                            });

                            return;
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
                                    templates.get('AlertTemplate')
                                        .then(function(template) {
                                            $container.html(template({
                                                alertText: 'You have successfully changed your profile details.'
                                            }));
                                            $('#okBtn').on('click', function() {
                                                context.redirect('#/user/:' + localStorage.LOCAL_STORAGE_USERNAME_KEY);
                                            });
                                        });

                                });
                        });
                    });
            });
    };

    var validateUserInput = function($container, username, password, repeatedPassword) {
        if (!validators.validateUsername(username)) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: 'Ivalid Username! Username must be between 6 and 30 symbols.'
                    }));
                    $('#okBtn').on('click', function() {
                        sammyApp.refresh();
                    });
                });
            return false;
        }

        if (!password) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: 'You have not given a password!'
                    }));
                    $('#okBtn').on('click', function() {
                        sammyApp.refresh();
                    });
                });
            return false;
        }

        if (password !== repeatedPassword) {
            templates.get('AlertTemplate')
                .then(function(template) {
                    $container.html(template({
                        alertText: 'You have given different input in the two password fields!'
                    }));
                    $('#okBtn').on('click', function() {
                        sammyApp.refresh();
                    });
                });
            return false;
        }

        return true;
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
