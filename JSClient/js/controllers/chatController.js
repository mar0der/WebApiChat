'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope) {

    $scope.chatLog = [];
    $scope.currentContactId = "";
    $scope.currentChanelId = "";


    signalR.on('toggleMessage', function (message) {
        $scope.chatLog.push(message);

    });

    $scope.getChatWithUser = function (userId) {

        chatService.GetChatWithUser(userId)
            .then(function (data) {
                console.log(data.data.Id)
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToUser = function (messageData) {
        chatService.sendMessage(messageData, $scope.currentChanelId)
            .then(function (data) {
                
                console.log(data);
                $scope.chatLog.push(data);
                messageData.text = '';
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
