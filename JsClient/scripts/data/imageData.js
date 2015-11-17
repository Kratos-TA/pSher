/*jslint white: true */
import {
    jsonRequester
}
from '../requester.js';

var imageData = (function() {
    /* use strict */

    // Just for test
    /* var images = [{
        url: './images/galeries/galery%20%28' + '1' + '%29.jpg',
        imageId: '1',
        name: 'fdsdfsfsd'
    }, {
        url: './images/galeries/galery%20%28' + '2' + '%29.jpg',
        imageId: '2',
        name: 'photo 3'
    }, {
        url: './images/galeries/galery%20%28' + '3' + '%29.jpg',
        imageId: '3',
        name: 'photo some'
    }, {
        url: './images/galeries/galery%20%28' + '4' + '%29.jpg',
        imageId: '4',
        name: 'photo kurami'
    }, {
        url: './images/galeries/galery%20%28' + '5' + '%29.jpg',
        imageId: '5',
        name: 'yasabi photo'
    }, {
        url: './images/galeries/galery%20%28' + '6' + '%29.jpg',
        imageId: '6',
        name: 'ohala'
    }, {
        url: './images/galeries/galery%20%28' + '7' + '%29.jpg',
        imageId: '7',
        name: 'tydrsayt'
    }, {
        url: './images/galeries/galery%20%28' + '8' + '%29.jpg',
        imageId: '8',
        name: 'koki'
    }, {
        url: './images/galeries/galery%20%28' + '9' + '%29.jpg',
        imageId: '9',
        name: 'e typ'
    }, {
        url: './images/galeries/galery%20%28' + '10' + '%29.jpg',
        imageId: '10',
        name: 'sd'
    }, {
        url: './images/galeries/galery%20%28' + '11' + '%29.jpg',
        imageId: '11',
        name: 'fsdfs'
    }, {
        url: './images/galeries/galery%20%28' + '12' + '%29.jpg',
        imageId: '12',
        name: 'dsfs'
    }, {
        url: './images/galeries/galery%20%28' + '13' + '%29.jpg',
        imageId: '13',
        name: 'fsdsdfs'
    }, {
        url: './images/galeries/galery%20%28' + '14' + '%29.jpg',
        imageId: '14',
        name: 'ddddddd'
    }];
    var imageExample = {
        id: 23,
        name: 'Chosen image',
        description: 'Beacause reasons I chose this one!',
        url: './images/1_75/image3.jpg',
        tags: 'sea,cool,summer',
        rating: 4.6,
        currentUserRating: 2,
        comments: [{
            text: 'Ebasi qkata snimka',
            commentId: 1
        }, {
            text: 'Da be da, super tupa e!',
            commentId: 2
        }],
        user: 'veselin'
    }; */

    const LOCAL_STORAGE_USERNAME_KEY = 'USERNAME_KEY',
        LOCAL_STORAGE_AUTHKEY_KEY = 'AUTHENTICATION_KEY';

    function getImage(imageId) {
        return jsonRequester.get('/api/images/' + imageId)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function getAllImages(queryString) {
        return jsonRequester.get('/api/images' + queryString)
            .then(function(res) {
                // console.log(res);
                return res; // Check what to pass
            });
    }

    function uploadImage(imageDetails) {
        var options = {
            data: imageDetails
        };

        return jsonRequester.post('/api/images', options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function changeImage(imageDetails) {
        var options = {
            data: imageDetails
        };

        return jsonRequester.put('/api/images/' + imageDetails.imageId, options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function deleteImage(imageId) {
        return jsonRequester.delete('/api/images/' + imageId, options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function rateImage(mark, currentImageId) {
        var options = {
            data: {
                'user': localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY),
                'imageId': currentImageId,
                'mark': mark
            }
        };

        return jsonRequester.post('/api/marks', options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function commentImage(comment, currentImageId) {
        var options = {
            data: {
                'user': localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY),
                'imageId': currentImageId,
                'comment': comment
            }
        };

        return jsonRequester.post('/api/comments', options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function changeComment(comment, commentId) {
        var options = {
            data: {
                'user': localStorage.getItem(LOCAL_STORAGE_USERNAME_KEY),
                'commentId': commentId,
                'comment': comment
            }
        };

        return jsonRequester.put('/api/comments', options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    function deleteComment(commentId) {
        return jsonRequester.delete('/api/comments/' + commentId, options)
            .then(function(res) {
                return res; // Check what to pass
            });
    }

    return {
        getImage: getImage,
        getAll: getAllImages,
        upload: uploadImage,
        change: changeImage,
        delete: deleteImage,

        rateImage: rateImage,
        
        commentImage: commentImage,
        changeComment: changeComment,
        deleteComment: deleteComment
    };
}());

export {
    imageData
};