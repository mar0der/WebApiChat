webchat.controller("contactsController",
    function ($scope, contactService, $location, $rootScope, signalR) {

        $scope.contacts = [];

        signalR.on('onDisconnected', function(){
            // TODO logic 

        });

        signalR.on('userLogged', function (userLogged) {

            var user = {
                Id: userLogged.Id,
                UserName: userLogged.UserName,
                IsOnline: true
            };

            console.log(user);

            for(var i = 0; i<  $scope.contacts.length; i++){
                if($scope.contacts[i].UserName == user.UserName){
                    $scope.contacts[i].IsOnline = true;
                    console.log("contact status changed");
                    break;
                }
            }

        });

        $scope.getAllOnlineUsers = function () {
            contactService.getAllOnlineUsers()
                .then(function(data) {
                    $scope.onlineUsers = data;
                    console.log(data);
                }, function(err) {
                    console.error(err.responseText)
                });

        };

        $scope.getAllFriends = function () {
            contactService.getAllFriends()
                .then(function(data) {
                    console.log(data.data)
                    $scope.contacts = data.data;
                }, function(err) {
                    console.error(err.responseText)
                });

        }



    });