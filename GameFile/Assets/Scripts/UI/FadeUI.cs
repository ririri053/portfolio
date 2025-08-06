using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace UI
{
    public class FadeUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration = 0.01f;

        public static FadeUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            fadePanel.alpha = 1f;
            fadePanel.blocksRaycasts = true;
            fadePanel.gameObject.SetActive(false);
        }

        public void FadeOutAndHide(System.Action onComplete = null)
        {
            StartCoroutine(FadeOutRoutine(onComplete));
        }

        private IEnumerator FadeOutRoutine(System.Action onComplete)
        {
            // アクティブ化（念のため）
            fadePanel.gameObject.SetActive(true);
            fadePanel.blocksRaycasts = true;

            // フェードアウト（黒→透明）
            yield return fadePanel.DOFade(0f, fadeDuration).WaitForCompletion();

            // 非表示にして次の準備
            fadePanel.blocksRaycasts = false;
            fadePanel.gameObject.SetActive(false);
            fadePanel.alpha = 1f;

            onComplete?.Invoke();
        }
    }
}
