/**
 * Created by ibaky on 9/6/2015.
 */
'use strict';

webchat.controller("groupsController", function ($scope, chatService, $location, signalR, $rootScope, authenticationService,
                                                 usersService, groupService, $routeParams, contactService) {
    $scope.groups = [];
    $scope.groupMessages = [];
    $scope.groupUsersPreview = [];
    $scope.groupContactPreview = [];
    $scope.groupChatUsers = [];
    $scope.groupName = $routeParams.groupName;

    //todo clear currentGroup on click on 

    function updateChatWindow() {
        setTimeout(function () {
            $('#chatContent').stop().animate({
                scrollTop: $("#chatContent")[0].scrollHeight
            }, 400);
        }, 100);
    }
    signalR.on('toggleGroupCreation', function (group) {
        if (sessionStorage['currentSelection'] === "groups") {
            $scope.groups.push(group);
        }
        else {
            $scope.groupMissedMessages = true;
        }
    });


    $scope.me = authenticationService.getUsername();
    if ($scope.me.length > 0) {
        setTimeout(function () {
            usersService.userStatusUpdate();
            usersService.updateUserCurrentChatId(null);
            if ($routeParams.chatId !== undefined) {
                $scope.getGroupMessages($routeParams.chatId);
            }
        }, 100);
    }

    $scope.getAllFriends = function getAllFriends() {
        contactService.getAllFriends()
        .then(function (serverData) {
            $scope.groupContactPreview = serverData.data;
        });
    };

    $scope.getGroupMessages = function (id) {
        $scope.groupMessages = [];
        groupService.getMessageByGroup(id)
            .then(function (data) {
                for (var i = 0; i < $scope.groups.length; i++) {
                    if ($scope.groups[i].GroupId == id) {
                        $scope.groups[i].UnreceivedMessages = 0;
                        break;
                    }
                }

                usersService.updateUserCurrentChatId(id);
                $scope.groupMessages = data.data;
                $rootScope.currentGroupId = id;
                updateChatWindow();
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToGroup = function (groupMessage) {
        groupMessage['GroupId'] = $rootScope.currentGroupId;
        groupService.SendMessageToGroup(groupMessage)
            .then(function (data) {
                updateChatWindow();
                $scope.groupMessages.push(data.data);
                groupMessage.Text = '';
            }, function (err) {
                console.log(err);
            });
    };

    $scope.addToGroupPreview = function (user) {
        $scope.groupUsersPreview.push(user);
        for (var i = 0 ; i < $scope.groupContactPreview.length; i++) {
            if ($scope.groupContactPreview[i].Id === user.Id) {
                $scope.groupContactPreview.splice(i, 1);
                break;
            }
        }
    };

    $scope.removeFromGroupPreview = function (user) {
        for (var i = 0 ; i < $scope.groupUsersPreview.length; i++) {
            if ($scope.groupUsersPreview[i].Id === user.Id) {
                $scope.groupUsersPreview.splice(i, 1);
                $scope.groupContactPreview.push(user);
                break;
            }
        }
    };

    $scope.addUserIdToGroup = function (id) {
        $scope.groupChatUsers.push(id);

    };

    $scope.removeUserIdFromGroup = function (id) {
        var index = $scope.groupChatUsers.indexOf(id);
        if (index > -1) {
            $scope.groupChatUsers.splice(index, 1);
        }
    };

    $scope.createGroup = function (groupName) {
        var groupData = {
            GroupName: groupName || '',
            UserIds: $scope.groupChatUsers
        };

        groupService.createGroup(groupData)
            .then(function (serverData) {
                $rootScope.$broadcast('newGroupAdded', serverData.data);
                $location.path('/groupChat/' + serverData.data.GroupId + '/' + serverData.data.GroupName);
            }, function (error) {
                console.log(error);
            });
    };

    $scope.checkForGroupCreation = function () {
        if ($scope.groupChatUsers.length > 0) {
            return true;
        }
        return false;
    };

    $scope.getMissedGroupChats = function () {
        groupService.getMissedGroupChats()
            .then(function (data) {
                for (var i = 0; i < data.data.length; i++) {
                    for (var j = 0; j < $scope.groups.length; j++) {
                        if (data.data[i].Id == $scope.groups[j].GroupId) {
                            $scope.groups[j].UnreceivedMessages = data.data[i].UnreceivedMessages;
                        }
                    }
                }
            }, function (err) {
                console.log(err);
            });
    }

    $scope.getAllGroups = function getAllGroups() {
        groupService.getAllGroups()
        .then(function (serverData) {
            $scope.groups = serverData.data;
            $scope.getMissedGroupChats();
        });
    };

    $rootScope.$on('newGroupAdded', function (event, data) {
        $scope.groups.push(data);
    });

    $scope.$on('updateGroupMessages', function (event, groupMessages) {
        for (var i = 0; i < groupMessages.length; i++) {
            for (var j = 0; j < $scope.groupMessages.length; j++) {
                if (groupMessages[i].Id === $scope.groupMessages[j].Id) {
                    $scope.groupMessages[j].SeenBy = groupMessages[i].SeenBy;
                }
            }
        }

    });

    $scope.$on('newGroupMessageAdded', function (event, message) {
        if ($rootScope.currentGroupId == message.GroupId) {
            $scope.groupMessages.push(message);
            updateChatWindow();
        } else {
            for (var i = 0; i < $scope.groups.length; i++) {
                if ($scope.groups[i].GroupId == message.GroupId) {
                    $scope.groups[i].UnreceivedMessages++;
                    console.log('+');
                }
            }
        }
    });
});
