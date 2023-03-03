using System.Windows;
using Bulimia.MessengerClient.BLL;
using Bulimia.MessengerClient.ViewModel;

namespace Bulimia.MessengerClient.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       private readonly UserManagerClient _userManagerClient;
       public MainWindow()
       {
           _userManagerClient = new UserManagerClient();
           InitializeComponent();
           DataContext = new MainWindowViewModel();
       }
       
       private async void ButtonAuthenticate_OnClick(object sender, RoutedEventArgs e)
       {
           var username = TextBoxUsername.Text.Trim();
       
           if (string.IsNullOrWhiteSpace(username))
           {
               MessageBox.Show("Имя не может быть пустым");
               return;
           }
       
           var result = await _userManagerClient.Authenticate(username);
       
           if (result == null)
           {
               MessageBox.Show("Неверное имя");
               return;
           }
       
           var userChatsWindow = new UserChatsWindow(result.Id);
           userChatsWindow.Show();
           await userChatsWindow.Init();
           Close();
       }
       
       
       private void LinkChangeButtonToRegister_OnClick(object sender, RoutedEventArgs e)
       {
           ButtonAuthenticate.Visibility = Visibility.Collapsed;
           ButtonRegister.Visibility = Visibility.Visible;
       
           LabelChangeAuthenticateButton.Visibility = Visibility.Collapsed;
           LabelChangeRegisterButton.Visibility = Visibility.Visible;
       }
       
       private void LinkChangeButtonToAuthenticate_OnClick(object sender, RoutedEventArgs e)
       {
           ChangeLinksAndButtonsToAuthenticate();
       }
       
       public void ChangeLinksAndButtonsToAuthenticate()
       {
           ButtonAuthenticate.Visibility = Visibility.Visible;
           ButtonRegister.Visibility = Visibility.Collapsed;
       
           LabelChangeAuthenticateButton.Visibility = Visibility.Visible;
           LabelChangeRegisterButton.Visibility = Visibility.Collapsed;
       }
    }
}
