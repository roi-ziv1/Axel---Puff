using System;
using Camera;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private Vector3 movement;

    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is not null) {
                
                print("camera triggered!!");
                CoreManager.Instance.CameraManager.LerpCameraPosition(movement);
                
                // GetComponent<Collider2D>().enabled = false;
            }
        }
    }


}