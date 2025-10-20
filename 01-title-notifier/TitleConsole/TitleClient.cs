using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace TitleConsole;


// Object
public class TitleClient
{
    // core
    public TitleClient()
    {
        this._logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        _logger.ForContext<TitleClient>();

        TitleClientManager.Register(this);

        _logger.Information("TitleClient 생성 (ClientId: {ClientId})", Id.RawValue);

    }

    // state
    public ID Id { get; } = ID.Random();
    private readonly ILogger _logger;

    public string Title { get; private set; } = "";
    public string TitleInput { get; set; } = "";

    public List<string> Notifications { get; private set; } = [];

    private HubConnection? Connection { get; set; } = null;

    // acton
    public async Task Subscribe()
    {
        // capture
        const string hubUrl = "https://localhost:7297/title";
        if (this.Connection is not null)
        {
            _logger.Warning("이미 연결된 Connection이 존재합니다. (ClientId: {ClientId})", Id.RawValue);
            return;
        }

        // mutate - SignalR을 통해 https://localhost:7297/title 로부터 connection을 생성한다.
        try
        {
            this.Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7297/title")
                .Build();

            Connection.On<string>("UpdateTitle", newTitle =>
            {
                _logger.Information("UpdateTitle 이벤트 수신: {NewTitle} (ClientId: {ClientId})",
                                   newTitle, Id.RawValue);
                this.Title = newTitle;
                _logger.Debug("Title 최신화 완료: {Title} (ClientId: {ClientId})", Title, Id.RawValue);
            });


            await Connection.StartAsync();
            _logger.Information("서버와 연결 성공 (Hub: {HubUrl}, ClientId: {ClientId})",
                                 hubUrl, Id.RawValue);


            await Connection.InvokeAsync("Subscribe");
            _logger.Information("서버에 Subscribe 호출 완료 (ClientId: {ClientId})", Id.RawValue);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Subscribe 중 예외 발생 (ClientId: {ClientId})", Id.RawValue);
        }
    }

    public async Task PushTitle()
    {
        // capture
        if (Connection is not HubConnection connection)
        {
            _logger.Warning("Connection이 null이어서 종료됩니다. (ClientId: {ClientId})", Id.RawValue);
            return;
        }
        if (string.IsNullOrEmpty(TitleInput))
        {
            _logger.Warning("TitleInput이 비어 있어 취소됩니다. (ClientId: {ClientId})", Id.RawValue);
        }
        var newTitle = this.TitleInput;


        // compute - SignalR 연결을 통해 PushTitle 플로우를 실행한다.
        try
        {
            await connection.InvokeAsync("PushTitle", newTitle);

            _logger.Information("서버에 PushTitle 호출 완료: {NewTitle} (ClientId: {ClientId})",
                                newTitle, Id.RawValue);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "PushTitle 중 예외 (ClientId: {ClientId}, NewTitle: {NewTitle})",
                          Id.RawValue, newTitle);
        }
    }


    // value
    public readonly record struct ID
    {
        // core
        public required Guid RawValue {  get; init; }

        public static ID Random() => new ID { RawValue = Guid.NewGuid() };
    }
}


// ObjectManager
internal static class TitleClientManager
{
    internal static ConcurrentDictionary<TitleClient.ID, TitleClient> Container = [];
    internal static void Register(TitleClient obj)
    {
        Container.TryAdd(obj.Id, obj);
    }
}