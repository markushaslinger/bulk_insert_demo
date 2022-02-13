namespace BulkInsertDemo;

public record UpdatePackage(int StoreNo, DateTime Timestamp, IReadOnlyCollection<ProductStock> Stocks);