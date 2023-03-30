using System.Windows;
using System.Windows.Forms;

namespace Bulimia.MessengerClient.Messages
{
    public interface IMessageBoxCreator
    {
        MessageBoxResult CreateMessageBox(string text, string title, MessageBoxButton messageBoxButton);
        void CreateMessageBox(string text);
    }
}