'use strict';

webchat.factory('groupService', function ($http, $q, authenticationService, configService) {

    var serviceUrl = configService.baseServiceUrl + 'group/';
    var service = {};

    service.getAllGroups = function () {
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl);
    };

    service.getMessageByGroup = function(id){
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl + 'getMessages/'+ id);
    };

    service.SendMessageToGroup = function(messageData){
        authenticationService.setHeaders($http);
        return $http.post(serviceUrl + 'addMessage', messageData)
    }


    return service;

});