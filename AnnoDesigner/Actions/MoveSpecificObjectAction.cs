// using System.Collections.Generic;
// using AnnoDesigner.Models;
// using NLog;
// using System.Windows;

// namespace AnnoDesigner.Actions
// {
//     /// <summary>
//     /// Moves specific objects on the canvas.
//     /// </summary>
//     public class MoveSpecificObjectsAction : ICombinableAction
//     {
//         private static readonly Logger logger = LogManager.GetCurrentClassLogger();

//         private List<LayoutObject> _objectsToMove;
//         private double _delta_x;
//         private double _delta_y;

//         public MoveSpecificObjectsAction(List<LayoutObject> objectsToMove, double delta_x, double delta_y)
//         {
//             _objectsToMove = objectsToMove;
//             _delta_x = delta_x;
//             _delta_y = delta_y; 
//         }

//         public void PerformAction(List<LayoutObject> placedObjects)
//         {
//             logger.Debug("Perform move all action.");
//             foreach (var curLayoutObject in placedObjects)
//             {
//                 curLayoutObject.Position = new Point(curLayoutObject.Position.X + _delta_x, curLayoutObject.Position.Y + _delta_y);
//             }
//         }

//         public void UndoAction(List<LayoutObject> placedObjects)
//         {
//             logger.Debug("Undo move all action.");
//             foreach (var curLayoutObject in placedObjects)
//             {
//                 curLayoutObject.Position = new Point(curLayoutObject.Position.X - _delta_x, curLayoutObject.Position.Y - _delta_y);
//             }
//         }

//         public bool CombinableWith(ICombinableAction other)
//         {
//             if (other is MoveSpecificObjectsAction && ((MoveSpecificObjectsAction)other)._objectsToMove == _objectsToMove)
//             {
//                 return true;
//             }

//             return false;
//         }

//         public void CombineActions(ICombinableAction other)
//         {
//             logger.Debug("Combine two move all actions.");
//             if (!CombinableWith(other)) 
//             {
//                 // If this has happened then we are going to end up with some screwed up action history
//                 throw new System.ArgumentException("Cannot combine target object with a MoveSpecificObjects action.", "other");
//             }

//             MoveSpecificObjectsAction toCombine = (MoveSpecificObjectsAction)other;

//             _delta_x += toCombine._delta_x;
//             _delta_y += toCombine._delta_y;
//         }
//     }
// }