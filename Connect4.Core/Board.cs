using System;

namespace Connect4.Core;

public class Board
{
    public const int Columns = 7;
    public const int Rows = 6;
    private Player[,] _grid;

    public Board()
    {
        _grid = new Player[Columns, Rows];
    }

    public Player GetCell(int col, int row) => _grid[col, row];

    public bool DropToken(int col, Player player)
    {
        if (col < 0 || col >= Columns) return false;

        for (int r = Rows - 1; r >= 0; r--)
        {
            if (_grid[col, r] == Player.None)
            {
                _grid[col, r] = player;
                return true;
            }
        }
        return false;
    }

    public void UndoDrop(int col)
    {
        for (int r = 0; r < Rows; r++)
        {
            if (_grid[col, r] != Player.None)
            {
                _grid[col, r] = Player.None;
                break;
            }
        }
    }

    public bool IsColumnFull(int col)
    {
        return _grid[col, 0] != Player.None;
    }

    public GameStatus CheckStatus()
    {
        // Check horizontal
        for (int c = 0; c <= Columns - 4; c++)
        {
            for (int r = 0; r < Rows; r++)
            {
                if (_grid[c, r] != Player.None &&
                    _grid[c, r] == _grid[c + 1, r] &&
                    _grid[c, r] == _grid[c + 2, r] &&
                    _grid[c, r] == _grid[c + 3, r])
                {
                    return _grid[c, r] == Player.Player1 ? GameStatus.Player1Wins : GameStatus.Player2Wins;
                }
            }
        }

        // Check vertical
        for (int c = 0; c < Columns; c++)
        {
            for (int r = 0; r <= Rows - 4; r++)
            {
                if (_grid[c, r] != Player.None &&
                    _grid[c, r] == _grid[c, r + 1] &&
                    _grid[c, r] == _grid[c, r + 2] &&
                    _grid[c, r] == _grid[c, r + 3])
                {
                    return _grid[c, r] == Player.Player1 ? GameStatus.Player1Wins : GameStatus.Player2Wins;
                }
            }
        }

        // Check diagonal (down-right)
        for (int c = 0; c <= Columns - 4; c++)
        {
            for (int r = 0; r <= Rows - 4; r++)
            {
                if (_grid[c, r] != Player.None &&
                    _grid[c, r] == _grid[c + 1, r + 1] &&
                    _grid[c, r] == _grid[c + 2, r + 2] &&
                    _grid[c, r] == _grid[c + 3, r + 3])
                {
                    return _grid[c, r] == Player.Player1 ? GameStatus.Player1Wins : GameStatus.Player2Wins;
                }
            }
        }

        // Check diagonal (up-right)
        for (int c = 0; c <= Columns - 4; c++)
        {
            for (int r = 3; r < Rows; r++)
            {
                if (_grid[c, r] != Player.None &&
                    _grid[c, r] == _grid[c + 1, r - 1] &&
                    _grid[c, r] == _grid[c + 2, r - 2] &&
                    _grid[c, r] == _grid[c + 3, r - 3])
                {
                    return _grid[c, r] == Player.Player1 ? GameStatus.Player1Wins : GameStatus.Player2Wins;
                }
            }
        }

        // Check draw
        bool isDraw = true;
        for (int c = 0; c < Columns; c++)
        {
            if (!IsColumnFull(c))
            {
                isDraw = false;
                break;
            }
        }

        if (isDraw) return GameStatus.Draw;

        return GameStatus.Ongoing;
    }
}
