using System;
using System.Threading.Tasks;
using Game.Models;

namespace Game
{
    public class GameRunner
    {
        private Task _gameTask;
        private bool _isRunning;

        public State GameState { get; }

        public GameRunner(string gameMasterId)
        {
            GameState = new State
            {
                GameMasterId = gameMasterId
            };
        }

        public void AddPlayer(Player player)
        {
            GameState.Players.Add(player);
        }

        public void Start()
        {
            GameState.CurrentState = EnumGameState.WaitingForNextLevel;
            GameState.CurrentTime = 0;
            _gameTask = Task.Run(Running);
        }

        private void Running()
        {
            for (; ; )
            {
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

        private void EndGame()
        {
            _isRunning = false;
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