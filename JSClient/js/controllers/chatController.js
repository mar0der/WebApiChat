'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope) {

    $rootScope.chatLog = [];
    $rootScope.currentContactId = "";
    $rootScope.currentChanelId = "";
    $scope.me = localStorage['username'];

    function updateChatWindow() {
        setTimeout(function () {
            $('#chatContent').stop().animate({
                scrollTop: $("#chatContent")[0].scrollHeight
            }, 400);
        }, 100);
    }

    signalR.on('toggleMessage', function (message) {
        if (message.SenderId === $rootScope.currentPrivateChatReceiver) {
            $rootScope.chatLog.push(message);
            updateChatWindow();
        }

        for (var i = 0; i < $rootScope.contacts.length; i++) {
            if ($rootScope.contacts[i].UserName === message.Sender) {
                $rootScope.contacts[i].UnreceivedMessages++;
                break;
            }
        }
    });

    $scope.getUnreceived = function () {
        chatService.getUnreceived()
            .then(function (data) {
                console.log(data);
            }, function (error) {
                console.log(error);
            });
    };

    $scope.setCurrentReceiver = function setCurrentReceiver(receiverId) {
        $rootScope.currentPrivateChatReceiver = receiverId;
    }

    $scope.getChatWithUser = function (userId) {
        chatService.GetChatWithUser(userId)
            .then(function (serverResponse) {

                for (var i = 0; i < $rootScope.contacts.length; i++) {
                    if ($rootScope.contacts[i].Id === userId) {
                        $rootScope.contacts[i].UnreceivedMessages = 0;
                        break;
                    }
                }
                $rootScope.chatLog = serverResponse.data;
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToUser = function (messageData) {
        messageData.receiverId = $rootScope.currentPrivateChatReceiver;
        chatService.sendMessage(messageData)
            .then(function (data) {
                $rootScope.chatLog.push(data.data);
                updateChatWindow();
                messageData.Text = '';
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
