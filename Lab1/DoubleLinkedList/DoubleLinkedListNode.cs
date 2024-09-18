namespace DoubleLinkedList;

public sealed class DoubleLinkedListNode<T>
{
    private T _data;
    public DoubleLinkedListNode<T> Next;
    public DoubleLinkedListNode<T> Previous;

    public DoubleLinkedListNode(T data) =>
        (Next, Previous, _data) = (null!, null!, data);
    
    
    public static implicit operator T (DoubleLinkedListNode<T> node)
    {
        return node is null ? default : node._data;
    }

    public override string ToString()
    {
        return _data?.ToString() ?? string.Empty;
    }

    public override bool Equals(object obj)
    {
        if (obj is DoubleLinkedListNode<T> node)
        {
            return node!._data!.Equals(_data);
        }

        return false;
    }

    public bool Equals(T data)
    {
        if (data is null)
        {
            return false;
        }
        
        return _data!.Equals(data);
    }

    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }
}