using UnityEngine;
using Battle;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private DialogTextManager dialogTextManager;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] public BattleCommandUI battleCommandUI;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private BattleEffectManager battleEffectManager;
    [SerializeField] private SkillDatabase skillDatabase;
    [SerializeField] private QuestSelectionUI questSelectionUI; // ◀ 追加
    [SerializeField] private SkillEffectManager skillEffectManager;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private EnemyUI enemyUI;

    private EffectManager effectManager;
    private void Awake()
    {
        effectManager = FindFirstObjectByType<EffectManager>();
    }

    private HashSet<SkillType> usedOncePerBattleSkills = new HashSet<SkillType>();

    public bool HasUsedOncePerBattleSkill(SkillType skillType)
    {
        return usedOncePerBattleSkills.Contains(skillType);
    }

    public void MarkOncePerBattleSkillUsed(SkillType skillType)
    {
        usedOncePerBattleSkills.Add(skillType);
    }

    public void StartBattle()
    {
        playerManager.skillEffectManager.ClearEffects();
        enemyManager.skillEffectManager.ClearEffects();
        playerManager.Initialize();
        usedOncePerBattleSkills.Clear();
        battleCommandUI.EnablePlayerButtons(false);
        playerManager.PrepareBattle();
        EnemyStartDialog();
        Debug.Log("Battle Started");
    }

    private int selectedSkillIndex = -1; // 選択中のスキルを保持

    public void OnClick_ShowSkillConfirm(int skillIndex)
    {
        selectedSkillIndex = skillIndex;
        SkillType selectedSkill = (SkillType)skillIndex;
        SkillData skillData = skillDatabase.Get(selectedSkill);

        battleCommandUI.ShowSkillPanel(skillData);// 確認画面を表示
    }

    public void OnClick_UseSkillConfirmed()
    {
        if (selectedSkillIndex >= 0)
        {
            OnClick_UseSkill(selectedSkillIndex); // 既存の発動処理を呼ぶ
            battleCommandUI.UseSkillHidePanel();   // 確認画面を閉じる
            selectedSkillIndex = -1;
        }
    }

    public void OnClick_CancelSkillConfirm()
    {
        battleCommandUI.HideSkillPanel(); // 確認画面を閉じる
        selectedSkillIndex = -1;
    }
    public void OnClick_UseSkill(int skillIndex)
    { 
        SkillType selectedSkill = (SkillType)skillIndex;
        UsePlayerCommand(selectedSkill);
        battleCommandUI.EnablePlayerButtons(false);
    }
    public void UsePlayerCommand(SkillType skillType)
    {
        skillManager.UsePlayerSkill(skillType, () => {
            ProcessTurnEnd();
        });
        battleCommandUI.EnablePlayerButtons(false);
    }

    public void BackToMainMenu()
    {
        SoundManager.instance.StopBGM();
        panelManager.GoHome();
        battleEffectManager.ClearAllEffectIcons();
    }

    public void EnemyStartDialog()
    {
        string[] EnemyStartMessages = new string[]
        {
            $"{enemyManager.questManager.questData.enemyData.Name}" + $"が現れた！",
            $"{enemyManager.questManager.questData.enemyData.EnemyStartText}"
        };

        dialogTextManager.SetScenarios(EnemyStartMessages, () =>
        {
            StartPlayerTurn();
        });
    }

    private void EndWinText()
    {
        dialogTextManager.ShowMessage(enemyManager.questManager.questData.enemyData.EnemyEndWinText, () => {
            EndWin();
        }, false);
    }

    private void EndLoseText()
    {
        dialogTextManager.ShowMessage(enemyManager.questManager.questData.enemyData.EnemyEndLoseText, () => {
            EndLose();
        }, false);
    }

    public void EnemyTalkText()
    {
        dialogTextManager.ShowMessage(enemyManager.questManager.questData.enemyData.EnemyTalkText, () => {
            battleCommandUI.EnablePlayerButtons(true);
            dialogTextManager.ClearMessage();
        }, false);
    }
    private void StartPlayerTurn()
    {
        dialogTextManager.ShowMessage($"{menuUI.nameInputField.text}" + $"のターン！", () => {
            battleCommandUI.EnablePlayerButtons(true); // ダイアログが終わったらボタン有効化
        }, false);
    }

    private void StartEnemyTurn()
    {
        dialogTextManager.ShowMessage($"{enemyManager.questManager.questData.enemyData.Name}" + $"のターン！", () => {
            SkillType enemySkill = enemyAI.DecideAction();
            skillManager.UseEnemySkill(enemySkill, () => {// ←ここで実際に敵が行動
                ProcessTurnEnd2();
        });
        }, false);
    }

    // ターン終了処理を共通化
    private void ProcessTurnEnd()
    {
        var playerEffectResult = playerManager.skillEffectManager.ProcessEffects();
        var enemyEffectResult = enemyManager.skillEffectManager.ProcessEffects();

        // 毒アイコン削除判定
        if (enemyEffectResult.RemovedTypes.Contains(SkillType.PoisonAttack))
        {
            battleEffectManager.ClearPoisonEffectIcons();
        }

        // ダメージ処理
        int poisonDamageToPlayer = playerEffectResult.TotalDamage;
        int poisonDamageToEnemy = enemyEffectResult.TotalDamage;

        // --- 毒エフェクト表示 ---
        SkillData poisonSkillData = skillDatabase.Get(SkillType.PoisonAttack);
        if (poisonSkillData != null && poisonSkillData.EffectPrefab != null)
        {
            if (poisonDamageToPlayer > 0 && effectManager != null && playerManager.HP > 0)
            {
                effectManager.ShowEffect(poisonSkillData.EffectPrefab, playerManager.transform.position, 1.0f);
            }
            if (poisonDamageToEnemy > 0 && effectManager != null && enemyManager.HP > 0)
            {
                effectManager.ShowEffect(poisonSkillData.EffectPrefab, enemyManager.transform.position, 1.0f);
            }
        }

        if (poisonDamageToPlayer > 0)
        {
            SoundManager.instance.PlaySE(9);
            playerManager.HP -= poisonDamageToPlayer;
            Debug.Log($"[HP減算] 毒ダメージ {poisonDamageToPlayer} を受けた。現在HP: {playerManager.HP}");
        }
        if (poisonDamageToEnemy > 0)
        {
            SoundManager.instance.PlaySE(9);
            enemyManager.TakeDamage(poisonDamageToEnemy);
            Debug.Log($"[敵HP減算] 毒ダメージ {poisonDamageToEnemy} を受けた。現在HP: {enemyManager.HP}");
        }

        UpdateAllUI();

        // ★ここでバトル終了判定を先に行う
        if (CheckBattleEnd()) return;

        // ↓ここから先のメッセージ・演出はHPが0でなければ実行
        if (poisonDamageToPlayer > 0 && playerManager.HP > 0)
        {
            dialogTextManager.ShowMessage($"毒ダメージを受けた！({poisonDamageToPlayer})", () => {
                if (poisonDamageToEnemy > 0 && enemyManager.HP > 0)
                {
                    dialogTextManager.ShowMessage($"敵が毒ダメージを受けた！({poisonDamageToEnemy})", () => {
                        battleCommandUI.EnablePlayerButtons(false);
                        StartEnemyTurn();
                    }, true);
                }
                else
                {
                    battleCommandUI.EnablePlayerButtons(false);
                    StartEnemyTurn();
                }
            }, true);
        }
        else if (poisonDamageToEnemy > 0 && enemyManager.HP > 0)
        {
            dialogTextManager.ShowMessage($"敵が毒ダメージを受けた！({poisonDamageToEnemy})", () => {
                battleCommandUI.EnablePlayerButtons(false);
                StartEnemyTurn();
            }, true);
        }
        else
        {
            battleCommandUI.EnablePlayerButtons(false);
            StartEnemyTurn();
        }
    }
    private void ProcessTurnEnd2()
    {
        UpdateAllUI(); // 追加：HP/MPバーなどのUIを更新
        if (CheckBattleEnd()) return; // 追加：バトル終了判定
        battleCommandUI.EnablePlayerButtons(false);
        StartPlayerTurn();
    }

        private string FormatWithTransparentZeros(int value)
    {
        return value.ToString().PadLeft(3, '_').Replace("_", "<color=#00000000>0</color>");
    }

    private void UpdateAllUI()
    {
        playerManager.playerUI.UpdateHP(playerManager.HP);
        playerManager.playerUI.UpdateMP(playerManager.MP);
        enemyManager.enemyUI.UpdateHPBar(enemyManager.HP, enemyManager.MaxHP);
        enemyManager.enemyUI.enemyHPText.text = $"HP : {FormatWithTransparentZeros(enemyManager.HP)}";
    }
    
    // 追加：バトル終了判定
    private bool CheckBattleEnd()
{
    if (playerManager.HP <= 0)
    {
        dialogTextManager.ShowMessage($"{menuUI.nameInputField.text}" + $"の敗北…", () => {
            EndLoseText();
        }, false);
        return true;
    }
    else if (enemyManager.HP <= 0)
    {
        dialogTextManager.ShowMessage($"{enemyManager.questManager.questData.enemyData.Name}" + $"を倒した！", () => {
            EndWinText();
            enemyUI.DeadEnemy();
        }, false);
        return true;
    }
    return false;
}
    private void EndWin()
    {
        Debug.Log("EndWinメソッドが呼ばれました。"); // ★追加：メソッドが呼ばれているか確認

        // 報酬付与
        GetResult();

        // 現在のクエストIDを取得
        int clearedQuestId = enemyManager.questManager.selectedQuestId;
        Debug.Log("クリアしたクエストID: " + clearedQuestId); // ★追加：クエストIDの値を確認

        if (questSelectionUI == null)
        {
            Debug.LogError("BattleManagerにQuestSelectionUIが設定されていません！"); // ★追加：参照がnullでないか確認
            return;
        }

        // セーブデータを取得
        var loadedSaveData = SaveManager.LoadQuestUnlocked();
        if (loadedSaveData == null) return;
        
        // クエストクリア　→次のIDのクエストをアンロック
        if (clearedQuestId == 0)
            loadedSaveData.questUnlocked[1] = true;
        else if (clearedQuestId == 1)
            loadedSaveData.questUnlocked[2] = true;

        // --- セーブ処理 ---
        SaveManager.SaveQuestUnlocked(loadedSaveData);

        // ボタンの状態を更新
        questSelectionUI.QuestButtonUIUpdate(loadedSaveData);

        battleCommandUI.EnablePlayerButtons(false);
        dialogTextManager.ShowMessage("バトル終了！", () => {
            panelManager.ShowWinResult();
            enemyUI.SetEnemy();
            battleEffectManager.ClearAllEffectIcons();
        }, false);    // 報酬付与やシーン遷移処理など
    }

    private void EndLose()
    {
        battleCommandUI.EnablePlayerButtons(false);
        dialogTextManager.ShowMessage("バトルに敗北した…", () => {
            panelManager.ShowLoseResult();
            battleEffectManager.ClearAllEffectIcons();
        }, false);
    }

    private void GetResult()
    {
        enemyManager.GainExp();
        enemyManager.GainGachaPoint();
    }
}


