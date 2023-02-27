
namespace Bulimia.MessengerClient.DAL
{
    public static class Api
    {
        private const string ServerName = "https://localhost:7014";
        public static string Authenticate = ServerName + "/UserManager/Authenticate";
        public static string Register = ServerName + "/UserManager/Register";
        public static string GetChats = ServerName + "/Messages/GetChatsOfUser";
        public static string GetMessages = ServerName + "/Messages/GetUserChat";
        public static string GetUsernameById = ServerName + "/Users/GetUsernameById";
        public static string CreateMessage = ServerName + "/Messages/CreateMessage";
        public static string DeleteMessage = ServerName + "/Messages/DeleteMessage";
    }
}