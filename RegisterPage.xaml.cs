using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoTraining
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        UserContext db = new UserContext();
        public RegisterPage()
        {
            InitializeComponent();
            db.Database.EnsureCreated();
        }

        private void registrationToMain(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("Не все поля заполнены.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (string currentUsername in db.Users.Select(u => u.GetUsername()))
            {
                if (currentUsername == username.Text.Trim())
                {
                    MessageBox.Show($"Пользователь {username.Text.Trim()} уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            User newUser = new User(username.Text.Trim(), password.Password.Trim());

            db.Users.Add(newUser);
            db.SaveChanges();

            MessageBox.Show($"Пользователь {username.Text.Trim()} зарегистрирован. Переадресация на главную страницу...", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            Thread.Sleep(2000);

            NavigationService.Navigate(new MainPage());
        }

        private void toLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
