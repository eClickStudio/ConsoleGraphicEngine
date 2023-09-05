namespace ConsoleGraphicEngine.Engine.Basic.Tools.Hierarchy
{
    /// <summary>
    /// The interface of hierarchy member
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IHierarchyMember<T>
        where T : class, IHierarchyMember<T>
    {
        IHierarchyManager<T> hierarchy { get; }
    }
}
