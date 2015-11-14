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

                    userData.users.login(user);

                    $('#log').attr('href', '#/logout');
                    $('#log').html('Logout');
                    context.redirect('#/');
                        // .then(function() {
                               // $('#log').attr('href', '#/logout');
                               //  $('#log').html('Logout'); 
                        //     context.redirect('#/');
                        // }, function(err) {
                        //     return alertHelper.getOkAlert('Unable to log user.');
                        // });
                });
            });
    };

    var register = function(context) {
        var $container = $('#container');
        templates.get('RegisterTemplate')
            .then(function(template) {
                $container.html(template({
                    text: 'register'
                }));
                scrollFixedHelper.switchToFixed();

                $('#registerBtn').on('click', function() {
                    var username = $('#usernameInput').val();
                    var password = $('#userPassword').val();
                    var repeatedPassword = $('#repeateUserPassword').val();
                    var firstName = $('#firstName').val();
                    var lastName = $('#lastName').val();


                    var validUserInput = validateUserInput($container, username, password, repeatedPassword);

                    if (!validUserInput) {
                        return;
                    }

                    var user = {
                        username,
                        password,
                        firstName,
                        lastName
                    };

                    userData.users.register(user)
                        .then(function() {
                            return alertHelper.getGoHomeAlert('You have been successfully registered.', context);
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
        var currentUsername = localStorage.USERNAME_KEY;
        activeLink.toggle('#profileLink');


        if (!currentUsername) {
            return alertHelper.getGoHomeAlert('You must be logged in order to view user profile.', context);
        }

        templates.get('ProfilePage')
            .then(function(template) {

                // this should be replaced by the next request!!!
                var currentUser = userData.users.getUser(currentUsername);
                scrollFixedHelper.switchToUserFixed();
                $container.html(template(currentUser));

                $('#deleteProfile').on('click', function() {
                    return alertHelper.getChioseAlert('Are you sure you want to delete your profile?', context, currentUsername);
                });

                $('#changeProfile').on('click', function() {
                    context.redirect('#/user/change/' + currentUsername);
                });

                $('#seeCurrentPhoto').on('click', function() {
                    var $fotoramaDiv = $('#fotorama').fotorama();
                    var imgUrl = $fotoramaDiv.data('fotorama').activeFrame._html;
                    var route = '#/images/' + imgUrl;
                    context.redirect(route);
                });

                $('#addNewPhoto').on('click', function() {
                    context.redirect('#/createImages');
                });

                // userData.users.getUser(currentUsername)
                //     .then(function(userData) {
                //         $container.html(template(userData));
                //         scrollFixedHelper.switchToScroll();

                //         $('#deleteProfile').on('click', function() {
                //              return alertHelper.getChioseAlert('Are you sure you want to delete your profile?', context, currentUsername);
                //         });

                //         $('#changeProfile').on('click', function() {
                //             context.redirect('#/user/change/' + currentUsername);
                //         });
                //     });
            });
    };

    var deleteUser = function(context) {
        console.log('we are in!');
        var $container = $('#container');

        if (!localStorage.AUTHENTICATION_KEY) {
            return alertHelper.getGoHomeAlert('You must be logged in order delete your profile.', context);
        }

        userData.users.delete()
            .then(function() {
                logout(context);
            }, function(err) {
                return alertHelper.getGoHomeAlert('User ' + err.statusText, context);
            });
    };

    var changeDetails = function(context) {
        // Not implemented!
        var $container = $('#container');
        var currentUsername = this.params['username'];
        console.log(currentUsername);

        templates.get('RegisterTemplate')
            .then(function(template) {
                $container.html(template({
                    text: 'change'
                }));

                scrollFixedHelper.switchToFixed();

                $('#registerBtn').on('click', function() {
                    var username = $('#usernameInput').val();
                    var password = $('#userPassword').val();
                    var repeatedPassword = $('#repeateUserPassword').val();
                    var firstName = $('#firstName').val();
                    var lastName = $('#lastName').val();

                    var validUserInput = validateUserInput($container, username, password, repeatedPassword);

                    if (!validUserInput) {
                        return;
                    }

                    var user = {
                        username,
                        password,
                        firstName,
                        lastName
                    };

                    userData.users.changeUser(user)
                        .then(function() {
                            return alertHelper.getOkAlert('You have successfully changed your profile details.');
                        }, function(err) {
                            return alertHelper.getOkAlert('User ' + err.statusText);
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
