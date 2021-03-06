﻿'use strict';

webchat.controller("authenticationController", function ($scope, usersService, authenticationService, $location, signalR, $rootScope, usSpinnerService) {
    var clearData = function () {
        //$scope.loginData = "";
        $scope.registerData = "";
        $scope.userData = "";
        $scope.passwordData = "";
    };
    $scope.me = authenticationService.getUsername();

    if ($scope.me && $scope.me.length > 0) {
        setTimeout(function () {
            usersService.userStatusUpdate();
            usersService.updateUserCurrentChatId(null);
            $rootScope.currentPrivateChatReceiverId = "";
        }, 200);
    }

    $scope.login = function login(loginData) {
        usSpinnerService.spin('spinner');
        usersService.login(loginData)
        .then(function (serverData) {
            //notyService.showInfo("Successful Login!");
            authenticationService.setCredentials(serverData.data);
            $rootScope.$broadcast('login');
            clearData();
            usSpinnerService.stop('spinner');
            $location.path('/');
        },
        function (serverError) {
            usSpinnerService.stop('spinner');
            console.log(serverError);
            //notyService.showError("Unsuccessful Login!", serverError);
        });
    };

    $scope.register = function register(registerData) {
        //usSpinnerService.spin('spinner');
        usSpinnerService.spin('spinner');
        usersService.register(registerData)
        .then(function (serverData) {
            //notyService.showInfo("Successful Registeration!");
            authenticationService.setCredentials(serverData.data);
            $rootScope.$broadcast('login');
            clearData();
            usSpinnerService.stop('spinner');
            //usSpinnerService.stop('spinner');
            $location.path('/');
        },
        function (serverError) {
            usSpinnerService.stop('spinner');
            //usSpinnerService.stop('s
            // pinner');
            //notyService.showError("Unsuccessful Registeration!", serverError);
        });
    };

    $scope.logout = function logout() {
        usersService.logout()
            .then(function () {
                authenticationService.clearCredentials();
                $rootScope.$broadcast('logout');
                sessionStorage.clear();
                //notyService.showInfo("Successful Logout!");
                $location.path('/login');
            }, function () {
                authenticationService.clearCredentials();
                $location.path('/');
            });
    }

    $scope.isLoggedIn = function isLoggedIn() {
        return authenticationService.isLoggedIn();
    }
});