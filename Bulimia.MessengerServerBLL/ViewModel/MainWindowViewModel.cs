using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.Common;
using Bulimia.MessengerClient.Model;
using ReactiveUI;

namespace Bulimia.MessengerClient.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly UserManagerClient _userManagerClient;

        private string _username;
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }


        private RelayCommand _registerCommand;

        public RelayCommand RegisterCommand
        {
            get
            {
                return _registerCommand ??
                       (_registerCommand = new RelayCommand(
                           Execute,
                           obj =>
                               !string.IsNullOrWhiteSpace(Username)));
            }
        }

        public MainWindowViewModel()
        {
            _userManagerClient = new UserManagerClient();
        }

        private async void Execute(object obj)
        {
            var username = Username.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Имя не может быть пустым");
                return;
            }

            var result = await _userManagerClient.Register(username);

            if (result == null)
            {
                MessageBox.Show("Такое имя уже существует");
                return;
            }

            MessageBox.Show("Вы успешно зарегистрированы!");
            Username = string.Empty;
        }

    }
}