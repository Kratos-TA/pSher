/*jslint white: true */

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

    // Introduce Sammy:
    window.sammyApp = Sammy('#container', function() {

        // Home/search routes
        this.get('#/', searchController.getMain);
        this.get('#/advancedSearch', searchController.getAdvanced);

        // User routes
        this.get('#/user/:username', userController.getProfile);
        this.get('#/user/delete/:username', userController.deleteUser);
        this.get('#/user/change/:username', userController.changeDetails);
        this.get('#/login', userController.login);
        this.get('#/logout', userController.logout);
        this.get('#/register', userController.register);
        
        // Images routes
        this.get('#/images', imagesController.getAll);
        this.get('#/images/:id', imagesController.getImage);
        this.get('#/images/create', imagesController.createImage);
        // this.get('#/images/change/:id', imagesController.changeImage);
        // this.get('#/images/delete/:id', imagesController.deleteImage);


        // Marks routes
        this.get('#marks/create', marksController.create);
        this.get('#marks/change/:id', marksController.change);
        this.get('#marks/delete/:id', marksController.delete);

        // Comments routes
        this.get('#comments/create', commentsController.create);
        this.get('#comments/change/:id', commentsController.change);
        this.get('#comments/delete/:id', commentsController.delete);

        // Images routes
        this.get('#/albums', albumsController.getAll);
        this.get('#/albums/:id', albumsController.getAlbum);
        this.get('#/albums/create', albumsController.createAlbum);
        this.get('#/albums/change/:id', albumsController.changeAlbum);
        this.get('#/albums/delete/:id', albumsController.deleteAlbum);
    });

    $(function() {
        sammyApp.run('#/');

    });

    
}());

export default appInitialize;
