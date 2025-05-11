using System.Collections;
using UnityEngine;

namespace SpongeScene.Cutscene
{
    public class EndingCutsceneManager : CutsceneManager
    {
        protected override void Start()
        {
            playerFish.SetActive(true);
        }
        
        protected override IEnumerator PlayCutscene()
        {
            foreach (string dialogueString in dialogue)
            {
                dialogueText.text = dialogueString;
                float waitTime = GetWaitTime(dialogueString);
                yield return new WaitForSeconds(waitTime);
            }
            
            cutsceneMC.MoveAgain();
            dialogueText.text = "";

        }
    }
}
