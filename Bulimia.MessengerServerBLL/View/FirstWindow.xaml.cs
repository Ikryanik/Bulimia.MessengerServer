using ReactiveUI;
using System.Reactive.Disposables;
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
                this.OneWayBind(ViewModel,
                        viewModel => viewModel.Router,
                        view => view.RoutedViewHost.Router)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    viewModel => viewModel.LogOutCommand,
                    view => view.ButtonLogOut)
                    .DisposeWith(disposables);

                this.Bind(ViewModel,
                    viewModel => viewModel.LogOutButtonVisibility,
                    view => view.ButtonLogOut.Visibility)
                    .DisposeWith(disposables);

                this.Bind(ViewModel,
                    viewModel => viewModel.TextBlockUsernameVisibility,
                    view => view.TextBlockUsername.Visibility)
                    .DisposeWith(disposables);

                this.Bind(ViewModel,
                    viewModel => viewModel.Greetings,
                    view => view.TextBlockUsername.Text);

            });
        }

    }


}
