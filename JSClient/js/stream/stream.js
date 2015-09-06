'use strict';

webchat.factory('signalR', ['$rootScope', 'configService', function ($rootScope, configService) {

    var connection = $.hubConnection(configService.signalRUrl);
    var postHubProxy = connection.createHubProxy('baseHub');
    return {
        on: function (eventName, callback) {

            postHubProxy.on(eventName, function () {
                var args = arguments;
                $rootScope.$apply(function () {
                    callback.apply(postHubProxy, args);
                });
            });
            connection.start().done(function () { });
        },
        baseHub: postHubProxy
    }
}]);