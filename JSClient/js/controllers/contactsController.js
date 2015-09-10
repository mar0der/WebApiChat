'use strict';

webchat.controller("contactsController", function ($scope, contactService, $location, $rootScope, signalR, chatService) {

    $scope.contacts = [];
    $rootScope.searchedUsers = [];

    signalR.on('contactListUpdate', function () {
        $scope.getAllFriends();
    });


    $scope.getAllFriends = function () {
        contactService.getAllFriends()
            .then(function (data) {
                $scope.contacts = data.data;
                $scope.getUnreceived();
            }, function (err) {
                console.error(err.responseText);
            });
    };

    //what is this doing?
    //$scope.getAllOnlineUsers = function () {
    //    contactService.getAllOnlineUsers()
    //        .then(function (data) {
    //            $scope.onlineUsers = data;
    //            console.log(data);
    //        }, function (err) {
    //            console.error(err.responseText);
    //        });
    //};

    $scope.searchUser = function searchUser(searchPattern) {
        contactService.searchContact(searchPattern)
            .then(function (serverResponse) {
                $scope.searchContactResults = serverResponse.data;
            }, function (serverError) {
                console.log(serverError);
            });
    };

    //$scope.searchInContacts = function searchUser(searchPattern) {
    //    contactService.searchInContacts(searchPattern)
    //        .then(function (serverResponse) {
    //            $scope.searchInContactsResults = serverResponse.data;
    //        }, function (serverError) {
    //            console.log(serverError);
    //        });
    //};

    function attachNotificationsToSpecificContacts(notification) {
        for (var i = 0; i < $scope.contacts.length; i++) {
            for (var k = 0; k < notification.data.length; k++) {
                if ($scope.contacts[i].UserName === notification.data[k].sender) {
                    $scope.contacts[i].UnreceivedMessages = notification.data[k].count;
                    break;
                }
            }
        }
    }


    $scope.getUnreceived = function () {
        chatService.getUnreceived()
            .then(function (serverdata) {
                attachNotificationsToSpecificContacts(serverdata);
            }, function (error) {
                console.log(error);
            });
    };

    signalR.on('newPrivateMessage', function (message) {
        if (message.SenderId !== $rootScope.currentPrivateChatReceiverId) {
            for (var i = 0; i < $scope.contacts.length; i++) {
                if ($scope.contacts[i].UserName === message.Sender) {
                        $scope.contacts[i].UnreceivedMessages++;
                    break;
                }
            }
        }
    });

    $scope.clearNotificaions = function clearNotifications(username) {
        for (var i = 0; i < $scope.contacts.length; i++) {
            if ($scope.contacts[i].UserName === username) {
                $scope.contacts[i].UnreceivedMessages = 0;
                break;
            }
        }
    };

    $scope.addContact = function addContact(userId) {
        contactService.addContact(userId)
            .then(function (serverResponse) {
                $rootScope.$broadcast('newContact', serverResponse.data);
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
            });
    };

    $rootScope.$on('newContact', function (event, data) {
        $scope.contacts.push(data);
    });

    $rootScope.$on('userDisconnected', function (event, username) {
        for (var i = 0; i < $scope.contacts.length; i++) {
            if ($scope.contacts[i].UserName === username) {
                $scope.contacts[i].IsOnline = false;
                break;
            }
        }
    });

    $rootScope.$on('userLogged', function (event, userLogged) {
        var user = {
            Id: userLogged.Id,
            UserName: userLogged.UserName,
            IsOnline: true,
            UnreceivedMessages: 0
        };

        for (var i = 0; i < $scope.contacts.length; i++) {
            if ($scope.contacts[i].UserName === user.UserName) {
                $scope.contacts[i].IsOnline = true;
                break;
            }
        }
    });
});