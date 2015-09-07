'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope, authenticationService,
                                               usersService, contactService) {
    $rootScope.rightContainerTemplate = 'partials/welcomeScreen.html';
    $scope.chatLog = [];
    $rootScope.currentChanelId = "";
    $scope.me = authenticationService.getUsername();


    if ($scope.me.length > 0) {
        setTimeout(function () {
            usersService.userStatusUpdate();
            usersService.updateUserCurrentChatId(null);
        }, 100);
    }

    $scope.getAllFriends = function () {
        contactService.getAllFriends()
            .then(function (data) {
                $rootScope.contacts = data.data;
                console.log(data)
            }, function (err) {
                console.error(err.responseText);
            });
    };

    function updateChatWindow() {
        setTimeout(function () {
            $('#chatContent').stop().animate({
                scrollTop: $("#chatContent")[0].scrollHeight
            }, 400);
        }, 100);
    }

    function attachNotificationsToSpecificContacts(notification) {
        for (var i = 0; i < $scope.contacts.length; i++) {
            for (var k = 0; k < notification.data.length; k++) {
                if ($rootScope.contacts[i].UserName === notification.data[k].sender) {
                    $rootScope.contacts[i].UnreceivedMessages = notification.data[k].count;
                    break;
                }
            }
        }
    }

    function updateMessageStatus(messageId) {
        chatService.updateMessageStatus(messageId)
            .then(function (data) {
                console.log(data);
            }, function (error) {
                console.log(error);
            });
    }

    signalR.on('pushMessageToClient', function (message) {
        console.log('bah maa mu');
        if (message.SenderId === $scope.currentPrivateChatReceiver) {
            console.log(message);
            $scope.chatLog.push(message);
            updateChatWindow();
        }
        else {
            updateMessageStatus(message);
        }

        for (var i = 0; i < $scope.contacts.length; i++) {
            if ($scope.contacts[i].UserName === message.Sender) {
                if (message.SenderId !== $scope.currentPrivateChatReceiver) {
                    $scope.contacts[i].UnreceivedMessages++;
                }
                break;
            }
        }
    });

    signalR.on('seenMessages', function (messages) {
        $scope.chatLog = messages;
    });

    $scope.getUnreceived = function () {
        chatService.getUnreceived()
            .then(function (serverdata) {
                attachNotificationsToSpecificContacts(serverdata);
            }, function (error) {
                console.log(error);
            });
    };

    $scope.setCurrentReceiver = function setCurrentReceiver(receiverId) {
        $scope.currentPrivateChatReceiver = receiverId;
    };

    $scope.getChatWithUser = function (contact) {
        $rootScope.rightContainerTemplate = 'partials/chatBox.html';

        chatService.GetChatWithUser(contact.Id)
            .then(function (serverResponse) {
                usersService.updateUserCurrentChatId(contact.Id);
                $scope.currentPrivateChatReceiver = contact.FirstName + ' ' + contact.LastName;
                for (var i = 0; i < $scope.contacts.length; i++) {
                    if ($scope.contacts[i].Id === contact.Id) {
                        $scope.contacts[i].UnreceivedMessages = 0;
                        break;
                    }
                }

                $scope.chatLog = serverResponse.data;
                updateChatWindow();
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToUser = function (messageData) {
        messageData.receiverId = $scope.currentPrivateChatReceiver;
        chatService.sendMessage(messageData)
            .then(function (data) {
                $scope.chatLog.push(data.data);
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
            sessionStorage['currentSelection'] = "contacts";
        } else {
            $scope.currentSidebar = {
                isContacts: false,
                isGroups: true
            };
            sessionStorage['currentSelection'] = "groups";
        }
    };

    $scope.laodAddContactForm = function () {
        $rootScope.rightContainerTemplate = 'partials/addContactForm.html';
    }

});
