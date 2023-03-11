using System;
using Bulimia.MessengerClient.View;
using ReactiveUI;
using Splat;

namespace Bulimia.MessengerClient.ViewModel
{
    public class FirstViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public FirstViewModel()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.Register<IScreen>(() => this);
            Router.Navigate.Execute(new MainWindowViewModel());
        }
    }
}