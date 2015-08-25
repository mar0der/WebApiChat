///<reference path="credentials.js"/>

var webchat = webchat || {}

function SetCredentials(serverData) {
    sessionStorage['accessToken'] = serverData.access_token;
    sessionStorage['username'] = serverData.userName;
   
};

function SetHeaders($http) {
    $http.defaults.headers.common = GetHeaders();
}

function GetHeaders() {
    return {
        'Authorization': 'Bearer ' + sessionStorage['accessToken']
    };
}