using System;
using System.Linq;
using System.Text.Json;
using Arch;
using Arch.Bus;
using Contracts.Events;
using GamesList.Services;

namespace GamesList
{
    public class EventsConsumer
    {
        private readonly ServerState _state;

        public EventsConsumer(ServerState state)
        {
            _state = state;
            var gameConsumerBus = new RabbitMQBus("amqp://guest:guest@localhost:5672", "games-list", "game-ex");
            gameConsumerBus.Consumer(GameEventsConsumer);
        }

        private void GameEventsConsumer((string eventType, string body) data)
        {
            var eventType = EventRecognizer.GetEventBy(data.eventType);
            var e = JsonSerializer.Deserialize(data.body, eventType);

            switch (e)
            {
                case CreateGame createGame:
                    _state.Games.Add(new ProtoContracts.GameItem { Id = createGame.ClientId });
                    break;
                case EndGame endGame:
                    _state.Games.Remove(_state.Games.FirstOrDefault(f => f.Id == endGame.GameId));
                    break;
                case AddPlayer addPlayer:
                    var game = _state.Games.FirstOrDefault(f => f.Id == addPlayer.GameId);
                    game.PlayersCount++;
                    break;
            }
        }
    }
}