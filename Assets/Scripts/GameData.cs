using System.Collections.Generic;

public struct GameData
{
    public static GameData InitGameData()
    {
        return new GameData()
        {
            Gold = 0,
            Level = 0,
            GameState = GameState.Station,
            PlayerWagons = new List<int>(){0,1}
        };
    }

    public List<int> PlayerWagons { get; set; }

    public int Gold { get; set; }
    public int Level { get; set; }
    public GameState GameState { get; set; }
}