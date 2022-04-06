using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared;
using WebSocket.Shared.DataAcess;
using WebSocket.Shared.DataAcess.Local;
using WebSocket.Shared.DataAcess.Local.Repositories;
using WebSocket.Shared.DataAcess.Models;
using Wpf.MVVM.Models;
using Wpf.MVVM.ViewModels;
using Wpf.MVVM.Views;
using Wpf.Services;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public static RepositoryFactory RepositoryFactory;
        public App()
        {
            ConfigureDependencyInjection();
            InitializeComponent();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            App.RepositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        private void ConfigureDependencyInjection()
        {
            var servicesCollection = new ServiceCollection();
            //
            servicesCollection.AddTransient<IRepository<Usuario>, UsuarioRepository>();
            servicesCollection.AddTransient<IRepository<Contato>, ContatoRepository>();
            servicesCollection.AddTransient<IRepository<Message>, MessageRepository>();
            servicesCollection.AddTransient<SalkySqlLiteDbContext>();

            //
            servicesCollection.AddSingleton<UserService>();
            //
            servicesCollection.AddSingleton<IServiceProvider>((x) => this.serviceProvider);
            servicesCollection.AddSingleton<RepositoryFactory>();
            //
            servicesCollection.AddSingleton<RepositoryFactory>();
            servicesCollection.AddSingleton<Login>();
            servicesCollection.AddSingleton<LoginViewModel>();
            servicesCollection.AddSingleton<MainViewModel>();
            servicesCollection.AddSingleton<MainWindow>();
            this.serviceProvider = servicesCollection.BuildServiceProvider();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            foreach(var window in App.Current.Windows)
            {
                var type = window.GetType();
                var disposeFunc = type.GetMethod("Dispose");
                if(disposeFunc != null)
                    disposeFunc.Invoke(window,null);

                var dataContext = ((Window)window).DataContext;
                if(dataContext != null)
                {
                    var dataContextType = dataContext.GetType();
                    if(dataContextType != null)
                    {
                        var disposeFunc2 = dataContextType.GetMethod("Dispose");
                        if (disposeFunc2 != null)
                            disposeFunc2.Invoke(dataContext, null);
                    }
                }

            }
            base.OnExit(e);
        }
    }

}
