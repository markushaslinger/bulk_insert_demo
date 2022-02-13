namespace BulkInsertDemo.Persistence;

public interface IStockUpdateHandler
{
    Task PersistStockUpdate(UpdatePackage package);
}