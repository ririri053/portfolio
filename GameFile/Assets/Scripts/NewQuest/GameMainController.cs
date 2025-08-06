using UnityEngine;


public class GameMainController : MonoBehaviour
{
    [SerializeField] private QuestSelectionUI questSelectionUI;
    [SerializeField] private CollectionManager collectionManager;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private PlayerCollection playerCollection;


    void Start()
    {
        playerManager.InitBaseStatus();

        if (!PlayerPrefs.HasKey("IsInitialized"))
        {
            // --- 初回起動：初期データ作成 ---
            Debug.Log("[Init] 初回起動のため初期化を行います");

            // Playerデータ
            playerManager.InitBaseStatus();
            var playerData = new PlayerSaveData
            {
                playerLevel = playerManager.currentPlayer.Level,
                playerAT = playerManager.currentPlayer.Attack,
                playerMP = playerManager.currentPlayer.MaxMP,
                playerNextLevel = playerManager.currentPlayer.LevelUpEXP,
                playerHP = playerManager.currentPlayer.MaxHP
            };
            SaveManager.SavePlayer(playerData);

            // EXP
            SaveManager.SaveEXP(new EXPSaveData { EXP = 0 });
            EXPSaveData loaded = SaveManager.LoadEXP();
            playerManager.EXPApplySaveData(loaded);
            SaveManager.SaveEXP(loaded);


            // Gachaポイント
            SaveManager.SaveGachaPoint(new GachaPointSaveData { gachaPoint = 0 });
            GachaPointSaveData loaded2 = SaveManager.LoadGachaPoint();
            gachaManager.ApplySaveData(loaded2);
            SaveManager.SaveGachaPoint(loaded2);

            // クエスト解放：最初の1つだけ true
            var questData = new QuestUnlockedSaveData
            {
                questUnlocked = new bool[3] { true, false, false }
            };
            SaveManager.SaveQuestUnlocked(questData);
            questSelectionUI.ApplySaveData(questData);

            // プレイヤー名
            SaveManager.SavePlayerName(new PlayerNameSaveData { playerName = "プレイヤー" });
            PlayerNameSaveData loaded4 = SaveManager.LoadPlayerName();
            menuUI.ApplySaveData(loaded4);
            SaveManager.SavePlayerName(loaded4);

            // 音量
            SaveManager.SaveSystem(new SystemSaveData { seVolume = 0.3f, bgmVolume = 0.1f });
            SystemSaveData loaded3 = SaveManager.LoadSystem();
            soundManager.ApplySaveData(loaded3);
            menuUI.AudioUpdate();  
            SaveManager.SaveSystem(loaded3);

            // --- コレクションデータ初期化 ---
            CollectionSaveData newCollectionData = new CollectionSaveData();
            foreach (var character in collectionManager.collectionList.characterDatas)
            {
                newCollectionData.ownedCharacters.Add(new CharacterSaveData
                {
                    characterId = character.characterId,
                    count = 0
                });
            }
            SaveManager.SaveCollection(newCollectionData);
            playerCollection.ApplySaveData(newCollectionData, collectionManager.collectionList);
            collectionManager.RefreshCollectionUI();

            // メニュー画面に反映
            menuUI.PlayerSettings();

            // フラグ立てて完了
            PlayerPrefs.SetInt("IsInitialized", 1);
            PlayerPrefs.Save();

            soundManager.PlayBGM(3);
        }
        else
        {
            // --- 通常起動：保存データ読み込みと反映 ---
            Debug.Log("[Init] 保存データ読み込み");

            // プレイヤーステータス
            var playerData = SaveManager.LoadPlayer();
            if (playerData != null)
            {
                playerManager.PlayerStatusApplySaveData(playerData);
            }
            

            // EXP
            var expData = SaveManager.LoadEXP();
            if (expData != null)
            {
                playerManager.EXPApplySaveData(expData);
            }
            

            // Gachaポイント
            var gachaData = SaveManager.LoadGachaPoint();
            if (gachaData != null)
            {
                gachaManager.ApplySaveData(gachaData);
            }
            

            // プレイヤー名
            var nameData = SaveManager.LoadPlayerName();
            if (nameData != null)
            {
                menuUI.ApplySaveData(nameData);
            }
            

            // 音量
            var soundData = SaveManager.LoadSystem();
            soundManager.ApplySaveData(soundData);
            menuUI.AudioUpdate();     

            // クエスト解放
            var questData = SaveManager.LoadQuestUnlocked();
            if (questData != null)
            {
                questSelectionUI.ApplySaveData(questData); // ←必要に応じてこのメソッドを実装
            }

            // コレクション
            var collectionData = SaveManager.LoadCollection();
            if (collectionData != null)
            {
                playerCollection.ApplySaveData(collectionData, collectionManager.collectionList);
                collectionManager.RefreshCollectionUI();
            }

            // メニュー画面に反映
            menuUI.PlayerSettings(); // ステータス表示など更新

            soundManager.PlayBGM(3);
        }
    }

