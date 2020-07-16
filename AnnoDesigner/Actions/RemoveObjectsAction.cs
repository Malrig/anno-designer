using System.Collections.Generic;
using AnnoDesigner.Models;
using System;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Action class that removes objects from the canvas.
    ///
    /// This stores off the removed objects so that they can be replaced easily.
    /// </summary>
    public class RemoveObjectsAction : IAction
    {
        private List<Guid> _objectsToRemove;
        private Dictionary<Guid, LayoutObject> _removedObjects;

        protected RemoveObjectsAction(List<Guid> objectsToRemove)
        {
            _objectsToRemove = objectsToRemove;
        }

        public void PerformAction(Dictionary<Guid, LayoutObject> placedObjects)
        {
            _removedObjects = new Dictionary<Guid, LayoutObject>();

            foreach (Guid objectGuid in _objectsToRemove)
            {
                _removedObjects.Add(objectGuid, placedObjects[objectGuid]);
                placedObjects.Remove(objectGuid);
            }
        }

        public void UndoAction(Dictionary<Guid, LayoutObject> placedObjects)
        {
            foreach(KeyValuePair<Guid, LayoutObject> entry in _removedObjects)
            {
                placedObjects.Add(entry.Key, entry.Value);
            }
        }
    }
}