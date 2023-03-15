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
    /// Логика взаимодействия для PaysWindow.xaml
    /// </summary>
    public partial class PaysWindow : Window
    {
        public double SetSumCredit, SetPercentCredit, SetMonthQuantity;//переменные которые не используются в вычислениях, но нужны для передачи в MainWindow
        bool check;
        public PaysWindow(double SumCredit, double PercentCredit, double MonthQuantity, bool check)
        {
            InitializeComponent();
            SetSumCredit= SumCredit;
            SetPercentCredit= PercentCredit*100;
            SetMonthQuantity= MonthQuantity;
            if (check)//проверка чекбокса на выбор вида платежа
            {
                //Диф
                DifferentialPay(SumCredit, PercentCredit, MonthQuantity);
                check= true;
            }
            else{
                //Анн
                AnnuityPay(SumCredit, PercentCredit, MonthQuantity);
               check = false;
            }
            
        }



        public double SumCredit, PercentCredit, MonthQuantity, RamainsPay;//переменные для вычисления 

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex) { }
        }
        private void closeApp(object sender, MouseButtonEventArgs e)
        {

            try
            {
                Close();
            }
            catch (Exception ex) { }
        }
        private void RollUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex) { }
        }

        private void BactToMain_Click(object sender, RoutedEventArgs e)//возврат к mainWidnod с последующим закрытием окна PaysWindow
        {   
            MainWindow mainWindow = new MainWindow();

            mainWindow.SetNum(SetSumCredit,SetPercentCredit,SetMonthQuantity,check);//Возвращаю вводные значения в MainWindow
            mainWindow.Show();
            Close();
        }

        public class Credits //класс для подвязки ListView
        {
            public string Month { get; set; }
            public double Pays { get; set; }
            public double Ramains { get; set; }
            public double MainDebt { get; set; }
            public double PercentPay { get; set; }
        }
        public decimal Truncate(decimal number, int digits)//метод для усечения числа
        {
            decimal stepper = (decimal)(Math.Pow(10.0, (double)digits));
            int temp = (int)(stepper * number);
            return (decimal)temp / stepper;
        }
        private void AnnuityPay(double SumCredit, double PercentCredit, double MonthQuantity) //метод по вычислению аннуитетного платежа
        {
            List<Credits> pay = new List<Credits>();
            Credits cred = new Credits { Month = null, Pays = 0, Ramains = 0, MainDebt = 0, PercentPay = 0 }; //Создаю экземпляр класса Credit с обнулеными 
            double percentForPay = PercentCredit;
            PercentCredit = (Math.Round (PercentCredit / 12,5)%10); 
            
            double coefficient =  PercentCredit * (Math.Pow(1 + PercentCredit,MonthQuantity)) /(Math.Pow(1+PercentCredit,MonthQuantity) -1);
            decimal TruncateCoefficient = Truncate((decimal)coefficient,6);
            double annPayInMonth = SumCredit * (double)TruncateCoefficient;
            double[] RemainsPay = new double[(int)MonthQuantity];
            double SumCreditInRemains = SumCredit;//для расчета остатка
            int month = 1;
            double[] MainDebt = new double[(int)MonthQuantity];
            double[] percentPay  = new double[(int)MonthQuantity];
            
            for(int i = 0; i < MonthQuantity; i++)
            {
                 

                percentPay[i] = SumCreditInRemains * percentForPay / 12;//процент по плтажу 
                MainDebt[i] = annPayInMonth - percentPay[i];
                SumCreditInRemains -= annPayInMonth - percentPay[i]; // основной долг из таблицы
                
                RemainsPay[i] = SumCreditInRemains;
                

                RemainsPay[(int)MonthQuantity-1] = 0;
                
                cred = new Credits { Month = Convert.ToString(month), Ramains = RemainsPay[i], Pays = annPayInMonth,PercentPay = percentPay[i],MainDebt = MainDebt[i] };
                month++;
                pay.Add(cred);
            }
            listView.ItemsSource = pay;
            double GeneralCostCredit = Math.Round( annPayInMonth * MonthQuantity,2);
            buttonDebt.Content = $"Общая стоимость кредита: {GeneralCostCredit}";
            double RemainsForPay = Math.Round( annPayInMonth * MonthQuantity - SumCredit,2);
            buttonRemains.Content = $"Переплата по процентам: {RemainsForPay}";
        }
        void DifferentialPay(double SumCredit, double PercentCredit, double MonthQuantity)//метод по вычислению дифференцального  платежа
        {

            List<Credits> pay = new List<Credits>();
            double mainDebt = 0;
            double percentPay = 0;
            double percentPayCount = 0;
            double SumDebt = 0;
            double effectivePercent = 0;
                Credits cred = new Credits { Month = null, Pays = 0, Ramains = 0, MainDebt = 0, PercentPay = 0 }; //Создаю экземпляр класса Credit с обнулеными переменными
                double[] Pays = new double[Convert.ToInt32(MonthQuantity)];
                RamainsPay = SumCredit;
                mainDebt = SumCredit / MonthQuantity;
                int month = 1;
                for (int i = 0; i < MonthQuantity; i++)
                {

                    Pays[i] = Math.Round(SumCredit / MonthQuantity + RamainsPay * PercentCredit / MonthQuantity, 4); //вычисление оплаты в месяц
                    RamainsPay -= SumCredit / MonthQuantity;//остаток по оплате
                    percentPay = Pays[i] - mainDebt;
                    percentPayCount += percentPay;
                    cred = new Credits { Month = Convert.ToString(month), Pays = Pays[i], Ramains = RamainsPay, MainDebt = mainDebt, PercentPay = percentPay };
                    month++;
                    
                    pay.Add(cred);

                }
            SumDebt = SumCredit + percentPayCount;
            effectivePercent = Math.Round(((SumDebt * 100) / SumCredit) - 100, 2);
            listView.ItemsSource = pay;
            buttonDebt.Content = $"Общая стоимость кредита: {Math.Round(SumDebt, 2)}";
            buttonRemains.Content = $"Переплата по процентам: {Math.Round(percentPayCount, 2)}";
            
        }

    }
}
