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

var searchController = (function() {
    /* use strict */

    console.log('çontroller loaded successfully');

    var getMainSearch = function(context) {
        var $container = $('#container');
        activeLink.toggle('#homeLink');

        templates.get('SearchFormTemplate')
            .then(function(template) {
                $container.html(template());
                scrollFixedHelper.switchToFixed();

                $('#searchPhotoBtn').on('click', function() {
                    var queryText = $('#photoSearcher').val();
                    context.redirect('#/images' + '?name=' + queryText);
                });
            });
    };

    var getAdvancedSearch = function(context) {
        var $container = $('#container');
        activeLink.toggle('#homeLink');

        templates.get('AdvancedSearchTemplate')
            .then(function(template) {
                $container.html(template());
                scrollFixedHelper.switchToFixed();

                $('#advancedSearchPhotoBtn').on('click', function() {
                    var name = $('#photoSearcherByName').val();
                    var user = $('#photoSearcherByUser').val();
                    var tags = $('#photoSearcherByTags').val();
                    context.redirect('#/images' + '?name=' + name + '&user=' + user + '&tags=' + tags);
                });
            });
    };

    return {
        getMain: getMainSearch,
        getAdvanced: getAdvancedSearch
    };
}());

export {
    searchController
};
