using BulletInBoardServer.Models.Announcements.Categories.Subscribers;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Categories;

public class AnnouncementCategory(Guid id, Subscribers.Subscribers subscribers)
{
    public Guid Id { get; } = id;
    public ReadOnlySubscribers Subscribers => subscribers.AsReadOnly();



    public AnnouncementCategory(Guid id) : this(id, new Subscribers.Subscribers())
    {
    }

    public AnnouncementCategory(Subscribers.Subscribers subscribers) : this(Guid.NewGuid(), subscribers)
    {
    }



    public void Subscribe(User user)
    {
        if (subscribers.Contains(user))
            throw new InvalidOperationException("Пользователь уже подписан на категорию объявлений");
        
        subscribers.Add(user);
    }
    
    public void Unsubscribe(User user)
    {
        subscribers.Remove(user);
    }
}