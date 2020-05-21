using System.Collections.Generic;

namespace Contracts.Events
{
    public class WaitingForNextLevel : Event
    {
        public override string Type => "StartGameEvent";

        public int StateTime { get; set; }
    }

    public class NewLevel : Event
    {
        public override string Type => "NewLevel";

        public List<Answer> Answers { get; set; }
    }

    public class SummaryLevel : Event
    {
        public override string Type => "SummaryLevel";

        // id - number
        public Dictionary<string, int> Points { get; set; }

        public int Attempts { get; set; }

        // can be empty
        public string WinnerId { get; set; }
    }

    public class EndGame : Event
    {
        public override string Type => "EndGame";

        public Dictionary<string, int> Points { get; set; }

        public string WinnerId { get; set; }
    }

    public class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}