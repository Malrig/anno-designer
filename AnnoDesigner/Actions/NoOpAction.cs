using System.Collections.Generic;
using AnnoDesigner.Models;
using NLog;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Doesn't do anything, it is used to split up actions so they aren't combined together into a single action.
    ///
    /// For example multiple consecutive MoveAllObjects actions will be combined into a single one (to save memory and
    /// so the user doesn't need to undo them all individually). However, we do want some move actions to remain separated
    /// these should be separated by a NoOpAction.
    /// </summary>
    public class NoOpAction : ICombinableAction 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NoOpAction() {}


        public void PerformAction(List<LayoutObject> placedObjects)
        {
            logger.Debug("Performing no-op action.");
        }

        public void UndoAction(List<LayoutObject> placedObjects)
        {
            logger.Debug("Undoing no-op action");
        }

        public bool CombinableWith(ICombinableAction other)
        {
            if (other is NoOpAction)
            {
                return true;
            }

            return false;
        }

        public void CombineActions(ICombinableAction other)
        {
            // We shouldn't really be in a situation where we combine two no-op actions, however, in principle
            // we should do so if we attempt to perform two in a row.
            logger.Debug("Combine two no-op actions.");
            if (!CombinableWith(other)) 
            {
                // If this has happened then we are going to end up with some screwed up action history
                throw new System.ArgumentException("Cannot combine these ICombinableActions", "other");
            }
        }
    }
}