/*jslint white: true */
import {
    jsonRequester
}
from '../requester.js';

var imageData = (function() {
    /* use strict */

    const LOCAL_STORAGE_USERNAME_KEY = 'USERNAME_KEY',
        LOCAL_STORAGE_AUTHKEY_KEY = 'AUTHENTICATION_KEY';

    function uploadImage(imageDetails) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            },
            data: imageDetails
        };

        return jsonRequester.push('api/images', options)
            .then(function(res) {
                return res.result;
            });
    }

    function deleteImage(imageId) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };

        return jsonRequester.delete('api/images/' + imageId, options)
            .then(function(res) {
                return res.result;
            });
    }


    return {
        upload: uploadImage,
        delete: deleteImage

    };
}());

export {
    imageData
};
