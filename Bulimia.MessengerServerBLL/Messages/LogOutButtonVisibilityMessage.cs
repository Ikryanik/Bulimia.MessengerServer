namespace Bulimia.MessengerClient.Messages
{
    public class LogOutButtonVisibilityMessage
    {
        public bool IsVisible { get; set; }

        public LogOutButtonVisibilityMessage(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}