using System;
using System.Collections.Generic;

namespace Connect4.Core;

public class AI
{
    private Random _random = new Random();

    public int GetBestMove(Board board, Player aiPlayer, int depth)
    {
        Player opponent = aiPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
        int bestScore = int.MinValue;
        int bestCol = -1;

        List<int> validCols = new List<int>();
        for (int c = 0; c < Board.Columns; c++)
        {
            if (!board.IsColumnFull(c))
            {
                validCols.Add(c);
            }
        }

        if (validCols.Count == 0) return -1; // Should not happen if game is not over

        foreach (int col in validCols)
        {
            board.DropToken(col, aiPlayer);
            int score = Minimax(board, depth - 1, false, aiPlayer, opponent);
            board.UndoDrop(col);

            if (score > bestScore)
            {
                bestScore = score;
                bestCol = col;
            }
            else if (score == bestScore)
            {
                // Randomize equal moves a bit to add variety
                if (_random.Next(2) == 0)
                {
                    bestCol = col;
                }
            }
        }

        return bestCol;
    }

    private int Minimax(Board board, int depth, bool isMaximizing, Player aiPlayer, Player opponent)
    {
        GameStatus status = board.CheckStatus();
        if (status == (aiPlayer == Player.Player1 ? GameStatus.Player1Wins : GameStatus.Player2Wins))
            return 1000000 + depth; // Favor earlier wins
        if (status == (opponent == Player.Player1 ? GameStatus.Player1Wins : GameStatus.Player2Wins))
            return -1000000 - depth; // Favor later losses
        if (status == GameStatus.Draw)
            return 0;
        if (depth == 0)
            return EvaluateBoard(board, aiPlayer, opponent);

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            for (int c = 0; c < Board.Columns; c++)
            {
                if (!board.IsColumnFull(c))
                {
                    board.DropToken(c, aiPlayer);
                    int eval = Minimax(board, depth - 1, false, aiPlayer, opponent);
                    board.UndoDrop(c);
                    maxEval = Math.Max(maxEval, eval);
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            for (int c = 0; c < Board.Columns; c++)
            {
                if (!board.IsColumnFull(c))
                {
                    board.DropToken(c, opponent);
                    int eval = Minimax(board, depth - 1, true, aiPlayer, opponent);
                    board.UndoDrop(c);
                    minEval = Math.Min(minEval, eval);
                }
            }
            return minEval;
        }
    }

    private int EvaluateBoard(Board board, Player aiPlayer, Player opponent)
    {
        // Simple heuristic: center column is good
        int score = 0;
        for (int r = 0; r < Board.Rows; r++)
        {
            if (board.GetCell(Board.Columns / 2, r) == aiPlayer)
                score += 3;
            else if (board.GetCell(Board.Columns / 2, r) == opponent)
                score -= 3;
        }
        return score;
    }
}
