namespace Lab1;

internal sealed class TypeUnit
{
    public readonly string BaseName;
    public readonly int AssociatedIndex;

    public TypeUnit(int index, string baseName)
        => (AssociatedIndex, BaseName) = (index, baseName);
}
