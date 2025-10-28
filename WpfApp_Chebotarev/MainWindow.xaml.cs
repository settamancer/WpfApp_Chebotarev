
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using WpfApp_Chebotarev;

namespace WpfApp_Chebotarev
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
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль");
                return;
            }
            using (var context = new DBEntities())
            {
                var user = await context.Users
                    .Where(u => u.username == username)
                    .FirstOrDefaultAsync();

                Debug.WriteLine("Введённый логин: " + username);
                Debug.WriteLine("Введённый пароль: '" + password + "'");

                if (user != null)
                {
                    Debug.WriteLine("Пароль пользователя из базы: '" + user.password + "'");
                }
                else
                {
                    Debug.WriteLine("Пользователь не найден!");
                }
                if (user.IsLocked.HasValue && user.IsLocked.Value)
{
    MessageBox.Show("Вы заблокированы. Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    return;
}
if (user.LastLoginDate.HasValue && (DateTime.Now - user.LastLoginDate.Value).TotalDays > 30 && user.role != "Admin")
{
    user.IsLocked = true;
    await context.SaveChangesAsync();
    MessageBox.Show("Вы заблокированы. Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    return;
}
                // Debug
                Debug.WriteLine($"Сравнение: [{user.password.Trim()}] == [{password}]");

if (user.password.Trim() == password.Trim())
{
    user.LastLoginDate = DateTime.Now;
    user.FailedLoginAttempts = 0;
    await context.SaveChangesAsync();
    MessageBox.Show("Вы успешно авторизовались!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);


    if (user.IsFirstLogin.HasValue && user.IsFirstLogin.Value)
    {
        var changePasswordWindow = new ChangePasswordWindow(user.id);
        changePasswordWindow.Owner = this;
        changePasswordWindow.ShowDialog();

    }
    else
    {
        if (user.role == "Admin")
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }
        else if (user.role == "Manager")
        {
            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.Show();
        }
        else if (user.role == "Cleaning_staff")
        {
            clSchedule cleaningStaffWin = new clSchedule();
            cleaningStaffWin.Show();
        }
        else
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        this.Close();
    }
}
else
{
    user.FailedLoginAttempts++;
    if (user.FailedLoginAttempts == 5)
    {
        user.IsLocked = true;
        MessageBox.Show($"Вы заблокированы. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    else
    {
        int attemptsLeft = 3 - (user.FailedLoginAttempts ?? 0);
        MessageBox.Show($"Неправильный пароль. Осталось попыток {attemptsLeft}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    await context.SaveChangesAsync();
}

            }
            

        }
    }
}
