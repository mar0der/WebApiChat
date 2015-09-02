

//<reference path="authenticationController.js" />
webchat.controller("authenticationController", function ($scope,
                                                         userServices, $location, signalR, $rootScope) {



    $scope.login = function () {

        userServices.login($scope.loginData)
            .then(function (data) {
                console.log(data);
                SetCredentials(data);
                $location.path('/chat');
            }, function(err){
                console.log(err);
            });
    };
});