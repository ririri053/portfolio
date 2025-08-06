using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "Game/QuestData")]
public class QuestData : ScriptableObject
{
    [SerializeField] public int questID;
    [SerializeField] public Sprite bgImage;  // 背景画像
    [SerializeField] public EnemyData enemyData;  // エネミーの配列
}
