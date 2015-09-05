webchat.controller("contactsController",
    function ($scope, contactService, $location, $rootScope, signalR) {

        $rootScope.contacts = [];
        $rootScope.searchedUsers = []


        signalR.on('userDisconnected', function (username) {

            for (var i = 0; i < $rootScope.contacts.length; i++) {
                if ($rootScope.contacts[i].UserName == username) {
                    $rootScope.contacts[i].IsOnline = false;
                    console.log("contact status changed");
                    break;
                }
            }

        });

        signalR.on('userLogged', function (userLogged) {

            var user = {
                Id: userLogged.Id,
                UserName: userLogged.UserName,
                IsOnline: true,
                UnreceivedMessages: 0
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
        };

        $scope.searchUser = function searchUser(searchPattern) {
            contactService.searchContact(searchPattern)
                .then(function (serverResponse) {
                    $scope.searchContactResults = serverResponse.data;
                }, function (serverError) {
                    console.log(serverError)
                });
        };

        $scope.addContact = function addContact(userId) {
            contactService.addContact(userId)
            .then(function (serverResponse) {
                $scope.contacts.push(serverResponse.data);
                var temp = [];
                var i;
                for (i = 0; i < $scope.searchContactResults.length; i++) {
                    if ($scope.searchContactResults[i].Id !== serverResponse.data.Id) {
                        temp.push($scope.searchContactResults[i]);
                    }
                }

                $scope.searchContactResults = temp;
            }, function (serverError) {
                console.log(serverError);
            })
        };

    });