using System;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.WaterTriggers
{
    public class WoodenDoor : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private float magnitude;
        [SerializeField] private float duration;
        [SerializeField] private ParticleSystem p;
        private SpriteRenderer r;
        private Collider2D c;

        private void Start()
        {
            r = GetComponent<SpriteRenderer>();
            c = GetComponent<Collider2D>();
            p.Stop();
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, OnDie);
        }

        private void OnDestroy()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, OnDie);

        }

        private void OnDie(object obj)
        {
            c.enabled = true;
            Color color = r.color;
            color.a = 1; // Set alpha to 0 (fully transparent)
            r.color = color;   
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            print($"Collided with {other.gameObject.name}");
            if (other.gameObject.GetComponent<ObjectGrowth>() is not null)
            {
                print("SHATTER DOOR");
                Shatter();
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            if (player is not null && player.ForceFromAbove != default)
            {
                Shatter();
                player.Rb.AddForce(player.ForceFromAbove*1100);
            }
        }

        private void Shatter()
        {
            c.enabled = false;
            p.Play();
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.WallBreak);
            CoreManager.Instance.CameraManager.ShakeCamera(duration,magnitude);
            Color color = r.color;
            color.a = 0; // Set alpha to 0 (fully transparent)
            r.color = color;    
        }
    }
}