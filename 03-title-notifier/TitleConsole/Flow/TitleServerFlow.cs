using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Java;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;
using TitleConsole.Value;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TitleConsole.Flow;

public static class TitleServerFlow
{
    private static HubConnection? Connection { get; set; } = null;
    private static List<Action<string>> EventHandlers { get; set; } = new List<Action<string>>();
    private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
    {
        // ⚠️ 개발 환경에서만 잠깐 사용하세요. 프로덕션에서는 절대 금지!
        // ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    })
    {
        BaseAddress = new Uri("https://localhost:9191"),
        DefaultRequestVersion = HttpVersion.Version11,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
    };
    
    public static async Task Subscribe(ClientId clientId, Action<string> eventHandler)
    {
        // 이벤트 핸들러 추가
        EventHandlers.Add(eventHandler);

        Console.WriteLine($"EventHandler 수: {EventHandlers.Count}");
        
        if (Connection is null)
        {
            try
            {
                Connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:9191/title-server-hub")
                    .Build();

                Connection.Closed += async (error) =>
                {
                    Console.WriteLine($"[HUB CLOSED] {error}");
                    await Task.CompletedTask;
                };

                Connection.On<string>("TitleChanged", newTitle =>
                {
                    Console.WriteLine("TitleChanged 이벤트가 도착했습니다.");
                    foreach (var handler in EventHandlers)
                    {
                        handler(newTitle);
                    }
                });
                
                await Connection.StartAsync();

                Console.WriteLine("서버와 연결 성공");
                
                await Connection.InvokeAsync("Subscribe", clientId);
            }  
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public static async Task PushTitle(string newTitle)
    {
        try
        {
            var message = new NewTitleMessage(newTitle);
            Console.WriteLine($"[HTTP] POST {_httpClient.BaseAddress}/title-server/updateTitle  body={{ content: \"{newTitle}\" }}");

            var response = await _httpClient.PostAsJsonAsync("/title-server/updateTitle", message);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[HTTP 200] {body}");
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[HTTP {(int)response.StatusCode}] {response.StatusCode} {body}");
            }
        }
        catch (HttpRequestException hre)
        {
            Console.WriteLine($"[HTTP ERROR] {hre.Message}");
            if (hre.InnerException is IOException ioe)
            {
                Console.WriteLine($"[HTTP IOEXCEPTION] {ioe.Message}");
            }
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex}");
            throw;
        }
    }
}