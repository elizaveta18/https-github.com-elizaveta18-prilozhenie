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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Anketa_01._01__1_.pages;

namespace Anketa_01._01__1_.pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            DoubleAnimation DA = new DoubleAnimation();
            DA.Duration = new TimeSpan(0, 0, 1);
            DA.From = 100;
            DA.To = 500;
            DA.Duration = TimeSpan.FromSeconds(2);
            DA.AutoReverse = true;
            DA.RepeatBehavior = RepeatBehavior.Forever;
            btnMir.BeginAnimation(WidthProperty, DA);
        }
        string kode;
        bool flagKode = false;

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth currentUser = DB.Base.auth.FirstOrDefault(x => x.login == txtLogin.Text && x.password == txtPassword.Password);
                if (currentUser != null)
                {//сюда напишем алгоритм перехода на страницу в зависимости от роли
                    if (flagKode == false)
                    {
                        generateKey();
                        flagKode = true;
                    }
                    else if (kode == txtKod.Text)
                    {
                        switch (currentUser.role)
                        {
                            case 1:
                                MessageBox.Show("Вы вошли как администратор");
                                User.frmMain.Navigate(new List());
                                break;
                            case 2:
                            default:
                                MessageBox.Show("Вы вошли как обычный пользователь");
                                User.frmMain.Navigate(new Page4(currentUser));
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Такого пользователя нет");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.Navigate(new Page1());
        }

        private void btMir_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.Navigate(new Media());

        }

        Random random = new Random();
        private void generateKey()
        {
            imgRefresh.IsEnabled = false;
            kode = "";

            for (int i = 0; i < 8; i++)
            {
                kode += ((char)random.Next(33, 122)).ToString();
            }
            txtKod.Text = kode;
            MessageBox.Show(kode, "введите код в соответствующее поле на форме.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            imgRefresh.IsEnabled = true;
            kode = ((char)random.Next(33, 122)).ToString();
            // MessageBox.Show("время вышло");
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            generateKey();
            flagKode = true;
        }

        private void btnGraph_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.Navigate(new Graphic());
        }
    }
}
