using System;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bulimia.MessengerClient.Model
{
    public class ChatModel : ReactiveObject
    {
        public int Id { get; set; }
        [Reactive] public string Username { get; set; }
        [Reactive] public DateTime DateTimeOfLastMessage { get; set; }
        [Reactive] public string LastMessage { get; set; }
        [Reactive] public string SenderUsernameOfMessage { get; set; }
        [Reactive] public Visibility NewMessageSignVisibility { get; set; } = Visibility.Collapsed;
    }
}