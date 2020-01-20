using System.Threading.Tasks;

namespace BulkInsertDemo.Persistence
{
    public sealed class RegularInserter : StockUpdateHandlerBase
    {
        // this is a little rough but should suffice for the example
        private readonly BulkContext _context;

        public RegularInserter(BulkContext context)
        {
            this._context = context;
        }

        protected override async Task Persist(UpdatePackage package)
        {
            await using var transaction = await this._context.Database.BeginTransactionAsync();
            foreach (var stockRow in GetRows(package))
            {
                await this._context.Stocks.AddAsync(stockRow);
            }

            await this._context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}