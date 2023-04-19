using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bulimia.MessengerClient.Model
{
    public class MessageModel : ReactiveObject
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public DateTime DateTimeOfDelivery { get; set; }
        [Reactive] public string Text { get; set; }

    }
}