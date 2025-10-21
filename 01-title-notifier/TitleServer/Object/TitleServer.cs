using TitleWeb.UserInterface;

namespace TitleWeb.Object;



// Object
public class TitleServer(TitleHub.TitleEventHandler eventHandler)
{
    // core
    private readonly TitleHub.TitleEventHandler _eventHandler = eventHandler;


    // state
    private string _title = "";

    public string GetTitle() => _title;
    public void SetTitle(string newTitle)
    {
        _title = newTitle;
        Console.WriteLine($"{_title}에서 {newTitle}로 수정되었습니다.");
    }
    
    private List<string> Subscribers { get; set; } = [];
    public int SubscribersCount() =>  Subscribers.Count;
    public void RegisterSubscriber(string connectionId)
    {
        Subscribers.Add(connectionId);
        Console.WriteLine($"{connectionId} 구독자가 추가되었습니다.");
    }

    
    // action
    public void NotifyTitleChanged()
    {
        // capture
        var subscribers = Subscribers;
        var newTitle = _title;


        // compute
        subscribers.ForEach(clientId =>
        {
            _eventHandler.Execute(newTitle, clientId);
        });
    }
}


// Extensions
public static class TitleServerExtensions
{
    public static WebApplicationBuilder AddTitleServerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<TitleServer>();
        builder.Services.AddSingleton<TitleHub.TitleEventHandler>();

        return builder;
    }
}
