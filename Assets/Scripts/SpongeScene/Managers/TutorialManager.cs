using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DoubleTrouble.Utilities;
using SpongeScene.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using SpongeScene.Triggers;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<SerializableTuple<Trigger, GameObject>> triggerToText;
    [SerializeField] private List<GamepadKeyData> inputsToAccompanyText;
    [SerializeField] private ParticleSystem splashParticleSystem; // Reusable particle system

    private int index = 0;
    private Coroutine tutorialCoroutine;

    private void OnDisable()
    {
        CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, OnEndGame);
    }
    
    private void OnEnable()
    {
        CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEndGame);
        print($"splash is alive: {splashParticleSystem is not null}");
    }

    private void OnEndGame(object obj)
    {
        StopAllCoroutines();
        tutorialCoroutine = null;
        index = 0;
        foreach (var kvp in triggerToText)
        {
            kvp.second.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerManager>())
        {
            print("CHIT PLAYER 23");
            if (tutorialCoroutine != null)
            {
                return;
            }
            print("START TUTORIAL 23");
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartTimer, null);
            tutorialCoroutine = StartCoroutine(RunTutorial());
        }
    }

    private IEnumerator RunTutorial()
    {        print($"splash is alive1: {splashParticleSystem is not null}");

        while (index != triggerToText.Count)
        {
            if (triggerToText[index].first.IsActivated())
            {
                triggerToText[index].second.SetActive(true);
                CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Pop);
                ShowSplashEffect(triggerToText[index].second.transform.position); // Show splash effect

                if (index > 0)
                {
                    triggerToText[index - 1].second.SetActive(false);
                }

                if (index == 1)
                {
                    index++;
                    continue;
                }
                yield return new WaitUntil(() =>
                {
                    var control = Gamepad.current.TryGetChildControl(inputsToAccompanyText[index].controlPath);

                    if (control is ButtonControl buttonControl)
                    {
                        return buttonControl.wasPressedThisFrame;
                    }
                    else if (control is AxisControl axisControl)
                    {
                        return Mathf.Abs(axisControl.ReadValue()) > 0.05f;
                    }

                    return false;
                });

                if (index++ == triggerToText.Count) break;
            }

            yield return null;
        }

        foreach (var kvp in triggerToText)
        {
            kvp.second.SetActive(false);
        }
    }

    private void ShowSplashEffect(Vector3 position)
    {
        if (splashParticleSystem != null)
        {
            splashParticleSystem.transform.position = position; // Move to new position
            splashParticleSystem.Stop(); // Ensure it's reset
            splashParticleSystem.Play(); // Play particle effect
        }
    }

    [Serializable]
    public class GamepadKeyData
    {
        public string controlPath; // Path to identify the control
    }
}
