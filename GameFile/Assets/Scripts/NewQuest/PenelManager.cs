using UI;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private QuestSelectionUI questSelectionUI;
    [SerializeField] private GachaUI gachaUI;
    [SerializeField] private QuestUI questUI;
    [SerializeField] private StartUI startUI;
    [SerializeField] private BattleUI battleUI;
    [SerializeField] private BattleCommandUI battleCommandUI;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private DialogUI dialogUI;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private ConfirmUI confirmUI;
    [SerializeField] private CollectionUI collectionUI;
    [SerializeField] private GachaResultUI gachaResultUI;
    [SerializeField] private CollectionResultUI collectionResultUI;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private BattleResultUI battleResultUI;
    [SerializeField] private LevelupUI levelupUI;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private FadeUI fadeUI;
    [SerializeField] private BattleEffectUI battleEffectUI;
    [SerializeField] private TitleUI titleUI;

    public void PlayGame()
    {
        titleUI.HideTitlePanel();
        menuUI.ShowMenuPanel();
        questSelectionUI.ShowQuestSelectPanel();
    }
    public void ShowQuestSelection()
    {
        // フェードアウトして非表示にする
        fadeUI.FadeOutAndHide();
        gachaUI.HideachaPanel();
        collectionUI.HideCollectionPanel();
        confirmUI.HideGachaConfirmPanel();
        confirmUI.HideQuestConfirmPanel();
        questSelectionUI.ShowQuestSelectPanel();
    }

    public void ShowQuestConfirmPanel()
    {
        confirmUI.ShowQuestConfirmPanel();
    }

    public void HideQuestConfirmPanel()
    {
        confirmUI.HideQuestConfirmPanel();
    }
 
    private int enemyID = 0;
    public void ShowEnemy(int enemyId)
    {
        enemyID = enemyId;
        SoundManager.instance.StopBGM();
        enemyManager.SetupQuest(enemyId);
        questManager.selectedQuestId = enemyId; // ◀ この行を追加
        menuUI.HideMenuPanel();
        questSelectionUI.HideQuestSelectPanel();
        confirmUI.HideQuestConfirmPanel();
        startUI.ShowEnemyVSPanel();
        battleManager.StartBattle();
    }
    public void ShowQuest()
    {
        SoundManager.instance.PlayBGM(enemyID);
        startUI.HideEnemyVSPanel();
        battleUI.ShowBattlePanel();
        playerUI.ShowPlayerPanel();
        enemyUI.ShowEnemyPanel();
        battleCommandUI.ShowBattleCommandPanel(); 
        battleEffectUI.ShowBattleEffectPanel();
        dialogUI.ShowDialogPanel();
    }

    public void ShowWinResult()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(2);
        battleUI.HideBattlePanel();
        playerUI.HidePlayerPanel();
        enemyUI.HideEnemyPanel();
        battleCommandUI.HideBattleCommandPanel(); 
        dialogUI.HideDialogPanel();
        battleEffectUI.HideBattleEffectPanel();
        battleResultUI.ResultWin(); // 勝利時の結果表示
    }
    public void ShowLoseResult()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(6);
        battleUI.HideBattlePanel();
        playerUI.HidePlayerPanel();
        enemyUI.HideEnemyPanel();
        battleCommandUI.HideBattleCommandPanel(); 
        dialogUI.HideDialogPanel();
        battleEffectUI.HideBattleEffectPanel();
        battleResultUI.ResultLose(); // 勝利時の結果表示
    }
    public void GoHome()
    {
        SoundManager.instance.PlayBGM(3);
        fadeUI.FadeOutAndHide();
        battleUI.HideBattlePanel();
        playerUI.HidePlayerPanel();
        enemyUI.HideEnemyPanel();
        battleCommandUI.HideBattleCommandPanel(); 
        dialogUI.HideDialogPanel();
        menuUI.ShowMenuPanel();
        questSelectionUI.ShowQuestSelectPanel();
    }

    public void ShowLevelupPanel()
    {
        SoundManager.instance.PlaySE(13); 
        battleResultUI.HideBattleResultPanel();
        levelupUI.ShowLevelupPanel();
        dialogUI.ShowDialogPanel();
    }
    public void HideLevelupPanel()
    {
        SoundManager.instance.PlayBGM(3);
        levelupUI.HideLevelupPanel();
        menuUI.ShowMenuPanel();
        questSelectionUI.ShowQuestSelectPanel();
    }

    public void HideQuest()
    {
        SoundManager.instance.PlayBGM(3);
        battleResultUI.HideBattleResultPanel();
        menuUI.ShowMenuPanel();
        questSelectionUI.ShowQuestSelectPanel();
    }

    public void ShowGacha()
    {
        fadeUI.FadeOutAndHide();
        gachaUI.ShowachaPanel();
        questSelectionUI.HideQuestSelectPanel();
        confirmUI.HideGachaConfirmPanel();
        confirmUI.HideQuestConfirmPanel();
        collectionUI.HideCollectionPanel();
    }

    public void ShowGachaConfirmPanel()
    {
        confirmUI.ShowGachaConfirmPanel();
    }

    public void HideGachaConfirmPanel()
    {
        confirmUI.HideGachaConfirmPanel();
    }

    public void ShowGachaResultPanel()
    {
        confirmUI.HideGachaConfirmPanel();
        gachaResultUI.ShowGachaResultPanel();
    }

    public void HideGachaResultPanel()
    {
        gachaResultUI.HideGachaResultPanel();
    }

    public void ShowCollection()
    {
        fadeUI.FadeOutAndHide();
        gachaUI.HideachaPanel();
        questSelectionUI.HideQuestSelectPanel();
        confirmUI.HideGachaConfirmPanel();
        confirmUI.HideQuestConfirmPanel();
        collectionUI.ShowCollectionPanel();
    }

    public void ShowCollectionResultPanel()
    {
        collectionResultUI.ShowCollectionResultPanel();
    }

    public void HideCollectionResultPanel()
    {
        collectionResultUI.HideCollectionResultPanel();
    }
}
