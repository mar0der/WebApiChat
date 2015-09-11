/// <reference path="groupsController.js" />
'use strict';

webchat.controller("chatController", function ($scope, chatService, $location, signalR, $rootScope, authenticationService,
                                               usersService, contactService, $routeParams) {
    $scope.chatLog = [];
    $rootScope.currentChanelId = "";
    $scope.me = authenticationService.getUsername();


    if ($scope.me.length > 0) {
        setTimeout(function () {
            usersService.userStatusUpdate();
            usersService.getUserPreview($routeParams.username)
            .then(function (serverData) {
                usersService.updateUserCurrentChatId(serverData.data.Id);
                $rootScope.currentPrivateChatReceiverId = serverData.data.Id;   
                $scope.getChatWithUser(serverData.data);
            }, function (serverError) {
                console.log(serverError);
            });

        }, 200);
    }

    function updateChatWindow() {
        setTimeout(function () {
            $('#chatContent').stop().animate({
                scrollTop: $("#chatContent")[0].scrollHeight
            }, 400);
        }, 100);
    }

    function updateMessageStatus(messageId) {
        chatService.updateMessageStatus(messageId)
            .then(function (data) {
                console.log(data);
            }, function (error) {
                console.log(error);
            });
    }

    signalR.on('seenMessages', function (messages) {
        $scope.chatLog = messages;
    });

    $scope.getChatWithUser = function (contact) {
        chatService.GetChatWithUser(contact.Id)
            .then(function (serverResponse) {
                usersService.updateUserCurrentChatId(contact.Id);
                $scope.currentPrivateChatReceiver = contact.FirstName + ' ' + contact.LastName;
                $scope.chatLog = serverResponse.data;
                updateChatWindow();
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToUser = function (messageData) {
        messageData.receiverId = $rootScope.currentPrivateChatReceiverId;
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

    $scope.$on('newPrivateMessage', function (event, message) {
        $scope.chatLog.push(message);
        updateChatWindow();
    });

});
