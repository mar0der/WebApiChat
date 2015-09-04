///<reference path="contactService.js" />

'use strict';
webchat.factory('contactService', function ($http, $q, authenticationService, configService) {

    var serviceUrl = configService.baseServiceUrl +'Contacts/';
    var service = {};

    service.getAllFriends = function(){
        authenticationService.setHeaders($http);
         return $http.get(serviceUrl);
    };

    service.searchContact = function (username) {
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl + 'searchByUsername?username=' + username);
    };

    return service;




});