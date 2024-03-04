using System.Collections;

namespace BulletInBoardServer.Domain.Models.UserGroups;

/// <summary>
/// Права всех участников группы пользователей
/// </summary>
public class GroupMemberRights : ICollection<SingleMemberRights>
{
    // private readonly Dictionary<User, SingleMemberRights> _rightsMap = new();
    private readonly Dictionary<Guid, SingleMemberRights> _rightsMap = new();



    public int Count => _rightsMap.Count;
    public bool IsReadOnly => false;



    public GroupMemberRights(IEnumerable<SingleMemberRights> rights)
    {
        foreach (var singleMemberRights in rights)
            _rightsMap[singleMemberRights.UserId] = singleMemberRights;
    }

    public GroupMemberRights()
    {
    }



    public IEnumerator<SingleMemberRights> GetEnumerator() =>
        _rightsMap.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public void Add(SingleMemberRights item) =>
        _rightsMap[item.UserId] = item;

    public void Clear() =>
        _rightsMap.Clear();

    public bool Contains(SingleMemberRights item) =>
        _rightsMap.ContainsKey(item.UserId);

    public void CopyTo(SingleMemberRights[] array, int arrayIndex) =>
        _rightsMap.Values.CopyTo(array, arrayIndex);

    public bool Remove(SingleMemberRights item) =>
        _rightsMap.Remove(item.UserId);
}