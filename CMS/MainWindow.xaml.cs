using CMS.Models;
using CMS.Services;
using CMS.Services.Interfaces;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Notification.Wpf;

namespace CMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool passwordVisibility;
        private readonly IAuthenticationService authenticationService;
        private readonly NotificationManager notificationManager = new NotificationManager();
        private User? currentUser;
        public MainWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            passwordVisibility = false;
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            EyeIcon.Visibility = Visibility.Collapsed;
            UsernameTextBox.Focus();

            authenticationService = new AuthenticationService();

        }
        private void ShowToast(string title,string message, NotificationType type = NotificationType.Error)
        {
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type // Success, Error, Information, Warning
            },
            expirationTime: TimeSpan.FromSeconds(3));
            
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            /*List<User> users = xmlDataService.LoadAll();

            if (users.Count == 0)
            {
                UsernameTextBox.Text = "Nema korisnika.";
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var user in users)
            {
                sb.AppendLine($"{user.Name} {user.Surname} ({user.Username}) - {user.Role}");
            }

            UsernameTextBox.Text = sb.ToString();*/

            string? username = UsernameTextBox.Text.Trim();
            string? password;
            if (PasswordBox.Visibility == Visibility.Visible)
                password = PasswordBox.Password.Trim();
            else
                password = PasswordTextBox.Text.Trim();

            var result = authenticationService.LoginSuccessful(username, password);
            if (result.IsValidationError)
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    UsernameNote.Content = "Fill in username field";
                    UsernameTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    UsernameNote.Content = "";
                    UsernameTextBox.ClearValue(Border.BorderBrushProperty);
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    PasswordNote.Content = "Fill in password field";
                    PasswordBox.BorderBrush = Brushes.Red;
                    PasswordTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    PasswordNote.Content = "";
                    PasswordBox.ClearValue(Border.BorderBrushProperty);
                    PasswordTextBox.ClearValue(Border.BorderBrushProperty);
                }
                return;
            }

            if (!result.Success)
            {
                ShowToast("Error","Invalid username or password", NotificationType.Error);
                PasswordNote.Content = "";
                PasswordBox.ClearValue(Border.BorderBrushProperty);
                PasswordTextBox.ClearValue(Border.BorderBrushProperty);

                UsernameNote.Content = "";
                UsernameTextBox.ClearValue(Border.BorderBrushProperty);

                return;
            }
            currentUser = result.AuthenticatedUser;
            ShowToast("Login successful",$"Welcome, {currentUser.Name} ", NotificationType.Success);

            Menu menuWindow = new Menu(currentUser);
            menuWindow.Show();
            this.Close();
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EyeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (passwordVisibility)
            {
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Password = PasswordTextBox.Text;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/zatvoreno_oko.png", System.UriKind.Relative));
            }
            else
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Focus();
                PasswordTextBox.CaretIndex = PasswordTextBox.Text.Length;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/otvoreno_oko.png", System.UriKind.Relative));
            }

            passwordVisibility = !passwordVisibility;

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
                EyeIcon.Visibility = Visibility.Collapsed;
            else
                EyeIcon.Visibility = Visibility.Visible;
        }
    }
}