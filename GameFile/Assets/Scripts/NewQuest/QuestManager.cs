using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestData> questList; // 複数のクエストを登録

    public QuestData questData { get; private set; }
    public int selectedQuestId = -1; // ◀ この行を追加

    public void LoadQuest(int questID)
    {
        selectedQuestId = questID; // ← この行を追加してください
        questData = questList.Find(q => q.questID == questID);

        if (questData == null)
        {
            Debug.LogError($"QuestData with ID {questID} not found in questList!");
        }
        else
        {
            Debug.Log($"QuestData loaded: {questData.questID} / Enemy: {questData.enemyData?.Name}");
        }
    }
}

