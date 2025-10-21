using Microsoft.AspNetCore.Mvc;
using TitleServer.Object;
using TitleServer.UserInterface;
using TitleServer.Value;

var builder = WebApplication.CreateBuilder(args);

// TitleServer
builder.AddTitleServerHubServices();
builder.AddTitleServerServices();


var app = builder.Build();

// TitleServer

app.MapGet("/", () => "Hello World!");
        
        
app.MapPost("/title-server/updateTitle", async ([FromServices] TitleServer.Object.TitleServer titleServer ,[FromBody] NewTitleMessage message) =>
{
    Console.WriteLine("요청이 도착했습니다.");
    titleServer.Title = message.Content;
            
    await titleServer.NotifyTitleChangedAsync();
            
    return Results.Ok();
});

app.RegisterTitleServerHub();


app.Run();
