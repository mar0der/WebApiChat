'use strict';

webchat.factory('configService', function () {
    var config = {
        baseServiceUrl: 'http://localhost:3660/api/',
        signalRUrl: 'http://localhost:3660/signalr/'
    };

    return config;
});