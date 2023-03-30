using System.Collections.ObjectModel;
using Bulimia.MessengerClient.BLL;
using ReactiveUI;
using System.Windows;
using Splat;
using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Bulimia.MessengerClient.Domain.Core;
using Bulimia.MessengerClient.Messages;
using ReactiveUI.Fody.Helpers;
using DynamicData;
using Application = System.Windows.Application;

namespace Bulimia.MessengerClient.ViewModel
{
    public class UserChatsViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        public string UrlPathSegment => "userChats";
        private readonly IMessageBoxCreator _messageBoxCreator;
        private readonly int _id;
        private readonly ChatClient _chatClient;
        private readonly UserClient _userClient;
        private bool _isEditing;
        private Task _updatingMessages;
        private Task _updatingChats;
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private readonly CancellationTokenSource _cancelTokenSource2 = new CancellationTokenSource();
        private readonly SoundPlayer _player = new SoundPlayer(Directory.GetCurrentDirectory() + @"\Sounds\noti.wav");
        private bool _isDisposed = false;
        private int IdOfChat { get; set; }
        public IScreen HostScreen { get; }
        [Reactive]
        public string CompanionUsername { get; set; }
        [Reactive]
        public Chat SelectedChat { get; set; }
        [Reactive]
        public MessageDto SelectedMessage { get; set; }
        [Reactive]
        public UserModel SelectedUserInSearch { get; set; }
        [Reactive]
        public string MessageText { get; set; }
        [Reactive]
        public string UserSearchText { get; set; }
        [Reactive]
        public Visibility TextBlockZeroChatsVisibility { get; set; } = Visibility.Hidden;
        [Reactive]
        public Visibility ListViewLastMessagesVisibility { get; set; } = Visibility.Hidden;
        [Reactive]
        public Visibility ChatProgressBarVisibility { get; set; } = Visibility.Visible;
        [Reactive]
        public Visibility MessagesGridVisibility { get; set; } = Visibility.Hidden;
        [Reactive]
        public Visibility StackPanelZeroMessagesVisibility { get; set; } = Visibility.Collapsed;
        [Reactive]
        public Visibility ListViewMessagesVisibility { get; set; } = Visibility.Collapsed;
        [Reactive]
        public Visibility MessagesProgressBarVisibility { get; set; } = Visibility.Visible;
        [Reactive]
        public Visibility ManipulatingPanelVisibility { get; set; } = Visibility.Collapsed;
        [Reactive]
        public Visibility CancelButtonVisibility { get; set; } = Visibility.Collapsed;
        [Reactive]
        public bool IsSearchingOpen { get; set; }
        [Reactive]
        public ObservableCollection<Chat> Chats { get; set; }
        [Reactive]
        public ObservableCollection<UserModel> Users { get; set; }
        [Reactive]
        public ObservableCollection<MessageDto> Messages { get; set; }
        public ReactiveCommand<Unit, Unit> DeletingMessageCommand { get; }
        public ReactiveCommand<Unit, Unit> CreatingMessageCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangingToEditingCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshingMessagesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public UserChatsViewModel(int id = 0)
        {
            _chatClient = new ChatClient();
            _userClient = new UserClient();
            _id = id;

            HostScreen = Locator.Current.GetService<IScreen>();

            this.WhenAnyValue(x => x.SelectedChat).Subscribe(OpenChat);
            this.WhenAnyValue(x => x.SelectedMessage).Subscribe(x =>
            {
                if (x == null || x.SenderId != _id)
                {
                    ManipulatingPanelVisibility = Visibility.Collapsed;
                    return;
                }

                ManipulatingPanelVisibility = Visibility.Visible;
            });
            this.WhenAnyValue(x => x.UserSearchText).Subscribe(SearchUsers);
            this.WhenAnyValue(x => x.SelectedUserInSearch).Subscribe(y =>
            {
                if (y == null)
                {
                    return;
                }

                var chat = new Chat
                {
                    Id = y.Id,
                    Username = y.Username
                };

                OpenChat(chat);
                UserSearchText = string.Empty;
            });

            _messageBoxCreator = Locator.Current.GetService<IMessageBoxCreator>();

            ChangingToEditingCommand = ReactiveCommand.Create(ChangeToEditing);
            DeletingMessageCommand = ReactiveCommand.CreateFromTask(DeleteMessage);
            CreatingMessageCommand = ReactiveCommand.CreateFromTask(UploadMessage);
            RefreshingMessagesCommand = ReactiveCommand.CreateFromTask(Refresh);
            CancelCommand = ReactiveCommand.Create(CancelUpdate);

            MessageBus.Current.Listen<DisposeMessage>().Subscribe(x => Dispose());
        }

        private async void SearchUsers(string x)
        {

            var result = await _userClient.SearchUsers();

            if (result == null) return;

            if (string.IsNullOrEmpty(x))
            {
                Users = new ObservableCollection<UserModel>(result);
                IsSearchingOpen = false;
                return;
            }

            x = x.Trim().ToLower();

            Users = new ObservableCollection<UserModel>(result.Where(y =>
                y.Username.ToLower().Contains(x)));

            IsSearchingOpen = Users.Any();
        }

