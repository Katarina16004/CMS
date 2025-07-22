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
using System.Windows.Threading;

namespace CMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer notificationTimer;
        private bool passwordVisibility;
        private readonly IAuthenticationService authenticationService;
        //private readonly NotificationManager notificationManager = new NotificationManager();
        private User? currentUser;
        public MainWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            passwordVisibility = false;
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            EyeIcon.Visibility = Visibility.Collapsed;
            UsernameTextBox.Focus();

            #region notificationSetup
            notificationTimer = new DispatcherTimer();
            notificationTimer.Interval = TimeSpan.FromSeconds(3);
            notificationTimer.Tick += (s, e) =>
            {
                NotificationPanel.Visibility = Visibility.Collapsed;
                notificationTimer.Stop();
            };
            #endregion

            authenticationService = new AuthenticationService();

        }
        private void ShowToast(string title,string message)
        {
            NotificationTitle.Text = title;
            NotificationMessage.Text = message;
            NotificationPanel.Visibility = Visibility.Visible;

            notificationTimer.Stop();
            notificationTimer.Start();

        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
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
                ShowToast("Error","Invalid username or password");
                PasswordNote.Content = "";
                PasswordBox.ClearValue(Border.BorderBrushProperty);
                PasswordTextBox.ClearValue(Border.BorderBrushProperty);

                UsernameNote.Content = "";
                UsernameTextBox.ClearValue(Border.BorderBrushProperty);

                return;
            }
            currentUser = result.AuthenticatedUser;

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