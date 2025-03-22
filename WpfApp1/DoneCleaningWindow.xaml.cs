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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for DoneCleaningWindow.xaml
    /// </summary>
    public partial class DoneCleaningWindow : Window
    {
        Lysakovskiy_A_SEntities _context;
        int _userID;
        public DoneCleaningWindow(int userID)
        {
            InitializeComponent();
            _context = new Lysakovskiy_A_SEntities();
            _userID = userID;
        }

        private void Button_DoneRoom_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = RoomComboBox.SelectedItem.ToString();
            if (selectedRoom == null)
            {
                MessageBox.Show("Выберите комнату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var schedule = _context.Cleaning_Schedule.FindAsync(_userID);


        }
    }
}
