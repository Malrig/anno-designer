namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Interface for all actions that are combinable. 
    /// The reason for this concept is that some actions that logically are one "user" action happen through many individual
    /// actions. For example, moving all objects five steps to the right means that the objects move to the right one step five
    /// times, it makes more sense to combine these into the single logic user action of one move.
    /// </summary>
    public interface ICombinableAction : IAction
    {
        /// <summary>
        /// Combine the target action into this one.
        /// </summary>
        public void CombineActions(ICombinableAction toCombine);

        /// <summary>
        /// Returns <see langword="true"> if the other action can be combined into this one, otherwise return <see langword="false">.
        /// </summary>
        public bool CombinableWith(ICombinableAction other);
    }
}