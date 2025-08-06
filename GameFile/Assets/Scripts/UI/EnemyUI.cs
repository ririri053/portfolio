using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] public Text enemyNameText;
    [SerializeField] public Text enemyHPText;
    [SerializeField] public Slider enemyHPBar;
    [SerializeField] public Image enemyImage; 
    [SerializeField] public Image enemyImage2; 
    [SerializeField] public Image bgImage;
    [SerializeField] public GameObject EnemyPanel;

    public void ShowEnemyPanel() => EnemyPanel.SetActive(true);
    public void HideEnemyPanel() => EnemyPanel.SetActive(false);
    public void ShowEnemyImage() => enemyImage.gameObject.SetActive(true);
    public void HideEnemyImage() => enemyImage.gameObject.SetActive(false);
    public void UpdateHPBar(int currentHP, int maxHP)
    {
        float hpPercentage = (float)currentHP / maxHP;
        enemyHPBar.value = hpPercentage;  // Slider の値を更新
    }

    public void DeadEnemy()
    {
        enemyImage.gameObject.transform.Rotate(0, 0, -30);
        enemyImage2.gameObject.transform.Rotate(0, 0, -30);
    }

    public void SetEnemy()
    {
        enemyImage.gameObject.transform.Rotate(0, 0, 30);
        enemyImage2.gameObject.transform.Rotate(0, 0, 30);
    }
}