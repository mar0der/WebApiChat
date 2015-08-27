///<reference path="userServices.js" />

webchat.factory('userServices', function ($http, $q) {
    var serviceUrl = webchat.BASE_URL + 'Account';
    var service = {};

    service.login = function (loginData) {
        var deferred = $q.defer();
        $http({
            url: serviceUrl + '/Login',
            method: 'POST',
            data: "username=" + loginData.username + "&password=" + loginData.password +
                  "&grant_type=password"
        })
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (error) {
                deferred.reject(error);
            });

        return deferred.promise;
    };





    //function GetHeaders() {
    //    return {
    //        'Authorization': 'Bearer ' + sessionStorage['accessToken']
    //    };
    //}

    return service;

});