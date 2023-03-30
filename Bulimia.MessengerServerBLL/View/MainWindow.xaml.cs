using System;
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

            this.WhenActivated(disposables =>
                {
                    this.Bind(ViewModel,
                            viewModel => viewModel.Username,
                            view => view.TextBoxUsername.Text)
                        .DisposeWith(disposables);

                    this.Bind(ViewModel,
                        viewModel => viewModel.LinkAuthenticateVisibility,
                        view => view.LabelChangeRegisterButton.Visibility)
                        .DisposeWith(disposables);

                    this.Bind(ViewModel,
                        viewModel => viewModel.LinkRegisterVisibility,
                        view => view.LabelChangeAuthenticateButton.Visibility)
                        .DisposeWith(disposables);

                    this.Bind(ViewModel,
                        viewModel => viewModel.ButtonAuthenticateVisibility,
                        view => view.ButtonAuthenticate.Visibility)
                        .DisposeWith(disposables);
                    
                    this.Bind(ViewModel,
                        viewModel => viewModel.ButtonRegisterVisibility,
                        view => view.ButtonRegister.Visibility)
                        .DisposeWith(disposables);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.RegisterCommand,
                        view => view.ButtonRegister)
                        .DisposeWith(disposables);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.AuthenticateCommand,
                        view => view.ButtonAuthenticate)
                        .DisposeWith(disposables);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.ChangingButtonToAuthenticationCommand,
                        view => view.LabelChangeRegisterButton)
                        .DisposeWith(disposables);

                    this.BindCommand(ViewModel,
                        viewModel => viewModel.ChangingButtonToRegistrationCommand,
                        view => view.LabelChangeAuthenticateButton)
                        .DisposeWith(disposables);
                }
            );
        }
    }
}
