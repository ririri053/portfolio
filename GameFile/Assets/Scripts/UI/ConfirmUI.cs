using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    [SerializeField] private Text QuestCheckText;
    [SerializeField] private Text GachaCheckText;
    [SerializeField] private GameObject GachaCheckButton;
    // [SerializeField] private Button GachaCloseButton;
    [SerializeField] private GameObject QuestConfirmPanel;
    [SerializeField] private GameObject GachaConfirmPanel;
    [SerializeField] private GameObject DelConfirmPanel;


    public void ShowQuestConfirmPanel() => QuestConfirmPanel.SetActive(true);
    public void HideQuestConfirmPanel() => QuestConfirmPanel.SetActive(false);
    public void ShowGachaConfirmPanel() => GachaConfirmPanel.SetActive(true);
    public void HideGachaConfirmPanel() => GachaConfirmPanel.SetActive(false);
    public void ShowDelConfirmPanel() => DelConfirmPanel.SetActive(true);
    public void HideDelConfirmPanel() => DelConfirmPanel.SetActive(false);
}