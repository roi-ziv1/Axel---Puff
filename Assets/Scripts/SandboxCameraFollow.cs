// using Camera;
// using SpongeScene.Managers;
// using UnityEngine;
//
// namespace DefaultNamespace
// {
//     public class SandboxCameraFollow : MonoBehaviour
//     {
//
//         public GameObject Player;
//
//         public GameObject MainCamera;
//
//         // values between 1 - 3 make camera sluggish, 10 seems instant
//         public float SmoothSpeed = 3;
//
//         public static float PPUScale;
//         public static float ScreenPPU;
//
//         public int NativePPU;
//
//
//         // ---------------------------------------- private ----------------------------------------
//         private Camera _mainCamera;
//
//         private float _zStartPositionCamera;
//         [SerializeField] private float _xOffsetCameraToPlayer;
//         [SerializeField] private float _yOffsetCameraToPlayer;
//         [SerializeField] private float _zOffsetCameraToPlayer;
//         private float _orthographicCameraSize;
//
//         private int _screenResolutionWidth;
//         private int _nativeResolutionWidth;
//
//
//         // Use this for initialization
//         void Start()
//         {
//             _mainCamera = MainCamera.GetComponent<Camera>();
//             _zStartPositionCamera = MainCamera.transform.position.z;
//             _screenResolutionWidth = 0;
//         }
//
//
//         // Update is called once per frame
//         void Update()
//         {
//             // if screen resolution changes or orthographic size of the camera --> adapt values to maintain pixel perfection
//             if (_screenResolutionWidth != Screen.currentResolution.width ||
//                 !Mathf.Approximately(_orthographicCameraSize, _mainCamera.orthographicSize))
//             {
//                 // update values for pixel perfection
//                 UpdatePixelPerfectScaleValues();
//             }
//         }
//
//
//         void LateUpdate()
//         {
//             // only move camera if distance between player position and camera is bigger then 10* 1/ScreenPPU
//             // reason: single latecomer stutters when player is standing still (single adjustings of the script)
//             if (Mathf.Abs(Player.transform.position.x - GrafikAndGuiSettings.PPV(transform.position.x)) >
//                 10 * 1 / GrafikAndGuiSettings.ScreenPPU)
//             {
//                 SmoothPixelPerfectCamera();
//             }
//         }
//
//
//         //Smooth pixel perfect camera
//         private void SmoothPixelPerfectCamera()
//         {
//             Vector2 desiredPosition = new Vector2(Player.transform.position.x + _xOffsetCameraToPlayer,
//                 Player.transform.position.y + _yOffsetCameraToPlayer);
//             Vector2 pixelPerfectDesiredPosition = new Vector2(PPV(desiredPosition.x), PPV(desiredPosition.y));
//             Vector2 smoothPosition = Vector2.Lerp(transform.position, pixelPerfectDesiredPosition,
//                 Time.deltaTime * SmoothSpeed);
//             Vector2 smoothPixelPerfectPosition = new Vector2(PPV(smoothPosition.x), PPV(smoothPosition.y));
//
//             // add z-start-position of camera
//             MainCamera.transform.position = (Vector3)smoothPixelPerfectPosition +
//                                             Vector3.forward * (PPV(_zStartPositionCamera) * _zOffsetCameraToPlayer);
//         }
//
//
//         // PPV = Pixel Perfect Value
//         public static float PPV(float valueWithoutPixelPerfection)
//         {
//             // divide value without pixel perfection by the inversed pixel per unit value (unit per pixel)
//             // _screenPPU = 1/PPU
//             float screenPixelPosition = valueWithoutPixelPerfection * ScreenPPU;
//
//             // resDivisionInt = integer amount number of _screenPPU for closestWholePixelValue
//             float pixelPerfectScreenUnitPosition = Mathf.Round(screenPixelPosition) / ScreenPPU;
//
//             return pixelPerfectScreenUnitPosition;
//         }
//
//
//         private void UpdatePixelPerfectScaleValues()
//         {
//             float aspectRatio = (float)16 / 9;
//             // calculate native resolution width, float auxiliary variable for a full float calculation
//             float auxiliaryVar = aspectRatio * _mainCamera.orthographicSize * NativePPU * 2f;
//             _nativeResolutionWidth = (int)auxiliaryVar;
//
//             // calculate PPUScale
//             PPUScale = (float)Screen.currentResolution.width / _nativeResolutionWidth;
//             // translate Native Pixel Per Unit (PPU) to actually ScreenPPU
//             ScreenPPU = PPUScale * NativePPU;
//
//             // save new resolution
//             _orthographicCameraSize = _mainCamera.orthographicSize;
//             _screenResolutionWidth = Screen.currentResolution.width;
//         }
//     }
//
//     public static class GrafikAndGuiSettings1
//     {
//         public static float ScreenPPU = 100f; // Example pixels per unit
//
//         public static float PPV(float value)
//         {
//             return value * ScreenPPU; // Convert to pixel-perfect value
//         }
//     }
// }
