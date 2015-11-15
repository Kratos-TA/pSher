/*jslint white: true */

var jsonRequester = (function() {
    /* use strict */

    function _send(method, url, options) {
        options = options || {};

        var headers = options.headers || {},
            data = options.data || undefined;

        // Check how to implement this as it wont work like that!!!    
        // headers.x-auth-key = localStorage.getItem('AUTHENTICATION_KEY');

        var promise = new Promise(function(resolve, reject) {
            $.ajax({
                url: url,
                method: method,
                contentType: 'application/json; charset=utf-8',
                headers: headers,
                data: JSON.stringify(data),
                success: function(res) {
                    resolve(res);
                },
                error: function(err) {
                    reject(err);
                }
            });
        });
        return promise;
    }

    function get(url, options) {
        return _send('GET', url, options);
    }

    function post(url, options) {
        return _send('POST', url, options);
    }

    function put(url, options) {
        return _send('PUT', url, options);
    }

    function del(url, options) {
        return _send('DELETE', url, options);
    }

    return {
        // send: send,
        get: get,
        post: post,
        put: put,
        delete: del
    };
}());

export {
    jsonRequester
};
