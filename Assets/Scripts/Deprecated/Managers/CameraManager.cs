// using DoubleTrouble.Camera;
//
// namespace DoubleTrouble.Managers
// {
//     using UnityEngine;
//     using System.Collections;
//
//     public class CameraManager : MonoBehaviour
//     {
//         public static CameraManager Instance;
//
//         public Camera MainCamera => mainCamera;
//         public CameraFollower CameraFollower => cameraFollower;
//         [SerializeField] private Camera mainCamera;
//         [SerializeField] private float shakeDuration = 0.5f;
//         [SerializeField] private float shakeMagnitude = 0.2f;
//         [SerializeField] private float dampingSpeed = 1.0f;
//
//         private Vector3 originalPos;
//         private CameraFollower cameraFollower;
//
//         private void Awake()
//         {
//             // Singleton pattern to make sure there's only one instance of the CameraManager
//             if (Instance == null)
//             {
//                 Instance = this;
//             }
//             else
//             {
//                 Destroy(gameObject);
//             }
//
//             // Ensure mainCamera reference is set, if not manually set it in the inspector
//             if (mainCamera == null)
//             {
//                 mainCamera = Camera.main;
//             }
//
//             cameraFollower = GetComponent<CameraFollower>();
//
//         }
//
//         private void Start()
//         {
//             originalPos = mainCamera.transform.localPosition;
//         }
//
//         // Camera Shake Function
//         public void ShakeCamera(float duration = -1f, float magnitude = -1f)
//         {
//             if (duration < 0) duration = shakeDuration;
//             if (magnitude < 0) magnitude = shakeMagnitude;
//
//             StartCoroutine(ShakeCoroutine(duration, magnitude));
//         }
//
//         private IEnumerator ShakeCoroutine(float duration, float magnitude)
//         {
//             float elapsed = 0.0f;
//
//             while (elapsed < duration)
//             {
//                 float x = Random.Range(-1f, 1f) * magnitude;
//                 float y = Random.Range(-1f, 1f) * magnitude;
//
//                 mainCamera.transform.localPosition = new Vector3(x, y, originalPos.z);
//
//                 elapsed += Time.deltaTime;
//
//                 yield return null;
//             }
//
//             // After shaking, reset the camera to its original position
//             mainCamera.transform.localPosition = originalPos;
//         }
//
//         // Focus on a target by moving the camera to its position
//         public void FocusOnTarget(Transform target, float duration = 1f)
//         {
//             StartCoroutine(FocusOnTargetCoroutine(target, duration));
//         }
//
//         private IEnumerator FocusOnTargetCoroutine(Transform target, float duration)
//         {
//             Vector3 targetPosition = target.position;
//             Vector3 startPosition = mainCamera.transform.position;
//             float elapsedTime = 0f;
//
//             while (elapsedTime < duration)
//             {
//                 mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
//                 elapsedTime += Time.deltaTime;
//                 yield return null;
//             }
//
//             mainCamera.transform.position = targetPosition;
//         }
//
//         // Zoom in or out over a given duration
//         public void Zoom(float targetFieldOfView, float duration = 1f)
//         {
//             StartCoroutine(ZoomCoroutine(targetFieldOfView, duration));
//         }
//
//         private IEnumerator ZoomCoroutine(float targetFieldOfView, float duration)
//         {
//             float startFOV = mainCamera.fieldOfView;
//             float elapsedTime = 0f;
//
//             while (elapsedTime < duration)
//             {
//                 mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFieldOfView, elapsedTime / duration);
//                 elapsedTime += Time.deltaTime;
//                 yield return null;
//             }
//
//             mainCamera.fieldOfView = targetFieldOfView;
//         }
//
//         public Vector2 ReturnLeftMostPosition()
//         {
//             return mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
//         }
//
//         public Vector2 ReturnRightMostPosition()
//         {
//             return mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));
//         }
//
//
//         // Function to get a position above the current sight of the camera, outside its view
//         public Vector3 GetPositionAboveCamera()
//         {
//             if (mainCamera == null) return Vector3.zero;
//
//             // Get the camera's current position in 2D (ignoring the Z axis)
//             Vector3 cameraPosition = mainCamera.transform.position;
//
//             // Get the camera's orthographic size (for 2D cameras)
//             float cameraHeight = mainCamera.orthographicSize;
//
//             // Get the camera's aspect ratio (for the width of the camera's view)
//             float cameraWidth = cameraHeight * mainCamera.aspect;
//
//             // Calculate a position above the camera's current view, offset by 2 units higher
//             Vector3 positionAbove =
//                 new Vector3(cameraPosition.x, cameraPosition.y + cameraHeight + 2, cameraPosition.z);
//
//             return positionAbove;
//         }
//     }
// }