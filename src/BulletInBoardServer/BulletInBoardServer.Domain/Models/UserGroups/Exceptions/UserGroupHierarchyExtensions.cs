namespace BulletInBoardServer.Domain.Models.UserGroups.Exceptions;

public static class UserGroupHierarchyExtensions
{
    public static IEnumerable<UserGroup> FlattenUserGroupHierarchy(this UserGroup root) => 
        root.GetDescendants().DistinctBy(ug => ug.Id) /*.SelectMany(node => node)*/;

    private static IEnumerable<UserGroup> GetDescendants(this UserGroup? userGroup)
    {
        if (userGroup == null) 
            yield break;
        
        yield return userGroup;
        
        foreach (var child in userGroup.ChildrenGroups)
            foreach (var descendant in child.GetDescendants())
                yield return descendant;
    }
}