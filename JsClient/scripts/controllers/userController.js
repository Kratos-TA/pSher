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

                    userData.login(user)
                        .then(function(username) {
                            $('#log').attr('href', '#/logout');
                            $('#log').html('Logout');
                            return alertHelper.getGoHomeAlert(username + ' successfully logged.', context);
                        }, function(err) {
                            return alertHelper.getOkAlert('User ' + err.statusText);
                        });
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
                    var firstName = $('#firstName').val();
                    var lastName = $('#lastName').val();
                    var email = $('#email').val();
                    var password = $('#userPassword').val();
                    var repeatedPassword = $('#repeateUserPassword').val();

                    var userToSend = {
                        username,
                        firstName,
                        lastName,
                        email,
                        password,
                        repeatedPassword
                    };

                    var validUserInput = validateUserInput($container, userToSend, true);

                    if (!validUserInput) {
                        return;
                    }

                    if (!firstName || firstName.length < 2) {
                        userToSend.firstName = null;
                    }

                    if (!lastName || lastName.length < 2) {
                        userToSend.lastName = null;
                    }

                    userData.register(userToSend)
                        .then(function() {
                            return alertHelper.getGoHomeAlert('You have been successfully registered.', context);
                        }, function(err) {
                            return alertHelper.getOkAlert('User ' + err.statusText);
                        });
                });
            });
    };

    var logout = function(context) {
        userData.logout()
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
            return alertHelper.getGoHomeAlert('You must be logged in order to view a user profile.', context);
        }

        templates.get('ProfilePage')
            .then(function(template) {
                userData.getUser(currentUsername)
                    .then(function(currentUser) {
                        scrollFixedHelper.switchToUserFixed();
                        $container.html(template(currentUser));

                        $('#deleteProfile').on('click', function() {
                            return alertHelper.getChioseAlert('Are you sure you want to delete your profile?', context);
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
                    }, function(err) {
                        return alertHelper.getGoHomeAlert('User ' + err.statusText, context);
                    });
            });
    };

    var deleteUser = function(context) {
        if (!localStorage.AUTHENTICATION_KEY) {
            return alertHelper.getGoHomeAlert('You must be logged in order delete your profile.', context);
        }

        userData.delete()
            .then(function() {
                logout(context);
            }, function(err) {
                return alertHelper.getGoHomeAlert('User ' + err.statusText, context);
            });
    };

    var changeDetails = function(context) {
        var $container = $('#container');
        var currentUsername = this.params['username'];

        userData.getUser(currentUsername)
            .then(function(currentUser) {

                if (localStorage.USERNAME_KEY != currentUsername) {
                    return alertHelper.getGoHomeAlert('You are not allowed to edit other users profiles!', context);
                }

                templates.get('ChangeDetailsTemplate')
                    .then(function(template) {
                        $container.html(template(currentUser));
                        scrollFixedHelper.switchToFixed();

                        $('#changeDetails').on('click', function() {
                            var firstName = $('#firstName').val();
                            var lastName = $('#lastName').val();
                            var email = $('#email').val();

                            var userToSend = {
                                FirstName,
                                LastName,
                                Email,
                                ChangePasswordBindingModel: null
                            };

                            if (!firstName || firstName.length < 2) {
                                userToSend.firstName = null;
                            }

                            if (!lastName || lastName.length < 2) {
                                userToSend.lastName = null;
                            }

                            // If you have time - iplement full checks for the pass!
                            if (!email || email.length < 6) {
                                userToSend.email = null;
                            }

                            userData.changeUser(userToSend)
                                .then(function() {
                                    return alertHelper.getGoHomeAlert('You have successfully changed your profile details.', context);
                                }, function(err) {
                                    return alertHelper.getOkAlert('User ' + err.statusText);
                                });
                        });

                        $('#changeAll').on('click', function() {
                            var firstName = $('#firstName').val();
                            var lastName = $('#lastName').val();
                            var email = $('#email').val();
                            var password = $('#userPassword').val();
                            var repeatedPassword = $('#repeateUserPassword').val();
                            var oldPass = $('#oldPass').val();

                            var userToSend = {
                                firstName,
                                lastName,
                                email,
                                password,
                                repeatedPassword,
                                oldPass
                            };

                            var validUserInput = validateUserInput($container, userToSend, false);

                            if (!validUserInput) {
                                return;
                            }

                            if (!firstName || firstName.length < 2) {
                                userToSend.firstName = null;
                            }

                            if (!lastName || lastName.length < 2) {
                                userToSend.lastName = null;
                            }
                            
                            userData.changeUser(userToSend)
                                .then(function() {
                                    return alertHelper.getGoHomeAlert('You have successfully changed your profile details.', context);
                                }, function(err) {
                                    return alertHelper.getOkAlert('User ' + err.statusText);
                                });
                        });
                    });
            }, function(err) {
                return alertHelper.getGoHomeAlert('User ' + err.statusText, context);
            });
    };

    var validateUserInput = function($container, user, validateUsername) {
        var valid = true;
        if (validateUsername) {
            if (!validators.validateUsername(user.username)) {
                valid = false;
                alertHelper.getOkAlert('Ivalid Username! Username must be between 6 and 30 symbols.');
            }
        } else if (!validators.validateEmail(user.email)) {
            valid = false;
            alertHelper.getOkAlert('You have not given a valid email!');
        } else if (!user.password) {
            valid = false;
            alertHelper.getOkAlert('You have not given a password!');
        } else if (user.password.length < 6 || user.password.length > 30) {
            valid = false;
            alertHelper.getOkAlert('Ivalid new password! Password must be between 6 and 30 symbols.');
        } else if (user.password !== user.repeatedPassword) {
            valid = false;
            alertHelper.getOkAlert('You have given different input in the two new password fields!');
        } else if (!validateUsername) {
            if (user.oldPass.length < 6 || user.oldPass.length > 30) {
                valid = false;
                alertHelper.getOkAlert('Ivalid old password! Password must be between 6 and 30 symbols.');
            }
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