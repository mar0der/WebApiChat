/// <reference path="groupsController.js" />
'use strict';

webchat.controller("sidebarController", function ($scope, authenticationService, groupService) {
    $scope.me = authenticationService.getUsername();
    $scope.currentSidebar = {
        isContacts: true,
        isGroups: false
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

});
