using System.Collections.Concurrent;
using System.Diagnostics;
using Nito.AsyncEx;

namespace BulkInsertDemo.Persistence;

public abstract class StockUpdateHandlerBase : IStockUpdateHandler
{
    private static readonly AsyncLock mutex = new();
    private static readonly ConcurrentQueue<UpdatePackage> queue = new();
    private static bool _running;

    public Task PersistStockUpdate(UpdatePackage package)
    {
        queue.Enqueue(package);
        return Task.Run(async () =>
        {
            try
            {
                await StartPersisting();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start persisting stock updates: {ex.Message}");
            }
        });
    }

    protected abstract Task Persist(UpdatePackage package);

    protected static IEnumerable<StockRow> GetRows(UpdatePackage package)
    {
        return package.Stocks.Select(s => new StockRow
        {
            ProductCode = s.ProductCode,
            Stock = s.Stock,
            StoreNo = package.StoreNo,
            Timestamp = package.Timestamp
        }).OrderBy(r => r.Timestamp);
    }

    private async Task StartPersisting()
    {
        using (await mutex.LockAsync())
        {
            if (_running)
            {
                return;
            }

            _running = true;
        }

        while (queue.TryDequeue(out var package))
        {
            var sw = Stopwatch.StartNew();
            await Persist(package);
            var elapsed = sw.Elapsed;
            var rem = queue.Count;
#pragma warning disable 4014
            Task.Run(() =>
            {
                Console.WriteLine($"Took {elapsed} to persist update package, {rem} packages remaining in queue");
            });
#pragma warning restore 4014
        }

        using (await mutex.LockAsync())
        {
            _running = false;
        }
    }
}