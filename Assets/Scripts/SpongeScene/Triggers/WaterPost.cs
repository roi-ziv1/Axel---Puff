using System.Collections;
using System.Collections.Generic;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Triggers
{
   
 
       public class WaterPost : MonoBehaviour
       {
           private bool reachedThisCheckpoint = false;
           [SerializeField] public ParticleSystem p;
           [SerializeField] private Animator animator;
   
           private void OnEnable()
           {
               CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEnd);
               if (p != null)
               {
                   p.Stop();
               }

               animator = GetComponent<Animator>();
           }
           
           private void OnDisable()
           {
               CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, OnEnd);
           }
   
           private void OnEnd(object obj)
           {
               reachedThisCheckpoint = false;
               animator.Rebind();
               animator.Update(0f);
               p.Stop();
               
           }

           public void PlayParticles()
           {
               // p.Play();
           }

           public void StopParticles()
           {
               // p.Stop();
           }
   
           private void OnTriggerEnter2D(Collider2D other)
           {
               if (other.GetComponent<PlayerManager>() is not null)
               {
                   if (!reachedThisCheckpoint)
                   {
                       if (p is not null)
                       {
                           GetComponent<Animator>().SetTrigger("Activate");
                           p.Play();
                           // StartCoroutine(PlayAfterDelay());
                           CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.CheckPoint);

                       }
   
                   }
   
                   reachedThisCheckpoint = true;
               }
           }
       }
   

}