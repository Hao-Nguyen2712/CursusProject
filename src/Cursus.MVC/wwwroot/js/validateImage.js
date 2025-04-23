$(document).on("click", "#submitFormAvatar", function (event) {
        event.preventDefault();

        var fileInput = document.getElementById('fileInput');
        var file = fileInput.files[0];
        if (!file) {
            sweetAlertError('Error', 'Please select a file.');
            return;
        }
        var validImageHeaders = {
            "image/jpeg": [0xFF, 0xD8, 0xFF],
            "image/png": [0x89, 0x50, 0x4E, 0x47],
            "image/gif": [0x47, 0x49, 0x46, 0x38]
        };

        var fileReader = new FileReader();

        fileReader.onloadend = function(e) {
            if (e.target.readyState === FileReader.DONE) {
                var arrayBuffer = e.target.result;
                var uint = new Uint8Array(arrayBuffer);
                
                var valid = false;
                for (var header in validImageHeaders) {
                    var validHeader = validImageHeaders[header];
                    var isValid = validHeader.every((byte, index) => byte === uint[index]);
                    if (isValid) {
                        valid = true;
                        break;
                    }
                }

                if (!valid) {
                    sweetAlertError('Error', 'Invalid file format.');
                } else {
                    // Tạo một đối tượng FormData và thêm file vào đó
                    var formData = new FormData();
                    formData.append('file', file);

                    // Sử dụng fetch để gửi form
                    fetch('/Instructor/UploadAvatarStudent', {
                        method: 'POST',
                        body: formData
                    })
                    .then(response => response.text())
                    .then(result => {
                        console.log(result);
                        // Hiển thị thông báo hoặc xử lý kết quả ở đây
                        sweetAlertSuccess('Success', 'File uploaded successfully.');
                        window.location.reload();
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        sweetAlertError('Error', 'Failed to upload file.');
                    });
                }
            }
        };
        fileReader.readAsArrayBuffer(file.slice(0, 4));

});
$(document).on("click", "#submitAvatarInstructor", function (event) {
        event.preventDefault();
        debugger;
        var fileInput = document.getElementById('fileInput');
        var file = fileInput.files[0];
        if (!file) {
            sweetAlertError('Error', 'Please select a file.');
            return;
        }
        var validImageHeaders = {
            "image/jpeg": [0xFF, 0xD8, 0xFF],
            "image/png": [0x89, 0x50, 0x4E, 0x47],
            "image/gif": [0x47, 0x49, 0x46, 0x38]
        };

        var fileReader = new FileReader();

        fileReader.onloadend = function(e) {
            if (e.target.readyState === FileReader.DONE) {
                var arrayBuffer = e.target.result;
                var uint = new Uint8Array(arrayBuffer);
                
                var valid = false;
                for (var header in validImageHeaders) {
                    var validHeader = validImageHeaders[header];
                    var isValid = validHeader.every((byte, index) => byte === uint[index]);
                    if (isValid) {
                        valid = true;
                        break;
                    }
                }

                if (!valid) {
                    sweetAlertError('Error', 'Invalid file format.');
                } else {
                    // Tạo một đối tượng FormData và thêm file vào đó
                    var formData = new FormData();
                    formData.append('file', file);

                    // Sử dụng fetch để gửi form
                    fetch('/Instructor/UploadAvatar', {
                        method: 'POST',
                        body: formData
                    })
                    .then(response => response.text())
                    .then(result => {
                        console.log(result);
                        // Hiển thị thông báo hoặc xử lý kết quả ở đây
                        sweetAlertSuccess('Success', 'File uploaded successfully.');
                        window.location.reload();
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        sweetAlertError('Error', 'Failed to upload file.');
                    });
                }
            }
        };
        fileReader.readAsArrayBuffer(file.slice(0, 4));

});
