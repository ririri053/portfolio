using UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject QuestSelectPanel;
    [SerializeField] private ConfirmUI confirmUI;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] public Button[] challengeButtons;
    [SerializeField] private Text text1;
    [SerializeField] private Text text2;
    [SerializeField] private FadeUI fadeUI;
    [SerializeField] private BattleManager battleManager;
    
    public void ShowQuestSelectPanel() => QuestSelectPanel.SetActive(true);
    public void HideQuestSelectPanel() => QuestSelectPanel.SetActive(false);

    public void QuestButtonUIUpdate(QuestUnlockedSaveData data)
    {
        if (data == null || data.questUnlocked == null)
        {
            return;
        }

        int len = Mathf.Min(challengeButtons.Length, data.questUnlocked.Length);

        for (int i = 0; i < len; i++)
        {
            SetButtonState(challengeButtons[i], data.questUnlocked[i]);
        }
    }

    public void SetButtonState(Button btn, bool isUnlocked)
    {
        btn.interactable = isUnlocked;

        var image = btn.GetComponent<Image>();
            if (image != null)
            {
            image.color = isUnlocked ? Color.white : Color.black;
        }

        var text = btn.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.color = isUnlocked ? new Color32(50, 50, 50, 255) : new Color32(50, 50, 50, 0);
        }
    }

    private int selectedQuestId = -1;
    public void OnQuestButtonClicked(int questId)
    {
        selectedQuestId = questId;
        confirmUI.ShowQuestConfirmPanel(); // 確認画面を表示
        Debug.Log("Quest ID: " + selectedQuestId);
    }

        public void OnConfirmYesButton()
    {
        fadeUI.FadeOutAndHide();
        panelManager.ShowEnemy(selectedQuestId); // クエストIDでバトル開始
    }

    public void ApplySaveData(QuestUnlockedSaveData data)
    {
        for (int i = 0; i < challengeButtons.Length; i++)
        {
            bool isUnlocked = data.questUnlocked != null && i < data.questUnlocked.Length && data.questUnlocked[i];
            SetButtonState(challengeButtons[i], isUnlocked);
        }
    }
}   