/*jslint white: true */

var constants = (function() {
	/* use strict */

    // Checking if the module is successfully loaded
    console.log('Constants successfully loaded!');

    return {
        MIN_USERNAME_LENGTH: 2,
        MAX_USERNAME_LENGTH: 9
    };
}());

export {constants};
