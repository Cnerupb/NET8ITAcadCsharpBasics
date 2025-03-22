using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for BookingPage.xaml
    /// </summary>
    public partial class BookingPage : Page
    {
        public BookingPage()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings()
        {
            using(var context = new Lysakovskiy_A_SEntities())
            {
                var bookings = context.Reservations
                    .Include("Guests")
                    .Include("Rooms")
                    .ToList();

                if (bookings.Count == 0)
                {
                    MessageBox.Show("Нет доступных записей для отображения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedBookings = bookings
                    .Select(r => new
                        {
                            r.id,
                            full_name = r.Guests != null ? r.Guests.first_name + " " + r.Guests.last_name : "Не найдено",
                            room_number = r.Rooms != null ? r.Rooms.number.ToString() : "Не найдено",
                            r.check_in_date,
                            r.check_out_date,
                            total_price = r.total_price != null ? r.total_price.ToString() : "Не найдено",
                            r.Status.name,
                        })
                    .ToList();

                BookingDataGrid.ItemsSource = selectedBookings;
            }
        }

        private void Button_Add_Booking_Click(object sender, RoutedEventArgs e)
        {
            AddBookingWindow addBookingWindow = new AddBookingWindow();
            addBookingWindow.Show();
        }

        private void Button_Refresh_Table_Click(object sender, RoutedEventArgs e)
        {
            LoadBookings();
            MessageBox.Show("Таблица обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
