webchat.controller("authenticationController", function ($scope, usersService, authenticationService, $location, signalR, $rootScope) {
    var clearData = function () {
        //$scope.loginData = "";
        $scope.registerData = "";
        $scope.userData = "";
        $scope.passwordData = "";
    };


    $scope.login = function login(loginData) {
        usersService.login(loginData)
        .then(function (serverData) {
            //notyService.showInfo("Successful Login!");
            authenticationService.setCredentials(serverData.data);
            $rootScope.$broadcast('login');
            clearData();

            $location.path('/');
        },
        function (serverError) {
            console.log(serverError);
            //notyService.showError("Unsuccessful Login!", serverError);
        });
    };


    $scope.register = function register(registerData) {
        //usSpinnerService.spin('spinner');
        usersService.register(registerData)
        .then(function (serverData) {
            //notyService.showInfo("Successful Registeration!");
            authenticationService.setCredentials(serverData.data);
            $rootScope.$broadcast('login');
            clearData();
            //usSpinnerService.stop('spinner');
            $location.path('/');
        },
        function (serverError) {
            //usSpinnerService.stop('s
            // pinner');
            //notyService.showError("Unsuccessful Registeration!", serverError);
        });
    };

    $scope.logout = function logout() {
        usersService.logout()
            .then(function () {
                authenticationService.clearCredentials();
                $rootScope.$broadcast('logout');
                sessionStorage.clear();
                console.log('bye')
                //notyService.showInfo("Successful Logout!");
                $location.path('/login');
            }, function () {
                authenticationService.clearCredentials();
                $location.path('/');
            });
    }

    $scope.isLoggedIn = function isLoggedIn() {
        return authenticationService.isLoggedIn();
    }
});