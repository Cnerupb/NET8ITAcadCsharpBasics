using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private async void Button_Add_User_Click(object sender, RoutedEventArgs e)
        {
            string lastName = txtLastName.Text;
            string firstName = txtFirstName.Text;
            string userName = txtUserName.Text;
            string role = txtRole.Text;
            string email = txtEmail.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string password = txtPassword.Password;

            bool isLastNameEmpty = string.IsNullOrEmpty(lastName) | string.IsNullOrWhiteSpace(lastName);
            bool isFirstNameEmpty = string.IsNullOrEmpty(firstName) | string.IsNullOrWhiteSpace(firstName);
            bool isUserNameEmpty = string.IsNullOrEmpty(userName) | string.IsNullOrWhiteSpace(userName);
            bool isRoleEmpty = string.IsNullOrEmpty(role) | string.IsNullOrWhiteSpace(role);
            bool isEmailEmpty = string.IsNullOrEmpty(email) | string.IsNullOrWhiteSpace(email);
            bool isPhoneNumberEmpty = string.IsNullOrEmpty(phoneNumber) | string.IsNullOrWhiteSpace(phoneNumber);
            bool isPasswordEmpty = string.IsNullOrEmpty(password) | string.IsNullOrWhiteSpace(password);

            if (isLastNameEmpty | isFirstNameEmpty | isUserNameEmpty | isRoleEmpty | isEmailEmpty | isPhoneNumberEmpty | isPasswordEmpty)
            {
                MessageBox.Show("Все поля обязательны к заполнению не пустыми символами", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string emailRegex = "^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(?:\\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@(?:[a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\\.)*(?:aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$";
            bool isEmailInCorrect = !Regex.IsMatch(email, emailRegex);
            if (isEmailInCorrect)
            {
                MessageBox.Show("Email не соответствует формату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string phoneNumberRegex = "^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$";
            bool isPhoneNumberInCorrect = !Regex.IsMatch(phoneNumber, phoneNumberRegex);
            if (isPhoneNumberInCorrect)
            {
                MessageBox.Show("Номер телефона не соответствует российскому формату (+7-xxx-xxx-xx-xx)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isPasswordInCorrect = password.Contains(" ");
            if (isPasswordInCorrect)
            {
                MessageBox.Show("Пароль не должен содержать пробелов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new Lysakovskiy_A_SEntities())
            {
                try
                {
                    var sameUserNameUser = await context.Users.Where(u => u.user_name == userName).FirstAsync();
                    if (sameUserNameUser != null)
                    {
                        MessageBox.Show($"Пользователь с таким именем пользователя сущесвуют", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var user = new Users();
                    user.last_name = lastName;
                    user.first_name = firstName;
                    user.user_name = userName;
                    user.role = role;
                    user.email = email;
                    user.phone = phoneNumber;
                    user.password = password;

                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                    MessageBox.Show($"Пользователь добавлен", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
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
