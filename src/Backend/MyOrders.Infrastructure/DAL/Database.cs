namespace MyOrders.Infrastructure.DAL
{
    internal class Database
    {
        public DatabaseKind DatabaseKind { get; set; }
    }

    internal enum DatabaseKind
    {
        MySql, EFCoreInMemory, InMemory
    }
}
