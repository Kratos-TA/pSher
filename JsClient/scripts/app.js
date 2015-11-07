 import {
     templates
 }
 from '../templates/templates.js';

 // import { playlistControler } './Controllers/PlayGenreController.js';

 var appInitialize = (function() {
     // Checking if the module is successfully loaded
     console.log('App loaded successfully!');

     if (localStorage.USERNAME_STORAGE_KEY && localStorage.PASS_STORAGE_KEY) {
        $('#log').attr('href', '#/logout');
        $('#log').html('Logout');
     }

     templates.get('SliderTemplate')
         .then(function(template) {
            var backgroundImageContainer = $('#backgroundContainer');
            //  backgroundImageContainer.html(template(imageUrlContainerObject));
             backgroundImageContainer.html(template());
         });


     // Introduce Sammy:
     window.sammyApp = Sammy('#container', function() {

         this.get('#/', function(context) {
             var $container = $('#container');
             $('#backgroundContainer').css('display', 'block');

             templates.get('SearchFormTemplate')
                 .then(function(template) {
                     $container.html(template());
                     $container.removeClass('searchResultsContainer');
                     $container.addClass('cover');

                     $('#searchPlaylistBtn').on('click', function() {
                         context.redirect('#/searchresult');
                     });
                 });
         });

         this.get('#/profile:username', function() {
             var $container = $('#container');
             $('#backgroundContainer').css('display', 'none');

             templates.get('ProfilePage')
                 .then(function(template) {
                     // 
                 });
         });

         this.get('#/login', function(context) {
             var $container = $('#container');
             // $('#backgroundContainer').css('display', 'none');

             // Displlay login page
             templates.get('LoginTemplateNew')
                 .then(function(template) {
                     $container.html(template);
                     // System.import('scripts/login.js');
                     $('#loginBtn').on('click', function() {
                        var user = {
                            username: $('#userName').val(),
                            password: $('#userPassword').val()
                        };
                        $.ajax({
                            method: 'POST',
                            url: 'http://localhost:3000/users',
                            data: JSON.stringify(user),
                            contentType: 'application/json',
                            success: function() {
                                window.alert('Loged in!');
                                localStorage.setItem('USERNAME_STORAGE_KEY', user.username);
                                localStorage.setItem('PASS_STORAGE_KEY', user.password);
                                $('#log').attr('href', '#/logout');
                                $('#log').html('Logout');
                                context.redirect('#/');
                            },
                            error: function() {
                                window.alert('Failed to log user!');
                            }
                        });
                     });
                 });             
         });

         this.get('#/logout', function(context) {
            var $container = $('#container');

            localStorage.removeItem('PASS_STORAGE_KEY');
            localStorage.removeItem('USERNAME_STORAGE_KEY');

            $('#log').attr('href', '#/login');
            $('#log').html('Login');
            context.redirect('#/');
         });
     });

     $(function() {
         sammyApp.run('#/');

     });
 }());

 export default appInitialize;
