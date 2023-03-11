using System.Reactive.Disposables;
using System.Windows;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.ViewModel;
using ReactiveUI;

namespace Bulimia.MessengerClient.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveUserControl<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();

            this.WhenActivated(disposableRegistration =>
                {
                    this.Bind(ViewModel,
                            viewModel => viewModel.Username,
                            view => view.TextBoxUsername.Text)
                        .DisposeWith(disposableRegistration);

                    this.Bind(ViewModel,
                        viewModel => viewModel.LinkAuthenticateVisibility,
                        view => view.LabelChangeRegisterButton.Visibility);

                    this.Bind(ViewModel,
                        viewModel => viewModel.LinkRegisterVisibility,
                        view => view.LabelChangeAuthenticateButton.Visibility);

                    this.Bind(ViewModel,
                        viewModel => viewModel.ButtonAuthenticateVisibility,
                        view => view.ButtonAuthenticate.Visibility);
                    
                    this.Bind(ViewModel,
                        viewModel => viewModel.ButtonRegisterVisibility,
                        view => view.ButtonRegister.Visibility);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.RegisterCommand,
                        view => view.ButtonRegister);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.AuthenticateCommand,
                        view => view.ButtonAuthenticate);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.ChangingButtonToAuthenticationCommand,
                        view => view.LabelChangeRegisterButton);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.ChangingButtonToRegistrationCommand,
                        view => view.LabelChangeAuthenticateButton);
                }
            );
        }
    }
}
