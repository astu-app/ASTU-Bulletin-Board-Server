using System.Text;

namespace BulletInBoardServer.Services.Services.Notifications;

public class NotificationService
{
    private readonly string _applicationToken;
    private readonly string _notificationMessageServerHost;
    
    private readonly Random _random = new();

    
    
    public NotificationService(string applicationToken, string notificationMessageServerHost)
    {
        _applicationToken = applicationToken;
        _notificationMessageServerHost = notificationMessageServerHost;
    }



    public virtual void NotifyAll(IEnumerable<Guid> receiverIds, string title, string message, int priority = 0) =>
        receiverIds
            .AsParallel()
            .ForAll(receiverId => Notify(receiverId, title, message, priority));

    public virtual void Notify(Guid receiverId, string title, string message, int priority = 0)
    {
        var messageStr =
            $$"""
              {
                  "id": {{_random.NextInt64()}},
                  "title": "{{title}}",
                  "message": "{{message}}",
                  "priority": {{priority}},
                  "extras": {
                      "receiver::options": {
                          "user_id": "{{receiverId}}"
                      }
                  }
              }
              """;
        
        var httpClient = new HttpClient();
        var uri = $"{_notificationMessageServerHost}/message?token={_applicationToken}";
        Console.WriteLine(uri);
        var response = httpClient.PostAsync(uri, new StringContent(messageStr, Encoding.UTF8, "application/json")).Result;

        var res = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(res);
        Console.WriteLine(response.StatusCode);
    }
}