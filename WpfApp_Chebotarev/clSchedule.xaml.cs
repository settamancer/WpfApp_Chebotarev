using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp_Chebotarev
{
    
    public partial class clSchedule : Window
    {
        public clSchedule()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }

        private void RoomsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var room = e.Row.Item as Room;
            if (room == null) return;

            var checkBoxColumn = e.Column as DataGridCheckBoxColumn;
            var boundColumn = e.Column as DataGridBoundColumn;

            if (checkBoxColumn != null &&
                boundColumn != null &&
                boundColumn.Binding is Binding binding &&
                binding.Path != null &&
                binding.Path.Path == "status")
            {
                using (var context = new DBEntities())
                {
                    var dbRoom = context.Rooms.Find(room.id);
                    if (dbRoom != null)
                    {
                        dbRoom.status = room.status;
                        context.SaveChanges();
                    }
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?",
                                         "Подтверждение",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new DBEntities())
                {
                    foreach (var room in (RoomsDataGrid.ItemsSource as IEnumerable<Room>))
                    {
                        var dbRoom = context.Rooms.Find(room.id);
                        if (dbRoom != null)
                        {
                            dbRoom.status = room.status;
                        }
                    }
                    context.SaveChanges();
                }
                MessageBox.Show("Изменения успешно сохранены!", "Сохранено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

}

