using System.Reactive;
using System.Windows;
using Bulimia.MessengerClient.Messages;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Bulimia.MessengerClient.ViewModel
{
    public class FirstViewModel : ReactiveObject, IScreen
    {
        private readonly IMessageBoxCreator _messageBoxCreator;
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, Unit> LogOutCommand { get; set; }
        [Reactive]
        private string Username { get; set; }
        [Reactive]
        public string Greetings { get; set; }

        [Reactive]
        public Visibility TextBlockUsernameVisibility { get; set; } = Visibility.Collapsed;
        [Reactive]
        public Visibility LogOutButtonVisibility { get; set; } = Visibility.Collapsed;


        public FirstViewModel()
        {
            _messageBoxCreator = Locator.Current.GetService<IMessageBoxCreator>();

            Router = new RoutingState();

            MessageBus.Current.Listen<LogOutButtonVisibilityMessage>().Subscribe(x =>
            {
                LogOutButtonVisibility = x.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            });

            MessageBus.Current.Listen<UsernameChangingMessage>().Subscribe(x =>
            {
                Username = x.Username;
                TextBlockUsernameVisibility = x.UsernameVisibility ? Visibility.Visible : Visibility.Collapsed;
                Greetings = !string.IsNullOrEmpty(Username) ? $"Добро пожаловать, {Username}" : null;
            });

            Locator.CurrentMutable.Register<IScreen>(() => this);
            Router.Navigate.Execute(new MainWindowViewModel());

            LogOutCommand = ReactiveCommand.Create(LogOut);
        }

        public void LogOut()
        {
            var result = _messageBoxCreator.CreateMessageBox("Выйти из профиля?", "", MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
                return;

            Router.NavigateBack.Execute();
            LogOutButtonVisibility = Visibility.Collapsed;

            MessageBus.Current.SendMessage(new DisposeMessage());
            MessageBus.Current.SendMessage(new UsernameChangingMessage(null));
        }
    }


}