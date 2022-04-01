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

namespace Wpf.MVVM.Views
{
    
    
    /// <summary>
    /// Lógica interna para MessageBoxC.xaml
    /// </summary>
    public partial class MessageBoxC : Window
    {
        public Action SeConfirmar { get; }
        public Action SeNegar { get; }

        private MessageBoxC()
        {
            InitializeComponent();
        }
        public MessageBoxC(string Text,Action SeConfirmar, Action SeNegar) : this()
        {
            PrincipalText.Text = Text;
            StackPanelInform.Visibility = Visibility.Collapsed;
            this.SeConfirmar = SeConfirmar;
            this.SeNegar = SeNegar;
        }
        public MessageBoxC(string Text, Action? OnClose = null) : this()
        {
            PrincipalText.Text = Text;
            StackPanelConfimOrDenied.Visibility = Visibility.Collapsed;
            SeConfirmar = () => { };
            SeNegar = () => { };
            if(OnClose!=null)
                this.Closing += (s,e) => OnClose();
        }
        private void Negar_Click(object sender, RoutedEventArgs e)
        {
            SeNegar();
            this.Close();
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            SeConfirmar();
            this.Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }














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
