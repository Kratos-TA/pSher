/*jslint white: true */
import {
    slider
}
from './helpers/slider.js';

import {
    templates
}
from './templates.js';

import {
    searchController
}
from './controllers/searchController.js';

import {
    userController
}
from './controllers/userController.js';

import {
    imagesController
}
from './controllers/imagesController.js';

var appInitialize = (function() {
    /* use strict */

    // Checking if the module is successfully loaded
    console.log('App loaded successfully!');

    if (localStorage.USERNAME_KEY && localStorage.AUTHENTICATION_KEY) {
        $('#log').attr('href', '#/logout');
        $('#log').html('Logout');
    }

    // Load background
    templates.get('SliderTemplate')
        .then(function(template) {
            $('#backgroundContainer').html(template());
            jQuery(document).ready(slider.get());
        });

    // Introduce Sammy:
    window.sammyApp = Sammy('#container', function() {

        // Home/search routes
        this.get('#/', searchController.getMain);
        this.get('#/advancedSearch', searchController.getAdvanced);

        // User routes
        this.get('#/user', userController.getProfile);
        this.get('#/user/change/:username', userController.changeDetails);
        this.get('#/login', userController.login);
        this.get('#/logout', userController.logout);
        this.get('#/register', userController.register);

        // Images routes
        this.get('#/images', imagesController.getAll);
        this.get('#/images/:id', imagesController.getImage);
        this.get('#/createImages', imagesController.createImage);

        // Albums routes
        // this.get('#/albums', albumsController.getAll);
        // this.get('#/albums/:id', albumsController.getAlbum);
        // this.get('#/albums/create', albumsController.createAlbum);
        // this.get('#/albums/change/:id', albumsController.changeAlbum);
        // this.get('#/albums/delete/:id', albumsController.deleteAlbum);
    });

    $(function() {
        sammyApp.run('#/');

    });
}());

export default appInitialize;
