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
            });
    };

    return {
        getAll: getAllImages,
        getImage: getImage
    };
}());

export {
    imagesController
};
