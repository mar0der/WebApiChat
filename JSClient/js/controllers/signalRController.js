/**
 * Created by ibaky on 9/6/2015.
 */
'use strict';

webchat.controller("signalRController", function ($rootScope, signalR) {
    signalR.on('pushGroupMessage', function (message) {
        $rootScope.$broadcast('newGroupMessageAdded', message);
    });

    signalR.on('seenGroupMessages', function (groupMessages) {
        $rootScope.$broadcast('updateGroupMessages', groupMessages);
    });

    signalR.on('userDisconnected', function (username) {
        $rootScope.$broadcast('userDisconnected', username);
    });

    signalR.on('userLogged', function (userLogged) {
        $rootScope.$broadcast('userLogged', userLogged);
    });

    signalR.on('pushSelfMessage', function (message) {
        $rootScope.$broadcast('pushSelfMessage', message);
    });


    signalR.on('pushMessageToClient', function (message) {
        $rootScope.$broadcast('newPrivateMessage', message);
    });
});
