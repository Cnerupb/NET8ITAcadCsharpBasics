using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Utilities;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for PasswordChangeWindow.xaml
    /// </summary>
    public partial class PasswordChangeWindow : Window
    {
        private readonly int _userID;
        public PasswordChangeWindow(int userID)
        {
            InitializeComponent();
            _userID = userID;
        }

        private async void Change_Button_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = txtOldPassword.Password;
            string newPassword = txtNewPassword.Password;
            string newPasswordConfirm = txtNewPasswordConfirm.Password;

            using (var context = new Lysakovskiy_A_SEntities())
            {
                try
                {
                    var user = context.Users.Where(u => u.id == _userID).FirstOrDefault();

                    if (user == null)
                    {
                        MessageBox.Show("Пользователь не найден в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                        return;
                    }

                    if (!PasswordHasher.VerifyPassword(currentPassword, user.password))
                    {
                        MessageBox.Show("Неверный текущий пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (newPassword != newPasswordConfirm)
                    {
                        MessageBox.Show("Неверный новый пароль и подтверждение пароля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(newPassword))
                    {
                        MessageBox.Show("Все поля обязательны к заполнению", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (newPassword.Contains(" "))
                    {
                        MessageBox.Show("Пароль не должен содержать пробельные символы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    user.is_first_login = false;
                    user.password = PasswordHasher.HashPassword(newPassword);
                    await context.SaveChangesAsync();

                    MessageBox.Show("Новый пароль сохранён", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
