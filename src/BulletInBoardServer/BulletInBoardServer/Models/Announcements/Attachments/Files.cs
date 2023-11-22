using System.Collections;

namespace BulletInBoardServer.Models.Announcements.Attachments;

public class Files : IEnumerable<File>
{
    private readonly IEnumerable<File> _files;



    public Files(IEnumerable<File> files) => 
        _files = files;

    public Files() => 
        _files = new List<File>();



    public IEnumerator<File> GetEnumerator() => 
        _files.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}