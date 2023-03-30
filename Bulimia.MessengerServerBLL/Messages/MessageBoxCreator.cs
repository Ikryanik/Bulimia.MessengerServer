using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Bulimia.MessengerClient.Messages
{
    public class MessageBoxCreator : IMessageBoxCreator
    {
        public MessageBoxResult CreateMessageBox(string text, string title, MessageBoxButton messageBoxButton)
        {
            var result = MessageBox.Show(text, title, messageBoxButton);
            return result;
        }

        public void CreateMessageBox(string text)
        {
            MessageBox.Show(text);
        }
    }
}