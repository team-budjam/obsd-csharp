using Microsoft.AspNetCore.SignalR;
using TitleWeb.Object;

namespace TitleWeb.UserInterface;

// TitleHub는 MapGet, MapPost와 동일한 서버의 UI이다.
// https://localhost:7297/title connection Interface
public class TitleHub(TitleServer titleServer) : Hub
{
    public void Subscribe()
    {
        titleServer.RegisterSubscriber(Context.ConnectionId);
        
        Console.WriteLine($"현재 Subscribers 수: {titleServer.SubscribersCount}");
    }

    public class TitleEventHandler(IHubContext<TitleHub> hub)
    {
        private readonly IHubContext<TitleHub> _hubContext = hub;

        public void Execute(string newTitle, string connectionId)
        {
            // newTitle을 전달받아 처리하는 Handler
            var clientProxy = _hubContext.Clients.Client(connectionId);
            clientProxy.SendAsync("UpdateTitle", newTitle);
            
            Console.WriteLine($"{connectionId}에게 newTitle 이벤트가 전달되었습니다.");
        }
    }


    // PushTitle - Flow
    public void PushTitle(string newTitle)
    {
        // 하나의 Flow를 실행한다. 
        titleServer.SetTitle(newTitle);

        titleServer.NotifyTitleChanged();

        Console.WriteLine($"현재 Subscribers 수: {titleServer.SubscribersCount}");
    }
}



// Extensions

