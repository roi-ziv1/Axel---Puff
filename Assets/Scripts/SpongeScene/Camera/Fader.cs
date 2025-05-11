using DG.Tweening;
using SpongeScene.Managers;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public static Fader Instance { get; private set; }
    
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    void Start()
    {
        canvasGroup.alpha = 0;
        CoreManager.Instance.EventsManager.AddListener(EventNames.StartNewScene, OnStartScene);
    }
    
    public void FadeOut(float duration)
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, duration);
    }
    
    public void FadeIn(float duration)
    {
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, duration);
    }
    
    public void FadeInAndOut(float duration)
    {
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(0, duration)) // Fade In
            .AppendInterval(0.2f) // Wait before fading out
            .Append(canvasGroup.DOFade(1, duration)) // Fade Out
            .OnComplete(() => canvasGroup.blocksRaycasts = false);
    }
    
    private void OnStartScene(object obj)
    {
        FadeIn(1f);
    }
    
    
}
