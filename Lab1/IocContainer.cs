namespace Lab1;

internal class IocContainer
{
    private static Dictionary<Type, object> _items;

    static IocContainer()
    { 
        _items = new Dictionary<Type, object>();
    }

    public static void AddService<T>(T item) => _items.Add(typeof(T), item);

    public static T GetService<T>() 
        => (T)_items[typeof(T)];
}
