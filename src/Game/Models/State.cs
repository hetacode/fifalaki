using System.Collections.Generic;

namespace Game.Models
{
    public class State
    {
        public string GameMasterId { get; set; }

        public List<Player> Players { get; set; }

        public int Level { get; set; }

        public int LevelTime { get; set; }

        // Should be update each second
        public int CurrentTime { get; set; }

        // Count of lost games
        public int GameAttempts { get; set; }

        public EnumGameState CurrentState { get; set; }

        public LevelData LevelData { get; set; }

        public int TotalStateTime()
            => CurrentState switch
            {
                EnumGameState.Level => 10,
                EnumGameState.Summary => 5,
                EnumGameState.WaitingForNextLevel => 5,
                EnumGameState.WaitingForPlayers => -1,
                _ => -1
            };
    }

    public class LevelData {
        public int CorrectWordIndex { get; set; }    

        public List<string> Words { get; set; }
        public string WinnerId { get; set; }
        public string LooserId { get; set; }
    }

    public enum EnumGameState
    {
        WaitingForPlayers,
        WaitingForNextLevel,
        Level,
        Summary,
        End
    }
}