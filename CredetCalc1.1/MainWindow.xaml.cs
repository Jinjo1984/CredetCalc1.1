using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CredetCalc1._1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
     
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public bool ChekRadioBox;
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            
            try
            {
                DragMove();
            }
            catch(Exception ex) { }
        }

        private void closeApp(object sender, MouseButtonEventArgs e)
        {

            try
            {
                Close();
            }
            catch(Exception ex) { }
        }

        private void RollUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch(Exception ex) { }
        }

        public double SumCredit, PercentCredit, MonthQuantity;     
        
        private void button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SumCredit = Convert.ToDouble(SummCreditTextBox.Text);
                PercentCredit = Convert.ToDouble(PercentCreditTextBox.Text)/100;
                MonthQuantity = Convert.ToDouble(MonthQuantityTextBox.Text);
                PaysWindow paysWindow = new PaysWindow(SumCredit, PercentCredit, MonthQuantity, ChekRadioBox);
                try
                {
                    paysWindow.Show();
                    CloseMain();
                }
                catch { }
            }
            catch (FormatException ex) { new WindowError().Show(); Close(); }
        }
        
        private void DiffChecked(object sender, RoutedEventArgs e)
        {
            ChekRadioBox = true;
        }

        private void AnnChecked(object sender, RoutedEventArgs e)
        {
            ChekRadioBox = false;
        }
        public void CloseMain() 
        {
            Close();
        }


    }
}
