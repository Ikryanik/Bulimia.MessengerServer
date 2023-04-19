using System;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.Messages;
using Bulimia.MessengerClient.Model;
using Bulimia.MessengerClient.View;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bulimia.MessengerClient.ViewModel
{
    public class MainWindowViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly UserManagerClient _userManagerClient;
        private readonly IMessageBoxCreator _messageBoxCreator;
        [Reactive]
        public string Username { get; set; }
        [Reactive]
        public Visibility ButtonAuthenticateVisibility { get; set; } = Visibility.Visible;
        [Reactive] 
        public Visibility ButtonRegisterVisibility { get; set; } = Visibility.Collapsed;
        [Reactive] 
        public Visibility LinkAuthenticateVisibility { get; set; } = Visibility.Collapsed;
        [Reactive] 
        public Visibility LinkRegisterVisibility { get; set; } = Visibility.Visible;


        public string UrlPathSegment => "main";
        public IScreen HostScreen { get; }
        
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
        public ReactiveCommand<Unit, Unit> AuthenticateCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangingButtonToRegistrationCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangingButtonToAuthenticationCommand { get; }

        public MainWindowViewModel()
        {
            _userManagerClient = new UserManagerClient();

            var canExecute = this.WhenAnyValue(
                x => x.Username,
                (userName) => !string.IsNullOrWhiteSpace(userName));

            RegisterCommand = ReactiveCommand.CreateFromTask(Register, canExecute);
            AuthenticateCommand = ReactiveCommand.CreateFromTask(Authenticate, canExecute);
            ChangingButtonToRegistrationCommand = ReactiveCommand.Create(ChangeButtonToRegistration);
            ChangingButtonToAuthenticationCommand = ReactiveCommand.Create(ChangeButtonToAuthentication);

            _messageBoxCreator = Locator.Current.GetService<IMessageBoxCreator>();
            
            HostScreen = Locator.Current.GetService<IScreen>();
        }

        private async Task Authenticate()
        {
            var username = Username.Trim();

            var result = await _userManagerClient.Authenticate(username);

            if (result == null)
            {
                _messageBoxCreator.CreateMessageBox("Неверное имя");
                return;
            }

            App.Username = username;

            var chatsViewModel = new UserChatsViewModel(result.Id);
            await chatsViewModel.Init();
            
            HostScreen.Router.Navigate.Execute(chatsViewModel);
            MessageBus.Current.SendMessage(new LogOutButtonVisibilityMessage(true));
            Username = string.Empty;
        }

        private async Task Register()
        {
            var username = Username.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                _messageBoxCreator.CreateMessageBox("Имя не может быть пустым");
                return;
            }

            var result = await _userManagerClient.Register(username);

            if (result == null)
            {
                _messageBoxCreator.CreateMessageBox("Такое имя уже существует");
                return;
            }

            _messageBoxCreator.CreateMessageBox("Вы успешно зарегистрированы!");
            Username = string.Empty;
        }

        private void ChangeButtonToRegistration()
        {
            ButtonAuthenticateVisibility = Visibility.Collapsed;
            ButtonRegisterVisibility = Visibility.Visible;

            LinkAuthenticateVisibility = Visibility.Visible;
            LinkRegisterVisibility = Visibility.Collapsed;
        }

        private void ChangeButtonToAuthentication()
        {
            ButtonAuthenticateVisibility = Visibility.Visible;
            ButtonRegisterVisibility = Visibility.Collapsed;

            LinkAuthenticateVisibility = Visibility.Collapsed;
            LinkRegisterVisibility = Visibility.Visible;
        }
    }
    
}