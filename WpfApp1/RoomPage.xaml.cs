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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for RoomPage.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        public RoomPage()
        {
            InitializeComponent();
            LoadRooms();
            FillRoomsStats();
        }

        private void LoadRooms()
        {
            using (var context = new Lysakovskiy_A_SEntities())
            {
                var rooms = context.Rooms.ToList();
                RoomDataGrid.ItemsSource = rooms;
            }
        }

        private void FillRoomsStats()
        {
            using (var context = new Lysakovskiy_A_SEntities())
            {
                var rooms = context.Rooms.ToList();
                var roomAmount = rooms.Count();
                var freeRooms = rooms.Where(r => r.Status.name == "clean").Count();
                var roomUsage = 1 - ((double)freeRooms / (double)roomAmount);

                txtRoomStats.Text = $"Загруженность комнат: {string.Format("{0:f2}", roomUsage)} %";
            }
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Lysakovskiy_A_SEntities())
            {
                LoadRooms();
                FillRoomsStats();
            }
        }
    }
}
