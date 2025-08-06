using UnityEngine;
using System.Collections.Generic;
using Battle;

public class BattleEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject effectPlayerPrefab;
    [SerializeField] private GameObject effectEnemyPrefab;
    [SerializeField] private Transform playerEffectContent;
    [SerializeField] private Transform enemyEffectContent;

    private Dictionary<SkillType, EffectIcon> playerIcons = new Dictionary<SkillType, EffectIcon>();
    private Dictionary<SkillType, EffectIcon> enemyIcons = new Dictionary<SkillType, EffectIcon>();

    // SkillTypeの相反関係を返すメソッド
    private SkillType? GetOppositeSkillType(SkillType type)
    {
        switch (type)
        {
            case SkillType.AttackDown:
                return SkillType.EnemyAttackUp;
            case SkillType.EnemyAttackUp:
                return SkillType.AttackDown;
            case SkillType.DefenseDown:
                return SkillType.EnemyDefenseUp;
            case SkillType.EnemyDefenseUp:
                return SkillType.DefenseDown;
            default:
                return null;
        }
    }

    public void AddPlayerEffectIcon(SkillType type, Sprite icon)
    {
        // 相反するアイコンがあれば削除
        var opposite = GetOppositeSkillType(type);
        if (opposite.HasValue && playerIcons.ContainsKey(opposite.Value))
        {
            var iconToRemove = playerIcons[opposite.Value];
            if (iconToRemove != null)
                Destroy(iconToRemove.gameObject);
            playerIcons.Remove(opposite.Value);
        }
        if (!playerIcons.ContainsKey(type))
        {
            var obj = Instantiate(effectPlayerPrefab, playerEffectContent);
            var effectIcon = obj.GetComponent<EffectIcon>();
            effectIcon.SetIcon(icon);
            playerIcons[type] = effectIcon;
        }
    }

    public void AddEnemyEffectIcon(SkillType type, Sprite icon) 
    {
        // 相反するアイコンがあれば削除
        var opposite = GetOppositeSkillType(type);
        if (opposite.HasValue && enemyIcons.ContainsKey(opposite.Value))
        {
            var iconToRemove = enemyIcons[opposite.Value];
            if (iconToRemove != null)
                Destroy(iconToRemove.gameObject);
            enemyIcons.Remove(opposite.Value);
        }
        if (!enemyIcons.ContainsKey(type))
        {
            var obj = Instantiate(effectEnemyPrefab, enemyEffectContent);
            var effectIcon = obj.GetComponent<EffectIcon>();
            effectIcon.SetIcon(icon);
            enemyIcons[type] = effectIcon;
        }
    }

    public void ClearAllEffectIcons()
    {
        // プレイヤー側のアイコンを全削除
        foreach (var icon in playerIcons.Values)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }
        playerIcons.Clear();

        // エネミー側のアイコンを全削除
        foreach (var icon in enemyIcons.Values)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }
        enemyIcons.Clear();
    }

    public void ClearPoisonEffectIcons()
    {
        Debug.Log("ClearPoisonEffectIcons called");
        // 毒アイコンを削除
        if (enemyIcons.TryGetValue(SkillType.PoisonAttack, out var enemyPoisonIcon))
        {
            if (enemyPoisonIcon != null)
                Destroy(enemyPoisonIcon.gameObject);
            enemyIcons.Remove(SkillType.PoisonAttack);
        }
    }
}
