using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.Bus;
using Contracts.Events;
using Game.Models;

namespace Game
{
    public class GameRunner
    {
        private Task _gameTask;
        private bool _isRunning;
        private bool _stop;

        public State GameState { get; }

        private readonly IBus _publishEvent;

        public GameRunner(string gameMasterId, IBus publishEvent)
        {
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
            Console.WriteLine($"Game is start | Id: {GameState.GameMasterId}");
            GameState.CurrentState = EnumGameState.WaitingForNextLevel;
            GameState.CurrentTime = 0;

            var ev = new WaitingForNextLevel
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                StateTime = GameState.TotalStateTime()
            };
            _publishEvent.Publish(ev);

            _gameTask = Task.Run(Running);
        }

        public void Answer(string playerId, int answerId)
        {
            if (GameState.LevelData.CorrectWordIndex == answerId)
            {
                GameState.LevelData.WinnerId = playerId;
            }
            else
            {
                GameState.LevelData.LooserId = playerId;
            }

            GameState.CurrentState = EnumGameState.Summary;
            ShowSummary();
        }

        private void Running()
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
                            ShowSummary();
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
                            EndLevel();
                        }
                        break;
                    case EnumGameState.End:
                        EndGame();
                        break;
                }

                GameState.CurrentTime++;
                Task.Delay(1000);
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
                Points = GameState.Players.ToDictionary(k => k.Id, e => e.Points),
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
                Points = GameState.Players.ToDictionary(k => k.Id, e => e.Points)
            };
            _publishEvent.Publish(ev);
            GameState.CurrentTime = 0;
        }

        private void NextLevel()
        {
            GameState.LevelData = new LevelData
            {
                CorrectWordIndex = 2,
                Words = new List<string>{
                    "oluszon",
                    "bejbuszon",
                    "sebuszon",
                    "gofer"
                }
            };
            GameState.CurrentTime = 0;

            var ev = new NewLevel
            {
                GameId = GameState.GameMasterId,
                PlayersIds = GameState.Players.Select(s => s.Id).ToList(),
                Answers = GameState.LevelData.Words.Select((s, i) => new Answer { Id = i, Value = s }).ToList()
            };
            _publishEvent.Publish(ev);
            GameState.CurrentTime = 0;
        }

        private void EndLevel()
        {
            GameState.GameAttempts++;
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