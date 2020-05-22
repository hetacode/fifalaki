using System;
using System.Collections.Generic;
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

        public GamesManager(IBus bus)
        {
            bus.Consumer(ConsumeEvent);
        }

        private void ConsumeEvent((string eventType, string body) data)
        {
            var eventType = EventRecognizer.GetEventBy(data.eventType);
            var e = JsonSerializer.Deserialize(data.body, eventType);

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
            }
        }

        private void CallEndGame(CallEndGame endGame)
        {
             var game = _games[endGame.GameId];
             game.EndGame();
             _games.Remove(endGame.GameId);
        }

        private void GiveAnswer(GiveAnswer giveAnswer)
        {
            var game = _games[giveAnswer.GameId];
            game?.Answer(giveAnswer.PlayerId, giveAnswer.AnswerId);
        }

        private void StartGame(StartGame startGame)
        {
            var game = _games[startGame.GameId];
            game?.Start();
        }

        private void AddPlayer(AddPlayer addPlayer)
        {
            var game = _games[addPlayer.GameId];
            game?.AddPlayer(new Player { Id = addPlayer.Id, Name = addPlayer.Name });
        }

        private void NewGame(CreateGame createGame)
        {
            var runner = new GameRunner(createGame.ClientId);
            _games.Add(createGame.ClientId, runner);
        }
    }
}