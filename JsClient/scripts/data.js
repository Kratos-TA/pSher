/*jslint white: true */
import {
    jsonRequester
}
from './requester.js';

var data = (function() {
    /* use strict */

    const LOCAL_STORAGE_USERNAME_KEY = 'USERNAME_KEY',
        LOCAL_STORAGE_AUTHKEY_KEY = 'AUTHENTICATION_KEY';

    /* Users */

    function register(user) {
        var reqUser = {
            username: user.username,
            passHash: CryptoJS.SHA1(user.username + user.password).toString()
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


    // I need this for the starting screen
    // function hasUser() {
    //     return !!localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY) &&
    //         !!localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY);
    // }

    function usersGet() {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };
        return jsonRequester.get('api/users', options)
            .then(function(res) {
                return res.result;
            });
    }

    return {
        users: {
            login: login,
            logout: logout,
            register: register,
            get: usersGet,
        }
    };
}());

export {
    data
};
