using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments;

public class ReadOnlyAttachmentList(IList<AttachmentBase> list)
    : ReadOnlyCollection<AttachmentBase>(list);