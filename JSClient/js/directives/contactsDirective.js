'use strict';
///<reference path="contactsDirective.js" />
webchat.directive('contacts', function(){
    return{
        restrict : 'E',
        templateUrl : 'partials/contacts-template.html',
        controller: 'contactsController'
    }
});