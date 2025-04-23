function AlertConfirmWhenSubmitForm(title, text, icon, value) {
    Swal.fire({
        title: title,
        text: text.replace('${value}', value),
        icon: icon,
        showCancelButton: true,
        confirmButtonText: '<i class="fa fa-thumbs-up"></i>  Ok!',
        cancelButtonText: 'Cancel',
        confirmButtonColor: "#28a745",
        cancelButtonColor: "#dc3545",
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: 'Processing...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
            document.querySelector('#form').submit();
        }
    });
}
function unenrollAlert(courseId) {
    Swal.fire({
        title: `Are you sure you want to leave this course?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Leave',
        cancelButtonText: 'Cancel',
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: 'Processing...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            // Perform an action with the courseId, e.g., redirect
            window.location.href = `/Course/UnEnroll?courseId=${courseId}`;
        }
    });
}
function sweetAlertError(title, text) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: text,
        showConfirmButton: false,
        timer: 1500
    });
}

function sweetAlertSuccess(title, text) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: text,
        showConfirmButton: false,
        timer: 1500
    });
}