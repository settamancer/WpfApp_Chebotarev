using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
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
using WpfApp_Chebotarev;
using System.Linq;


namespace WpfApp_Chebotarev
{

    public partial class AdminWindow : Window
    {
    
        public ObservableCollection<User> Users { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
            LoadUsers();
            DataGridUsers.ItemsSource = Users;
        }

        private void LoadUsers()
        {
            using (var context = new DBEntities())
            {
                var usersFromDb = context.Users.ToList();
                Users.Clear();
                foreach (var user in usersFromDb)
                {
                    Users.Add(user); 
                }
            }
        }

        private void Button_Click_AddUser(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUser(); 
            if (addUserWindow.ShowDialog() == true) 
            {
                var newUser = addUserWindow.NewUser; 
                if (newUser == null)
                {
                    MessageBox.Show("Новый пользователь не был создан или возвращен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    using (var context = new DBEntities())
                    {
                        context.Users.Add(newUser); 
                        context.SaveChanges();
                    }

                    Users.Add(newUser); 
                    MessageBox.Show($"Пользователь '{newUser.username}' успешно добавлен.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    Console.WriteLine($"Добавлен пользователь: {newUser.username}");
                    Console.WriteLine($"Текущее количество в Users: {Users.Count}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_Delete_User(object sender, RoutedEventArgs e)
        {
            var user = DataGridUsers.SelectedItem as User;

            if (user == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подтверждение удаления
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пользователя '{user.username.Trim()}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return; 

            try
            {
                using (var context = new DBEntities())
                {
                   
                    var userToDelete = context.Users.Find(user.id);

                    if (userToDelete != null)
                    {
                        context.Users.Remove(userToDelete);
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                Users.Remove(user); 
                MessageBox.Show($"Пользователь '{user.username.Trim()}' успешно удалён.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_Unblock(object sender, RoutedEventArgs e)
        {
            User selectedUser = DataGridUsers.SelectedItem as User;

            if (selectedUser == null)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для разблокировки.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return; 
            }

            var result = MessageBox.Show(
                $"Вы уверены, что хотите разблокировать пользователя '{selectedUser.username.Trim()}'?", "Подтверждение разблокировки", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) 
                return;
            try
            {
                using (var context = new DBEntities())
                {
                    var userToUnlock = context.Users.Find(selectedUser.id);
                    if (userToUnlock != null)
                    {
                        userToUnlock.IsLocked = false;
                        userToUnlock.FailedLoginAttempts = 0;
                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                selectedUser.IsLocked = false;
                selectedUser.FailedLoginAttempts = 0;

                MessageBox.Show($"Пользователь '{selectedUser.username.Trim()}' успешно разблокирован", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при разблокировке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}