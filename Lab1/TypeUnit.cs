namespace Lab1;


internal readonly struct TypeUnit
{
    public readonly string BaseName;
    public readonly int AssociatedIndex;
    public readonly Type Type;

    public TypeUnit(int index, string baseName, Type type)
        => (AssociatedIndex, BaseName, Type) = (index, baseName, type);

    public override bool Equals(object obj)
    {
        bool result = false;

        if(obj is TypeUnit typeUnit)
        {
            result = Type == typeUnit.Type
                && AssociatedIndex == typeUnit.AssociatedIndex;
        }

        return result;
    }

    public override int GetHashCode() 
        => AssociatedIndex;
}
