﻿using Flunt.Notifications;

namespace ProdutosApp.Domain;

public abstract class Entity :Notifiable<Notification>
{

    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string EditedBy { get; set; }
    public DateTime EditedOn { get; set; }
}
