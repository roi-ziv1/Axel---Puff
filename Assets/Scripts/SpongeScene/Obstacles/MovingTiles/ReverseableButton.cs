using SpongeScene.Managers;
using DoubleTrouble.Interfaces;
using UnityEngine;

namespace SpongeScene.Obstacles.MovingTiles
{
    public class ReverseableButton : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour LinkedObject;
        [SerializeField] private Vector2 directionForLinkedObject;

        [SerializeField] private int hitCount;
        [SerializeField] private int activationCount = 10;

        private IHandleAction linkedObject;
        private float buttonPressedOffset;
        private bool isPressed = false;
        private Animator animator;
        private static readonly int Activate = Animator.StringToHash("Activate");
        private static readonly int Deactivate = Animator.StringToHash("Deactivate");

        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetObject);
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, ResetObject);
        }

        private void ResetObject(object obj)
        {
            hitCount = 0;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (LinkedObject is IHandleAction)
            {
                linkedObject = (IHandleAction)LinkedObject;
            }

            buttonPressedOffset = 0.25f;
        }

        private void OnParticleCollision(GameObject other)
        {
            if (++hitCount == activationCount)
            {
                animator.SetTrigger(Activate);
                CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.ButtonPressed);
            }
        }

        public void TriggerAction()
        {
            linkedObject.Activate(directionForLinkedObject);
            animator.SetTrigger(Deactivate);
        }

        public void TriggerReverseAction()
        {
            linkedObject.Deactivate(directionForLinkedObject*-1);
        }
    }
}