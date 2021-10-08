using System;
using System.IO;
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

namespace Anketa_01._01__1_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        people[] pip = new people[1];
        int ind = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        public struct people
        {
            public string name;
            public string dr;
            public string gender;
            public string dopinfo;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string dop = "";
            people p1 = new people();
            p1.name = nameTextBox.Text;
            p1.dr = dateBirth.ToString();
            ListBoxItem li = (ListBoxItem)lbGender.SelectedItem;
            p1.gender = li.Content.ToString();
            if (good.IsChecked == true)
            {
                dop += good.Content;
            }
            if (zloi.IsChecked == true)
            {
                dop += zloi.Content;
            }
            if (psih.IsChecked == true)
            {
                dop += psih.Content;
            }
            p1.dopinfo = dop;
            using (BinaryWriter wu = new BinaryWriter(File.Open("export.txt", FileMode.Append)))
            {
                wu.Write(p1.name);
                wu.Write(p1.dr);
                wu.Write(p1.gender);
                wu.Write(p1.dopinfo);
            }
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pip = new people[1];
            using(BinaryReader br = new BinaryReader(File.Open("export.txt", FileMode.Open)))
            {
                
                int i = 0;
                while (br.PeekChar() > -1)
                {
                    pip[i].name = br.ReadString();
                    pip[i].dr = br.ReadString();
                    pip[i].gender = br.ReadString();
                    pip[i].dopinfo = br.ReadString();
                    i++;
                    Array.Resize(ref pip, pip.Length + 1);
                }
            }
            
            n.Text = pip[ind].name;
            Na.Text = pip[ind].dr;
            Nav.Text = pip[ind].gender;
            Navi.Text = pip[ind].dopinfo;
            ind++;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            n.Text = pip[ind].name;
            Na.Text = pip[ind].dr;
            Nav.Text = pip[ind].gender;
            Navi.Text = pip[ind].dopinfo;
            ind++;
        }
    }
}
