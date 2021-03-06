﻿'use strict';

webchat.factory('contactService', function ($http, $q, authenticationService, configService) {

    var serviceUrl = configService.baseServiceUrl + 'Contacts/';
    var service = {};

    service.getAllFriends = function () {
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl);
    };

    service.searchContact = function (searchPattern) {
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl + 'searchUser?searchPattern=' + searchPattern);
    };

    //service.searchInContacts = function (searchPattern) {
    //    authenticationService.setHeaders($http);
    //    return $http.get(serviceUrl + 'searchInContacts?searchPattern=' + searchPattern);
    //};

    service.addContact = function addContact(userId) {
        authenticationService.setHeaders($http);
        return $http.post(serviceUrl + userId);
    }

    return service;




});