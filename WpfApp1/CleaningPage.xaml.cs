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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CleaningPage.xaml
    /// </summary>
    public partial class CleaningPage : Page
    {
        Lysakovskiy_A_SEntities _context;
        public CleaningPage()
        {
            InitializeComponent();

            _context = new Lysakovskiy_A_SEntities();
            LoadCleaningStaff();
            LoadRooms();
            LoadDataGrid();
        }

        async void LoadCleaningStaff()
        {
            var cleaningStaff = await _context.Users
                .Where(u => u.role == "cleaning")
                .Select(u => new { full_name = (u.first_name ?? "") + " " + (u.last_name ?? "") } )
                .Select(u => u.full_name)
                .ToListAsync();
            CleaningStaffComboBox.ItemsSource = cleaningStaff;
        }

        async void LoadRooms()
        {
            var rooms = await _context.Rooms.Where(r => r.Status.name == "dirty").Select(r => r.number).ToListAsync();
            RoomComboBox.ItemsSource = rooms;
        }

        async void LoadDataGrid()
        {
            var includeData = _context.Cleaning_Schedule.Include("Users").Include("Rooms");
            var data = await includeData.Select(d => new
            {
                d.cleaning_date,
                room_number = d.Rooms.number,
                employee_full_name = d.Users.first_name + d.Users.last_name,
                d.Status.name,
            }
            ).ToListAsync();
            
            CleaningDataGrid.ItemsSource = data;
        }
        private async void Button_AddSchedule_Click(object sender, RoutedEventArgs e)
        {
            var cleaningStaffFullName = CleaningStaffComboBox.SelectedItem.ToString();
            var roomNumber = RoomComboBox.SelectedItem.ToString();
            var cleaningDate = DatePicker.SelectedDate;

            bool isCleaningStaffFullNameEmpty = string.IsNullOrEmpty(cleaningStaffFullName) | string.IsNullOrWhiteSpace(cleaningStaffFullName);
            bool isRoomNumberFullNameEmpty = string.IsNullOrEmpty(roomNumber) | string.IsNullOrWhiteSpace(roomNumber);
            bool isCleaningDateNull = cleaningDate.HasValue ? false : true;

            if (isCleaningStaffFullNameEmpty || isRoomNumberFullNameEmpty || isCleaningDateNull)
            {
                MessageBox.Show("Все поля обязательны к заполнению и не должны содержать пустые значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var firstName = cleaningStaffFullName.Split()[0];
            var lastName = cleaningStaffFullName.Split()[1];
            var staff = await _context.Users.Where(u => u.first_name == firstName && u.last_name == lastName && u.role == "cleaning").FirstOrDefaultAsync();
            var room = await _context.Rooms.Where(r => r.number.ToString() == roomNumber).FirstOrDefaultAsync();

            var cleaningStatus = await _context.Status.Where(s => s.name == "cleanup").Select(s => s.id).FirstOrDefaultAsync();

            var cleaningSchedule = new Cleaning_Schedule() { room_id = room.id, cleaning_date = cleaningDate.Value, user_id = staff.id, status_id=cleaningStatus};
            _context.Cleaning_Schedule.Add(cleaningSchedule);

            await _context.SaveChangesAsync();

            MessageBox.Show("Уборка назначена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_RefreshTable_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

    }
}
