using System;
using DoubleTrouble.Interfaces;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Turbines
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour LinkedObject;
        [SerializeField] private Vector2 directionForLinkedObject;
        [SerializeField] private Sprite pressedSprite;
        [SerializeField] private Sprite notPressedSprite;
        [SerializeField] private SpriteRenderer _renderer;

        [SerializeField] private int hitCount;
        [SerializeField] private int activationCount = 8;

        private IHandleAction linkedObject;
        private float buttonPressedOffset;
        private bool isPressed = false;
        private Animator animator;
        private static readonly int Activate = Animator.StringToHash("Activate");

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
            _renderer.sprite = notPressedSprite;
            // Reset the animator to the default state
            animator.Rebind();
            animator.Update(0f);
        }

        private void Start()
        {
            if (LinkedObject is IHandleAction)
            {
                linkedObject = (IHandleAction)LinkedObject;
            }

            animator = GetComponent<Animator>();
            buttonPressedOffset = 0.25f;
        }

        private void OnParticleCollision(GameObject other)
        {
            if (++hitCount ==activationCount) {
                
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.ButtonPressed);
            animator.SetTrigger(Activate);
            //after the animation the sprite should be pressed 
            
            }
        }
        
        public void ActivateButton() {
            print("ACTIVATE67");
            linkedObject.Activate(directionForLinkedObject);
            _renderer.sprite = pressedSprite;

        }
    }
}