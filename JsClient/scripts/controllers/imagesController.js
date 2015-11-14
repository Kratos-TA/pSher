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
                var queryString = '?name=' + imageName + '&user=' + imageUser + '&tags' + imageTags;

                // To refactor this with then! See below
                var images = imageData.getAll(queryString);
                var imagesLength = images.length;

                for (var i = 0; i < imagesLength; i++) {
                    images[i].link = '#/images/' + images[i].imageId.toString();
                }

                $container.html(template(images));
                scrollFixedHelper.switchToScroll();

                // .then(function(images) {

                // });


                // The old way - to be deleted
                // var imageUrlArray = [];
                // var i;

                // for (i = 1; i <= 24; i++) {
                //     var currentImageName = 'galery%20%28' + i + '%29.jpg';

                //     if (currentImageName.indexOf(imageName) >= 0 &&
                //         currentImageName.indexOf(imageUser) >= 0 &&
                //         currentImageName.indexOf(imageTags) >= 0) {
                //         var currentImage = {
                //             url: './images/galeries/' + currentImageName,
                //             link: '#/images/' + i
                //         };
                //         imageUrlArray.push(currentImage);
                //     }
                // }
                // var imageUrlContainerObject = {
                //     urls: imageUrlArray
                // };
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
                var currentImage = imageData.getImage(currentImageId);

                if (currentImage.user != localStorage.USERNAME_KEY) {
                    currentImage.user = false;
                }

                $container.html(template(currentImage));
                scrollFixedHelper.switchToUserFixed();



                // Delete image functionality
                $('#deleteImgBtn').on('click', function() {
                    imageData.delete(currentImageId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            return alertHelper.getOkAlert('Image ' + err.statusText);
                        });
                });

                $('#markInput').on('change', function(ev) {
                    var mark = $('#markInput').val();
                    imageData.rateImage(mark, currentImageId)
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
                    var currentCommentText = target.html();
                    var comment = {
                        commentId: currentCommentId,
                        commentText: currentCommentText
                    };

                    templates.get('EditCommentTemplate')
                        .then(function(template) {
                            $container.html(template(comment));
                            scrollFixedHelper.switchToFixed();

                            $('#editBtn').on('click', function() {
                                var editedCommentText = $('#editedComment').val();

                                imageData.changeComment(editedCommentText, currentCommentId)
                                    .then(function() {
                                        sammyApp.refresh();
                                    }, function(err) {
                                        return alertHelper.getOkAlert('Comment ' + err.statusText);
                                    });

                            });

                            $('#cancelBtn').on('click', function() {
                                sammyApp.refresh();
                            });
                            $('#deleteBtn').on('click', function() {
                                imageData.deleteComment(currentCommentId)
                                    .then(function() {
                                        sammyApp.refresh();
                                    }, function(err) {
                                        return alertHelper.getOkAlert('Comment ' + err.statusText);
                                    });
                            });
                        });
                });

                $('#editLink').on('click', function() {
                    editImage(context, currentImage);
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

                $('#sendImage').on('click', function() {
                    var tags = $('#imageTags').val();
                    var name = $('#imageName').val();
                    var description = $('#imageDescription').val();
                    var file = document.getElementById('fileinput').files[0];

                    if (!file || !file.type.match('image.*')) {
                        return alertHelper.getOkAlert('You have not selected an image.');
                    }

                    $('#sendImage').html('uploading...');

                    var reader = new FileReader();
                    reader.readAsBinaryString(file);

                    reader.onload = function() {
                        var binary = reader.result;
                        console.log(binary);
                        var image = {
                            tags,
                            name,
                            description,
                            binary
                        };

                        imageData.upload(image)
                            .then(function() {
                                return alertHelper.getOkAlert('You have uploaded successfully this image.');
                            }, function(err) {
                                return alertHelper.getOkAlert('Image ' + err.statusText);
                            });
                    };


                });
            });
    };

    var editImage = function(context, currentImage) {
        var $container = $('#container');
        activeLink.toggle('#imagesLink');

        templates.get('EditImage')
            .then(function(template) {
                $container.html(template(currentImage));
                scrollFixedHelper.switchToFixed();

                // Upload images functionality!!!!!!

                $('#sendImage').on('click', function() {
                    var tags = $('#imageTags').val();
                    var name = $('#imageName').val();
                    var description = $('#imageDescription').val();
                    var image = {
                        tags,
                        name,
                        description,
                        imageId: currentImage.id
                    };

                    imageData.change(image)
                        .then(function() {
                            return alertHelper.getOkAlert('You have uploaded successfully this image.');
                        }, function(err) {
                            return alertHelper.getOkAlert('Image ' + err.statusText);
                        });
                });
            });

        // TODO: Implement the edit which will look like the add without uploading!!!
    };

    return {
        getAll: getAllImages,
        getImage: getImage,
        createImage: createImage,
        editImage: editImage
    };
}());

export {
    imagesController
};
