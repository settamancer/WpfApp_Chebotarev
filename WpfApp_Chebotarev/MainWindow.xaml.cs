
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
        string password = txtPassword.Password;

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
                if (user == null)
                {
                    MessageBox.Show("Такого пользователя не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
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
if (user.password == password)
{
    user.LastLoginDate = DateTime.Now;
    user.FailedLoginAttempts = 0;
    await context.SaveChangesAsync();
    MessageBox.Show("Вы успешно авторизовались!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);


    if (user.IsFirstLogin.HasValue && user.IsFirstLogin.Value)
    {
        ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(user.id);
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
        else if (user.role == "Management")
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

        //private async void Button_Click_1(object sender, RoutedEventArgs e)
        //{

        //    // Получаем данные из полей
        //    string login = txtUsername.Text.Trim();
        //    string password = txtPassword.Password.Trim();


        //    if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        //    {
        //        MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    using (var context = new DBEntities()) // Или как называется ваш контекст
        //    {
        //        var user = await context.Users.FirstOrDefaultAsync(u => u.username == login && u.password == password);
              


        //        if (user == null)
        //        {
        //            MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        // Проверка блокировки по времени (если не админ)
        //        if (user.LastLoginDate.HasValue &&
        //            (DateTime.Now - user.LastLoginDate.Value).TotalDays > 30 &&
        //            user.role != "Admin")
        //        {
        //            user.IsLocked = true;
        //            await context.SaveChangesAsync();
        //            MessageBox.Show("Вы заблокированы. Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        // Проверка блокировки по количеству попыток
        //        if ((bool)user.IsLocked)
        //        {
        //            MessageBox.Show("Вы заблокированы. Обратитесь к Администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }
      
        //        // Проверка пароля
        //        if (user.password == password)
        //        {
        //            // Успешный вход
        //            user.LastLoginDate = DateTime.Now;
        //            user.FailedLoginAttempts = 0;
        //            await context.SaveChangesAsync();

        //            MessageBox.Show("Вы успешно авторизовались!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);

        //            // Проверяем, нужно ли менять пароль
        //            if (user.IsFirstLogin.HasValue && user.IsFirstLogin.Value)
        //            {
        //                ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(user.id);
        //                changePasswordWindow.Owner = this;
        //                changePasswordWindow.ShowDialog();
        //            }
        //            else
        //            {
        //                // Открываем окно по роли
        //                if (user.role == "Admin")
        //                {
        //                    AdminWindow adminWindow = new AdminWindow();
        //                    adminWindow.Show();
        //                }
        //                else if (user.role == "Management")
        //                {
        //                    var managerWindow = new ManagerWindow();
        //                    managerWindow.Show();
        //                }
        //                else if (user.role == "Cleaning_staff")
        //                {
        //                    var cleaningStaffWin = new clSchedule();
        //                    cleaningStaffWin.Show();
        //                }
        //            }

        //            this.Close(); // Закрываем MainWindow
        //        }
        //        else
        //        {
        //            // Неверный пароль
        //            user.FailedLoginAttempts++;
        //            if (user.FailedLoginAttempts == 10)
        //            {
        //                user.IsLocked = true;
        //                await context.SaveChangesAsync();
        //                MessageBox.Show("Вы заблокированы. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            else
        //            {
        //                int attemptsLeft = 3 - (int)user.FailedLoginAttempts;
        //                MessageBox.Show($"Неправильный пароль. Осталось попыток {attemptsLeft}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //}
    }
}
