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
    /// Interaction logic for GuestPage.xaml
    /// </summary>
    public partial class GuestPage : Page
    {
        private Lysakovskiy_A_SEntities _context;
        public GuestPage()
        {
            InitializeComponent();
            _context = new Lysakovskiy_A_SEntities();
            LoadData();
        }

        private void LoadData()
        {
            var guests = _context.Guests.ToList();
            GuestDataGrid.ItemsSource = guests;
        }

        private void Button_Refresh_Table_Click(object sender, RoutedEventArgs e)
        {
            var guests = _context.Guests.ToList();
            GuestDataGrid.ItemsSource = guests;
        }
    }
}
