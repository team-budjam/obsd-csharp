using Microsoft.AspNetCore.SignalR.Client;


// connection 생성
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7185/title")
    .Build();

// ReceiveTitle 정의
connection.On<string>("ReceiveTitle", newTitle =>
{
    var text = $"NewTitle: {newTitle}";
    Console.WriteLine(text);
});

await connection.StartAsync();
Console.WriteLine("Successfully started");


// Register
await connection.InvokeAsync("Register");
Console.WriteLine("Successfully registered");


// User 등록
while (true)
{
    Console.WriteLine("Enter new Title -> ");
    string? titleInput = Console.ReadLine();

    await connection.InvokeAsync("ModifyTitle", titleInput);
}