'use strict';
var webchat = angular.module('webchat', ['ngRoute', 'ngResource']);

webchat.config(function($routeProvider) {

    $routeProvider

         .when('/login', {
             templateUrl: 'partials/login.html',
              controller: 'authenticationController'
        })
        .when('/chat', {
            templateUrl: 'partials/online-users-template.html',
            controller: 'contactsController'
        })
        .otherwise({ redirectTo: '/login' });
});

webchat.BASE_URL = 'http://localhost:3660/api/';