namespace Hierarchy
{
    /// <summary>
    /// The interface of hierarchy member
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHierarchyMember<T>
        where T : class, IHierarchyMember<T>
    {
        /// <summary>
        /// Name of member in hierarchy
        /// </summary>
        string HierarchyName { get; set; }

        //TODO: getter below should be ReadOnly
        IHierarchyManager<T> Hierarchy { get; }
    }
}
