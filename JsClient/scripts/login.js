/// <reference path="../node_modules/jquery/dist/jquery.js" />

// initiate auth popup
$("#connectButton").on("click", function () {
  SC.connect(function () {
    location.hash = "/"

    SC.get('/me', function (me) {
        $('<div/>').html('Hello, ' + me.username).appendTo($('#container'))
    });

  });
});
