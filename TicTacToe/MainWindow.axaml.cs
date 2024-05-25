using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private string _ongoingPlayer; // Stores the symbol of the current player ('X' or 'O')
        private string[] _canvas; // Represents the Tic-Tac-Toe board
        private Button[] _buttons; // Array to hold references to the grid cell buttons
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            // Initialize the buttons array with references to the grid cell buttons
            _buttons = new Button[9] {
                this.FindControl<Button>("Button1"),
                this.FindControl<Button>("Button2"),
                this.FindControl<Button>("Button3"),
                this.FindControl<Button>("Button4"),
                this.FindControl<Button>("Button5"),
                this.FindControl<Button>("Button6"),
                this.FindControl<Button>("Button7"),
                this.FindControl<Button>("Button8"),
                this.FindControl<Button>("Button9"),
            };
            // Register event handlers for button click events
            foreach (var button in _buttons)
            {
                button.Click += OnButtonClicked;
            }
        }
        public MainWindow()
        {
            InitializeComponent(); // Initialize the UI components
            InitializeGame(); // Set up the initial game state
        }

        private void InitializeGame()
        {
            _ongoingPlayer = "X"; // Set the starting player
            _canvas = new string[9]; // Initialize the board array

            // Reset button contents and enable all buttons
            foreach (var button in _buttons)
            {
                button.Content = string.Empty;
                button.IsEnabled = true;
            }
            
            // Update status label to indicate whose turn it is
            var statusLabel = this.FindControl<TextBlock>("StatusLabel");
            statusLabel.Text = "Player X's turn";
        }

        private async void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int index = Array.IndexOf(_buttons, button);
            
            // Check if the cell is empty
            if (_canvas[index] == null)
            {
                // Update board and button content
                _canvas[index] = _ongoingPlayer;
                button.Content = _ongoingPlayer;
                button.IsEnabled = false;
                
                // Check for winner or tie
                if (CheckWinner())
                {
                    // Display win message
                    await ShowMessageAsync($"Congratulations! Player {_ongoingPlayer} wins!");
                    ResetGame();
                }
                else if (Array.IndexOf(_canvas, null) == -1)
                {
                    // Display tie message
                    await ShowMessageAsync("Oh no, it is a tie!");
                    ResetGame();
                }
                else
                {
                    // Switch player and update status label
                    _ongoingPlayer = _ongoingPlayer == "X" ? "O" : "X";
                    var statusLabel = this.FindControl<TextBlock>("StatusLabel");
                    statusLabel.Text = $"It is player {_ongoingPlayer}'s turn";
                }
            }
        }

        private bool CheckWinner()
        {
            // Define winning combinations
            int[][] winningCombinations = new int[][]
            {
                new int[] { 0, 1, 2 },
                new int[] { 3, 4, 5 },
                new int[] { 6, 7, 8 },
                new int[] { 0, 3, 6 },
                new int[] { 1, 4, 7 },
                new int[] { 2, 5, 8 },
                new int[] { 0, 4, 8 },
                new int[] { 2, 4, 6 }
            };
            
            // Check each winning combination
            foreach (var combination in winningCombinations)
            {
                if (_canvas[combination[0]] != null &&
                    _canvas[combination[0]] == _canvas[combination[1]] &&
                    _canvas[combination[1]] == _canvas[combination[2]])
                {
                    return true;
                }
            }

            return false;
        }

        private void ResetGame()
        {
            // Resets game
            InitializeGame();
        }

        private async System.Threading.Tasks.Task ShowMessageAsync(string message)
        {
            // Creates and displays a message box
            var messageBox = new Window
            {
                Title = "Tic Tac Toe!",
                Width = 300,
                Height = 300,
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock
                        {
                            Text = message,
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                        },
                        new Button
                        {
                            Content = "Finish",
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Margin = new Thickness(20, 20, 20, 20)
                        }
                    }
                }
            };

            var button = (Button)((StackPanel)messageBox.Content).Children[1];
            // Closes the message box on button click
            button.Click += (sender, e) => messageBox.Close();
            
            // Shows the message box
            await messageBox.ShowDialog(this);
        }
    }
}
