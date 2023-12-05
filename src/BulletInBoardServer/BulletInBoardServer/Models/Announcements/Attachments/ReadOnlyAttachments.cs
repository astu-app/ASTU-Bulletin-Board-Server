using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments;

public class ReadOnlyAttachments(IList<IAttachment> list) : ReadOnlyCollection<IAttachment>(list);