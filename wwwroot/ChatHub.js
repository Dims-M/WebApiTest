const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

let userName = '';
// получение сообщения от сервера
hubConnection.on('Send', function (message, userName) {

    // создаем элемент <b> для имени пользователя
    let userNameElem = document.createElement("b");
    userNameElem.appendChild(document.createTextNode(userName + ': '));

    // создает элемент <p> для сообщения пользователя
    let elem = document.createElement("p");
    elem.appendChild(userNameElem);
    elem.appendChild(document.createTextNode(message));

    var firstElem = document.getElementById("chatroom").firstChild;
    document.getElementById("chatroom").insertBefore(elem, firstElem);

});

// установка имени пользователя
document.getElementById("loginBtn").addEventListener("click", function (e) {
    userName = document.getElementById("userName").value;
    document.getElementById("header").innerHTML = '<h3>Welcome ' + userName + '</h3>';
});
// отправка сообщения на сервер
document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;
    hubConnection.invoke("Send", message, userName);
});

hubConnection.start();

//Старые методы

//////Устанавливается подключение
//const hubConnection = new signalR.HubConnectionBuilder() // объект подключения
//    .withUrl("/chat") //Метод withUrl устанавливает адрес, по котору приложение будет обращаться к хабу.
//    .build();

////получаем данные от сервера
//hubConnection.on("Send", function (data) {  // (data) - данные которые отправляются из класса ChatHub.cs метода Send
//    let elem = document.createElement("p");
//    elem.appendChild(document.createTextNode(data));
//    let firstElem = document.getElementById("chatroom").firstChild;
//    document.getElementById("chatroom").insertBefore(elem, firstElem);

//});

////обработчик события нажатия кнопки
//document.getElementById("sendBtn").addEventListener("click", function (e) {
//    let message = document.getElementById("message").value; // получаем элемент и его значение 
//    hubConnection.invoke("Send", message); // отправка сообщения
//});

//hubConnection.start();