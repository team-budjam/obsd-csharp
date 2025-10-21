using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TitleServer.UserInterface;
using TitleServer.Value;

namespace TitleServer.Object;



// Object
public class TitleServer(IHubContext<TitleServerHub> hub)
{
    // core
    private readonly IHubContext<TitleServerHub>  _hub = hub;

    
    // state
    public string Title { get; set; } = string.Empty;

    public ConcurrentDictionary<ClientId, string> Subscribers { get; } = new();
    

    // action
    public async Task NotifyTitleChangedAsync()
    {
        // capture
        var subscribers = Subscribers;
        var newTitle = Title;
        
        // compute
        var connectionIds = subscribers.Values.ToArray();

        if (connectionIds.Length == 0)
        {
            Console.WriteLine("TitleChanged 이벤트를 받을 구독자가 없습니다.");
            return;
        }

        await _hub.Clients.Clients(connectionIds).SendAsync("TitleChanged", newTitle);

        Console.WriteLine($"TitleChanged 이벤트가 전달되었습니다. {connectionIds.Length}");
    }
}


// Extensions
public static class TitleServerExtensions
{
    public static WebApplicationBuilder AddTitleServerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<TitleServer>();
        
        return builder;
    }
}
