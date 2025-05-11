using UnityEngine;
using UnityEngine.InputSystem;

namespace DoubleTrouble.Managers
{
    public class UserInput2P : MonoBehaviour
    {
        public static UserInput2P instance;
        [HideInInspector] public Controls2P[] controls;  // Array to store controls for each player
        [HideInInspector] public Vector2[] movementInput;
        [HideInInspector] public Vector2[] aimInput;
        [HideInInspector] public bool[] shootInput;
        [HideInInspector] public bool[] jumpInput;
        [HideInInspector] public bool[] mergeInput1; // Left Bumper for Player 1
        [HideInInspector] public bool[] mergeInput2; // Right Bumper for Player 2

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Initialize the Controls2P array for 2 players
            controls = new Controls2P[2];
            controls[0] = new Controls2P();  // Player 1
            controls[1] = new Controls2P();  // Player 2

            // Assign devices for Player 1 and Player 2 (ensure the gamepads are connected)
            if (Gamepad.all.Count > 1)
            {
                controls[0].devices = new InputDevice[1] { Gamepad.all[0] };  // Player 1
                controls[1].devices = new InputDevice[1] { Gamepad.all[1] };  // Player 2
            }
            // else
            // {
            //     // If no gamepads, use the keyboard for both players
            //     controls[0].devices = new InputDevice[1] { Keyboard.current };
            //     controls[1].devices = new InputDevice[1] { Keyboard.current };
            // }
            

            // Initialize input arrays
            movementInput = new Vector2[2];
            aimInput = new Vector2[2];
            shootInput = new bool[2];
            jumpInput = new bool[2];
            mergeInput1 = new bool[2]; // Left bumper
            mergeInput2 = new bool[2]; // Right bumper
        }

        private void OnEnable()
        {
            // Enable controls for both players
            controls[0].Enable();
            controls[1].Enable();

            // Bind the controls for Player 1
            controls[0].Movement.Move.performed += ctx => movementInput[0] = ctx.ReadValue<Vector2>();
            controls[0].Movement.Aim.performed += ctx => aimInput[0] = ctx.ReadValue<Vector2>();
            controls[0].Movement.Shoot.performed += ctx => shootInput[0] = true;
            // Cancel action when button is released
            controls[0].Movement.Shoot.canceled += ctx => jumpInput[0] = false;
            controls[0].Movement.Jump.performed += ctx => jumpInput[0] = true;
            controls[0].Movement.Jump.canceled += ctx => jumpInput[0] = false; 

            // Merge Left (Player 1)
            controls[0].Movement.MergeLeft.performed += ctx => mergeInput1[0] = true;
            controls[0].Movement.MergeLeft.canceled += ctx => mergeInput1[0] = false; // Cancel action when button is released

            // Merge Right (Player 1)
            controls[0].Movement.MergeRight.performed += ctx => mergeInput2[0] = true;
            controls[0].Movement.MergeRight.canceled += ctx => mergeInput2[0] = false; // Cancel action when button is released

            // Bind the controls for Player 2
            controls[1].Movement.Move.performed += ctx => movementInput[1] = ctx.ReadValue<Vector2>();
            controls[1].Movement.Aim.performed += ctx => aimInput[1] = ctx.ReadValue<Vector2>();
            controls[1].Movement.Shoot.performed += ctx => shootInput[1] = true;
            controls[1].Movement.Shoot.canceled += ctx => jumpInput[1] = false;
            controls[1].Movement.Jump.performed += ctx => jumpInput[1] = true;
            controls[1].Movement.Jump.canceled += ctx => jumpInput[1] = false;

            // Merge Left (Player 2)
            controls[1].Movement.MergeLeft.performed += ctx => mergeInput1[1] = true;
            controls[1].Movement.MergeLeft.canceled += ctx => mergeInput1[1] = false; // Cancel action when button is released

            // Merge Right (Player 2)
            controls[1].Movement.MergeRight.performed += ctx => mergeInput2[1] = true;
            controls[1].Movement.MergeRight.canceled += ctx => mergeInput2[1] = false; // Cancel action when button is released
        }

        private void OnDisable()
        {
            // Disable controls for both players
            controls[0].Disable();
            controls[1].Disable();
        }
    }
}
