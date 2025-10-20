using Microsoft.AspNetCore.SignalR;
using ChatCommon;

namespace ChatServer.MyHubs;


public class ChatHub: Hub
{
    private static Dictionary<string, UserModel> Users = new();

    public async Task Register(UserModel newUser)
    {
        UserModel user;
        string action = "registered as a new user";

        // Try to get a stored user with a match on new user
        if (ChatHub.Users.ContainsKey(newUser.Name))
        {
            user = Users[newUser.Name];

            // Remove any existing grup registerations
            if (user.Groups is not null)
            {
                foreach (var group in user.Groups.Split(','))
                {
                    await Groups.RemoveFromGroupAsync(user.ConnectionId, group);
                }
            }
            user.Groups = newUser.Groups;

            // ConnectionId might have changed if the browser refreshed so update it
            user.ConnectionId = Context.ConnectionId;

            action = "updated yout registered user";
        }
        else
        {
            if (string.IsNullOrEmpty(newUser.Name))
            {
                // Assign a GUID for name if they are anonymous.
                newUser.Name = Guid.NewGuid().ToString();
            }

            newUser.ConnectionId = Context.ConnectionId;

            Users.Add(key: newUser.Name, value: newUser);
            user = newUser;
        }


        if (user.Groups is not null)
        {
            // A user does not have to belong to any groups
            // but if they do, register them with the hub

            foreach(string group in user.Groups.Split(","))
            {
                await Groups.AddToGroupAsync(user.ConnectionId, group);
            }

            // Send a message to the registering user informing of success.
            var message = new MessageModel
            {
                From = "SignalR Hub",
                To = newUser.Name,
                Body = $"you have successfully {action} with connection ID {user.ConnectionId}"
            };

            // get Client from this.Clients
            var proxy = this.Clients.Client(user.ConnectionId);
            await proxy.SendAsync("ReceiveMessage", message);
        }
    }

    public async Task SendMessage(MessageModel message)
    {
        // proxy
        IClientProxy proxy;

        if (string.IsNullOrEmpty(message.To))
        {
            message.To = "Everyone";
            proxy = Clients.All;
            await proxy.SendAsync("ReceiveMessage", message);
            return;
        }

        // message.To로부터 users, groups 추출
        string[] userAndGroupList = message.To.Split(',');

        // user 또는 group에게 메시지 전송
        foreach (string userOrGroup in userAndGroupList)
        {
            if (Users.ContainsKey(userOrGroup))
            {
                // If the item is in Users then send the message to that user
                // by looking up their connection ID in the dictionary
                message.To = $"User: {Users[userOrGroup].Name}";
                proxy = Clients.Client(Users[userOrGroup].ConnectionId);
            }
            else
            {
                // 만약 특정 group으로 메시지가 보내졌을 때.
                message.To = $"Group: {userOrGroup}";
                proxy = Clients.Group(userOrGroup);
            }

            await proxy.SendAsync("ReceiveMessage", message);
        }
    }
}