using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject QuestPanel;

    public void ShowQuestPanel() => QuestPanel.SetActive(true);
    public void HideQuestPanel() => QuestPanel.SetActive(false);
}
