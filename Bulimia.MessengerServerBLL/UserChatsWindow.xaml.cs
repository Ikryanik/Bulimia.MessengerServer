using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.Domain.Core;

namespace Bulimia.MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для UserChatsWindow.xaml
    /// </summary>
    public partial class UserChatsWindow : Window
    {
        private readonly int _id;
        private readonly ChatClient _chatClient;
        public UserChatsWindow(int id)
        {
            _id = id;
            _chatClient = new ChatClient();
            InitializeComponent();
        }
        
        public async Task Init()
        {
            TextBlockZeroChats.Visibility = Visibility.Hidden;
            ListViewLastMessages.Visibility = Visibility.Hidden;

            var chats = await _chatClient.GetUserChats(_id);
            
            ProgressBar.Visibility = Visibility.Collapsed;

            if (chats == null)
            {
                TextBlockZeroChats.Visibility = Visibility.Visible;
                return;
            }

            ListViewLastMessages.ItemsSource = chats;
            ListViewLastMessages.Visibility = Visibility.Visible;
        }

        private async void SelectionChanged_OnSelected(object sender, RoutedEventArgs e)
        {
            var item = ListViewLastMessages.SelectedItem as Chat;

            if (ListViewLastMessages.SelectedItem == null)
                return;

            var chatWindow = new ChatWindow(item, _id);
            await chatWindow.Init();
            Hide();
            chatWindow.ShowDialog();
            Show();
            await Init();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Init();
        }
    }

    
}
