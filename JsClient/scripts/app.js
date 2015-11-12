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
    credentialsController
}
from './controllers/credentialsController.js';

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

    // // Get the slider template
    // templates.get('SliderTemplate')
    //     .then(function(template) {
    //         var backgroundImageContainer = $('#backgroundContainer');
    //         //  backgroundImageContainer.html(template(imageUrlContainerObject));
    //         backgroundImageContainer.html(template());
    //         // Load slider
    //         jQuery(document).ready(slider.get());
    //     });


    // Introduce Sammy:
    window.sammyApp = Sammy('#container', function() {

        this.get('#/', searchController.getMain);
        this.get('#/advancedSearch', searchController.getAdvanced);
        this.get('#/profile/:username', userController.getProfile);
        this.get('#/login', credentialsController.login);
        this.get('#/logout', credentialsController.logout);
        this.get('#/register', credentialsController.register);
        this.get('#/images', imagesController.getAll);
        this.get('#/images/:id', imagesController.getImage);



        // this.get('#/', function(context) {
        //     var $container = $('#container');
        //     $('#backgroundContainer').css('display', 'block');
        //     toggleActiveLink('#homeLink');

        //     templates.get('SearchFormTemplate')
        //         .then(function(template) {
        //             $container.html(template());
        //             $container.removeClass('searchResultsContainer');
        //             $container.addClass('cover');

        //             $('#searchPhotoBtn').on('click', function() {
        //                 var queryText = $('#photoSearcher').val();
        //                 console.log(queryText);
        //                 context.redirect('#/galeries' + '?name=' + queryText);
        //             });
        //         });
        // });

        // this.get('#/profile/:username', function() {
        //     var $container = $('#container');
        //     $('#backgroundContainer').css('display', 'none');
        //     toggleActiveLink('#profileLink');

        //     templates.get('ProfilePage')
        //         .then(function(template) {
        //             // Still not implemented
        //             $container.html(template);
        //             $container.removeClass('searchResultsContainer');
        //             $container.addClass('cover');
        //         });
        // });

        // this.get('#/login', function(context) {
        //     var $container = $('#container');
        //     $('#backgroundContainer').css('display', 'block');
        //     toggleActiveLink('#logLink');

        //     // Displlay login page
        //     templates.get('LoginTemplateNew')
        //         .then(function(template) {
        //             $container.html(template);
        //             $container.removeClass('searchResultsContainer');
        //             $container.addClass('cover');
        //             // System.import('scripts/login.js');
        //             $('#loginBtn').on('click', function() {
        //                 var user = {
        //                     username: $('#userName').val(),
        //                     password: $('#userPassword').val()
        //                 };
        //                 $.ajax({
        //                     method: 'POST',
        //                     // ?? which will it be? --> url: 'http://localhost:3000/users',
        //                     data: JSON.stringify(user),
        //                     contentType: 'application/json',
        //                     success: function() {
        //                         window.alert('Loged in!');
        //                         localStorage.setItem('USERNAME_STORAGE_KEY', user.username);
        //                         localStorage.setItem('PASS_STORAGE_KEY', user.password);
        //                         $('#log').attr('href', '#/logout');
        //                         $('#log').html('Logout');
        //                         context.redirect('#/');
        //                     },
        //                     error: function() {
        //                         window.alert('Failed to log user!');
        //                     }
        //                 });
        //             });
        //         });
        // });

        // this.get('#/logout', function(context) {
        //     var $container = $('#container');

        //     localStorage.removeItem('PASS_STORAGE_KEY');
        //     localStorage.removeItem('USERNAME_STORAGE_KEY');

        //     $('#log').attr('href', '#/login');
        //     $('#log').html('Login');
        //     context.redirect('#/');
        // });

        // this.get('#/galeries', function(context) {
        //     var $container = $('#container');
        //     $('#backgroundContainer').css('display', 'none');
        //     toggleActiveLink('#galeriesLink');

        //     // Use query strings in Sammy.js => "#/galeries/?name=aaaa&user=bbb&year=ccc"
        //     var galeriesName = context.params.name || '';
        //     var galeriesUser = context.params.user || '';
        //     var galeriesYear = context.params.year || '';

        //     templates.get('SearchResults')
        //         .then(function(template) {
        //             var imageUrlArray = [];
        //             var i;

        //             // Her we should access data for galeries from DB
        //             for (i = 1; i <= 24; i++) {
        //                 var imageName = 'galery%20%28' + i + '%29.jpg';

        //                 if (imageName.indexOf(galeriesName) >= 0 && imageName.indexOf(galeriesUser) >= 0 && imageName.indexOf(galeriesYear) >= 0) {
        //                     var currentImage = {
        //                         url: './images/galeries/' + imageName,
        //                         link: '#/galeries/:' + i
        //                     };
        //                     imageUrlArray.push(currentImage);
        //                 }
        //             }
        //             var imageUrlContainerObject = {
        //                 urls: imageUrlArray
        //             };

        //             $container.addClass('searchResultsContainer');
        //             $container.removeClass('cover');
        //             $container.html(template(imageUrlContainerObject));
        //         });
        // });

        // this.get('#/galeries/:id', function(context) {
        //     var $container = $('#container');
        //     var currentGaleryId = this.params['id'];
        //     toggleActiveLink('#galeriesLink');

        //     $('#backgroundContainer').css('display', 'none');

        //     templates.get('Galery')
        //         .then(function(template) {
        //             // Implement the galery itself

        //             // Access data for this specific galery below using currentGaleryId property!

        //             $container.removeClass('searchResultsContainer');
        //             $container.addClass('cover');
        //             // $container.html(template(...insert here the Galery template ready...));
        //         });
        // });

        // this.get('#/advancedSearch', function(context) {
        //     var $container = $('#container');
        //     $('#backgroundContainer').css('display', 'block');
        //     toggleActiveLink('#homeLink');

        //     templates.get('AdvancedSearchTemplate')
        //         .then(function(template) {
        //             $container.html(template());
        //             $container.removeClass('searchResultsContainer');
        //             $container.addClass('cover');

        //             $('#advancedSearchPhotoBtn').on('click', function() {
        //                 var name = $('#photoSearcherByName').val();
        //                 var user = $('#photoSearcherByUser').val();
        //                 var year = $('#photoSearcherByYear').val();
        //                 context.redirect('#/galeries' + '?name=' + name + '&user=' + user + '&year=' + year);
        //             });
        //         });
        // });
    });

    $(function() {
        sammyApp.run('#/');

    });

    
}());

export default appInitialize;
