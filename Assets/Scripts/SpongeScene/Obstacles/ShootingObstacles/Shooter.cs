using System;
using System.Collections;
using System.Collections.Generic;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public class Shooter : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Vector3 bulletDirection;
        [SerializeField] private float bulletForce;
        [SerializeField] private Transform shootingPosition;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private SpriteRenderer renderer;


        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, OnDie);
        }
        
        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, OnDie);
        }

        private void OnDie(object obj)
        {
           StopAllCoroutines();
           renderer.sprite = closedSprite;
        }


        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            if (bulletDirection == Vector3.right)
            {
                renderer.flipX = true;
            }
        }

        public void Shoot()
        {
            Bullet bullet = Instantiate(bulletPrefab, shootingPosition.position, Quaternion.identity).GetComponent<Bullet>();
            StartCoroutine(ChangeSpriteForShortDuration());
            bullet.Activate(bulletDirection, bulletForce);
        }

        private IEnumerator ChangeSpriteForShortDuration()
        {
            renderer.sprite = openSprite;
            yield return new WaitForSeconds(1f);
            renderer.sprite = closedSprite;
        }
    }
}
