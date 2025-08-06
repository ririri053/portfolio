using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] public GameObject enemyVSPanel;
    [SerializeField] public Text enemyNameText;
    [SerializeField] public Image enemyImage;
    [SerializeField] private List<GameObject> startPanels;

    public void ShowEnemyVSPanel() => enemyVSPanel.SetActive(true);
    public void HideEnemyVSPanel() => enemyVSPanel.SetActive(false);

}