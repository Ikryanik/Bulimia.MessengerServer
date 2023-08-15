using System.Collections.ObjectModel;
using Bulimia.MessengerClient.BLL;
using ReactiveUI;
using System.Windows;
using Splat;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Reactive;
using System.Threading.Tasks;
using Bulimia.MessengerClient.Domain.Core;
using Bulimia.MessengerClient.Messages;
using Bulimia.MessengerClient.Model;
using ReactiveUI.Fody.Helpers;
using DynamicData;
using Application = System.Windows.Application;
using UserModel = Bulimia.MessengerClient.Domain.Core.UserModel;

namespace Bulimia.MessengerClient.ViewModel
{
    public class UserChatsViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private readonly int _id;
        private readonly IMessageBoxCreator _messageBoxCreator;
        private readonly ChatClient _chatClient;
        private readonly UserClient _userClient;
        private readonly SoundPlayer _player = new SoundPlayer(Directory.GetCurrentDirectory() + @"\Sounds\noti.wav");
        private bool _isFirstGettingUpdates = true;
        private bool _isDisposed;
        public string UrlPathSegment => "userChats";

        private ChatModel OpenedChat { get; set; }
        public IScreen HostScreen { get; }

        [Reactive] public string CompanionUsername { get; set; }
        [Reactive] public ChatModel SelectedChat { get; set; }
        [Reactive] public MessageModel SelectedMessage { get; set; }
        [Reactive] public UserModel SelectedUserInSearch { get; set; }
        [Reactive] public string MessageText { get; set; }
        [Reactive] public string UserSearchText { get; set; }
        [Reactive] private bool IsEditing { get; set; }
        [Reactive] public bool IsSearchingOpen { get; set; }
        [Reactive] public bool TextBlockZeroChatsVisibility { get; set; }
        [Reactive] public bool ListViewLastMessagesVisibility { get; set; }
        [Reactive] public bool StackPanelZeroMessagesVisibility { get; set; }
        [Reactive] public bool ListViewMessagesVisibility { get; set; }
        [Reactive] public bool ManipulatingPanelVisibility { get; set; }
        [Reactive] public bool CancelButtonVisibility { get; set; }
        [Reactive] public bool MessagesGridVisibility { get; set; }
        [Reactive] public bool ChatProgressBarVisibility { get; set; } = true;
        [Reactive] public bool MessagesProgressBarVisibility { get; set; } = true;

        [Reactive] public ObservableCollection<ChatModel> Chats { get; set; } = new ObservableCollection<ChatModel>();
        [Reactive] public ObservableCollection<UserModel> Users { get; set; }

        [Reactive] public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public ReactiveCommand<Unit, Unit> DeletingMessageCommand { get; }
        public ReactiveCommand<Unit, Unit> CreatingMessageCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangingToEditingCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshingMessagesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public UserChatsViewModel(int id)
        {
            _chatClient = new ChatClient();
            _userClient = new UserClient();
            _id = id;
            _messageBoxCreator = Locator.Current.GetService<IMessageBoxCreator>();

            HostScreen = Locator.Current.GetService<IScreen>();

            this.WhenAnyValue(x => x.TextBlockZeroChatsVisibility).Subscribe(x =>
            {
                ListViewLastMessagesVisibility = !x;
            });
            this.WhenAnyValue(x => x.IsEditing).Subscribe(x =>
            {
                CancelButtonVisibility = x;
            });
            this.WhenAnyValue(x => x.SelectedMessage).Subscribe(x =>
            {
                ManipulatingPanelVisibility = (x != null && x.SenderId == _id);
            });
            this.WhenAnyValue(x => x.StackPanelZeroMessagesVisibility).Subscribe(x =>
            {
                ListViewMessagesVisibility = !x;
            });
            Messages.CollectionChanged += (obj, args) =>
            {
                StackPanelZeroMessagesVisibility = (Messages == null || Messages.Count == 0);
            };
            Chats.CollectionChanged += (obj, args) =>
            {
                TextBlockZeroChatsVisibility = (Chats == null || Chats.Count == 0);
            };
            this.WhenAnyValue(x => x.SelectedUserInSearch).Subscribe(y =>
            {
                if (y == null) return;

                var chat = new ChatModel
                {
                    Id = y.Id,
                    Username = y.Username
                };

                OpenChat(chat);
                UserSearchText = string.Empty;
            });
            this.WhenAnyValue(x => x.SelectedChat).Subscribe(OpenChat);
            this.WhenAnyValue(x => x.UserSearchText).Subscribe(SearchUsers);
            
            ChangingToEditingCommand = ReactiveCommand.Create(ChangeToEditing);
            DeletingMessageCommand = ReactiveCommand.CreateFromTask(DeleteMessage);
            CreatingMessageCommand = ReactiveCommand.CreateFromTask(UploadMessage);
            RefreshingMessagesCommand = ReactiveCommand.CreateFromTask(Refresh);
            CancelCommand = ReactiveCommand.Create(CancelUpdate);

            MessageBus.Current.Listen<DisposeMessage>().Subscribe(x => Dispose());
        }

