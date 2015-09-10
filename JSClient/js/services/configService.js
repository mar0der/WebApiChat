'use strict';

webchat.factory('configService', function () {
    var config = {
        baseServiceUrl: 'http://viber.apphb.com/api/',
        signalRUrl: 'http://viber.apphb.com/signalr/'
    };

    return config;
});