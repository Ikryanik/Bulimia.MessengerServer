using Bulimia.MessengerClient.Model;

namespace Bulimia.MessengerClient.ViewModel
{
    public class MainWindowViewModel
    {
        private UserModel User { get; set; }
        public string Username { get; set; }

        public MainWindowViewModel()
        {
            User = new UserModel();
        }
    }
}