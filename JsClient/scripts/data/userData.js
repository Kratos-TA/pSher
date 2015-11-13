/*jslint white: true */
import {
    jsonRequester
}
from '../requester.js';

var userData = (function() {
    /* use strict */

    const LOCAL_STORAGE_USERNAME_KEY = 'USERNAME_KEY',
        LOCAL_STORAGE_AUTHKEY_KEY = 'AUTHENTICATION_KEY';

    /* Users */

    function register(user) {
        var reqUser = {
            username: user.username,
            passHash: CryptoJS.SHA1(user.username + user.password).toString(),
            firstName: user.firstName,
            lastName: user.lastName
        };

        return jsonRequester.post('api/users', {
                data: reqUser
            })
            .then(function(resp) {
                var user = resp.result;
                localStorage.setItem(LOCAL_STORAGE_USERNAME_KEY, user.username);
                localStorage.setItem(LOCAL_STORAGE_AUTHKEY_KEY, user.authKey);
                return {
                    username: resp.result.username
                };
            });
    }


    function login(user) {
        var reqUser = {
            username: user.username,
            passHash: CryptoJS.SHA1(user.username + user.password).toString()
        };

        var options = {
            data: reqUser
        };

        return jsonRequester.put('api/auth', options)
            .then(function(resp) {
                var user = resp.result;
                localStorage.setItem(LOCAL_STORAGE_USERNAME_KEY, user.username);
                localStorage.setItem(LOCAL_STORAGE_AUTHKEY_KEY, user.authKey);
                return user;
            });
    }

    function logout() {
        var promise = new Promise(function(resolve, reject) {
            localStorage.removeItem(LOCAL_STORAGE_USERNAME_KEY);
            localStorage.removeItem(LOCAL_STORAGE_AUTHKEY_KEY);
            resolve();
        });
        return promise;
    }

    function getUser(currentUsername) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };
        return jsonRequester.get('api/users/' + currentUsername, options)
            .then(function(res) {
                return res.result;
            });
    }

    function userDelete() {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };
        return jsonRequester.delete('api/users/' + localStorage.USERNAME_KEY, options)
            .then(function(res) {
                return res.result;
            });
    }

    function changeUser(user) {
        var reqUser = {
            username: user.username,
            passHash: CryptoJS.SHA1(user.username + user.password).toString(),
            firstName: user.firstName,
            lastName: user.lastName
        };

        return jsonRequester.put('api/users', {
                data: reqUser
            })
            .then(function(resp) {
                var user = resp.result;
                localStorage.setItem(LOCAL_STORAGE_USERNAME_KEY, user.username);
                localStorage.setItem(LOCAL_STORAGE_AUTHKEY_KEY, user.authKey);
                return {
                    username: resp.result.username
                };
            });
    }

    return {
        users: {
            login: login,
            logout: logout,
            register: register,
            delete: userDelete,
            getUser: getUser,
            changeUser: changeUser
        }
    };
}());

export {
    userData
};
