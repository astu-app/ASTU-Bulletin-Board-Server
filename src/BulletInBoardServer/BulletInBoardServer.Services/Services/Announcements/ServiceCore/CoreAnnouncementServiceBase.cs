using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

// public class CoreAnnouncementServiceBase(ApplicationDbContext dbContext)
public class CoreAnnouncementServiceBase(IServiceScopeFactory scopeFactory)
{
    /// <summary>
    /// Метод загружает объявление без связанных сущностей
    /// </summary>
    /// <param name="announcementId">Id загружаемого объявления</param>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <returns>Краткая информация об объявлении</returns>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="InvalidOperationException">Не удалось загрузить объявление из БД</exception>
    protected static Announcement GetAnnouncementSummary(Guid announcementId, ApplicationDbContext dbContext)
    {
        try
        {
            var announcement = dbContext.Announcements.SingleOrDefault(a => a.Id == announcementId);
            if (announcement is null)
                throw new AnnouncementDoesNotExistException($"Объявление с Id = {announcementId} отсутствует в БД");

            return announcement;
        }
        catch (InvalidOperationException err) when (err is not AnnouncementDoesNotExistException)
        {
            throw new InvalidOperationException("Не удалось загрузить объявление из БД", err);
        }
    }

    /// <summary>
    /// Загрузка краткой информации о связанных с объявлением опросах
    /// </summary>
    /// <param name="announcement">Объявление, опросы которого требуется загрузить</param>
    /// <param name="requesterId">Id пользователя, запросившего загрузку</param>
    /// <param name="dbContext">Контекст базы данных</param>
    protected static void LoadAnnouncementSurveys(Announcement announcement, Guid requesterId, ApplicationDbContext dbContext)
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

    protected IServiceScope CreateScope() =>
        scopeFactory.CreateScope();
    
    protected static ApplicationDbContext GetDbContextForScope(IServiceScope scope) =>
        scope.ServiceProvider.GetService<ApplicationDbContext>() ??
        throw new ApplicationException(
            $"{nameof(ApplicationDbContext)} не зарегистрирован в качестве сервиса");
}