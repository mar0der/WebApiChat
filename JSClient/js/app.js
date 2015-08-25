'use strict';
var webchat = angular.module('webchat', ['ngRoute', 'ngResource']);

webchat.config(function($routeProvider) {

    $routeProvider
        .when('/chat', {
            templateUrl: 'partials/chat.html',
            controller: 'chatController'
        })
         .when('/login', {
             templateUrl: 'partials/login.html',
             controller: 'authenticationController'
        })
        .otherwise({ redirectTo: '/chat' });
});

webchat.BASE_URL = 'http://localhost:3660/api/';