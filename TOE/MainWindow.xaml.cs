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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void place(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                button.Content = (tac.place(button.Name[4] - 48, button.Name[5] - 48, player));

                if (tac.checkWinner() > 0)
                {
                    MessageBox.Show($"Player {tac.checkWinner()} won!");
                }
                else if (tac.checkWinner() == -1) MessageBox.Show("Tie!");

                if (++player == 3) player = 1;

                tac.aiMove(player);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}