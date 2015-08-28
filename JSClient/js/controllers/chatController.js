webchat.controller("chatController", function ($scope,
                                               chatService, $location, signalR, $rootScope) {

    $scope.chatLog = [];
    $scope.currentContactId = "";
    $scope.currentChanelId = "";



    signalR.on('toggleMessage', function (message) {
        $scope.chatLog.push(message);

    });

    $scope.getChatWithUser = function (userId) {

        chatService.GetChatWithUser(userId)
            .then(function (data) {
                console.log(data[0].Name)
                console.log(data[0].Messages);
                $scope.currentChanelId = data[0].Name;
                $scope.currentContactId = userId;
                $scope.chatLog = data[0].Messages;
                console.log( $scope.currentChanelId )
            }, function (err) {
                console.log(err);
            });
    };

    $scope.sendMessageToUser = function (messageData) {

        chatService.sendMessage(messageData, $scope.currentChanelId)
            .then(function (data) {
                console.log(data);
                $scope.chatLog.push(data);

            }, function (err) {
                console.log(err);
            });
    };



});