        public async Task Init()
        {
            var username = await _userClient.GetUsernameById(_id);
            if (username == null)
            {
                _messageBoxCreator.CreateMessageBox("Произошла ошибка");
                return;
            }

            MessageBus.Current.SendMessage(new UsernameChangingMessage(username));

            var chats = await _chatClient.GetUserChats(_id);

            var chatModels = chats.Select(Map);

            ChatProgressBarVisibility = false;

            _ = Task.Run(GetUpdates);

            if (chats.Count == 0) return;

            Chats.AddRange(chatModels);

        }

        public async Task GetUpdates()
        {
            while (true)
            {
                try
                {
                    if (_isDisposed) return;

                    var result = await _chatClient.GetUpdates(_id);

                    if (result == null) continue;

                    var chats = await _chatClient.GetUserChats(_id);
                    var chatModels = chats.Select(Map);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Chats.Clear();
                        Chats.AddRange(chatModels);
                    });

                    if (chats.Count != 0)
                    {
                        foreach (var chat in result.Select(itemId => Chats.FirstOrDefault(x => x.Id == itemId)))
                        {
                            if (chat == null) continue;

                            chat.NewMessageSignVisibility = Visibility.Visible;
                        }
                    }

                    if (OpenedChat != null && result.Contains(OpenedChat.Id))
                    {
                        var messages = await _chatClient.GetMessages(_id, OpenedChat.Id);

                        var messageModels = messages.Select(Map);

                        var chat = Chats.FirstOrDefault(x => x.Id == OpenedChat.Id);
                        chat.NewMessageSignVisibility = Visibility.Collapsed;

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Messages.Clear();
                            Messages.AddRange(messageModels);
                            SelectedMessage = Messages.LastOrDefault();
                            SelectedMessage = null;
                        });
                    }
                    else
                    {
                        if (_isFirstGettingUpdates)
                        {
                            _isFirstGettingUpdates = false;
                            continue;
                        }
                        _player.Play();
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    _messageBoxCreator.CreateMessageBox(e.Message);
                }
            }
        }

        private async void OpenChat(ChatModel selectedChat)
        {
            if (selectedChat == null) return;

            OpenedChat = selectedChat;
            CompanionUsername = selectedChat.Username;

            MessagesGridVisibility = true;
            var chat = Chats.FirstOrDefault(x => x.Id == selectedChat.Id);
            MessagesProgressBarVisibility = false;

            //Если открывается диалог с новым пользователем
            if (chat == null) return;

            chat.NewMessageSignVisibility = Visibility.Collapsed;

            await Refresh();
        }

        public async Task Refresh()
        {
            var chat = await _chatClient.GetMessages(_id, OpenedChat.Id);

            MessagesProgressBarVisibility = false;

            if (chat.Count == 0) return;

            var chatModels = chat.Select(Map);

            Messages.Clear();
            Messages.AddRange(chatModels);

            SelectedMessage = Messages.LastOrDefault();
            SelectedMessage = null;
        }

        private async Task UploadMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText)) return;

            var result = IsEditing ? await UpdateMessage() : await CreateMessage();
            IsEditing = false;

            if (result == 0)
            {
                _messageBoxCreator.CreateMessageBox("Произошла ошибка");
                return;
            }

            MessageText = string.Empty;

            SelectedMessage = Messages.LastOrDefault();
            SelectedMessage = null;
        }

        private async Task<int> CreateMessage()
        {
            var message = new MessageApiModel
            {
                SenderId = _id,
                ReceiverId = OpenedChat.Id,
                Text = MessageText.Trim(),
            };

            var result = await _chatClient.CreateMessage(message);

            if (result == 0) return result;

            var messageModel = new MessageModel
            {
                DateTimeOfDelivery = DateTime.Now,
                Id = result,
                SenderId = _id,
                Text = MessageText,
                ReceiverUsername = OpenedChat.Username,
                SenderUsername = App.Username
            };

            Messages.Add(messageModel);

            var newChat = new ChatModel
            {
                Username = messageModel.ReceiverUsername,
                DateTimeOfLastMessage = messageModel.DateTimeOfDelivery,
                Id = OpenedChat.Id,
                LastMessage = messageModel.Text,
                SenderUsernameOfMessage = messageModel.SenderUsername
            };

            var oldChat = Chats.FirstOrDefault(x => x.Id == OpenedChat.Id);

            if (oldChat == null)
            {
                Chats.Insert(0, newChat);
            }
            else
            {
                var index = Chats.IndexOf(oldChat);
                Chats[index] = newChat;
                var orderedChats = Chats.OrderByDescending(x => x.DateTimeOfLastMessage).ToArray();
                Chats.Clear();
                Chats.AddRange(orderedChats);
            }
            OpenedChat = newChat;

            return result;
        }

        private async Task<int> UpdateMessage()
        {
            var message = new MessageApiModel
            {
                Id = SelectedMessage.Id,
                SenderId = _id,
                ReceiverId = OpenedChat.Id,
                Text = MessageText.Trim(),
            };

            var result = await _chatClient.UpdateMessage(message);

            var oldMessage = SelectedMessage.Text;

            if (SelectedMessage != null)
            {
                SelectedMessage.Text = message.Text;
            }
            else
            {
                _messageBoxCreator.CreateMessageBox("Ошибка в обновлении сообщения во внутреннем списке");
                return result;
            }

            if (OpenedChat != null && OpenedChat.DateTimeOfLastMessage == SelectedMessage.DateTimeOfDelivery
                                     && OpenedChat.LastMessage == oldMessage)
            {
                var index = Chats.IndexOf(OpenedChat);
                Chats[index].LastMessage = message.Text;
            }

            return result;
        }

        private async Task DeleteMessage()
        {
            var messageBoxResult = _messageBoxCreator.CreateMessageBox("Удалить сообщение?", "Внимание", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) return;
            
            if (SelectedMessage == null) return;

            var result = await _chatClient.DeleteMessage(SelectedMessage.Id);
            if (result == 0)
            {
                _messageBoxCreator.CreateMessageBox("Произошла ошибка");
                return;
            }

            var deletedMessage = SelectedMessage;

            Messages.Remove(SelectedMessage);

            if (Messages.Count == 0)
            {
                Chats.Remove(OpenedChat);
                return;
            }

            if (OpenedChat != null && OpenedChat.DateTimeOfLastMessage == deletedMessage.DateTimeOfDelivery
                                   && OpenedChat.LastMessage == deletedMessage.Text)
            {
                var lastMessage = Messages.Last();

                var chat = new ChatModel
                {
                    Username = OpenedChat.Username,
                    DateTimeOfLastMessage = lastMessage.DateTimeOfDelivery,
                    Id = OpenedChat.Id,
                    LastMessage = lastMessage.Text,
                    SenderUsernameOfMessage = lastMessage.SenderUsername
                };

                var oldChat = Chats.FirstOrDefault(x => x.Id == OpenedChat.Id);
                if (oldChat == null)
                {
                    _messageBoxCreator.CreateMessageBox("Ошибка при загрузке нахождении чата");
                    return;
                }

                var index = Chats.IndexOf(oldChat);
                Chats[index] = chat;

                var orderedChats = Chats.OrderByDescending(x => x.DateTimeOfLastMessage).ToArray();
                Chats.Clear();
                Chats.AddRange(orderedChats);

                OpenedChat = chat;
            }

        }

        private void ChangeToEditing()
        {
            MessageText = SelectedMessage.Text;
            IsEditing = true;
        }

        public void CancelUpdate()
        {
            MessageText = string.Empty;
            IsEditing = false;
        }

        public async void SearchUsers(string x)
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

        public ChatModel Map(Chat chat)
        {
            return new ChatModel
            {
                Id = chat.Id,
                Username = chat.Username,
                SenderUsernameOfMessage = chat.SenderUsernameOfMessage,
                DateTimeOfLastMessage = chat.DateTimeOfLastMessage,
                LastMessage = chat.LastMessage
            };
        }

        public MessageModel Map(MessageDto message)
        {
            return new MessageModel
            {
                Id = message.Id,
                ReceiverUsername = message.ReceiverUsername,
                DateTimeOfDelivery = message.DateTimeOfDelivery,
                Text = message.Text,
                SenderId = message.SenderId,
                SenderUsername = message.SenderUsername
            };
        }
    }
}