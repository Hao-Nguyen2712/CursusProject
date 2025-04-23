$(document).ready(function () {
  $(".btn-reply-container").click(function () {
    $(this).find(".bi").toggleClass("bi bi-caret-up");
  });

  $("#comment-input-text").click(function () {
    $(".cmt-action-submit").toggleClass("active");
  });

  // $(".btn-comment").click(function(){
  //     $(".cmt-action-submit").toggleClass("active");
  // })

  $(".btn-cancel-comment").click(function () {
    $(".cmt-action-submit").toggleClass("active");
  });

  $(".btn-cancel-lv1").click(function () {
    var collapseElement = $(this).parent().parent().parent();
    collapseElement.collapse("hide");
  });
});

$(document).on("click", ".btn-cancel-lv2", function () {
  console.log("click cancel lv2");
  var collapseElement = $(this).parent().parent().parent();
  collapseElement.collapse("hide");
});

// add comment
$(document).ready(function () {
  $(".btn-comment").on("click", function () {
    // get value from input
    var comment = $("#comment-input-text").val();
    var lessonID = $("#LessonID").val();
      if (comment == "") {
        sweetAlertError("Error", "Please enter your comment");
        return;
    }


    $.ajax({
      url: "/Comment/AddComment",
      type: "POST",
      data: {
        lessonID: lessonID,
        content: comment,
        reply: "",
      },
      success: function (data) {
        console.log(data);
        console.log(data.lessonID);
        var newComment = newCommnet(
          data.lessonID,
          data.comment,
          data.accountViewModel
        );
        // append new comment
          var elementParent = $(".btn-comment").parent().parent().parent().parent();
          console.log($(this))
          console.log(elementParent);
        elementParent.append(newComment);
        // clear input
        $("#comment-input-text").val("");
        $(".cmt-action-submit").toggleClass("active");
        $(".countComments").val(parseInt($(".countComments").val()) + 1);
      },
      error: function (err) {
        console.log(err);
      },
    });
    function newCommnet(lessionID, comment, Account) {
      var idCollapse = "collapse" + comment.cmtId;
      var idCollapseReply = "collapseReply" + comment.cmtId;
      var newComment =
        `<div class="cmt-item" id="` +
        comment.cmtId +
        `">
                  <div class="cmt-item-parent">
                    <div class="cmt-avatar">
                      <img src="` +
        Account.avatar +
        `" alt="" />
                    </div>
                    <div class="cmt-content-box">
                      <div class="cmt-header">
                        <div class="cmt-name">
                          <h4>` +
        Account.fullName +
        `</h4>
                        </div>
                        <div class="cmt-time">
                          <p>2 giờ trước</p>
                        </div>
                      </div>
                      <div class="cmt-content">
                        <p>
                          ` +
        comment.cmtContent +
        `
                        </p>
                      </div>
                      <div class="cmt-action">
                        <div class="cmt-report">
                          <span>Report</span>
                        </div>
                        <div
                          class="cmt-reply"
                          data-bs-toggle="collapse"
                          href="#` +
        idCollapse +
        `"
                          role="button"
                          aria-expanded="false"
                          aria-controls="` +
        idCollapse +
        `"
                        >
                          <span>Reply</span>
                        </div>
                      </div>
                      <div class="cmt-item-childen">
                        <div
                          class="comment-reply-input cmt-item-parent collapse"
                          id="` +
        idCollapse +
        `"
                        >
                          <div class="comment-reply-avatar">
                            <img src="` +
        Account.avatar +
        `" alt="" />
                          </div>
                          <div class="comment-reply--content">
                            <textarea
                              type="text"
                              placeholder="Add a reply..."
                              class="comment-input--text"
                            ></textarea>
                            <div class="cmt-reply-action">
                              <button class="comment-input--btn btn-cancel btn-cancel-lv1">
                                Cancel
                              </button>
                              <button class="comment-input--btn btn-reply">
                                Reply
                              </button>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>`;
      return newComment;
    }
  });
});

