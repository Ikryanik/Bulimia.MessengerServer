using Bulimia.MessengerServer.BLL.Services;
using Bulimia.MessengerServer.Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Bulimia.MessengerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MessagesController
{
    private readonly ChatService _chatService;

    public MessagesController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet(Name = "GetMessages")]
    public Task<List<MessageDto>> GetMessages()
    {
        return _chatService.GetAllMessages();
    }

    [HttpPost(Name = "CreateMessage")]
    public Task<int> CreateMessage(MessageModel messageModel)
    {
        return _chatService.CreateMessage(messageModel);
    }

    [HttpPost(Name = "DeleteMessage")]
    public Task<int> DeleteMessage(int id)
    {
        return _chatService.DeleteMessage(id);
    }

    [HttpPost(Name = "UpdateMessage")]
    public Task<int> UpdateMessage(MessageModel messageModel)
    {
        return _chatService.UpdateMessage(messageModel);
    }

    [HttpPost(Name = "GetUserChat")]
    public Task<List<MessageRecord>> GetUserChat(UserChatRequest request)
    {
        return _chatService.GetUserChat(request);
    }

    [HttpPost(Name = "GetChatsOfUser")]
    public Task<List<Chat>> GetChatsOfUser(int id)
    {
        return _chatService.GetChatsOfUser(id);
    }
}
