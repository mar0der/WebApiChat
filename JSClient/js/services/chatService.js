//<reference path="chatService.js" />

'use strict';
webchat.factory('chatService',function ($http, $q) {

    //var serviceUrl = webchat.BASE_URL + 'messages/';
    var service2 = webchat.BASE_URL + 'chat/';
    var service = {};

    service.getChatMessages = function () {
        var deferred = $q.defer();
        SetHeaders($http);
        $http.get(serviceUrl)
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (error) {
                deferred.reject(error);
            });
        return deferred.promise;
    };

    service.GetChatWithUser = function(userId){
        var deferred = $q.defer();
        SetHeaders($http);
        $http.get(service2 + userId)
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (error) {
                deferred.reject(error);
            });
        return deferred.promise;
    };
    
    service.sendMessage = function (message, chatId) {
        var deferred = $q.defer();
        SetHeaders($http);
        message['senderName'] = sessionStorage.username;
        $http.post(service2 +'send/' + chatId, message )
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (error) {
                deferred.reject(error);
            });
        return deferred.promise;
    };


    return service;


});