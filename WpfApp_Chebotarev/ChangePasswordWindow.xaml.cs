using System.Linq;
using System.Windows;
namespace WpfApp_Chebotarev
{
    public partial class ChangePasswordWindow : Window
    {

        public int UserId { get; set; }

        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        public ChangePasswordWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = txtCurrentPassword.Password.Trim();
            string newPassword = txtNewPassword.Password.Trim();

            using (var context = new DBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.id == UserId);

                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.");
                    return;
                }

                if (user.password.Trim() != oldPassword)
                {
                    MessageBox.Show("Старый пароль введён неверно.");
                    return;
                }

                if (newPassword.Length < 6)
                {
                    MessageBox.Show("Новый пароль должен содержать минимум 6 символов.");
                    return;
                }

                user.password = newPassword.Trim();
                user.IsFirstLogin = false;
                context.SaveChanges();

                MessageBox.Show("Пароль успешно изменён.");

                // Открытие окна по роли
                Window nextWindow = null;
                switch (user.role)
                {
                    case "Admin": nextWindow = new AdminWindow(); break;
                    case "Manager": nextWindow = new ManagerWindow(); break;
                    case "Cleaning_staff": nextWindow = new clSchedule(); break;
                    default: nextWindow = new MainWindow(); break;
                }
                nextWindow.Show();
                this.Close();
            }
        }
    }
}
