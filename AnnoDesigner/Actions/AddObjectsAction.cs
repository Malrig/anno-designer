using System.Collections.Generic;
using AnnoDesigner.Models;
using System;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Action class that adds objects to the canvas.
    ///
    /// This stores off the new Guids of the objects so that they can be removed easily.
    /// </summary>
    public class AddObjectsAction : IAction
    {
        private List<LayoutObject> _objectsToAdd;
        private List<Guid> _addedObjectGuids;

        protected AddObjectsAction(List<LayoutObject> objectsToAdd)
        {
            _objectsToAdd = objectsToAdd;
        }

        public void PerformAction(Dictionary<Guid, LayoutObject> placedObjects)
        {
            _addedObjectGuids = new List<Guid>();
            
            foreach (LayoutObject toAdd in _objectsToAdd)
            {
                Guid objectGuid = Guid.NewGuid();
                placedObjects.Add(objectGuid, toAdd);
                _addedObjectGuids.Add(objectGuid);
            }
        }

        public void UndoAction(Dictionary<Guid, LayoutObject> placedObjects)
        {
            foreach (Guid objectGuid in _addedObjectGuids)
            {
              placedObjects.Remove(objectGuid);
            }
        }
    }
}