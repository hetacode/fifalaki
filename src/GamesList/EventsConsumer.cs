using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Arch;
using Arch.Bus;
using Contracts.Events;
using GamesList.Services;
using Microsoft.Extensions.Configuration;
using ProtoContracts;

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

            Console.WriteLine(data.body);

            switch (e)
            {
                case CreateGame createGame:
                    var selectedGame = _state.Games.FirstOrDefault(a => a.item.Id == createGame.ClientId);
                    if (!(selectedGame == default((GameItem, List<string>))))
                    {
                        _state.Games.Remove(selectedGame);
                    }
                    _state.Games.Add((new ProtoContracts.GameItem { Id = createGame.ClientId }, new List<string> { createGame.ClientId }));
                    break;
                case EndGame endGame:
                    _state.Games.Remove(_state.Games.FirstOrDefault(f => f.item.Id == endGame.GameId));
                    break;
                case AddPlayer addPlayer:
                    var game = _state.Games.FirstOrDefault(f => f.item.Id == addPlayer.GameId);
                    game.item.PlayersCount++;
                    game.players.Add(addPlayer.Id);
                    break;
                case RemovePlayer removePlayer:
                    var gameByRemovePlayer = _state.Games.FirstOrDefault(f => f.players.Any(a => a == removePlayer.Id));
                    gameByRemovePlayer.item.PlayersCount--;

                    if (gameByRemovePlayer.item.PlayersCount <= 0 || gameByRemovePlayer.item.Id == removePlayer.Id)
                    {
                        _state.Games.Remove(gameByRemovePlayer);
                    }
                    gameByRemovePlayer.players.Remove(removePlayer.Id);
                    break;
            }
        }
    }
}