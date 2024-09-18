namespace DoubleLinkedList;

public sealed class DoubleLinkedList<T>
{
    private DoubleLinkedListNode<T> _tail = null;
    private DoubleLinkedListNode<T> _head = null;
    public int Capacity { get; private set; }
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Capacity)
            {
                throw new IndexOutOfRangeException();
            }
            
            var val = TakeElementByInd(index);
            
            if (val is null)
            {
                throw new NullReferenceException();
            }
            

            return val;
        }
    }
    private bool InitRoot(T data)
    {
        if (_head is null)
        {
            var temp = new DoubleLinkedListNode<T>(data);
            _head = temp;
            _tail = temp;
            
            Capacity = 1;
            
            return true;
        }
        return false;
    }

    public static implicit operator DoubleLinkedList<T>(T[] values)
    {
        var result = new DoubleLinkedList<T>();

        foreach (var item in values)
        {
            result.AddToEnd(item);
        }

        return result;
    }
    
    public void AddToEndFromBegin(T data)
    {
        if (InitRoot(data))
        {
            return;
        }

        Capacity++;
        var temp = new DoubleLinkedListNode<T>(data);
        var lastNode = _head; //a pointer (copy of a reference) to _root
            
        while (lastNode.Next is not null) // in C# i can't make here (lastNode is not null) condition
        {
            lastNode = lastNode.Next;
        }

        lastNode.Next = temp; // because here if i write (lastNode = temp)
                              // it will not affect _root because i use a COPY of a refernce to _root
                              // also i can't take value of _root by reference (ref var lastNode = ref root)
                              // because it will affect _root (in while loop)!
                              // My approach is working correctly because i only change the state of pointer but
                              // not the pointer exactly. If you comapre this approach to c++ one you can go crazy.
                              
        temp.Previous = lastNode;
        _tail = temp;
    }

    public void AddToBeginFromEnd(T data)
    {
        if (InitRoot(data))
        {
            return;
        }
        
        Capacity++;
        var temp = new DoubleLinkedListNode<T>(data);
        var lastNode = _tail;
            
        while (lastNode.Previous is not null)
        {
            lastNode = lastNode.Previous;
        }

        lastNode.Previous = temp;
        _head = lastNode.Previous;
        _head.Next = lastNode;
    }

    public void AddToBeg(T data)
    {
        if (InitRoot(data))
        {
            return;
        }
        
        Capacity++;
        var node = new DoubleLinkedListNode<T>(data);
        node.Next = _head!;
        _head!.Previous = node;

        if (_tail == _head)
        {
            _head = node;
            _tail = _head.Next;
        }
        else
        {
            _head = node;
        }
    }

    public void AddToEnd(T data)
    {
        if (InitRoot(data))
        {
            return;
        }
        
        Capacity++;
        var node = new DoubleLinkedListNode<T>(data);
        node.Previous = _tail!;
        
        if (_tail == _head)
        {
            _tail = node;
            _tail.Previous = _head!;
            _head!.Next = node;
            
        }
        else
        {
            _tail = node;
            _tail!.Previous!.Next = _tail;
        }
    }
    
    public IEnumerable<T> ReadFromTail()
    {
        var temp = _tail;

        while (temp is not null)
        {
            yield return temp;
            temp = temp.Previous;
        }

    }
    
    public IEnumerable<T> ReadFromHead()
    {
        var temp = _head;

        while (temp is not null)
        {
            yield return temp;
            temp = temp.Next;
        }
    }

    public T PeekHead() => _head;
    public T PeekTail() => _tail;
    public T PeekTailPrev() => _tail!.Previous;

    public T PopHead()
    {
        if (_head is null)
        {
            return default;
        }

        Capacity--;
        T value = _head!;
        _head = _head.Next;
        _head.Previous = null!;
        return value;
    }

    public T PopTail()
    {
        if (_tail is null)
        {
            return default;
        }

        Capacity--;
        T value = _tail!;
        _tail = _tail.Previous;
        _tail.Next = null!;
        return value;
    }

    public void RemoveFirstFromTail(T elem)
    {
        var temp = _tail;
        
        while (temp.Previous is not null && !temp.Equals(elem))
        {
            temp = temp.Previous;
        }

        if (temp.Previous is null)
        {
            PopHead();
        }
        else if (temp.Next is null)
        {
            PopTail();
        }
        else
        {
            temp.Previous.Next = temp.Next;
            temp.Next.Previous = temp.Previous;
            temp.Previous = null!;
            temp.Next = null!;
            temp = null!;
            Capacity--;
        }
    }

    public bool Contains(T data)
    {
        var lastNode = _head;
        
        if (lastNode is null)
        {
            return false;
        }
        
        if (lastNode.Next is null)
        {
            return lastNode.Equals(data);
        }
        
        while (lastNode.Next is not null)
        {
            if (lastNode.Equals(data))
            {
                return true;
            }
            lastNode = lastNode.Next;
        }

        return false;
    }

    public T TakeElementByInd(int ind)
    {
        if (ind < 0 || ind >= Capacity)
        {
            return default;
        }
        
        if (ind <= Capacity / 2 || Capacity / 2 == 0)
        {
            var temp = _head;
            var counter = 0;
            
            while (temp is not null && counter != ind)
            {
                temp = temp.Next;
                counter++;
            }

            return temp;
        }
        else
        {
            var temp = _tail;
            var counter = Capacity - 1;
            while (temp is not null && counter != ind)
            {
                temp = temp.Previous;
                counter--;
            }

            return temp;
        }
    }

    public void RemoveByInd(int ind)
    {
        if (ind < 0 || ind >= Capacity)
        {
            return;
        }
        
        DoubleLinkedListNode<T> temp;
        if (ind <= Capacity / 2 || Capacity / 2 == 0)
        {
            temp = _head!;
            var counter = 0;

            while (temp is not null && counter != ind)
            {
                temp = temp.Next;
                counter++;
            }
        }
        else
        {
            temp = _tail!;
            var counter = Capacity - 1;

            while (temp is not null && counter != ind)
            {
                temp = temp.Previous;
                counter--;
            }
        }

        if (temp.Previous is null) // temp is head
        {
            if (_head == _tail)
            {
                _head = null;
                _tail = null;
            }
            else
            {
                temp = _head.Next;
             
                if (_head.Next is not null)
                {
                    _head.Next.Previous = null!;
                }
                _head = null!;
                _head = temp;
            }
        }
        else if (temp.Next is null) // temp is tail
        {
            if (_head == _tail)
            {
                _head = null;
                _tail = null;
            }
            else
            {
                temp = _tail.Previous;
                if (_tail.Previous is not null)
                {
                    _tail.Previous.Next = null!;
                }
                _tail = null;
                _tail = temp;
            }
        }
        else  // remove middle
        {
            var previous = temp.Previous;
            var next = temp.Next;
            temp.Previous = null!;
            temp.Next = null!;
            temp = null;
            previous.Next = next;
            next.Previous = previous;
        }
        
        Capacity--;
    }

    public void Clear()
    {
        _head = _tail = null;
    }

    private DoubleLinkedListNode<T> TakeNodeByInd(int ind)
    {
        if (ind < 0 || ind >= Capacity)
        {
            return default;
        }
        
        if (ind <= Capacity / 2 || Capacity / 2 == 0)
        {
            var temp = _head;
            var counter = 0;
            
            while (temp is not null && counter != ind)
            {
                temp = temp.Next;
                counter++;
            }

            return temp;
        }
        else
        {
            var temp = _tail;
            var counter = Capacity - 1;
            while (temp is not null && counter != ind)
            {
                temp = temp.Previous;
                counter--;
            }

            return temp;
        }
    }
    
    public void Swap(int indA, int indB)
    {
        if (indA == indB 
            || indA < 0 
            || indB < 0 
            || indA >= Capacity 
            || indB >= Capacity)
        {
            return;
        }
        
        DoubleLinkedListNode<T> first = null, second = null;
        
        if (indA > indB)
        {
            (indA, indB) = (indB, indA);
        }
        
        first = TakeNodeByInd(indA);
        second = TakeNodeByInd(indB);
        

        var firstPrev = first?.Previous;
        var firstNext = first?.Next;


        if (first.Next != second && second.Previous != first) // if elements is not close
        {
            var tempFirstPrev = first?.Previous;
            var tempFirstNext = first?.Next;
            var tempSecondPrev = second?.Previous;
            var tempSecondNext = second?.Next;
            
            first.Previous = second.Previous;
            first.Next = second.Next;
            second.Previous = firstPrev;
            second.Next = firstNext;

            if (tempFirstPrev is not null)
            {
                tempFirstPrev.Next = second;
            }
            else
            {
                _head = second;
            }
            if (tempFirstNext is not null)
            {
                tempFirstNext.Previous = second;
            }
            if (tempSecondNext is not null)
            {
                tempSecondNext.Previous = first;
            }
            else
            {
                _tail = first;
            }
            if (tempSecondPrev is not null)
            {
                tempSecondPrev.Next = first;
            }
        }
        else if(first != _head)
        {
            first.Next = second.Next;
            if (second.Next != null)
            { 
                second.Next.Previous = first;
            }
            first.Previous = second;
            second.Previous = firstPrev;
            firstPrev.Next = second;
            second.Next = first;
            
            if (first.Next is null)
            {
                _tail = first;
            }
        }
        else
        {
            _head.Next = second.Next;
            second.Next.Previous = _head;
            _head.Previous = second;
            second.Previous = null!;
            second.Next = _head;

            _head = second;
        }
    }

    public int TakeInd(T data)
    {
        var temp = _head;
        int counter = 0;

        while (temp != null)
        {
            if (temp.Equals(data))
            {
                return counter;
            }
            counter++;
            temp = temp.Next;
        }

        return -1;
    }
}