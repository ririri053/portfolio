using UnityEngine;

namespace Battle
{
    public enum SkillType
    {
        // プレイヤースキル
        NormalAttack, //0
        AttackUp, 
        AttackDown,
        DefenseDown,
        PoisonAttack,
        // エネミースキル
        EnemyNormalAttack,
        EnemySkillAttack,
        EnemyAttackUp,
        EnemyDefenseUp,
    }

    // エフェクトの種類を追加
    public enum SkillEffectType
    {
        Poison,
        AttackUp,
        DefenseUp,
        AttackDown,
        DefenseDown,
        EnemyAttackUp,
        EnemyDefenseUp,
        // 今後他の効果も追加可能
    }

    public class SkillEffect
    {
        public SkillEffectType EffectType { get; private set; }
        public int Duration { get; private set; }
        public int Power { get; private set; }

        public SkillEffect(SkillEffectType effectType, int duration, int power)
        {
            EffectType = effectType;
            Duration = duration;
            Power = power;
        }

        public void DecreaseDuration()
        {
            Duration--;
            Debug.Log($"[毒] 残りターン: {Duration}");
        }

        // エフェクトの効果を適用（例：毒ダメージを返す）
        public int ApplyEffect()
        {
            if (EffectType == SkillEffectType.Poison)
            {
                Debug.Log($"[毒] ダメージ発生: {Power}（残りターン: {Duration}）");
                return Power;
            }
            return 0;
        }
    }

    public class SkillSystem
    {
        public static int CalculateNormalAttackDamage(int playerAttack, int enemyDefense)
        {
            return Mathf.Max(1, playerAttack - enemyDefense);
        }

        public static int CalculateAttackUp(int currentAttack)
        {
            return currentAttack * 2;
        }

        public static int GetAttackDownValue()
        {
            return 1;
        }

        public static int GetDefenseDownValue()
        {
            return 0;
        }

        public static SkillEffect CreatePoisonEffect(int playerAttack)
        {
            return new SkillEffect(SkillEffectType.Poison, 4, playerAttack);
        }

        // エネミースキル
        public static int CalculateSkillAttackDamage()
        {
            return 25;
        }

        public static int CalculateEnemyAttackUp(int currentAttack)
        {
            return currentAttack * 100;
        }

        public static int CalculateEnemyDefenseUp(int currentDefense)
        {
            return currentDefense * 100;
        }

        public static SkillEffect CreateAttackUpEffect(int power, int duration = 99)
        {
            return new SkillEffect(SkillEffectType.AttackUp, duration, power);
        }
        public static SkillEffect CreateAttackDownEffect(int power, int duration = 99)
        {
            return new SkillEffect(SkillEffectType.AttackDown, duration, power);
        }
        public static SkillEffect CreateDefenseDownEffect(int power, int duration = 99)
        {
            return new SkillEffect(SkillEffectType.DefenseDown, duration, power);
        }
        public static SkillEffect CreateEnemyAttackUpEffect(int power, int duration = 99)
        {
            return new SkillEffect(SkillEffectType.EnemyAttackUp, duration, power);
        }
        public static SkillEffect CreateEnemyDefenseUpEffect(int power, int duration = 99)
        {
            return new SkillEffect(SkillEffectType.EnemyDefenseUp, duration, power);
        }
    }
}