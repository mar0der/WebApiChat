webchat.filter('yesNo', function () {
    return function (input) {
        return input ? 'online' : 'offline';
    }
});