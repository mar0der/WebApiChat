'use strict';

var webchat = angular.module('webchat', ['ngRoute', 'ngResource', 'angularSpinner']);

webchat.config(function ($routeProvider) {

    $routeProvider
        .when('/', {
            templateUrl: 'partials/welcomeScreen.html'
        })
        .when('/register', {
            templateUrl: 'partials/register.html'
        })
        .when('/login', {
            templateUrl: 'partials/login.html'
        })
        .when('/privateChat/:username', {
            controller: 'chatController',
            templateUrl: 'partials/chatBox.html'
        })
        .when('/groupChat/:chatId/:groupName', {
            controller: 'groupsController',
            templateUrl: 'partials/GroupChatBox.html'
        })
        .when('/addContact', {
            controller: 'contactsController',
            templateUrl: 'partials/addContactForm.html'
        })
        .when('/createGroup', {
            controller: 'groupsController',
            templateUrl: 'partials/addGroupView.html'
        })
        .otherwise({ redirectTo: '/login' });
});

webchat.run(function ($rootScope, $location, authenticationService, usersService) {
    $rootScope.$on('$locationChangeStart', function (event) {
        if ($location.path().indexOf('login') === -1 && $location.path().indexOf('register') === -1 && !authenticationService.isLoggedIn()) {
            $location.path('/login');
        }
        var deniedPaths = ['/login', '/register'];

        if (authenticationService.isLoggedIn() && deniedPaths.indexOf($location.path()) !== -1) {
            $location.path("/");
        }

        var privateChat = $location.path().indexOf('privateChat');
        var groupChat = $location.path().indexOf('groupChat');
        if (privateChat == -1) {
            $rootScope.currentPrivateChatReceiverId = null;
            if (authenticationService.isLoggedIn) {
                usersService.updateUserCurrentChatId(null);
            }
        }
        if (groupChat == -1) {
            $rootScope.currentGroupId = null;
        }
    });

});
