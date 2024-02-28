namespace BulletInBoardServer.Domain.Models.AnnouncementCategories.Exceptions;

public class ColorIsInvalidHexException() : FormatException("Цвет представлен в формате, отличном от HEX");