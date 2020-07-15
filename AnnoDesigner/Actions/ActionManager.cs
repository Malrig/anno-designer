using System.Collections.Generic;
using AnnoDesigner.Models;
using NLog;

namespace AnnoDesigner.Actions
{
    /// <summary>
    /// Class which manages the actions that have been taken. Allows performing new actions, and undoing and redoing actions.
    /// </summary>
    public class ActionManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<LayoutObject> _placedObjects;
        private Stack<IAction> _previousActions;
        private Stack<IAction> _futureActions;

        public ActionManager(ref List<LayoutObject> placedObjects){
            _placedObjects = placedObjects;
            _previousActions = new Stack<IAction>();
            _futureActions = new Stack<IAction>();
        }

        public void PerformAction(IAction action){
            logger.Debug("Performing action");

            action.PerformAction(_placedObjects);

            if (action is ICombinableAction && _previousActions.Count > 0 &&
                _previousActions.Peek() is ICombinableAction)
            {
                // Both the new action and the last action are combinable, check if they can combine.
                // If they can then pop off the previous action and combine them.
                if (((ICombinableAction)action).CombinableWith((ICombinableAction)_previousActions.Peek()))
                {
                    logger.Debug("Have two compatible combinable actions, combine them.");
                    ((ICombinableAction)action).CombineActions((ICombinableAction)_previousActions.Pop());
                }
            }

            _previousActions.Push(action);

            // Performing a new action so clear all future actions as they are no longer valid.
            _futureActions.Clear();
        }

        public void UndoAction() {
            IAction actionToUndo;
            logger.Debug("Attempt an undo.");

            // User wants to undo something so continue until we reach an action which is not a no-op
            do
            {
                if (_previousActions.Count == 0)
                {
                    logger.Warn("No actions to undo");
                    return;
                }
                actionToUndo = _previousActions.Pop();
                _futureActions.Push(actionToUndo);
            } while (actionToUndo is NoOpAction);

            actionToUndo.UndoAction(_placedObjects);
        }

        public void RedoAction() {
            logger.Debug("Attempt a redo.");
            if (_futureActions.Count == 0)
            {
                logger.Warn("No actions to redo");
                return;
            }
            IAction actionToRedo = _futureActions.Pop();
            _previousActions.Push(actionToRedo);
            actionToRedo.PerformAction(_placedObjects);

            // We never want NoOpActions on the top of the _futureActions stack so pop all of them off
            while (_futureActions.Count > 0 && _futureActions.Peek() is NoOpAction)
            {
                _previousActions.Push(_futureActions.Pop());
            }
        }
    }
}