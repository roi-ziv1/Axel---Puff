// using SpongeScene;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// namespace Deprecated.DeprecatedSponge
// {
//     public class CursorController : MonoBehaviour
//     {
//         private Camera mainCamera;
//         private IInteractable pickedObject;
//         private Mouse mouse;
//
//         void Start()
//         {
//             mainCamera = Camera.main;
//             mouse = Mouse.current;
//             print(mainCamera.gameObject.name);
//         }
//
//         void Update()
//         {
//             HandleInput();
//             FollowCursor();
//         }
//
//         void HandleInput()
//         {
//             if (pickedObject is null && Input.GetMouseButtonDown(0)) // Left click
//             {
//                 print("11TRY TO PICK OBJECT");
//                 TryPickObject();
//             }
//             else if (Input.GetMouseButtonDown(1)) // Right click
//             {
//                 DropObject();
//             }
//         }
//
//         void TryPickObject()
//         {
//             if (mouse.leftButton.wasPressedThisFrame)
//             {
//                 // Get the mouse position in screen space
//                 Vector3 mousePosition = mouse.position.ReadValue();
//                 // Convert screen space position to world space (2D)
//                 Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
//                 print($"mouse pos : {worldMousePosition}");
//
//                 // Perform a 2D raycast at the mouse position
//                 RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);
//
//                 if (hit.collider != null)
//                 {
//                     pickedObject = hit.collider.GetComponent<IInteractable>();
//                     if (pickedObject != null)
//                     {
//                         pickedObject.OnInteract();
//                     }
//                     else
//                     {
//                         Debug.LogWarning("No IInteractable component found on the hit object.");
//                     }
//                 }
//                 else
//                 {
//                     Debug.Log("No 2D object hit.");
//                 }
//             }
//         }
//
//
//         void DropObject()
//         {
//             pickedObject?.OnDrop();
//             pickedObject = null;
//         }
//
//         void FollowCursor()
//         {
//             if (pickedObject != null)
//             {
//                 Vector3 mousePosition = Input.mousePosition;
//                 mousePosition.z = mainCamera.WorldToScreenPoint(pickedObject.objectTransform.position).z;
//                 pickedObject.objectTransform.position = mainCamera.ScreenToWorldPoint(mousePosition);
//             }
//         }
//     }
// }