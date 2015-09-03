webchat.controller("contactsController",
    function ($scope, contactService, $location, $rootScope, signalR) {

        $rootScope.contacts = [];

        signalR.on('onDisconnected', function () {
            // TODO logic 

        });

        signalR.on('userLogged', function (userLogged) {

            var user = {
                Id: userLogged.Id,
                UserName: userLogged.UserName,
                IsOnline: true,
                UnreceivedMessages :0
            };

            console.log(user);

            for (var i = 0; i < $rootScope.contacts.length; i++) {
                if ($rootScope.contacts[i].UserName == user.UserName) {
                    $rootScope.contacts[i].IsOnline = true;
                    console.log("contact status changed");
                    break;
                }
            }
        });

        $scope.getAllOnlineUsers = function () {
            contactService.getAllOnlineUsers()
                .then(function (data) {
                    $scope.onlineUsers = data;
                    console.log(data);
                }, function (err) {
                    console.error(err.responseText);
                });
        };

        $scope.getAllFriends = function () {
            contactService.getAllFriends()
                .then(function (data) {
                    console.log(data.data);
                    $rootScope.contacts = data.data;
                }, function (err) {
                    console.error(err.responseText);
                });
        }
    });