'use strict';
///<reference path="authenticationController.js" />

webchat.controller("authenticationController", function ($scope, userServices, $location) {
    $scope.login = function () {

        userServices.login($scope.loginData)
            .then(function (data) {
                console.log(data);
                SetCredentials(data);
                //$location.path('/chat');
                $scope.username = sessionStorage['username'];
            }, function (error) {
                console.error(error);
            }).finally(function () {
            });
    };
});