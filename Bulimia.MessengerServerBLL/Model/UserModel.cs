using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bulimia.MessengerClient.Model
{
    public class UserModel : ReactiveObject
    {
        [Reactive] public string Username { get; set; }
    }
}