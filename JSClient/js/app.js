'use strict';

var webchat = angular.module('webchat', ['ngRoute', 'ngResource']);

webchat.config(function ($routeProvider) {

    $routeProvider
        .when('/register', {
            templateUrl: 'partials/register.html'
        })
        .when('/login', {
            templateUrl: 'partials/login.html'
        })
        .when('/test', {
            templateUrl: 'partials/test.html'
        })
        .when('/', {
            templateUrl: 'partials/chatContainer.html',
            controller: 'chatController'
        })
        .otherwise({ redirectTo: '/login' });
});

webchat.run(function ($rootScope, $location, authenticationService) {
    $rootScope.$on('$locationChangeStart', function (event) {
        if ($location.path().indexOf('login') === -1 && $location.path().indexOf('register') === -1 && !authenticationService.isLoggedIn()) {
            $location.path('/login');
        }
        var deniedPaths = ['/login', '/register'];

        if (authenticationService.isLoggedIn() && deniedPaths.indexOf($location.path()) !== -1) {
            $location.path("/");
        }
    });
});
