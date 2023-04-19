using System.Security.Cryptography.X509Certificates;
using Bulimia.MessengerServer.DAL.Models;
using Bulimia.MessengerServer.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Bulimia.MessengerServer.DAL.Repositories;

public class MessageRepository
{
    private readonly MessengerContext _context;
    public static List<int> ChatsUpdatesList { get; set; } = new();
    public static Dictionary<int, List<int>> UpdatesList { get; set; } = new();

    public MessageRepository(MessengerContext context)
    {
        _context = context;
    }

    public async Task<List<MessageDto>> GetAllMessages()
    {
        var messages = await _context.Messages.ToListAsync();

        return messages.Select(Map).ToList();
    }

    public async Task<int> CreateMessage(MessageModel messageModel)
    {
        var result = await _context.Messages.AddAsync(Map(messageModel));
        await _context.SaveChangesAsync();

        AddUpdatesInUpdatesList(messageModel.SenderId, messageModel.ReceiverId);

        return result.Entity.Id;
    }

    public void AddUpdatesInUpdatesList(int senderId, int receiverId)
    {
        if (!UpdatesList.ContainsKey(receiverId))
        {
            UpdatesList.TryAdd(receiverId, new List<int>());
        }

        if (UpdatesList[receiverId].Contains(senderId))
        {
            return;
        }

        UpdatesList[receiverId].Add(senderId);
    }

    public async Task<int> DeleteMessage(int id)
    {
        var result = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);

        if (result == null) return 0;

        _context.Messages.Remove(result);
        await _context.SaveChangesAsync();

        AddUpdatesInUpdatesList(result.SenderId, result.ReceiverId);

        return 1;
    }

    public async Task<MessageModel?> GetMessageById(int messageId)
    {
        var result = await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);

        return result == null ? null : MapMessage(result);
    }

    public async Task<int> UpdateMessage(MessageModel messageModel)
    {
        var result = _context.Messages.FirstOrDefault(x => x.Id == messageModel.Id);

        if (result == null) return 0;

        result.Id = messageModel.Id;
        result.ReceiverId = messageModel.ReceiverId;
        result.SenderId = messageModel.SenderId;
        result.Text = messageModel.Text;

        await _context.SaveChangesAsync();

        AddUpdatesInUpdatesList(result.SenderId, result.ReceiverId);

        return 1;
    }

    public async Task<List<MessageRecord>> GetUserChat(UserChatRequest request)
    {
        var result = await _context.Messages.Where(x =>
            x.ReceiverId == request.ReceiverId && x.SenderId == request.SenderId ||
            x.ReceiverId == request.SenderId && x.SenderId == request.ReceiverId
        ).ToListAsync();

        return result.Select(x => new MessageRecord
        {
            Id = x.Id,
            DateTimeOfDelivery = x.DateTimeDelivery,
            Text = x.Text,
            ReceiverId = x.ReceiverId,
            SenderId = x.SenderId
        }
        ).OrderByDescending(x => x.DateTimeOfDelivery).ToList();
    }

    public async Task<List<int>> GetChatsByUserId(int id)
    {
        var result = await _context.Messages
            .Where(x =>
                x.SenderId == id || x.ReceiverId == id)
            .Select(x =>
                x.SenderId == id ? x.ReceiverId : x.SenderId)
            .Distinct().ToListAsync();

        return result;
    }

    public async Task<Chat?> GetChat(int userId, int companionId)
    {
        var result = await _context.Messages.Where(x =>
            x.ReceiverId == userId && x.SenderId == companionId || x.ReceiverId == companionId && x.SenderId == userId)
            .OrderBy(x => x.DateTimeDelivery)
            .LastOrDefaultAsync();

        var companion = await _context.Users.FirstOrDefaultAsync(x => x.Id == companionId);

        if (result == null) return null;

        if (companion == null) return null;

        return new Chat
        {
            Id = companionId,
            Username = companion.Username,
            DateTimeOfLastMessage = result.DateTimeDelivery,
            LastMessage = result.Text,
            SenderUsernameOfMessage = result.Sender.Username
        };
    }

    public bool GetUpdatesInChats(int id)
    {
        try
        {
            var result = ChatsUpdatesList.Contains(id);

            if (result)
            {
                Console.WriteLine("в ChatsUpdatesList есть этот айди");
                ChatsUpdatesList?.Remove(id);
            }

            return result;
        }
        catch
        {
            return false;
        }
    }

    public List<int>? GetUpdatesInMessages(int id)
    {
        try
        {

            var result = UpdatesList.ContainsKey(id);

            if (!result) return null;

            var ids = UpdatesList[id];

            UpdatesList.Remove(id);
            return ids;
        }
        catch
        {
            return null;
        }
    }

    private MessageModel MapMessage(Message message)
    {
        return new MessageModel
        {
            Id = message.Id,
            ReceiverId = message.ReceiverId,
            SenderId = message.SenderId,
            Text = message.Text
        };
    }

    private MessageDto Map(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            Text = message.Text,
        };
    }

    private Message Map(MessageModel messageModel)
    {
        return new Message()
        {
            DateTimeDelivery = DateTime.Now,
            ReceiverId = messageModel.ReceiverId,
            SenderId = messageModel.SenderId,
            Text = messageModel.Text
        };
    }

}

