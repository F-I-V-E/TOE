using System.Text;
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
        int player = 1;
        TAC tac = new TAC();
        bool alreadyShowedWinningScreen = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void place(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                tac.place(button.Name[4] - 48, button.Name[5] - 48, player);
                refreshField();

                if (++player == 3) player = 1;

                tac.aiMove(player);
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
    }
}