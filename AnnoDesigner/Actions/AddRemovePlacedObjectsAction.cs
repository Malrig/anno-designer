using System.Collections.Generic;
using AnnoDesigner.Models;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Abstract base class that implements adding or removing objects on the canvas.
    /// </summary>
    public abstract class AddRemovePlacedObjectsAction : IAction
    {
        private List<LayoutObject> _objectsToPlace;
        private AddRemove _addOrRemove;

        protected enum AddRemove {
            Add,
            Remove
        }

        protected AddRemovePlacedObjectsAction(List<LayoutObject> objectsToPlace, AddRemove addOrRemove)
        {
            _objectsToPlace = objectsToPlace;
            _addOrRemove = addOrRemove;
        }

        public void PerformAction(List<LayoutObject> placedObjects)
        {
            switch (_addOrRemove)
            {
                case AddRemove.Add:
                    addObject(placedObjects);
                    break;
                case AddRemove.Remove:
                    removeObject(placedObjects);
                    break;
            }
        }

        public void UndoAction(List<LayoutObject> placedObjects)
        {
            switch (_addOrRemove) 
            {
                case AddRemove.Add:
                    removeObject(placedObjects);
                    break;
                case AddRemove.Remove:
                    addObject(placedObjects);
                    break;
            }

        }

        private void addObject(List<LayoutObject> placedObjects)
        {
            placedObjects.AddRange(_objectsToPlace);
            // sort the objects because borderless objects should be drawn first
            placedObjects.Sort((a, b) => b.WrappedAnnoObject.Borderless.CompareTo(a.WrappedAnnoObject.Borderless));
        }

        private void removeObject(List<LayoutObject> placedObjects)
        {
            // placedObjects = placedObjects.Except(_objectsToPlace).ToList();
            _objectsToPlace.ForEach(_ => placedObjects.Remove(_));
        }
    }

    /// <summary>
    /// Places objects on the canvas.
    /// </summary>
    public class AddPlacedObjectsAction : AddRemovePlacedObjectsAction
    {
        public AddPlacedObjectsAction(List<LayoutObject> objectsToPlace) : base(objectsToPlace, AddRemove.Add) { }
    }

    /// <summary>
    /// Removes objects from the canvas.
    /// </summary>
    public class RemovePlacedObjectsAction : AddRemovePlacedObjectsAction
    {
        public RemovePlacedObjectsAction(List<LayoutObject> objectsToPlace) : base(objectsToPlace, AddRemove.Remove) { }
    }

}