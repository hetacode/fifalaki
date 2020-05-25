using System;
using System.Linq;
using System.Text.Json;
using Arch;
using Arch.Bus;
using Contracts.Events;
using GamesList.Services;
using Microsoft.Extensions.Configuration;

namespace GamesList
{
    public class EventsConsumer
    {
        private readonly ServerState _state;

        public EventsConsumer(IConfiguration config, ServerState state)
        {
            _state = state;
            var gameConsumerBus = new RabbitMQBus(config["RabbitServer"], "games-list", "game-ex");
            gameConsumerBus.Consumer(GameEventsConsumer);
        }

        private void GameEventsConsumer((string eventType, string body) data)
        {
            var eventType = EventRecognizer.GetEventBy(data.eventType);
            var e = JsonSerializer.Deserialize(data.body, eventType);

            switch (e)
            {
                case CreateGame createGame:
                    var selectedGame = _state.Games.FirstOrDefault(a => a.Id == createGame.ClientId);
                    if (!(selectedGame is null))
                    {
                        _state.Games.Remove(selectedGame);
                    }
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