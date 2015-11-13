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

var imagesController = (function() {
    /* use strict */

    var getAllImages = function(context) {
        var $container = $('#container');
        var galeriesName = context.params.name || '';
        var galeriesUser = context.params.user || '';
        var galeriesYear = context.params.tags || '';

        activeLink.toggle('#imagesLink');

        templates.get('SearchResults')
            .then(function(template) {
                var imageUrlArray = [];
                var i;

                // Her we should access data for images from DB
                for (i = 1; i <= 24; i++) {
                    var imageName = 'galery%20%28' + i + '%29.jpg';

                    if (imageName.indexOf(galeriesName) >= 0 && imageName.indexOf(galeriesUser) >= 0 && imageName.indexOf(galeriesYear) >= 0) {
                        var currentImage = {
                            url: './images/galeries/' + imageName,
                            link: '#/images/:' + i
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

        templates.get('Image')
            .then(function(template) {
                // Implement the galery itself

                // Access data for this specific galery below using currentGaleryId property!

                scrollFixedHelper.switchToScroll();
                // $container.html(template(...insert here the Galery template ready...));

                // Delete image functionality
                $('#deleteImgBtn').on('click', function() {
                    imageData.delete(currentImageId)
                        .then(function() {
                            sammyApp.refresh();
                        }, function(err) {
                            templates.get('AlertTemplate')
                                .then(function() {
                                    $container.html(template({
                                        //not sure wether it works like this
                                        alertText: err.responseJSON.toString()
                                    }));
                                    $('#okBtn').on('click', function() {
                                        sammyApp.refresh();
                                    });
                                });
                        });
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

                $('#sendBtn').on('click', function() {
                    var tags = $('#tagsInput').val();
                    var name = $('#nameInput').val();
                    var description = $('#descriptionInput').val();
                    var byteArray; /*To be implemented*/

                    if (!byteArray) {
                        templates.get('AlertTemplate')
                            .then(function(template) {
                                $container.html(template({
                                    alertText: 'You have not uploaded an image.'
                                }));
                                $('#okBtn').on('click', function() {
                                    sammyApp.refresh();
                                });
                            });
                        return;
                    }

                    var image = {
                        tags,
                        name,
                        description,
                        byteArray
                    };

                    imageData.upload(image)
                        .then(function() {
                            templates.get('AlertTemplate')
                                .then(function() {
                                    $container.html(template({
                                        alertText: 'You have uploaded successfully this image.'
                                    }));
                                    $('#okBtn').on('click', function() {
                                        sammyApp.refresh();
                                    });
                                });
                        }, function(err) {
                            templates.get('AlertTemplate')
                                .then(function() {
                                    $container.html(template({
                                        //not sure wether it works like this
                                        alertText: err.responseJSON.toString()
                                    }));
                                    $('#okBtn').on('click', function() {
                                        sammyApp.refresh();
                                    });
                                });
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
