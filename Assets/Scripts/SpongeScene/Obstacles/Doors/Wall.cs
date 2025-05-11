
    using System;
    using Character;
    using SpongeScene.Managers;
    using UnityEngine;

    namespace SpongeScene.WaterTriggers
    {
        public class Wall : MonoBehaviour
        {
            // Start is called once before the first execution of Update after the MonoBehaviour is created
            [SerializeField] private float magnitude;
            [SerializeField] private float duration;
            [SerializeField] private ParticleSystem p;
            private SpriteRenderer r;
            private Collider2D c;
            private void Start()
            {
                p.Stop();
                r = GetComponent<SpriteRenderer>();
                c = GetComponent<Collider2D>();
                CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetObject);
            }

            private void OnDestroy()
            {
                CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die,ResetObject);
            }

            private void ResetObject(object obj)
            {
                c.enabled = true;
                Color color = r.color;
                color.a = 1; // Set alpha to 0 (fully transparent)
                r.color = color;    
            }

            private void OnTriggerEnter2D(Collider2D other)
            {
                if (other.gameObject.GetComponent<PlayerManager>() is not null)
                {
                    Shatter();
                }
            }
            private void Shatter()
            {
                p.Play();
                c.enabled = false;
                CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.WallBreak);
                CoreManager.Instance.CameraManager.ShakeCamera(duration,magnitude);

                Color color = r.color;
                color.a = 0; // Set alpha to 0 (fully transparent)
                r.color = color;        
            

            }
        }
    }
