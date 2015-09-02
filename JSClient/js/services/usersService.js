'use strict';

webchat.factory('usersService', function ($http, $q, authenticationService, configService) {
    var service = {};
    var serviceUrl = configService.baseServiceUrl + 'Account/';

    //POST api/users/Register	
    service.register = function (registerData) {
        return $http.post(serviceUrl + 'Register', registerData);
    };

    //POST api/users/Login	
    service.login = function (loginData) {
        var urlEncodedData = "username=" + loginData.username + "&password=" + loginData.password + "&grant_type=password";
        return $http.post(serviceUrl + 'Login', urlEncodedData, authenticationService.getLoginHeaders());
    };

    //POST api/users/Logout	
    service.logout = function () {
        return $http.post(serviceUrl + 'Logout', null, authenticationService.getHeaders());
    };

    //GET api/users/{username}/preview	
    service.getUserPreviewData = function getUserPreviewData(username) {
        return $http.get(serviceUrl + username + '/preview', authenticationService.getHeaders());
    }

    //GET api/users/search?searchTerm={searchTerm}	
    service.searchUserByName = function searchUserByName(searchTerm) {
        return $http.get(serviceUrl + 'search?searchTerm=' + searchTerm, authenticationService.getHeaders());
    }

    //GET api/users/{username}	
    service.getUserData = function getUserData(username) {
        return $http.get(serviceUrl + username, authenticationService.getHeaders());
    }

    //GET api/users/{username}/wall?StartPostId={StartPostId}&PageSize={PageSize}	
    service.getUsersWallByPages = function getFriendUsersByPages(friendName, startPostId, pageSize) {
        return $http.get(serviceUrl + friendName + '/wall?StartPostId=' + startPostId + '&PageSize=' + pageSize, authenticationService.getHeaders());
    }
    return service;
});