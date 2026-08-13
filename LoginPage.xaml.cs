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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        UserContext db = new UserContext();
        public LoginPage()
        {
            InitializeComponent();
            db.Database.EnsureCreated();
        }

        private void toRegistration(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        private void loginToMain(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("Не все поля заполнены.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (User user in db.Users)
            {
                if (user.GetUsername() == username.Text.Trim() && user.GetPassword() == password.Password.Trim())
                {
                    NavigationService.Navigate(new MainPage());
                }
                else
                {
                    MessageBox.Show("Пользователь с введёнными данными не найден в системе.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }
    }
}
