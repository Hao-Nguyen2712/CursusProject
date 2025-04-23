function notification(type, message) {
    var notificationElement = $("#notification");

    notificationElement.kendoNotification({
      templates: [
        {
          // define a custom template for the built-in "warning" notification type
          type: "warning",
          template: "<div class='myWarning'>Warning: #= myMessage #</div>",
        },
        {
          type: "info",
          template: "<div class='myInfo'>Info: #= myMessage #</div>",
        },
        {
          type: "success",
          template: "<div class='mySuccess'>Success: #= myMessage #</div>",
        },
        {
          type: "error",
          template: "<div class='myError'>Error: #= myMessage #</div>",
        },
      ],
      autoHideAfter: 3000,
      stacking: "down",
      position: {
        pinned: true,
        top: 80,
        right: 30,
      },
      show: function (e) {
        e.element.hide().fadeIn(500);
      },
      hide: function (e) {
        e.element.fadeOut(500);
      },
    });

    function showNotification(type, message) {
      var n = notificationElement.data("kendoNotification");
      n.show(
        {
          myMessage: message,
        },
        type
      );
    }
    showNotification(type, message);
  }
