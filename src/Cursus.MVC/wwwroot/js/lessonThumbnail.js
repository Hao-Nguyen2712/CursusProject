// Lesson Thumbnail Upload Functionality
// Global variable to store current lesson thumbnail URL
window.currentLessonThumbnailUrl = null;

$(document).ready(function () {
    let currentLessonThumbnailUrl = null;

    // Handle lesson thumbnail upload
    $("#lessonThumbnailInput").change(function () {
        var formData = new FormData();
        var file = $(this)[0].files[0];
        
        if (!file) {
            return;
        }

        // Validate file type
        const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
        if (!allowedTypes.includes(file.type)) {
            sweetAlertError("Error", "Please select a valid image file (JPG, PNG, or GIF)");
            return;
        }

        // Validate file size (max 5MB)
        const maxSize = 5 * 1024 * 1024; // 5MB in bytes
        if (file.size > maxSize) {
            sweetAlertError("Error", "Image size should be less than 5MB");
            return;
        }

        formData.append("file", file);

        // Show loading state
        $("#lessonThumbnailContainer").css('opacity', '0.5');
        
        $.ajax({
            url: "/Instructor/UploadLessonThumbnail",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                console.log("Upload response:", response);
                $("#lessonThumbnailContainer").css('opacity', '1');
                
                if (response.success && response.url) {
                    sweetAlertSuccess("Success", "Lesson thumbnail uploaded successfully");
                    
                    // Update the preview image
                    $("#lessonThumbnailContainer").attr("src", response.url);
                    
                    // Store the thumbnail URL for use when creating the lesson
                    currentLessonThumbnailUrl = response.url;
                    window.currentLessonThumbnailUrl = response.url;
                    
                    // Add the thumbnail URL to any existing lesson creation forms
                    updateLessonFormWithThumbnail(response.url);
                } else {
                    sweetAlertError("Error", response.message || "An error occurred while uploading the thumbnail");
                }
            },
            error: function (xhr, status, error) {
                $("#lessonThumbnailContainer").css('opacity', '1');
                console.error("Upload error:", error);
                sweetAlertError("Error", "Failed to upload thumbnail. Please try again.");
            }
        });
    });

    // Function to update lesson creation forms with thumbnail URL
    function updateLessonFormWithThumbnail(thumbnailUrl) {
        // Add hidden input for thumbnail URL if it doesn't exist
        if ($('#lessonThumbnailUrl').length === 0) {
            $('<input>').attr({
                type: 'hidden',
                id: 'lessonThumbnailUrl',
                name: 'thumbnailUrl',
                value: thumbnailUrl
            }).appendTo('#lessonThumbnailForm');
        } else {
            $('#lessonThumbnailUrl').val(thumbnailUrl);
        }
    }

    // Integrate with existing lesson creation functionality
    $(document).on('click', '#add-lesson-btn', function() {
        // If a thumbnail was uploaded, include it in the lesson data
        if (currentLessonThumbnailUrl) {
            // Update any lesson creation AJAX calls to include thumbnail URL
            updateLessonCreationWithThumbnail();
        }
    });

    // Function to modify existing lesson creation AJAX calls
    function updateLessonCreationWithThumbnail() {
        // This will integrate with existing lesson creation functionality
        // The thumbnail URL will be available in currentLessonThumbnailUrl
        console.log("Lesson thumbnail ready:", currentLessonThumbnailUrl);
    }

    // Reset thumbnail when form is reset
    $(document).on('reset', '#lessonThumbnailForm', function() {
        $("#lessonThumbnailContainer").attr("src", "~/images/thumbnail-demo.jpg");
        currentLessonThumbnailUrl = null;
        $('#lessonThumbnailUrl').remove();
    });
});

// Sweet Alert helper functions (if not already defined elsewhere)
if (typeof sweetAlertSuccess === 'undefined') {
    function sweetAlertSuccess(title, message) {
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'success',
                title: title,
                text: message,
                timer: 3000,
                showConfirmButton: false
            });
        } else {
            alert(title + ": " + message);
        }
    }
}

if (typeof sweetAlertError === 'undefined') {
    function sweetAlertError(title, message) {
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'error',
                title: title,
                text: message
            });
        } else {
            alert(title + ": " + message);
        }
    }
}