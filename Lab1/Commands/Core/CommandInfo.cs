namespace Lab1.Commands.Core;

internal readonly ref struct CommandInfo
{
    public readonly string CommandName;
    public readonly string CommandDescription;

    public CommandInfo(string name, string description)
        => (CommandName, CommandDescription) = (name, description);

    public override string ToString() => $"Name: {CommandName}.\nDescription: {CommandDescription}";
}
