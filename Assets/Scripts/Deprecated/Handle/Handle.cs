// using DoubleTrouble.Interfaces;
// using DoubleTrouble.Managers;
// using UnityEngine;
//
// namespace DoubleTrouble.Handle
// {
//     public class Handle : MonoBehaviour
//     {
//         [SerializeField] private Transform handleTransform; // Handle visual representation
//         [SerializeField] private Vector2 activationDirection = Vector2.right; // Direction for activation
//         [SerializeField] private MonoBehaviour action; // Action to be activated (must implement IHandleAction)
//
//         private IHandleAction handleAction;
//
//         private void Awake()
//         {
//             if (action is IHandleAction)
//             {
//                 handleAction = action as IHandleAction;
//                 print("FOUND ACTION HANDLE");
//             }
//             else
//             {
//                 Debug.LogError("Assigned action does not implement IHandleAction interface!");
//             }
//         }
//
//         private void OnTriggerEnter2D(Collider2D other)
//         {
//             // Check if the collider belongs to the player
//             if (other.GetComponent<Player>())
//             {
//                 // Check the direction of interaction
//                 Vector2 interactionDirection = (other.transform.position - transform.position).normalized;
//
//                 // Adjust threshold as needed
//                 {
//                     // Switch direction
//                     handleAction?.Activate(activationDirection);
//
//                     SwitchDirection();
//
//                     // Trigger the action
//                 }
//             }
//         }
//
//         private void SwitchDirection()
//         {
//             // Get the current rotation
//             Vector3 currentRotation = handleTransform.eulerAngles;
//
//             // Calculate the new rotation (negating the current z-axis rotation)
//             float newZRotation = -currentRotation.z;
//
//             // Apply the new rotation
//             handleTransform.eulerAngles = new Vector3(
//                 currentRotation.x,
//                 currentRotation.y,
//                 newZRotation
//             );
//
//             // Reverse the activation direction
//             activationDirection = -activationDirection;
//         }
//     }
// }