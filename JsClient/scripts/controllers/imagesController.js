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
        var queryString;

        activeLink.toggle('#imagesLink');

        if (imageName.length === 0 && imageUser.length === 0 && imageTags.length === 0) {
            queryString = '';
        } else {
            queryString = '?name=' + imageName + '&user=' + imageUser + '&tags=' + imageTags;
        }

        templates.get('SearchResults')
            .then(function(template) {
                imageData.getAll(queryString)
                    .then(function(images) {


                        for (var i = 0; i < images.length; i++) {
                            images[i].link = '#/images/' + images[i].Id;
                        }
                        $container.html(template(images));
                        scrollFixedHelper.switchToScroll();

                        // For test only
                        // var images = imageData.getAll(queryString);
                        // var imagesLength = images.length;

                        // for (var i = 0; i < imagesLength; i++) {
                        //     images[i].link = '#/images/' + images[i].imageId.toString();
                        // }

                    }, function(err) {
                        return alertHelper.getGoHomeAlert('Images ' + err.statusText, context);
                    });

            });
    };

    var getImage = function(context) {
        var $container = $('#container');
        var currentImageId = this.params['id'];
        activeLink.toggle('#imagesLink');

        templates.get('Image')
            .then(function(template) {
                imageData.getImage(currentImageId)
                    .then(function(currentImage) {
                        console.log(currentImage);
                        if (currentImage.AuthorName === localStorage.USERNAME_KEY) {
                            currentImage.editLink = true;
                        }

                        currentImage.userMark = 0;
                        if (currentImage.Rating) {
                            var marks = currentImage.Rating.Marks;

                            for (var i = 0; i < marks.length; i++) {
                                if (marks[i].GivenBy === localStorage.USERNAME_KEY) {
                                    currentImage.userMark = marks[i].Value;
                                }
                            }
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
                    }, function(err) {
                        return alertHelper.getGoHomeAlert('Images ' + err.statusText, context)
                    });
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
                    var Tags = $('#imageTags').val().trim();
                    var Title = $('#imageName').val().trim();
                    var Description = $('#imageDescription').val().trim();
                    var file = document.getElementById('fileinput').files[0];

                    if (!file || !file.type.match('image.*')) {
                        return alertHelper.getOkAlert('You have not selected an image.');
                    }

                    $('#sendImage').html('uploading...');

                    var reader = new FileReader();
                    reader.readAsBinaryString(file);

                    reader.onload = function() {
                        var fileAsBinary = reader.result;
                        var b64encoded = btoa(fileAsBinary);
                        // console.log(fileAsBinary);
                        // console.log(b64encoded);

                        var IsPrivate = document.getElementById('isPrivate').checked;
                        var fileName = file.name;

                        var ImageInfo = {
                            OriginalName: fileName,
                            OriginalExtension: fileName.split('.').pop(),
                            Base64Content: b64encoded
                        };

                        var image = {
                            Tags,
                            Title,
                            Description,
                            IsPrivate,
                            ImageInfo
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

        console.log(currentImage);

        var $container = $('#container');
        activeLink.toggle('#imagesLink');

        templates.get('EditImage')
            .then(function(template) {
                $container.html(template(currentImage));
                scrollFixedHelper.switchToFixed();

                $('#sendImage').on('click', function() {

                    // Implement checks here!!!
                    var Tags = $('#imageTags').val().trim();
                    var Title = $('#imageName').val().trim();
                    var Description = $('#imageDescription').val().trim();
                    var IsPrivate = document.getElementById('isPrivate').checked;
                    var Id = currentImage.Id;

                    var image = {
                        Tags,
                        Title,
                        Description,
                        IsPrivate,
                        Id
                    };

                    imageData.change(image)
                        .then(function() {
                            return alertHelper.getOkAlert('You have edited successfully this image.');
                        }, function(err) {
                            return alertHelper.getOkAlert('Image ' + err.statusText);
                        });
                });
            });
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
