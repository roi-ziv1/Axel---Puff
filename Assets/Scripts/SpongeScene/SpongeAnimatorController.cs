using Character;
using UnityEngine;

namespace SpongeScene
{
    public class SpongeAnimatorController : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody2D rb;
        private PlayerManager player;

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerManager>();
        }

        void Update()
        {
            HandleMovementAnimation();
            HandleJumpAnimation();
        }

        private void HandleMovementAnimation()
        {
            float speed = Mathf.Abs(rb.linearVelocity.x);

            if (speed > 0f)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        private void HandleJumpAnimation()
        {
            if (UserInput.instance.controls.Movement.Jump.WasPressedThisFrame() && player.IsOnSurface())
            {
                animator.SetTrigger("Jumping");
            }
        }
    }
}
