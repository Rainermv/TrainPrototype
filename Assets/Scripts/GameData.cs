public struct GameData
{
    public static GameData InitGameData()
    {
        return new GameData()
        {
            Gold = 0,
            Level = 0,
            GameState = GameState.Station
        };
    }
    public int Gold { get; set; }
    public int Level { get; set; }
    public GameState GameState { get; set; }


    

}