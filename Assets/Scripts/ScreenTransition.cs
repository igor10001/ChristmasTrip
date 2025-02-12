using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition Instance;
    private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float fullBlackDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void StartTransition(System.Action onMidTransition = null)
    {
        Sequence transitionSequence = DOTween.Sequence();

        transitionSequence.Append(canvasGroup.DOFade(1, fadeDuration))
            .AppendCallback(() => onMidTransition?.Invoke())
            .AppendInterval(fullBlackDuration)
            .Append(canvasGroup.DOFade(0, fadeDuration));
    }
}