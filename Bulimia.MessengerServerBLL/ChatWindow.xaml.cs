using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.Domain.Core;

namespace Bulimia.MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly Chat _chat;
        private readonly int _userId;
        private readonly ChatClient _chatClient;
        public ChatWindow(Chat chat, int userId)
        {
            _chat = chat;
            _userId = userId;
            _chatClient = new ChatClient();
            InitializeComponent();
        }

        public async Task Init()
        {
            StackPanelZeroMessages.Visibility = Visibility.Collapsed;
            ListViewMessages.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Visible;

            var chat = await _chatClient.GetMessages(_userId, _chat.Id);

            ProgressBar.Visibility = Visibility.Collapsed;

            if (chat == null)
            {
                StackPanelZeroMessages.Visibility = Visibility.Visible;
                return;
            }

            ListViewMessages.ItemsSource = chat;
            ListViewMessages.Visibility = Visibility.Visible;
        }

        private void DeleteMessage_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateMessage_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private async void CreateMessage_OnClick(object sender, RoutedEventArgs e)
        {
            var message = new MessageModel
            {
                SenderId = _userId,
                ReceiverId = _chat.Id,
                Text = TextBoxMessage.Text,
            };
            
            var result = await _chatClient.CreateMessage(message);

            if (result == 0)
            {
                MessageBox.Show("Произошла ошибка");
                return;
            }

            TextBoxMessage.Text = "";

            await Init();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Init();
        }

        private void GridMessage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewMessages.SelectedItem != null)
                PanelItemManipulating.Visibility = Visibility.Visible;
            else
                PanelItemManipulating.Visibility = Visibility.Collapsed;
        }
    }
}
