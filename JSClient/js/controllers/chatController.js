'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope) {
    $scope.rightContainerTemplate = 'partials/welcomeScreen.html';
    $rootScope.chatLog = [];
    $rootScope.currentContactId = "";
    $rootScope.currentChanelId = "";
    $scope.me = sessionStorage['username'];

    function updateChatWindow() {
        setTimeout(function () {
            $('#chatContent').stop().animate({
                scrollTop: $("#chatContent")[0].scrollHeight
            }, 400);
        }, 100);
    }

    signalR.on('toggleMessage', function (message) {
        if (message.SenderId === $rootScope.currentPrivateChatReceiver) {
            $rootScope.chatLog.push(message)
            console.log(message),
            updateChatWindow();
        }

        else{
            updateMessageStatus(message);
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
            .then(function (serverdata) {
                console.log(serverdata);
                attachNotificationsToSpecificContacts(serverdata);
            }, function (error) {
                console.log(error);
            });
    };

    $scope.setCurrentReceiver = function setCurrentReceiver(receiverId) {
        $rootScope.currentPrivateChatReceiver = receiverId;
    };

    $scope.getChatWithUser = function (userId) {
        $scope.rightContainerTemplate = 'partials/chatBox.html';
        chatService.GetChatWithUser(userId)
            .then(function (serverResponse) {
                for (var i = 0; i < $rootScope.contacts.length; i++) {
                    if ($rootScope.contacts[i].Id === userId) {
                        $rootScope.contacts[i].UnreceivedMessages = 0;
                        break;
                    }
                }
                $rootScope.chatLog = serverResponse.data;
                updateChatWindow();
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
    };

    $scope.laodAddContactForm = function () {
        $scope.rightContainerTemplate = 'partials/addContactForm.html';
    }


    function attachNotificationsToSpecificContacts(notification){
        for(var i = 0; i < $rootScope.contacts.length; i++){
            for(var k = 0 ; k < notification.data.length; k++){
                if($rootScope.contacts[i].UserName == notification.data[k].sender){
                    $rootScope.contacts[i].UnreceivedMessages = notification.data[k].count;
                     break;
                    //maybe break
                }
            }

        }
    }


    function updateMessageStatus(messageId){
        chatService.updateMessageStatus(messageId)
            .then(function (data) {
                console.log(data);
            }, function (error) {
                console.log(error);
            })
    }



});