    public void StartSetting()
    {
        playerManager.InitBaseStatus();

        if (!PlayerPrefs.HasKey("IsInitialized"))
        {
            // --- 初回起動：初期データ作成 ---
            Debug.Log("[Init] 初回起動のため初期化を行います");

            // Playerデータ
            playerManager.InitBaseStatus();
            var playerData = new PlayerSaveData
            {
                playerLevel = playerManager.currentPlayer.Level,
                playerAT = playerManager.currentPlayer.Attack,
                playerMP = playerManager.currentPlayer.MaxMP,
                playerNextLevel = playerManager.currentPlayer.LevelUpEXP,
                playerHP = playerManager.currentPlayer.MaxHP
            };
            SaveManager.SavePlayer(playerData);

            // EXP
            SaveManager.SaveEXP(new EXPSaveData { EXP = 0 });
            EXPSaveData loaded = SaveManager.LoadEXP();
            playerManager.EXPApplySaveData(loaded);
            SaveManager.SaveEXP(loaded);


            // Gachaポイント
            SaveManager.SaveGachaPoint(new GachaPointSaveData { gachaPoint = 0 });
            GachaPointSaveData loaded2 = SaveManager.LoadGachaPoint();
            gachaManager.ApplySaveData(loaded2);
            SaveManager.SaveGachaPoint(loaded2);

            // クエスト解放：最初の1つだけ true

            var questData = new QuestUnlockedSaveData
            {
                questUnlocked = new bool[3] { true, false, false }
            };
            SaveManager.SaveQuestUnlocked(questData);
            questSelectionUI.ApplySaveData(questData);

            // プレイヤー名
            SaveManager.SavePlayerName(new PlayerNameSaveData { playerName = "プレイヤー" });
            PlayerNameSaveData loaded4 = SaveManager.LoadPlayerName();
            menuUI.ApplySaveData(loaded4);
            SaveManager.SavePlayerName(loaded4);

            // 音量
            SaveManager.SaveSystem(new SystemSaveData { seVolume = 0.3f, bgmVolume = 0.1f });
            SystemSaveData loaded3 = SaveManager.LoadSystem();
            soundManager.ApplySaveData(loaded3);
            menuUI.AudioUpdate();  
            SaveManager.SaveSystem(loaded3);

            // --- コレクションデータ初期化 ---
            CollectionSaveData newCollectionData = new CollectionSaveData();
            foreach (var character in collectionManager.collectionList.characterDatas)
            {
                newCollectionData.ownedCharacters.Add(new CharacterSaveData
                {
                    characterId = character.characterId,
                    count = 0
                });
            }
            SaveManager.SaveCollection(newCollectionData);
            playerCollection.ApplySaveData(newCollectionData, collectionManager.collectionList);
            collectionManager.RefreshCollectionUI();

            // メニュー画面に反映
            menuUI.PlayerSettings();

            // フラグ立てて完了
            PlayerPrefs.SetInt("IsInitialized", 1);
            PlayerPrefs.Save();

            soundManager.PlayBGM(3);
        }
    }
}