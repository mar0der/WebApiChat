'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope) {

    $scope.chatLog = [];
    $rootScope.currentContactId = "";
    $rootScope.currentChanelId = "";


    signalR.on('toggleMessage', function (message) {
        $scope.chatLog.push(message);

    });

    $scope.getChatWithUser = function (userId) {

        chatService.GetChatWithUser(userId)
            .then(function (data) {
                //console.log(data.data.Id)
                $rootScope.currentChanelId = data.data.Id;
                console.log($rootScope.currentChanelId)
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
