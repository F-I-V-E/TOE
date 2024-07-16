using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TOE
{
    partial class Preferences : Window
    {
        Brush[] brushes = new Brush[] { Brushes.Black, Brushes.Black, Brushes.Black };
        int difficulty = int.MaxValue;
        public int Difficulty => difficulty;
        public Brush[] Brushess => brushes;
        public Preferences() => InitializeComponent();

        private void difficultySelected(object sender, SelectionChangedEventArgs e)
        {
            switch (cmb_setting_difficulty.SelectedIndex)
            {
                case 0:
                    difficulty = 1;
                    break;
                case 1:
                    difficulty = 3;
                    break;
                case 2:
                    difficulty = 6;
                    break;
                case 3:
                    difficulty = int.MaxValue;
                    break;
                default:
                    break;
            }
        }
        private void ColorChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BrushConverter bC = new BrushConverter();
                TextBox tbx = (TextBox)sender;

                string text = tbx.Text.ToLower();

                foreach (char c in text)
                {
                    if (!(c < 103 && c > 96) && (c < 48 || c > 57)) throw new Exception("Wrong Format");
                }
                if (text.Length != 6) throw new Exception("Wrong Format");

                int[] colorsHex = new int[3];
                
                for (int i = 0; i < colorsHex.Length; i++)
                {
                    colorsHex[i] = Convert.ToInt32(text.Substring(i*2,2),16);
                }

                byte[] colors = new byte[3];

                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = Convert.ToByte(colorsHex[i]);
                }

                Color temp = Color.FromRgb(colors[0], colors[1], colors[2]);

                switch (tbx.Name)
                {
                    case "tb_menu_color":
                        brushes[0] = new SolidColorBrush(temp);
                        break;
                    case "tb_p1_color":
                        brushes[1] = new SolidColorBrush(temp);
                        break;
                    case "tb_p2_color":
                        brushes[2] = new SolidColorBrush(temp);
                        break;
                }
                tbx.Background = new SolidColorBrush(temp);
            }
        }
    }
}
