using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private GameObject BattlePanel;

    public void ShowBattlePanel() => BattlePanel.SetActive(true);
    public void HideBattlePanel() => BattlePanel.SetActive(false);
}
