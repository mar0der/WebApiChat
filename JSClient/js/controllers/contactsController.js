'use strict';

webchat.controller("contactsController", function ($scope, contactService, $location, $rootScope) {
    $scope.getAllContacts = function () {
        contactService.getAllContacts()
            .then(function (data) {
                $rootScope.contacts = data;
                console.log(data);
            }, function (error) {
                console.error(error);
            });
    }
});
