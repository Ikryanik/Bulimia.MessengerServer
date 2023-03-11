using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.Domain.Core;
using Bulimia.MessengerClient.ViewModel;
using ReactiveUI;

namespace Bulimia.MessengerClient.View
{
    /// <summary>
    /// Логика взаимодействия для UserChatsWindow.xaml
    /// </summary>
    public partial class UserChatsWindow : ReactiveUserControl<UserChatsViewModel>
    {
        private readonly int _id;
        private readonly ChatClient _chatClient;
        public UserChatsWindow()
        {
            InitializeComponent();
            
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Chats,
                    view => view.ListViewLastMessages.ItemsSource);
                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedChat,
                    view => view.ListViewLastMessages.SelectedItem);
                this.Bind(ViewModel,
                    viewModel => viewModel.ProgressBarVisibility,
                    view => view.ProgressBar.Visibility);
                this.Bind(ViewModel,
                    viewModel => viewModel.TextBlockZeroChatsVisibility,
                    view => view.TextBlockZeroChats.Visibility);


            });
        }
        
        //public async Task Init()
        //{
        //    TextBlockZeroChats.Visibility = Visibility.Hidden;
        //    ListViewLastMessages.Visibility = Visibility.Hidden;

        //    var chats = await _chatClient.GetUserChats(_id);
            
        //    ProgressBar.Visibility = Visibility.Collapsed;

        //    if (chats == null)
        //    {
        //        TextBlockZeroChats.Visibility = Visibility.Visible;
        //        return;
        //    }

        //    ListViewLastMessages.ItemsSource = chats;
        //    ListViewLastMessages.Visibility = Visibility.Visible;
        //}

        private async void SelectionChanged_OnSelected(object sender, RoutedEventArgs e)
        {
            var item = ListViewLastMessages.SelectedItem as Chat;

            if (ListViewLastMessages.SelectedItem == null)
                return;

            var chatWindow = new ChatWindow(item, _id);
            await chatWindow.Init();
            //Hide();
            chatWindow.ShowDialog();
            //Show();
            //await Init();
        }
        
    }

    
}
