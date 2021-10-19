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
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class List : Page
    {
        public async void Load()
        {
            while (true)
            {
                lbUsersList.ItemsSource = DB.Base.auth.ToList();
                await Task.Delay(1000);
            }
        }
        List<users> users;
        public List()
        {
            InitializeComponent();
            users = DB.Base.users.ToList();
            lbUsersList.ItemsSource = users;
            lbUsersList.ItemsSource = DB.Base.auth.ToList();
            Load();           
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            //senser содержит объект, который вызвал данное событие, но только у него объектный тип, надо преобразовать
            ListBox lb = (ListBox)sender;//lb содержит ссылку на тот список, который вызвал это событие
            int index = Convert.ToInt32(lb.Uid);//получаем ID элемента списка. в данном случае оно совпадает с id user
            lb.ItemsSource = DB.Base.users_to_traits.Where(x => x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";//показываем пользователю текстовое описание качества
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            int OT = Convert.ToInt32(txtOT.Text) - 1;//т.к. индексы начинаются с нуля
            int DO = Convert.ToInt32(txtDO.Text);
            List<users> lu1 = users.Skip(OT).Take(DO - OT).ToList();
            //skip - пропустить определенное количество записей
            //take - выбрать определенное количество записей
            lbUsersList.ItemsSource = lu1;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;//в качестве источника данных новый список
        }

        private void btnNaz_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.GoBack();
        }

        private void Udal_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя из системы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            auth SelectedUser = (auth)lbUsersList.SelectedItem;//сохраняем выбранную строку datagrid в отдельный объект
            DB.Base.auth.Remove(SelectedUser);//удаляем эту строку из модели
            DB.Base.SaveChanges();//синхронизируем изменения с сервером
            MessageBox.Show("Выбранный пользователь удален");//обратная связь с оператором программы
            lbUsersList.ItemsSource = DB.Base.auth.ToList();//обновить строки в datagrid
        }

        private auth SelectedUser()
        {
            return (auth)lbUsersList.SelectedItem;
        }

        private void red_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.Navigate(new Page5(SelectedUser()));
        }
    }
}
