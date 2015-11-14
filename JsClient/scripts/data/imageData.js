/*jslint white: true */
import {
    jsonRequester
}
from '../requester.js';

var imageData = (function() {
    /* use strict */

    const LOCAL_STORAGE_USERNAME_KEY = 'USERNAME_KEY',
        LOCAL_STORAGE_AUTHKEY_KEY = 'AUTHENTICATION_KEY';

    function getImage(imageId) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };

        return jsonRequester.push('api/images/' + imageId, options)
            .then(function(res) {
                return res.result;
            });
    }

    function getAllImages(queryString) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            }
        };

        return jsonRequester.push('api/images' + queryString, options)
            .then(function(res) {
                return res.result;
            });
    }

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

    function rateImage(mark, currentImageId) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            },
            data: {
                'user': localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY),
                'imageId': currentImageId,
                'mark': mark
            }
        };

        return jsonRequester.post('/marks', options)
            .then(function(res) {
                return res.result;
            });
    }

    function deleteMark(currentImageId) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            },
            data: {
                'imageId': currentImageId
            }
        };

        return jsonRequester.delete('/marks', options)
            .then(function(res) {
                return res.result;
            });
    }

    function commentImage(comment, currentImageId) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            },
            data: {
                'user': localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY),
                'imageId': currentImageId,
                'comment': comment
            }
        };

        return jsonRequester.post('/comments', options)
            .then(function(res) {
                return res.result;
            });
    }

    function changeComment(comment, commentId) {
        var options = {
            headers: {
                'x-auth-key': localStorage.getItem(LOCAL_STORAGE_AUTHKEY_KEY)
            },
            data: {
                'user': localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY),
                'commentId': commentId,
                'comment': comment
            }
        };

        return jsonRequester.put('/comments', options)
            .then(function(res) {
                return res.result;
            });
    }


    return {
        getImage: getImage,
        getAll: getAllImages,
        upload: uploadImage,
        delete: deleteImage,
        rateImage: rateImage,
        deleteMark: deleteMark,
        commentImage: commentImage,
        changeComment: changeComment
    };
}());

export {
    imageData
};
