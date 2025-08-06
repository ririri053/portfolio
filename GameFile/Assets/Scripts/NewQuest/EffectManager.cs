using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] public GameObject enemyImage;
    [SerializeField] public GameObject shakeObject;
    [SerializeField] public GameObject effectContent;
// 画像エフェクトを表示 → プレハブを生成して表示
public void ShowEffect(GameObject effectPrefab, Vector3 position, float duration = 1.0f)
{
    if (effectPrefab == null || effectContent == null) return;

    GameObject effectInstance = Instantiate(effectPrefab, effectContent.transform);
    effectInstance.transform.localPosition = position; // ローカル座標で配置
    Destroy(effectInstance, duration);
    Debug.Log("ShowEffect called with effectPrefab: " + effectPrefab.name + ", position: " + position);
}

    // GameObjectを揺らす
    public void ShakeObject(Transform target, float duration = 0.2f, float magnitude = 20f)
    {
        StartCoroutine(ShakeObjectCoroutine(target, duration, magnitude));
    }

    private IEnumerator ShakeObjectCoroutine(Transform target, float duration, float magnitude)
    {
        Vector3 originalPos = target.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            target.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localPosition = originalPos;
    }

    public void ShakeScreen(RectTransform canvasRect, float duration = 0.2f, float magnitude = 20f)
    {
        StartCoroutine(ShakeObjectCoroutine(canvasRect, duration, magnitude));
    }
}