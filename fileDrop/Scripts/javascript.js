$(document).ready(function () {

    // Makes sure the dataTransfer information is sent when we
    // Drop the item in the drop box.
    jQuery.event.props.push('dataTransfer');

    // Get all of the data URIs and put them in an array
    var dataArray = [];

    // Bind the drop event to the dropzone.
    $('#drop-files').bind('drop', function (e) {

        // Stop the default action, which is to redirect the page
        // To the dropped file

        var files = e.dataTransfer.files;

        // Show the upload holder
        $('#uploaded-holder').show();

        // For each file
        $.each(files, function (index, file) {

            // Some error messaging
            if (!files[index].type.match('image.*')) {
                $('#drop-files').html('Sorry, images only');
                return false;
            }
            // Start a new instance of FileReader
            var fileReader = new FileReader();

            // When the filereader loads initiate a function
            fileReader.onload = (function (file) {

                return function (e) {

                    // Push the data URI into an array
                    dataArray.push({ name: file.name, value: this.result, uploaded: false });

                    uploadFiles();
                    // Place extra files in a list
                    //                    if ($('#dropped-files > .image').length < maxFiles) {
                    //                        uploadFiles();

                    //                    }
                };

            })(files[index]);

            // For data URI purposes
            fileReader.readAsDataURL(file);

        });

        dataArray.length = 0;


    });

    function uploadFiles() {
        $("#loading").show();
        var totalPercent = 100 / dataArray.length;
        var x = 0;
        var y = 0;

        $('#loading-content').html('Uploading ' + dataArray[0].name);

        $.each(dataArray, function (index, file) {
            if (!dataArray[index].uploaded) {
                //mark as uploaded immediately to avoid race condition, display error if problem uploading
                dataArray[index].uploaded = true;
                $.post('/home/upload', dataArray[index], function (data) {

                    var fileName = dataArray[index].name;
                    ++x;

                    // Change the bar to represent how much has loaded
                    $('#loading-bar .loading-color').css({ 'width': totalPercent * (x) + '%' });

                    if (totalPercent * (x) == 100) {
                        // Show the upload is complete
                        $('#loading-content').html('Uploading Complete!');

                        // Reset everything when the loading is completed
                        setTimeout(restartLoadingBar, 500);

                    } else if (totalPercent * (x) < 100) {

                        // Show that the files are uploading
                        $('#loading-content').html('Uploading ' + fileName);

                    }

                    var dataSplit = data.split(':');
                    if (dataSplit[1] == 'uploaded successfully') {
                        dataArray[index].id = dataSplit[2];
                        $('.dropped-files').prepend(
                        '<div class="image" style="background: url(' + dataArray[index].value + '); background-size: cover;" id="image-' + dataSplit[2] + '">' +
				            '<a href="#" class="delete" id="' + dataSplit[2] + '">delete</a>' +
                        '</div>'
                        );
                        
                    } else {
                        $('body').append('<h3>error uploading file ' + dataArray[index].name + '</h3>');
                    }
                });
            }
        });

        return false;
    }

    function restartLoadingBar() {

        // This is to set the loading bar back to its default state
        $('#loading-bar .loading-color').css({ 'width': '0%' });
        $('#loading').css({ 'display': 'none' });
        $('#loading-content').html(' ');
        // --------------------------------------------------------

        return false;
    }

    // Just some styling for the drop file container.
    $('#drop-files').bind('dragenter', function () {
        $(this).css({ 'box-shadow': 'inset 0px 0px 20px rgba(0, 0, 0, 0.1)', 'border': '4px dashed #bb2b2b' });
        return false;
    });

    $('#drop-files').bind('drop', function () {
        $(this).css({ 'box-shadow': 'none', 'border': '4px dashed rgba(0,0,0,0.2)' });
        return false;
    });

    // For the file list
    $('#extra-files .number').toggle(function () {
        $('#file-list').show();
    }, function () {
        $('#file-list').hide();
    });

    //$('#dropped-files .upload-button .delete').click(restartFiles);

    $('.image .delete').live('click', (function (e) {
        //grab id from image element
        var image_id = $(this).attr('id');

        $.ajax({
            url: '/home/delete',
            type: 'POST',
            data: { 'id': image_id },
            statusCode: {
                500: function (xhr) {
                    if (window.console) console.log(xhr.responseText);
                },
                200: function (xhr) {
                    //remove image
                    $('#image-' + image_id).remove();
                }
            }
        });

    }));

});