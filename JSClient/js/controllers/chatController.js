'use strict';
///<reference path="chatController.js" />
webchat.controller('chatController', function ($scope, chatService) {

    //$scope.addCommentToPost = function addCommentToPost(post, commentContent) {

    //}

    $scope.getAllChats = function () {
        chatService.getChatMessages()
        .then(function (data) {
            console.log(data);
        }, function (error) {
            console.error(error);
        }).finally(function () {

        });
    }

});
