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

                    if (!name || name.length < 3) {
                        name = '';
                    }
                    if (!user || user.length < 3) {
                        user = '';
                    }

                    // Check if this plit is correct!!!!
                    var tagsArray = tags.split(',');
                    tags = '';
                    var len = tagsArray.length;
                    for (var i = 0; i < len; i++) {
                        if (tagsArray[i] && tagsArray[i].length > 2) {
                            tags += tags[i];
                        }
                    }

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
