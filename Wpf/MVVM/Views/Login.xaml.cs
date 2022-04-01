using System;
using System.Collections.Generic;
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
using Wpf.MVVM.Models;
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
            loginViewModel.Close = this.Hide;
            this.DataContext = this.loginViewModel;
        }
        public UsuarioVM GetLoggedUser() => loginViewModel.SelectedAccount ?? throw new Exception("Nenhuma conta logada");

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
            this.Close();
        }
    }
}
