using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Bulimia.MessengerClient.Model
{
    public class UserModel : ReactiveObject
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
        
        
    }
}