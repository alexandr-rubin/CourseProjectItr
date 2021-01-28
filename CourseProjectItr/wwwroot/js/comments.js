const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/comment")
    .build();

let userName = document.getElementById("userName").value;

function insertAfter(referenceNode, newNode) {
  referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}

hubConnection.on("Send", function (comment, userName) {
    $("<div><b class='d-inline-block'>" + userName + ": </b> <p class='d-inline-block'>" + comment + "</p></div>").insertBefore("#commentroom");

});


document.getElementById("sendBtn").addEventListener("click", function (e) {
    let comment = document.getElementById("comment").value;
    hubConnection.invoke("Send", comment, userName);
});

hubConnection.start();