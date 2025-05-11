using System;
using SpongeScene.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SpongeScene.Menus
{
    public class NameInputHandler : MonoBehaviour
    {
        public TMP_InputField nameInputField; // Use InputField if not using TextMeshPro

        private void OnEnable()
        {
            // Listen for Enter key press
            nameInputField.gameObject.SetActive(true);
            nameInputField.onSubmit.AddListener(SubmitName);
            nameInputField.text = "Enter name..";
        }
        
        private void OnDisable()
        {
            // Listen for Enter key press
            nameInputField.gameObject.SetActive(false);
            nameInputField.onSubmit.RemoveListener(SubmitName);
        }


        // Submit the name and trigger the event
        private void SubmitName(string input = null)
        {
            string playerName = nameInputField.text.Trim();
            if (!string.IsNullOrEmpty(playerName) && playerName.Length < 10)
            {
                CoreManager.Instance.GameManager.SetPlayerName(input);
                CoreManager.Instance.EventsManager.InvokeEvent(EventNames.ShowScoreBoard, null);
                ClosePanel();
            }
            else
            {
                nameInputField.text = "name must be less than 11 characters";
            }
        }

        // Close the input panel
        private void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}