namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AutoHidingAnAlreadyHiddenAnnouncementException()
    : InvalidOperationException("Нельзя задать срок автоматического сокрытия уже скрытому объявлению");