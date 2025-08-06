using UnityEngine;
using Battle;
using System.Collections.Generic;


public class SkillManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private DialogTextManager dialogTextManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private SkillIconDatabase skillIconDatabase;
    [SerializeField] private BattleEffectManager battleEffectManager;
    [SerializeField] private EffectManager effectManager;
    [SerializeField] private SkillDatabase skillDatabase; 
    [SerializeField] private MenuUI menuUI;

    public void UsePlayerSkill(SkillType skillType, System.Action onSkillUsed = null)
    {
        if (IsOncePerBattleSkill(skillType) && battleManager.HasUsedOncePerBattleSkill(skillType))
        {
            dialogTextManager.ShowMessage(
                "このスキルは1バトルに1回しか使えません！",
                () => { battleManager.battleCommandUI.EnablePlayerButtons(true); dialogTextManager.ClearMessage();}, // ボタンを再度有効化
                false
            );
            return;
        }

        SkillData skill = skillDatabase.Get(skillType);

        if (playerManager.MP < skill.MpCost)
        {
            dialogTextManager.ShowMessage(
                "MPが足りません！",
                () => { battleManager.battleCommandUI.EnablePlayerButtons(true); dialogTextManager.ClearMessage();}, // ボタンを再度有効化
                false
            );
            return;
        }
        // 画像エフェクト
        if (skill.EffectPrefab != null)
        {
            effectManager.ShowEffect(skill.EffectPrefab, enemyManager.transform.position, 1.0f);
            Debug.Log("エフェクトを表示");
        }

        if (skillType == SkillType.EnemyNormalAttack || skillType == SkillType.EnemySkillAttack)
        {
            effectManager.ShakeObject(effectManager.enemyImage.transform, 0.2f, 20f);
        }


        playerManager.MP -= skill.MpCost;
        dialogTextManager.ShowMessage($"{menuUI.nameInputField.text}は「{skill.Name}」を使った！");

        if (IsOncePerBattleSkill(skillType))
        {
            battleManager.MarkOncePerBattleSkillUsed(skillType);
        }

        switch (skillType)
        {
            case SkillType.NormalAttack:
                int damage = SkillSystem.CalculateNormalAttackDamage(playerManager.Attack, enemyManager.Defense);
                enemyManager.TakeDamage(damage);
                SoundManager.instance.PlaySE(10); 
                effectManager.ShakeObject(effectManager.enemyImage.transform, 0.2f, 20f);
                dialogTextManager.ShowMessage(
                    $"{menuUI.nameInputField.text}の通常攻撃！\n{enemyManager.Name}に{damage}のダメージ！",
                    onSkillUsed == null ? null : (() => onSkillUsed()),
                    false
                );
                break;

            case SkillType.AttackUp:
                playerManager.Attack = SkillSystem.CalculateAttackUp(playerManager.Attack);
                SoundManager.instance.PlaySE(3); 
                battleEffectManager.AddPlayerEffectIcon(
                    SkillType.AttackUp,
                    skillIconDatabase.GetIcon(SkillType.AttackUp)
                );
                dialogTextManager.ShowMessage(
                    $"{menuUI.nameInputField.text}の攻撃力が2倍になった！",
                    onSkillUsed == null ? null : (() => onSkillUsed()),
                    false
                );
                break;

            case SkillType.AttackDown:
                enemyManager.Attack = SkillSystem.GetAttackDownValue();
                SoundManager.instance.PlaySE(11); 
                battleEffectManager.AddEnemyEffectIcon(
                    SkillType.AttackDown,
                    skillIconDatabase.GetIcon(SkillType.AttackDown)
                );
                dialogTextManager.ShowMessage(
                    $"{enemyManager.Name}の攻撃力が1になった！",
                    onSkillUsed == null ? null : (() => onSkillUsed()),
                    false
                );
                break;

            case SkillType.DefenseDown:
                enemyManager.Defense = SkillSystem.GetDefenseDownValue();
                SoundManager.instance.PlaySE(11); 
                battleEffectManager.AddEnemyEffectIcon(
                    SkillType.DefenseDown,
                    skillIconDatabase.GetIcon(SkillType.DefenseDown)
                );
                dialogTextManager.ShowMessage(
                    $"{enemyManager.Name}の防御力が0になった！",
                    onSkillUsed == null ? null : (() => onSkillUsed()),
                    false
                );
                break;

            case SkillType.PoisonAttack:
                var poisonEffect = SkillSystem.CreatePoisonEffect(playerManager.Attack);
                enemyManager.skillEffectManager.AddEffect(SkillType.PoisonAttack, poisonEffect);
                SoundManager.instance.PlaySE(9); 
                int poisonDamage = Mathf.Max(1, playerManager.Attack);
                enemyManager.TakeDamage(poisonDamage);
                effectManager.ShakeObject(effectManager.enemyImage.transform, 0.2f, 20f);
                battleEffectManager.AddEnemyEffectIcon(
                    SkillType.PoisonAttack,
                    skillIconDatabase.GetIcon(SkillType.PoisonAttack)
                );
                dialogTextManager.ShowMessage(
                    $"{menuUI.nameInputField.text}の毒攻撃！\n{enemyManager.Name}に{poisonDamage}のダメージ！毒状態になった！",
                    onSkillUsed == null ? null : (() => onSkillUsed()),
                    false
                );
                break;
        }
    }

    public void UseEnemySkill(SkillType skillType, System.Action onComplete = null)
    {
        switch (skillType)
        {
        case SkillType.EnemyNormalAttack:
            int damage = enemyManager.Attack;
            SoundManager.instance.PlaySE(8); 
            playerManager.HP -= damage;
            effectManager.ShakeObject(effectManager.shakeObject.transform, 0.2f, 20f);
            Debug.Log("オブジェクトシェイク");
            dialogTextManager.ShowMessage(
                $"{enemyManager.Name}の通常攻撃！\n{menuUI.nameInputField.text}に{damage}のダメージ！",
                () => { onComplete?.Invoke(); }, // ここで次の処理をコールバック
                false
            );
            break;

            case SkillType.EnemySkillAttack:
                int skillDamage = SkillSystem.CalculateSkillAttackDamage();
                SoundManager.instance.PlaySE(7); 
                playerManager.HP -= skillDamage;
                effectManager.ShakeObject(effectManager.shakeObject.transform, 0.2f, 20f);
                Debug.Log("オブジェクトシェイク");
                dialogTextManager.ShowMessage(
                    $"{enemyManager.Name}の強力な攻撃！\n{menuUI.nameInputField.text}に{skillDamage}のダメージ！", 
                    () => { onComplete?.Invoke(); },
                    false
                );
                break;

            case SkillType.EnemyAttackUp:
                enemyManager.Attack = SkillSystem.CalculateEnemyAttackUp(enemyManager.Attack);
                SoundManager.instance.PlaySE(3); 
                battleEffectManager.AddEnemyEffectIcon(
                    SkillType.EnemyAttackUp,
                    skillIconDatabase.GetIcon(SkillType.EnemyAttackUp)
                );
                dialogTextManager.ShowMessage(
                    $"{enemyManager.Name}の攻撃力が100倍になった！",
                    () => { onComplete?.Invoke(); },
                    false
                );
                break;

            case SkillType.EnemyDefenseUp:
                enemyManager.Defense = SkillSystem.CalculateEnemyDefenseUp(enemyManager.Defense);
                SoundManager.instance.PlaySE(5); 
                battleEffectManager.AddEnemyEffectIcon(
                    SkillType.EnemyDefenseUp,
                    skillIconDatabase.GetIcon(SkillType.EnemyDefenseUp)
                );
                dialogTextManager.ShowMessage(
                    $"{enemyManager.Name}の防御力が100倍になった！",
                    () => { onComplete?.Invoke(); },
                    false
                );
                break;
        }
    }

    public void ProcessEffects()
    {
        playerManager.ProcessSkillEffects();
        enemyManager.ProcessSkillEffects();
    }

    // 1バトル1回制限スキルか判定
    private bool IsOncePerBattleSkill(SkillType skillType)
    {
        return skillType == SkillType.AttackUp ||
            skillType == SkillType.PoisonAttack;
    }
}