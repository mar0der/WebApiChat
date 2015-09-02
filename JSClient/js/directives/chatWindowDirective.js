'use strict';
///<reference path="chatWindowDirective.js" />
webchat.directive('chatwindow', function(){
    return{
        restrict : 'E',
        templateUrl : 'partials/chat-window-template.html',
        controller: 'chatController'
    }
});