using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D rb;
        private SpriteRenderer renderer;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            renderer = GetComponent<SpriteRenderer>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerManager>() is not null)
            {
                CoreManager.Instance.EventsManager.InvokeEvent(EventNames.Die, null);
                CoreManager.Instance.player.Die();
            }

            if (other.gameObject.GetComponent<Bullet>() is not null)
            {
                return;
            }
            
            Destroy(gameObject);
            
        }

        public void Activate(Vector3 bulletDirection, float bulletForce)
        {
            if (bulletDirection == Vector3.right)
            {
                renderer.flipX = true;
            } else if (bulletDirection == Vector3.down)
            {
                transform.Rotate(0,0,-90);
            }
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Shuriken);
            rb.AddForce(bulletDirection * bulletForce);

        }
    }
}