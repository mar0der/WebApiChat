///<reference path="contactService.js" />

'use strict';
webchat.factory('contactService', function ($http, $q) {

    var serviceUrl = webchat.BASE_URL + 'Contacts';
    var service = {};

    service.getAllContacts = function () {
        var deferred = $q.defer();
        SetHeaders($http);
        $http.get(serviceUrl)
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (error) {
                deferred.reject(error);
            });

        return deferred.promise;
    }

    return service;


});