using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.Domain.Core;
using Bulimia.MessengerClient.ViewModel;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Bulimia.MessengerClient.View
{
    /// <summary>
    /// Логика взаимодействия для UserChatsWindow.xaml
    /// </summary>
    public partial class UserChatsWindow : ReactiveUserControl<UserChatsViewModel>
    {

        public UserChatsWindow()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Users,
                    view => view.ComboBoxUsers.ItemsSource);

                this.Bind(ViewModel,
                    viewModel => viewModel.UserSearchText,
                    view => view.ComboBoxUsers.Text);

                this.Bind(ViewModel,
                    viewModel => viewModel.IsSearchingOpen,
                    view => view.ComboBoxUsers.IsDropDownOpen);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedUserInSearch,
                    view => view.ComboBoxUsers.SelectedItem);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Chats,
                    view => view.ListViewLastMessages.ItemsSource)
                    .DisposeWith(disposables);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedChat,
                    view => view.ListViewLastMessages.SelectedItem)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.ChatProgressBarVisibility,
                    view => view.ChatProgressBar.Visibility)
                    .DisposeWith(disposables);

                this.Bind(ViewModel,
                    viewModel => viewModel.TextBlockZeroChatsVisibility,
                    view => view.TextBlockZeroChats.Visibility)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.ListViewLastMessagesVisibility,
                    view => view.ListViewLastMessages.Visibility)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.MessagesGridVisibility,
                    view => view.MessagesGrid.Visibility)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.ListViewMessagesVisibility,
                    view => view.ListViewMessages.Visibility)
                    .DisposeWith(disposables);
                
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Messages,
                    view => view.ListViewMessages.ItemsSource)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.MessageText,
                    view => view.TextBoxMessage.Text)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.CompanionUsername,
                    view => view.CompanionUsernameHeader.Text)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedMessage,
                    view => view.ListViewMessages.SelectedItem)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.StackPanelZeroMessagesVisibility,
                    view => view.StackPanelZeroMessages.Visibility)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.MessagesProgressBarVisibility,
                    view => view.MessagesProgressBar.Visibility)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.ManipulatingPanelVisibility,
                    view => view.PanelItemManipulating.Visibility)
                    .DisposeWith(disposables);
                
                this.Bind(ViewModel,
                    viewModel => viewModel.CancelButtonVisibility,
                    view => view.CancelUpdateButton.Visibility)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel,
                    viewModel => viewModel.CreatingMessageCommand,
                    view => view.CreateMessageButton)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel,
                    viewModel => viewModel.RefreshingMessagesCommand,
                    view => view.RefreshButton)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel,
                    viewModel => viewModel.DeletingMessageCommand,
                    view => view.DeleteMessageButton)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel,
                    viewModel => viewModel.ChangingToEditingCommand,
                    view => view.UpdateMessageButton)
                    .DisposeWith(disposables);
                
                this.BindCommand(ViewModel,
                          viewModel => viewModel.CancelCommand,
                          view => view.CancelUpdateButton)
                    .DisposeWith(disposables);

                this.WhenAnyValue(x => x.ViewModel.SelectedMessage).Subscribe(x =>
                {
                    if (x != null)
                    {
                        ListViewMessages.ScrollIntoView(ListViewMessages.SelectedItem);
                    }
                });
            });
        }


    }


}
