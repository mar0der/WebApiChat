﻿/**
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


    signalR.on('pushMessageToClient', function (message) {
        console.log('bah maa mu');
        $rootScope.$broadcast('newPrivateMessage', message);
    });
});