using UnityEngine;
using Battle;

public class PlayerManager : MonoBehaviour
{
    [Header("UI関連")]
    [SerializeField] public EnemyUI enemyUI;
    [SerializeField] public PlayerUI playerUI;
    [SerializeField] public QuestManager questManager;
    [SerializeField] public BattleResultUI battleResultUI;
    [SerializeField] public MenuUI menuUI;
    [SerializeField] public LevelupUI levelupUI;
    [SerializeField] public PanelManager panelManager;
    [SerializeField] public DialogTextManager dialogTextManager;
    [SerializeField] public DialogUI dialogUI;

    [System.Serializable]
    public class PlayerStatus
    {
        public int Level = 1;
        public int MaxHP = 50;
        public int MaxMP = 30;
        public int Attack = 5;
        public int SkillPower = 0;
        public int EXP = 0;
        public int LevelUpEXP = 10;
        public int GachaPoint = 0;
        
        // コピー用メソッド
        public PlayerStatus Clone()
        {
            return (PlayerStatus)this.MemberwiseClone();
        }
    }

    // ステータス管理
    private PlayerStatus baseStatus;       // 初期ステータス（変化しない）
    public PlayerStatus currentPlayer;     // 成長後ステータス（レベルアップ後）
    private int battleHP;
    private int battleMP;
    private int battleAttack;

    public SkillEffectManager skillEffectManager { get; private set; }

    // レベルアップ時の成長量
    private int LevelUpHP = 5;
    private int LevelUpMP = 2;
    private int LevelUpAttack = 1;
    private int LevelUpSkillPower = 0;

    public void InitBaseStatus()
    {
        baseStatus = new PlayerStatus();
        currentPlayer = baseStatus.Clone();
    }

    void Awake()
    {
        // すでにアタッチされていればそれを使う
        skillEffectManager = GetComponent<SkillEffectManager>();
        // なければAddComponentで追加
        if (skillEffectManager == null)
            skillEffectManager = gameObject.AddComponent<SkillEffectManager>();
    }

    private string FormatWithTransparentZeros(int value)
    {
        return value.ToString().PadLeft(3, '_').Replace("_", "<color=#00000000>0</color>");
    }

    public void Initialize()
    {
        Debug.Log("最新のプレイヤーバトルステータス更新");
        playerUI.hpText.text = $"HP : {FormatWithTransparentZeros(currentPlayer.MaxHP)}";
        playerUI.mpText.text = $"MP : {FormatWithTransparentZeros(currentPlayer.MaxMP)}";
        playerUI.levelText.text = $"Lv.{currentPlayer.Level}";
    }

    public void PrepareBattle()
    {
        Debug.Log("プレイヤーのHP,MPを補充！バトル準備完了");
        battleHP = currentPlayer.MaxHP;
        battleMP = currentPlayer.MaxMP;
        battleAttack = currentPlayer.Attack; // ◀ この行を追加
    }

    public void LevelUp()
    {
        int levelUpCount = 0;
        int totalHP = 0;
        int totalMP = 0;
        int totalAttack = 0;
        int totalSkillPower = 0;

        while (currentPlayer.EXP >= currentPlayer.LevelUpEXP)
        {
            Debug.Log("プレイヤーのレベルアップを検知");
            currentPlayer.Level++;
            currentPlayer.MaxHP += LevelUpHP;
            currentPlayer.MaxMP += LevelUpMP;
            currentPlayer.Attack += LevelUpAttack;
            currentPlayer.SkillPower += LevelUpSkillPower;
            currentPlayer.EXP -= currentPlayer.LevelUpEXP;
            currentPlayer.LevelUpEXP += 5;

            levelUpCount++;
            totalHP += LevelUpHP;
            totalMP += LevelUpMP;
            totalAttack += LevelUpAttack;
            totalSkillPower += LevelUpSkillPower;
        }

        if (levelUpCount > 0)
        {
            levelupUI.LevelupLevelText.text = $"Lv.{currentPlayer.Level}";
            levelupUI.LevelupHPText.text = $"HP : {currentPlayer.MaxHP}";
            levelupUI.LevelupMPText.text = $"MP : {currentPlayer.MaxMP}";
            levelupUI.LevelupAttackText.text = $"ATK : {currentPlayer.Attack}";
            levelupUI.LevelupNextLevelText.text = $"Next Level : {currentPlayer.LevelUpEXP}";
            levelupUI.AddLevelText.text = $"+{levelUpCount}";
            levelupUI.AddHPText.text = $"+{totalHP}";
            levelupUI.AddMPText.text = $"+{totalMP}";
            levelupUI.AddAttackText.text = $"+{totalAttack}";
            // 必要ならSkillPowerも
            // levelupUI.AddSkillPowerText.text = $"+{totalSkillPower}";

            panelManager.ShowLevelupPanel();
            LevelUpDialog(totalHP, totalMP, totalAttack);
        }
        else
        {
            panelManager.HideQuest();
        }

        // --- セーブ処理 ---
        EXPSaveLoad();
        PlayerStatusSaveLoad();
    }

