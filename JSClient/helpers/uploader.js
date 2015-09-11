function sendFile(event) {
    var input = event.target;
    var statusDiv = $('#uploadStatus');
    var uploadInput = $('#file');
    var form_data = new FormData();
    form_data.append('file', input.files[0]);
    statusDiv.text('Uploading...');
    statusDiv.fadeIn(200);
    uploadInput.fadeOut(100);
    $.ajax({
        url: 'http://viber.azurewebsites.net/api/File', // point to server-side PHP script 
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        headers: {
            Authorization: 'Bearer ' + sessionStorage.accessToken
        },
        type: 'post',
        success: function (serverData) {
            statusDiv.fadeOut(100);
            uploadInput.val([]);
            uploadInput.fadeIn(200);
            console.log(serverData);
        },
        error: function (error) {
            console.log(error);
            statusDiv.text("Something went wrong.");
            uploadInput.fadeIn(200);
        }
    });
};