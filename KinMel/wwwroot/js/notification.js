// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/notificationHub")
  .build();

connection.on("Receivecount", (count) => {
  var toastHTML = '<span>You have ' + count + ' unread notifications!</span><a href="/notifications" class="btn-flat toast-action">Show All</a>';
  M.toast({ html: toastHTML, classes: 'rounded' });

  updateNotificationComponent(count);
});

connection.on("NotificationDeleted", (count) => {
  updateNotificationComponent(count);
});
function updateNotificationComponent(count) {

  var notificationCount = $("#notificationCount");
  notificationCount.empty();
  if (count > 0) {
    notificationCount.css("display", "block");
    notificationCount.html(count);
  }
  var container = $("#notificationdropdown");
  $.get("/Notifications/NotificationViewComponent", function (data) {
    container.empty();
    container.html(data);
  });
}

connection.start().catch(err => console.error(err.toString()));
//document.getElementById("sendButton").addEventListener("click", event => {
//    const user = document.getElementById("userInput").value;
//    const message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
//    event.preventDefault();
//});
//setInterval(updateMessages, 30000);
//function updateMessages() {
//    connection.invoke("NotificationCount").catch(err => console.error(err.toString()));
//    event.preventDefault();
//}

//document.getElementById("updatecount").addEventListener("click", event => {

//    connection.invoke("NotificationCount").catch(err => console.error(err.toString()));
//    event.preventDefault();
//});
