/**
 * Created by ibaky on 9/6/2015.
 */
webchat.controller("groupsController", function ($scope, chatService, $location, signalR, $rootScope, authenticationService,
                                                 usersService, groupService) {
    $rootScope.rightContainerTemplate = 'partials/GroupChatBox.html';
    $rootScope.groups = [];
    $rootScope.groupMessages = [];
    $rootScope.currentGroupId = "";

    //todo clear currentGroup on click on chat

    signalR.on('toggleGroupMessage', function (message) {
        if(message.GroupId == $rootScope.currentGroupId){
            $rootScope.groupMessages.push(message)
        }


        console.log(message);
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
                console.log(data)
            }, function (err) {
                console.log(err)
            })
    };

    $scope.getGroupMessages = function (id) {
        $rootScope.rightContainerTemplate = 'partials/GroupChatBox.html';
        console.log("container")
        $scope.groupMessages = [];
        groupService.getMessageByGroup(id)
            .then(function (data) {
                $rootScope.groupMessages = data.data;
                $rootScope.currentGroupId = id;
                console.log($rootScope.groupMessages)
            }, function (err) {
                console.log(err)
            })
    }

    $rootScope.sendMessageToGroup = function (groupMessage) {
        groupMessage['GroupId'] = $rootScope.currentGroupId;
        console.log(groupMessage)
        groupService.SendMessageToGroup(groupMessage)
            .then(function (data) {
                console.log(data.data)
            }, function (err) {
                console.log(err)
            })
    }

});
