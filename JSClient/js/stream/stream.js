'use strict';
///<reference path="contactsDirective.js" />
webchat.factory('signalR', ['$rootScope', 'configService', function ($rootScope, configService) {
    'use strict';

    return {
        on: function (eventName, callback) {
            var connection = $.hubConnection(configService.signalRUrl);
            var postHubProxy = connection.createHubProxy('baseHub');

            postHubProxy.on(eventName, function () {
                var args = arguments;
                $rootScope.$apply(function () {
                    callback.apply(postHubProxy, args);
                });
            });

            connection.start().done(function () { });

        }
    }
}]);