/**
 * Created by ibaky on 9/6/2015.
 */
webchat.controller("groupsController", function ($scope, chatService, $location, signalR, $rootScope, authenticationService,
                                                 usersService, groupService) {
    $rootScope.rightContainerTemplate = 'partials/GroupChatBox.html';
    $rootScope.groups = [];
    $rootScope.groupMessages = [];
    $rootScope.currentGroupId = "";
    $rootScope.groupUsersPreview = [];
    $rootScope.groupContactPreview = $rootScope.contacts;
    $rootScope.groupName = "";
    $rootScope.groupMissedMessages = 0;
    $rootScope.groupChatUsers = [];

    //todo clear currentGroup on click on chat

    signalR.on('toggleGroupMessage', function (message) {
        if (message.GroupId == $rootScope.currentGroupId) {
            $rootScope.groupMessages.push(message)
        }


        console.log(message);
    });

    signalR.on('toggleGroupCreation', function (group) {
        if(sessionStorage['currentSelection'] == "groups"){
            $rootScope.groups.push(group);
        }
        else{
            $rootScope.groupMissedMessages = true;
        }
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
        console.log("container");
        $scope.groupMessages = [];
        groupService.getMessageByGroup(id)
            .then(function (data) {
                $rootScope.groupMessages = data.data;
                $rootScope.currentGroupId = id;
                console.log($rootScope.groupMessages)
            }, function (err) {
                console.log(err)
            })
    };

    $rootScope.sendMessageToGroup = function (groupMessage) {
        groupMessage['GroupId'] = $rootScope.currentGroupId;
        console.log(groupMessage);
        groupService.SendMessageToGroup(groupMessage)
            .then(function (data) {
                console.log(data.data);
                if ($rootScope.currentGroupId == data.data.GroupId) {
                    $rootScope.groupMessages.push(data.data);
                }
            }, function (err) {
                console.log(err)
            })
    };


    $rootScope.addToGroupPreview = function (user) {
        $rootScope.groupUsersPreview.push(user);
        for(var i =0 ; i < $rootScope.groupContactPreview.length; i++){
            if($rootScope.groupContactPreview[i].Id == user.Id){
                $rootScope.groupContactPreview.splice(i, 1);
                break;
            }
        }

        console.log($rootScope.groupUsersPreview)
    };

    $rootScope.addUserIdToGroup = function (id) {
        $rootScope.groupChatUsers.push(id);

        console.log($rootScope.groupChatUsers)
    };

    $rootScope.loadAddGroupFrom = function () {
        $rootScope.rightContainerTemplate = 'partials/addGroupView.html';
    };

    $rootScope.removeFromGroupPreview = function(user){
        for(var i =0 ; i < $rootScope.groupUsersPreview.length; i++){
            if($rootScope.groupUsersPreview[i].Id == user.Id){
                $rootScope.groupUsersPreview.splice(i, 1);
                $rootScope.groupContactPreview.push(user)
                break;
            }
        }
    };

    $rootScope.createGroup = function(){
        var groupData = {
            GroupName : $rootScope.groupName,
            UserIds : $rootScope.groupChatUsers
        };

        groupService.createGroup(groupData)
            .then(function(data){
                console.log(data)
            }, function(error){
                console.log(error)
            })
    };


    $rootScope.removeUserIdFromGroup = function(id){
        var index =  $rootScope.groupChatUsers.indexOf(id);
        if (index > -1) {
            $rootScope.groupChatUsers.splice(index, 1);
        }

        //console.log($rootScope.groupChatUsers)
    };

    $rootScope.checkForGroupCreation = function (groupName) {

        if($rootScope.groupChatUsers.length >0 && groupName.length > 0){
            $rootScope.groupName = groupName;
            return true;
        }
    };

    $scope.getMissedGroupChats = function(){
        groupService.getMissedGroupChats()
            .then(function(data){
                console.log("missed")
                console.log(data)
            }, function(err){
                console.log(err)
            })
    }


});
