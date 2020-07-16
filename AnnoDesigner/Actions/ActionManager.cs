using System.Collections.Generic;
using AnnoDesigner.Models;
using System;
using NLog;
using System.Linq;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Class which manages the actions that have been taken. Allows performing new actions, and undoing and redoing actions.
    /// </summary>
    public class ActionManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<Guid, LayoutObject> _placedObjects;
        public List<LayoutObject> placedObjects {
            get
            {
                var objects = _placedObjects.Values.ToList();
                // sort the objects because borderless objects should be drawn first
                objects.Sort((a, b) => b.WrappedAnnoObject.Borderless.CompareTo(a.WrappedAnnoObject.Borderless));
                return objects;
            } 
        }

        private Stack<IAction> previousActions;
        private Stack<IAction> futureActions;

        public ActionManager() {
            previousActions = new Stack<IAction>();
            futureActions = new Stack<IAction>();
            _placedObjects = new Dictionary<Guid, LayoutObject>();
        }

        public ActionManager(List<LayoutObject> newPlacedObjects) : base() {
            previousActions = new Stack<IAction>();
            futureActions = new Stack<IAction>();
            _placedObjects = newPlacedObjects.ToDictionary(_ => Guid.NewGuid(), x => x);
        }

        public void PerformAction(IAction action){
            logger.Debug("Performing action");

            action.PerformAction(_placedObjects);

            if (action is ICombinableAction && previousActions.Count > 0 &&
                previousActions.Peek() is ICombinableAction)
            {
                // Both the new action and the last action are combinable, check if they can combine.
                // If they can then pop off the previous action and combine them.
                if (((ICombinableAction)action).CombinableWith((ICombinableAction)previousActions.Peek()))
                {
                    logger.Debug("Have two compatible combinable actions, combine them.");
                    ((ICombinableAction)action).CombineActions((ICombinableAction)previousActions.Pop());
                }
            }

            previousActions.Push(action);

            // Performing a new action so clear all future actions as they are no longer valid.
            futureActions.Clear();
        }

        public void UndoAction() {
            IAction actionToUndo;
            logger.Debug("Attempt an undo.");

            // User wants to undo something so continue until we reach an action which is not a no-op
            do
            {
                if (previousActions.Count == 0)
                {
                    logger.Warn("No actions to undo");
                    return;
                }
                actionToUndo = previousActions.Pop();
                futureActions.Push(actionToUndo);
            } while (actionToUndo is NoOpAction);

            actionToUndo.UndoAction(_placedObjects);
        }

        public void RedoAction() {
            logger.Debug("Attempt a redo.");
            if (futureActions.Count == 0)
            {
                logger.Warn("No actions to redo");
                return;
            }
            IAction actionToRedo = futureActions.Pop();
            previousActions.Push(actionToRedo);
            actionToRedo.PerformAction(_placedObjects);

            // We never want NoOpActions on the top of the _futureActions stack so pop all of them off
            while (futureActions.Count > 0 && futureActions.Peek() is NoOpAction)
            {
                previousActions.Push(futureActions.Pop());
            }
        }
    }
}