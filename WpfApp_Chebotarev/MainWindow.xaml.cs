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
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace WpfApp_Rogatina_MS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин или пароль");
                return;
            }
            else
            {
                user.Failed_login++;
                if (user.Failed_login == 3)
                {
                    user.IsLocked = true;
                    MessageBox.Show($"Вы заблокированы. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int attemptsLeft = 3 - (username.Failed_login ?? 0);
                    MessageBox.Show($"Неправильный пароль. Осталось попыток {attemptsLeft}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                }
                await context.SaveChangesAsync();
            }
            using (var context = new RogatinaEntities())
            {
                var user = await context.Users
                    .Where(u => u.username == username)
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    MessageBox.Show("Такого ползователя не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.IsLocked.HasValue && user.IsLocked.Value) 
                {
                    MessageBox.Show("Вы заблокированы. Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Last_login.HasValue && (DateTime.Now - user.Last_login.Value).TotalDays > 30 && user.role != "Admin") 
                {
                    user.IsLocked = true;
                    await context.SaveChangesAsync();
                    MessageBox.Show("Вы заблокированы. Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.password == password) 
                { 
                    user.Last_login = DateTime.Now;
                    user.Failed_login = 0;
                    await context.SaveChangesAsync();
                    MessageBox.Show("Вы успешно авторизовались!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    if (user.role == "Admin")
                    {
                        Admin adminWindow = new Admin();
                        adminWindow.Show();
                    }
                    else if (user.role == "Management")
                    {
                        ManagerWindow managerWindow = ManagerWindow();
                        managerWindow.Show();
                    }
                    else if (user.role == "Cleaning_staff")
                    {
                        CleaningStaffWindow cleaningStaffWin = CleaningStaffWindow();
                        cleaningStaffWin.Show();
                    }
                    else
                    {
                        MainWindow window = new MainWindow();
                        window.Show();
                    }
                    this.Close();
                }
            }
            
        }
    }
}
