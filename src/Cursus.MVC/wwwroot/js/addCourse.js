// This script is used to add a course to the database.
// It is called when the user clicks on the "Pushlish" button in the "AddCourses" view.
$(document).ready(function () {
    $("#submitForm").on("click", function () {
        Swal.fire({
            title: "Are you sure you want to submit add course?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Ok!",
            cancelButtonText: "Cancel",
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: "Processing...",
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
                debugger;
                var priceCourse = $("#price").val();
                var discount = $("#discount").val();
                if (priceCourse < 0 || discount < 0) {
                    sweetAlertError(
                        "Error",
                        "Price and Discount must be greater than 0"
                    );
                    return;
                }
                if (priceCourse == "") {
                    priceCourse = 0;
                    $("#price").val(0);
                }
                if (discount == "") {
                    discount = 0;
                    $("#discount").val(0);
                }
                if (discount > priceCourse) {
                    sweetAlertError(
                        "Error",
                        "Discount must be less than price"
                    );
                    return;
                }

                var category = $("#selectcategory").val();
                if (category == "Select Category") {
                    $(".selectcategory").val("Development");
                    category = "Development";
                }

                var thumbnail = $("#fileInput").val();
                if (thumbnail == "") {
                    thumbnail = "thumbnail-demo.jpg";
                }

                var course = {
                    CourseName: $("#courseTitle").val(),
                    CourseShortDes: $("#shortDescription").val(),
                    CourseDescription: window.editor1.getData(),
                    CourseWlearn: $("#whatLearn").val(),
                    CourseRequirement: $("#requirements").val(),
                    CourseAvatar: thumbnail,
                    CourseDate: Date.now(),
                    CourseMoney: priceCourse,
                    CourseStatus: "Pending Approval",
                    CourseProcess: 1,
                    DiscountId: 1,
                    CategoryId: 1,
                    Discount: discount,
                    CategoryId: category,
                };

                if (!isCourseValid(course)) {
                    sweetAlertError(
                        "Error",
                        "Please fill in all fields before submitting the form"
                    );
                    return;
                }

                var youtube = $("#youtobeLinkCourse").val();

                var data = {
                    course: course,
                    youtube: youtube,
                };

                // Send the course object to the server using AJAX
                $.ajax({
                    type: "POST",
                    url: "/Instructor/AddCourse",
                    data: data,
                    success: function (data) {
                        debugger;
                        if (data == "True") {
                            sweetAlertSuccess(
                                "Success",
                                "Course added successfully"
                            );
                            window.location.href = "/Instructor/GetAllCourseByStatus";
                        } else {
                            sweetAlertError(
                                "Error",
                                "An error occurred while adding the course"
                            );
                        }
                    },
                    error: function () {
                        debugger;
                        sweetAlertError(
                            "Error",
                            "An error occurred while adding the course"
                        );
                    },
                });
            }
        });
    });
});

// add thumanail image
$(document).ready(function () {
    $("#uploadForm").change(function () {
        var formData = new FormData();
        var file = $("#fileInput")[0].files[0];
        formData.append("file", file);

        $.ajax({
            url: "/Instructor/UploadImage",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                console.log(data);
                if (data.url) {
                    sweetAlertSuccess("Success", "Image uploaded successfully");
                    var url = data.url;
                    $("#imageContainer").attr("src", url);
                } else {
                    sweetAlertError(
                        "Error",
                        "An error occurred while uploading the image"
                    );
                }
            },
            error: function () {
                sweetAlertError(
                    "Error",
                    "An error occurred while uploading the image"
                );
            },
        });
    });
});

function isCourseValid(course) {
    debugger;
    for (var key in course) {
        if(key != "CourseMoney" && key != "Discount"){
            if (
                course[key] == "" ||
                course[key] == null ||
                course[key] == "undefined"
            ) {
                return false;
            }
        }
    }
    return true;
}