    public void LevelUpDialog(int totalHP, int totalMP, int totalAttack)
    {
        string[] levelUpMessages = new string[]
        {
            $"レベルが {currentPlayer.Level} に上がりました。",
            $"HPが {totalHP} 上がりました。",
            $"MPが {totalMP} 上がりました。",
            $"ATKが {totalAttack} 上がりました。"
        };

        dialogTextManager.SetScenarios(levelUpMessages, () =>
        {
            dialogUI.HideDialogPanel();
        });
    }

    // バトル中の一時HP/MP
    public int HP
    {
        get => battleHP;
        set => battleHP = Mathf.Clamp(value, 0, currentPlayer.MaxHP);
    }

    public int MP
    {
        get => battleMP;
        set => battleMP = Mathf.Clamp(value, 0, currentPlayer.MaxMP);
    }

    public int Attack
    {
        get => battleAttack;
        set => battleAttack = value; // ◀ この行を修正
    }

    // 読み取り専用
    public int SkillPower => currentPlayer.SkillPower;

    public int AttackEnemy()
    {
        return currentPlayer.Attack;
    }

    public int ProcessSkillEffects()
    {
        var result = skillEffectManager.ProcessEffects();
        int damage = result.TotalDamage;
        if (damage > 0)
        {
            HP -= damage;
            Debug.Log($"[HP減算] 毒ダメージ {damage} を受けた。現在HP: {HP}");
        }
        return damage;
    }

    // 初期ステータスにリセットしたい時用
    public void ResetToBaseStatus()
    {
        currentPlayer = baseStatus.Clone();
        PrepareBattle();
    }

    // --- セーブデータ連携 ---
    public void PlayerStatusApplySaveData(PlayerSaveData data)
    {
        currentPlayer.Level = data.playerLevel;
        currentPlayer.MaxHP = data.playerHP;
        currentPlayer.MaxMP = data.playerMP;
        currentPlayer.Attack = data.playerAT;
        currentPlayer.EXP = data.LevelUpEXP;
        currentPlayer.LevelUpEXP = data.playerNextLevel;
    }

    public void PlayerStatusFillSaveData(PlayerSaveData data)
    {
        data.playerLevel = currentPlayer.Level;
        data.playerHP = currentPlayer.MaxHP;
        data.playerMP = currentPlayer.MaxMP;
        data.playerAT = currentPlayer.Attack;
        data.playerNextLevel = currentPlayer.LevelUpEXP;
        data.LevelUpEXP = currentPlayer.EXP;
    }

    public void PlayerStatusSaveLoad()
    {
        var playerData = SaveManager.LoadPlayer();
        PlayerStatusFillSaveData(playerData);
        SaveManager.SavePlayer(playerData);
    }

    public void EXPApplySaveData(EXPSaveData data)
    {
        currentPlayer.EXP = data.EXP;
    }

    public void EXPFillSaveData(EXPSaveData data)
    {
        data.EXP = currentPlayer.EXP;
    }

    public void EXPSaveLoad()
    {
        var expData = SaveManager.LoadEXP();
        EXPFillSaveData(expData);
        SaveManager.SaveEXP(expData);
    }
}