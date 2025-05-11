using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SpongeScene.Cutscene
{
    public class CutsceneEndMC : CutsceneMC
    {
        [SerializeField] private GameObject playerFish;
        [SerializeField] private GameObject fish;
        [SerializeField] private GameObject midPoint2;
        [SerializeField] private GameObject[] pathObjects;
        [SerializeField] private Animator fishAnimator;
        [SerializeField] private FollowFish followFish;
        
        [SerializeField] private ParticleSystem waterParticles;

        [SerializeField] private GameObject[] fishPathObjects1;
        [SerializeField] private GameObject[] fishPathObjects2;
        [SerializeField] private GameObject[] fishPathObjects3;
        [SerializeField] private GameObject[] fishPathObjects4;
        [SerializeField] private GameObject[] fishPathObjects5;

        public Vector3[] path;
        private Vector3[][] fishPaths;

        // [SerializeField] private CutsceneManager cutsceneManager;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Swim = Animator.StringToHash("Swim");

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            playerFish.SetActive(true);
            fish.SetActive(false);
            path = new Vector3[pathObjects.Length];
            for (int i = 0; i < pathObjects.Length; i++)
            {
                path[i] = pathObjects[i].transform.position;
            }
            
            fishPaths = new[]
            {
                new Vector3[6],
                new Vector3[6],
                new Vector3[6],
                new Vector3[6],
                new Vector3[6]
            };
            
            for (int i = 0; i < fishPathObjects1.Length; i++)
            {
                fishPaths[0][i] = fishPathObjects1[i].transform.position;
            }
            
            for (int i = 0; i < fishPathObjects2.Length; i++)
            {
                fishPaths[1][i] = fishPathObjects2[i].transform.position;
            }
            
            for (int i = 0; i < fishPathObjects3.Length; i++)
            {
                fishPaths[2][i] = fishPathObjects3[i].transform.position;
            }
            
            for (int i = 0; i < fishPathObjects4.Length; i++)
            {
                fishPaths[3][i] = fishPathObjects4[i].transform.position;
            }
            
            for (int i = 0; i < fishPathObjects5.Length; i++)
            {
                fishPaths[4][i] = fishPathObjects5[i].transform.position;
            }
            

            animator.SetBool(Moving, true);
            transform.DOMove(target1.transform.position, 2f).OnComplete(() =>
            {
                animator.SetBool(Moving, false);
                cutsceneManager.StartText();
            });
            
            
        }

        public override void MoveAgain()
        {
            fish.transform.position = playerFish.transform.position;
            fish.SetActive(true);
            playerFish.SetActive(false);

            fish.transform.DOPath(path, 1.5f, PathType.CatmullRom, pathMode: PathMode.Full3D).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    StartCoroutine(FinishAnimation());
                });
        }
        
        private IEnumerator FinishAnimation()
        {
            fishAnimator.SetTrigger(Swim);
            yield return new WaitForSeconds(2f);
            followFish.StartFollowing();
            fish.transform.DOMove(target3.transform.position, 20f);
            // MoveFish(0);
            
        }

        private void MoveFish(int index)
        {
            if (index > fishPaths.Length)
            {
                return;
            }
            fish.transform.DOPath(fishPaths[index], 1.5f, PathType.CatmullRom, pathMode: PathMode.Full3D).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // waterParticles.Play();
                    MoveFish(index + 1);
                });
        }
    }
}