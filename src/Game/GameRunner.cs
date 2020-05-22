using System;
using System.Threading.Tasks;
using Arch.Bus;
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
                GameMasterId = gameMasterId
            };
            _publishEvent = publishEvent;
        }

        public void AddPlayer(Player player)
        {
            GameState.Players.Add(player);
        }

        public void Start()
        {
            Console.WriteLine($"Game is start | Id: {GameState.GameMasterId}");
            GameState.CurrentState = EnumGameState.WaitingForNextLevel;
            GameState.CurrentTime = 0;
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
                if (_stop) {
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
        }

        private void ShowSummary()
        {
            
        }

        private void NextLevel()
        {

        }

        private void EndLevel()
        {
            GameState.GameAttempts++;
            if (GameState.GameAttempts >= 5)
            {
                GameState.CurrentState = EnumGameState.End;
            }
            else
            {
                GameState.CurrentState = EnumGameState.Summary;
            }
            // TODO: send event
        }
    }
}