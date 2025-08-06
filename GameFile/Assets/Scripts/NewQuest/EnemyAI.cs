using UnityEngine;
using Battle;
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;

    public SkillType DecideAction()
    {
        if (enemyManager.ShouldUseFirstSkill())
        {
            enemyManager.MarkFirstSkillUsed();
            return enemyManager.FirstSkill.Value;
        }
        if (enemyManager.ShouldUseTriggerSkill())
        {
            enemyManager.MarkTriggerSkillUsed(); // ←追加
            return enemyManager.TriggerSkill.Value;
        }
        return enemyManager.NormalAttackSkill;
    }
}