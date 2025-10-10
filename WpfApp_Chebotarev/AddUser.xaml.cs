using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_Chebotarev
{
    public partial class AddUser : Window
    {
        public User NewUser { get; private set; }
        public AddUser()
        {
            InitializeComponent();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)

        {
            string firstname = LName_user.Text.Trim();
            string lastname = FName_user.Text.Trim();
            string username = Username.Text.Trim();
            string password = Password.Text.Trim();

            string role = string.Empty;
            if (RoleComboBox.SelectedItem is ComboBoxItem selectedItem) // Проверяем, что это ComboBoxItem
            {
                role = selectedItem.Content?.ToString(); // Если это ComboBoxItem, берем Content
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


                    if (string.IsNullOrWhiteSpace(firstname))
                    {
                        MessageBox.Show("Поле 'Имя' обязательно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(lastname))
                    {
                        MessageBox.Show("Поле 'Фамилия' обязательно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(username))
                    {
                        MessageBox.Show("Поле 'Имя Пользователя' обязательно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        MessageBox.Show("Поле 'Пароль' обязательно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(role)) 
                    { 
                        MessageBox.Show("Выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return;

                    }

            this.NewUser = new User
            {
                firstname = firstname,
                lastname = lastname,
                username = username,
                password = password,
                role = role,
                IsLocked = false,
                IsFirstLogin = true,
                LastLoginDate = null
            };

            try
            {
                using (var context = new DBEntities())
                {
                    context.Users.Add(NewUser);
                    context.SaveChanges();
                }

                this.DialogResult = true;
                this.Close();


                LName_user.Clear();
                FName_user.Clear();
                Username.Clear();
                Password.Clear();
                RoleComboBox.SelectedIndex = -1;


            }
            catch (DbEntityValidationException ex)
            {
                // Обработка ошибок валидации EF6
                var errors = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                    .ToList();

                string errorMessage = string.Join("\n", errors);
                MessageBox.Show($"Ошибка валидации:\n{errorMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Обработка ошибок подключения к бд
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                // ...
                MessageBox.Show("Данные пользователя введены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // ...
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка");
            }
        }   


       
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close ();
        }

    }
    
}