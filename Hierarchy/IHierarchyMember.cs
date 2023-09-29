namespace Hierarchy
{
    /// <summary>
    /// The interface of hierarchy member
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHierarchyMember<T>
        where T : class, IHierarchyMember<T>
    {
        IHierarchyManager<T> Hierarchy { get; }
    }
}
