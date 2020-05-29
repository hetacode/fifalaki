using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Arch;
using Arch.Bus;
using Contracts.Events;
using Game.Models;

namespace Game
{
    public class GamesManager
    {
        private Dictionary<string, GameRunner> _games = new Dictionary<string, GameRunner>();
        private readonly IBus _rtmPublisherBus;

        public GamesManager(IBus consumerBus, IBus rtmPublisherBus)
        {
            rtmPublisherBus.InitPublisher();
            consumerBus.Consumer(ConsumeEvent);

            _rtmPublisherBus = rtmPublisherBus;
        }

        private void ConsumeEvent((string eventType, string body) data)
        {
            var eventType = EventRecognizer.GetEventBy(data.eventType);
            var e = JsonSerializer.Deserialize(data.body, eventType);

            Console.WriteLine(data.body);

            switch (e)
            {
                case CreateGame createGame:
                    NewGame(createGame);
                    break;
                case AddPlayer addPlayer:
                    AddPlayer(addPlayer);
                    break;
                case StartGame startGame:
                    StartGame(startGame);
                    break;
                case GiveAnswer giveAnswer:
                    GiveAnswer(giveAnswer);
                    break;
                case CallEndGame endGame:
                    CallEndGame(endGame);
                    break;
                case RemovePlayer removePlayer:
                    RemovePlayer(removePlayer);
                    break;
            }
        }

        private void RemovePlayer(RemovePlayer removePlayer)
        {
            var game = _games.FirstOrDefault(f => f.Value.GameState.Players.Any(a => a.Id == removePlayer.Id)).Value;
            game?.EndGame();
            if (!(game is null))
            {
                _games.Remove(game.GameState.GameMasterId);
            }
        }

        private void CallEndGame(CallEndGame endGame)
        {
            if (!_games.ContainsKey(endGame.GameId))
            {
                return;
            }
            var game = _games[endGame.GameId];
            game.EndGame();
            _games.Remove(endGame.GameId);
        }

        private void GiveAnswer(GiveAnswer giveAnswer)
        {
            if (!_games.ContainsKey(giveAnswer.GameId))
            {
                return;
            }
            var game = _games[giveAnswer.GameId];
            game?.Answer(giveAnswer.PlayerId, giveAnswer.AnswerId);
        }

        private void StartGame(StartGame startGame)
        {
            if (!_games.ContainsKey(startGame.GameId))
            {
                return;
            }
            var game = _games[startGame.GameId];
            game?.Start();
        }

        private void AddPlayer(AddPlayer addPlayer)
        {
            if (!_games.ContainsKey(addPlayer.GameId))
            {
                return;
            }
            var game = _games[addPlayer.GameId];
            game?.AddPlayer(new Player { Id = addPlayer.Id, Name = addPlayer.Name });
        }

        private void NewGame(CreateGame createGame)
        {
            var runner = new GameRunner(createGame.ClientId, _rtmPublisherBus);
            _games.Add(createGame.ClientId, runner);
        }
    }
}