using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            using (var context = new Lysakovskiy_A_SEntities())
            {
                List<Users> users = await context.Users.Where(u => u.role != "admin").ToListAsync();
                Users.ItemsSource = users; // Привязываем данные к DataGrid
            }
        }

        private void Button_Add_User_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
        }

        private async void Button_Unblock_Click(object sender, RoutedEventArgs e)
        {
            if (Users.SelectedItem is Users selectedUser)
            {
                using (var context = new Lysakovskiy_A_SEntities())
                {
                    try
                    {
                        var users = await context.Users.FindAsync(selectedUser.id);

                        if (users != null)
                        {
                            users.is_locked = false;
                            users.last_login_date = null;
                            await context.SaveChangesAsync();
                            LoadUsers();
                            MessageBox.Show($"Пользователь разблокирован", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Произошла непредвиденная ошибка {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            } 
            else
            {
                MessageBox.Show($"Выберите пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Save_Changes_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            MessageBox.Show($"Изменения сохранены", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
    }
}
