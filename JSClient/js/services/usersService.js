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

    //GET api/users/search?searchTerm={searchTerm}	
    service.searchUserByName = function searchUserByName(searchTerm) {
        return $http.get(serviceUrl + 'search?searchTerm=' + searchTerm, authenticationService.getHeaders());
    }

    //GET api/users/{username}	
    service.getUserData = function getUserData(username) {
        return $http.get(serviceUrl + username, authenticationService.getHeaders());
    }

    //Post api/users/userStatusUpdate
    service.userStatusUpdate = function userStatusUpdate() {
        return $http.post(configService.baseServiceUrl + 'users/userStatusUpdate', authenticationService.getHeaders());
    };

    service.updateUserCurrentChatId = function udpateUserCurrentChatId(chatId) {
        return $http.put(configService.baseServiceUrl + 'chat/updateUserCurrentChatId/' + chatId,
            authenticationService.getHeaders());
    };

    return service;
});