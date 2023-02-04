using System;
using System.Collections.Generic;

namespace Bulimia.MessengerServer.DAL.Models;

public partial class Message
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public DateTime DateTimeDelivery { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