        public async Task Init()
        {
            var chats = await _chatClient.GetUserChats(_id);

            ChatProgressBarVisibility = Visibility.Collapsed;
            
            Chats = new ObservableCollection<Chat>(chats);

            _updatingChats = new Task(() => GetUpdatesOfChats());
            _updatingChats.RunSynchronously();

            if (chats.Count == 0)
            {
                TextBlockZeroChatsVisibility = Visibility.Visible;
                return;
            }

            ListViewLastMessagesVisibility = Visibility.Visible;

        }

        private async Task GetUpdatesOfChats()
        {
            while (true)
            {
               
                try
                {
                    var result = await _chatClient.GetChatsUpdates(_id);
                    
                    if (_isDisposed)
                        return;

                    if (result == null) continue;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Chats.Clear();
                        Chats.AddRange(result);
                        ListViewLastMessagesVisibility = result.Count < 1 ? Visibility.Collapsed : Visibility.Visible;
                        TextBlockZeroChatsVisibility = result.Count < 1 ? Visibility.Visible : Visibility.Collapsed;
                    });
                }
                catch (Exception e)
                {
                    _messageBoxCreator.CreateMessageBox(e.Message);
                }
            }
        }

        private async Task GetUpdatesOfMessages()
        {
            while (true)
            {
                try
                {
                    var result = await _chatClient.GetMessagesUpdates(_id, IdOfChat);
                    
                    if (_isDisposed)
                        return;

                    if (result != null)
                    {
                        _player.Play();

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Messages.Clear();
                            Messages.AddRange(result);
                            SelectedMessage = Messages.LastOrDefault();
                            SelectedMessage = null;
                        });
                    }
                }
                catch (Exception e)
                {
                    _messageBoxCreator.CreateMessageBox(e.Message);
                }
            }
        }

        private async void OpenChat(Chat x)
        {
            if (x == null) return;

            if (_updatingMessages != null && !_updatingMessages.IsCanceled)
            {
                _cancelTokenSource.Cancel();
            }

            IdOfChat = x.Id;
            MessagesGridVisibility = Visibility.Visible;
            CompanionUsername = x.Username;
            await Refresh();

            var token = _cancelTokenSource.Token;
            _updatingMessages = Task.Run(GetUpdatesOfMessages, token);
        }

        public async Task Refresh()
        {
            var chat = await _chatClient.GetMessages(_id, IdOfChat);

            MessagesProgressBarVisibility = Visibility.Collapsed;

            if (chat.Count == 0)
            {
                ListViewMessagesVisibility = Visibility.Collapsed;
                StackPanelZeroMessagesVisibility = Visibility.Visible;
                return;
            }

            StackPanelZeroMessagesVisibility = Visibility.Collapsed;

            Messages = new ObservableCollection<MessageDto>(chat);
            ListViewMessagesVisibility = Visibility.Visible;

            SelectedMessage = Messages.LastOrDefault();
            SelectedMessage = null;
        }

        private async Task DeleteMessage()
        {
            var messageBoxResult = _messageBoxCreator.CreateMessageBox("Удалить сообщение?", "Внимание", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes)
                return;

            if (SelectedMessage == null)
                return;

            var result = await _chatClient.DeleteMessage(SelectedMessage.Id);

            if (result == 0)
            {
                _messageBoxCreator.CreateMessageBox("Произошла ошибка");
                return;
            }

            Messages.Remove(SelectedMessage);
        }

        private void ChangeToEditing()
        {
            MessageText = SelectedMessage.Text;
            _isEditing = true;
            CancelButtonVisibility = Visibility.Visible;
        }

        private async Task UploadMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;

            var result = _isEditing ? await UpdateMessage() : await CreateMessage();

            if (result == 0)
            {
                _messageBoxCreator.CreateMessageBox("Произошла ошибка");
                return;
            }

            MessageText = string.Empty;


            await Refresh();
        }

        private async Task<int> UpdateMessage()
        {
            var message = new MessageModel
            {
                Id = SelectedMessage.Id,
                SenderId = _id,
                ReceiverId = IdOfChat,
                Text = MessageText.Trim(),
            };

            var result = await _chatClient.UpdateMessage(message);
            CancelButtonVisibility = Visibility.Collapsed;
            _isEditing = false;

            return result;
        }

        private async Task<int> CreateMessage()
        {
            var message = new MessageModel
            {
                SenderId = _id,
                ReceiverId = IdOfChat,
                Text = MessageText.Trim(),
            };

            var result = await _chatClient.CreateMessage(message);

            return result;
        }

        public void CancelUpdate()
        {
            MessageText = string.Empty;
            _isEditing = false;
            CancelButtonVisibility = Visibility.Collapsed;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            
            _player?.Dispose();
            DeletingMessageCommand?.Dispose();
            CreatingMessageCommand?.Dispose();
            ChangingToEditingCommand?.Dispose();
            RefreshingMessagesCommand?.Dispose();
            CancelCommand?.Dispose();
            _isDisposed = true;
        }
    }
}