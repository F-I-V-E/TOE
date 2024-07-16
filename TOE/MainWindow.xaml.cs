﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            if (!alreadyShowedWinningScreen)
            {
                if (tac.checkWinner() > 0)
                {
                    MessageBox.Show($"Player {tac.checkWinner()} won!");
                    alreadyShowedWinningScreen = true;
                }
                else if (tac.checkWinner() == -1)
                {
                    MessageBox.Show("Tie!");
                    alreadyShowedWinningScreen = true;
                }
            }

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
            gameRunning = false;
            alreadyShowedWinningScreen = false;
            tac.Reset();
            refreshField();
        }
    }
}