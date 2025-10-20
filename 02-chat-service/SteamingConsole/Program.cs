using Microsoft.AspNetCore.SignalR.Client;
using ChatCommon;

// Main
Console.Write("Enter a stock (press Enter for MSFT): ");
string? stock = Console.ReadLine();

if (string.IsNullOrWhiteSpace(stock))
{
    stock = "MSFT";
}


// connection 연결
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7185/stockprice")
    .Build();

await connection.StartAsync();


// 전송
try
{
    var cts = new CancellationTokenSource();

    var stockPrices = connection.StreamAsync<StockPrice>("GetStockPriceUpdates", stock, cts.Token);

    await foreach (var stockPrice in stockPrices)
    {
        Console.WriteLine($"{stockPrice.Stock} is now {stockPrice.Price:C}");

        Console.Write("Do you want to cancel(y/n)?");
        var key = Console.ReadKey().Key;
        if (key == ConsoleKey.Y) cts.Cancel();
        Console.WriteLine();
    }
}
catch(Exception ex)
{
    Console.WriteLine($"{ex.GetType()} says {ex.Message}");
}
Console.WriteLine();

Console.WriteLine("Streaming download completed");

await connection.SendAsync("UploadStocks", GetStocksAsync());

Console.WriteLine("Uploading stocks to service... (press ENTER to stop)");
Console.ReadLine();

Console.WriteLine("Ending console app.");



// GetStocksAsync()
async IAsyncEnumerable<string> GetStocksAsync()
{
    for (int i = 0; i < 10; i++)
    {
        // Return a random four-letter stock code.
        yield return $"{AtoZ()}{AtoZ()}{AtoZ()}{AtoZ()}";

        await Task.Delay(TimeSpan.FromSeconds(3));
    }
}


string AtoZ()
{
    return char.ConvertFromUtf32(Random.Shared.Next(65, 91));
}