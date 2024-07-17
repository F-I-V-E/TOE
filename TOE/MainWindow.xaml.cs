using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TOE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool gameRunning = false;
        Brush[] brushes = [Brushes.Black, Brushes.Black, Brushes.Black];
        int difficulty = int.MaxValue;
        int player = 1;
        TAC tac = new TAC();
        bool alreadyShowedWinningScreen = false;
        
        int[] score = new []{0,0};
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void place(object sender, RoutedEventArgs e)
        {
            try
            {
                gameRunning = true;
                Button? button = sender as Button;
                refreshField();
                tac.place(x: button.Name[4] - 48, y: button.Name[5] - 48, player: player);
                if (++player == 3) player = 1;
                
                refreshField();

                tac.aiMove(player,difficulty);
                await Task.Delay(10);
                Thread.Sleep(490);
                refreshField();

                if (++player == 3) player = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void refreshField()
        {
            MessageBoxResult result = MessageBoxResult.None;

            Button[,] buttons = {
                { btn_00, btn_01, btn_02 },
                { btn_10, btn_11, btn_12 },
                { btn_20, btn_21, btn_22 }
                };

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    buttons[x, y].Content = tac.convertPlayerData([x, y]);
                    buttons[x, y].Foreground = brushes[tac.getPlayerInt(x, y)];
                    buttons[x,y].FontSize = 200;
                }
            }
            
            lb_score.Content = $"{score[0]} : {score[1]}";

            if (!alreadyShowedWinningScreen)
            {
                if (tac.checkWinner() > 0)
                {
                    if (tac.checkWinner() == 1)
                        score[0]++;
                    else
                        score[1]++;
                    
                    result = MessageBox.Show($"Player {tac.checkWinner()} won!");
                    alreadyShowedWinningScreen = true;
                }
                else if (tac.checkWinner() == -1)
                {
                    result = MessageBox.Show("Tie!");
                    alreadyShowedWinningScreen = true;
                }
            }
            if (result == MessageBoxResult.OK)
                ResetGame();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Preferences preferences = new();
            preferences.ShowDialog();
            if (!gameRunning) difficulty = preferences.Difficulty;
            brushes = preferences.Brushess;
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            score = new []{0,0};
            ResetGame();
        }

        private void ResetGame()
        {
            gameRunning = false;
            tac.Reset();
            alreadyShowedWinningScreen = false;
            refreshField();
        }
    }
}