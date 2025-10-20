global using TitleWeb.Infra;
using Microsoft.AspNetCore.SignalR;
using TitleWeb.Object;
using TitleWeb.UserInterface;

namespace TitleWeb;


// EntryPoint
public static class Program
{
    public static void Main(string[] args)
    {
        // builder
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();
        builder.AddTitleServerServices();

        // app
        var app = builder.Build();
        app.MapGet("/", () => "Hello World!");
        app.MapHub<TitleHub>("/title");
        
        
        // run
        app.Run();
    }
}

