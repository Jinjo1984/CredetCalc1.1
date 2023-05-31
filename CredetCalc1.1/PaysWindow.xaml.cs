using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace CredetCalc1._1
{
    /// <summary>
    /// Логика взаимодействия для PaysWindow.xaml
    /// </summary>
    public partial class PaysWindow : Window
    {
        public double SetSumCredit, SetPercentCredit, SetMonthQuantity;//переменные которые не используются в вычислениях, но нужны для передачи в MainWindow
        bool checkInSet;
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
                checkInSet= true;
            }
            else{
                //Анн
                AnnuityPay(SumCredit, PercentCredit, MonthQuantity);
                checkInSet = false;
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
        List<Credits> AnnuitExcel;
        List<Credits> DiferentExcel;
        public string RemoveStr(string str)
        {
            string newStr = "";
            
            foreach(char c in str) 
            {
                if (char.IsDigit(c) || c == ',')
                {
                    newStr += c;
                }
              
            }
            return newStr;
        }
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            string path, typePay;
            List<Credits> SetList = new List<Credits>();
            if (checkInSet)
            {
                SetList = DiferentExcel;
                typePay = "Дифференцированный";
            }
            else
            {
                SetList = AnnuitExcel;
                typePay = "Аннуитетный";
            }
            using (FolderBrowserDialog folderdialog = new FolderBrowserDialog())
            {

                folderdialog.ShowDialog();
                path = $"{folderdialog.SelectedPath}\\{DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss")}.xlsx"; 
                

                using (var package = new ExcelPackage())
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Отчёт");


                    //sheet.Cells["A1"].Style.Numberformat.Format = "@";


                    sheet.Cells[1, 2].Value = "Платеж";
                    sheet.Cells[1, 3].Value = "Остаток";
                    sheet.Cells[1, 4].Value = "Основной долг";
                    sheet.Cells[1, 5].Value = "Проценты";
                    sheet.Cells[1, 1].Value = "Месяц";
                    

                    sheet.Cells["A1:E1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells["A1:E1"].Style.Font.Bold = true;

                    SetList[SetList.Count - 1].Ramains = 0;
                   
                    string Debt =  RemoveStr(buttonDebt.Content.ToString());
                    string remains = RemoveStr(buttonRemains.Content.ToString()); 

                    sheet.Cells[1, 9].Value = "Общая стоимость кредита:";
                    sheet.Cells[2, 9].Value = Debt+ " ₽";

                    sheet.Cells[1, 10].Value = "Переплата по процентам:";
                    sheet.Cells[2, 10].Value = remains + " ₽";

                    sheet.Cells[1, 6].Value = "Сумма кредита";
                    sheet.Cells[2, 6].Value = SetSumCredit;

                    sheet.Cells[1, 7].Value = "Процент кредита";
                    sheet.Cells[2, 7].Value = SetPercentCredit + " %";

                    sheet.Cells["I1:J2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells["I1:J2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells["I1:J2"].Style.Font.Bold = true;
                    sheet.Cells["I2:J2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells["I2:J2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);


                    sheet.Cells["H1"].Value = "Тип платежа:";
                    sheet.Cells["H2"].Value = typePay;
                    sheet.Cells["H1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells["H1:H2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells["H1"].Style.Font.Bold = true;

                    sheet.Cells["F1:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells["F1:G2"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet.Cells["F1:G2"].Style.Font.Bold = true;

                    int k = 2;

                    for (int i = 0; i < SetList.Count; i++)
                    {
                        sheet.Cells[k, 1].Value = SetList[i].Month;
                        sheet.Cells[k, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[k, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        sheet.Cells[k, 3].Value = Math.Round( SetList[i].Ramains,2);
                        sheet.Cells[k, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[k, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        sheet.Cells[k, 2].Value = Math.Round( SetList[i].Pays,2);
                        sheet.Cells[k, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[k, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        sheet.Cells[k, 4].Value = Math.Round( SetList[i].MainDebt,2);
                        sheet.Cells[k, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[k, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        sheet.Cells[k, 5].Value = Math.Round(SetList[i].PercentPay, 2);
                        sheet.Cells[k, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet.Cells[k, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                        k++;
                    
                    }

                    //for (int i = 0; i < credits.Count; i++)
                    //{
                    //    sheet.Cells[$"A{i}"].Value = credits[i].Pays;
                    //}


                    // Format as text
                    //  sheet.Cells["A1"].Style.Numberformat.Format = "@";

                    // Numbers
                    //sheet.SetValue("A1", "Numbers");
                    //sheet.Cells["B1"].Value = 15.32;
                    //sheet.Cells["B1"].Style.Numberformat.Format = "#,##0.00";

                    // Percentage
                    //sheet.Cells["C1"].Value = 0.5;
                    //sheet.Cells["C1"].Style.Numberformat.Format = "0%";


                    // Money
                    //sheet.Cells["A2"].Value = "Moneyz";
                    //sheet.Cells["B2,D2"].Value = 15000.23D;
                    //sheet.Cells["C2,E2"].Value = -2000.50D;
                    //sheet.Cells["B2:C2"].Style.Numberformat.Format = "#,##0.00 [$€-813];[RED]-#,##0.00 [$€-813]";
                    //sheet.Cells["D2:E2"].Style.Numberformat.Format = "[$$-409]#,##0";

                    // DateTime
                    //sheet.Cells["A3"].Value = "Timey Wimey";
                    //sheet.Cells["B3"].Style.Numberformat.Format = "yyyy-mm-dd";
                    //sheet.Cells["B3"].Formula = $"=DATE({DateTime.Now:yyyy,MM,dd})";
                    //sheet.Cells["C3"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.FullDateTimePattern;
                    //sheet.Cells["C3"].Value = DateTime.Now;
                    //sheet.Cells["D3"].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                    //sheet.Cells["D3"].Value = DateTime.Now;

                    // A hyperlink (mailto: works also)
                    //sheet.Cells["C25"].Hyperlink = new Uri("https://itenium.be", UriKind.Absolute);
                    //sheet.Cells["C25"].Value = "Visit us";

                    //sheet.Cells["C25"].Style.Font.UnderLine = true;

                    //sheet.Cells["Z1"].Clear();

                    sheet.Cells.AutoFitColumns();

                    try //если окно диалога закрыть и не выбрать путь
                    {
                        package.SaveAs(new FileInfo(path));
                    }
                    catch(Exception ex) { }
                }
            }

        }

        private void BactToMain_Click(object sender, RoutedEventArgs e)//возврат к mainWidnod с последующим закрытием окна PaysWindow
        {   
            MainWindow mainWindow = new MainWindow();

            mainWindow.SetNum(SetSumCredit,SetPercentCredit,SetMonthQuantity,checkInSet);//Возвращаю вводные значения в MainWindow
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
            AnnuitExcel = pay;
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
            DiferentExcel = pay;
        }

    }
}
