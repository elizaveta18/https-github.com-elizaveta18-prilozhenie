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
using ClassLibrary1;

namespace Anketa_01._01__1_
{
    /// <summary>
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class List : Page
    {
        public List<auth> Anketa_01;
        List<users> users;
        List<users> l1;
        public List()
        {
            InitializeComponent();
            users = DB.Base.users.ToList();
            lbUsersList.ItemsSource = users;
            lbGenderFilter.ItemsSource = DB.Base.genders.ToList();
            lbGenderFilter.SelectedValuePath = "id";
            lbGenderFilter.DisplayMemberPath = "gender";
            l1 = users;
        }      
        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = Convert.ToInt32(lb.Uid);
            lb.ItemsSource = DB.Base.users_to_traits.Where(x => x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";
        }       
        private void Filter(object sender, RoutedEventArgs e)
        {
            //по строкам
            try
            {
                int start = Convert.ToInt32(txtOT.Text) - 1;
                int finish = Convert.ToInt32(txtDO.Text);
                l1 = users.Skip(start).Take(finish - start).ToList();

            }
            catch
            {
                //null
            }
            //по полу
            try
            {
                if (lbGenderFilter.SelectedIndex != -1)
                    l1 = l1.Where(x => x.gender == Convert.ToInt32(lbGenderFilter.SelectedValue)).ToList();

            }
            catch
            {
                //null
            }

            //по имени
            try
            {
                if (txtNameFilter.Text != "")
                {
                    l1 = l1.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();
                }
            }
            catch
            {
                //null
            }

            lbUsersList.ItemsSource = l1;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;//в качестве источника данных исходный список
            lbGenderFilter.SelectedIndex = -1; //сбрасываем выбранный элемент списка
            txtNameFilter.Text = "";//сбрасываем фильтр на строку
            txtOT.Text = "";
            txtDO.Text = "";
        }

        private void btnNaz_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.GoBack();
        }
        private void Udal_Click(object sender, RoutedEventArgs e)
        { 
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя из системы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            users Udal = (users)lbUsersList.SelectedItem;
            auth SelectedUser = DB.Base.auth.FirstOrDefault(x => x.id == Udal.id);
            DB.Base.auth.Remove(SelectedUser);
            DB.Base.SaveChanges();
            MessageBox.Show("Выбранный пользователь удален");
            TimeSpan.FromSeconds(3);
            lbUsersList.ItemsSource = DB.Base.users.ToList();
        }
        private void red_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int id = Convert.ToInt32(b.Uid);
            auth tUser = DB.Base.auth.FirstOrDefault(x => x.id == id);
            User.frmMain.Navigate(new Page5(tUser));
            lbUsersList.ItemsSource = DB.Base.users.ToList();
        }

        private void tbStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
        int currpg = 1;
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock tb = (TextBlock)sender;
                int countList = users.Count;
                int countzap = Convert.ToInt32(txtPageCount.Text);
                int countpage = countList / countzap;

                switch (tb.Uid)
                {
                    case "prev":
                        currpg--;
                        break;
                    case "1":
                        if (currpg < 3) currpg = 1;
                        else if (currpg > countpage) currpg = countpage - 4;
                        else currpg -= 2;
                        break;
                    case "2":
                        if (currpg < 3) currpg = 2;
                        else if (currpg > countpage) currpg = countpage - 3;
                        else currpg -= 1;
                        break;
                    case "3":
                        if (currpg < 3) currpg = 3;
                        else if (currpg > countpage) currpg = countpage - 2;
                        break;
                    case "4":
                        if (currpg < 3) currpg = 4;
                        else if (currpg > countpage) currpg = countpage - 1;
                        else currpg++;
                        break;
                    case "5":
                        if (currpg < 3) currpg = 5;
                        else if (currpg > countpage) currpg = countpage;
                        else currpg += 2;
                        break;
                    case "next":
                        currpg++;
                        break;
                    default:
                        currpg = 1;
                        break;
                }
                if (currpg < 3)
                {
                    txt1.Text = " 1 ";
                    txt2.Text = " 2 ";
                    txt3.Text = " 3 ";
                    txt4.Text = " 4 ";
                    txt5.Text = " 5 ";
                }
                else if (currpg > countpage - 2)
                {
                    txt1.Text = " " + (countpage - 4).ToString() + " ";
                    txt2.Text = " " + (countpage - 3).ToString() + " ";
                    txt3.Text = " " + (countpage - 2).ToString() + " ";
                    txt4.Text = " " + (countpage - 1).ToString() + " ";
                    txt5.Text = " " + (countpage).ToString() + " ";

                }
                else
                {
                    txt1.Text = " " + (currpg - 2).ToString() + " ";
                    txt2.Text = " " + (currpg - 1).ToString() + " ";
                    txt3.Text = " " + (currpg).ToString() + " ";
                    txt4.Text = " " + (currpg + 1).ToString() + " ";
                    txt5.Text = " " + (currpg + 2).ToString() + " ";

                }
                txtCurentPage.Text = "Текущая страница: " + (currpg).ToString();

                if (currpg < 1) currpg = 1;
                if (currpg >= countpage) currpg = countpage;

                l1 = users.Skip(currpg * countzap - countzap).Take(countzap).ToList();
                lbUsersList.ItemsSource = l1;
            }
            catch
            {
                //null
            }
        }
        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtPageCount.Text == "")
                {
                    l1 = users.ToList();
                }
                else
                    l1 = users.Take(Convert.ToInt32(txtPageCount.Text)).ToList();

                lbUsersList.ItemsSource = l1;
            }
            catch
            {
                //null
            }
        }

        private void btnVoz_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            DateTime[] mas = new DateTime[users.Count];
            foreach (users us in users)
            {
                mas[i] = us.dr;
                i++;
            }

            MessageBox.Show("Средний возраст: " + Func.AgeAVG(mas).ToString());
        }
    }
}