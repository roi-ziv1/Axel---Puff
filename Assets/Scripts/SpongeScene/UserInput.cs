using UnityEngine;

namespace SpongeScene
{
    public class UserInput : MonoBehaviour
    {
        public static UserInput instance;
        [HideInInspector]public Controls controls;
        [HideInInspector] public Vector2 movementInput;
        [HideInInspector] public bool absorbInput;
    
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            // else
            // {
            //     Destroy(gameObject);
            // }

            controls = new Controls();
            controls.Movement.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            controls.Movement.Absorb.performed += ctx => absorbInput = true;
            controls.Movement.Absorb.canceled += ctx => absorbInput = false;
            
        }
    
        private void OnEnable()
        {
            controls.Enable();
        }
    
        private void OnDisable()
        {
            controls.Disable();
        }
    }
}
