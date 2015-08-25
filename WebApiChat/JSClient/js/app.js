'use strict';
var app = angular.module('webchat', ['ngRoute', 'ngResource']);

app.config(function ($routeProvider, $httpProvider) {
  //  $httpProvider.interceptors.push('httpResponseInterceptorService');
    $routeProvider
        .when('/chat', {
            templateUrl: 'partials/chat.html',
            controller: 'chatController'
        })

        .otherwise({ redirectTo: '/chat' });
});