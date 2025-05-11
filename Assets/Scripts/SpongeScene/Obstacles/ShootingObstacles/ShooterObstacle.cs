using System.Collections;
using System.Collections.Generic;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public abstract class ShooterObstacle : MonoBehaviour
    {
        [SerializeField] protected List<Shooter> shooters;

        private Collider2D _collider;

        public void Start()
        {
            _collider = GetComponent<Collider2D>();
            print($"collider of {name} is {_collider}");
        }


        public virtual void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetObject);
        }

        public virtual void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, ResetObject);
        }
        
        public abstract IEnumerator Shoot();

        public void ResetObject(object obj)
        {
            StopAllCoroutines();
            _collider.enabled = true;
        }
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is not null)
            {   
                StopAllCoroutines();
                StartCoroutine(Shoot());
                GetComponent<Collider2D>().enabled = false;
            }

        }
        


        
    }
}