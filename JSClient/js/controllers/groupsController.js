/**
 * Created by ibaky on 9/6/2015.
 */
'use strict';

webchat.controller("groupsController", function ($scope, chatService, $location, signalR, $rootScope, authenticationService,
                                                 usersService, groupService) {
    $rootScope.rightContainerTemplate = 'partials/GroupChatBox.html';
    $rootScope.groups = [];
    $scope.groupMessages = [];
    $rootScope.currentGroupId = "";
    $rootScope.groupUsersPreview = [];
    $rootScope.groupContactPreview = $rootScope.contacts;
    $rootScope.groupName = "";
    $rootScope.groupMissedMessages = 0;
    $rootScope.groupChatUsers = [];

    //todo clear currentGroup on click on 

    function updateChatWindow() {
        setTimeout(function () {
            $('#chatContent').stop().animate({
                scrollTop: $("#chatContent")[0].scrollHeight
            }, 400);
        }, 100);
    }

    signalR.on('pushGroupMessage', function (message) {
        if (message.GroupId === $rootScope.currentGroupId) {
            $scope.groupMessages.push(message);
        } else {
            
            for (var i = 0; i < $scope.groups.length; i++) {
                if ($scope.groups[i].GroupId == message.GroupId) {
                    $scope.groups[i].UnreceivedMessages++;
                    break;
                }
            }
        }

        updateChatWindow();
    });

    signalR.on('toggleGroupCreation', function (group) {
        if (sessionStorage['currentSelection'] === "groups") {
            $rootScope.groups.push(group);
        }
        else {
            $rootScope.groupMissedMessages = true;
        }
    });

    signalR.on('seenMessages', function (groupMessages) {
        $scope.groupMessages = groupMessages;
    });

    $scope.me = authenticationService.getUsername();
    if ($scope.me.length > 0) {
        setTimeout(function () {
            usersService.userStatusUpdate();
            usersService.updateUserCurrentChatId(null);
        }, 100);
    }

    $rootScope.getAllGroups = function () {
        groupService.getAllGroups()
            .then(function (data) {
                $rootScope.groups = data.data;
                $rootScope.getUnreceived();
            }, function (err) {
                console.log(err);
            });
    };

    $rootScope.getGroupMessages = function (id) {
        $rootScope.rightContainerTemplate = 'partials/GroupChatBox.html';
        console.log("container");
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

    $rootScope.sendMessageToGroup = function (groupMessage) {
        groupMessage['GroupId'] = $rootScope.currentGroupId;
        groupService.SendMessageToGroup(groupMessage)
            .then(function (data) {
                updateChatWindow();
                if ($rootScope.currentGroupId === data.data.GroupId) {
                    $scope.groupMessages.push(data.data);
                    groupMessage.Text = '';
                }
            }, function (err) {
                console.log(err);
            });
    };

    $rootScope.addToGroupPreview = function (user) {
        $rootScope.groupUsersPreview.push(user);
        for (var i = 0 ; i < $rootScope.groupContactPreview.length; i++) {
            if ($rootScope.groupContactPreview[i].Id === user.Id) {
                $rootScope.groupContactPreview.splice(i, 1);
                break;
            }
        }
        console.log($rootScope.groupUsersPreview);
    };

    $rootScope.removeFromGroupPreview = function (user) {
        for (var i = 0 ; i < $rootScope.groupUsersPreview.length; i++) {
            if ($rootScope.groupUsersPreview[i].Id === user.Id) {
                $rootScope.groupUsersPreview.splice(i, 1);
                $rootScope.groupContactPreview.push(user);
                break;
            }
        }
    };

    $rootScope.addUserIdToGroup = function (id) {
        $rootScope.groupChatUsers.push(id);
        console.log($rootScope.groupChatUsers);
    };

    $rootScope.removeUserIdFromGroup = function (id) {
        var index = $rootScope.groupChatUsers.indexOf(id);
        if (index > -1) {
            $rootScope.groupChatUsers.splice(index, 1);
        }
        console.log($rootScope.groupChatUsers);
    };

    $rootScope.loadAddGroupFrom = function () {
        $rootScope.rightContainerTemplate = 'partials/addGroupView.html';
    };



    $rootScope.createGroup = function (groupName) {
        var groupData = {
            GroupName: groupName,
            UserIds: $rootScope.groupChatUsers
        };

        groupService.createGroup(groupData)
            .then(function (data) {
                console.log(data);
            }, function (error) {
                console.log(error);
            });
    };

    $rootScope.checkForGroupCreation = function () {
        if ($rootScope.groupChatUsers.length > 0) {
            return true;
        }
        return false;
    };

    $scope.getMissedGroupChats = function () {
        groupService.getMissedGroupChats()
            .then(function (data) {
            }, function (err) {
                console.log(err);
            });
    }

    function attachMissedGroupMessages(notification) {
        for (var i = 0; i < $scope.groups.length; i++) {
            for (var k = 0; k < notification.length; k++) {
                if ($scope.groups[i].GroupName == notification[k].Name) {
                    $scope.groups[i].UnreceivedMessages = notification[k].Count;
                    break;
                }
            }
        }
    }

    $rootScope.getUnreceived = function () {
        groupService.getUnreceived()
            .then(function (serverdata) {
                attachMissedGroupMessages(serverdata.data);
            }, function (error) {
                console.log(error);
            });
    };

});
