//<reference path="chatService.js" />

'use strict';
webchat.factory('chatService', function ($http, $q, authenticationService, configService) {

    var serviceUrl = configService.baseServiceUrl + 'chat/';
    var service = {};

    service.GetChatWithUser = function (userId) {
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl + userId);
    };

    service.sendMessage = function (message, chatId) {
        console.log(chatId);
        authenticationService.setHeaders($http);
        return $http.post(serviceUrl + chatId, message);
    };

    service.getUnreceived = function () {
        authenticationService.setHeaders($http);
        return $http.get(serviceUrl + 'unreceived');
    };

    //service.sendMessage = function (message, chatId) {
    //    var deferred = $q.defer();
    //    SetHeaders($http);
    //    message['senderName'] = sessionStorage.username;
    //    $http.post(serviceUrl +'send/' + chatId, message )
    //        .success(function (data) {
    //            deferred.resolve(data);
    //        }).error(function (error) {
    //            deferred.reject(error);
    //        });
    //    return deferred.promise;
    //};


    return service;


});