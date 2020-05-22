using System.Collections.Generic;

namespace Contracts.Events
{
    public class UpdatedPlayers : FromGameEvent
    {
        public override string Type => "UpdatedPlayers";

        public List<PlayerData> Players { get; set; }
    }

    public class WaitingForNextLevel : FromGameEvent
    {
        public override string Type => "WaitingForNextLevel";

        public int StateTime { get; set; }
    }

    public class NewLevel : FromGameEvent
    {
        public override string Type => "NewLevel";

        public List<Answer> Answers { get; set; }
    }

    public class SummaryLevel : FromGameEvent
    {
        public override string Type => "SummaryLevel";

        // id - number
        public Dictionary<string, int> Points { get; set; }

        public int Attempts { get; set; }

        // can be empty
        public string WinnerId { get; set; }

        // can be empty
        public string LooserId { get; set; }
    }

    public class EndGame : FromGameEvent
    {
        public override string Type => "EndGame";

        public Dictionary<string, int> Points { get; set; }

        public string WinnerId { get; set; }
    }

    public class PlayerData {
        public string Id { get; set; }

        public string Name { get; set; }
    }
    public class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }

    public class FromGameEvent : Event
    {
        public string GameId { get; set; }

        public List<string> PlayersIds { get; set; }
    }
}