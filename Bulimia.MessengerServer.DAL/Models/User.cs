using System;
using System.Collections.Generic;

namespace Bulimia.MessengerServer.DAL.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<Message> MessageReceivers { get; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; } = new List<Message>();
}
