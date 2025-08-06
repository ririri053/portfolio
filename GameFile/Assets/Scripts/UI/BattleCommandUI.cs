using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Battle;

public class BattleCommandUI : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button skillButton;
    [SerializeField] private Button runButton;
    [SerializeField] private Button talkButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillDescriptionText;
    [SerializeField] private Text skillMpCostText;
    [SerializeField] private GameObject skillCommandPanel;
    [SerializeField] private GameObject battleCommandPanel;
    [SerializeField] private GameObject skillPanel;

    public void ShowBattleCommandPanel() => battleCommandPanel.SetActive(true);
    public void HideBattleCommandPanel() => battleCommandPanel.SetActive(false);
    public void HideSkillCommandPanel() => skillCommandPanel.SetActive(false);
    public void ShowSkillCommandPanel() => skillCommandPanel.SetActive(true);

    public void ShowSkillPanel(SkillData skillData) 
    {
        skillPanel.SetActive(true);
        skillNameText.text = skillData.Name;
        skillDescriptionText.text = skillData.Description;
        skillMpCostText.text = "$消費MP:" + skillData.MpCost.ToString();
        backButton.gameObject.SetActive(false);
    }
    public void HideSkillPanel() 
    {
        skillPanel.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void UseSkillHidePanel()
    {
        HideSkillPanel();
        HideSkillCommandPanel();

    }

    public void EnablePlayerButtons(bool enable)
    {
        attackButton.interactable = enable;
        skillButton.interactable = enable;
        runButton.interactable = enable;
        talkButton.interactable = enable;;
    }
}
