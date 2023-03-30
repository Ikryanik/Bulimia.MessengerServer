using System.Reactive;
using System.Windows;
using Bulimia.MessengerClient.Messages;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;

namespace Bulimia.MessengerClient.ViewModel
{
    public class FirstViewModel : ReactiveObject, IScreen
    {
        private readonly IMessageBoxCreator _messageBoxCreator;
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, Unit> LogOutCommand { get; set; }

        [Reactive]
        public Visibility LogOutButtonVisibility { get; set; } = Visibility.Collapsed;
        [Reactive]
        public Visibility TextBlockUsernameVisibility { get; set; } = Visibility.Collapsed;

        public FirstViewModel()
        {
            _messageBoxCreator = Locator.Current.GetService<IMessageBoxCreator>();

            Router = new RoutingState();

            MessageBus.Current.Listen<LogOutButtonVisibilityMessage>().Subscribe(x =>
            {
                LogOutButtonVisibility = x.IsVisible ? Visibility.Visible : Visibility.Collapsed;
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
        }
    }
}