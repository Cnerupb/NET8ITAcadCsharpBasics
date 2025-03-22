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
    /// Interaction logic for CleaningInfoWindow.xaml
    /// </summary>
    public partial class CleaningInfoWindow : Window
    {
        Lysakovskiy_A_SEntities _context;
        int _userID;
        public CleaningInfoWindow(int userID)
        {
            InitializeComponent();
            _context = new Lysakovskiy_A_SEntities();
            _userID = userID;
            LoadRooms();
        }

        async void LoadRooms()
        {
            var schedule = await _context.Cleaning_Schedule
                .Include("Status")
                .Include("Rooms")
                .Include("Users")
                .Where(cs => cs.Users.id == _userID && cs.Status.name == "assigned")
                .Select(cs => new {cs.id, cs.cleaning_date, cs.Rooms, cs.Status })
                .ToListAsync();

            CleaningScheduleDataGrid.ItemsSource = schedule;
        }

        private async void Button_DoneRoom_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedSchedule = button?.DataContext as dynamic;

            if (selectedSchedule == null)
            {
                MessageBox.Show("Не выбрана запись для изменения статуса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var scheduleEntry = await _context.Cleaning_Schedule.FindAsync(selectedSchedule.id);

            if (scheduleEntry == null)
            {
                MessageBox.Show("Запись не найдена в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            scheduleEntry.status_id = 1;

            _context.SaveChanges();
            _context = new Lysakovskiy_A_SEntities();
            MessageBox.Show("Статус успешно изменен на 'done'.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_RefreshTable_Click(object sender, RoutedEventArgs e)
        {
            LoadRooms();
        }
    }
}
