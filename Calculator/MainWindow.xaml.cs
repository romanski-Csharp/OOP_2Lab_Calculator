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

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private double currentValue = 0;
        private string currentOperator = "";
        private bool isNewEntry = true;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            isNewEntry = true;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            currentValue = 0;
            currentOperator = "";
            isNewEntry = true;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 1)
                Display.Text = Display.Text.Remove(Display.Text.Length - 1);
            else
                Display.Text = "0";
        }
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (isNewEntry)
            {
                Display.Text = (number == "00") ? "0" : number;
                isNewEntry = false;
            }
            else if (Display.Text == "0")
            {
                Display.Text = (number == "0" || number == "00") ? "0" : number;
            }
            else
            {
                Display.Text += number;
            }
        }
        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (isNewEntry)
            {
                Display.Text = "0,";
                isNewEntry = false;
            }
            else if (!Display.Text.Contains(","))
            {
                Display.Text += ",";
            }
        }
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string op = button.Content.ToString();

            if (!string.IsNullOrEmpty(currentOperator) && !isNewEntry)
            {
                CalculateResult();
            }
            else
            {
                currentValue = double.Parse(Display.Text);
            }

            currentOperator = op;
            isNewEntry = true;
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            CalculateResult();
            currentOperator = "";
        }

        private void CalculateResult()
        {
            if (string.IsNullOrEmpty(currentOperator)) return;

            double displayValue = double.Parse(Display.Text);
            double result = 0;

            switch (currentOperator)
            {
                case "+":
                    result = currentValue + displayValue;
                    break;
                case "-":
                    result = currentValue - displayValue;
                    break;
                case "×":
                    result = currentValue * displayValue;
                    break;
                case "÷":
                    if (displayValue != 0)
                        result = currentValue / displayValue;
                    else
                    {
                        Display.Text = "Помилка";
                        currentValue = 0;
                        isNewEntry = true;
                        return;
                    }
                    break;

            }

            Display.Text = result.ToString();
            currentValue = result;
            isNewEntry = true;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                string keyText = e.Key.ToString().Replace("NumPad", "").Replace("D", "");
                SimulateButtonClick(keyText);
            }
            else if (e.Key == Key.Add || (e.Key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.Shift))
                SimulateOperatorClick("+");
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                SimulateOperatorClick("-");
            else if (e.Key == Key.Multiply)
                SimulateOperatorClick("×");
            else if (e.Key == Key.Divide || e.Key == Key.Oem2) // Oem2 - "/"
                SimulateOperatorClick("÷");
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.OemComma)
                Decimal_Click(null, null);
            else if (e.Key == Key.Enter || e.Key == Key.OemPlus)
                Equal_Click(null, null);
            else if (e.Key == Key.Escape)
                Clear_Click(null, null);
            else if (e.Key == Key.Back)
                Back_Click(null, null);
        }

        private void SimulateButtonClick(string content)
        {
            Button btn = new Button { Content = content };
            Number_Click(btn, null);
        }

        private void SimulateOperatorClick(string op)
        {
            Button btn = new Button { Content = op };
            Operator_Click(btn, null);
        }
    }
}