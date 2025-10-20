using Microsoft.AspNetCore.SignalR;
using ChatServer.MyHubs;
using System.Diagnostics;

namespace ChatServer.Core;

public class TitleBox
{
    // state
    public static string Title = "";
    public static void SetTitle(string newTitle)
    {
        Console.WriteLine($"{Title} -> {newTitle} 수정됩니다.");
        Title = newTitle;
    }

    public static List<string> subscribers = [];
    public static void Subscribe(string connectionId) => subscribers.Add(connectionId);

    public static ActionBuilder? Builder { get; set; } = null;


    // action
    public static void NotifyTitleChanged()
    {
        if (Builder is not ActionBuilder builder) return;
        var newTitle = Title;

        subscribers.ForEach(subscriber =>
        {
            var handler = builder.Build(subscriber);

            handler(newTitle);
        });
    }


    // value
    public readonly record struct ActionBuilder
    {
        public required Action<string, string> Template { get; init; } // connectionId, Message

        // operator
        public Action<string> Build(string connectionId)
        {
            var template = this.Template; 

            return newTitle =>
            {
                template(connectionId, newTitle);
            };
        }
    }
}


