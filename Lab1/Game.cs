namespace Lab1;

internal sealed class Game
{
    public readonly GameAccount FirstPlayer;
    public readonly GameAccount SecondPlayer;
    public readonly Guid Id;
    public readonly int Rating;
    private string _index = null;
    public string Index => _index ??= Id.ToString().Substring(0, 5);

    public Game(GameAccount firstPlayer, GameAccount secondPlaer, int rating) =>
        (FirstPlayer, SecondPlayer, Id, Rating) = (firstPlayer, secondPlaer, Guid.NewGuid(), rating);

    public string GetShortId() => Id.ToString().Substring(0, 5);
}
