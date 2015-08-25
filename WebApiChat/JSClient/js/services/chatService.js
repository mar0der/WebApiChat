'use strict';
app.factory('chatService', ['SignalR'])
.factory('Employees', ['$rootScope', 'Hub', '$timeout', function ($rootScope, Hub, $timeout) {

    //declaring the hub connection
    var hub = new Hub('mesagessHub', {

        //client side methods
        listeners: {
            'receiveMessage': function (msg) {
                alert(1);
            }
        },

        //server side methods
        methods: ['lock', 'unlock'],

        //query params sent on initial connection
        queryParams: {
            'token': 'exampletoken'
        },

        //handle connection error
        errorHandler: function (error) {
            console.error(error);
        },

        //specify a non default root
        //rootPath: '/api

        stateChanged: function (state) {
            switch (state.newState) {
                case $.signalR.connectionState.connecting:
                    //your code here
                    break;
                case $.signalR.connectionState.connected:
                    //your code here
                    break;
                case $.signalR.connectionState.reconnecting:
                    //your code here
                    break;
                case $.signalR.connectionState.disconnected:
                    //your code here
                    break;
            }
        }
    });

    var edit = function (employee) {
        hub.lock(employee.Id); //Calling a server method
    };
    var done = function (employee) {
        hub.unlock(employee.Id); //Calling a server method
    }

    return {
        editEmployee: edit,
        doneWithEmployee: done
    };
}]);