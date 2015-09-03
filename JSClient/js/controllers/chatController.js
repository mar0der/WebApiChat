'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope) {

    $rootScope.chatLog = [];
    $rootScope.currentContactId = "";
    $rootScope.currentChanelId = "";
    $scope.me = localStorage['username'];


    signalR.on('toggleMessage', function (message) {
         console.log(message)

        //TODO FIX

    });

    $scope.getChatWithUser = function (userId) {

        chatService.GetChatWithUser(userId)
            .then(function (data) {
                //console.log(data.data.Id)
                $rootScope.currentChanelId = data.data.Id;
                //console.log($rootScope.currentChanelId)
                //console.log(data.data.Messages)
                $rootScope.chatLog = data.data.Messages;
                console.log($scope.chatLog)
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToUser = function (messageData) {
        console.log($rootScope.currentChanelId);
        console.log(messageData)
        chatService.sendMessage(messageData, $rootScope.currentChanelId)
            .then(function (data) {
                console.log(data);
                //console.log(data);
                //$scope.chatLog.push(data);
                messageData = '';
            }, function (err) {
                console.log(err);
            });
    };


    $scope.toggleSidebar = function (clickedButton) {
        if (clickedButton === 'contacts') {
            $scope.currentSidebar = {
                isContacts: true,
                isGroups: false
            };
        } else {
            $scope.currentSidebar = {
                isContacts: false,
                isGroups: true
            };
        }
    }

});
