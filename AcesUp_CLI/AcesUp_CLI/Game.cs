using System;
using AcesUp_CoreLibrary;

namespace AcesUp_CLI
{
    class Game
    {
        private bool _gameOver;
        private AcesUp _acesUp = new AcesUp();

        public Game()
        {
            Console.Title = "ACES UP";

            Console.WriteLine("╔════════════════════════════╗" + "\n" +
                              "║       A C E S    U P       ║" + "\n" +
                              "╚════════════════════════════╝" + "\n");
            showHelp();
            _acesUp.Deal();
            this.Play();
        }

        private void showHelp()
        {
            Console.WriteLine("INSTRUCTION:\n" +
                              "────────────\n" +
                              " → DEAL: deal\n\n" +
                              " → PLAY: 1 ... 4\n" +
                              "         Ex: 3\n\n" +
                              " → MOVE: move <from> <to>\n" +
                              "         Ex: move 1 2\n\n" +
                              " → HELP: help\n\n");
        }

        public void Play()
        {
            if (!_gameOver)
            {
                Console.WriteLine("\n[ Score: " + _acesUp.Score + " ]");

                Console.WriteLine(_acesUp);
                Console.Write("-> PLAYER: ");
                string userCommand = Console.ReadLine();

                _gameOver = _acesUp.IsGameOver();

                try
                {
                    if (userCommand.Trim().Length == 1)
                        _acesUp.RemoveCardFromPile(Convert.ToInt32(userCommand.Trim()));
                    else if (userCommand.Trim().ToLower() == "deal")
                        _acesUp.Deal();
                    else if (userCommand.Trim().ToLower().Substring(0, 4) == "move")
                        _acesUp.MoveCard(Convert.ToInt32(userCommand[5].ToString()), Convert.ToInt32(userCommand[7].ToString()));
                    else if ((userCommand.Trim().ToLower() == "help") | (userCommand.Trim() == "?"))
                        showHelp();
                }
                catch (Exception exception)
                {
                    // Do nothing...
                }
                finally
                {
                    Play();
                }
            }
            else
            {
                if (_acesUp.Won)
                    Console.WriteLine("GAME OVER: You've Won");
                else
                    Console.WriteLine("GAME OVER: No More Moves");

                return;
            }
        }
    }
}
