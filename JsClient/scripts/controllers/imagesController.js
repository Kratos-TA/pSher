/*jslint white: true */

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
    imageData
}
from '../data/imageData.js';

import {
    alertHelper
}
from '../helpers/alertHelper.js';

var imagesController = (function() {
    /* use strict */

    var getAllImages = function(context) {
        var $container = $('#container');
        var imageName = context.params.name || '';
        var imageUser = context.params.user || '';
        var imageTags = context.params.tags || '';

        activeLink.toggle('#imagesLink');

        templates.get('SearchResults')
            .then(function(template) {
                var imageUrlArray = [];
                var i;

                // Here we should access data for images from DB

                // var queryString = '?name='+imageName + '&user=' + imageUser + '&tags' + imageTags;
                // imageData.getAll(queryString)
                //     .then(function() {

                //     });


                for (i = 1; i <= 24; i++) {
                    var currentImageName = 'galery%20%28' + i + '%29.jpg';

                    if (currentImageName.indexOf(imageName) >= 0 &&
                        currentImageName.indexOf(imageUser) >= 0 &&
                        currentImageName.indexOf(imageTags) >= 0) {
                        var currentImage = {
                            url: './images/galeries/' + currentImageName,
                            link: '#/images/' + i
                        };
                        imageUrlArray.push(currentImage);
                    }
                }
                var imageUrlContainerObject = {
                    urls: imageUrlArray
                };

                $container.html(template(imageUrlContainerObject));
                scrollFixedHelper.switchToScroll();
            });
    };

    var getImage = function(context) {
        var $container = $('#container');
        var currentImageId = this.params['id'];
        activeLink.toggle('#imagesLink');

        // Implement the galery itself
        // iMPLEMENT HERE EVERITHING --> CONNECTED WITH 
        // Access data for this specific galery below using currentGaleryId property!
        templates.get('Image')
            .then(function(template) {
                // Fix it as it should work with services
                console.log('Before templating');
                var currentImage = imageData.getImage(currentImageId);
                scrollFixedHelper.switchToUserFixed();
                $container.html(template(currentImage));
                console.log('After templating');


                // Delete image functionality
                $('#deleteImgBtn').on('click', function() {
                    imageData.delete(currentImageId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            return alertHelper.getOkAlert('Image ' + err.statusText);
                        });
                });

                $('#rateImage').on('click', function() {
                    var mark = $('#markInput').val();
                    if (!mark) {
                        return alertHelper.getOkAlert('You have not entered a mark.');
                    }

                    if (0 > mark || mark > 5) {
                        return alertHelper.getOkAlert('You have given an invalid mark.');
                    }

                    imageData.rateImage(mark, currentImageId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            return alertHelper.getOkAlert('Mark ' + err.statusText);
                        });
                });

                $('#deleteMarkBtn').on('click', function() {
                    imageData.deleteMark(currentImageId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            return alertHelper.getOkAlert('Mark ' + err.statusText);
                        });
                });

                $('#leaveComment').on('click', function() {
                    var comment = $('#commentInput').val();
                    if (!comment) {
                        return alertHelper.getOkAlert('You have not entered a comment.');
                    }

                    // Validate comment!!!

                    imageData.commentImage(comment, currentImageId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            return alertHelper.getOkAlert('Comment ' + err.statusText);
                        });
                });

                $('#comments').on('click', function(ev) {
                    var target = $(ev.target);
                    var currentCommentId = target.attr('commentId');
                    var currentCommentText = target.val();

                    // Distplay the template for edditing the comment
                    // Get the text from the new comment
                    var comment;

                    // then:!!!!!!!!!!!!!!!

                    imageData.changeComment(comment, currentCommentId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            return alertHelper.getOkAlert('Comment ' + err.statusText);
                        });
                });


                // Should follow with then as below!
                // .then(function() {
                //     scrollFixedHelper.switchToScroll();
                //     // $container.html(template(...insert here the Galery template ready...));

                //     // Delete image functionality
                //     $('#deleteImgBtn').on('click', function() {
                //         imageData.delete(currentImageId)
                //             .then(function() {
                //                 sammyApp.refresh();
                //             }, function(err) {
                //                 return alertHelper.getOkAlert('Image ' + err.statusText);
                //             });
                //     });

                //     $('#rateImage').on('click', function() {
                //         var mark = $('#markInput').val();
                //         if (!mark) {
                //             return alertHelper.getOkAlert('You have not entered a mark.');
                //         }

                //         if (0 > mark || mark > 5) {
                //             return alertHelper.getOkAlert('You have given an invalid mark.');
                //         }

                //         imageData.rateImage(mark, currentImageId)
                //             .then(function() {
                //                 sammyApp.refresh();
                //             }, function(err) {
                //                 return alertHelper.getOkAlert('Mark ' + err.statusText);
                //             });
                //     });

                //     $('#deleteMarkBtn').on('click', function() {
                //         imageData.deleteMark(currentImageId)
                //             .then(function() {
                //                 sammyApp.refresh();
                //             }, function(err) {
                //                 return alertHelper.getOkAlert('Mark ' + err.statusText);
                //             });
                //     });

                //     $('#leaveComment').on('click', function() {
                //         var comment = $('#commentInput').val();
                //         if (!comment) {
                //             return alertHelper.getOkAlert('You have not entered a comment.');
                //         }

                //         // Validate comment!!!

                //         imageData.commentImage(comment, currentImageId)
                //             .then(function() {
                //                 sammyApp.refresh();
                //             }, function(err) {
                //                 return alertHelper.getOkAlert('Comment ' + err.statusText);
                //             });
                //     });

                //     $('#comments').on('click', function(ev) {
                //         var target = $(ev.target);
                //         var currentCommentId = target.attr('commentId');
                //         var currentCommentText = target.val();

                //         // Distplay the template for edditing the comment
                //         // Get the text from the new comment
                //         var comment;

                //         // then:!!!!!!!!!!!!!!!

                //         imageData.changeComment(comment, currentCommentId)
                //             .then(function() {
                //                 sammyApp.refresh();
                //             }, function(err) {
                //                 return alertHelper.getOkAlert('Comment ' + err.statusText);
                //             });
                //     });
                // });
            });
    };

    var createImage = function(context) {
        var $container = $('#container');
        activeLink.toggle('#imagesLink');

        templates.get('CreateImage')
            .then(function(template) {
                $container.html(template);
                scrollFixedHelper.switchToFixed();

                // Upload images functionality!!!!!!

                $('#sendBtn').on('click', function() {
                    var tags = $('#tagsInput').val();
                    var name = $('#nameInput').val();
                    var description = $('#descriptionInput').val();
                    var byteArray; /*To be implemented*/

                    if (!byteArray) {
                        return alertHelper.getOkAlert('You have not uploaded an image.');
                    }

                    var image = {
                        tags,
                        name,
                        description,
                        byteArray
                    };

                    imageData.upload(image)
                        .then(function() {
                            return alertHelper.getOkAlert('You have uploaded successfully this image.');
                        }, function(err) {
                            return alertHelper.getOkAlert('Image ' + err.statusText);
                        });
                });
            });
    };

    return {
        getAll: getAllImages,
        getImage: getImage,
        createImage: createImage
    };
}());

export {
    imagesController
};