$(document).on("click", ".btn-reply", function () {
  var inputReply = $(this)
    .closest(".comment-reply--content")
    .find(".comment-input--text")
        .val();
    if (inputReply == "") {
        sweetAlertError("Error", "Please enter your comment");
        return;
    }
  var lessonID = $("#LessonID").val();


  // Append the new reply comment
  var parentElement = $(this).parent().parent().parent().parent();
  var numChilden = parentElement.children().length;
  if (numChilden == 1) {
    // get cloeset class .cmt-item
    var commentElement = $(this).closest(".cmt-item");
    var idComment = commentElement.attr("id");
    $.ajax({
      url: "/Comment/AddReplyComment",
      type: "POST",
      data: {
        lessonID: lessonID,
        content: inputReply,
        reply: idComment,
      },
      success: function (data) {
        console.log(data);
        console.log(data.lessonID);
        var newCM = newReply(
          data.lessonID,
          data.comment,
          data.accountViewModel
        );
        console.log(newCM);
        parentElement.append(newCM);
        // clear input
        $("#comment-input-text").val("");
        $(".countComments").val(parseInt($(".countComments").val()) + 1);
      },
      error: function (err) {
        console.log(err);
      },
    });
    function newReply(lessionID, comment, Account) {
      var idCollapse = "collapse" + comment.cmtId;
      var idCollapseReply = "collapseReply" + comment.cmtId;
      var newComment =
        `
                    <button
                      class="btn-reply-container"
                      type="button"
                      data-bs-toggle="collapse"
                      data-bs-target="#` +
                      idCollapseReply +
        `"
                      aria-expanded="false"
                      aria-controls="` +
                      idCollapseReply +
        `"
                    >
                      <i class="bi bi-caret-down"></i> <span>1</span> Reply
                    </button>
                    <div class="collapse" id="` +
                    idCollapseReply +
        `">
                      <div class="cmt-item" id="` +
        comment.cmtReply +
        `">
                        <div class="cmt-item-parent">
                          <div class="cmt-avatar">
                            <img src="` +
        Account.avatar +
        `" alt="" />
                          </div>
                          <div class="cmt-content-box">
                            <div class="cmt-header">
                              <div class="cmt-name">
                                <h4>` +
        Account.fullName +
        `</h4>
                              </div>
                              <div class="cmt-time">
                                <p>2 giờ trước</p>
                              </div>
                            </div>
                            <div class="cmt-content">
                              <p>
                                ` +
        inputReply +
        `
                              </p>
                            </div>
                            <div class="cmt-action">
                              <div class="cmt-report">
                                <span>Report</span>
                              </div>
                              <div
                                class="cmt-reply"
                                data-bs-toggle="collapse"
                                href="#` +
                                idCollapse +
        `"
                                role="button"
                                aria-expanded="false"
                                aria-controls="` +
                                idCollapse +
        `"
                              >
                                <span>Reply</span>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class=" comment-reply-input cmt-item-parent collapse" id="` +
                      idCollapse +
        `">
                        <div class="comment-reply-avatar">
                          <img src="` +
        Account.avatar +
        `" alt="" />
                        </div>
                        <div class="comment-reply--content">
                          <textarea
                            type="text"
                            placeholder="Add a reply..."
                            class="comment-input--text"
                            id="comment-input-text-reply"
                          ></textarea>
                          <div class="cmt-reply-action">
                            <button class="comment-input--btn btn-cancel btn-cancel-lv2">
                              Cancel
                            </button>
                            <button class="comment-input--btn btn-reply">
                              Reply
                            </button>
                          </div>
                        </div>
                      </div>
                      </div>
`;
      return newComment;
    }
  } else {
    var commentElement = $(this).closest(".cmt-item");
    var idComment = commentElement.attr("id");
    var lastChild = $(this).closest(".cmt-item-childen").children().last();
    $.ajax({
      url: "/Comment/AddReplyComment",
      type: "POST",
      data: {
        lessonID: lessonID,
        content: inputReply,
        reply: idComment
      },
      success: function (data) {
        console.log(data);
        console.log(data.lessonID);
        var newCM = newReplyLV2(
          data.lessonID,
          data.comment,
          data.accountViewModel
        );
        console.log(newCM);
        lastChild.append(newCM);
        // clear input
        $("#comment-input-text").val("");
        $(".countComments").val(parseInt($(".countComments").val()) + 1);
      },
      error: function (err) {
        console.log(err);
      },
    });
  
    var numberElement = $(this)
      .closest(".cmt-item-childen")
      .children()
      .eq(1)
      .find("span");
    numberElement.text(parseInt(numberElement.text()) + 1);
    function newReplyLV2(lessionID, comment, Account) {
      var idCollapse = "collapse" + comment.cmtId;
      var idCollapseReply = "collapseReply" + comment.cmtId;
      var  newComment =   `<div class="cmt-item">
                            <div class="cmt-item-parent">
                              <div class="cmt-avatar">
                                <img src="`+Account.avatar+`" alt="" />
                              </div>
                              <div class="cmt-content-box">
                                <div class="cmt-header">
                                  <div class="cmt-name">
                                    <h4>`+Account.fullName+`</h4>
                                  </div>
                                  <div class="cmt-time">
                                    <p>2 giờ trước</p>
                                  </div>
                                </div>
                                <div class="cmt-content">
                                  <p>
                                    ` +
    inputReply +
    `
                                  </p>
                                </div>
                                <div class="cmt-action">
                                  <div class="cmt-report">
                                    <span>Report</span>
                                  </div>
                                  <div
                                    class="cmt-reply"
                                    data-bs-toggle="collapse"
                                    href="#`+idCollapse+`"
                                    role="button"
                                    aria-expanded="false"
                                    aria-controls="`+idCollapse+`"
                                  >
                                    <span>Reply</span>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div class=" comment-reply-input cmt-item-parent collapse" id="`+idCollapse+`">
                            <div class="comment-reply-avatar">
                              <img src="`+Account.avatar+`" alt="" />
                            </div>
                            <div class="comment-reply--content">
                              <textarea
                                type="text"
                                placeholder="Add a reply..."
                                class="comment-input--text"
                                id="comment-input-text-reply"
                              ></textarea>
                              <div class="cmt-reply-action">
                                <button class="comment-input--btn btn-cancel btn-cancel-lv2">
                                  Cancel
                                </button>
                                <button class="comment-input--btn btn-reply">
                                  Reply
                                </button>
                              </div>
                            </div>
                          </div>`;
                          return  newComment;
    }
  }
  // Clear input
  $(this)
    .closest(".comment-reply--content")
    .find(".comment-input--text")
    .val("");
  // Hide the reply input area
  $(this).closest(".comment-reply-input").collapse("hide");
  // element string function
});


