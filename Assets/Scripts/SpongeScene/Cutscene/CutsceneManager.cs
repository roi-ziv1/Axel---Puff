using System.Collections;
using SpongeScene.Cutscene;
using SpongeScene.Managers;
using UnityEngine;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] protected GameObject fish;
    [SerializeField] protected GameObject playerFish;
    [SerializeField] protected TextMeshProUGUI dialogueText;
    [SerializeField] protected CutsceneMC cutsceneMC;
    [SerializeField] protected string[] dialogue;

    private int dialogueIndex = 0;

    protected virtual void Start()
    {
        playerFish.SetActive(false);
    }

    public void StartText()
    {
        dialogueText.text = dialogue[dialogueIndex];
        dialogueIndex++;
        StartCoroutine(PlayCutscene());
    }

    protected virtual IEnumerator PlayCutscene()
    {

        foreach (string dialogueString in dialogue)
        {
            dialogueText.text = dialogueString;
            float waitTime = GetWaitTime(dialogueString);
            yield return new WaitForSeconds(waitTime);
        }

        fish.SetActive(false);
        playerFish.SetActive(true);
        cutsceneMC.MoveAgain();

    }

    protected float GetWaitTime(string dialogueString)
    {
        int minLength = 10;  // Minimum length threshold
        int maxLength = 100; // Maximum length threshold
        float minTime = 2f;  // Minimum wait time
        float maxTime = 4f;  // Maximum wait time

        int length = dialogueString.Length;

        // Clamp the length to ensure it's between minLength and maxLength
        length = Mathf.Clamp(length, minLength, maxLength);

        // Linearly interpolate between minTime and maxTime
        return Mathf.Lerp(minTime, maxTime, (length - minLength) / (float)(maxLength - minLength));
    }
}