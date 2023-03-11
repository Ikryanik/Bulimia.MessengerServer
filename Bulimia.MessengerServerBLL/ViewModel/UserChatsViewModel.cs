using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bulimia.MessengerClient.BLL;
using ReactiveUI;
using System.Windows;
using Splat;
using System.Windows.Forms;
using Bulimia.MessengerClient.Model;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Bulimia.MessengerClient.Domain.Core;
using Bulimia.MessengerClient.View;
using ReactiveUI.Fody.Helpers;

namespace Bulimia.MessengerClient.ViewModel
{
    public class UserChatsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "userChats";
        private readonly int _id;
        private readonly ChatClient _chatClient;
        private ChatModel _chat;
        private ChatModel _selectedChat;
        public string Username
        {
            get => _chat.Username;
            set => _chat.Username = value;
        }

        public DateTime Time
        {
            get => _chat.Time;
            set => _chat.Time = value;
        }

        public string LastMessage
        {
            get => _chat.LastMessage;
            set => _chat.LastMessage = value;
        }
        [Reactive]
        public Chat SelectedChat { get; set; }
        public IScreen HostScreen { get; }
        [Reactive]
        public Visibility TextBlockZeroChatsVisibility { get; set; } = Visibility.Hidden;
        [Reactive] 
        public Visibility ListViewLastMessagesVisibility { get; set; } = Visibility.Hidden;
        [Reactive]
        public Visibility ProgressBarVisibility { get; set; } = Visibility.Visible;
        [Reactive]
        public ObservableCollection<Chat> Chats { get; set; }
        public ReactiveCommand<Unit, Unit> ChangingSelectedItem { get; }

        public UserChatsViewModel(int id = 0)
        {
            _chatClient = new ChatClient();
            _id = id;

            this.WhenAnyValue(x => x.SelectedChat).Subscribe(async x =>
            {
                if (x == null)
                    return;

                MessageBus.Current.SendMessage(new MessageBoxMessage(x.Username));

               // var chatWindow = new ChatWindow(item, _id);

            });

            HostScreen = Locator.Current.GetService<IScreen>();
        }

        public async Task Init()
        {
            var chats = await _chatClient.GetUserChats(_id);

            ProgressBarVisibility = Visibility.Collapsed;

            if (chats == null)
            {
                TextBlockZeroChatsVisibility = Visibility.Visible;
                return;
            }

            Chats = new ObservableCollection<Chat>(chats);
            ListViewLastMessagesVisibility = Visibility.Visible;

        }
    }
}