// ajax study
$(document).on("click", "#submitStudy", function () {
    var lessonID = $("#LessonID").val();
    var courseID = $("#CourseID").val();
    var btnStudy = $(this);
    var value = btnStudy.val();
    console.log(value);
    if (value == "Finish") {
        $.ajax({
            url: "/Comment/FinishStudy",
            type: "POST",
            data: {
                lessonID: lessonID,
                courseID: courseID
            },
            success: function (data) {
                console.log(data);
                if (data == "True") {
                    btnStudy.addClass("active");
                    btnStudy.text("Not Finish");
                    btnStudy.val("Not Finish");
                    var iconSuccess = $(".iconSuccess");
                    console.log(iconSuccess);
                    for (var i = 0; i < iconSuccess.length; i++) {
                        if(iconSuccess[i].getAttribute('data-lessonid') == lessonID) {
                            // change css
                            $(iconSuccess[i]).css("color", "green");
                        }
                        
                    }
                }
                else {
                    // alert("Error");
                    alert("Error when not finish study");
                }
            },
            error: function (err) {
                console.log(err);
            },
        });
    } else {
        $.ajax({
            url: "/Comment/NotFinishStudy",
            type: "POST",
            data: {
                lessonID: lessonID,
                courseID: courseID
            },
            success: function (data) {
                console.log(data);
                if (data == "True") {
                    btnStudy.removeClass("active");
                    btnStudy.text("Finish");
                    btnStudy.val("Finish");
                    var iconSuccess = $(".iconSuccess");
                    console.log(iconSuccess);
                    for (var i = 0; i < iconSuccess.length; i++) {
                        if(iconSuccess[i].getAttribute('data-lessonid') == lessonID) {
                            // change css
                            $(iconSuccess[i]).css("color", "transparent");
                        }
                        
                    }
                }
                else {
                    // alert("Error");
                    alert("Error when not finish study");
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
  

});