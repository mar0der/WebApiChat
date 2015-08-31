webchat.controller("contactsController",
    function ($scope, contactService, $location, $rootScope, signalR) {

        signalR.on('onDisconnected', function(){
            // TODO logic 

        });

        signalR.on('userLogged', function (userLogged) {

            var user = {
                Id: userLogged.id,
                Name: userLogged.name,
                IsOnline: true
            };

            var found = false;
            for (var i = 0; i < $scope.onlineUsers.length; i++) {
                if ($scope.onlineUsers[i].Name == user.Name) {
                    found = true;
                    break;
                }
            }

            //this is useless - if statement
            //if (found == true) {
                for (var k = 0; k < $scope.contacts.length; k++) {
                    if ($scope.contacts[k].Name == user.Name) {
                        $scope.contacts[k].IsOnline = true;
                        break;
                    }
                }
            //}

            if (found == false) {
                $scope.onlineUsers.push(user);
            }


        });

        $scope.getAllOnlineUsers = function () {
            contactService.getAllOnlineUsers()
                .then(function (data) {
                    $scope.onlineUsers = data;
                    console.log(data);
                }, function (err) {
                    console.error(err.responseText)
                })

        };

        $scope.getAllFriends = function () {
            contactService.getAllFriends()
                .then(function (data) {
                    $scope.contacts = data;
                }, function (err) {
                    console.error(err.responseText)
                })

        }



    });