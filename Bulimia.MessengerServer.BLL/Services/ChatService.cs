using Bulimia.MessengerServer.DAL.Repositories;
using Bulimia.MessengerServer.Domain.Core;

namespace Bulimia.MessengerServer.BLL.Services;

public class ChatService
{
    private readonly MessageRepository _messageRepository;
    private readonly UserRepository _userRepository;

    public ChatService(MessageRepository messageRepository, UserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<List<MessageDto>> GetAllMessages()
    {
        return await _messageRepository.GetAllMessages();
    }

    public async Task<int> CreateMessage(MessageModel messageModel)
    {
        await Validate(messageModel);

        return await _messageRepository.CreateMessage(messageModel);
    }

    public Task<int> DeleteMessage(int id)
    {
        return _messageRepository.DeleteMessage(id);
    }

    public async Task<int> UpdateMessage(MessageModel messageModel)
    {
        await ValidateUpdate(messageModel);

        return await _messageRepository.UpdateMessage(messageModel);
    }

    private async Task Validate(MessageModel messageModel)
    {
        var result = string.Empty;
        var receiver = await _userRepository.GetUserById(messageModel.ReceiverId);

        if (receiver == null)
            result = "Получателя не существует";

        var sender = await _userRepository.GetUserById(messageModel.SenderId);

        if (sender == null)
            result += "\nОтправителя не существует";

        if (string.IsNullOrWhiteSpace(messageModel.Text))
            result += "\nСообщение не может быть пустым";

        if (!string.IsNullOrWhiteSpace(result))
            throw new Exception(result);
    }

    private async Task ValidateUpdate(MessageModel messageModel)
    {
        var result = string.Empty;

        var oldMessage = await _messageRepository.GetMessageById(messageModel.Id);

        if (oldMessage == null) throw new Exception("Сообщения не существует");

        if (oldMessage.SenderId != messageModel.SenderId)
            result += "\nОтправитель не может быть изменен";

        if (oldMessage.ReceiverId != messageModel.ReceiverId)
            result += "\nПолучатель не может быть изменен";

        if (string.IsNullOrWhiteSpace(messageModel.Text))
            result += "\nСообщение не может быть пустым";

        if (!string.IsNullOrWhiteSpace(result))
            throw new Exception(result);
    }

    public Task<List<MessageRecord>> GetUserChat(UserChatRequest request)
    {
        return _messageRepository.GetUserChat(request);
    }

    public async Task<List<Chat>> GetChatsOfUser(int id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new Exception("Пользователя не существует");

        var result = await _messageRepository.GetChatsByUserId(id);

        var chatList = new List<Chat>();

        foreach (var ids in result)
        {
            var chat = await _messageRepository.GetChat(id, ids);

            if (chat != null)
                chatList.Add(chat);
        }

        return chatList;
    }
}