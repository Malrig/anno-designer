using System.Collections.Generic;
using AnnoDesigner.Models;
using System;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Interface for all actions that can affect the objects placed on the canvas.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Change the placedObjects according to this action.
        /// </summary>
        public void PerformAction(Dictionary<Guid, LayoutObject> placedObjects);

        /// <summary>
        /// Undo the change to the placedObjects according to this action.
        /// </summary>
        public void UndoAction(Dictionary<Guid, LayoutObject> placedObjects);
    }
}