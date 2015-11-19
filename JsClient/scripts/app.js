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

import {
    alertHelper
}
from './helpers/alertHelper.js';

var appInitialize = (function() {
    /* use strict */

    // Checking if the module is successfully loaded
    console.log('App loaded successfully!');


    // Subscribe here to PubNub
    var pubnub = PUBNUB.init({
        publish_key: 'pub-c-8e80f845-211b-439e-9f31-bf15f247d4a3',
        subscribe_key: 'sub-c-e5b4d678-8c6b-11e5-bf00-02ee2ddab7fe'
    });

    pubnub.subscribe({
        channel: 'pSher',
        message: function(mess) {
            alertHelper.getOkAlert(mess);
        }
    });

    // I dont know where is supposed to use this
    // Should be implemented like that but really what should users publish to PubNub
    // pubnub.publish({
    //     channel: 'pSher',
    //     // message: {
    //     //     "color": "blue"
    //     // }
    // });

    // Check if user is logged
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
