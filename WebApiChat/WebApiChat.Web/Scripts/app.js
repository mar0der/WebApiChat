$(document).ready(function () {
    jQuery.support.cors = true;
    var connection = $.hubConnection('http://localhost:3660');
    var hub = connection.createHubProxy('messagesHub');
  //  hub.hello('kuf');

    //$.on('messageReceived', function(data) {
    //    alert(data);
    //});

    connection.start()
    .done(function () { console.log('Now connected, connection ID=' + connection.id); })
    .fail(function () { console.log('Could not connect'); });


    //var connection = $.hubConnection();
    //var contosoChatHubProxy = connection.createHubProxy('contosoChatHub');
    //contosoChatHubProxy.on('addContosoChatMessageToPage', function (userName, message) {
    //    console.log(userName + ' ' + message);
    //});
    //connection.start()
    //    .done(function () { console.log('Now connected, connection ID=' + connection.id); })
    //    .fail(function () { console.log('Could not connect'); });
})