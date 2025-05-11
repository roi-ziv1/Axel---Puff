using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace SpongeScene.Managers.UI
{
    public class WaterUI : MonoBehaviour
    {
        [SerializeField] private Image[] waterBars = new Image[3];
        [SerializeField] private Image[] waterFillers = new Image[3];
        [SerializeField] private Image currentWaterBar;
        [SerializeField] private Image currentWaterFiller;

        public PlayerManager playerManager;

        private void Start()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEndGame);
            CoreManager.Instance.EventsManager.AddListener(EventNames.ToMainMenu, OnToMainMenu);
        }
        
        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEndGame);
            CoreManager.Instance.EventsManager.AddListener(EventNames.ToMainMenu, OnToMainMenu);
        }

        public void UpdateWater(float water)
        {
            currentWaterFiller.fillAmount = water / playerManager.MaxWater;
        }
        
        public void SetPlayerManager(PlayerManager player)
        {
            playerManager = player;
            InitializeWater(player.CurrentWater);
        }
        
        public void InitializeWater(float water)
        {
            currentWaterBar = waterBars[0];
            currentWaterFiller = waterFillers[0];
            currentWaterFiller.fillAmount = water / playerManager.MaxWater;
        }
        
        public void ChangeWaterBar(int index)
        {
            currentWaterBar.gameObject.SetActive(false);
            currentWaterBar = waterBars[index];
            currentWaterFiller = waterFillers[index];
            currentWaterBar.gameObject.SetActive(true);
            currentWaterFiller.fillAmount = playerManager.CurrentWater / playerManager.MaxWater;
        }
        
        private void OnEndGame(object obj)
        {
            DisableBars();
        }
        
        private void OnToMainMenu(object obj)
        {
            print("ToMainMenu");
            DisableBars();
        }
        
        private void OnStartGame(object obj)
        {
            currentWaterBar.gameObject.SetActive(false);
            currentWaterBar = waterBars[0];
            currentWaterFiller = waterFillers[0];
            currentWaterFiller.fillAmount = playerManager.CurrentWater / playerManager.MaxWater;
            currentWaterBar.gameObject.SetActive(true);
        }
        
        public void DisableBars()
        {
            foreach (var waterBar in waterBars)
            {
                waterBar.gameObject.SetActive(false);
            }

            foreach (var filler in waterFillers)
            {
                filler.fillAmount = 0;
            }
            currentWaterBar = waterBars[0];
            currentWaterFiller = waterFillers[0];
        }
        
    }
}
