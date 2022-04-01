using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared;
using WebSocket.Shared.DataAcess.Local;
using WebSocket.Shared.DataAcess.Local.Services;
using WebSocket.Shared.DataAcess.Models;
using Wpf.MVVM.Models;
using Wpf.MVVM.ViewModels;
using Wpf.MVVM.Views;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ConfigureDependencyInjection();
            InitializeComponent();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        private void ConfigureDependencyInjection()
        {
            var servicesCollection = new ServiceCollection();
            servicesCollection.AddSingleton<IServiceProvider>((x) => this.serviceProvider);
            servicesCollection.AddSingleton<IRepository<Usuario>, WebSocket.Shared.DataAcess.Local.Repositories.UsuarioRepository>();
            servicesCollection.AddSingleton<SalkySqlLiteDbContext>();
            servicesCollection.AddSingleton<Login>();
            servicesCollection.AddSingleton<LoginViewModel>();
            servicesCollection.AddSingleton<MainViewModel>();
            servicesCollection.AddSingleton<MainWindow>();
            this.serviceProvider = servicesCollection.BuildServiceProvider();
        }







    }

}
