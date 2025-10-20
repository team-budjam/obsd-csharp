using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using ChatCommon;

namespace ChatServer.MyHubs
{
    public class StockPriceHub: Hub
    {
        // 데이터를 다운받는 함수
        public async IAsyncEnumerable<StockPrice> GetStockPriceUpdates(string stock, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // 데이터 정의
            double currentPrice = 267.10;

            for (int i = 0; i < 10; i++)
            {
                // Check the cancellation token regularly so that the server will stop
                // producing items if the client disconnects.
                cancellationToken.ThrowIfCancellationRequested();

                // Increment or decrement the current price vy a random amount.
                // The compiler does not need the extra parentheses but it
                // is clearer for humans if you put them in.
                currentPrice += (Random.Shared.NextDouble() * 10.0) - 5.0;

                var stockPrice = new StockPrice(stock, currentPrice);
                Console.WriteLine($"[{DateTime.UtcNow}] {stockPrice.Stock} at {stockPrice.Price.ToString("F2")}");

                yield return stockPrice;

                await Task.Delay(4000, cancellationToken);
            }
        }

        // 데이터를 업로드하는 함수
        public async Task UploadStocks(IAsyncEnumerable<string> stocks)
        {
            await foreach(string stock in stocks)
            {
                Console.WriteLine($"Receving {stock} from client...");
            }
        }
    }
}
