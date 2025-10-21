using Serilog;
using Serilog.Core;
using TitleConsole.Flow;
using TitleConsole.Value;
using ILogger = Serilog.ILogger;

namespace TitleConsole.Object;


// Object
public class TitleClient
{
    // core
    private readonly ILogger _logger;
    public TitleClient()
    {
        this._logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        _logger.ForContext<TitleClient>();
        
        _logger.Information("TitleClient 생성됨");
    }
    
    
    // state
    public ClientId Id { get; } = ClientId.New(); 
    public string Title { get; set; } = string.Empty;
    
    
    // action
    public async Task Subscribe()
    {
        // caputure
        var myId = Id;
        
        // compute
        await TitleServerFlow.Subscribe(myId, (newTitle =>
        {
            this.Title = newTitle;
            Console.WriteLine($"새로운 Title로 업데이트되었습니다 {newTitle}");
        }));
        
    }

    public async Task PushTitle()
    {
        // capture
        var newTitle = Title;
        
        // compute
        await TitleServerFlow.PushTitle(newTitle);
    }
}