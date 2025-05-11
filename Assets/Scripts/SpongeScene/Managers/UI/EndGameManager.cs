using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace SpongeScene.Managers.UI
{
    public class EndGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private ParticleSystem splashParticleSystem1; // Reusable particle system
        [SerializeField] private ParticleSystem splashParticleSystem2; // Reusable particle system
        [SerializeField] private ParticleSystem gayserParticleSystem; // Reusable particle system
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI deaths;
        [SerializeField] private Canvas c;
        [SerializeField] private GameObject inputPanel;
        [SerializeField] private LeaderBoard leaderBoard;
        [SerializeField] private float a = 3f;
        private void Start()
        {
            splashParticleSystem1.Stop();
            splashParticleSystem2.Stop();
            gayserParticleSystem.Stop();
            CoreManager.Instance.EventsManager.AddListener(EventNames.GameOver, OnGameOver);
            CoreManager.Instance.EventsManager.AddListener(EventNames.ShowScoreBoard, ShowScoreBoard);

        }
        
        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.GameOver, OnGameOver);
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.ShowScoreBoard, ShowScoreBoard);
        }

   

        private void ShowScoreBoard(object obj)
        {
            RemoveStats();
            leaderBoard.LoadLeaderboard();
            leaderBoard.AddEntry(CoreManager.Instance.GameManager.playerName, CoreManager.Instance.GameManager.playerTimeFloat,CoreManager.Instance.GameManager.GetDeathCount());
            leaderBoard.ActivateLeaderBoard();
            endGamePanel.SetActive(true);
        }

        private void RemoveStats()
        {
            ShowSplashEffect(splashParticleSystem1,time.rectTransform);
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Pop);
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Pop);
            time.gameObject.SetActive(false);
            ShowSplashEffect(splashParticleSystem2,deaths.rectTransform);
            deaths.gameObject.SetActive(false);
            
        }

        private void OnGameOver(object obj)
        {

            StartCoroutine(StartEndGameFlow());
        }

        private IEnumerator StartEndGameFlow()
        {
            yield return new WaitForSeconds(2f);
            ShowStats();
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.BoilingWater);
            yield return new WaitForSeconds(4f);
            gayserParticleSystem.gameObject.transform.position = new Vector3(-70, 12, 0);
                
            gayserParticleSystem.Play();
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Gayser);
            yield return new WaitForSeconds(1.2f);
            gayserParticleSystem.Stop();
            yield return new WaitForSeconds(0.4f);
            inputPanel.SetActive(true);

        }

        private void ShowStats()
        {
  
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Pop);
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Pop);
            time.gameObject.SetActive(true);
            ShowSplashEffect(splashParticleSystem1,time.rectTransform);

            time.text = "Time : " +CoreManager.Instance.GameManager.GetTimeSinceStart();
            deaths.gameObject.SetActive(true);
            ShowSplashEffect(splashParticleSystem2,deaths.rectTransform);

            deaths.text = "Deaths : " +CoreManager.Instance.GameManager.GetDeathCount();
            


        }
        
        private void ShowSplashEffect(ParticleSystem s, RectTransform position)
        {
            var newPos = UtilityFunctions.UIToWorldNearCamera(position, c,UnityEngine.Camera.main);
            print($"particle pos is {newPos}");
            if (s != null)
            {
                s.transform.position = newPos; // Move to new position
                s.Stop(); // Ensure it's reset
                s.Play(); // Play particle effect
            }
        }
    }
    
    /*
     * flow :
     *      player disapears
     *      sprites show
     *      2 text apear with effect
     *      name input apears with effect
     *      go to leaderboards
     */
}