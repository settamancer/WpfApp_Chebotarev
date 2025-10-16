using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
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

namespace WpfApp_Chebotarev
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
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


                if (CheckOldPassword(oldPassword.Trim()))
                {
                    if (ValidateNewPassword(newPassword.Trim()))
                    {
                        SaveNewPassword(newPassword.Trim());
                        MessageBox.Show("Пароль успешно изменён.");
                    }
                    else
                    {
                        MessageBox.Show("Новый пароль не соответствует требованиям.");
                    }
                }
                else
                {
                    MessageBox.Show("Старый пароль введён неверно.");
            }
        }

        private bool CheckOldPassword(string oldPassword)
        {
            using (var context = new DBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.password.Trim() == oldPassword.Trim());
                if (user != null)
                {
                    // Debug
                    Debug.WriteLine($"Сравнение: [{user.password}] == [{oldPassword}]");
                    return user.password.Trim() == oldPassword;

                }
            }
            return false;
        }

        private bool ValidateNewPassword(string newPassword)
        {
            return newPassword.Length >= 6;
        }

        private void SaveNewPassword(string newPassword)
        {
            using (var context = new DBEntities())
            {
                var user = context.Users.FirstOrDefault(u => u.id == UserId);
                if (user != null)
                {
                    user.password = newPassword.Trim();
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        
        }
        
    }
}
