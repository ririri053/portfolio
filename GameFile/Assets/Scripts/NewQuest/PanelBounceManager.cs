using UnityEngine;
using DG.Tweening;

namespace UI
{
    public class PanelBounceManager : MonoBehaviour
    {
        private float bounceDuration = 0.5f;
        private float bounceScale = 1.1f;

        public void PlayBounceAnimation(RectTransform target)
        {
            if (target == null)
            {
                Debug.LogError("PanelBounceManager: targetがnullです");
                return;
            }

            // 現在のアニメーションをキル
            target.DOKill();

            // バウンスアニメーションのシーケンス
            Sequence sequence = DOTween.Sequence();
            
            // 拡大
            sequence.Append(target.DOScale(Vector3.one * bounceScale, bounceDuration / 2)
                .SetEase(Ease.OutQuad));
            
            // 元のサイズに戻る
            sequence.Append(target.DOScale(Vector3.one, bounceDuration / 2)
                .SetEase(Ease.InQuad));

            Debug.Log($"バウンスアニメーション開始: {target.name}");
        }

        public void PlayShowAnimation(RectTransform target)
        {
            if (target == null)
            {
                Debug.LogError("PanelBounceManager: targetがnullです");
                return;
            }

            // 現在のアニメーションをキル
            target.DOKill();

            // 初期設定
            target.localScale = Vector3.zero;
            target.gameObject.SetActive(true);

            // 表示アニメーション
            target.DOScale(Vector3.one, bounceDuration)
                .SetEase(Ease.OutBack);

            Debug.Log($"表示アニメーション開始: {target.name}");
        }

        public void PlayHideAnimation(RectTransform target)
        {
            if (target == null)
            {
                Debug.LogError("PanelBounceManager: targetがnullです");
                return;
            }

            // 現在のアニメーションをキル
            target.DOKill();

            // 非表示アニメーション
            target.DOScale(Vector3.zero, bounceDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => target.gameObject.SetActive(false));

            Debug.Log($"非表示アニメーション開始: {target.name}");
        }
    }
} 