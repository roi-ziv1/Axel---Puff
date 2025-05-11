using DG.Tweening;
using SpongeScene.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpongeScene.Cutscene
{
    public class CutsceneMC : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] protected GameObject target1;
        [SerializeField] protected GameObject target2;
        [SerializeField] protected GameObject target3;
        [SerializeField] protected GameObject midPoint;
        [SerializeField] protected Animator animator;
        [SerializeField] protected CutsceneManager cutsceneManager;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Jumping = Animator.StringToHash("Jump");

        protected virtual void Start()
        {
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartEndCutScene, null);

            animator.SetBool(Moving, true);
            // Move the transform of the object to the target1 position ind 5 seconds and at the end set the Moving bool to false
            transform.DOMove(target1.transform.position, 2.5f).OnComplete(() =>
            {
                animator.SetBool(Moving, false);
                cutsceneManager.StartText();
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                CoreManager.Instance.SceneManager.LoadNextScene();
            }
        }

        public virtual void MoveAgain()
        {
            animator.SetBool(Moving, true);
            Vector3[] path = {target2.transform.position, midPoint.transform.position, target3.transform.position};
            // Move the transform of the object to the target2 position ind 4 seconds and at the end set the Moving bool to false, set the Jumping bool to true then move the transform to the target3 position in an jumping way in 2 seconds
            transform.DOMove(target2.transform.position, 2f).OnComplete(() =>
            {
                animator.SetBool(Moving, false);
                animator.SetBool(Jumping, true);
                transform.DOPath(path, 1.5f, PathType.CatmullRom, pathMode: PathMode.Full3D).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    print("CALL START EVENT!");
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    CoreManager.Instance.SceneManager.LoadNextScene();
                });
            });
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            CoreManager.Instance.player.gameObject.SetActive(true);
            CoreManager.Instance.player.transform.position =
                CoreManager.Instance.PositionManager.GetSceneStartingPosition(3);
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartGame,null);
        }
    }
}
