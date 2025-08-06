using UnityEngine;
using UnityEngine.UI;

public class LevelupUI : MonoBehaviour
{
    [SerializeField] public Text LevelupHPText;
    [SerializeField] public Text AddHPText;
    [SerializeField] public Text LevelupMPText;
    [SerializeField] public Text AddMPText;
    [SerializeField] public Text LevelupLevelText;
    [SerializeField] public Text LevelupAttackText;
    [SerializeField] public Text AddAttackText;
    [SerializeField] public Text AddLevelText;
    [SerializeField] public Text LevelupNextLevelText;
    [SerializeField] public GameObject LevelupPanel;

    public void ShowLevelupPanel() => LevelupPanel.SetActive(true);
    public void HideLevelupPanel() => LevelupPanel.SetActive(false); 
}
