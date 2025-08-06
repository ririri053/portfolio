using UnityEngine;

public class BattleEffectUI : MonoBehaviour
{
    [SerializeField] public GameObject battleEffectPanel;

    public void ShowBattleEffectPanel() => battleEffectPanel.SetActive(true);
    public void HideBattleEffectPanel() => battleEffectPanel.SetActive(false);
}