$(document).on("click", ".editLesson", function () {
    console.log("edit lesson");
    var boxLesson = $(this).closest(".section-list-item");
    var id = $(this).val();
    console.log(id);

    // ajax get lesson
    $.ajax({
        type: "POST",
        url: "/Instructor/GetLessonToEdit",
        data: {
            id: id,
        },
        success: function (data) {
            console.log(data);
            if (data.title != "") {
                $("#inputEditTitle").val(data.lessionTilte);
                window.editor10.setData(data.lessionContent);
                $("#videoLinkEdit").val(data.lessionVideo);
                $("#btnEditLesson").val(id);
            } else {
                sweetAlertError(
                    "Error",
                    "An error occurred while getting the lesson"
                );
            }
        },
        error: function () {
            sweetAlertError(
                "Error",
                "An error occurred while getting the lesson"
            );
        },
    });

    $(document).on("click", "#btnEditLesson", function () {
        console.log("edit lesson");
        var title = $("#inputEditTitle").val();
        var description = window.editor10.getData();
        var videoLink = $("#videoLinkEdit").val();
        console.log(title, description, videoLink);
        var groupItem = $("#groupTitle");
        // check value is null
        if (title == "" || description == "" || videoLink == "") {
            sweetAlertError(
                "Error",
                "Please fill in all fields before submitting the form"
            );
            return;
        }

        $.ajax({
            type: "POST",
            url: "/Instructor/EditLesson",
            data: {
                title: title,
                description: description,
                videoLink: videoLink,
                id: id,
            },
            success: function (data) {
                console.log(data);
                if (data.title != "") {
                    var newItem =
                        `
											<div class="section-list-item">
												<div class="section-item-title">
													<i class="fas fa-file-alt me-2"></i>
													<span class="section-item-title-text"> ` +
                        data.title +
                        `</span>
												</div>
													<button id="edit` +
                        data.id +
                        `" value="` +
                        data.id +
                        `" type="button" class="section-item-tools editLesson"><i
													class="fas fa-edit"></i></button>
													<button id="remove` +
                        data.id +
                        `" value="` +
                        data.id +
                        `" type="button" class="section-item-tools removeLesson"><i
													class="fas fa-trash-alt"></i></button>
											</div>
								`;
                    // delete old item
                    var oldItem = groupItem.children().eq(id - 1);
                    console.log(oldItem);
                    if (oldItem.length > 0) {
                        console.log("replace");
                        oldItem.replaceWith(newItem);
                    } else {
                        console.log("append");
                        groupItem.append(newItem);
                    }
                    $("#btnCancelLesson").click();
                    sweetAlertSuccess("Success", "Lesson edited successfully");
                } else {
                    sweetAlertError(
                        "Error",
                        "An error occurred while editing the lesson"
                    );
                }
            },
            error: function () {
                sweetAlertError(
                    "Error",
                    "An error occurred while editing the lesson"
                );
            },
        });
    });
});

$(document).on("click", ".removeLesson", function () {
    var id = $(this).val();
    if (id == "" || id == null) {
        sweetAlertError("Error", "An error occurred while removing the lesson");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Instructor/RemoveLesson",
        data: {
            id: id,
        },
        success: function (data) {
            if (data == "True") {
                $("#remove" + id)
                    .closest(".section-list-item")
                    .remove();
                sweetAlertSuccess("Success", "Lesson removed successfully");
            } else {
                sweetAlertError(
                    "Error",
                    "An error occurred while removing the lesson"
                );
            }
        },
        error: function () {
            sweetAlertError(
                "Error",
                "An error occurred while adding the course"
            );
        },
    });
});

$(document).on("click", "#submitEditForm", function () {
    var priceCourse = $("#price").val();
    var discount = $("#discount").val();
    if (priceCourse < 0 || discount < 0) {
        sweetAlertError("Error", "Price and Discount must be greater than 0");
        return;
    }
    if (priceCourse == "") {
        priceCourse = 0;
        $("#price").val(0);
    }
    if (discount == "") {
        discount = 0;
        $("#discount").val(0);
    }
    if (discount > priceCourse) {
        sweetAlertError("Error", "Discount must be less than price");
        return;
    }

    var category = $("#selectcategory").val();
    if (category == "Select Category") {
        sweetAlertError("Error", "Please select a category");
        return;
    }

    var thumbnail = $("#fileInput").val();
    if (thumbnail == "") {
        thumbnail = "thumbnail-demo.jpg";
    }

    var course = {
        CourseName: $("#courseTitle").val(),
        CourseShortDes: $("#shortDescription").val(),
        CourseDescription: window.editor1.getData(),
        CourseWlearn: $("#whatLearn").val(),
        CourseRequirement: $("#requirements").val(),
        CourseAvatar: thumbnail,
        CourseDate: Date.now(),
        CourseMoney: priceCourse,
        CourseStatus: "Pending Approval",
        CourseProcess: 10,
        DiscountId: 10,
        CategoryId: 10,
        Discount: discount,
        CategoryId: category,
    };
    console.log(course);
    console.log(isCourseValid(course));
    if (!isCourseValid(course)) {
        sweetAlertError(
            "Error",
            "Please fill in all fields before submitting the form"
        );
        return;
    }
    var youtube = $("#youtobeLinkCourse").val();
    var data = {
        course: course,
        youtube: youtube,
        id: $("#CourseID").val(),
    };
    $.ajax({
        type: "POST",
        url: "/Instructor/EditCourse",
        data: data,
        success: function (data) {
            if (data == "True") {
                sweetAlertSuccess("Success", "Course edited successfully");
                window.location.href = "/Instructor/GetAllCourseByStatus";
            } else {
                sweetAlertError(
                    "Error",
                    "An error occurred while editing the course"
                );
            }
        },
        error: function () {
            sweetAlertError(
                "Error",
                "An error occurred while editing the course"
            );
        },
    });
});
