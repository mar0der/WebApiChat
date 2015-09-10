'use strict';

webchat.factory('authenticationService', function () {
    var service = {};

    service.setCredentials = function setCredentials(serverData) {
        sessionStorage['accessToken'] = serverData.access_token;
        sessionStorage['username'] = serverData.userName;
    };

    service.getUsername = function getUsername() {
        return sessionStorage['username'];
    };

    service.clearCredentials = function clearCredentials() {
        sessionStorage.clear();
    };

    service.getLoginHeaders = function getLoginHeaders() {
        return {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }
    };

    service.getHeaders = function getHeaders() {
        return {
            Authorization: "Bearer " + sessionStorage['accessToken']
        }
    };

    service.isLoggedIn = function isLoggedIn() {
        if (sessionStorage['accessToken']) {
            return true;
        }
        return false;
    };

    service.setHeaders = function ($http) {
        $http.defaults.headers.common = service.getHeaders();
    };

    return service;
});