/*jslint white: true */
import {
    jsonRequester
}
from '../requester.js';

var userData = (function() {
    /* use strict */

    var users = [{
        firstName: "veso",
        lastName: "tsve",
        passHash: "c8f56e5c7fec26b5d99177a658d6204e923eed7f",
        userId: 1,
        username: "veselin",
        images: [{
            url: './images/galeries/galery%20%28' + '1' + '%29.jpg',
            imageId: '1'
        }, {
            url: './images/galeries/galery%20%28' + '2' + '%29.jpg',
            imageId: '2'
        }, {
            url: './images/galeries/galery%20%28' + '3' + '%29.jpg',
            imageId: '3'
        }, {
            url: './images/galeries/galery%20%28' + '4' + '%29.jpg',
            imageId: '4'
        }, {
            url: './images/galeries/galery%20%28' + '5' + '%29.jpg',
            imageId: '5'
        }, {
            url: './images/galeries/galery%20%28' + '6' + '%29.jpg',
            imageId: '6'
        }, {
            url: './images/galeries/galery%20%28' + '7' + '%29.jpg',
            imageId: '7'
        }, {
            url: './images/galeries/galery%20%28' + '8' + '%29.jpg',
            imageId: '8'
        }, {
            url: './images/galeries/galery%20%28' + '9' + '%29.jpg',
            imageId: '9'
        }, {
            url: './images/galeries/galery%20%28' + '10' + '%29.jpg',
            imageId: '10'
        }, {
            url: './images/galeries/galery%20%28' + '11' + '%29.jpg',
            imageId: '11'
        }, {
            url: './images/galeries/galery%20%28' + '12' + '%29.jpg',
            imageId: '12'
        }, {
            url: './images/galeries/galery%20%28' + '13' + '%29.jpg',
            imageId: '13'
        }, {
            url: './images/galeries/galery%20%28' + '14' + '%29.jpg',
            imageId: '14'
        }]
    }];

    var userId = 0;

    const LOCAL_STORAGE_USERNAME_KEY = 'USERNAME_KEY',
        LOCAL_STORAGE_AUTHKEY_KEY = 'AUTHENTICATION_KEY';

    /* Users */

    function register(user) {
        var reqUser = {
            UserName: user.username,
            FirstName: user.firstName,
            LastName: user.lastName,
            Email: user.email,
            Password: CryptoJS.SHA1(user.username + user.password).toString(),
            ConfirmPassword: CryptoJS.SHA1(user.username + user.repeatedPassword).toString()

            // remove this in the production code!!!
            // userId: ++userId
        };

        return jsonRequester.post('http://localhost:4380/api/account/register', {
            data: reqUser
        });

        // Remove this!
        // var promise = new Promise(function(resolve, reject) {
        //     users.push(reqUser);
        //     localStorage.setItem(LOCAL_STORAGE_USERNAME_KEY, user.username);
        //     localStorage.setItem(LOCAL_STORAGE_AUTHKEY_KEY, user.authKey);
        //     resolve(reqUser.username);
        // });

        // return promise;
    }


    function login(user) {
        var reqUser = {
            username: user.username,
            password: CryptoJS.SHA1(user.username + user.password).toString(),
            grant_type: 'password'
        };

        // localStorage.setItem(LOCAL_STORAGE_USERNAME_KEY, reqUser.username);
        // localStorage.setItem(LOCAL_STORAGE_AUTHKEY_KEY, reqUser.passHash);
        // return reqUser;

        return jsonRequester.sendLogIn('http://localhost:4380/api/users/login', reqUser)
            .then(function(resp) {
                console.log(resp);
                localStorage.setItem(LOCAL_STORAGE_USERNAME_KEY, resp.userName);
                localStorage.setItem(LOCAL_STORAGE_AUTHKEY_KEY, resp.access_token);
                return resp.userName;
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
        return users[0];

        // var options = {
        //     headers: {
        //         'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
        //     }
        // };
        // return jsonRequester.get('/api/users/' + currentUsername, options)
        //     .then(function(res) {
        //         return res.result;
        //     });
    }

    function userDelete() {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };
        return jsonRequester.delete('http://localhost:4380/api/users/' + localStorage.USERNAME_KEY, options)
            .then(function(res) {
                return res.result;
            });
    }

    function changeUser(user) {
        var headers = {
            'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
        };

        var reqUser = {
            FirstName: user.firstName,
            LastName: user.lastName,
            Email: user.email,
            ChangePasswordBindingModel: {
                OldPassword: CryptoJS.SHA1(user.username + user.oldPass).toString(),
                NewPassword: CryptoJS.SHA1(user.username + user.password).toString(),
                ConfirmPassword: CryptoJS.SHA1(user.username + user.password).toString()
            }
        };

        return jsonRequester.put('/api/users', {
                data: reqUser,
                headers: headers
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
