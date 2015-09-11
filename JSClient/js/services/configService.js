'use strict';

webchat.factory('configService', function () {
    var config = {
        baseServiceUrl: 'http://viber.azurewebsites.net/api/',
        signalRUrl: 'http://viber.azurewebsites.net/signalr/'
    };

    return config;
});