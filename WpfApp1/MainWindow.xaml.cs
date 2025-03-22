using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
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
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) | string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин или пароль");
                return;
            }

            using (var context = new Lysakovskiy_A_SEntities())
            {
                var user = await context.Users
                    .Where(u => u.user_name == username)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    MessageBox.Show("Неверный логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!PasswordHasher.VerifyPassword(password, user.password))
                {
                    user.failed_login_attempts += 1;
                    if (user.role != "admin" && user.failed_login_attempts > 3)
                    {
                        user.is_locked = true;
                        await context.SaveChangesAsync();
                        MessageBox.Show("Вы заблокированы, обратитесь к администратору", "Доступ запрещён", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    await context.SaveChangesAsync();
                    MessageBox.Show($"Неверный пароль. Попытка {user.failed_login_attempts} из 3", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.role != "admin" && user.is_locked)
                {
                    MessageBox.Show("Вы заблокированы, обратитесь к администратору", "Доступ запрещён", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.role != "admin" && user.last_login_date.HasValue && (DateTime.Now - user.last_login_date.Value).TotalDays > 31)
                {
                    user.is_locked = true;
                    await context.SaveChangesAsync();
                    MessageBox.Show("Вы заблокированы из-за длительного отсутствия, обратитесь к администратору", "Доступ запрещён", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (PasswordHasher.VerifyPassword(password, user.password))
                {
                    user.last_login_date = DateTime.Now;
                    user.failed_login_attempts = 0;
                    await context.SaveChangesAsync();
                    MessageBox.Show("Вход выполнен успешно", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (user.is_first_login)
                    {
                        PasswordChangeWindow passwordChangeWindow = new PasswordChangeWindow(user.id);
                        passwordChangeWindow.Owner = this;
                        passwordChangeWindow.ShowDialog();
                    }
                    else
                    {
                        if (user.role == "admin")
                        {
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                        }
                        else if (user.role == "hotel-admin")
                        {
                            ManagerWindow managerWindow = new ManagerWindow();
                            managerWindow.Show();
                        }
                        else if (user.role == "cleaning")
                        {
                            CleaningInfoWindow cleaningInfoWindow = new CleaningInfoWindow(user.id);
                            cleaningInfoWindow.Show();
                        }
                        else
                        {
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                        }
                        this.Close();
                    }
                }
            }
        }
    }
}
