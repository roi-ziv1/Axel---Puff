using System;
using System.Collections;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.WaterSource
{
    public class WaterToken : MonoBehaviour
    {
        [SerializeField] private ParticleSystem p;
        [SerializeField] private SpriteRenderer r1;
        [SerializeField] private SpriteRenderer r2;
        private Collider2D col;
        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetObject);
            p.Stop();
            col = GetComponent<Collider2D>();
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, ResetObject);

        }

        public void Activate()
        {
            print("activate token 1111");
            p.Play();
            StartCoroutine(Respawn());
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Token);
            col.enabled = false;
            r1.enabled = false;
            r2.enabled = false;
        }


        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(5f);
            col.enabled = true;
            r1.enabled = true;
            r2.enabled = true;
          
        }

        private void ResetObject(object obj)
        {
            StopAllCoroutines();
            col.enabled = true;
            r1.enabled = true;
            r2.enabled = true;
           }

            // gameObject.SetActive(true);
        }
    }
