'use strict';
///<reference path="authenticationController.js" />

webchat.controller("authenticationController", function ($scope, userServices, $location) {
    $scope.login = function () {

        userServices.login($scope.loginData)
            .then(function (data) {
                console.log(data);
                SetCredentials(data);
                startConnection();
//                var connection = $.hubConnection('http://localhost:3660/signalr/');
//                var postHubProxy = connection.createHubProxy('baseHub');

////                connection.start();

//                connection.start().done(function () {
//                    postHubProxy.invoke("onConnected");
//                    console.log('invoked hub');
//                });



                //$location.path('/chat');
                $scope.username = sessionStorage['username'];
                console.log("logged");
            }, function (error) {
                console.error(error);
            }).finally(function () {
            });
    };
});

function startConnection() {

    //$.connection.hub.start().done(function() {

    //    //baseHub.server.sendMessage().done(function() {
           
    //    //});
    //    console.log('asd');

    //}).fail(function(err) {

    //    console.log(err);
    //});


    var chat = $.connection.baseHub;
    chat.client.send = function (message) {
        alert(message);
    };

    $.connection.hub.logging = true;

    $.connection.hub.url = "http://localhost:3660/signalr";

    $.connection.hub.start()
        .done(function (param) {
           // chat.server.send("Connected");
        })
        .fail(function (param) {
           // alert("Could not Connect: " + param);
        });
}