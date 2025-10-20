using Microsoft.AspNetCore.SignalR;
using ChatServer.Core;
using System.Diagnostics;

namespace ChatServer.MyHubs;



public class TitleHub: Hub
{
    // SetUp
    private readonly IHubContext<TitleHub> context;
    public TitleHub(IHubContext<TitleHub> context)
    {
        this.context = context;
    }
    private void SetUp()
    {
        // set ActionBuilder
        TitleBox.Builder = new TitleBox.ActionBuilder
        {
            Template = (conId, newTitle
            ) =>
            {
                var clientProxy = context.Clients.Client(conId);

                clientProxy.SendAsync("ReceiveTitle", newTitle);
            }
        };
    }

    // interface
    public void Register()
    {
        SetUp();

        var myConnectionId = Context.ConnectionId;

        TitleBox.Subscribe(myConnectionId);
    }

    public void ModifyTitle(string newTitle)
    {
        SetUp();

        // 제목 수정
        TitleBox.SetTitle(newTitle);

        // 전파
        TitleBox.NotifyTitleChanged();
    }
}

