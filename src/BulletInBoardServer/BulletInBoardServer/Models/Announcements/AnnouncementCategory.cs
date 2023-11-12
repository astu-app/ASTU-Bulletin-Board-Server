namespace BulletInBoardServer.Models.Announcements.Attachments;

public class AnnouncementCategory
{
    public Guid Id { get; set; }
    public Subscribers Subscribers { get; set; }



    public AnnouncementCategory(Guid id, Subscribers subscribers)
    {
        Id = id;
        Subscribers = subscribers;
    }

    public AnnouncementCategory(Guid id) : this(id, new Subscribers())
    {
    }

    public AnnouncementCategory(Subscribers subscribers) : this(Guid.NewGuid(), subscribers)
    {
    }
}