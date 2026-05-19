// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let connection;
$(document).ready(() => {
    connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/notify")
            .withUrl("/notificationHub")
            .build();

    connection.start()
        .then(() => {
            console.log("Connected");
        })
        .catch(err => console.error(err));

    connection.on(
        "ReceiveMessage",
        function (message) {
            const li =
                document.createElement("li");

            li.textContent = message;

            document
                .getElementById("messages")
                .appendChild(li);
        });

    connection.on("ReceiveNotification",
        function (message) {
            let box =
                document.getElementById(
                    "notifyBox");

            let div =
                document.createElement("div");

            div.className =
                "alert alert-success";

            div.innerHTML = message;

            box.appendChild(div);

            setTimeout(() => {
                div.remove();
            }, 3000);
        });

});

function sendMessage() {
    const message =
        document.getElementById("message").value;

    connection.invoke(
        "SendMessage",
        message);
}

function openEditModal(id) {
    $.get('/Employee/Edit/' + id,
        function (response) {
            $('#modalBody').html(response);

            $('#employeeModal').modal('show');
        });
}