using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AddBookingWindow.xaml
    /// </summary>
    public partial class AddBookingWindow : Window
    {
        Lysakovskiy_A_SEntities _context;
        public AddBookingWindow()
        {
            InitializeComponent();
            _context = new Lysakovskiy_A_SEntities();
            LoadFloors();
            LoadStatus();
        }

        async void LoadFloors()
        {
            using (var context = new Lysakovskiy_A_SEntities())
            {
                var floors = await context.Rooms.Select(r => r.floor).Distinct().ToListAsync();
                floorComboBox.ItemsSource = floors;
            }
        }

        async void LoadRooms(int floor)
        {
            using (var context = new Lysakovskiy_A_SEntities())
            {
                var rooms = await context.Rooms.Distinct().Where(r => r.floor == floor && r.Status.name == "clean").Select(r => r.number).ToListAsync();
                roomComboBox.ItemsSource = rooms;
            }
        }

        void LoadStatus()
        {
            var statusList = new List<string>() { "free", "busy" };
            StatusComboBox.ItemsSource = statusList;
        }

        private async void Button_Add_Booking_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string document = txtDocumentNumber.Text;
            string checkInDate = checkInDatePicker.SelectedDate.ToString();
            string checkOutDate = checkOutDatePicker.SelectedDate.ToString();
            string floor = floorComboBox.SelectedItem.ToString();
            string roomNumber = roomComboBox.SelectedItem.ToString();
            string status = StatusComboBox.SelectedItem.ToString();

            bool isFirstNameEmpty = string.IsNullOrEmpty(firstName) | string.IsNullOrWhiteSpace(firstName);
            bool isLastNameEmpty = string.IsNullOrEmpty(lastName) | string.IsNullOrWhiteSpace(lastName);
            bool isEmailEmpty = string.IsNullOrEmpty(email) | string.IsNullOrWhiteSpace(email);
            bool isPhoneNumberEmpty = string.IsNullOrEmpty(phoneNumber) | string.IsNullOrWhiteSpace(phoneNumber);
            bool isDocumentNumberEmpty = string.IsNullOrEmpty(document) | string.IsNullOrWhiteSpace(document);
            bool isFloorEmpty = string.IsNullOrEmpty(floor) | string.IsNullOrWhiteSpace(floor);
            bool isRoomEmpty = string.IsNullOrEmpty(roomNumber) | string.IsNullOrWhiteSpace(roomNumber);
            bool isCheckInDateEmpty = string.IsNullOrEmpty(checkInDate) | string.IsNullOrWhiteSpace(checkInDate);
            bool isCheckOutDateEmpty = string.IsNullOrEmpty(checkOutDate) | string.IsNullOrWhiteSpace(checkOutDate);
            bool isStatusEmpty = string.IsNullOrEmpty(status) | string.IsNullOrWhiteSpace(status);

            if (isLastNameEmpty | isFirstNameEmpty | isEmailEmpty | isPhoneNumberEmpty | isDocumentNumberEmpty | isFloorEmpty | isRoomEmpty | isCheckInDateEmpty | isCheckOutDateEmpty | isStatusEmpty)
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
            bool isPhoneNumberIncorrect = !Regex.IsMatch(phoneNumber, phoneNumberRegex);
            if (isPhoneNumberIncorrect)
            {
                MessageBox.Show("Номер телефона не соответствует российскому формату (+7-xxx-xxx-xx-xx)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isCheckInOutIncorrect = !(DateTime.TryParse(checkInDate, out var cid) ? DateTime.TryParse(checkOutDate, out var cod) ? (cid < cod) : false : false);
            if (isCheckInOutIncorrect)
            {
                MessageBox.Show("Неверные даты въезда или выезда", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new Lysakovskiy_A_SEntities())
            {
                var newGuest = new Guests() {
                    first_name=firstName, last_name=lastName, email=email, phone=phoneNumber, document_number=document
                };
                var newGuestInDb = context.Guests.Add(newGuest);

                await context.SaveChangesAsync();

                var room = await context.Rooms.Where(r => r.floor.ToString().Equals(floor) && r.number.ToString().Equals(roomNumber)).FirstOrDefaultAsync();
                if (room is null)
                {
                    MessageBox.Show($"Неверные номер этажа или комнаты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedStatus = await context.Status.Where(u => u.name == status).Select(u => u.id).FirstOrDefaultAsync();

                var newReservation = new Reservations() { 
                    guest_id=newGuestInDb.id, room_id=room.id, check_in_date=DateTime.Parse(checkInDate), check_out_date=DateTime.Parse(checkOutDate), status_id=selectedStatus
                };
                context.Reservations.Add(newReservation);

                await context.SaveChangesAsync();

                MessageBox.Show("Бронирование добавлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void floorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFloor = floorComboBox.SelectedItem.ToString();

            if (!(int.TryParse(selectedFloor, out var floor))) return;

            LoadRooms(floor);
        }
    }
}
