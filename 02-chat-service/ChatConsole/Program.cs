using Microsoft.AspNetCore.SignalR.Client;
using ChatCommon;


// username 입력 받기
Console.Write("Enter a username (required): ");
string? username = Console.ReadLine();

if (string.IsNullOrEmpty(username))
{
    Console.WriteLine("You must enter a username to register with chat!");
    return;
}

// group 입력 받기
Console.Write("Enter your groups (optional): ");
string? groups = Console.ReadLine();


// connection 생성
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7185/chat")
    .Build();


// 메시지 수신
connection.On<MessageModel>("ReceiveMessage", message =>
{
    var text = $"To: {message.To}, From: {message.From}: {message.Body}";
    Console.WriteLine(text);
});

// connection 연결
await connection.StartAsync();
Console.WriteLine("Successfully started");


// User 등록
var registration = new UserModel
{
    Name = username,
    Groups = groups
};

await connection.InvokeAsync("Register", registration);

Console.WriteLine("Successfully registered");

// 대기
Console.WriteLine("Listening... (press ENTER to stop.)");
Console.ReadLine(); // 종료하지 않고 대기, 아무키나 누르면 종료