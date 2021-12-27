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
using Anketa_01._01__1_.pages;

namespace Anketa_01._01__1_
{
    /// <summary>
    /// Логика взаимодействия для Media.xaml
    /// </summary>
    public partial class Media : Page
    {
        public Media()
        {
            InitializeComponent();
        }

        private void btnPr_Click(object sender, RoutedEventArgs e)
        {
            med.Play();

        }

        private void btnPa_Click(object sender, RoutedEventArgs e)
        {
            med.Pause();

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            med.Stop();

        }

        private void myVideo_Loaded(object sender, RoutedEventArgs e)
        {
            Sl.Minimum = 0;
            Sl.Maximum = med.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Sl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                TimeSpan duration = med.NaturalDuration.TimeSpan;//получаем длительность видео в формате timespan
                double d = duration.TotalMilliseconds;// длительность в миллисекундах           
                //med.Position = TimeSpan.FromMilliseconds(d * Sl.Value);//установка конкретного значения
                TimeSpan newPosition = TimeSpan.FromSeconds(med.NaturalDuration.TimeSpan.TotalSeconds * e.NewValue);
                med.Position = newPosition;
        }

        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void btnNaz_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.Navigate(new Page2());
        }
    }
}
