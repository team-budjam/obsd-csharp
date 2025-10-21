using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TitleServer.Value;

namespace TitleServer.UserInterface;


// UserInterface
public class TitleServerHub(Object.TitleServer titleServer) : Hub
{
    // SignalR 엔드포인트
    // https://localhost:9191/title-server-hub
    public Task Subscribe(ClientId  clientId)
    {
        Console.WriteLine("Subscribe 요청이 왔습니다.");
        titleServer.Subscribers[clientId] = Context.ConnectionId;

        return Task.CompletedTask;
    }
}


// Extensions
public static class TitleServerHubExtensions
{
    public static WebApplicationBuilder AddTitleServerHubServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        
        return builder;
    }

    public static WebApplication RegisterTitleServerHub(this WebApplication app)
    {
        app.MapHub<TitleServerHub>("/title-server-hub");
        
        return app;
    }
}
