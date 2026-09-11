using System;
using Connect4.Core;

namespace Connect4.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenue dans Puissance 4 !");
        Console.WriteLine("1. Joueur vs Joueur");
        Console.WriteLine("2. Joueur vs Ordinateur");
        Console.Write("Choisissez un mode (1-2) : ");
        
        string choice = Console.ReadLine();
        bool playAgainstAI = choice == "2";
        
        Board board = new Board();
        Player currentPlayer = Player.Player1;
        AI ai = new AI();
        bool gameEnded = false;

        while (!gameEnded)
        {
            Console.Clear();
            DrawBoard(board);

            if (currentPlayer == Player.Player1 || !playAgainstAI)
            {
                int col = -1;
                bool validMove = false;
                while (!validMove)
                {
                    Console.Write($"\nJoueur {(currentPlayer == Player.Player1 ? "1 (Rouge)" : "2 (Jaune)")}, choisissez une colonne (1-7) : ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out col) && col >= 1 && col <= Board.Columns)
                    {
                        col--; // 0-indexed
                        if (board.DropToken(col, currentPlayer))
                        {
                            validMove = true;
                        }
                        else
                        {
                            Console.WriteLine("Colonne pleine, réessayez.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrée invalide.");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nL'ordinateur réfléchit...");
                int bestMove = ai.GetBestMove(board, Player.Player2, 6); // Depth 6
                board.DropToken(bestMove, Player.Player2);
            }

            GameStatus status = board.CheckStatus();
            if (status != GameStatus.Ongoing)
            {
                Console.Clear();
                DrawBoard(board);
                if (status == GameStatus.Draw)
                {
                    Console.WriteLine("\nMatch nul !");
                }
                else
                {
                    string winner = status == GameStatus.Player1Wins ? "Joueur 1 (Rouge)" : "Joueur 2 (Jaune) / Ordinateur";
                    Console.WriteLine($"\n{winner} a gagné !");
                }
                gameEnded = true;
            }
            else
            {
                currentPlayer = currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            }
        }
    }

    static void DrawBoard(Board board)
    {
        Console.WriteLine(" 1 2 3 4 5 6 7");
        Console.WriteLine("---------------");
        for (int r = 0; r < Board.Rows; r++)
        {
            Console.Write("|");
            for (int c = 0; c < Board.Columns; c++)
            {
                Player p = board.GetCell(c, r);
                char symbol = ' ';
                if (p == Player.Player1) symbol = 'X';
                else if (p == Player.Player2) symbol = 'O';
                Console.Write($"{symbol}|");
            }
            Console.WriteLine();
        }
        Console.WriteLine("---------------");
        Console.WriteLine(" X = Joueur 1, O = Joueur 2");
    }
}
