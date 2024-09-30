namespace Lab1.Database.Context;

internal static class DIExtensions
{
    public static void ConfigureDbContext()
    {
        IAsyncDbContext dbContext = new DbContext();
        IocContainer.AddService(dbContext);
    }
}