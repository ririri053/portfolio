using UnityEngine;
using UnityEngine.UI;

public class BattleResultUI : MonoBehaviour
{
    [SerializeField] public Text winText;
    [SerializeField] public Text loseText;
    [SerializeField] public Text expText;
    [SerializeField] public Text gachaPointText;
    [SerializeField] public GameObject BattleResultPanel;
    [SerializeField] public GameObject winResult;
    [SerializeField] public GameObject loseResult;
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public QuestManager questManager;
    [SerializeField] public EnemyManager enemyManager;
    


    public void ShowBattleResultPanel() => BattleResultPanel.SetActive(true);
    public void HideBattleResultPanel() 
    {
        BattleResultPanel.SetActive(false);
        winResult.SetActive(false);
        loseResult.SetActive(false);
    }

    public void ResultWin()
    {
        winText.text = "VS " + questManager.questData.enemyData.Name  + " 勝利!!";
        winResult.SetActive(true);
        ShowBattleResultPanel();
    }

    public void ResultLose()
    {
        loseText.text = "VS " + questManager.questData.enemyData.Name  + " 敗北";
        loseResult.SetActive(true);
        ShowBattleResultPanel();
    }
}