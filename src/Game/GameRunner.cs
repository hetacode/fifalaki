using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.Bus;
using Contracts.Events;
using Game.Models;
using Game.Services;

namespace Game
{
    public class GameRunner
    {
        private static Random rnd = new Random();
        private readonly WordsGrpcService _words;

        private Task _gameTask;
        private bool _isRunning;
        private bool _stop;

        public State GameState { get; }

        private readonly IBus _publishEvent;

        public GameRunner(string gameMasterId, IBus publishEvent, WordsGrpcService words)
        {
            _words = words;
            GameState = new State
            {
                GameMasterId = gameMasterId,
                Players = new System.Collections.Generic.List<Player> { new Player { Id = gameMasterId, Name = "Game Master" } }
            };
            _publishEvent = publishEvent;
        }

        public void AddPlayer(Player player)
        {
            GameState.Players.Add(player);

            var ev = new UpdatedPlayers
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                Players = GameState.Players.Select(s => new PlayerData { Id = s.Id, Name = s.Name }).ToList()
            };
            _publishEvent.Publish(ev);
        }

        public void Start()
        {
            Console.WriteLine($"Game is started | Id: {GameState.GameMasterId}");
            GameState.CurrentState = EnumGameState.WaitingForNextLevel;
            GameState.CurrentTime = 0;

            var ev = new WaitingForNextLevel
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                StateTime = GameState.TotalStateTime()
            };
            _publishEvent.Publish(ev);

            _isRunning = true;
            _gameTask = Task.Run(Running);
        }

        public void Answer(string playerId, int answerId)
        {
            if (GameState.LevelData.CorrectWordIndex == answerId)
            {
                GameState.LevelData.WinnerId = playerId;
                GameState.Players.FirstOrDefault(f => f.Id == playerId).Points++;
            }
            else
            {
                GameState.LevelData.LooserId = playerId;
                GameState.Players.FirstOrDefault(f => f.Id == playerId).Points--;
                GameState.GameAttempts++;
            }

            EndLevel();
        }

        private async void Running()
        {
            for (; ; )
            {
                if (_stop)
                {
                    return;
                }
                if (!_isRunning)
                {
                    continue;
                }

                switch (GameState.CurrentState)
                {
                    case EnumGameState.Summary:
                        if (GameState.CurrentTime >= GameState.TotalStateTime())
                        {
                            GameState.CurrentState = EnumGameState.WaitingForNextLevel;
                            GameState.CurrentTime = 0;
                        }
                        break;
                    case EnumGameState.WaitingForNextLevel:
                        if (GameState.CurrentTime >= GameState.TotalStateTime())
                        {
                            NextLevel();
                        }
                        break;
                    case EnumGameState.Level:
                        if (GameState.CurrentTime >= GameState.TotalStateTime())
                        {
                            GameState.GameAttempts++;
                            EndLevel();
                        }
                        break;
                    case EnumGameState.End:
                        EndGame();
                        break;
                }

                GameState.CurrentTime++;
                await Task.Delay(1000);
            }
        }

        public void EndGame()
        {
            _isRunning = false;
            _stop = true;

            var ev = new EndGame
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                Points = GameState.Players.Where(w => w.Id != GameState.GameMasterId).ToDictionary(k => k.Id, e => e.Points),
                WinnerId = GameState.Players.OrderByDescending(m => m.Points).First().Id
            };
            _publishEvent.Publish(ev);
        }

        private void ShowSummary()
        {
            var ev = new SummaryLevel
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                Attempts = GameState.GameAttempts,
                LooserId = GameState.LevelData.LooserId,
                WinnerId = GameState.LevelData.WinnerId,
                Points = GameState.Players.Where(w => w.Id != GameState.GameMasterId).ToDictionary(k => k.Id, e => e.Points)
            };
            _publishEvent.Publish(ev);
            GameState.CurrentTime = 0;
        }

        private async void NextLevel()
        {
            var newWords = await _words.GetNewWordsList();
            GameState.LevelData = new LevelData
            {
                CorrectWordIndex = rnd.Next(0, newWords.Count - 1),
                Words = newWords
            };
            GameState.CurrentTime = 0;
            var correctWord = GameState.LevelData.Words[GameState.LevelData.CorrectWordIndex];
            var shuffle = new string(correctWord.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
            var ev = new NewLevel
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                Letters = shuffle,
                Answers = GameState.LevelData.Words.Select((s, i) => new Answer { Id = i, Value = s }).ToList()
            };
            await _publishEvent.Publish(ev);
            GameState.CurrentState = EnumGameState.Level;
        }

        private void EndLevel()
        {
            // GameState.GameAttempts++;
            if (GameState.GameAttempts >= 5)
            {
                GameState.CurrentState = EnumGameState.End;
                EndGame();
            }
            else
            {
                GameState.CurrentState = EnumGameState.Summary;
                ShowSummary();
            }
        }
    }
}