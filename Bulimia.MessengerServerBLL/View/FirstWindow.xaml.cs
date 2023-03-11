using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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
using Bulimia.MessengerClient.ViewModel;

namespace Bulimia.MessengerClient.View
{
    /// <summary>
    /// Логика взаимодействия для FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : ReactiveWindow<FirstViewModel>
    {
        public FirstWindow()
        {
            InitializeComponent();
            ViewModel = new FirstViewModel();
            this.WhenActivated(disposables =>
            {
                // Bind the view model router to RoutedViewHost.Router property.
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                    .DisposeWith(disposables);
            });
        }
    }
}
