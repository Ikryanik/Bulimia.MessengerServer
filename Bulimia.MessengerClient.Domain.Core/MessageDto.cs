using System;
using System.Windows;
using System.Windows.Media;

namespace Bulimia.MessengerClient.Domain.Core
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public DateTime DateTimeOfDelivery { get; set; }
        public string Text { get; set; }

        public Brush Brush
        {
            get
            {
                if (SenderId == myId)
                    return Brushes.Aqua;
                return Brushes.BlueViolet;
            }
        }
        private int myId { get; set; }
        public MessageDto(int myId)
        {
            this.myId = myId;
        }

        public Visibility ManipulatingButtonsVisibility => myId == SenderId ? Visibility.Visible : Visibility.Collapsed;
    }
}