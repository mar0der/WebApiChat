'use strict';
///<reference path="contactsDirective.js" />
webchat.factory('signalR', ['$rootScope', function ($rootScope) {
    'use strict';

    return {
        on: function (eventName, callback) {
            var connection = $.hubConnection('http://localhost:3660/signalr/');
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