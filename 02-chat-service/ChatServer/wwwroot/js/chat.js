"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat").build();

document.getElementById("registerButton").disabled = true;
document.getElementById("sendButton").disabled = true;

document.getElementById("myName").addEventListener("input",
    function () {
        document.getElementById("from").value = document.getElementById("myName").value;
    }
);


// connection 시작
connection.start().then(function () {
    document.getElementById("registerButton").disabled = false;
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


// connection 구독
connection.on("ReceiveMessage", function (received) {
    var li = document.createElement("li");
    document.getElementById("messages").appendChild(li);

    li.textContent =
        `To ${received.to}, From ${received.from}: ${received.body}`;
});

// registerButton 이벤트 등록
document.getElementById("registerButton").addEventListener("click", function (event) {
    var registerModel = {
        name: document.getElementById("myName").value,
        groups: document.getElementById("myGroups").value
    };

    // connection을 통해 메서드 호출
    connection.invoke("Register", registerModel).catch(function (err) {
        return console.error(err.toString());
    })

    event.preventDefault();
});


// sendButton 이벤트리스너 등록
document.getElementById("sendButton").addEventListener("click",
    function (event) {
        var messageModel = {
            from: document.getElementById("from").value,
            to: document.getElementById("to").value,
            body: document.getElementById("body").value
        };

        // connection으로 메서드 호출
        connection.invoke("SendMessage", messageModel).catch(function (err) {
            return console.error(err.toString());
        })

        event.preventDefault();
    }
);
