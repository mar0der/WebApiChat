'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope) {

    $rootScope.chatLog = [];
    $rootScope.currentContactId = "";
    $rootScope.currentChanelId = "";
    $scope.me = localStorage['username'];


    signalR.on('toggleMessage', function (message) {
         console.log(message)
        if(message.CurrentChatId == $rootScope.currentChanelId){
            $rootScope.chatLog.push(message);
        }

        for(var i = 0; i < $rootScope.contacts.length; i ++){
            if($rootScope.contacts[i].UserName == message.Sender){
                $rootScope.contacts[i].UnreceivedMessages++;
                break;
            }
        }

    });

    $scope.getChatWithUser = function (userId) {

        chatService.GetChatWithUser(userId)
            .then(function (data) {
                //console.log(data.data.Id)
                $rootScope.currentChanelId = data.data.Id;

                for(var i = 0; i < $rootScope.contacts.length; i ++){
                    if($rootScope.contacts[i].Id ==userId){
                        $rootScope.contacts[i].UnreceivedMessages=0;
                        break;
                    }
                }


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
              $rootScope.chatLog.push(data.data)
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
