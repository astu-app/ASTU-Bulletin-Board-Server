using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class PublishedAnnouncementService(
    IServiceScopeFactory scopeFactory,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : DispatcherDependentAnnouncementServiceBase(scopeFactory, dispatcher)
{
    /// <summary>
    /// Метод возвращает список опубликованных для пользователя объявлений
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetListForUser(Guid requesterId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);

        var announcementGroups = dbContext.Announcements
            .Include(a => a.Author)
            .Include(a => a.Attachments)
            .Join(dbContext.AnnouncementAudience,
                a => a.Id, au => au.AnnouncementId,
                (a, au) => new { Announcement = a, Audience = au })
            .Where(res =>
                res.Announcement.IsPublished && (res.Audience.UserId == requesterId ||
                                                 res.Announcement.AuthorId == requesterId))
            .GroupBy(res => res.Announcement.Id)
            .ToList();
        
       // foreach (var group in announcementGroups)
       // {
       //     // так как группируем по Id объявления и все объявления группы будут содержать одно и то же объявление,
       //     // из группы выбираем объявление первого элемента
       //     var announcement = group.First().Announcement;
       //     
       //     // Так как к объявлению могут быть прикреплены разные типы вложений и присутствует необходимость загрузить
       //     // связанные с этими вложениями сущности, для каждого из типов вложений используется явная загрузка.
       //     dbContext.Entry(announcement).Collection(a => a.Attachments)
       //         .Query()
       //         .Where(a => a.Type == AttachmentTypes.Survey)
       //         .Cast<Survey>()
       //         .Include(s => s.Voters)
       //         .Include(s => s.Questions)
       //         .ThenInclude(q => q.Answers)
       //         .ThenInclude(q => q.Participation)
       //         .Load();
       // }
       
       // Так как к объявлению могут быть прикреплены разные типы вложений и присутствует необходимость загрузить
       // связанные с этими вложениями сущности, для каждого из типов вложений используется явная загрузка.
       foreach (var announcement in announcementGroups.Select(group => group.First().Announcement))
       {
           dbContext.Entry(announcement).Collection(a => a.Attachments)
               .Query()
               .Where(a => a.Type == AttachmentTypes.Survey)
               .Cast<Survey>()
               .Include(s => s.Voters)
               .Include(s => s.Questions)
               .ThenInclude(q => q.Answers)
               .ThenInclude(q => q.Participation)
               .Load();
           
           foreach (var survey in announcement.Attachments.OfType<Survey>()) 
               survey.IsVotedByRequester = survey.Voters.Any(v => v.Id == requesterId);
       }

       var summaries = announcementGroups
           .Select(group => group.First().Announcement.GetSummary( 
               group.Count(g => g.Audience.Viewed)))
           .OrderByDescending(a => a.PublishedAt)
           .ToList(); 
       return summaries;
    }

    /// <summary>
    /// Метод скрывает объявление указанным пользователем
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <param name="announcementId">Id объявления, которое требуется скрыть</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void HideManually(Guid requesterId, Guid announcementId, DateTime hiddenAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (announcement.AuthorId != requesterId)
            throw new OperationNotAllowedException("Объявление может скрыть только его автор");

        if (announcement.ExpectsDelayedHiding)
            Dispatcher.DisableDelayedHiding(announcementId);

        announcement.Hide(DateTime.Now, hiddenAt);
        dbContext.SaveChanges();
    }
}