using UnityEngine;
using System.Collections.Generic;
using Battle;

public class EnemyManager : MonoBehaviour
{
    // エネミーの基本情報（クエストからロード）
    [SerializeField] public QuestManager questManager;
    [SerializeField] public EnemyUI enemyUI;
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public BattleResultUI battleResultUI;
    [SerializeField] public DialogTextManager dialogTextManager;
    [SerializeField] public StartUI startUI;
    [SerializeField] public GachaManager gachaManager;


    public SkillEffectManager skillEffectManager { get; private set; }

    // 敵のステータス
    public string Name { get; private set; }
    public Sprite EnemyImage { get; private set; }
    public int MaxHP { get; private set; }
    public int HP { get; private set; }
    public int MP { get; private set; } = 0;
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SkillPower { get; private set; } = 0;
    public int expReward { get; private set; }
    public int gachaPointReward { get; private set; }


    void Awake()
    {
        // すでにアタッチされていればそれを使う
        skillEffectManager = GetComponent<SkillEffectManager>();
        // なければAddComponentで追加
        if (skillEffectManager == null)
            skillEffectManager = gameObject.AddComponent<SkillEffectManager>();
    }
    
    // ダメージ処理
    public void TakeDamage(int damage)
    {
        HP = Mathf.Max(0, HP - damage);
        enemyUI.enemyHPText.text = $"HP : {FormatWithTransparentZeros(HP)}";
        enemyUI.UpdateHPBar(HP, MaxHP);
    }

    public int ProcessSkillEffects()
    {
        var result = skillEffectManager.ProcessEffects();
        int damage = result.TotalDamage;
        if (damage > 0)
        {
            TakeDamage(damage); // HP減算＋UI更新
            Debug.Log($"[敵HP減算] 毒ダメージ {damage} を受けた。現在HP: {HP}");
        }
        return damage;
    }

    // クエストに基づく敵の初期化
    public SkillType? FirstSkill { get; private set; }
    public SkillType? TriggerSkill { get; private set; }
    public float TriggerHPRate { get; private set; }
    public SkillType NormalAttackSkill { get; private set; }
    private bool usedFirstSkill = false;

        private string FormatWithTransparentZeros(int value)
    {
        return value.ToString().PadLeft(3, '_').Replace("_", "<color=#00000000>0</color>");
    }
    public void SetupQuest(int questID)
    {
        questManager.LoadQuest(questID);
        Debug.Log("SetupQuest後 questData: " + (questManager.questData != null ? "OK" : "NULL"));
        var data = questManager.questData.enemyData;

        // 各ステータスをセット
        Name = data.Name;
        EnemyImage = data.EnemyImage;
        HP = MaxHP = data.HP;
        Attack = data.Attack;
        Defense = data.Defense;
        expReward = data.EXP;
        gachaPointReward = data.GachaPoint;

        // UI 反映
        enemyUI.enemyImage.sprite = EnemyImage;
        enemyUI.enemyImage2.sprite = EnemyImage;
        enemyUI.enemyHPText.text = $"HP : {FormatWithTransparentZeros(HP)}";
        enemyUI.enemyNameText.text = Name;
        enemyUI.bgImage.sprite = questManager.questData.bgImage;
        startUI.enemyNameText.text = Name;
        startUI.enemyImage.sprite = EnemyImage;
        enemyUI.UpdateHPBar(HP, MaxHP);
        enemyUI.ShowEnemyImage();

        // 敵のスキル設定
        FirstSkill = data.FirstSkill;
        TriggerSkill = data.TriggerSkill;
        TriggerHPRate = data.TriggerHPRate;
        NormalAttackSkill = data.NormalAttackSkill;
        usedFirstSkill = false;

        usedFirstSkill = false;
        usedTriggerSkill = false; 
    }
    private bool usedTriggerSkill = false;
    
    public bool ShouldUseFirstSkill()
    {
        return FirstSkill.HasValue && !usedFirstSkill;
    }

    public bool ShouldUseTriggerSkill()
    {
        return TriggerSkill.HasValue && HP <= MaxHP * TriggerHPRate && !usedTriggerSkill;
    }

    public void MarkFirstSkillUsed()
    {
        usedFirstSkill = true;
    }

    public void MarkTriggerSkillUsed()
    {
        usedTriggerSkill = true;
    }

    public void GainExp()
    {
        playerManager.currentPlayer.EXP += expReward;
        battleResultUI.expText.text = "獲得経験値 : " + expReward.ToString();
        // --- セーブ処理 ---
        playerManager.EXPSaveLoad();
    }

    public void GainGachaPoint()
    {
        playerManager.currentPlayer.GachaPoint += gachaPointReward;
        battleResultUI.gachaPointText.text = "獲得ガチャポイント : " + gachaPointReward.ToString();
        // --- セーブ処理 ---
        gachaManager.GachaLoadSave();
    }
}