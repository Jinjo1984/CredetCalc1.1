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
using System.Windows.Shapes;

namespace CredetCalc1._1
{
    /// <summary>
    /// Логика взаимодействия для WindowError.xaml
    /// </summary>
    public partial class WindowError : Window
    {
        public WindowError()
        {
            InitializeComponent();
        }
        private void closeApp(object sender, MouseButtonEventArgs e)
        {

            try
            {
                Close();
            }
            catch (Exception ex) { }
        }
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {

            try
            {
                DragMove();
            }
            catch (Exception ex) { }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWidow = new MainWindow();
            mainWidow.Show();
            Close();
        }
    }
}
