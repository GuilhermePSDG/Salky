using System;
using System.Windows;
using System.Windows.Input;
using WebSocket.Shared.DataAcess.Models;
using Wpf.MVVM.ViewModels;

namespace Wpf.MVVM.Views
{
    /// <summary>
    /// Lógica interna para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginViewModel loginViewModel;

        public Login(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            this.loginViewModel = loginViewModel;
            this.DataContext = this.loginViewModel;
        }

        public UsuarioVM GetLoggedUserVM() => loginViewModel.SelectedAccount ?? throw new Exception("Nenhuma conta logada");

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimazineButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void MaximazeButtonClick(object sender, RoutedEventArgs e)
        {

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            else
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
