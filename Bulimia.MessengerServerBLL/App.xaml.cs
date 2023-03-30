using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Bulimia.MessengerClient.Messages;
using Bulimia.MessengerClient.View;
using ReactiveUI;

namespace Bulimia.MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            Locator.CurrentMutable.Register<IMessageBoxCreator>(() => new MessageBoxCreator());
        }
    }
}
