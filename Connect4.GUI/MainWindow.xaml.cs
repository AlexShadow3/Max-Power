using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Connect4.Core;

namespace Connect4.GUI;

public partial class MainWindow : Window
{
    private Board _board;
    private Player _currentPlayer;
    private bool _playAgainstAI;
    private AI _ai;
    private bool _gameEnded;
    private Ellipse[,] _uiCells;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        GameGrid.Children.Clear();
        _uiCells = new Ellipse[Board.Columns, Board.Rows];

        for (int r = 0; r < Board.Rows; r++)
        {
            for (int c = 0; c < Board.Columns; c++)
            {
                Border border = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A535C")),
                    Margin = new Thickness(2),
                    CornerRadius = new CornerRadius(50) // Try to make it somewhat circular if stretch allows, or rely on ellipse
                };

                Ellipse ellipse = new Ellipse
                {
                    Fill = Brushes.White,
                    Margin = new Thickness(5),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                int captureCol = c;
                ellipse.MouseLeftButtonDown += (s, e) => Cell_Click(captureCol);

                border.Child = ellipse;
                GameGrid.Children.Add(border);
                _uiCells[c, r] = ellipse; // Store 0,0 as top-left
            }
        }
    }

    private void StartGame(bool againstAI)
    {
        _board = new Board();
        _currentPlayer = Player.Player1;
        _playAgainstAI = againstAI;
        _ai = new AI();
        _gameEnded = false;
        
        UpdateUI();
        TxtStatus.Text = "C'est au tour du Joueur 1 (Rouge)";
    }

    private void BtnPVP_Click(object sender, RoutedEventArgs e)
    {
        StartGame(false);
    }

    private void BtnPVE_Click(object sender, RoutedEventArgs e)
    {
        StartGame(true);
    }

    private void Cell_Click(int col)
    {
        if (_gameEnded || _board == null) return;
        
        if (_currentPlayer == Player.Player2 && _playAgainstAI) return; // Prevent clicking while AI plays

        if (_board.DropToken(col, _currentPlayer))
        {
            UpdateUI();
            CheckGameEnd();

            if (!_gameEnded && _playAgainstAI)
            {
                _currentPlayer = Player.Player2;
                TxtStatus.Text = "L'ordinateur réfléchit...";
                
                // Use a dispatcher timer or simple async to not block UI
                System.Threading.Tasks.Task.Delay(500).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (_gameEnded) return;
                        int bestCol = _ai.GetBestMove(_board, Player.Player2, 6);
                        if (bestCol != -1)
                        {
                            _board.DropToken(bestCol, Player.Player2);
                            UpdateUI();
                            CheckGameEnd();
                            if (!_gameEnded)
                            {
                                _currentPlayer = Player.Player1;
                                TxtStatus.Text = "C'est au tour du Joueur 1 (Rouge)";
                            }
                        }
                    });
                });
            }
            else if (!_gameEnded)
            {
                _currentPlayer = _currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
                TxtStatus.Text = $"C'est au tour du {(_currentPlayer == Player.Player1 ? "Joueur 1 (Rouge)" : "Joueur 2 (Jaune)")}";
            }
        }
    }

    private void CheckGameEnd()
    {
        GameStatus status = _board.CheckStatus();
        if (status != GameStatus.Ongoing)
        {
            _gameEnded = true;
            if (status == GameStatus.Draw)
            {
                TxtStatus.Text = "Match nul !";
            }
            else if (status == GameStatus.Player1Wins)
            {
                TxtStatus.Text = "Joueur 1 (Rouge) a gagné !";
            }
            else
            {
                TxtStatus.Text = _playAgainstAI ? "L'ordinateur (Jaune) a gagné !" : "Joueur 2 (Jaune) a gagné !";
            }
        }
    }

    private void UpdateUI()
    {
        for (int r = 0; r < Board.Rows; r++)
        {
            for (int c = 0; c < Board.Columns; c++)
            {
                Player p = _board.GetCell(c, r);
                if (p == Player.Player1)
                {
                    _uiCells[c, r].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF233C")); // Red
                }
                else if (p == Player.Player2)
                {
                    _uiCells[c, r].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD166")); // Yellow
                }
                else
                {
                    _uiCells[c, r].Fill = Brushes.White;
                }
            }
        }
    }
}