namespace Contracts.Events
{
    public class CreateGame : Event
    {
        public override string Type => "CreateGame";

        public string ClientId { get; set; }
    }

    public class AddPlayer : Event
    {
        public override string Type => "AddPlayer";

        public string GameId { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }
    }

    public class StartGame : Event
    {
        public override string Type => "StartGame";

        public string GameId { get; set; }
    }

    public class GiveAnswer : Event
    {
        public override string Type => "GiveAnswer";

        public string PlayerId { get; set; }

        public string GameId { get; set; }

        public int AnswerId { get; set; }


    }
}